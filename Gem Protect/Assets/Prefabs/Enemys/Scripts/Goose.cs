using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Goose : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float minDist;
    [SerializeField] private float bulletSpeed;
    private float distance;
    private GameObject Gem;
    private GameObject player;
    private GameObject target;
    [SerializeField] private GameObject ShootingProjektile;
    [SerializeField] private GameObject shootinTransform;
    [SerializeField] private float shootinInterval;
    private float shootingTime;
    private Vector3 direction;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    private AudioSource audioSource;
    public AudioClip sound1;
    public AudioClip sound2;

    void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();
        anim = GetComponent<Animator>();
        Gem = GameObject.FindGameObjectWithTag("Gem");
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        float myRandomNumber = Random.Range(0, 10);
        target = Gem;
    }

    void Update()
    {
        direction = (target.transform.position - transform.position).normalized;
        distance = Vector2.Distance(transform.position, target.transform.position);
        RotateSprite();
        if (distance >= minDist)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
        else
        {
            shootingTime += Time.deltaTime;
            if (shootingTime >= shootinInterval)
            {
                shootingTime = 0;
                PlaySound();
                GameObject shootingObject = Instantiate(ShootingProjektile, shootinTransform.transform.position, Quaternion.identity);
                shootingObject.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            }
        }
    }

    public void PlaySound()
    {
        if (sound1 != null)
        {
            AudioClip pickedClip = Random.Range(0, 2) == 0 ? sound1 : sound2;
        audioSource.clip = pickedClip;
        audioSource.Play();    
        }
        
    }

    public void RotateSprite()
    {
        if (direction.x < 0)
        {
            spriteRenderer.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (direction.x > 0)
        {
            spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}