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
        m_toggleEvent.AddListener(Toggle);
    }

    // activate only if the player is moving in the correct direction and the switch is in the correct state
    void OnTriggerEnter2D(Collider2D coll) {
        Vector3 motionDir = coll.attachedRigidbody.linearVelocity.normalized;
        if (motionDir == transform.right && !m_active) {
            m_audioSource.Play();
            m_toggleEvent.Invoke();
        } else if (motionDir == -transform.right && m_active) {
            m_audioSource.Play();
            m_toggleEvent.Invoke();
        }
    }

    // visually flip the switch
    void Toggle() {
        if (m_active) {
            m_renderer.sprite = m_sprites[0];
            m_active = false;
        } else {
            m_renderer.sprite = m_sprites[1];
            m_active = true;
        }
    }

    // flip the switch, and tell other objects to change state
    // void Flip(bool on) {
    //     m_audioSource.Play();
    //     if (on) {
    //         m_renderer.sprite = m_sprites[1];
    //         m_active = true;
    //     } else {
    //         m_renderer.sprite = m_sprites[0];
    //         m_active = false;
    //     }
    //     m_toggleEvent.Invoke();
    // }

    // reset the switch
    void Reset() {
        m_renderer.sprite = m_sprites[0];
        m_active = false;
    }

    // activate on click in debug mode
    void OnMouseDown() {
        if (m_gameManager.m_debugMode) {
            m_audioSource.Play();
            m_toggleEvent.Invoke();
        }
    }
}
