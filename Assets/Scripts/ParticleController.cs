using UnityEngine;

public class ParticleController : MonoBehaviour {
    public Vector3 m_motion;
    public float m_spinRate;
    public float m_shrinkFactor;

    void Start() {
        
    }

    void Update() {
        transform.Rotate(0, 0, m_spinRate * Time.deltaTime);
        transform.Translate(m_motion * Time.deltaTime, Space.World);
        if (m_shrinkFactor != 0) {
            if (transform.localScale.x > 0.01f) transform.localScale *= m_shrinkFactor;
            else Destroy(gameObject);
        }
    }
}
