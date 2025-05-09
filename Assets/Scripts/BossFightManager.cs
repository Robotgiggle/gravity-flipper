using UnityEngine;
using System.Collections;

public class BossFightManager : MonoBehaviour {
    public GameObject m_phase2Floor;
    public Transform[] m_phase2Seals;
    public GameObject m_phase3Floor;
    public Transform[] m_phase3Seals;

    public void StartPhase(int phase) {
        transform.position = new Vector3(0, (phase-1)*31, 0);
        StartCoroutine(CloseSeals(phase));
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
