using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{

    private Animator anim;


    private bool isShooting;
    private bool isSelecting;
    [SerializeField] private float shootingInterval;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject shootingPartickle;
    [SerializeField] private Transform shootingPoint;

    private float timeToStartShootin;
    private float _elapsidTime;
    private float elapsedTime;
    private GameObject player;
    private Vector2 direction;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        timeToStartShootin = Random.Range(1, 2);
    }

    void Update()
    {
        direction = (player.transform.position - transform.position).normalized;

        if (!isShooting && !isSelecting)
        {
            _elapsidTime += Time.deltaTime;
            if (_elapsidTime > timeToStartShootin)
            {
                elapsedTime = 0;
                isSelecting = true;
            }
        }
       
        Animations();
        RotateSprite();

    }   


    public void Animations()
    {
        if (isSelecting && !isShooting)
        {
            anim.SetBool("Select", true);
            AnimatorStateInfo _animState = anim.GetCurrentAnimatorStateInfo(0);
            if(_animState.IsName("SnakeSelect") && _animState.normalizedTime >= .9f)
            {
                anim.SetBool("Select", false);
                Debug.Log("select");
                isShooting = true;
                isSelecting = false;
            }
        }

        if (isShooting)
        {
            anim.SetBool("Shoot", true);
            Shoot();
        }
       
    }


    public void Shoot()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= shootingInterval)
        {
            elapsedTime = 0;
            AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);
            if (animState.IsName("SnakeShoot") && animState.normalizedTime >= .9f)
            {
                anim.SetBool("Shoot", false);
                isShooting = false;
            }

            GameObject shootingObject = Instantiate(shootingPartickle, shootingPoint.transform.position, Quaternion.identity);
            shootingObject.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            shootingObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
    public void RotateSprite()
    {
        if (direction.x > 0)
        {
            spriteRenderer.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (direction.x < 0)
        {
            spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
