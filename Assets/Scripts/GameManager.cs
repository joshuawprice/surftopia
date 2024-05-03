using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get => _instance; private set { _instance = value; } }

    public static event Action OnReset;

    void Awake()
    {
        // Using the singleton pattern, ensure only one instance of GameManager exists.
        if (Instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ResetLevel()
    {
        OnReset?.Invoke();
    }
}
