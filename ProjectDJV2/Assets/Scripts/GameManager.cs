using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Canvas GameOverScreen;
    [SerializeField] Canvas PauseScreen;
    bool canPause;

    public static GameManager Instance;

    public bool CanPause()
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

    }
}
