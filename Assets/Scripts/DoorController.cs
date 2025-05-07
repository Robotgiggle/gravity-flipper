using UnityEngine;

public class DoorController : MonoBehaviour {
    public AudioClip m_unlockSound;
    public AudioClip m_teleportSound;
    public bool m_open = false;

    GameManager m_gameManager;
    AudioSource m_audioSource;
    Animator m_animator;
    SwitchController[] m_switchCons;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        m_gameManager = GameManager.TheInstance;
        m_audioSource = gameObject.GetComponent<AudioSource>();
        m_animator = gameObject.GetComponent<Animator>();
        GameObject[] switches = GameObject.FindGameObjectsWithTag("Switch");
        m_switchCons = new SwitchController[switches.Length];
        for (int i = 0; i < switches.Length; i++) {
            m_switchCons[i] = switches[i].GetComponent<SwitchController>();
        }
        m_gameManager.m_resetLevelEvent.AddListener(Reset);
    }

    // when colliding with the player, move to the next level if the door is open
    void OnTriggerEnter2D(Collider2D coll) {
        if (m_open && coll.CompareTag("Player")) {
            coll.gameObject.SetActive(false);
            m_audioSource.PlayOneShot(m_teleportSound, 0.8f * m_gameManager.m_volumeScale);
            m_gameManager.LoadNextLevel(m_teleportSound.length);
        }
    }

    // open the door if all the switches are active; called whenever a switch is flipped
    public bool CheckSwitches() {
        foreach (SwitchController switchCon in m_switchCons) {
            if (switchCon.m_active == false) return false;
        }
        m_audioSource.PlayOneShot(m_unlockSound, m_gameManager.m_volumeScale);
        m_animator.SetBool("open", true);
        m_open = true;
        return true;
    }

    // close the door when the level resets
    void Reset() {
        m_animator.SetBool("open", false);
        m_open = false;
    }
}
