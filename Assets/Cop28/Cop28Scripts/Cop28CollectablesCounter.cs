using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Cop28CollectablesCounter : MonoBehaviour
{
    #region Variables
    [HideInInspector] private Cop28DataManager cop28DataManager;

    [Header("Pearls Variables")]
    [HideInInspector] private bool normalMode = true;
    [SerializeField] private List<GameObject> allPearls = new List<GameObject>();
    [SerializeField] private List<GameObject> currentLevelPearls = new List<GameObject>(10);
    [SerializeField] private List<GameObject> hardLevelPearls = new List<GameObject>(23);
    [SerializeField] private int totalPearls;

    [Header("Trash Variables")]
    [SerializeField] private List<GameObject> allTrash = new List<GameObject>();
    [SerializeField] private List<GameObject> currentLevelTrash = new List<GameObject>(10);
    [HideInInspector] private int trashAmount = 0;

    [Header("GhostNet Variables")]
    [SerializeField] private List<GameObject> allNets = new List<GameObject>();
    [SerializeField] private List<GameObject> currentLevelNets = new List<GameObject>(10);
    [HideInInspector] private int netsAmount = 0;

    [Header("Crabs & Turtles Variables")]
    [SerializeField] private GameObject crabTrap;
    [SerializeField] private GameObject trappedTurtle;

    [Header("UI Variables")]
    [SerializeField] private Text pearlsText;
    [SerializeField] private GameObject gameWinScreen;
    [SerializeField] private GameObject eventWinScreen;
    [SerializeField] private GameObject hardGameWinScreen;
    [SerializeField] private GameObject inGameUI;

    #endregion
    public int totalPearlsForShare { get; set; }
    private void Start()
    {
        cop28DataManager = FindObjectOfType<Cop28DataManager>();
        Time.timeScale = 1; //fix the freeze bug after pressing play again.
        ChoooseCurrentLevelPearls();
        ChoooseCurrentLevelTrash();
        ChoooseCurrentLevelGhostNets();
        StartCoroutine(ActivateCrabsAndTurtles());
        if (cop28DataManager.maxedPointsReached)
        {
            normalMode = false;
            print("normal mode is false");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (normalMode)
        {
            pearlsText.text = totalPearls + "/" + currentLevelPearls.Count;
            if (totalPearls == currentLevelPearls.Count)
            {
                totalPearlsForShare = totalPearls;
                GameWon();
            }
        }
        else
        {
            pearlsText.text = totalPearls + "/" + hardLevelPearls.Count;
            if (totalPearls == hardLevelPearls.Count)
            {
                totalPearlsForShare = totalPearls;
                HardGameWon();
            }
        }
    }
    public void GameWon()
    {
        Time.timeScale = 0;
        if (cop28DataManager.progressPoints < 300)
        {
            gameWinScreen.SetActive(true);//game win screen makes you repeat the level.
        }
        else
        {
            eventWinScreen.SetActive(true); //event win screen takes you to the hard level.
        }
        inGameUI.SetActive(false);

    }
    public void HardGameWon()
    {
        Time.timeScale = 0;
        hardGameWinScreen.SetActive(true);//hard game win screen makes you repeat the hard level.
        inGameUI.SetActive(false);
    }
    #region Pearls
    public void AddAPearl()
    {
        totalPearls += 1;
        //Debug.Log("Total Pearls are " + totalPearls);
    }

    public void ChoooseCurrentLevelPearls()
    {
        // Create a deep copy of `allPearls` using ConvertAll() to avoid gameobjects being removed from allPearls list after the copy
        // because the old way(temporaryPearls = allPearls;) only copies references, not creating a separate list with independent objects.
        List<GameObject> temporaryPearls = allPearls.ConvertAll(x => x);
        if(!cop28DataManager.maxedPointsReached)
        {
            for (int i = 0; i < 10; i++)
            {
                int randomIndex = Random.Range(0, temporaryPearls.Count - 1);
                currentLevelPearls[i] = temporaryPearls[randomIndex];
                temporaryPearls.RemoveAt(randomIndex);
            }
        }
        else
        {
            for (int i = 0; i < 23; i++)
            {
                int randomIndex = Random.Range(0, temporaryPearls.Count - 1);
                hardLevelPearls[i] = temporaryPearls[randomIndex];
                temporaryPearls.RemoveAt(randomIndex);
            }
        }
        for (int i = 0; i < temporaryPearls.Count; i++)
        {
            temporaryPearls[i].SetActive(false);
        }
    }
    #endregion
    #region Trash
    public void ChoooseCurrentLevelTrash()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3) //Game Level
        {
            ChooseTrashSpawnAmount();
            // Create a deep copy of `allTrash` using ConvertAll()
            List<GameObject> temporaryTrash = allTrash.ConvertAll(x => x);
            if(trashAmount > temporaryTrash.Count -1) // to avoid errors when there's not enough trash in the scene to choose from
            {
                trashAmount = temporaryTrash.Count - 1;
            }
            if (trashAmount > 0) // if it's 0 that mean we are at stage 3 and there's no need to spawn more trash
            {
                for (int i = 0; i < trashAmount; i++)
                {
                    int randomIndex = Random.Range(0, temporaryTrash.Count -1);
                    currentLevelTrash[i] = temporaryTrash[randomIndex];
                    temporaryTrash.RemoveAt(randomIndex);
                }
            }
            for (int i = 0; i < temporaryTrash.Count; i++)
            {
                temporaryTrash[i].SetActive(false);
            }
        }
    }
    private void ChooseTrashSpawnAmount()
    {
        if (cop28DataManager.currentLevelProgress == 0)
        {
            trashAmount = 8;
        }
        else if (cop28DataManager.currentLevelProgress == 1)
        {
            trashAmount = 6;
        }
        else if (cop28DataManager.currentLevelProgress == 2)
        {
            trashAmount = 5;
        }
        else if (cop28DataManager.currentLevelProgress == 3)
        {
            trashAmount = 0;
        }
    }
    #endregion
    #region GhostNets
    public void ChoooseCurrentLevelGhostNets()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3) //Game Level
        {
            ChooseGhostNetsSpawnAmount();
            // Create a deep copy of `allTrash` using ConvertAll()
            List<GameObject> temporaryNets = allNets.ConvertAll(x => x);
            if (netsAmount > temporaryNets.Count - 1) // to avoid errors when there's not enough nets in the scene to choose from
            {
                netsAmount = temporaryNets.Count - 1;
            }
            if (netsAmount > 0) // if it's 0 that mean we are at stage 3 and there's no need to spawn more nets
            {
                for (int i = 0; i < netsAmount; i++)
                {
                    int randomIndex = Random.Range(0, temporaryNets.Count - 1);
                    currentLevelNets[i] = temporaryNets[randomIndex];
                    temporaryNets.RemoveAt(randomIndex);
                }
            }
            for (int i = 0; i < temporaryNets.Count; i++)
            {
                temporaryNets[i].SetActive(false);
            }
        }
    }

    private void ChooseGhostNetsSpawnAmount()
    {
        if (cop28DataManager.currentLevelProgress == 0)
        {
            netsAmount = 5;
        }
        else if (cop28DataManager.currentLevelProgress == 1)
        {
            netsAmount = 4;
        }
        else if (cop28DataManager.currentLevelProgress == 2)
        {
            netsAmount = 2;
        }
        else if (cop28DataManager.currentLevelProgress == 3)
        {
            netsAmount = 0;
        }

    }
    #endregion
    #region Crabs & Turtles
    private IEnumerator ActivateCrabsAndTurtles()
    {
        if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            yield return new WaitForSecondsRealtime(0.2f); //to give a chance for the true value of the progress level to load.
            print("PrgressLevel = " + cop28DataManager.progressLevel);
            if (cop28DataManager.progressLevel == 2)
            {
                crabTrap.SetActive(true);
                trappedTurtle.SetActive(true);
            }
            else
            {
                crabTrap.SetActive(false);
                trappedTurtle.SetActive(false);
            }
        }
    }
    #endregion
}
