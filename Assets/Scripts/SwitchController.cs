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

    // activate only if both gravity and the player are moving in the correct direction
    // needs to account for flipping while next to a switch (hitbox enters but velocity is 0)
    void OnTriggerEnter2D(Collider2D coll) {
        Vector3 gravDir = Physics2D.gravity.normalized;
        Vector3 motionDir = coll.attachedRigidbody.linearVelocity.normalized;
        if (gravDir == transform.right && Vector3.Dot(gravDir, motionDir) != -1 && !m_active) {
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
