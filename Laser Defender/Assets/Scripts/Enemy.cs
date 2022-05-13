using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] float health = 100f;
    [SerializeField] int ScoreValue = 100;
    [SerializeField] GameObject ExplosionVFX;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0,1)] float deathSoundVolume = 0.6f;

    [Header("Shooting")]    
    [SerializeField] float ShotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject EnemyLaser;
    [SerializeField] float projectileSpeed = 10f; 
    [SerializeField] AudioClip LaserShotSound;
    [SerializeField] [Range(0, 1)] float projectileVolume;


    void Start()
    {
        ShotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);   
    }

    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        ShotCounter -= Time.deltaTime;
        if (ShotCounter <= 0)
        {
            Fire();
            ShotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(EnemyLaser, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(1, -projectileSpeed);
        AudioSource.PlayClipAtPoint(LaserShotSound, Camera.main.transform.position, projectileVolume);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        HitMethod(damageDealer);
    }

    private void HitMethod(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(ScoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(ExplosionVFX, transform.position, transform.rotation);
        Destroy(explosion, 1f);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }
}
