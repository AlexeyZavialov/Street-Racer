
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class Menu : MonoBehaviour
{

    public GameObject panChoice;                              // Панель выбора режима
    public GameObject panMenu;                                // Панель Меню
    public GameObject panExit;                                // Панель при выходе
    public GameObject SoundBG;
    public AudioSource SoundButton;

    public static float money;                                  // Кол-во денег

    public AudioMixerGroup MixerMusic;
    public AudioMixerGroup MixerSounds;

    public GameObject panSettings;                                     // Панель настроек
    public Toggle togMusic;
    public static float musVolume;

    public Toggle togSounds;
    public static float musSounds;

    private void Awake()
    {        
        DontDestroyOnLoad(SoundBG);

        if (GameObject.FindGameObjectsWithTag("music").Length == 2) Destroy(SoundBG);
    }

    void Start()
    {
        // Загрузка данных о деньгах
        if (!PlayerPrefs.HasKey("money")) money = 0;
        else money = PlayerPrefs.GetFloat("money");

        // Загрузка данных о музыке
        if (!PlayerPrefs.HasKey("musVolume")) musVolume = 0;
        else musVolume = PlayerPrefs.GetFloat("musVolume");
        MixerMusic.audioMixer.SetFloat("MusicVolume", musVolume);

        if (musVolume == 0) togMusic.isOn = true;
        else togMusic.isOn = false;

        // Загрузка данных о звуках
        if (!PlayerPrefs.HasKey("musSounds")) musSounds = 0;
        else musSounds = PlayerPrefs.GetFloat("musSounds");
        MixerSounds.audioMixer.SetFloat("MusicSounds", musSounds);

        if (musSounds == 0) togSounds.isOn = true;
        else togSounds.isOn = false;


        if (!PlayerPrefs.HasKey("NumCar")) PlayerPrefs.SetInt("NumCar", 0);
        
        panMenu.SetActive(true);
        panChoice.SetActive(false);
        panExit.SetActive(false);

        if (Inf_Garage.btnPlayOfInf_Garage)
        {
            panMenu.SetActive(false);
            panChoice.SetActive(true);
        }
    }

    void Update()
    {
        // Активация панели выхода кнопкой
        if (panSettings.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            panSettings.SetActive(false);
            panMenu.SetActive(true);
        }
        else if (panExit.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
            panExit.SetActive(false);

        else if (Input.GetKeyDown(KeyCode.Escape) && !panChoice.activeSelf)
            panExit.SetActive(true);

        else if (panChoice.activeSelf && Input.GetKeyDown(KeyCode.Escape)) OnClickBack();
    }

    public void OnSettings()
    {
        panSettings.SetActive(true);
        panMenu.SetActive(false);
    }

    public void Back()
    {
        panSettings.SetActive(false);
        panMenu.SetActive(true);
    }
    public void OnClickPlay()
    {
        SoundButton.Play();
        panChoice.SetActive(true);
        panMenu.SetActive(false);
    }

    public void OnClickBack()
    {
        SoundButton.Play();
        panMenu.SetActive(true);
        panChoice.SetActive(false);        
    }

    public void OnClickGarage()
    {
        SoundButton.Play();
        SceneManager.LoadScene(4);
    }

    public void OnClickStart(int mode)
    {
        SoundButton.Play();
        switch (mode)
        {
            case 1: SceneManager.LoadScene(1);
                Player.choice = 1;
                break;

            case 2: SceneManager.LoadScene(2);
                Player.choice = 2;
                break;

            case 3: SceneManager.LoadScene(3);
                Player.choice = 3;
                break;
        }        
        panChoice.SetActive(false);
    }

    public void OnClickExit()
    {
        SoundButton.Play();
        panExit.SetActive(true);        
    }

    public void ClickPanExit(int choice)
    {
        SoundButton.Play();
        switch (choice)
        {
            case 1: Application.Quit(); break;

            case 0: panExit.SetActive(false); break;

        }           
        
    }

    public void ToggleMusic(Toggle toggle)
    {
        switch (toggle.isOn)
        {
            case true:
                MixerMusic.audioMixer.SetFloat("MusicVolume", 0);
                PlayerPrefs.SetFloat("musVolume", 0);
                musVolume = 0;
                break;

            case false:
                MixerMusic.audioMixer.SetFloat("MusicVolume", -80);
                PlayerPrefs.SetFloat("musVolume", -80);
                musVolume = -80;
                break;
        }

    }

    public void ToggleSounds(Toggle toggle)
    {
        switch (toggle.isOn)
        {
            case true:
                MixerSounds.audioMixer.SetFloat("MusicSounds", 0);
                PlayerPrefs.SetFloat("musSounds", 0);
                musSounds = 0;
                break;

            case false:
                MixerSounds.audioMixer.SetFloat("MusicSounds", -80);
                PlayerPrefs.SetFloat("musSounds", -80);
                musSounds = -80;
                break;
        }

    }



}
