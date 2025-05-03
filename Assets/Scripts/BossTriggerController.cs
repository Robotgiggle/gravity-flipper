using UnityEngine;

public class BossTriggerController : MonoBehaviour {
    public BossFightManager m_fightManager;
    public int m_phase;

    GameManager m_gameManager;
    bool m_activated;

    void Start() {
        m_gameManager = GameManager.TheInstance;
        m_gameManager.m_resetLevelEvent.AddListener(() => m_activated = false);
    }

    void OnTriggerEnter2D() {
        if (!m_activated) {
            m_fightManager.StartPhase(m_phase);
            m_activated = true;
        }
    }
}
