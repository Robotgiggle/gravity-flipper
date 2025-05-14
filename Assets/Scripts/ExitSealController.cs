using UnityEngine;

public class ExitSealController : MonoBehaviour {
    public Transform m_topSeal;
    public Transform m_bottomSeal;
    public GameObject m_sealHitbox;
    public DoorController m_doorController;

    GameManager m_gameManager;
    bool m_unsealed;
    
    void Start() {
        m_gameManager = GameManager.TheInstance;
        m_gameManager.m_resetLevelEvent.AddListener(Reset);
    }

    void Update() {
        if (m_doorController.m_open && !m_unsealed) {
            m_topSeal.Translate(Vector3.right * Time.deltaTime * 2);
            m_bottomSeal.Translate(Vector3.right * Time.deltaTime * 2);
            if (m_topSeal.localPosition.y >= 2.5f) {
                m_topSeal.localPosition = new Vector3(0, 2.5f, 0);
                m_bottomSeal.localPosition = new Vector3(0, -2.5f, 0);
                m_sealHitbox.SetActive(false);
                m_unsealed = true;
            }
        }
    }

    void Reset() {
        m_topSeal.localPosition = Vector3.up;
        m_bottomSeal.localPosition = Vector3.down;
        m_sealHitbox.SetActive(true);
        m_unsealed = false;
    }
}
