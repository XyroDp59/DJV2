using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject GameOverScreen;
    [SerializeField] GameObject PauseScreen;
    [SerializeField] GameObject VictoryScreen;
    [SerializeField] GameObject VolumeControlScreen;
    
    bool canPause = true;

    public static GameManager Instance;

    public bool CanBePaused()
    {
        return canPause;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void LevelStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
        canPause = true;
    }
    public void Pause()
    {
        PauseScreen.gameObject.SetActive(true);
        Time.timeScale = 0.0f;
    }
    public void ResumePause()
    {
        PauseScreen.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void GameOver()
    {
        GameOverScreen.gameObject.SetActive(true);
        Time.timeScale = 0.0f;
        canPause = false;
    }

    public void LevelCleared()
    {
        GameOverScreen.gameObject.SetActive(true);
        Time.timeScale = 0.0f;
        canPause = false;
    }
}
