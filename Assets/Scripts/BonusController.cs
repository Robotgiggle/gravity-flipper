using UnityEngine;

public class BonusController : MonoBehaviour {
    public int index;
    
    GameManager m_gameManager;
    SpriteRenderer m_renderer;

    void Start() {
        m_gameManager = GameManager.TheInstance;
        m_renderer = gameObject.GetComponent<SpriteRenderer>();
        if (m_gameManager.BonusCollected()) {
            m_renderer.color = new Color(0.65f, 0.65f, 0.65f, 0.8f);
        }
        // respawn the bonus when the level resets
        m_gameManager.m_resetLevelEvent.AddListener(() => gameObject.SetActive(true));
    }
}
