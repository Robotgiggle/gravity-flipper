using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class MenuController : MonoBehaviour
{
    public GameObject m_startDisplay;
    public GameObject m_endDisplay;
    public TMP_Text m_deathText;
    public TMP_Text m_timeText;
    public string m_firstScene;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        // only show the info if you got here by beating the game
        if (ScoreTracker.timer != 0) {
            m_startDisplay.SetActive(false);
            m_endDisplay.SetActive(true);
            m_deathText.text = "Deaths: " + ScoreTracker.deaths;
            m_timeText.text = "Time: " + TimeSpan.FromSeconds(ScoreTracker.timer).ToString("mm':'ss");
        }
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void StartGame() {
        ScoreTracker.Reset();
        SceneManager.LoadScene(m_firstScene);
    }
}
