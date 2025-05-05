using UnityEngine;
using System.Collections;

public class BossFightManager : MonoBehaviour {
    public GameObject m_phase2Floor;
    public Transform[] m_phase2Seals;
    public GameObject m_phase3Floor;
    public Transform[] m_phase3Seals;

    GameManager m_gameManager;
    Coroutine m_closeRoutine;

    void Start() {
        m_gameManager = GameManager.TheInstance;
        m_gameManager.m_resetLevelEvent.AddListener(Reset);
    }

    void Reset() {
        if (m_closeRoutine != null) StopCoroutine(m_closeRoutine);
        transform.position = new Vector3(0, 0, 0);
        m_phase2Floor.SetActive(false);
        m_phase2Seals[0].localPosition = new Vector3(-8.5f, 20.5f, 0);
        m_phase2Seals[1].localPosition = new Vector3(4.5f, 20.5f, 0);
        m_phase3Floor.SetActive(false);
        m_phase3Seals[0].localPosition = new Vector3(-8.5f, 51.5f, 0);
        m_phase3Seals[1].localPosition = new Vector3(4.5f, 51.5f, 0);
    }

    public void StartPhase(int phase) {
        Debug.Log("starting phase " + phase);
        transform.position = new Vector3(0, (phase-1)*31, 0);
        m_closeRoutine = StartCoroutine(CloseSeals(phase));
        if (phase == 2) m_phase2Floor.SetActive(true);
        else if (phase == 3) m_phase3Floor.SetActive(true);
    }

    IEnumerator CloseSeals(int phase) {
        for (int i = 0; i < 8; i++) {
             yield return new WaitForSeconds(0.07f);
            if (phase == 2) {
                m_phase2Seals[0].Translate(new Vector3(0.5f, 0, 0));
                m_phase2Seals[1].Translate(new Vector3(-0.5f, 0, 0));
            } else if (phase == 3) {
                m_phase3Seals[0].Translate(new Vector3(0.5f, 0, 0));
                m_phase3Seals[1].Translate(new Vector3(-0.5f, 0, 0));
            }
        }
    }
}
