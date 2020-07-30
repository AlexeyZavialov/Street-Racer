using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCars : MonoBehaviour
{
    float choice = Player.choice;                                        // Выбор режима
    public float IndexSpeed;                                             // Коэффицент скорости (отличительная особенность у машин) 
    public string coltag;                                                // Сохранение начального тэга
    private GameObject[] wheels = new GameObject[] { };                  // Колёса тачек

    public GameObject Coin;                                              //  Триггер Coin
    public GameObject trigger;                                           //  Триггер Trigger
    public GameObject TriggerForTheMe;                                   //  Триггер TriggerForTheMe
    public GameObject PlayerTrigger;                                     //  Триггер PlayerTrigger

    public AudioSource signal;
    public bool playsound;

    private void Awake()
    {
        //DontDestroyOnLoad(Signal);

        //if (GameObject.FindGameObjectsWithTag("music").Length == 2) Destroy(SoundBG);
    }

    void Start()
    {
        trigger.SetActive(true);                                         //  Включение триггера Trigger
        TriggerForTheMe.SetActive(true);                                 //  Включение триггера TriggerForTheMe
        Coin.SetActive(true);                                            //  Включение триггера Coin

        //Signal.transform.SetSiblingIndex(-1);

        coltag = trigger.tag;

        // Определение направления авто
        if (transform.position.x < 0 && choice == 1) choice = 3;
        else if (transform.position.x > 0 && choice == 1) choice = 2;
       
       
    }
    void Update()
    {
        
        float speed = Player.speed;
        if (Player.s > 350)
        {
            speed *= 1.2f;
        }
        else if (Player.s > 600)
        {
            speed *= 1.3f;
        }

        // Движение машины вверх/вниз  

        switch (choice)
        {
            case 2: transform.Translate(Vector3.forward * (speed * -1.5f + IndexSpeed) * Time.deltaTime); break;

            case 3: transform.Translate(Vector3.forward * (speed * 0.5f + IndexSpeed*0.8f) * Time.deltaTime); break;

            case 0: speed = 0; break;
        }        
                           

        // Удаление авто
        
        if ((transform.position.z > 17f || transform.position.z < -5f) && !playsound)
            Destroy(gameObject);
                      
    }

    private IEnumerator PlayOnDestroy()
    {            
        yield return new WaitForSeconds(signal.clip.length);
        if (transform.position.z > 17f || transform.position.z < -5f)
            Destroy(gameObject);        
    }



    public void OnTriggerEnter(Collider collider)
    {

        // Сбавление скорости  перед авто 

        switch (collider.tag)
        {
            case "Bus": if (IndexSpeed >= 7)
                {
                    IndexSpeed = 6.8f;
                    trigger.tag = "Bus";
                }
                break;

            case "StanceCar": if (IndexSpeed >= 11)
                {
                    IndexSpeed = 10.8f;
                    trigger.tag = "StanceCar";
                }
                break;            

            case "Car":if (IndexSpeed >= 10) 
                {
                    IndexSpeed = 9.8f;
                    trigger.tag = "Car";
                }
                break;

            case "redCar": if (IndexSpeed >= 13)
                {
                    IndexSpeed = 12.8f;
                    trigger.tag = "redCar";
                }
                break;

            case "Taxi": if (IndexSpeed >= 12)
                {
                    IndexSpeed = 11.8f;
                    trigger.tag = "Taxi";
                }
                break;

            case "PoliceCar":
                if (IndexSpeed >= 14)
                {
                    IndexSpeed = 13.7f;
                    trigger.tag = "PoliceCar";
                }
                break;

            case "Player":
                int num = Random.Range(0, 3);
                if (num == 0)
                {
                    playsound = true;
                    signal.Play();
                    StartCoroutine(PlayOnDestroy());
                }
                break;

            default:                
                break;
        }

    }
   
}
