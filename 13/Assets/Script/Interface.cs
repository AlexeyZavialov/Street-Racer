using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Interface : MonoBehaviour {

    public GameObject panelPause;                                      // Панель Пауза
    public GameObject panBrake;                                        // Панель тормоза
    public GameObject panSettings;                                     // Панель настроек

    public static int Click = 0;                                       // Движение по Horizontal
    public static float a = 1.5f;                                        // Ускорение авто Player'a
    public static float aBrake_ = -5f;                                 // Торможение (обратное ускорение)      
    public static bool clickBrake;                                     // Нажати тормоза     
    public bool rightAct, leftAct;                                     // Check нажатия кнопки Left/Right                

    public AudioSource Sound;
    public AudioSource CarAccel;
    public AudioSource CarUsuale;

    [Header("Audio")]
    public AudioMixerGroup MixerMusic;
    public Toggle togMusic;
    [Space]
    public AudioMixerGroup MixerSounds;
    public Toggle togSounds;

    private void Start()
    {
        CarAccel.enabled = true;
        CarUsuale.enabled = false;

        switch(Menu.musVolume){
            case 0:  MixerMusic.audioMixer.SetFloat("MusicVolume", -12); break;

            case -80: MixerMusic.audioMixer.SetFloat("MusicVolume", Menu.musVolume); break;
        }
       

        if (Menu.musVolume == 0) togMusic.isOn = true;
        else togMusic.isOn = false;


        MixerSounds.audioMixer.SetFloat("MusicSounds", Menu.musSounds);

        if (Menu.musSounds == 0) togSounds.isOn = true;
        else togSounds.isOn = false;
    }

    void Update()
    {
        Sound.enabled = false;
        // Активация панели Pause     
        if (panelPause.activeInHierarchy == false && Input.GetKeyDown(KeyCode.Escape))
            OnPaused();

        if (Input.GetKeyDown(KeyCode.Space)) OnBrakeDown();

        if (Input.GetKeyUp(KeyCode.Space)) OnBrakeUp();
    }



    // Меню паузы
    public void OnPaused()
    {
        CarAccel.Stop();
        Time.timeScale = 0;
        panBrake.SetActive(false);
        panelPause.SetActive(true);
    }

    public void Back()
    {
        panSettings.SetActive(false);
        panelPause.SetActive(true);
    }

    public void ToggleMusic(Toggle toggle)
    {        
        switch (toggle.isOn)
        {
            case true:
                MixerMusic.audioMixer.SetFloat("MusicVolume", -12);                
                PlayerPrefs.SetFloat("musVolume", 0);
                Menu.musVolume = 0;
                break;

            case false:
                MixerMusic.audioMixer.SetFloat("MusicVolume", -80);
                Menu.musVolume = -80;                
                PlayerPrefs.SetFloat("musVolume", -80); 
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
                Menu.musSounds = 0;
                break;

            case false:
                MixerSounds.audioMixer.SetFloat("MusicSounds", -80);
                PlayerPrefs.SetFloat("musSounds", -80);
                Menu.musSounds = -80;
                break;
        }

    }
    public void OnSettings()
    {
        panSettings.SetActive(true);        
        panelPause.SetActive(false);
    }

    public void Continue()
    {
        CarAccel.Play();
        Sound.Play();
         Time.timeScale = 1;        
        panBrake.SetActive(true);       
        panelPause.SetActive(false);      
        
    }

    public void Mainmenu()
    {
        Inf_Garage.btnPlayOfInf_Garage = false;
        Time.timeScale = 1;
        Sound.Play();
        panBrake.SetActive(true);
        SceneManager.LoadScene(0);        
    }

    public void Restart()
    {   
       
        Time.timeScale = 1; 
        Sound.Play();

        switch (Player.choice)
        {
            case 1: SceneManager.LoadScene(1); break;

            case 2: SceneManager.LoadScene(2); break;

            case 3: SceneManager.LoadScene(3); break;
        }        

        Player.speed = 0;
        Player.s = 0;

        panBrake.SetActive(true);
    }

    

    // Реакция на нажатие кнопок
    public void OnMouseEnter(GameObject gm)
    {
        if (gm.tag == "Left")
        {
            Click = -1;           
            leftAct = true;
        }
        else if (gm.tag == "Right")
        {
            Click = 1;
            rightAct = true;
        }
      
       
    }

    public void OnMouseExit(GameObject gm)
    {
        if (gm.tag == "Left")leftAct = false;        
            else if (gm.tag == "Right") rightAct = false;

        if ((gm.tag == "Right" || gm.tag == "Left") && (!leftAct && !rightAct )) Click = 0;
        
    }

  

    public void OnBrakeUp()
    {
        CarAccel.enabled = true;
        CarUsuale.enabled = false;
        a = Mathf.Abs(a) ;              
        clickBrake = false;
        GameObject.Find("Player").GetComponent<Player>().IndexSp = GameObject.Find("Player").GetComponent<Player>().IndexSpCount;        
    }

    public void OnBrakeDown()
    {
        CarAccel.enabled = false;
        CarUsuale.enabled = true;
        clickBrake = true;
        a = aBrake_ ;
        GameObject.Find("Player").GetComponent<Player>().IndexSp *= 3;
    }
}
