using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public struct Level {
    public string scene;
    public int deaths;
    public float time;
    public bool bonus;
}

public class GameManager : MonoBehaviour {
    public Level[] m_levels = new Level[10];
    public bool m_holdingBonus;
    private int m_currentLevel;
    
    // static instance access
    private static GameObject theObject;
    public static GameManager TheInstance {
        get {
            if (theObject == null) {
                theObject = new GameObject();
                theObject.name = "GameManager";
                theObject.AddComponent<GameManager>();
            }
            return theObject.GetComponent<GameManager>();
        }
    }

    void Awake() {
        // make sure there's only ever one object
        if (theObject == null) theObject = gameObject;
        else if (theObject != gameObject) Destroy(gameObject);
        // persist across scenes
        DontDestroyOnLoad(gameObject);
        // load level scene names
        for (int i = 0; i < 3; i++) {
            m_levels[i].scene = "Level" + (i+1);
        }
        // link scene-loaded event to method
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        m_holdingBonus = false;
    }

    IEnumerator LoadSceneWithDelay(string name, float delay) {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(name);
        asyncOp.allowSceneActivation = false;
        yield return new WaitForSeconds(delay);
        asyncOp.allowSceneActivation = true;
    }

    public void Reset() {
        for (int i = 0; i < 10; i++) {
            m_levels[i].deaths = 0;
            m_levels[i].time = 0;
            m_levels[i].bonus = false;
        }
    }

    public void AddDeath() {
        m_levels[m_currentLevel].deaths++;
    }

    public void LoadNextLevel(float delay) {
        // store finish time
        float finishTime = Time.timeSinceLevelLoad;
        if (finishTime < m_levels[m_currentLevel].time) m_levels[m_currentLevel].time = finishTime;
        // store bonus if collected
        if (m_holdingBonus) m_levels[m_currentLevel].bonus = true;
        //load next level
        m_currentLevel++;
        StartCoroutine(LoadSceneWithDelay(m_levels[m_currentLevel].scene, delay));
    }

    public void LoadLevel(int index) {
        LoadSceneWithDelay(m_levels[index].scene, 0);
        m_currentLevel = index;
    }

    public void LoadMenu() {
        LoadSceneWithDelay("Menu", 0);
        m_currentLevel = -1;
    }

    public bool BonusCollected() {
        return m_levels[m_currentLevel].bonus;
    }

    public int TotalDeaths() {
        int output = 0;
        foreach (Level level in m_levels) {
            output += level.deaths;
        }
        return output;
    }

    public float TotalTime() {
        float output = 0;
        foreach (Level level in m_levels) {
            output += level.time;
        }
        return output;
    }

    public int TotalBonuses() {
        int output = 0;
        foreach (Level level in m_levels) {
            if (level.bonus) output++;
        }
        return output;
    }
}
