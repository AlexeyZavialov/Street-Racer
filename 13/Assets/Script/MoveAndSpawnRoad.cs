using System.Collections;
using UnityEditor;
using UnityEngine;

public class MoveAndSpawnRoad : MonoBehaviour
{
    public GameObject[] roads = new GameObject[]{};                         // Массив дорог

    public GameObject[] barriers = new GameObject[] { };

    private void Start()
    {
       for (int i=0;i<4;i++)
       barriers[i].transform.parent = roads[i].transform;

    }

    void Update()
    {        

        // Приём скорости для дороги
        float speed = Player.speed;
        
        
        // Движение дороги
        for (int i = 0; i < 4; i++)
        {
            
            roads[i].transform.Translate(Vector3.forward * speed * Time.deltaTime);
            if (roads[i].transform.position.z > 18f)
                roads[i].gameObject.SetActive(false);
            else roads[i].gameObject.SetActive(true);

            // Перемещение дороги
            if (roads[i].transform.position.z < -6.5f)                
                roads[i].transform.SetPositionAndRotation(new Vector3(0, 0, roads[i].transform.position.z + 26f),
                    new Quaternion(0, 180f, 0, 0));
            
        }

       
         


    }




}
