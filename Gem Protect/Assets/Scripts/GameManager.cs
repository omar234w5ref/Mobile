using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private TextMeshProUGUI StartGameText;
    [SerializeField] private GameObject StartGameButton;
    [SerializeField] private GameObject StartShop;
    [SerializeField] private GameObject[] NotSeeOnStart;


    private void Start()
    {
        enemySpawner.enabled = false;
        foreach (var gameObject in NotSeeOnStart)
        {
            gameObject.SetActive(false);
        }

    }

    void Update()
    {
       
    }

    public void StartGame()
    {
        enemySpawner.enabled = true;
        StartGameButton.SetActive(false);
        foreach (var gameObject in NotSeeOnStart)
        {
            gameObject.SetActive(true);
        }
        StartShop.SetActive(true);
    }
    


    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
