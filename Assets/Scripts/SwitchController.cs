using UnityEngine;

public class SwitchController : MonoBehaviour {
    public bool m_active = false;
    public Sprite[] m_sprites = new Sprite[2];

    AudioSource m_audioSource;
    SpriteRenderer m_renderer;
    DoorController m_doorCon;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        m_audioSource = gameObject.GetComponent<AudioSource>();
        m_renderer = gameObject.GetComponent<SpriteRenderer>();
        m_doorCon = GameObject.Find("ExitDoor").GetComponent<DoorController>();
    }

    // when colliding with the player, activate *only* if the player is moving in the correct direction
    // direction is indicated by the arrow on the sprite, and is the local positive X
    void OnTriggerStay2D(Collider2D coll) {
        Vector3 playerDir = Vector3.Normalize((Vector3) coll.attachedRigidbody.linearVelocity);
        if (playerDir == transform.right && !m_active) {
            Activate();
        }
    }

    void Activate() {
        m_audioSource.Play();
        m_renderer.sprite = m_sprites[1];
        m_active = true;
        m_doorCon.CheckSwitches();
    }

    // DEBUG PURPOSES ONLY
    void OnMouseDown() {
        Activate();
    }
}
