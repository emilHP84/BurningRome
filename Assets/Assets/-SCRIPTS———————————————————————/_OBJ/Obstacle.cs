using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IBreakable, IDetect, ICollisionable, ISpawnPowerUp
{
    public void Break()
    {
        SpawnPowerUp();
        Destroy(gameObject);
    }

    public void OnDetectionWith(IDetect detect)
    {
        SpawnPowerUp();
        Destroy(gameObject);

    }



    public void SpawnPowerUp()
    {
        if (UnityEngine.Random.Range(0f, 1f) >= 0.1f)
        {
            return; // Échec du tirage global  rien ne se passe
        }

        foreach (PowerUpData powerUp in powerUps)
        {
            float chance = powerUp.pourcentage / 100f;
            float tirage = UnityEngine.Random.Range(0f, 1f);

            if (tirage < chance)
            {
                Instantiate(powerUp.prefab, transform.position, Quaternion.identity);
                break; // On stoppe dès qu’un power-up est choisi
            }
        }

    }

    public void OnCollisionWith(ICollisionable collisionable)
    {

    }

    [SerializeField]
    private List<PowerUpData> powerUps;

    [Serializable]
    public class PowerUpData
    {
        public GameObject prefab;
        public float pourcentage;
    }
   
}
