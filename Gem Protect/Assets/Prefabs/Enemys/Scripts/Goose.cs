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
    public GameObject player;
    [SerializeField] private GameObject ShootingProjektile;
    [SerializeField] private GameObject shootinTransform;
    [SerializeField] private float shootinInterval;
    private float shootingTime;
    private Vector3 direction;
    private SpriteRenderer spriteRenderer;

   

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        direction = (player.transform.position - transform.position).normalized;
        distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance >= minDist)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else
        {
            shootingTime += Time.deltaTime;
            if (shootingTime >= shootinInterval)
            {
                shootingTime = 0;
                GameObject shootingObject = Instantiate(ShootingProjektile, shootinTransform.transform.position, Quaternion.identity);
                shootingObject.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            }
        }


        spriteRenderer.flipX = direction.x < 0;
    }

    

   
}