using UnityEngine;

public class CheckpointController : MonoBehaviour {
    public BossSwitchController[] m_switchesToSave;
    public BossFightManager m_fightManager;
    public BossController m_boss;
    public int m_phase;

    bool m_activated;

    void OnTriggerEnter2D() {
        if (!m_activated) {
            foreach (BossSwitchController sw in m_switchesToSave) sw.m_saved = true;
            m_boss.m_phase = (m_boss.m_phase * -1) + 1;
            m_fightManager.StartPhase(m_phase);
            m_activated = true;
        }
    }
}
