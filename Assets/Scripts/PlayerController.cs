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
    bool[] m_grounded = new bool[4];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        m_audioSource = GetComponent<AudioSource>();
        m_animator = GetComponent<Animator>();
        m_body = GetComponent<Rigidbody2D>();
        m_startPos = transform.position;
        ResetGravity();
    }

    // Update is called once per frame
    void Update() {
        // handle gravity changes and sprite flipping
        Vector3 newScale = transform.localScale;
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && CanFlip("up")) {
            m_audioSource.PlayOneShot(m_gravSound, 0.08f);
            Physics2D.gravity = new Vector2(0, 7f);
            m_body.linearVelocityX = 0;
            newScale.y = -5;
        } else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && CanFlip("down")) {
            m_audioSource.PlayOneShot(m_gravSound, 0.08f);
            Physics2D.gravity = new Vector2(0, -7f);
            m_body.linearVelocityX = 0;
            newScale.y = 5;
        } else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && CanFlip("left")) {
            m_audioSource.PlayOneShot(m_gravSound, 0.08f);
            Physics2D.gravity = new Vector2(-7f, 0);
            m_body.linearVelocityY = 0;
            newScale.x = -5;
        } else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && CanFlip("right")) {
            m_audioSource.PlayOneShot(m_gravSound, 0.08f);
            Physics2D.gravity = new Vector2(7f, 0);
            m_body.linearVelocityY = 0;
            newScale.x = 5;
        }
        transform.localScale = newScale;

        // tell animator whether the player is moving horizontally
        m_animator.SetFloat("Speed", Mathf.Abs(m_body.linearVelocityX));
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

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ceiling")) {
            m_grounded[0] = true;
        } else if (collision.gameObject.CompareTag("Floor")) {
            m_grounded[1] = true;
        } else if (collision.gameObject.CompareTag("LeftWall")) {
            m_grounded[2] = true;
        } else if (collision.gameObject.CompareTag("RightWall")) {
            m_grounded[3] = true;
        } else if (collision.gameObject.CompareTag("Spike")) {
            Die();
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ceiling")) {
            m_grounded[0] = false;
        } else if (collision.gameObject.CompareTag("Floor")) {
            m_grounded[1] = false;
        } else if (collision.gameObject.CompareTag("LeftWall")) {
            m_grounded[2] = false;
        } else if (collision.gameObject.CompareTag("RightWall")) {
            m_grounded[3] = false;
        }
    }

    void Die() {
        ScoreTracker.deaths++;
        Debug.Log("Total deaths: " + ScoreTracker.deaths);
        m_audioSource.PlayOneShot(m_deathSound, 0.8f);

        // respawn at start position
        transform.position = m_startPos;
        ResetGravity();
    }

    void ResetGravity() {
        transform.localScale = new Vector3(5f, 5f, 5f);
        Physics2D.gravity = new Vector2(0, -7f);
        m_body.linearVelocity = Vector2.zero;
        for (int i = 0; i < 4; i++) m_grounded[i] = false;
    }
}
