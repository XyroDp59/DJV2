using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Canvas GameOverScreen;

    public void LevelStart()
    {

    }
    public void Pause()
    {

    }
    public void GameOver()
    {
        GameOverScreen.gameObject.SetActive(true);
    }

    public void LevelCleared()
    {

    }
}
