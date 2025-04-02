using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public AudioClip m_gravSound;
    public AudioClip m_deathSound;
    public GameObject m_bonus;
    public float m_gravForce;

    GameManager m_gameManager;
    AudioSource m_audioSource;
    Animator m_animator;
    Rigidbody2D m_body;
    Vector3 m_startPos;
    Vector3 m_lastPos;
    float m_invulTime;
    [SerializeField] bool[] m_grounded = new bool[4];
    bool m_holdingBonus;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        m_gameManager = GameManager.TheInstance;
        m_audioSource = GetComponent<AudioSource>();
        m_animator = GetComponent<Animator>();
        m_body = GetComponent<Rigidbody2D>();
        m_startPos = m_lastPos = transform.position;
        ResetGravity();
    }

    // Update is called once per frame
    void Update() {
        // handle gravity changes
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && CanFlip("up")) {
            FlipGravity("up");
        } else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && CanFlip("down")) {
            FlipGravity("down");
        } else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && CanFlip("left")) {
            FlipGravity("left");
        } else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && CanFlip("right")) {
            FlipGravity("right");
        }
        // handle iframes
        if (m_invulTime > 0) m_invulTime -= Time.deltaTime;
        // track distance travelled
        m_gameManager.m_totalDistance += Vector3.Distance(transform.position, m_lastPos);
        m_lastPos = transform.position;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Hazard") && m_invulTime <= 0) {
            Die();
        } else if (m_bonus != null && collider.gameObject == m_bonus) {
            m_gameManager.m_holdingBonus = true;
            m_bonus.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Hazard")) {
            if (m_invulTime <= 0) Die();
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

    // returns whether you're allowed to flip in the provided direction
    bool CanFlip(string dir) {
        Vector2 gravDir = Physics2D.gravity.normalized;
        if (dir == "up") {
            if (gravDir == Vector2.up) return false;
            return !m_grounded[0] && (m_grounded[1] || m_grounded[2] || m_grounded[3]);
        } else if (dir == "down") {
            if (gravDir == Vector2.down) return false;
            return !m_grounded[1] && (m_grounded[0] || m_grounded[2] || m_grounded[3]);
        } else if (dir == "left") {
            if (gravDir == Vector2.left) return false;
            return !m_grounded[2] && (m_grounded[1] || m_grounded[0] || m_grounded[3]);
        } else if (dir == "right") {
            if (gravDir == Vector2.right) return false;
            return !m_grounded[3] && (m_grounded[1] || m_grounded[2] || m_grounded[0]);
        }
        return false;
    }

    // flip gravity to point in the provided direction
    void FlipGravity(string dir) {
        m_audioSource.PlayOneShot(m_gravSound, 0.06f * m_gameManager.m_volumeScale);
        m_gameManager.m_totalFlips++;
        // apply iframes to prevent cheap shots from hazards when rotating
        m_invulTime = 0.1f;
        // flip the gravity and rotate the player
        if (dir == "up") {
            Physics2D.gravity = Vector2.up * m_gravForce;
            m_body.rotation = 180;
        } else if (dir == "down") {
            Physics2D.gravity = Vector2.down * m_gravForce;
            m_body.rotation = 0;
        } else if (dir == "left") {
            Physics2D.gravity = Vector2.left * m_gravForce;
            m_body.rotation = -90;
        } else if (dir == "right") {
            Physics2D.gravity = Vector2.right * m_gravForce;
            m_body.rotation = 90;
        }
        // when the player rotates parallel to the wall, shove them into it to maintain contact
        if (dir == "left" || dir == "right") {
            m_body.linearVelocityY = 0;
            if (m_grounded[0]) m_body.AddForceY(10, ForceMode2D.Impulse);
            if (m_grounded[1]) m_body.AddForceY(-10, ForceMode2D.Impulse);
        } else {
            m_body.linearVelocityX = 0;
            if (m_grounded[2]) m_body.AddForceX(-10, ForceMode2D.Impulse);
            if (m_grounded[3]) m_body.AddForceX(10, ForceMode2D.Impulse);
        }
    }

    // die, drop the bonus if you have it, and respawn at startPos
    void Die() {
        m_gameManager.AddDeath();
        m_audioSource.PlayOneShot(m_deathSound, 0.25f * m_gameManager.m_volumeScale);
        transform.position = m_startPos;
        ResetGravity();
        m_gameManager.ResetLevel();
    }

    // set gravity to downward, used when spawning the player
    void ResetGravity() {
        Physics2D.gravity = Vector2.down * m_gravForce;
        m_body.linearVelocity = Vector2.zero;
        m_body.rotation = 0;
        for (int i = 0; i < 4; i++) m_grounded[i] = false;
    }
}
