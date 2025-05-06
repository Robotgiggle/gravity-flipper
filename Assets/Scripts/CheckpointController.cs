using UnityEngine;

public class CheckpointController : MonoBehaviour {
    public BossFightManager m_fightManager;
    public int m_phase;

    bool m_activated;

    void OnTriggerEnter2D() {
        if (!m_activated) {
            m_fightManager.StartPhase(m_phase);
            m_activated = true;
        }
    }
}
