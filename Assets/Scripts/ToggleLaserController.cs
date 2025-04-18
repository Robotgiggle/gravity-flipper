using UnityEngine;

public class ToggleLaserController : MonoBehaviour {
    public GameObject m_laser;
    public GameObject m_particles;
    public bool m_initialState;
    
    GameManager m_gameManager;
    bool m_active;
    
    void Start() {
        m_gameManager = GameManager.TheInstance;
        m_laser.SetActive(m_initialState);
        m_particles.SetActive(m_initialState);
        m_active = m_initialState;

        ToggleSwitchController.m_toggleEvent.AddListener(Toggle);
        m_gameManager.m_resetLevelEvent.AddListener(Reset);
    }

    void Toggle() {
        m_active = !m_active;
        m_laser.SetActive(m_active);
        m_particles.SetActive(m_active);
    }

    void Reset() {
        m_active = m_initialState;
        m_laser.SetActive(m_initialState);
        m_particles.SetActive(m_initialState);
    }
}
