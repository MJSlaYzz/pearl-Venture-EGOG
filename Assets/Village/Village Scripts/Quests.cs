using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quests : MonoBehaviour
{
    [SerializeField] private GameObject[] QuestList;
    public GameObject AvQuests;
    public StoreScript store;
    public int acceptedQuests;
    [SerializeField] private Text questsCounter;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        questsCounter.text = acceptedQuests.ToString();
    }

    public void AddQuest()
    {
        acceptedQuests += 1;

    }
    public void ShowQuests()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (AvQuests.activeInHierarchy)
            {
                AvQuests.SetActive(false);
            }
            else
            {
                Debug.Log("Quest Open");
                AvQuests.SetActive(true);
            }
        }

    }

}