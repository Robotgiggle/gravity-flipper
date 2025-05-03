using UnityEngine;

public class BossFightManager : MonoBehaviour {
    public GameObject m_phase2Floor;
    public GameObject m_phase2Seal;
    public GameObject m_phase3Floor;
    public GameObject m_phase3Seal;
    
    void Start() {
        
    }

    void Update() {
        
    }

    public void StartPhase(int phase) {
        Debug.Log("starting phase " + phase);
        transform.position = new Vector3(0, (phase-1)*31, 0);
        if (phase == 2) {
            m_phase2Floor.SetActive(true);
            // move seal 2 into place
        } else if (phase == 3) {
            m_phase3Floor.SetActive(true);
            // move seal 3 into place
        }
    }
}
