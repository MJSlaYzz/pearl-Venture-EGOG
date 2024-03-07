using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cop28EnvironmentControler : MonoBehaviour
{
    #region Variables

    [HideInInspector] private Cop28DataManager cop28DataManager;
    [SerializeField] private GameObject murkyWater;

    [Header("SeaGrass Variables")]
    [SerializeField] private List<GameObject> allSeaGrass = new List<GameObject>();
    [SerializeField] private List<GameObject> currentSeaGrass = new List<GameObject>(10);
    [HideInInspector] private int seaGrassAmount = 0;

    [Header("Plants Variables")]
    [SerializeField] private List<GameObject> allPlants = new List<GameObject>();
    [SerializeField] private List<GameObject> currentLevelPlants = new List<GameObject>(10);
    [HideInInspector] private int plantsAmount = 0;

    [SerializeField] private GameObject stage01Plants;
    [SerializeField] private GameObject stage2Plants;
    [SerializeField] private GameObject stage3Plants;

    [SerializeField] private GameObject stage01Vines;
    [SerializeField] private GameObject stage23Vines;


    [Header("Fish Variables")]
    [SerializeField] private List<GameObject> allFish = new List<GameObject>();
    [SerializeField] private List<GameObject> currentLevelFish = new List<GameObject>(10);
    [HideInInspector] private int fishAmount = 0;

    [Header("Crabs Variables")]
    [SerializeField] private List<GameObject> allCrabs = new List<GameObject>();
    [SerializeField] private List<GameObject> currentLevelCrabs = new List<GameObject>(10);
    [HideInInspector] private int crabsAmount = 0;

    [Header("Turtles Variables")]
    [SerializeField] private List<GameObject> allTurtles = new List<GameObject>();
    [SerializeField] private List<GameObject> currentLevelTurtles = new List<GameObject>(10);
    [HideInInspector] private int turtlesAmount = 0;

    [Header("Sharks Variables")]
    [SerializeField] private GameObject[] sharks;
    [SerializeField] private SharkAI[] sharkAIScript;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        cop28DataManager = FindObjectOfType<Cop28DataManager>();
        ChoooseCurrentLevelSeaGrass();
        ChoooseCurrentLevelPlants();
        ChoooseCurrentLevelFish();
        ActivateSharksAllTheTime();
        ChoooseCurrentLevelTurtles();
        ChoooseCurrentLevelCrabs();
    }
    #region Sea Grass
    public void ChoooseCurrentLevelSeaGrass()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3) //Game Level
        {
            ChooseSeaGrassSpawnAmount();
            List<GameObject> temporarySeaGrass = allSeaGrass.ConvertAll(x => x);
            //if (seaGrassAmount > temporarySeaGrass.Count - 1)
            //{
            //    seaGrassAmount = temporarySeaGrass.Count - 1;
            //}
            if (seaGrassAmount > 0)
            {
                for (int i = 0; i < seaGrassAmount; i++)
                {
                    int randomIndex = Random.Range(0, temporarySeaGrass.Count - 1);
                    currentSeaGrass[i] = temporarySeaGrass[randomIndex];
                    temporarySeaGrass.RemoveAt(randomIndex);
                }
            }
            for (int i = 0; i < temporarySeaGrass.Count; i++)
            {
                temporarySeaGrass[i].SetActive(false);
            }
        }
    }
    private void ChooseSeaGrassSpawnAmount()
    {
        if (cop28DataManager.currentLevelProgress == 0)
        {
            seaGrassAmount = 0;
        }
        else if (cop28DataManager.currentLevelProgress == 1)
        {
            seaGrassAmount = 14;
        }
        else if (cop28DataManager.currentLevelProgress == 2)
        {
            seaGrassAmount = 28;
        }
        else if (cop28DataManager.currentLevelProgress == 3)
        {
            seaGrassAmount = 44;
        }
    }
    #endregion
    #region Plants
    public void ChoooseCurrentLevelPlants()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3) //Game Level
        {
            ChoosePlantsSpawnAmount();
            ChooseWhichPlantsGroupsToShow();
            // Create a deep copy of `allTrash` using ConvertAll()
            List<GameObject> temporaryPlants = allPlants.ConvertAll(x => x);
            //if (plantsAmount > temporaryPlants.Count - 1) // to avoid errors when there's not enough trash in the scene to choose from
            //{
            //    plantsAmount = temporaryPlants.Count - 1;
            //}
            if (plantsAmount > 0) // if it's 0 that mean we are at stage 3 and there's no need to spawn more trash
            {
                for (int i = 0; i < plantsAmount; i++)
                {
                    int randomIndex = Random.Range(0, temporaryPlants.Count - 1);
                    currentLevelPlants[i] = temporaryPlants[randomIndex];
                    temporaryPlants.RemoveAt(randomIndex);
                }
            }
            for (int i = 0; i < temporaryPlants.Count; i++)
            {
                temporaryPlants[i].SetActive(false);
            }
        }
    }
    private void ChoosePlantsSpawnAmount()
    {
        if (cop28DataManager.currentLevelProgress == 0)
        {
            plantsAmount = 11;
        }
        else if (cop28DataManager.currentLevelProgress == 1)
        {
            plantsAmount = 22;
        }
        else if (cop28DataManager.currentLevelProgress == 2)
        {
            plantsAmount = 33;
        }
        else if (cop28DataManager.currentLevelProgress == 3)
        {
            plantsAmount = 45;
        }
    }
    private void ChooseWhichPlantsGroupsToShow()
    {
        if (cop28DataManager.currentLevelProgress == 0)
        {
            murkyWater.SetActive(true);

            stage01Plants.SetActive(true);
            stage2Plants.SetActive(false);
            stage3Plants.SetActive(false);

            stage01Vines.SetActive(true);
            stage23Vines.SetActive(false);
        }
        else if (cop28DataManager.currentLevelProgress == 1)
        {
            murkyWater.SetActive(true);

            stage01Plants.SetActive(true);
            stage2Plants.SetActive(false);
            stage3Plants.SetActive(false);

            stage01Vines.SetActive(true);
            stage23Vines.SetActive(false);
        }
        else if (cop28DataManager.currentLevelProgress == 2)
        {
            murkyWater.SetActive(false);

            stage01Plants.SetActive(false);
            stage2Plants.SetActive(true);
            stage3Plants.SetActive(false);

            stage01Vines.SetActive(false);
            stage23Vines.SetActive(true);
        }
        else if (cop28DataManager.currentLevelProgress == 3)
        {
            murkyWater.SetActive(false);

            stage01Plants.SetActive(false);
            stage2Plants.SetActive(false);
            stage3Plants.SetActive(true);

            stage01Vines.SetActive(false);
            stage23Vines.SetActive(true);
        }
    }
    #endregion
    #region Fish
    public void ChoooseCurrentLevelFish()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3) //Game Level
        {
            ChooseFishSpawnAmount();
            // Create a deep copy of `allTrash` using ConvertAll()
            List<GameObject> temporaryFish = allFish.ConvertAll(x => x);
            //if (fishAmount > temporaryFish.Count - 1) // to avoid errors when there's not enough trash in the scene to choose from
            //{
            //    fishAmount = temporaryFish.Count - 1;
            //}
            if (fishAmount > 0)
            {
                for (int i = 0; i < fishAmount; i++)
                {
                    int randomIndex = Random.Range(0, temporaryFish.Count - 1);
                    currentLevelFish[i] = temporaryFish[randomIndex];
                    temporaryFish.RemoveAt(randomIndex);
                }
            }
            for (int i = 0; i < temporaryFish.Count; i++)
            {
                temporaryFish[i].SetActive(false);
            }
        }
    }
    private void ChooseFishSpawnAmount()
    {
        if (cop28DataManager.currentLevelProgress == 0)
        {
            fishAmount = 0;
        }
        else if (cop28DataManager.currentLevelProgress == 1)
        {
            fishAmount = 4;
        }
        else if (cop28DataManager.currentLevelProgress == 2)
        {
            fishAmount = 8;
        }
        else if (cop28DataManager.currentLevelProgress == 3)
        {
            fishAmount = 13;
        }
    }
    #endregion
    #region Crabs
    public void ChoooseCurrentLevelCrabs()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3) //Game Level
        {
            ChooseCrabsSpawnAmount();
            // Create a deep copy of `allTrash` using ConvertAll()
            List<GameObject> temporaryCrabs = allCrabs.ConvertAll(x => x);
            //if (crabsAmount > temporaryCrabs.Count - 1) // to avoid errors when there's not enough trash in the scene to choose from
            //{
            //    crabsAmount = temporaryCrabs.Count - 1;
            //}
            if (crabsAmount > 0)
            {
                for (int i = 0; i < crabsAmount; i++)
                {
                    int randomIndex = Random.Range(0, temporaryCrabs.Count - 1);
                    currentLevelCrabs[i] = temporaryCrabs[randomIndex];
                    temporaryCrabs.RemoveAt(randomIndex);
                }
            }
            for (int i = 0; i < temporaryCrabs.Count; i++)
            {
                temporaryCrabs[i].SetActive(false);
            }
        }
    }
    private void ChooseCrabsSpawnAmount()
    {
        if (cop28DataManager.currentLevelProgress == 0)
        {
            crabsAmount = 0;
        }
        else if (cop28DataManager.currentLevelProgress == 1)
        {
            crabsAmount = 1;
        }
        else if (cop28DataManager.currentLevelProgress == 2)
        {
            crabsAmount = 2;
        }
        else if (cop28DataManager.currentLevelProgress == 3)
        {
            crabsAmount = 4;
        }
    }
    #endregion
    #region Turtles
    public void ChoooseCurrentLevelTurtles()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3) //Game Level
        {
            ChooseTurtlesSpawnAmount();
            // Create a deep copy of `allTrash` using ConvertAll()
            List<GameObject> temporaryTurtles = allTurtles.ConvertAll(x => x);
            //if (turtlesAmount > temporaryTurtles.Count - 1) // to avoid errors when there's not enough trash in the scene to choose from
            //{
            //    turtlesAmount = temporaryTurtles.Count - 1;
            //}
            if (turtlesAmount > 0)
            {
                for (int i = 0; i < turtlesAmount; i++)
                {
                    int randomIndex = Random.Range(0, temporaryTurtles.Count - 1);
                    currentLevelTurtles[i] = temporaryTurtles[randomIndex];
                    temporaryTurtles.RemoveAt(randomIndex);
                }
            }
            for (int i = 0; i < temporaryTurtles.Count; i++)
            {
                temporaryTurtles[i].SetActive(false);
            }
        }
    }
    private void ChooseTurtlesSpawnAmount()
    {
        if (cop28DataManager.currentLevelProgress == 0)
        {
            turtlesAmount = 0;
        }
        else if (cop28DataManager.currentLevelProgress == 1)
        {
            turtlesAmount = 1;
        }
        else if (cop28DataManager.currentLevelProgress == 2)
        {
            turtlesAmount = 2;
        }
        else if (cop28DataManager.currentLevelProgress == 3)
        {
            turtlesAmount = 4;
        }
    }
    #endregion
    #region Sharks
    private void ActivateSharksAllTheTime()
    {
        if (cop28DataManager.currentLevelProgress == 3 && SceneManager.GetActiveScene().buildIndex == 3) //Game Level)
            {
            if (sharks != null && sharkAIScript != null)
            {
                for (int c = 0; c < sharkAIScript.Length; c++)
                {
                    sharkAIScript[c].sharkShouldHide = false;
                }
                for (int i = 0; i < sharks.Length; i++)
                {
                    sharks[i].SetActive(true);
                }
            }
        }
    }
    #endregion
}
