using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] float Health = 100f;
    [SerializeField] int scoreValue = 150;

    [Header("Shooting")]
    [SerializeField] float ShotCounter;
    [SerializeField] float minTimeBetweenShots = .2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] GameObject explosion;
    [SerializeField] float explosionTimer = 1f;

    [Header("Sound Effects")]
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0,1)] float deathVolume;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume;
    

    // Start is called before the first frame update
    void Start()
    {
        ShotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
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
            ShotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer dealer = other.gameObject.GetComponent<DamageDealer>();
        if (!dealer)
        {
            return;
        }
        ProcessHit(dealer);
    }

    private void ProcessHit(DamageDealer dealer)
    {
        Health -= dealer.GetDamage();
        dealer.Hit();
        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        GameObject explo = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
        Destroy(explo, explosionTimer);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathVolume);
    }
}
