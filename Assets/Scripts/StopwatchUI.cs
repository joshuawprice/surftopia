using UnityEngine;
using TMPro;
using System;
using UnityEditor;

public class StopwatchUI : MonoBehaviour
{
    private float currentTime = 0f;
    private bool isRunning;

    private void Start()
    {
        isRunning = true;
        GameManager.Instance.OnResetLevel += ResetStopwatch;
        GameManager.Instance.OnFinishLevel += SaveStopwatch;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnResetLevel -= ResetStopwatch;
        GameManager.Instance.OnFinishLevel -= SaveStopwatch;
    }

    private void Update()
    {
        if (isRunning)
        {
            currentTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    private void StartStopwatch()
    {
        isRunning = true;
    }

    private void StopStopwatch()
    {
        isRunning = false;
    }

    private void ResetStopwatch()
    {
        currentTime = 0f;
        UpdateTimerUI();
    }

    private void SaveStopwatch()
    {
        GameManager.Instance.AddScore(currentTime);
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        int milliseconds = Mathf.FloorToInt((currentTime * 1000) % 1000);

        gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }
}
