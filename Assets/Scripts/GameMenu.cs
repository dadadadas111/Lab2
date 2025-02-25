using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject gameOverUI;

    private bool isPaused = false;
    private GameManager gameManager;

    private void Start()
    {
        pauseMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        pauseMenuUI.SetActive(true);
        // set pauseMenuUI to be the on top of other canvas UI
        pauseMenuUI.transform.SetAsLastSibling();
        Time.timeScale = 0f; // Pause game
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume game
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        // reload the main scene
        SceneManager.LoadScene("MainScene");
        Resume();
    }

    public void GameOver()
    {
        gameOverUI.SetActive(true);
        // set gameOverUI to be the on top of other canvas UI
        gameOverUI.transform.SetAsLastSibling();
        // update the text in the game over screen
        int killCount = gameManager.GetKillCount();
        var gameOverText = gameOverUI.transform.Find("GameOverText").GetComponent<TMPro.TextMeshProUGUI>();
        if (killCount > 0)
        {
            gameOverText.text = "Game Over! You killed " + killCount + " enemies!";
        }
        else
        {
            gameOverText.text = "Game Over! You didn't kill any enemies!";
        }
        Time.timeScale = 0f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        // SceneManager.LoadScene("MainMenu"); // Placeholder
        Debug.Log("Return to Main Menu (not implemented yet)");
    }
}
