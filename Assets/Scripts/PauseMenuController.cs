using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenuController : MonoBehaviour {
    public GameObject m_pauseMenu;
    public Image m_darkness;
    
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

    public IEnumerator FadeDarkness(bool fadeIn) {
        float alpha = fadeIn ? 0 : 0.69f;
        for (int i = 0; i < 23; i++) {
            alpha += fadeIn ? 0.03f : -0.03f;
            m_darkness.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }

    public void Pause() {
        StartCoroutine(FadeDarkness(true));
        m_pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Unpause() {
        StartCoroutine(FadeDarkness(false));
        m_pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void ResetLevel() {
        Unpause();
        m_gameManager.AddDeath();
        m_gameManager.ResetLevel();
    }

    public void ExitLevel() {
        Time.timeScale = 1;
        m_gameManager.LoadMenu(true);
    }
}
