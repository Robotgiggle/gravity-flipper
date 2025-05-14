using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class MenuController : MonoBehaviour {
    [Header("Settings Page")]
    public Slider m_gameVolumeSlider;
    public Slider m_musicVolumeSlider;
    public Toggle m_scrollToggle;
    public Toggle m_indicatorsToggle;
    public Toggle m_turboModeToggle;
    [Header("Start Page")]
    public Image m_playerIcon;
    public Sprite m_bonusSprite;
    [Header("Levels Page")]
    public Transform m_mainButton;
    public Transform m_statsButton;
    public GameObject m_bonusButton;
    public GameObject[] m_levelButtons = new GameObject[10];
    [Header("Stats Page")]
    public TMP_Text m_deathText;
    public TMP_Text m_timeText;
    public TMP_Text m_flipsText;
    public TMP_Text m_distanceText;

    GameManager m_gameManager;
    MusicController m_musicController;
    Vector3 m_slideTarget;
    bool m_sliding;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        m_gameManager = GameManager.TheInstance;
        m_musicController = GameObject.FindWithTag("Music").GetComponent<MusicController>();

        // change start icon if bonus level completed
        if (m_gameManager.GetTotalBonuses() == 11)
            m_playerIcon.sprite = m_bonusSprite;

        // sync settings controls to actual values
        m_gameVolumeSlider.value = m_gameManager.m_volumeScale;
        m_musicVolumeSlider.value = m_musicController.Volume;
        m_scrollToggle.isOn = m_gameManager.m_scrollBG;
        m_indicatorsToggle.isOn = m_gameManager.m_flipIndicators;
        m_turboModeToggle.isOn = m_gameManager.m_turboMode;

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

        // update button positions for bonus level
        if (m_gameManager.m_bonusLvlOpen) {
            m_mainButton.localPosition = new Vector3(-170, -116, 0);
            m_statsButton.localPosition = new Vector3(170, -116, 0);
            m_bonusButton.GetComponent<Image>().color = new Color(0.39f, 0.7f, 0.28f);
            m_bonusButton.GetComponentInChildren<TMP_Text>().color = new Color(0.2f, 0.2f, 0.2f);
            m_bonusButton.GetComponent<Button>().interactable = true;
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

        if (m_gameManager.GetTotalBonuses() == 10 && !m_gameManager.m_bonusLvlOpen) {
            m_mainButton.Translate(Vector3.left * 0.7f * Time.deltaTime);
            m_statsButton.Translate(Vector3.right * 0.7f * Time.deltaTime);
            Image bonusImg = m_bonusButton.GetComponent<Image>();
            TMP_Text bonusTxt = m_bonusButton.GetComponentInChildren<TMP_Text>();
            if (bonusImg.color.a < 1 && m_mainButton.localPosition.x <= -145) {
                Color oldB = bonusImg.color;
                Color oldT = bonusTxt.color;
                bonusImg.color = new Color(oldB.r, oldB.g, oldB.b, (oldB.a + Time.deltaTime));
                bonusTxt.color = new Color(oldT.r, oldT.g, oldT.b, (oldT.a + Time.deltaTime));
                m_bonusButton.GetComponent<Button>().interactable = true;
            }
            if (m_mainButton.localPosition.x <= -170) m_gameManager.m_bonusLvlOpen = true;
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

    public void UpdateGameVolume(System.Single newVolume) {
        m_gameManager.m_volumeScale = newVolume;
    }

    public void UpdateMusicVolume(System.Single newVolume) {
        m_musicController.Volume = newVolume;
    }

    public void ToggleScroll(bool newState) {
        m_gameManager.m_scrollBG = newState;
    }

    public void ToggleIndicators(bool newState) {
        m_gameManager.m_flipIndicators = newState;
    }

    public void ToggleTurboMode(bool newState) {
        m_gameManager.m_turboMode = newState;
    }
}
