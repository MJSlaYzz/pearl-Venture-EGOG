using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking.Types;

public class PauseMenu : MonoBehaviour
{
    private bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject settingsMenu;
    public GameObject loadingMenu;
    public GameObject gameOverScreen;
    public GameObject inGameUI;
    public GameObject tutorialScreen;
    public GameObject gameWinScreen;
    public GameObject youDiedScreen;

    //cop28
    public GameObject eventWinScreen;
    public GameObject hardModetWinScreen;

    public GameObject wholeMap;
    public GameObject miniMap;
    private bool miniMapIsoff = true;

    public GameObject noDeathMode;
    public bool noDeathModeButtonIsOn = false;

    private bool minimapIsRight = true;
    [SerializeField] RectTransform minimapRightPos;
    [SerializeField] RectTransform minimapLeftPos;

    [SerializeField] private PlayerMovementFour playerMovement;

    [HideInInspector] public bool adventure = false;
    [HideInInspector] public bool nowWeRide = false;
    [HideInInspector] public bool nightOfMystery = false;

    [SerializeField] GameObject adventureSong;
    [SerializeField] GameObject nowWeRideSong;
    [SerializeField] GameObject nightOfMysterySong;

    [HideInInspector] private bool timeIsStopped = false; 
    [HideInInspector] private bool timeIsSlowed = false;



