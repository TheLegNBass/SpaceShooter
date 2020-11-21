using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = .5f;
    [SerializeField] int health = 300;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)]  float deathVolume;
    [SerializeField] GameObject explosion;
    [SerializeField] float explosionTimer;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0,1)] float shootSoundVolume;

    [Header("Projectile")]
    [SerializeField] GameObject lazerPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.1f;

    Coroutine FiringCoroutine;

    float xMin;
    float xMax;
    float yMin;
    float yMax;


    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBounds();
    }

    private void SetUpMoveBounds()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
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
        health -= dealer.GetDamage();
        dealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        GameObject explo = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
        Destroy(explo, explosionTimer);
        AudioSource.PlayClipAtPoint(deathSound, transform.position, deathVolume);
        FindObjectOfType<Level>().LoadGameOver();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            FiringCoroutine = StartCoroutine(FireRepeatedly());
        }
        
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(FiringCoroutine);
        }
    }

    IEnumerator FireRepeatedly()
    {
        while (true)
        {
            GameObject laser = Instantiate(lazerPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);

        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
    }

    public int GetHealth()
    {
        return health;
    }
}
