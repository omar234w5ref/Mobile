using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSystem : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private GameObject[] enemys;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Speed_Power Up"))
        {
            Destroy(other.gameObject);
            float origSpeed = playerMovement.speed;
            StartCoroutine(SpeedPowerUp(5f));
        }

        if(other.gameObject.tag == "Cam_Power Up")
        {
            Destroy(other.gameObject);
            StartCoroutine(CamPowerUp(5f));
        }


        if(other.gameObject.tag == "StopWatch_Power Up")
        {
            Destroy(other.gameObject);
            StartCoroutine(StopWatchPowerUp(5f));
        }
    }

    public IEnumerator StopWatchPowerUp(float duration)
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < enemys.Length; i++)
        {
            if(enemys[i] != null)
            enemys[i].GetComponent<Enemy>().speed -= 3;
        }
        
        
        yield return new WaitForSeconds(duration);

        for(int i = 0; i < enemys.Length; i++)
        {
            if(enemys[i] != null)
            enemys[i].GetComponent<Enemy>().speed += 3;
        }
    }

    public IEnumerator CamPowerUp(float duration)
    {
        float originalSize = Camera.main.orthographicSize;
        float targetSize = Camera.main.orthographicSize + 2;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, originalSize + 2, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Camera.main.orthographicSize = originalSize + 2;
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetSize, (elapsedTime / (duration / 2)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Camera.main.orthographicSize = targetSize;
        yield return new WaitForSeconds(duration);
        elapsedTime = 0f;
        while (elapsedTime < duration / 2)
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, originalSize, (elapsedTime / (duration / 2)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Camera.main.orthographicSize = originalSize;
    }

    public IEnumerator SpeedPowerUp(float duration)
    {
        float origSpeed = playerMovement.speed;
        playerMovement.speed += 2;
        yield return new WaitForSeconds(duration);
        playerMovement.speed = origSpeed;
    }

}

