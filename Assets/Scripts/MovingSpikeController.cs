using UnityEngine;

public class MovingSpikeController : MonoBehaviour {
    Animator m_animator;
    
    void Start() {
        m_animator = gameObject.GetComponent<Animator>();
        GameManager.TheInstance.m_resetLevelEvent.AddListener(Reset);
    }

    void Reset() {
        m_animator.SetTrigger("Reset");
    }
}
