using UnityEngine;

public class ToggleLaserController : MonoBehaviour {
    public bool m_initialState;
    
    GameManager m_gameManager;
    GameObject m_laserObj;
    bool m_active;
    
    void Start() {
        m_gameManager = GameManager.TheInstance;
        m_laserObj = transform.GetChild(0).gameObject;
        m_laserObj.SetActive(m_initialState);
        m_active = m_initialState;

        ToggleSwitchController.m_toggleEvent.AddListener(Toggle);
        m_gameManager.m_resetLevelEvent.AddListener(Reset);
    }

    void Toggle() {
        m_active = !m_active;
        m_laserObj.SetActive(m_active);
    }

    void Reset() {
        m_active = m_initialState;
        m_laserObj.SetActive(m_initialState);
    }
}
