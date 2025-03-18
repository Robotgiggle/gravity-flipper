using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public AudioClip m_gravSound;
    public AudioClip m_deathSound;

    AudioSource m_audioSource;
    Animator m_animator;
    Rigidbody2D m_body;
    Vector3 m_startPos;
    public bool[] m_grounded = new bool[4];
    public float m_gravForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        m_audioSource = GetComponent<AudioSource>();
        m_animator = GetComponent<Animator>();
        m_body = GetComponent<Rigidbody2D>();
        m_startPos = transform.position;
        Respawn();
    }

    // Update is called once per frame
    void Update() {
        // handle gravity changes
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && CanFlip("up")) {
            m_audioSource.PlayOneShot(m_gravSound, 0.06f);
            Physics2D.gravity = new Vector2(0, m_gravForce);
            m_body.linearVelocityX = 0;
            m_body.rotation = 180;
            if (m_grounded[2]) m_body.AddForceX(-10, ForceMode2D.Impulse);
            if (m_grounded[3]) m_body.AddForceX(10, ForceMode2D.Impulse);
        } else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && CanFlip("down")) {
            m_audioSource.PlayOneShot(m_gravSound, 0.06f);
            Physics2D.gravity = new Vector2(0, -m_gravForce);
            m_body.linearVelocityX = 0;
            m_body.rotation = 0;
            if (m_grounded[2]) m_body.AddForceX(-10, ForceMode2D.Impulse);
            if (m_grounded[3]) m_body.AddForceX(10, ForceMode2D.Impulse);
        } else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && CanFlip("left")) {
            m_audioSource.PlayOneShot(m_gravSound, 0.06f);
            Physics2D.gravity = new Vector2(-m_gravForce, 0);
            m_body.linearVelocityY = 0;
            m_body.rotation = -90;
            if (m_grounded[0]) m_body.AddForceY(10, ForceMode2D.Impulse);
            if (m_grounded[1]) m_body.AddForceY(-10, ForceMode2D.Impulse);
        } else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && CanFlip("right")) {
            m_audioSource.PlayOneShot(m_gravSound, 0.06f);
            Physics2D.gravity = new Vector2(m_gravForce, 0);
            m_body.linearVelocityY = 0;
            m_body.rotation = 90;
            if (m_grounded[0]) m_body.AddForceY(10, ForceMode2D.Impulse);
            if (m_grounded[1]) m_body.AddForceY(-10, ForceMode2D.Impulse);
        }
    }

    // returns whether you're allowed to flip in the provided direction
    bool CanFlip(string dir) {
        if (dir == "up") {
            return !m_grounded[0] && (m_grounded[1] || m_grounded[2] || m_grounded[3]);
        } else if (dir == "down") {
            return !m_grounded[1] && (m_grounded[0] || m_grounded[2] || m_grounded[3]);
        } else if (dir == "left") {
            return !m_grounded[2] && (m_grounded[1] || m_grounded[0] || m_grounded[3]);
        } else if (dir == "right") {
            return !m_grounded[3] && (m_grounded[1] || m_grounded[2] || m_grounded[0]);
        }
        return false;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Hazard")) {
            Die();
        } else if (collider.CompareTag("Bonus")) {
            GameObject bonus = collider.gameObject;
            ScoreTracker.holdingBonus = bonus.GetComponent<BonusController>().index;
            bonus.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Hazard")) {
            Die();
        } else {
            ContactPoint2D point = collision.GetContact(0);
            if (point.normal == new Vector2(0, -1)) m_grounded[0] = true;
            else if (point.normal == new Vector2(0, 1)) m_grounded[1] = true;
            else if (point.normal == new Vector2(1, 0)) m_grounded[2] = true;
            else if (point.normal == new Vector2(-1, 0)) m_grounded[3] = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ceiling")) m_grounded[0] = false;
        else if (collision.gameObject.CompareTag("Floor")) m_grounded[1] = false;
        else if (collision.gameObject.CompareTag("LeftWall")) m_grounded[2] = false;
        else if (collision.gameObject.CompareTag("RightWall")) m_grounded[3] = false;
    }

    void Die() {
        ScoreTracker.deaths++;
        Debug.Log("Total deaths: " + ScoreTracker.deaths);
        m_audioSource.PlayOneShot(m_deathSound, 0.15f);

        Respawn();
    }

    void Respawn() {
        // drop bonus if you were carrying it
        ScoreTracker.holdingBonus = -1;

        // move to spawn pos
        transform.position = m_startPos;

        // reset gravity
        Physics2D.gravity = new Vector2(0, -m_gravForce);
        m_body.linearVelocity = Vector2.zero;
        m_body.rotation = 0;
        for (int i = 0; i < 4; i++) m_grounded[i] = false;
    }
}
