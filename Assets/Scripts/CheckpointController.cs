using UnityEngine;
using System.Collections;

public class CheckpointController : MonoBehaviour {
    public BossSwitchController[] m_switchesToSave;
    public Transform[] m_seals;
    public Transform m_viewport;
    public GameObject m_floor;
    public BossController m_boss;
    public float m_leftEdge;
    public float m_rightEdge;

    bool m_activated;

    void OnTriggerEnter2D(Collider2D collider) {
        if (!m_activated && collider.CompareTag("Player")) {
            // prevent switches in previous room from resetting on death
            foreach (BossSwitchController sw in m_switchesToSave) sw.m_saved = true;
            // activate next phase of boss
            m_boss.m_phase = -(m_boss.m_phase) + 1;
            // move camera and UI up to next room
            m_viewport.position = transform.position + new Vector3(0, 9.5f, 0);
            // close the tunnel up from the previous room
            StartCoroutine(CloseSeals());
            m_floor.SetActive(true);
            // make sure the player lands on safe ground
            NudgePlayer(collider.transform);
            // make sure this only triggers once
            m_activated = true;
        }
    }

    void NudgePlayer(Transform trans) {
        float newX = trans.position.x;
        if (trans.position.x < m_leftEdge) newX = m_leftEdge;
        if (trans.position.x > m_rightEdge) newX = m_rightEdge;
        trans.position = new Vector3(newX, transform.position.y - 1);
    }

    IEnumerator CloseSeals() {
        for (int i = 0; i < 8; i++) {
            yield return new WaitForSeconds(0.07f);
            m_seals[0].Translate(new Vector3(0.5f, 0, 0));
            m_seals[1].Translate(new Vector3(-0.5f, 0, 0));
        }
    }
}
