using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BossController : MonoBehaviour {
    public GameObject m_player;
    public List<BossSwitchController> m_phase1Switches;
    public List<BossSwitchController> m_phase2Switches;
    public List<BossSwitchController> m_phase3Switches;

    GameManager m_gameManager;
    int m_phase = 1;
    
    void Start() {
        m_gameManager = GameManager.TheInstance;
        m_gameManager.m_resetLevelEvent.AddListener(Reset);
    }

    void Update() {
        // check for phase transition
        if (m_phase1Switches.All(sw => sw.m_active) && m_phase == 1) {
            Debug.Log("moving to phase 2");
            m_phase1Switches.ForEach(sw => sw.StartCoroutine(sw.Vanish()));
            m_phase = 2;
        } else if (m_phase2Switches.All(sw => sw.m_active) && m_phase == 2) {
            Debug.Log("moving to phase 3");
            m_phase2Switches.ForEach(sw => sw.StartCoroutine(sw.Vanish()));
            m_phase = 3;
        } else if (m_phase3Switches.All(sw => sw.m_active) && m_phase == 3) {
            Debug.Log("boss defeated");
            m_phase = 0;
            m_phase3Switches.ForEach(sw => sw.StartCoroutine(sw.Vanish()));
            StartCoroutine(DeathAnim());
        }
        // move up to next room if necessary
        if ((m_phase == 2 && transform.position.y < 39) || (m_phase == 3 && transform.position.y < 70)) {
            transform.Translate(0, 8 * Time.deltaTime, 0);
        }
        // attack the player
        //TODO
    }

    void Reset() {
        transform.position = new Vector3(0, 8, 0);
        m_phase = 1;
    }

    IEnumerator DeathAnim() {
        // TODO: particle bursts
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
