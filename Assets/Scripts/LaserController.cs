using UnityEngine;
using System.Collections;

public class LaserController : MonoBehaviour {
    public GameObject m_laser;
    public GameObject m_particles;
    public SpriteRenderer m_teleRenderer;
    public float m_uptime;
    public float m_downtime;
    public float m_offset;
    public float m_telegraph;
    
    GameManager m_gameManager;
    Coroutine m_firingCoroutine;
    
    void Start() {
        m_gameManager = GameManager.TheInstance;
        m_laser.SetActive(false);
        m_gameManager.m_resetLevelEvent.AddListener(RestartFiring);

        if (m_gameManager.m_hardMode) {
            m_uptime /= 1.3f;
            m_downtime /= 1.3f;
            m_offset /= 1.3f;
            m_telegraph /= 1.3f;
        }
        
        m_firingCoroutine = StartCoroutine(FireLaser());
    }

    void SetTelegraphOpacity(float val) {
        Color col = m_teleRenderer.color;
        col.a = val;
        m_teleRenderer.color = col;
    }

    // this is used to reset the timing when the level resets
    void RestartFiring() {
        StopCoroutine(m_firingCoroutine);
        m_laser.SetActive(false);
        m_particles.SetActive(false);
        SetTelegraphOpacity(0);
        m_firingCoroutine = StartCoroutine(FireLaser());
    }

    IEnumerator FireLaser() {
        // offset initial firing to allow for interesting timing
        yield return new WaitForSeconds(m_offset);

        while (true) {
            // two-phase telegraph over the specified interval
            SetTelegraphOpacity(0.15f);
            yield return new WaitForSeconds(m_telegraph/2);
            SetTelegraphOpacity(0.45f);
            yield return new WaitForSeconds(m_telegraph/2);
            
            // activate laser for specified interval
            SetTelegraphOpacity(0);
            m_laser.SetActive(true);
            m_particles.SetActive(true);
            yield return new WaitForSeconds(m_uptime);

            // deactivate laser for specified interval
            m_laser.SetActive(false);
            m_particles.SetActive(false);
            yield return new WaitForSeconds(m_downtime - m_telegraph);
        }
    }
}
