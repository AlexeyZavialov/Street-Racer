﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cars: MonoBehaviour
{    
    public GameObject[] cars;                                            // Массив с авто
    public GameObject[] carsOnRoad;                                      // Массив для авто
    private float[] positions = { -1.52f, -0.5f, 0.57f, 1.51f };         // Позиции для спавна авто


    // Создание автомобиля через каждую секунду
    void Start()
    {
        Player.choice = 3;
        StartCoroutine(spawn());
    }

    IEnumerator spawn()
    {
        while (true)
        {
            bool spawnB = true;
            carsOnRoad = GameObject.FindGameObjectsWithTag("Car");                        // Нахождение всех авто на сцене

            for (int i = 0; i < carsOnRoad.Length; i++)
                if (carsOnRoad[i].transform.position.z > 13.5f) spawnB = false;           // Проверка, не мешают ли авто спавну

            if (Player.speed >= 3f && spawnB)
            {
                Instantiate(
                cars[Random.Range(0, cars.Length)],
                    new Vector3(positions[Random.Range(0, 2)], 0, 16), Quaternion.Euler(new Vector3(0, 180, 0)));

                Instantiate(
                cars[Random.Range(0, cars.Length)],
                   new Vector3(positions[Random.Range(2, 4)], 0, 16), Quaternion.Euler(new Vector3(0, 180, 0)));
            }
            yield return new WaitForSeconds(0.65f);
        }

    }






}