using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Profiling;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadingSlider;
    public Text loadingText;
    [SerializeField] Cop28DataManager cop28DataManager;

    private void Awake()
    {
        //Debug.Log("build index is" + SceneManager.GetActiveScene().buildIndex); // is 2
        //Debug.Log("scene count is" + SceneManager.sceneCountInBuildSettings); // is 3
        cop28DataManager = FindObjectOfType<Cop28DataManager>();
        Time.timeScale = 1f;
        if (cop28DataManager != null && File.Exists(Application.persistentDataPath + "/pearl.path"))
        {
            cop28DataManager.LoadData();

        }
        else
        {
            Debug.Log("ther's no saved file");
        }
    }
    private void Update()
    {
        /*
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (Time.timeScale < 1f)
            {
                Time.timeScale = 1.0f;
            }
        }
        */
    }
    public void LoadScene(int sceneIndex)
    {
        //Time.timeScale = 1.0f;
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }
    public void ReloadCurrentScene()
    {
        //Time.timeScale = 1.0f;
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex));
    }
    public void LoadNextScene()
    {
        if(SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            //Time.timeScale = 1.0f;
            StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex + 1));
        }
        else
        {
            //Time.timeScale = 1.0f;
            StartCoroutine(LoadAsynchronously(0));

        }
      
    }
    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            Time.timeScale = 0f;
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = operation.progress;
            //loadingText.text = operation.progress * 100f + "%";
            yield return null; // makes it refresh every frame.
        }
        /*
        if (operation.isDone)
        {
            Time.timeScale = 1f;
        }
        */
    }
    #region COP28

    public void LoadLevelAgainCop28()
    {
        if(cop28DataManager != null)
        {
            cop28DataManager.SaveData();
            ReloadCurrentScene();
        }

    }
    public void StartCleanSave()
    {
        Cop28SaveSystem.DeleteProgress();
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex + 1));
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex));
        }

    }

    #endregion
}
