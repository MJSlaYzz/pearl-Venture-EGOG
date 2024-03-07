using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    #region Music Variables
    [SerializeField] public AudioSource musicAudioSource;
    [SerializeField] public AudioSource backgroundAudioSource;
    [SerializeField] public AudioClip[] songs;

    [SerializeField] public AudioClip adventure;
    [SerializeField] public AudioClip nowWeRide;
    [SerializeField] public AudioClip nightOfMystery;
    [SerializeField] private AudioClip mainMenuSongClip;
    [SerializeField] private AudioClip cop28StorySong1;
    [SerializeField] private AudioClip cop28StorySong2;
    [SerializeField] private AudioClip cop28StorySong3;

    [HideInInspector] public bool menuArrayIsOn = false;
    [HideInInspector] private bool villageArrayIsOn = false;
    [HideInInspector] private bool gameArrayIsOn = false;
    [SerializeField] public float volume;
    [SerializeField] private int songsPlayed;
    [SerializeField] private bool[] beenPlayed;
    [HideInInspector] PauseMenu pauseMenu;
    [HideInInspector] Dialogue dialogue;
    #endregion
    void Start()
    {
        SwitchBtweenGameAndMenuSongs();
        //volume = 1f;
        beenPlayed = new bool[songs.Length];
    }

    void Update()
    {
        SwitchBtweenGameAndMenuSongs();
        SongNameDisplay();
        LowerVolumeInStory();
        //myAudioSource.volume = volume;

        if (!musicAudioSource.isPlaying || Input.GetKeyDown(KeyCode.N))
        {
            PlaySong(Random.Range(0, songs.Length));
        }
        if (songsPlayed == songs.Length)
        {
            //Debug.Log("we start a new shuffle");
            for (int i = 0; i < songs.Length; i++)
            {
                if (i == songs.Length)
                {
                    break;
                }
                else
                {
                    beenPlayed[i] = false;
                }
            }
            songsPlayed = 0;
        }
    }
    void PlaySong(int songPicked)
    {
        if (!beenPlayed[songPicked])
        {
            songsPlayed++;
            beenPlayed[songPicked] = true;
            musicAudioSource.clip = songs[songPicked];
            musicAudioSource.Play();
            //Debug.Log($"Playing Song Number: {songPicked + 1}");
        }
        else
        {
            musicAudioSource.Stop();
        }
    }
    void SwitchBtweenGameAndMenuSongs()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1 && SceneManager.GetActiveScene().name == "Cop28 Dialogue Scene")
        {
            if (!villageArrayIsOn)
            {
                print("called");
                for (int i = 0; i < songs.Length; i++)
                {
                    songs[i] = null;
                }
                songs[0] = cop28StorySong3;
                //songs[0] = cop28StorySong1;
                //songs[1] = cop28StorySong2;
                gameArrayIsOn = false;
                villageArrayIsOn = true;
                menuArrayIsOn = false;
                musicAudioSource.Stop();
            }
        }
        else
        {
            if (SceneManager.GetActiveScene().buildIndex == 0 && !menuArrayIsOn)
            {
                for (int i = 0; i < songs.Length; i++)
                {
                    songs[i] = null;
                }
                songs[0] = mainMenuSongClip;
                gameArrayIsOn = false;
                menuArrayIsOn = true;
                villageArrayIsOn = false;
                musicAudioSource.Stop();
            }
            else if (SceneManager.GetActiveScene().buildIndex > 0 && !gameArrayIsOn)
            {
                for (int i = 0; i < songs.Length; i++)
                {
                    songs[i] = null;
                }
                songs[0] = adventure;
                songs[1] = nowWeRide;
                songs[2] = nightOfMystery;
                gameArrayIsOn = true;
                menuArrayIsOn = false;
                villageArrayIsOn = false;
                musicAudioSource.Stop();
            }
        }


    }
    void SongNameDisplay()
    {
        if (!menuArrayIsOn)
        {
            pauseMenu = FindObjectOfType<PauseMenu>();
            if (pauseMenu != null)
            {
                //Debug.Log("Called");
                if (musicAudioSource.clip == adventure)
                {
                    pauseMenu.adventure = true;
                    pauseMenu.nowWeRide = false;
                    pauseMenu.nightOfMystery = false;
                }
                else if (musicAudioSource.clip == nowWeRide)
                {
                    pauseMenu.adventure = false;
                    pauseMenu.nowWeRide = true;
                    pauseMenu.nightOfMystery = false;
                }
                else if (musicAudioSource.clip == nightOfMystery)
                {
                    pauseMenu.adventure = false;
                    pauseMenu.nowWeRide = false;
                    pauseMenu.nightOfMystery = true;
                }
            }
            else
            {
                print("can't find pause menu script");
            }
        }
    }
    void LowerVolumeInStory()
    {
        dialogue = FindObjectOfType<Dialogue>();
        if (dialogue != null) 
        {
            //Debug.Log("Called");
            if (dialogue.lowVolume)
            {
                musicAudioSource.volume = 0.1f;
                backgroundAudioSource.volume = 0.1f;
            }
            else if(!dialogue.lowVolume)
            {

                musicAudioSource.volume = 1f;
                backgroundAudioSource.volume = 0.5f;
            }

        }
        else
        {
            musicAudioSource.volume = 1f;
            backgroundAudioSource.volume = 0.5f;
        }
    }
}
    

