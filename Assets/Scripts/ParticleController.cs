using UnityEngine;

public class ParticleController : MonoBehaviour {
    public Vector3 m_motion;
    public float m_fadeRate;
    public float m_spinRate;
    public float m_shrinkFactor;

    SpriteRenderer m_renderer;

    void Start() {
        m_renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update() {
        transform.Rotate(0, 0, m_spinRate * Time.deltaTime);
        transform.Translate(m_motion * Time.deltaTime, Space.World);
        if (m_shrinkFactor != 0) {
            if (transform.localScale.x > 0.01f) transform.localScale *= m_shrinkFactor;
            else Destroy(gameObject);
        }
        if (m_fadeRate != 0) {
            if (m_renderer.color.a > 0) {
                Color old = m_renderer.color;
                m_renderer.color = new Color(old.r, old.g, old.b, (old.a - m_fadeRate * Time.deltaTime));
            } else Destroy(gameObject);
        }
    }
}
