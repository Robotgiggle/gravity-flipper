using UnityEngine;

public class PauseMenuController : MonoBehaviour {
    public GameObject m_pauseMenu;
    
    GameManager m_gameManager;

    void Start() {
        m_gameManager = GameManager.TheInstance;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab) && !m_gameManager.m_inputsLocked) {
            if (m_pauseMenu.activeSelf) Unpause();
            else Pause();
        }
    }

    public void Pause() {
        m_pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Unpause() {
        m_pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void ResetLevel() {
        Unpause();
        m_gameManager.AddDeath();
        m_gameManager.ResetLevel();
    }

    public void ExitLevel() {
        Unpause();
        m_gameManager.LoadMenu(true);
    }
}
