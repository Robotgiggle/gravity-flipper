using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

[Serializable]
public class Level {
    public string scene;
    public int deaths;
    public float time = 9999;
    public bool completed;
    public bool bonus;
}

public class GameManager : MonoBehaviour {
    public Level[] m_levels = new Level[10];
    public UnityEvent m_resetLevelEvent;
    public bool m_holdingBonus;
    public bool m_inputsLocked;
    // stats
    public int m_totalFlips;
    public float m_totalDistance;
    public float m_totalPlaytime;
    // settings
    public float m_volumeScale = 1;
    public bool m_scrollBG = true;
    public bool m_flipIndicators = true;
    public bool m_hardMode = false;
    public bool m_debugMode = false;
    
    MusicController m_musicController;
    int m_currentLevel;
    
    // static instance access
    private static GameObject m_theObject;
    public static GameManager TheInstance {
        get {
            if (m_theObject == null) {
                m_theObject = new GameObject();
                m_theObject.name = "GameManager";
                m_theObject.AddComponent<GameManager>();
            }
            return m_theObject.GetComponent<GameManager>();
        }
    }

    void Awake() {
        // make sure there's only ever one object
        if (m_theObject == null) m_theObject = gameObject;
        else if (m_theObject != gameObject) Destroy(gameObject);
        // persist across scenes
        DontDestroyOnLoad(gameObject);
        // set up data
        m_resetLevelEvent = new UnityEvent();
        for (int i = 0; i < 10; i++) {
            m_levels[i] = new Level();
            m_levels[i].scene = "Level" + (i+1);
        }
        int index = SceneManager.GetActiveScene().buildIndex;
        if (index != 0) m_currentLevel = index - 1;
        // link scene-loaded event to method
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        m_holdingBonus = false;
        m_musicController = GameObject.FindWithTag("Music")?.GetComponent<MusicController>();
        if (scene.buildIndex == 0) m_musicController?.ChangeMusicTo("Menu", false);
        else if (scene.buildIndex == 10) m_musicController?.ChangeMusicTo("Boss", false);
        else m_musicController?.ChangeMusicTo("Level", false);
    }

    IEnumerator LoadSceneWithDelay(string name, float delay) {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(name);
        asyncOp.allowSceneActivation = false;
        yield return new WaitForSeconds(delay);
        asyncOp.allowSceneActivation = true;
    }

    public void ResetGame() {
        for (int i = 0; i < 10; i++) {
            m_levels[i].deaths = 0;
            m_levels[i].time = 9999;
            m_levels[i].completed = false;
            m_levels[i].bonus = false;
        }
        LoadMenu(false);
    }

    public void ResetLevel() {
        m_holdingBonus = false;
        m_resetLevelEvent.Invoke();
    }

    public void AddDeath() {
        m_levels[m_currentLevel].deaths++;
    }

    public void LoadNextLevel(float delay) {
        // store finish time
        float finishTime = Time.timeSinceLevelLoad;
        if (finishTime < m_levels[m_currentLevel].time) m_levels[m_currentLevel].time = finishTime;
        m_totalPlaytime += finishTime;
        // store bonus and level completion status
        if (m_holdingBonus) m_levels[m_currentLevel].bonus = true;
        m_levels[m_currentLevel].completed = true;
        //load next level
        if (m_currentLevel < 9) LoadLevel(m_currentLevel+1, delay);
        else LoadMenu(true, delay);
    }

    public void LoadLevel(int index, float delay = 0) {
        StartCoroutine(LoadSceneWithDelay(m_levels[index].scene, delay));
        m_currentLevel = index;
        //if (index == 9) m_musicController.ChangeMusicTo(2);
        //else m_musicController.ChangeMusicTo(1);
    }

    public void LoadMenu(bool fromLevel, float delay = 0) {
        if (fromLevel) m_totalPlaytime += Time.timeSinceLevelLoad;
        StartCoroutine(LoadSceneWithDelay("Menu", delay));
        m_currentLevel = -1;
    }

    public bool BonusCollected() {
        return m_levels[m_currentLevel].bonus;
    }

    public int GetTotalDeaths() {
        int output = 0;
        foreach (Level level in m_levels) {
            output += level.deaths;
        }
        return output;
    }

    public int GetTotalBonuses() {
        int output = 0;
        foreach (Level level in m_levels) {
            if (level.bonus) output++;
        }
        return output;
    }
}
