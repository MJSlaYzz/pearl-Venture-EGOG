using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesSpawner : MonoBehaviour
{
    #region Variables

    public VillageCollectables villageCollectables;

    //[SerializeField] private int questRequiredAmount;
    //[SerializeField] private string questRequiredItemName;
    [HideInInspector] private GameObject[] requiredItem;
    [SerializeField] private GameObject[] availableSpawnPoints;
    [HideInInspector] private GameObject[] spawnPoints;
    [HideInInspector] private GameObject[] itemsPrefabs;
    [HideInInspector] private List<GameObject> availableSpawnPointsEdited;

    [HideInInspector] private int index;

    [SerializeField] private int[] questRequiredAmount;
    [SerializeField] private GameObject[] questRequiredItemNames;
    [HideInInspector] private int totalQuestRequiredAmount;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        // Set the amount of object to be stored in the collections:
        requiredItem = new GameObject[10];
        spawnPoints = new GameObject[10];
        itemsPrefabs = new GameObject[10];
        if (availableSpawnPointsEdited == null) // to avoid null ref error I get when use [HideInInspector] for availableSpawnPointsEdited.
        {
            List<GameObject> availableSpawnPointsEdited = new List<GameObject>();
            availableSpawnPointsEdited.Capacity = spawnPoints.Length;
            this.availableSpawnPointsEdited = availableSpawnPointsEdited;
        }


        DeclareNameAndAmount();
        SetEditedPoints();
        Spawner();

    }

    private void DeclareNameAndAmount()
    {
        for (int i = 0; i < villageCollectables.details.Length; i++)
        {
            for (int x = 0; x < questRequiredItemNames.Length; x++)
            {
                if (villageCollectables.details[i].collectableName == questRequiredItemNames[x].name)
                {
                    itemsPrefabs[i] = villageCollectables.details[x].collectablePrefab;
                }
            }
        }
    }
    private void SetEditedPoints()
    {
        foreach (GameObject gameObject in availableSpawnPoints)
        {
            availableSpawnPointsEdited.Add(gameObject);
        }
    }
    private void CalculateTotalAmount()
    {
        totalQuestRequiredAmount = 0;
        foreach (int num in questRequiredAmount)
        {
            totalQuestRequiredAmount += num;
        }
        print("total quest Required Amount = " + totalQuestRequiredAmount);
    }
    private void PickSpawnPoint()
    {
        if(availableSpawnPointsEdited.Count > 0)
        {
            index = Random.Range(0, availableSpawnPointsEdited.Count - 1);
            //Debug.Log("index = " + index);
        }
        else
        {
            print("No more Avilable Spawn Points!");
        }

    }
    private void PickPrefab()
    {
        int currentItemIndex = 0;

        for (int i = 0; i < totalQuestRequiredAmount; i++)
        {
            for (int x = 0; x < questRequiredAmount.Length; x++)
            {
                for (int z = 0; z < questRequiredAmount[x]; z++)
                {
                    if (itemsPrefabs[x].name == questRequiredItemNames[x].name)
                    {
                        requiredItem[currentItemIndex] = itemsPrefabs[x];
                        if (currentItemIndex < totalQuestRequiredAmount - 1)
                        {
                            currentItemIndex++;
                        }
                    }
                }
            }
        }
    }

    private void RemoveEditedPoint()
    {
        availableSpawnPointsEdited.Remove(availableSpawnPointsEdited[index]);
        //Debug.Log("index = " + index + " got removed!");
    }

    private void Spawner()
    {
        CalculateTotalAmount();
        for (int y = 0; y < totalQuestRequiredAmount; y++)
        {
            PickSpawnPoint();
            if (spawnPoints[y] == null)
            {
                spawnPoints[y] = availableSpawnPointsEdited[index];
            }
            spawnPoints[y].transform.position = availableSpawnPointsEdited[index].transform.position;
            PickPrefab();
            requiredItem[y].transform.position = spawnPoints[y].transform.position;
            Instantiate(requiredItem[y]);
            RemoveEditedPoint();
        }
        
    }


    /* Old Codes
    private void StartCode()
    {
        questRequiredItemName = "red shroom";
        for (int i = 0; i < villageCollectables.details.Length; i++)
        {
            if (villageCollectables.details[i].collectableName == questRequiredItemName)
            {
                itemsPrefabs[i] = villageCollectables.details[i].collectablePrefab;
            }
        }
    }
    private void Spawner2()
    {
        for (int x = 0; x < questRequiredAmount; x++)
        {
            for (int y = 0; y < itemsPrefabs.Length; y++)
            {
                if (itemsPrefabs[y].name == questRequiredItemName)
                {
                    for (int z = 0; z < questRequiredAmount; z++)
                    {
                        requiredItem[z] = itemsPrefabs[y];
                    }
                }
            }
            PickSpawnPoint();
            if (spawnPoints[x] == null)
            {
                spawnPoints[x] = availableSpawnPointsEdited[index];
            }
            spawnPoints[x].transform.position = availableSpawnPointsEdited[index].transform.position;
            requiredItem[x].transform.position = spawnPoints[x].transform.position;
            Instantiate(requiredItem[x]);
            RemoveEditedPoint();
        }
    }
    */
}
