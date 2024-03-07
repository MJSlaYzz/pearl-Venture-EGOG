using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ShowFPS : MonoBehaviour
{
    [HideInInspector] GameObject fpsNumber;
    [HideInInspector] GameObject fps;
    [HideInInspector] public Text fpsText;
    [HideInInspector] public float deltaTime;

    [HideInInspector] public int targetFrameRate = 120;

    [HideInInspector] private bool iteamsAreHidden = false;
    [SerializeField] private Image[] uiIteamsToHide;
    [SerializeField] private Text[] uiTextToHide;

    [SerializeField] private Image o2Bar;
    [SerializeField] private Image dashBar;

    [SerializeField] private Image progressBar1;
    [SerializeField] private Image progressBar2;
    [SerializeField] private Image progressBar3;
    [SerializeField] private GameObject progressPointsText;

    [HideInInspector] private bool listOneDone = false;
    [HideInInspector] private bool listTowDone = false;
    [HideInInspector] private bool startHiding = false;
    [HideInInspector] private bool startShowing = false;

    private void Start()
    {
        GetFPSVariables();
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate; // limit the FPS to be 120 Max.
    }
    void Update()
    {
        FPSUpdate();
        FPSToggle();
        FPSLimiter();
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            HideInGameUI();
        }

    }
    void FPSUpdate()
    {
        if(Time.timeScale != 0) // the number bugs when the time stops.
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            fpsText.text = Mathf.Ceil(fps).ToString();
        }

    }
    void FPSToggle()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            if(fps != null) 
            {
                if (fps.activeInHierarchy)
                {
                    fps.SetActive(false);
                }
                else if (!fps.activeInHierarchy)
                {
                    fps.SetActive(true);
                }
            }
        }
    }
    void GetFPSVariables()
    {
        fpsNumber = GameObject.Find("FPS Number");
        fps = GameObject.Find("FPS");
        if (fpsNumber != null)
        {
            //Debug.Log("fpsNumber found");
            fpsText = fpsNumber.GetComponent<Text>();
        }
    }
    void FPSLimiter()
    {
        if(Application.targetFrameRate != targetFrameRate) 
        {
            Application.targetFrameRate = targetFrameRate;
        }
    }
    void HideInGameUI()
    {
        //print("iteamsAreHidden = " + iteamsAreHidden);
        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (!iteamsAreHidden)
            {
                startHiding = true;
                startShowing = false;
            }
            else if(iteamsAreHidden)
            {
                startHiding = false;
                startShowing = true;
            }
            //print("F2 pressed");
        }
        if (startHiding)
        {
            Cursor.visible = false;
            o2Bar.enabled = false;
            dashBar.enabled = false;
            if(progressBar1 != null && progressBar2 != null && progressBar3 != null && progressPointsText != null)
            {
                progressBar1.enabled = false;
                progressBar2.enabled = false;
                progressBar3.enabled = false;
                progressPointsText.GetComponent<TextMeshProUGUI>().enabled = false;
            }

            foreach (Image itemToHide in uiIteamsToHide)
            {
                //itemToHide.enabled = false;
                var color = itemToHide.GetComponent<Image>().color;
                color.a = 0f;
                itemToHide.GetComponent<Image>().color = color;

                //print(itemToHide);
                if (itemToHide == uiIteamsToHide[10])
                {
                    listOneDone = true;
                    //print("list 1 done");
                }
            }
            foreach (Text textToHide in uiTextToHide)
            {
                var text = textToHide.GetComponent<Text>().color;
                text.a = 0f;
                textToHide.GetComponent<Text>().color = text;


                if (textToHide == uiTextToHide[3])
                {
                    listTowDone = true;
                    //print("list 2 done");
                }
            }
            if (listOneDone && listTowDone)
            {
                iteamsAreHidden = true;
                listOneDone = false;
                listTowDone = false;
            }
        }
        else if (startShowing)
        {
            Cursor.visible = true;
            o2Bar.enabled = true;
            dashBar.enabled = true;
            if (progressBar1 != null && progressBar2 != null && progressBar3 != null && progressPointsText != null)
            {
                progressBar1.enabled = true;
                progressBar2.enabled = true;
                progressBar3.enabled = true;
                progressPointsText.GetComponent<TextMeshProUGUI>().enabled = true;
            }

            foreach (Image itemToHide in uiIteamsToHide)
            {
                //print(itemToHide.name);
                if (itemToHide.name == "Minimap shape")
                {
                    var color = itemToHide.GetComponent<Image>().color;
                    color.a = 0.1f;
                    itemToHide.GetComponent<Image>().color = color;
                }
                else if (itemToHide.name == "Minimap border")
                {
                    var color = itemToHide.GetComponent<Image>().color;
                    color.a = 0.6f;
                    itemToHide.GetComponent<Image>().color = color;
                }
                else
                {
                    var color = itemToHide.GetComponent<Image>().color;
                    color.a = 1f;
                    itemToHide.GetComponent<Image>().color = color;
                }


                if (itemToHide == uiIteamsToHide[10])
                {
                    listOneDone = true;
                }
            }
            foreach (Text textToHide in uiTextToHide)
            {
                var text = textToHide.GetComponent<Text>().color;
                text.a = 1f;
                textToHide.GetComponent<Text>().color = text;


                if (textToHide == uiTextToHide[3])
                {
                    listTowDone = true;
                }
            }
            if (listOneDone && listTowDone)
            {
                iteamsAreHidden = false;
                listOneDone = false;
                listTowDone = false;
            }
        }

    }
}