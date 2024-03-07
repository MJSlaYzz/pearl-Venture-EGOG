using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectablesCounter : MonoBehaviour
{
    [SerializeField] private Text pearlsText;
    [SerializeField] private GameObject[] allPearls;
    [SerializeField] private GameObject gameWinScreen;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private int totalPearls;

    public int totalPearlsForShare { get; set; }
    private void Start()
    {
        Time.timeScale = 1; //fix the freeze bug after pressing play again.
    }
    // Update is called once per frame
    void Update()
    {
        pearlsText.text = totalPearls + "/" + allPearls.Length;

        if(totalPearls == allPearls.Length)
        {
            totalPearlsForShare = totalPearls; //I added the += insted of = so i'm not sure about that. [save system]
            GameWon();
        }
    }
    public void AddAPearl()
    {
        totalPearls += 1;
        //Debug.Log("Total Pearls are " + totalPearls);
    }
    public void GameWon()
    {
        gameWinScreen.SetActive(true);
        inGameUI.SetActive(false);
        Time.timeScale = 0;
    }
}
