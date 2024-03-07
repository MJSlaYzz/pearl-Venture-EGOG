using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public TextMeshProUGUI questName;
    public TextMeshProUGUI questDescription;
    public GameObject questUI;

    public static QuestManager questManager;
    public QuestBase currentQuest { set; get; }
    public QuestDialogueTrigger currentQuestDialogueTrigger { set; get; }
    public Button questAcceptButton;
    public bool inQuestUI;


    public void SetQuestUIInfo(QuestBase quest) //Called at the end of a dialogue, sets info of quest in UI.
    {
        currentQuest = quest;
        questUI.SetActive(true);
        inQuestUI = true;

        questName.text = quest.questName;
        questDescription.text = quest.questDescription;
        questAcceptButton.Select();
    }
    public void StartQuestBuffer()
    {
        StartCoroutine(QuestBuffer());
    }

    private IEnumerator QuestBuffer()
    {
        yield return new WaitForSeconds(0.1f);
        inQuestUI = false;
        
    }

    public void Awake() 
    {
        if (questManager == null)
            questManager = this; //Sets up a singleton instance.       
    }

}
