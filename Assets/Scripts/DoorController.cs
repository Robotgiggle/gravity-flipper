using UnityEngine;

public class DoorController : MonoBehaviour {
    public AudioClip m_unlockSound;
    public AudioClip m_teleportSound;
    public Sprite[] m_sprites = new Sprite[2];
    public string m_nextScene;
    public bool m_open = false;

    GameManager m_gameManager;
    AudioSource m_audioSource;
    SpriteRenderer m_renderer;
    SwitchController[] m_switchCons;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        m_gameManager = GameManager.TheInstance;
        m_audioSource = gameObject.GetComponent<AudioSource>();
        m_renderer = gameObject.GetComponent<SpriteRenderer>();
        GameObject[] switches = GameObject.FindGameObjectsWithTag("Switch");
        m_switchCons = new SwitchController[switches.Length];
        for (int i = 0; i < switches.Length; i++) {
            m_switchCons[i] = switches[i].GetComponent<SwitchController>();
        }
    }

    // when colliding with the player, move to the next level if the door is open
    void OnTriggerEnter2D(Collider2D coll) {
        if (m_open && coll.CompareTag("Player")) {
            coll.gameObject.SetActive(false);
            m_audioSource.PlayOneShot(m_teleportSound, 0.85f);
            m_gameManager.LoadNextLevel(m_teleportSound.length);
        }
    }

    // open the door if all the switches are active; called whenever a switch is flipped
    public void CheckSwitches() {
        foreach (SwitchController switchCon in m_switchCons) {
            if (switchCon.m_active == false) return;
        }
        m_audioSource.PlayOneShot(m_unlockSound);
        m_renderer.sprite = m_sprites[1];
        m_open = true;
    }
}
