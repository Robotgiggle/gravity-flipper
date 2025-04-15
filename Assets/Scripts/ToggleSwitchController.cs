using UnityEngine;
using UnityEngine.Events;

public class ToggleSwitchController : MonoBehaviour {
    public static UnityEvent m_toggleEvent;
    
    public bool m_active = false;
    public Sprite[] m_sprites = new Sprite[2];

    GameManager m_gameManager;
    AudioSource m_audioSource;
    SpriteRenderer m_renderer;

    void Awake() {
        // this has to go here so it runs before any Start() methods
        if (m_toggleEvent == null) m_toggleEvent = new UnityEvent();
    }

    void Start() {
        m_gameManager = GameManager.TheInstance;
        m_audioSource = gameObject.GetComponent<AudioSource>();
        m_renderer = gameObject.GetComponent<SpriteRenderer>();
        m_gameManager.m_resetLevelEvent.AddListener(Reset);
    }

    // activate only if both gravity and the player are moving in the correct direction
    // needs to account for flipping while next to a switch (hitbox enters but velocity is 0)
    void OnTriggerEnter2D(Collider2D coll) {
        // TODO double flip
        Vector3 gravDir = Physics2D.gravity.normalized;
        Vector3 motionDir = coll.attachedRigidbody.linearVelocity.normalized;
        if (gravDir == transform.right && Vector3.Dot(gravDir, motionDir) != -1 && !m_active) {
            Flip(true);
        } else if (gravDir == -transform.right && Vector3.Dot(gravDir, motionDir) != -1 && m_active) {
            Flip(false);
        }
    }

    // flip the switch, and tell other objects to change state
    void Flip(bool on) {
        m_audioSource.Play();
        if (on) {
            m_renderer.sprite = m_sprites[1];
            m_active = true;
        } else {
            m_renderer.sprite = m_sprites[0];
            m_active = false;
        }
        m_toggleEvent.Invoke();
    }

    // reset the switch
    void Reset() {
        m_renderer.sprite = m_sprites[0];
        m_active = false;
    }

    // activate on click in debug mode
    void OnMouseDown() {
        if (m_gameManager.m_debugMode) {
            if (m_active) Flip(false);
            else Flip(true);
        }
    }
}
