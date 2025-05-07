using UnityEngine;

public class BonusController : MonoBehaviour {
    public Sprite m_graySprite;
    
    GameManager m_gameManager;
    ParticleFieldController m_field;
    ParticleBurstController m_burst;
    SpriteRenderer m_renderer;

    void Start() {
        m_gameManager = GameManager.TheInstance;
        m_field = gameObject.GetComponentInChildren<ParticleFieldController>();
        m_burst = gameObject.GetComponentInChildren<ParticleBurstController>();
        m_renderer = gameObject.GetComponent<SpriteRenderer>();
        if (m_gameManager.BonusCollected()) {
            // if already collected, turn self and particles gray
            m_renderer.color = new Color(1, 1, 1, 0.8f);
            m_renderer.sprite = m_graySprite;
            m_field.m_color = new Color(0.75f, 0.75f, 0.75f);
            m_burst.m_color = new Color(0.75f, 0.75f, 0.75f);
        }
        // respawn the bonus when the level resets
        m_gameManager.m_resetLevelEvent.AddListener(() => gameObject.SetActive(true));
    }
}
