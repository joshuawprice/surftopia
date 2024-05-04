using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    public event Action OnResetLevel;

    private List<float> _scores = new List<float>();
    public List<float> Scores { get { return _scores; } }

    public void ResetLevel()
    {
        OnResetLevel?.Invoke();
    }

    public void AddScore(float score)
    {
        _scores.Add(score);
    }
}
