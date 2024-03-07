using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonSelectHandler : MonoBehaviour
{
    [HideInInspector] EventSystem eventSystem;
    [HideInInspector] private bool mainMenuActive = false;
    [HideInInspector] private bool resetSelectedButton = true;
    

    #region Main Menu Variables
    [SerializeField] private MainMenuCam menuCam;

    [SerializeField] private GameObject mMLoadingScreen;  // main menu Loading Screen.
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject story;
    [SerializeField] private GameObject mMSettingsMenu;
    [SerializeField] private GameObject levelSelect;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject patches;

    [SerializeField] private GameObject mMStartButton;   //main menu start button.
    [SerializeField] private GameObject stortyMMButton;  // story main menu button.
    //[SerializeField] private GameObject settingsBButton; // settings back button.
    [SerializeField] private GameObject lSTButton;       // level select tutorial button.
    //[SerializeField] private GameObject creditsBButton;  // credits back button.
    //[SerializeField] private GameObject patchesBButton;  // patches back button.

    #endregion

    #region Levels Variavles

    [SerializeField] private GameObject lLoadingScreen;  // levels Loading Screen.

    [SerializeField] private GameObject pauseMenuUI;
    //[SerializeField] private GameObject lSettingsMenu;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject tutorialScreen;
    [SerializeField] private GameObject gameWinScreen;
    [SerializeField] private GameObject youDiedScreen;

    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject gOPlayAgainButton;  // gameover Playe Again Button.
    [SerializeField] private GameObject gWNextLevelButton;  // game won next level button.
    [SerializeField] private GameObject yDContinueButton;   // YouDied continue button.
    [SerializeField] private GameObject tutorialSkipButton;



    #endregion

    // Start is called before the first frame update
    void Start()
    {
        eventSystem = EventSystem.current;
        resetSelectedButton = true;
        SceneChecker();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(resetSelectedButton);
        MainmenuHandler();
        LevelsHandler();
    }
    void MainmenuHandler()
    {
        if (mainMenuActive && !mMLoadingScreen.activeInHierarchy)
        {
            if (mainMenu.activeInHierarchy && resetSelectedButton)
            {
                eventSystem.SetSelectedGameObject(mMStartButton.gameObject);
                resetSelectedButton = false;
            }
            else if (story.activeInHierarchy)
            {
                if (resetSelectedButton)
                {
                    eventSystem.SetSelectedGameObject(stortyMMButton.gameObject);
                    resetSelectedButton = false;
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    story.gameObject.SetActive(false);
                    mainMenu.gameObject.SetActive(true);
                    ResetSlectedButton();
                }
            }
            else if (mMSettingsMenu.activeInHierarchy)
            {
                if (resetSelectedButton)
                {
                    //eventSystem.SetSelectedGameObject(settingsBButton.gameObject);
                    resetSelectedButton = false;
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    mMSettingsMenu.gameObject.SetActive(false);
                    menuCam.EnableBool(1);
                    mainMenu.gameObject.SetActive(true);
                    ResetSlectedButton();
                }
            }
            else if (levelSelect.activeInHierarchy)
            {
                if (resetSelectedButton)
                {
                    eventSystem.SetSelectedGameObject(lSTButton.gameObject);
                    resetSelectedButton = false;
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    levelSelect.gameObject.SetActive(false);
                    menuCam.EnableBool(1);
                    mainMenu.gameObject.SetActive(true);
                    ResetSlectedButton();
                }
            }
            else if (credits.activeInHierarchy)
            {
                if (resetSelectedButton)
                {
                    //eventSystem.SetSelectedGameObject(creditsBButton.gameObject);
                    resetSelectedButton = false;
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    credits.gameObject.SetActive(false);
                    mainMenu.gameObject.SetActive(true);
                    ResetSlectedButton();
                }
            }
            else if (patches.activeInHierarchy)
            {
                if (resetSelectedButton)
                {
                    //eventSystem.SetSelectedGameObject(patchesBButton.gameObject);
                    resetSelectedButton = false;
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    patches.gameObject.SetActive(false);
                    mainMenu.gameObject.SetActive(true);
                    ResetSlectedButton();
                }
            }
        }
    }
    void LevelsHandler()
    {
        if (!mainMenuActive && !lLoadingScreen.activeInHierarchy)
        {
            if (pauseMenuUI.activeInHierarchy)
            {
                if (resetSelectedButton)
                {
                    eventSystem.SetSelectedGameObject(resumeButton.gameObject);
                    resetSelectedButton = false;
                } 
            }
            else if (gameOverScreen.activeInHierarchy)
            {
                if (resetSelectedButton)
                {
                    eventSystem.SetSelectedGameObject(gOPlayAgainButton.gameObject);
                    resetSelectedButton = false;
                }
            }
            else if (gameWinScreen.activeInHierarchy)
            {
                if (resetSelectedButton)
                {
                    eventSystem.SetSelectedGameObject(gWNextLevelButton.gameObject);
                    resetSelectedButton = false;
                }
            }
            else if (youDiedScreen.activeInHierarchy)
            {
                if (resetSelectedButton)
                {
                    eventSystem.SetSelectedGameObject(yDContinueButton.gameObject);
                    resetSelectedButton = false;
                }
            }
            if (tutorialScreen != null && tutorialScreen.activeInHierarchy)
            {
                if (resetSelectedButton)
                {
                    eventSystem.SetSelectedGameObject(tutorialSkipButton.gameObject);
                    resetSelectedButton = false;
                }
            }
            if(tutorialScreen != null)
            {
                if (!pauseMenuUI.activeInHierarchy && !gameOverScreen.activeInHierarchy && !gameWinScreen.activeInHierarchy && !youDiedScreen.activeInHierarchy && !tutorialScreen.activeInHierarchy)
                {
                    ResetSlectedButton();
                    //Cursor.visible = false;
                }
                else
                {
                    Cursor.visible = true;
                }
            }
            else
            {
                if (!pauseMenuUI.activeInHierarchy && !gameOverScreen.activeInHierarchy && !gameWinScreen.activeInHierarchy && !youDiedScreen.activeInHierarchy)
                {
                    ResetSlectedButton();
                    //Cursor.visible = false;
                }
                else
                {
                    Cursor.visible = true;
                }
            }
        }
    }
    void SceneChecker()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) // main menu
        {
            mainMenuActive = true;
        }
        else // levels
        {
            mainMenuActive = false;
        }
    }
    public void ResetSlectedButton() // must be attached to every button leads to main menu or out of main menu.
    {
        resetSelectedButton = true;
    }

}
