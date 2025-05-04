using UnityEngine;

public class SwitchController : MonoBehaviour {
    public bool m_active = false;
    public Sprite[] m_sprites = new Sprite[2];

    GameManager m_gameManager;
    AudioSource m_audioSource;
    protected SpriteRenderer m_renderer;
    DoorController m_doorCon;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start() {
        m_gameManager = GameManager.TheInstance;
        m_audioSource = gameObject.GetComponent<AudioSource>();
        m_renderer = gameObject.GetComponent<SpriteRenderer>();
        m_doorCon = GameObject.Find("ExitDoor").GetComponent<DoorController>();
        m_gameManager.m_resetLevelEvent.AddListener(Reset);
    }

    // activate only if the player is moving in the correct direction
    void OnTriggerEnter2D(Collider2D coll) {
        Vector3 motionDir = coll.attachedRigidbody.linearVelocity.normalized;
        if (motionDir == transform.right && !m_active) {
            Activate();
        }
    }

    // activate the switch, then tell the door to check all other switches
    protected virtual void Activate() {
        m_active = true;
        if (!m_doorCon.CheckSwitches()) m_audioSource.Play();
        m_renderer.sprite = m_sprites[1];
    }

    // reset the switch
    protected virtual void Reset() {
        m_renderer.sprite = m_sprites[0];
        m_active = false;
        m_doorCon.CheckSwitches();
    }

    // activate on click in debug mode
    void OnMouseDown() {
        if (m_gameManager.m_debugMode) Activate();
    }
}
