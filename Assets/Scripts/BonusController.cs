using UnityEngine;

public class BonusController : MonoBehaviour {
    public int index;
    
    GameManager m_gameManager;
    SpriteRenderer m_renderer;

    void Start() {
        m_gameManager = GameManager.TheInstance;
        m_renderer = gameObject.GetComponent<SpriteRenderer>();
        if (m_gameManager.BonusCollected()) {
            // if already collected, turn gray and hide particles
            m_renderer.color = new Color(0.65f, 0.65f, 0.65f, 0.8f);
            transform.GetChild(0).gameObject.SetActive(false);
        }
        // respawn the bonus when the level resets
        m_gameManager.m_resetLevelEvent.AddListener(() => gameObject.SetActive(true));
    }
}
