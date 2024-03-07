using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerPoints : MonoBehaviour
{
    [Header("Tutorial settings")]
    [SerializeField] private BoxCollider2D[] triggerPoints;
    [SerializeField] private GameObject[] tutorialImages;
    [HideInInspector] private Cop28TutorialText tutorialText;
    [SerializeField] private bool[] gotTriggered;
    [HideInInspector] private ShootingCop28 shootingCop28;
    [HideInInspector] public bool shootinUnlocked = false;
    // Start is called before the first frame update
    void Start()
    {
        tutorialText = FindObjectOfType<Cop28TutorialText>();
        shootingCop28 = FindObjectOfType<ShootingCop28>();
        shootingCop28.shootingIsAllowed = false;
    }

    // Update is called once per frame
    void Update()
    {
        ShowImageBasedOnDialogueLine();
    }
    void ShowImageBasedOnDialogueLine()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision) //should be on player
    {
        if (collision.name == "O2 Replenisher Trigger Point" && !gotTriggered[0])
        {
            gotTriggered[0] = true;
            tutorialText.TakeOrderOfLines(7,7);
        }
        else if (collision.name == "Pearls Trigger Point" && !gotTriggered[1])
        {
            gotTriggered[1] = true;
            tutorialText.TakeOrderOfLines(8,8);
        }
        else if (collision.name == "Crossbow Trigger Point" && !gotTriggered[7])
        {
            shootinUnlocked = true;
            gotTriggered[7] = true;
            tutorialText.TakeOrderOfLines(9, 9);
        }
        else if (collision.name == "Moving Obstacles Trigger Point" && !gotTriggered[3])
        {
            gotTriggered[3] = true;
            tutorialText.TakeOrderOfLines(10, 11);
        }
        else if (collision.name == "JellyFish Trigger Point" && !gotTriggered[4])
        {
            gotTriggered[4] = true;
            tutorialText.TakeOrderOfLines(12, 12);
        }
        else if (collision.name == "CheckPoint Trigger Point" && !gotTriggered[6])
        {
            gotTriggered[6] = true;
            tutorialText.TakeOrderOfLines(13, 13);
        }
        else if (collision.name == "Creatures Release Trigger Point" && !gotTriggered[5])
        {
            gotTriggered[5] = true;
            tutorialText.TakeOrderOfLines(14, 14);
        }

        else if (collision.name == "Ghost Net Trigger Point" && !gotTriggered[2])
        {
            gotTriggered[2] = true;
            tutorialText.TakeOrderOfLines(15,16);
        }
        else if (collision.name == "Shark Trigger Point" && !gotTriggered[8])
        {
            gotTriggered[8] = true;
            tutorialText.TakeOrderOfLines(17,19);
        }
    }
}
