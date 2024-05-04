using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    void Start()
    {
        string scoreboard = "";
        foreach (var score in GameManager.Instance.Scores)
        {
            int minutes = Mathf.FloorToInt(score / 60f);
            int seconds = Mathf.FloorToInt(score % 60f);
            int milliseconds = Mathf.FloorToInt((score * 1000) % 1000);

             
            scoreboard += string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds) + "\n";
        }

        gameObject.GetComponent<TextMeshProUGUI>().text = scoreboard;
    }
}
