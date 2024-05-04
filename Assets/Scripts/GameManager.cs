using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Game Manager singleton setup.
    private static GameManager _instance;
    // Lazy load singleton if we need to.
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

    private void Awake()
    {
        // Using the singleton pattern, ensure only one instance of GameManager exists.
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ResetLevel()
    {
        OnResetLevel?.Invoke();
    }

    public void FinishLevel()
    {
        OnFinishLevel?.Invoke();

        // Unlock the cursor from the center of the screen.
        Cursor.lockState = CursorLockMode.None;
        // Show the cursor again.
        Cursor.visible = true;

        SceneManager.LoadScene("Scoreboard");
    }

    public void AddScore(float score)
    {
        _scores.Add(score);
        _scores.Sort();
    }
}
