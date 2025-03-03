using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotFriend : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackInterval;
    [SerializeField] private GameObject attackingObject;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private float stoppingDistance = 1.0f; // Stopping distance when following the player

    // State flags
    private bool followPlayer;
    private bool followEnemy;
    private bool attackingEnemys;
    private float shootingTime;

    // References
    private GameObject player;
    private GameObject[] Enemys;
    private Vector3 nearestEnemy;
    private GameObject targetEnemyObj; // Currently targeted enemy
    private Animator anim;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        CheckForEnemys();

        if (followPlayer)
        {
            RobotMovement();
        }
        if (followEnemy)
        {
            RobotFollowEnemy();
        }
        if (attackingEnemys)
        {
            RobotAttack(nearestEnemy);
        }

        Animatons();
    }

    public void Animatons()
    {
        if (followPlayer) {

            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }
    }
    private void CheckForEnemys()
    {
        // If we already have a target enemy, check its status.
        if (targetEnemyObj != null)
        {
            // Check if the enemy is still active in the scene.
            if (targetEnemyObj.activeInHierarchy)
            {
                float distanceToTarget = Vector2.Distance(transform.position, targetEnemyObj.transform.position);
                // If within attack radius, attack the enemy.
                if (distanceToTarget <= attackRadius)
                {
                    nearestEnemy = targetEnemyObj.transform.position;
                    attackingEnemys = true;
                    followEnemy = false;
                    followPlayer = false;
                    return;
                }
                else
                {
                    // Enemy is still alive but outside attack radius: follow it.
                    nearestEnemy = targetEnemyObj.transform.position;
                    attackingEnemys = false;
                    followEnemy = true;
                    followPlayer = false;
                    return;
                }
            }
            else
            {
                // Target enemy is dead.
                targetEnemyObj = null;
            }
        }

        // If no target enemy exists, search among all enemies for one within the attack radius.
        GameObject foundEnemy = null;
        for (int i = 0; i < Enemys.Length; i++)
        {
            float distance = Vector2.Distance(transform.position, Enemys[i].transform.position);
            if (distance < attackRadius)
            {
                foundEnemy = Enemys[i];
                break;
            }
        }
        if (foundEnemy != null)
        {
            targetEnemyObj = foundEnemy;
            nearestEnemy = targetEnemyObj.transform.position;
            attackingEnemys = true;
            followEnemy = false;
            followPlayer = false;
        }
        else
        {
            // No enemy in range and no previously targeted enemy: follow the player.
            nearestEnemy = Vector3.zero;
            attackingEnemys = false;
            followEnemy = false;
            followPlayer = true;
        }
    }

    // Follow the player (with a stopping distance)
    public void RobotMovement()
    {
        // Flip based on player's position.
        FlipTowards(player.transform.position);

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer > stoppingDistance)
        {
            Vector2 target = player.transform.position;
            transform.position = Vector2.Lerp(transform.position, target, speed * Time.deltaTime);
        }
    }

    // Follow the target enemy when it's outside the attack range.
    public void RobotFollowEnemy()
    {
        if (targetEnemyObj != null)
        {
            // Flip based on enemy's position.
            FlipTowards(targetEnemyObj.transform.position);

            Vector2 enemyPos = targetEnemyObj.transform.position;
            transform.position = Vector2.Lerp(transform.position, enemyPos, speed * Time.deltaTime);
        }
    }

    // Attack the enemy when within range.
    public void RobotAttack(Vector3 enemyPosition)
    {
        // Flip based on enemy's position.
        FlipTowards(enemyPosition);

        // Ensure the enemyPosition is valid (not Vector3.zero)
        if (enemyPosition != Vector3.zero)
        {
            if (Time.time >= shootingTime)
            {
                GameObject bullet = Instantiate(attackingObject, shootingPoint.position, Quaternion.identity);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // Set continuous collision detection to avoid tunneling issues
                    rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

                    // Calculate the normalized direction from the shooting point to the enemy
                    Vector2 direction = ((Vector2)enemyPosition - (Vector2)shootingPoint.position).normalized;
                    rb.velocity = direction * attackSpeed;
                }
                shootingTime = Time.time + attackInterval;
            }
        }
    }

    // Flip the robot's sprite to face the target position.
    private void FlipTowards(Vector3 target)
    {
        if (target.x < transform.position.x)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else if (target.x > transform.position.x)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
