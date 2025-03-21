using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public ParticleSystem deathEffect;
    public Image HealthBar;
    public int maxHealth = 100;
    public int currentHealth;
    [SerializeField] private GameObject deathPanel;
    public GameObject[] afterDeathStop;
    public TextMeshProUGUI healthText;
    public bool waveOver;
    void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void Update()
    {
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (waveOver == true)
        {
            if (currentHealth != maxHealth)
            {
                AddHealth(maxHealth);
            }
        }

    }
    void UpdateHealthBar()
    {
        HealthBar.fillAmount =  (float)currentHealth / maxHealth;
        healthText.text = currentHealth.ToString()+"";
    }

    public void AddHealth(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthBar();
    }

    public void TakeHealth(int amount)
    {
        currentHealth -= amount;
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            Die();
        }
        FindAnyObjectByType<AudioManager>().Play("PlayerHitSfx");

    }

    void Die()
    {
        deathPanel.SetActive(true);
        for (int i = 0; i < afterDeathStop.Length; i++)
        {
            afterDeathStop[i].SetActive(false);
        }
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "EnemyBullet")
        {
            Destroy(other.gameObject);
            TakeHealth(5);
        }
    }
}
