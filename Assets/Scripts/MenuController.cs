using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class MenuController : MonoBehaviour {
    [Header("Settings Page")]
    public Slider m_volumeSlider;
    public Toggle m_scrollToggle;
    public Toggle m_indicatorsToggle;
    public Toggle m_hardModeToggle;
    [Header("Levels Page")]
    public GameObject[] m_levelButtons = new GameObject[10];
    [Header("Stats Page")]
    public TMP_Text m_deathText;
    public TMP_Text m_timeText;
    public TMP_Text m_flipsText;
    public TMP_Text m_distanceText;

    GameManager m_gameManager;
    Vector3 m_slideTarget;
    bool m_sliding;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        m_gameManager = GameManager.TheInstance;

        // sync settings controls to actual values
        m_volumeSlider.value = m_gameManager.m_volumeScale;
        m_scrollToggle.isOn = m_gameManager.m_scrollBG;
        m_indicatorsToggle.isOn = m_gameManager.m_flipIndicators;
        m_hardModeToggle.isOn = m_gameManager.m_hardMode;

        // update global stats
        m_deathText.text = "Total Deaths: " + m_gameManager.GetTotalDeaths();
        m_timeText.text = "Total Playtime: " + TimeSpan.FromSeconds(m_gameManager.m_totalPlaytime).ToString("mm':'ss");
        m_flipsText.text = "Flips Performed: " + m_gameManager.m_totalFlips;
        m_distanceText.text = "Distance Fallen: " + (int) m_gameManager.m_totalDistance + "m";

        // update level buttons
        for (int i = 0; i < 10; i++) {
            Level level = m_gameManager.m_levels[i];
            GameObject button = m_levelButtons[i];
            TMP_Text[] texts = button.GetComponentsInChildren<TMP_Text>();
            if (level.completed) {
                // completed levels show deaths+time and are dark blue or green
                texts[1].text = "Deaths: " + level.deaths;
                texts[2].text = "Time: " +TimeSpan.FromSeconds(level.time).ToString("mm':'ss");
                Color newColor = level.bonus ? new Color(0.5f, 0.9f, 0.36f, 0.7f) : new Color(0.17f, 0.7f, 0.9f, 0.7f);
                button.GetComponent<Image>().color = newColor;
            } else if (i == 0 || m_gameManager.m_levels[i-1].completed) {
                // the next available level shows only deaths and is light blue
                if (level.deaths > 0) texts[1].text = "Deaths: " + level.deaths;
                button.GetComponent<Image>().color = new Color(0.6f, 0.9f, 1f, 0.7f);
            } else {
                // locked levels show nothing and are gray
                button.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f, 0.7f);
            }
        }

        // if you're exiting from a level, start on the level-select screen
        if (m_gameManager.m_totalPlaytime > 0) {
            transform.position = new Vector3(-18, 0, 0);
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.D) && Input.GetKey(KeyCode.LeftShift))
            m_gameManager.m_debugMode = !m_gameManager.m_debugMode;

        if (m_sliding) {
            if (transform.position == m_slideTarget) m_sliding = false;
            else transform.position = Vector2.MoveTowards(transform.position, m_slideTarget, 40*Time.deltaTime);
        }
    }

    // start up the slide animation, in the specified direction
    public void Slide(bool left) {
        if (!m_sliding) {
            m_sliding = true;
            m_slideTarget = transform.position + new Vector3(left ? 18 : -18, 0, 0);
        }
    }

    // enter a level if it's unlocked
    public void EnterLevel(int index) {
        if (index == 0 || m_gameManager.m_levels[index-1].completed || m_gameManager.m_debugMode) {
            // TODO: play sound
            m_gameManager.LoadLevel(index);
        } else {
            // TODO: play different sound
            Debug.Log("Level is locked!");
        }
    }

    public void UpdateVolume(System.Single newVolume) {
        m_gameManager.m_volumeScale = newVolume;
    }

    public void ToggleScroll(bool newState) {
        m_gameManager.m_scrollBG = newState;
    }

    public void ToggleIndicators(bool newState) {
        m_gameManager.m_flipIndicators = newState;
    }

    public void ToggleHardMode(bool newState) {
        m_gameManager.m_hardMode = newState;
    }
}
