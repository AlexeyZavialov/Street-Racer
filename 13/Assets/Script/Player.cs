using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    public GameObject panelFail;                               // Панель проигрыша
    public GameObject PlayerCar;                               // Авто-Плэйер
    public GameObject Road;                                    // Удаление при Fail
    public GameObject Barrier;                                 // Удаление при Fail


    private GameObject[] wheels = new GameObject[] { };        // Колёса у Player'a
    public GameObject[] cars = new GameObject[] { };           // Массив авто для спавна Player'а

    public static float choice = 0;                            // Выбор режима

    public static float speedAngular =2.5f;                    // Угловая скорость
    public static float s;                                     // Пройденный путь
    public float IndexSp;                                      // Коэффицент скорости (отличительная особенность у машин)
    public float IndexSpCount;                                 // Сохранение начального коэфицента скорости
    public static float scor;                                  // Кол-во очков
    public int money;

    float hor1;                                                // Переменная для перемещения авто по Horizontal
    public static float speed;                                 // Скорость                             
    public static float aBrake;                                // Торможение (обратное ускорение)    

    public Text count;                                         // Вывод на экран Пройденной расстояние
    public Text recordS;                                       // Вывод на экран рекорд дистанции
    public Text speedText;                                     // Вывод на экран скорость    
    public Text scorText;                                      // Вывод на экран кол-во очков
    public Text recordScor;                                    // Вывод на экран рекорд очков
    public Text moneyText;                                     // Вывод на экран рекорд очков

    public TextMeshProUGUI rezultS;                            // Вывод пройденного расстояния на экран Пройгрыша
    public TextMeshProUGUI rezultScor;                         // Вывод результат очков на Панель пройгрыша
    public TextMeshProUGUI Fail_txt;                           // Надпись Fail c названием режима на Панели пройгрыша
    public TextMeshProUGUI record_1;                           // Красный индикатор нового рекорда расстояния
    public TextMeshProUGUI record_2;                           // Красный индикатор нового рекорда очков


    public AudioMixerGroup Mixer;
    public AudioSource SoundAcc;  
   


    void Start()
    {
       

        // Создание авто у Player'a                
        if (Inf_Garage.numCar == -1) Inf_Garage.numCar = PlayerPrefs.GetInt("NumCar");              
        PlayerCar = Instantiate(cars[Inf_Garage.numCar]) as GameObject;
        
         wheels = GameObject.FindGameObjectsWithTag("FWheel");                                         // Определение первых колёс для поворотов
        for (int i = 0; i < wheels.Length; i++) wheels[i].tag = "FirstWheel";
            
        PlayerCar.gameObject.GetComponent<MoveCars>().enabled = false;                                 // Выключение скрипта MoveCars
        PlayerCar.gameObject.GetComponent<BoxCollider>().enabled = false;                              // Выключение Box Collider'a
                
        Destroy(PlayerCar.gameObject.GetComponent<MoveCars>().Coin);                                   // Удаление компонента Coin
        Destroy(PlayerCar.gameObject.GetComponent<MoveCars>().trigger);                                // Удаление компонента Trigger
        Destroy(PlayerCar.gameObject.GetComponent<MoveCars>().TriggerForTheMe);                        // Удаление компонента TriggerForTheMe

        PlayerCar.gameObject.GetComponent<MoveCars>().PlayerTrigger.SetActive(true);                   // Активация компонента PlayerTrigger

        PlayerCar.transform.position = transform.position;
        PlayerCar.transform.parent = transform;                                                        // Привязка позиции
        PlayerCar.tag = "Player";

        IndexSp = PlayerCar.gameObject.GetComponent<MoveCars>().IndexSpeed;                            // Выгрузка IndexSpeed из скрипта MoveCars
        IndexSpCount = IndexSp;
                     
       

        s = 0;
        speed = 0;
        scor = 0;

        StartCoroutine(Coins());       
                
        // Вывод рекорда расстояния
        if (!PlayerPrefs.HasKey("recordOf"+choice)) recordS.text = "0 km";
            else recordS.text = "Record: " + PlayerPrefs.GetFloat("recordOf"+ choice).ToString("#0") + " km";

        // Вывод рекорда Очков
        if (!PlayerPrefs.HasKey("recordScor"+choice)) recordScor.text = "0 очков";
            else recordScor.text = "record: " + PlayerPrefs.GetFloat("recordScor"+choice).ToString("#0");
    }
    

    void Update()
    {

        // Расчёт скорости + вывод       
          
        if (Interface.clickBrake)
        {
            speed = speed +  Interface.a * 2f  * Time.deltaTime;
           
        }
        else
            speed = speed + IndexSp / 2 * Interface.a / 2 * Time.deltaTime;
        
        
        if (speed > 5f * IndexSp / 5) speed = 5f * IndexSp / 5;
         if (speed < 0) speed = 0;
            speedText.text = "Speed: " + (speed * 10).ToString("#00");


        // Расчёт пройденного пути + вывод
        s = s + speed * Time.deltaTime;
        if (PlayerPrefs.HasKey("money" + choice))
        {
            count.text = s.ToString("#0");
        }
        else count.text = s.ToString("#0") + "  / 1000 km" ;
        
       
        // Перемещение авто
        hor1 = Input.GetAxis("Horizontal");
        float hor = Interface.Click;
             if (hor1 != 0) hor = hor1;
        

        Vector3 dir = new Vector3(hor, 0, 0);
        
        if ((-1.9f > transform.position.x && hor == -1f) || (1.9f < transform.position.x && hor == 1f)) hor = hor + 0;
        else
        {
            // Поворот тачки
            transform.rotation = Quaternion.Euler(0, hor*3, 0);

                                                       
            // Движение авто влево/вправо
            transform.Translate(dir.normalized * Time.deltaTime * speedAngular);

            // Поворот передних колёс
            wheels = GameObject.FindGameObjectsWithTag("FirstWheel");
            for (int i = 0; i < wheels.Length; i++)                
                wheels[i].transform.rotation = Quaternion.Euler(0, hor * 40, 0);
        }

        if (transform.position.z != -2) transform.position = new Vector3(transform.position.x ,transform.position.y, -2);
                
    }


     //Столкновение с машинами
    public void DestroyCar(Collider collider, Collider collider2) 
    {      
        
       
        if (collider.tag == "TriggerME" && collider2.tag == "Player")
        {
            Time.timeScale = 0;
            SoundAcc.Play();
            // Destroy(Road);
             //Destroy(Barrier);
            count.enabled = false;                                        
            recordS.enabled = false;
            speedText.enabled = false;
            scorText.enabled = false;
            recordScor.enabled = false;
                    

            GameObject.Find("Left").SetActive(false);
            GameObject.Find("Right").SetActive(false);
            GameObject.Find("PauseBtn").SetActive(false);

            Destroy(gameObject);                      // Уничтожение авто

            GameObject.Find("Interface").GetComponent<Interface>().OnBrakeUp();

            Interface.Click = 0;
            
            // Запись рекорда расстояния
            if (s > PlayerPrefs.GetFloat("recordOf" + choice))
            {
                PlayerPrefs.SetFloat("recordOf" + choice, s);                
                record_1.gameObject.SetActive(true);
            }
            rezultS.text = "Distance = " + s.ToString("#0") + "  / 1000 km";


            // Запись рекорда очков
            if (scor > PlayerPrefs.GetFloat("recordScor"+choice)) 
            {
                PlayerPrefs.SetFloat("recordScor"+choice, scor);                
                record_2.gameObject.SetActive(true);
            }
             rezultScor.text = "Scor = " + scor.ToString("#0");


            switch (choice)
            {
                case 1: 
                    Fail_txt.text = "FAIL - 1 way";
                    if (s >= 1000 && PlayerPrefs.GetString("money1") != "done")
                    {                        
                        PlayerPrefs.SetString("money1", "done");
                    }
                    break;

                case 2: 
                    Fail_txt.text = "FAIL - Potok";
                    if (s >= 1000 && PlayerPrefs.GetString("money2") != "done")
                    {
                        money += 5000;
                        PlayerPrefs.SetString("money2", "done");
                    }
                    break;

                case 3:
                    Fail_txt.text = "FAIL - Vstrechka";
                    if (s >= 1000 && PlayerPrefs.GetString("money3") != "done")
                    {
                        money += 10000;
                        PlayerPrefs.SetString("money3", "done");
                    }
                    break;
            }

            money = Menu.money;                                        
            money += (int)scor / 8;                                         
            moneyText.text = "$ " + money.ToString("#0");
            rezultScor.text += "     + $ " + (scor / 10).ToString("#0");
            Menu.money = (int) money;
            PlayerPrefs.SetInt("money", money);

            // Загрузка Fail_Меню                       
            panelFail.SetActive(true);
        }
        
        // +100 Coin 
        if (collider.tag == "Coin" && speed > 8f)        {
            
            scor += 8 * speed;
            scorText.color = new Color(255,255,0);
            Invoke("ColorChange", 0.3f);           
        }
        
    }
    
   

    // Подсчёт очков
    IEnumerator Coins()
    {
        while (true)
        {
            if (speed >= 4f) 
                scor = scor + speed * 0.23f;
                    scorText.text = scor.ToString("#0") + " очков";

            yield return new WaitForSeconds(0.1f);
        }
    }

    // Жёлтое мигание очков (+100)
    public void ColorChange()
    {
        scorText.color = new Color(255, 255, 255);
    }
}