    void Update()
    {
        SwitchMiniMapLocation();
        PressButtonToShowMap();
        PressButtonToPauseOrResume();
        TurnDeathModeOnAndOff();
        TurnMiniMapOffInTutorial();
        SongNameDisplay();
        StopAndSlowInGameTime();


    }
    public void Resume()
    {
        if(playerMovement != null)
        {
            playerMovement.enabled = true;
        }
        pauseMenuUI.SetActive(false);
        inGameUI.SetActive(true);
        Time.timeScale = 1f; //Unfreeze game
        gameIsPaused = false;
    }
    void Pause()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        pauseMenuUI.SetActive(true);
        inGameUI.SetActive(false);
        Time.timeScale = 0f; //freeze game
        gameIsPaused = true;
    }
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    void PressButtonToPauseOrResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gameOverScreen.activeInHierarchy && !gameWinScreen.activeInHierarchy && !youDiedScreen.activeInHierarchy && !tutorialScreen.activeInHierarchy)
            {
                if (playerMovement != null && playerMovement.isCop28 && eventWinScreen != null && hardModetWinScreen != null)
                {
                    if (!eventWinScreen.activeInHierarchy && !hardModetWinScreen.activeInHierarchy)
                    {
                        if (gameIsPaused)
                        {
                            if (settingsMenu.activeInHierarchy == true && loadingMenu.activeInHierarchy == false)
                            {
                                settingsMenu.SetActive(false);
                                pauseMenuUI.SetActive(true);
                            }
                            else if (settingsMenu.activeInHierarchy == false && loadingMenu.activeInHierarchy == false)
                            {
                                Resume();
                            }
                        }
                        else
                        {
                            Pause();
                        }
                    }
                }
                else
                {
                    if (gameIsPaused)
                    {
                        if (settingsMenu.activeInHierarchy == true && loadingMenu.activeInHierarchy == false)
                        {
                            settingsMenu.SetActive(false);
                            pauseMenuUI.SetActive(true);
                        }
                        else if (settingsMenu.activeInHierarchy == false && loadingMenu.activeInHierarchy == false)
                        {
                            Resume();
                        }
                    }
                    else
                    {
                        Pause();
                    }
                }
            }
            
        }
    }
    void PressButtonToShowMap()
    {
        if(wholeMap != null)
        {
            if (Input.GetKey(KeyCode.M))
            {
                if (!gameOverScreen.activeInHierarchy && !gameWinScreen.activeInHierarchy && !youDiedScreen.activeInHierarchy && !tutorialScreen.activeInHierarchy)
                {
                    if (!gameIsPaused)
                    {
                        if (playerMovement != null && playerMovement.isCop28 && eventWinScreen != null && hardModetWinScreen != null) //for cop28
                        {
                            if (!eventWinScreen.activeInHierarchy && !hardModetWinScreen.activeInHierarchy)
                            {
                                Time.timeScale = 0f;
                                wholeMap.SetActive(true);
                            }
                        }
                        else
                        {
                            Time.timeScale = 0f;
                            wholeMap.SetActive(true);
                        }

                        /*
                        if (!WholeMap.activeInHierarchy)
                        {
                            WholeMap.SetActive(true);
                        }
                        else if (WholeMap.activeInHierarchy)
                        {
                            WholeMap.SetActive(false);
                        }
                        */
                    }
                }
                
            }
            else
            {
                if (!gameOverScreen.activeInHierarchy && !gameWinScreen.activeInHierarchy && !youDiedScreen.activeInHierarchy && !tutorialScreen.activeInHierarchy)
                {
                    if (!gameIsPaused && !timeIsSlowed && !timeIsStopped)
                    {
                        if (playerMovement != null && playerMovement.isCop28 && eventWinScreen != null && hardModetWinScreen != null) //for cop28
                        {
                            if (!eventWinScreen.activeInHierarchy && !hardModetWinScreen.activeInHierarchy)
                            {
                                Time.timeScale = 1f;
                                wholeMap.SetActive(false);
                            }
                        }
                        else
                        {
                            Time.timeScale = 1f;
                            wholeMap.SetActive(false);
                        }

                    }
                }
                
            }

        }
    }
    void TurnDeathModeOnAndOff()
    {
        if (gameIsPaused)
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if(noDeathModeButtonIsOn == false)
                {
                    noDeathMode.gameObject.SetActive(true);
                    noDeathModeButtonIsOn = true;
                }
                else if(noDeathModeButtonIsOn == true)
                {
                    noDeathMode.gameObject.SetActive(false);
                    noDeathModeButtonIsOn = false;
                }
            }
        }
    }
    void TurnMiniMapOffInTutorial()
    {
        if(miniMap != null)
        {
            if (tutorialScreen.activeInHierarchy)
            {
                miniMap.SetActive(false);
                miniMapIsoff = true;
            }
            else if (!tutorialScreen.activeInHierarchy && miniMapIsoff)
            {
                miniMap.SetActive(true);
                miniMapIsoff = false; // To prevent the code from turning the minimap on all the time.
            }
        }
    }
    void SwitchMiniMapLocation()
    {
        if(miniMap != null)
        {
            RectTransform rectTransform = miniMap.GetComponent<RectTransform>();
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (minimapIsRight && miniMap.activeInHierarchy)
                {
                    rectTransform.position = minimapLeftPos.position;
                    minimapIsRight = false;
                }
                else if (!minimapIsRight && miniMap.activeInHierarchy)
                {
                    rectTransform.position = minimapRightPos.position;
                    minimapIsRight = true;
                }
            }
        }
       
    }
    void SongNameDisplay()
    {
        if (pauseMenuUI.activeInHierarchy)
        {
            if (adventure)
            {
                adventureSong.gameObject.SetActive(true);
                nowWeRideSong.gameObject.SetActive(false);
                nightOfMysterySong.gameObject.SetActive(false);
            }
            else if (nowWeRide)
            {
                adventureSong.gameObject.SetActive(false);
                nowWeRideSong.gameObject.SetActive(true);
                nightOfMysterySong.gameObject.SetActive(false);
            }
            else if (nightOfMystery)
            {
                adventureSong.gameObject.SetActive(false);
                nowWeRideSong.gameObject.SetActive(false);
                nightOfMysterySong.gameObject.SetActive(true);
            }
        }
    }
    void StopAndSlowInGameTime()
    {
        if (!gameIsPaused)
        {
            if (Input.GetKeyDown(KeyCode.F3))
            {
                if (!timeIsStopped)
                {
                    timeIsStopped = true;
                    //gameIsPaused = true;
                    Time.timeScale = 0f;
                }
                else
                {
                    timeIsStopped = false;
                    //gameIsPaused = false;
                    Time.timeScale = 1.0f;
                }
            }
            if (Input.GetKeyDown(KeyCode.F4))
            {
                if (Time.timeScale != 0.5f)
                {
                    timeIsSlowed = true;
                    Time.timeScale = 0.5f;
                }
                else if (Time.timeScale == 0.5f)
                {
                    timeIsSlowed = false;
                    Time.timeScale = 1.0f;
                }
            }
        }
    }
}
