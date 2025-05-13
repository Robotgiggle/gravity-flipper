using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BossController : MonoBehaviour {
    public GameObject m_player;
    public List<BossSwitchController> m_phase1Switches;
    public List<BossSwitchController> m_phase2Switches;
    public List<BossSwitchController> m_phase3Switches;
    public Sprite[] m_eyelidSprites;
    public int m_phase = 1;

    GameManager m_gameManager;
    MusicController m_musicController;
    ParticleBurstController m_burst;
    SpriteRenderer m_eyelidRenderer;
    Collider2D m_collider;
    float m_animTimer;
    bool m_pulseOut;
    
    void Start() {
        m_gameManager = GameManager.TheInstance;
        m_musicController = GameObject.FindWithTag("Music")?.GetComponent<MusicController>();
        m_burst = gameObject.GetComponentInChildren<ParticleBurstController>();
        m_eyelidRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        m_collider = gameObject.GetComponent<Collider2D>();
        m_gameManager.m_resetLevelEvent.AddListener(Reset);
    }

    void Update() {
        // pulsate the body, this happens regardless of phase
        if (m_pulseOut) {
            transform.localScale += new Vector3(0.15f, 0.15f, 0) * Time.deltaTime;
            if (transform.localScale.x >= 6.45f) m_pulseOut = false;
        } else {
            transform.localScale -= new Vector3(0.15f, 0.15f, 0) * Time.deltaTime;
            if (transform.localScale.x <= 6.35f) m_pulseOut = true;
        }
        // negative phase = "phase N has been defeated"
        if (m_phase < 0) {
            if (m_animTimer > 0) {
                // wait for damage animation to finish
                m_animTimer -= Time.deltaTime;
            } else {
                // move up to the next room
                if ((m_phase == -1 && transform.position.y < 20) || (m_phase == -2 && transform.position.y < 51)) {
                    m_collider.enabled = false;
                    transform.Translate(0, 8 * Time.deltaTime, 0);
                    while (PlayerTooClose()) transform.Translate(0, 2 * Time.deltaTime, 0);
                } else if (m_phase == -1 && transform.position.y < 39) {
                    transform.position = new Vector3(0, 39, 0);
                    m_collider.enabled = true;
                } else if (m_phase == -2 && transform.position.y < 70) {
                    transform.position = new Vector3(0, 70, 0);
                    m_collider.enabled = true;
                }
            }
        // positive phase = "phase N is active"
        } else {
            // check switches, trigger animation if necessary
            if (m_phase1Switches.All(sw => sw.m_active) && m_phase == 1) {
                m_animTimer = 0.5f;
                m_burst.BurstOffset(new Vector3(-4.8f, -1f, 0));
                m_eyelidRenderer.enabled = true;
                m_phase1Switches.ForEach(sw => sw.Vanish());
                m_phase = -1;
            } else if (m_phase2Switches.All(sw => sw.m_active) && m_phase == 2) {
                m_animTimer = 0.5f;
                m_burst.BurstOffset(new Vector3(4.8f, -1f, 0));
                m_eyelidRenderer.sprite = m_eyelidSprites[1];
                m_phase2Switches.ForEach(sw => sw.Vanish());
                m_phase = -2;
            } else if (m_phase3Switches.All(sw => sw.m_active) && m_phase == 3) {
                m_burst.Burst();
                m_eyelidRenderer.sprite = m_eyelidSprites[2];
                m_phase3Switches.ForEach(sw => sw.Vanish());
                m_phase = -3;
                StartCoroutine(DeathAnim());
            }
        }
    }

    void Reset() {
        if (m_phase < 0) m_phase *= -1;
        if (m_phase == 1) {
            m_eyelidRenderer.enabled = false;
            transform.position = new Vector3(0, 8, 0);
        } else if (m_phase == 2) {
            m_eyelidRenderer.sprite = m_eyelidSprites[0];
            transform.position = new Vector3(0, 39, 0);
        } else if (m_phase == 3) {
            m_eyelidRenderer.sprite = m_eyelidSprites[1];
            transform.position = new Vector3(0, 70, 0);
            m_musicController?.ChangeMusicTo("Boss", false);
        }
    }

    bool PlayerTooClose() {
        Vector3 playerPos = m_player.transform.position;
        return Mathf.Abs(transform.position.x - playerPos.x) <= 5 
            && transform.position.y - playerPos.y <= 7f;
    }

    IEnumerator DeathAnim() {
        yield return new WaitForSeconds(0.5f);
        m_burst.m_initialScale = 0.9f;
        m_burst.m_color = new Color(0.52f, 0.16f, 0.39f);
        for (int i = 0; i < 8; i++) {
            m_burst.BurstOffset(new Vector3(Random.Range(-6f, 6f), Random.Range(-5f, 4f), 0));
            if (i == 5) m_musicController?.ChangeMusicTo("Level", true);
            yield return new WaitForSeconds(0.3f);
        }
        m_burst.m_burstForce = 4.5f;
        m_burst.m_initialScale = 1.2f;
        m_burst.BurstPos(new Vector3(0, 68.5f, 0));
        transform.position = new Vector3(0, 100, 0);
    }
}
