using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.VFX;

public class Obstacle : MonoBehaviour, ICollisionable, ISpawnPowerUp, IExplodable
{
    [SerializeField] GameObject fx_Bloc_Explose;

    public void SpawnPowerUp()
    {
        if (UnityEngine.Random.Range(0f, 1f) >= 1f)
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
        if (collisionable is DeadZone)
        {
            Explode();
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    MovementPlayerTest move = collision.gameObject.GetComponent<MovementPlayerTest>();
    //    if (move)
    //    {

    //        Debug.Log($"{gameObject.name}");

    //        move.PlayerBlocked();
    //    }
    //}

    public void Explode()
    {
        //Debug.Log("CAISSE EXPLOSEE");
        SpawnPowerUp();
        if (fx_Bloc_Explose) Instantiate(fx_Bloc_Explose, transform.position, Quaternion.identity);
        //GameObject Go = Instantiate(new GameObject(), transform.position, Quaternion.identity);
        //AudioSource aus = gameObject.AddComponent<AudioSource>();
        //aus.clip = destroyClip;
        //aus.Play();
        //Destroy(Go, aus.clip.length);
        DestroyObject();
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
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
