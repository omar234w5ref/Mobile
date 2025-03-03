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

        // Start fading loop at start
        StartCoroutine(FadeLoop(StartGameText, 1.5f));
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
    #region // fadeCode
    private IEnumerator FadeLoop(TextMeshProUGUI text, float fadeTime)
    {
        while (true)  // Infinite loop for continuous fading
        {
            yield return StartCoroutine(Fade(text, fadeTime, 0, 1));  // Fade in
            yield return StartCoroutine(Fade(text, fadeTime, 1, 0));  // Fade out
        }
    }

    private IEnumerator Fade(TextMeshProUGUI text, float fadeTime, float startAlpha, float endAlpha)
    {
        float elapsedTime = 0;
        Color c = text.color;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeTime);
            text.color = c;
            yield return null;
        }

        // Ensure exact final alpha
        c.a = endAlpha;
        text.color = c;
    }
    #endregion


    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
