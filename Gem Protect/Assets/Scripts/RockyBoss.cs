using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockyBoss : MonoBehaviour
{
    [Header("smallEnemys")]
    public GameObject smallEnemys;
    public float spawningInterval;

    [Header("MainAttack")]
    public GameObject rockThrow;
    public bool throwing;
    public Transform ThrowingPoint;
    public float throwingInterval;
    public float throwingSpeed;

    private GameObject gem;
    private Animator anim;
    private float elapsedTime;

    void Start()
    {
        anim = GetComponent<Animator>();
        gem = GameObject.FindGameObjectWithTag("Gem");
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        // Update animation state in every frame
        AnimatorStateInfo animationState = anim.GetCurrentAnimatorStateInfo(0);

        if (elapsedTime >= throwingInterval)
        {
            anim.SetBool("RockThrow", true);
        }

        // Check if animation is in the correct state
        if (animationState.IsName("RockThrow_RockyBos") && animationState.normalizedTime >= 0.5f)
        {
            // Only spawn the rock when throwing is true
            if (throwing)
            {
                elapsedTime = 0;
                SpawnRock();
                throwing = false; // Reset throwing after one rock throw
            }
        }

        // Stop animation after it plays
        if (animationState.IsName("RockThrow_RockyBos") && animationState.normalizedTime >= 0.9f)
        {
            anim.SetBool("RockThrow", false);
        }
    }

    void SpawnRock()
    {
        GameObject rock = Instantiate(rockThrow, ThrowingPoint.position, Quaternion.identity);
        Vector3 dir = (gem.transform.position - transform.position).normalized;
        Rigidbody2D rb = rock.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = dir * throwingSpeed;
        }
    }
}
