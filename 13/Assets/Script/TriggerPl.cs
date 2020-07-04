using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPl : MonoBehaviour
{
    public Collider collider1;                               // Свой collider

    public void OnTriggerEnter(Collider collider)
    {        
       // Обработка столкновений в методе Player'a
       GameObject.Find("Player").GetComponent<Player>().DestroyCar(collider, collider1);
    }

}