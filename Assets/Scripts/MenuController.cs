using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class MenuController : MonoBehaviour {
    public TMP_Text m_deathText;
    public TMP_Text m_timeText;
    public string m_firstScene;

    GameManager m_gameManager;
    GameObject[] m_levelButtons = new GameObject[10];
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        m_gameManager = GameManager.TheInstance;
        for (int i = 0; i < 10; i++) {
            m_levelButtons[i] = GameObject.Find("LevelButton" + i);
        }

        // update global stats
        m_deathText.text = "Deaths: " + m_gameManager.TotalDeaths();
        m_timeText.text = "Time: " + TimeSpan.FromSeconds(m_gameManager.TotalTime()).ToString("mm':'ss");

        // update level buttons
        for (int i = 0; i < 10; i++) {
            Level level = m_gameManager.m_levels[i];
            GameObject button = m_levelButtons[i];
            TMP_Text[] texts = button.GetComponentsInChildren<TMP_Text>();
            if (level.completed) {
                texts[1].text = "Deaths: " + level.deaths;
                texts[2].text = "Time: " +TimeSpan.FromSeconds(level.time).ToString("mm':'ss");
                Color newColor = level.bonus ? new Color(0.55f, 0.95f, 0.4f) : new Color(0.17f, 0.8f, 0.9f);
                button.GetComponent<Image>().color = newColor;
            } else if (i == 0 || m_gameManager.m_levels[i-1].completed) {
                if (level.deaths > 0) texts[1].text = "Deaths: " + level.deaths;
                button.GetComponent<Image>().color = new Color(0.6f, 0.9f, 1f);
            } else {
                button.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
            }
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

    public void EnterLevel(int index) {
        if (index == 0 || m_gameManager.m_levels[index-1].completed) {
            // TODO: play sound
            m_gameManager.LoadLevel(index);
        } else {
            // TODO: play different sound
            Debug.Log("Level is locked!");
        }
    }
}
