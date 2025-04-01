using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class MenuController : MonoBehaviour {
    public GameObject[] m_levelButtons = new GameObject[10];
    public TMP_Text m_deathText;
    public TMP_Text m_timeText;
    public TMP_Text m_flipsText;
    public TMP_Text m_distanceText;
    public string m_firstScene;

    GameManager m_gameManager;
    Vector3 m_slideTarget;
    bool m_sliding;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        m_gameManager = GameManager.TheInstance;

        // update global stats
        m_deathText.text = "Total Deaths: " + m_gameManager.GetTotalDeaths();
        m_timeText.text = "Total Playtime: " + TimeSpan.FromSeconds(m_gameManager.GetTotalTime()).ToString("mm':'ss");
        m_flipsText.text = "Flips Performed: " + m_gameManager.m_totalFlips;
        m_distanceText.text = "Distance Fallen: " + (int) m_gameManager.m_totalDistance + "m";

        // update level buttons
        for (int i = 0; i < 10; i++) {
            Level level = m_gameManager.m_levels[i];
            GameObject button = m_levelButtons[i];
            TMP_Text[] texts = button.GetComponentsInChildren<TMP_Text>();
            if (level.completed) {
                texts[1].text = "Deaths: " + level.deaths;
                texts[2].text = "Time: " +TimeSpan.FromSeconds(level.time).ToString("mm':'ss");
                Color newColor = level.bonus ? new Color(0.55f, 0.95f, 0.4f, 0.7f) : new Color(0.17f, 0.8f, 0.9f, 0.7f);
                button.GetComponent<Image>().color = newColor;
            } else if (i == 0 || m_gameManager.m_levels[i-1].completed) {
                if (level.deaths > 0) texts[1].text = "Deaths: " + level.deaths;
                button.GetComponent<Image>().color = new Color(0.6f, 0.9f, 1f, 0.7f);
            } else {
                button.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f, 0.7f);
            }
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) Slide(false);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) Slide(true);

        if (m_sliding) {
            if (transform.position == m_slideTarget) m_sliding = false;
            else transform.position = Vector2.MoveTowards(transform.position, m_slideTarget, 35*Time.deltaTime);
        }
    }

    public void StartGame() {
        m_gameManager.Reset();
        SceneManager.LoadScene(m_firstScene);
    }

    public void Slide(bool left) {
        if (!m_sliding) {
            m_sliding = true;
            m_slideTarget = transform.position + new Vector3(left ? 18 : -18, 0, 0);
        }
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
