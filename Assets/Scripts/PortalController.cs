using UnityEngine;

public class PortalController : MonoBehaviour {
    public GameObject m_pair;

    PortalController m_pairController;
    float m_cooldown;

    void Start() {
        m_pairController = m_pair.GetComponent<PortalController>();
    }

    void Update() {
        if (m_cooldown > 0) m_cooldown -= Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (m_cooldown <= 0) {
            Vector3 offset = (m_pair.transform.position - transform.position) + (0.7f * m_pair.transform.right);
            collider.transform.Translate(offset);
            m_pairController.m_cooldown = 0.2f;
        }
    }
}
