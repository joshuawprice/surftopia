using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Game Manager singleton setup.
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject gameManager = new GameObject("GameManager");
                _instance = gameManager.AddComponent<GameManager>();
                DontDestroyOnLoad(gameManager);
            }
            return _instance;
        }
    }

    private List<float> _scores = new List<float>();
    public List<float> Scores { get { return _scores; } }

    public event Action OnResetLevel;
    public event Action OnFinishLevel;

    public void ResetLevel()
    {
        OnResetLevel?.Invoke();
    }

    public void FinishLevel()
    {
        OnFinishLevel?.Invoke();
        SceneManager.LoadScene("Scoreboard");
    }

    public void AddScore(float score)
    {
        _scores.Add(score);
    }
}
