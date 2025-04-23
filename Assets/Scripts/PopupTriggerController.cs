using UnityEngine;

public class PopupTriggerController : MonoBehaviour {
    public TutorialController m_tutorialController;
    public int m_index;

    GameManager m_gameManager;
    bool m_activated;

    void Start() {
        m_gameManager = GameManager.TheInstance;
        m_gameManager.m_resetLevelEvent.AddListener(() => m_activated = false);
    }

    void OnTriggerEnter2D() {
        if (!m_activated) {
            m_tutorialController.ShowPopup(m_index);
            m_activated = true;
        }
    }
}
