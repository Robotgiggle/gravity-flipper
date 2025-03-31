using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class MenuController : MonoBehaviour {
    public TMP_Text m_deathText;
    public TMP_Text m_timeText;
    public string m_firstScene;

    GameManager m_gameManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        m_gameManager = GameManager.TheInstance;

        // only show the info if you got here by beating the game
        if (m_gameManager.TotalTime() != 0) {
            m_deathText.text = "Deaths: " + m_gameManager.TotalDeaths();
            m_timeText.text = "Time: " + TimeSpan.FromSeconds(m_gameManager.TotalTime()).ToString("mm':'ss");
        }
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void StartGame() {
        m_gameManager.Reset();
        SceneManager.LoadScene(m_firstScene);
    }
}
