using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class Inf_Garage : MonoBehaviour
{
    public static bool btnPlayOfInf_Garage;                        // Кнопка Play
    public static int numCar = -1;                                 // Номер авто
    public GameObject car;                                         // Авто
    public GameObject[] cars = new GameObject[6];                  // Массив авто
    public Text moneyText;
    public int money;
    int[] price = new int[]{0, 450, 1200, 3000, 0, 10000};
    public GameObject Zamok;
    public Text priceText;
    public Button PlayButton;
    public AudioSource SoundButton;

    public GameObject Left, Right;


   
    public void Start()
    {
        money = Menu.money;
        moneyText.text = money.ToString("#0") + " $";

        Player.choice = 0;

        // Загрузка первого авто

        if (!PlayerPrefs.HasKey("NumCar")) numCar = 0;
        else numCar = PlayerPrefs.GetInt("NumCar");     

        car = Instantiate(
                cars[numCar],
                    new Vector3(0.03f, 0, 0.02f), Quaternion.Euler(new Vector3(0, 156, 0)));
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Back();

        if (numCar == 0) Left.SetActive(false);
            else Left.SetActive(true);

        if (numCar == 5) Right.SetActive(false);
        else Right.SetActive(true);
        
    }
    public void Back()
    {
        SoundButton.Play();
        numCar = -1;
        SceneManager.LoadScene(0);
    }

    public void Mainmenu()
    {
        PlayerPrefs.SetInt("NumCar", numCar);
        btnPlayOfInf_Garage = true;        
        SceneManager.LoadScene(0);     
        
    }

    public void Buy()
    {
        SoundButton.Play();
        if (Menu.money > price[numCar] && numCar != 4)
        {
            Zamok.SetActive(false);
            PlayButton.interactable = true;
            PlayerPrefs.SetInt(numCar + "Car", 1);
            Menu.money -= price[numCar];
            moneyText.text = "$ " + Menu.money;
            PlayerPrefs.SetFloat("money", Menu.money);
        }
            
    }

    public void Change(bool ch)
    {        

            switch (ch)
            {
                case true:
                if (numCar != 5)
                {
                    numCar += 1;
                    Destroy(car);
                    car = Instantiate(cars[numCar],
                         new Vector3(0.03f, 0, 0.02f), Quaternion.Euler(new Vector3(0, 156, 0)));

                    if (PlayerPrefs.GetInt(numCar + "Car") == 0)
                    {
                        priceText.text = "$ " + price[numCar];
                        if (numCar == 4)
                        {
                            priceText.text = " 1000 km ";
                        }
                        if (numCar == 0)
                        {
                            Zamok.SetActive(false);
                            PlayButton.interactable = true;
                        }
                        else
                        {
                            Zamok.SetActive(true);
                            PlayButton.interactable = false;
                        }
                    }
                    else
                    {                        
                        Zamok.SetActive(false);
                        PlayButton.interactable = true;
                    }

                }break;

                case false:
                if (numCar != 0)
                {
                    numCar -= 1;
                    Destroy(car);
                    car = Instantiate(cars[numCar],
                         new Vector3(0.03f, 0, 0.02f), Quaternion.Euler(new Vector3(0, 156, 0)));                    

                    if (PlayerPrefs.GetInt(numCar + "Car") == 0)
                    {
                       
                        priceText.text = "$ " + price[numCar];
                        if (numCar == 4)
                        {
                            priceText.text = " 1000 km ";
                        }
                        if (numCar == 0)
                        {
                            Zamok.SetActive(false);
                            PlayButton.interactable = true;                           
                        }
                        else
                        {
                            Zamok.SetActive(true);
                            PlayButton.interactable = false;
                        }                        
                    }
                    else
                    {
                        Zamok.SetActive(false);
                        PlayButton.interactable = true;
                    }
                }
                
                break;
            }
        if (PlayerPrefs.GetString("money1") == "done" && numCar == 4)
        {
            Zamok.SetActive(false);
            PlayButton.interactable = true;            
        }
    }

}



