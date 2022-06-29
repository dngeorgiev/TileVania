using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;

    private readonly int _firstSceneBuildIndex = 0;

    private void Awake()
    {
        int numberOfGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numberOfGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    private void TakeLife()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        playerLives--;

        SceneManager.LoadScene(currentSceneIndex);
    }

    private void ResetGameSession()
    {
        SceneManager.LoadScene(_firstSceneBuildIndex);
        Destroy(gameObject);
    }
}
