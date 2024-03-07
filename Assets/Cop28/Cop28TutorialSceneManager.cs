using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cop28TutorialSceneManager : MonoBehaviour
{
    #region Variables
    [Header("Camera settings")]
    [SerializeField] private GameObject sceneCamera;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] public float cameraMovementSpeed = 5f;
    [SerializeField] public bool reachedTarget = false;

    [Header("NPCs settings")]
    [SerializeField] public GameObject rasheed;
    [SerializeField] public GameObject ammar;
    [SerializeField] public GameObject oldMan;
    [SerializeField] public GameObject rasheedDestination;

    [Header("Dialogue settings")]
    [HideInInspector] public Cop28DialogueManager dialogueManager;
    [SerializeField] public Text dialogueText;
    [SerializeField] public int stage = 1;

    [Header("EndScreen settings")]
    [SerializeField] public Image endScreen;
    [SerializeField] private float endScreenSpeed = 80f;
    [SerializeField] private float startAlpha = 0f;
    [SerializeField] private float endAlpha = 1f;
    [HideInInspector] private SceneLoader sceneloader;


    #endregion
    // Start is called before the first frame update
    void Start()
    {
        sceneCamera.transform.position = startPoint.position;
        dialogueManager = FindObjectOfType<Cop28DialogueManager>();
        sceneloader = FindObjectOfType<SceneLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckCurrentDialouge();
        CameraStages();
    }
    void CameraStages()
    {
        if(stage == 1)
        {
            dialogueManager.canDequeue = false;
            // move from sea to rasheed and follow him.
            CameraMoveToTarget(rasheed.transform);
            if (reachedTarget)
            {
                dialogueManager.canDequeue = true;
                sceneCamera.transform.position = rasheed.transform.position;
            }
        }
        else if (stage == 2)
        {
            dialogueManager.canDequeue = false;
            // move from rasheed to Ammar.
            CameraMoveToTarget(ammar.transform);
            if (reachedTarget)
            {
                dialogueManager.canDequeue = true;
                sceneCamera.transform.position = ammar.transform.position;
            }
        }
        else if (stage == 3)
        {
            dialogueManager.canDequeue = false;
            // move from Ammar back to Rasheed.
            // move Old man start moving.
            CameraMoveToTarget(rasheed.transform);
            if (reachedTarget)
            {
                dialogueManager.canDequeue = true;
                sceneCamera.transform.position = rasheed.transform.position;
                rasheed.GetComponentInParent<NPCMovement>().enabled = true;
                oldMan.GetComponent<NPCMovement>().enabled = true;
                stage = 4;
            }
        }
        else if (stage == 4)
        {
            dialogueManager.canDequeue = false;
            // make sure rasheed reached ammar
            sceneCamera.transform.position = rasheed.transform.position;
            float distance = Vector3.Distance(rasheed.transform.parent.transform.position, rasheedDestination.transform.position);
            if (distance <= 0.5)
            {
                dialogueManager.canDequeue = true;
            }
        }
        else if (stage == 5)
        {
            // move from Rasheed to the sea.
            // end the Level and move to tutorial.
            cameraMovementSpeed = 1f;
            CameraMoveToTarget(endPoint.transform);
            ammar.GetComponentInParent<NPCMovement>().enabled = true;
            //print(Vector3.Distance(sceneCamera.transform.position, endPoint.transform.position));
            if (Vector3.Distance(sceneCamera.transform.position, endPoint.transform.position) <= 13)
            {
                EndScreen();
            }
        }
    }
    void CheckCurrentDialouge()
    {
        //print("dialogueInfo.Count = " + dialogueManager.dialogueInfo.Count);
        //if (dialogueManager.dialogueInfo.Count == 30)
        //{
        //    print("dialogueInfo.Count == 30");
        //    stage = "1";
        //}
        if (dialogueText.text == "\"The old fisherman stood on the shore of Fujairah, staring at a wide expanse of sea, its surface sparkling under the morning sun.\"")
        {
            stage = 1;
        }
        if (dialogueText.text == "\"While he was standing there deeply absorbed in thought, he noticed a young man approaching the fishermen and reprimanding them firmly for what their actions had led to. The young man was asking them to help correct some of this damage, but no one paid any attention to him.\"")
        {
            stage = 2;
        }
        else if (dialogueText.text == "\"The old fisherman recognized the passion and firmness in the young man's eyes.\"")
        {
            stage = 3;
        }
        else if (dialogueText.text == "\"After much thought and hesitation, he approached the young man, his heart beating with a mixture of guilt and hope.\"")
        {
            stage = 4;
        }
        else if (dialogueManager.inDialogue == false)
        {
            stage = 5;
        }
    }
    void CameraMoveToTarget(Transform target)
    {
        sceneCamera.transform.position = Vector3.MoveTowards(sceneCamera.transform.position, target.position, cameraMovementSpeed * Time.deltaTime);
        if (sceneCamera.transform.position == target.position)
        {
            reachedTarget = true;
        }
        else
        {
            reachedTarget = false;
        }
    }
    void EndScreen()
    {
        var color = endScreen.GetComponent<Image>().color;
        if (color.a < endAlpha)
        {
            color.a += 0.001f * endScreenSpeed * Time.deltaTime;
        }
        else
        {
            //end scene and move to next one.
            print("LOAD NEXT SCENE!!");
            //sceneloader.LoadNextScene();
        }
        endScreen.GetComponent<Image>().color = color;
    }
}
