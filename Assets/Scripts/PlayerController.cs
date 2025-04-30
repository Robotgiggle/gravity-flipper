using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour {
    public AudioClip m_gravSound;
    public AudioClip m_deathSound;
    public AudioClip m_bonusSound;
    public GameObject m_bonus;
    public GameObject m_bonusAura;
    public GameObject m_afterimage;
    public GameObject[] m_indicators = new GameObject[4];
    public Sprite[] m_normalSprites = new Sprite[4];
    public Sprite[] m_bonusSprites = new Sprite[4];
    public ParticleBurstController m_deathBurst;
    public float m_gravForce;

    GameManager m_gameManager;
    AudioSource m_audioSource;
    SpriteRenderer m_renderer;
    Rigidbody2D m_body;
    Vector3 m_startPos;
    Vector3 m_lastPos;
    Vector2 m_lastVel;
    bool[] m_grounded = new bool[4];
    float m_trailCooldown;
    float m_flipCooldown;

    const int UP = 0, DOWN = 1, LEFT = 2, RIGHT = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        m_gameManager = GameManager.TheInstance;
        m_audioSource = GetComponent<AudioSource>();
        m_renderer = GetComponent<SpriteRenderer>();
        m_body = GetComponent<Rigidbody2D>();
        m_startPos = m_lastPos = transform.position;
        m_gameManager.m_resetLevelEvent.AddListener(Respawn);
        if (m_gameManager.m_hardMode) m_gravForce = 12;
        Respawn();
    }

    // FixedUpdate is called once per physics update
    void FixedUpdate() {
        // draw motion trail
        if (m_body.linearVelocity != Vector2.zero && m_trailCooldown <= 0) {
            GameObject newImage = Instantiate(m_afterimage, transform.position, Quaternion.identity);
            newImage.GetComponent<SpriteRenderer>().sprite = m_renderer.sprite;
            newImage.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.55f);
            newImage.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            newImage.GetComponent<SpriteRenderer>().sortingOrder = 1;
            newImage.GetComponent<ParticleController>().m_fadeRate = 3;
            m_trailCooldown = 0.01f;
        }
        // decrement trail cooldown
        if (m_trailCooldown > 0) m_trailCooldown -= Time.deltaTime;
    }

    // Update is called once per frame
    void Update() {
        // handle gravity changes
        if (!m_gameManager.m_inputsLocked) {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                if (CanFlip(UP)) FlipGravity(UP);
            } else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                if (CanFlip(DOWN)) FlipGravity(DOWN);
            } else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                if (CanFlip(LEFT)) FlipGravity(LEFT);
            } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                if (CanFlip(RIGHT)) FlipGravity(RIGHT);
            }
        }
        // display flip indicators
        for (int i = 0; i < 4; i++) {
            if (CanFlip(i) && m_gameManager.m_flipIndicators) {
                m_indicators[i].SetActive(true);
            } else {
                m_indicators[i].SetActive(false);
            }
        }
        // track distance travelled
        m_gameManager.m_totalDistance += Vector3.Distance(transform.position, m_lastPos);
        m_lastPos = transform.position;
        m_lastVel = m_body.linearVelocity;
        // decrement flip cooldown and reset belt-ignore status
        if (m_flipCooldown > 0) m_flipCooldown -= Time.deltaTime;
        if (m_flipCooldown <= 0.15f) gameObject.layer = 0;
        // debug mode
        if (Input.GetKeyDown(KeyCode.D) && Input.GetKey(KeyCode.LeftShift))
            m_gameManager.m_debugMode = !m_gameManager.m_debugMode;
        if (Input.GetKeyDown(KeyCode.T) && m_gameManager.m_debugMode){
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(mousePos.x, mousePos.y);
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Hazard")) {
            StartCoroutine(Die());
        } else if (m_bonus != null && collider.gameObject == m_bonus) {
            m_bonus.GetComponentInChildren<ParticleBurstController>().Burst();
            m_bonus.SetActive(false);
            m_audioSource.PlayOneShot(m_bonusSound, 1.1f * m_gameManager.m_volumeScale);
            if (m_gameManager.BonusCollected()) return;
            m_gameManager.m_holdingBonus = true;
            int index = Array.IndexOf(m_normalSprites, m_renderer.sprite);
            m_renderer.sprite = m_bonusSprites[index];
            m_bonusAura.SetActive(true);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Hazard")) {
            StartCoroutine(Die());
        } else {
            CheckCornerPop(collision);
            ContactPoint2D hitPoint = collision.GetContact(0);
            if (hitPoint.normal == new Vector2(0, -1)) m_grounded[UP] = true;
            else if (hitPoint.normal == new Vector2(0, 1)) m_grounded[DOWN] = true;
            else if (hitPoint.normal == new Vector2(1, 0)) m_grounded[LEFT] = true;
            else if (hitPoint.normal == new Vector2(-1, 0)) m_grounded[RIGHT] = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ceiling")) m_grounded[UP] = false;
        else if (collision.gameObject.CompareTag("Floor")) m_grounded[DOWN] = false;
        else if (collision.gameObject.CompareTag("LeftWall")) m_grounded[LEFT] = false;
        else if (collision.gameObject.CompareTag("RightWall")) m_grounded[RIGHT] = false;
    }

    // returns whether you're allowed to flip in the provided direction
    public bool CanFlip(int dir) {
        Vector2 gravDir = Physics2D.gravity.normalized;
        if (dir == UP) {
            if (gravDir == Vector2.up || m_flipCooldown > 0.1f) return false;
            if (gravDir == Vector2.down && m_flipCooldown > 0) return false;
            return !m_grounded[UP] && (m_grounded[DOWN] || m_grounded[LEFT] || m_grounded[RIGHT]);
        } else if (dir == DOWN) {
            if (gravDir == Vector2.down || m_flipCooldown > 0.1f) return false;
            if (gravDir == Vector2.up && m_flipCooldown > 0) return false;
            return !m_grounded[DOWN] && (m_grounded[UP] || m_grounded[LEFT] || m_grounded[RIGHT]);
        } else if (dir == LEFT) {
            if (gravDir == Vector2.left || m_flipCooldown > 0.1f) return false;
            if (gravDir == Vector2.right && m_flipCooldown > 0) return false;
            return !m_grounded[LEFT] && (m_grounded[DOWN] || m_grounded[UP] || m_grounded[RIGHT]);
        } else if (dir == RIGHT) {
            if (gravDir == Vector2.right || m_flipCooldown > 0.1f) return false;
            if (gravDir == Vector2.left && m_flipCooldown > 0) return false;
            return !m_grounded[RIGHT] && (m_grounded[DOWN] || m_grounded[LEFT] || m_grounded[UP]);
        }
        return false;
    }

    // change gravity to the specified direction, used when passing through a portal
    public void ChangeGravityTo(Vector3 newDir) {
        // rotate sprite
        int spriteNum = -1;
        if (newDir == Vector3.up) spriteNum = 0;
        else if (newDir == Vector3.down) spriteNum = 1;
        else if (newDir == Vector3.left) spriteNum = 2;
        else if (newDir == Vector3.right) spriteNum = 3;
        m_renderer.sprite = m_gameManager.m_holdingBonus ? m_bonusSprites[spriteNum] : m_normalSprites[spriteNum];
        // change world gravity
        Physics2D.gravity = newDir * m_gravForce;
        // rotate linear velocity to match new gravity
        Quaternion rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(m_body.linearVelocity, Physics2D.gravity, Vector3.forward));
        m_body.linearVelocity = (Vector2)(rotation * m_body.linearVelocity);
    }

    // flip gravity to point in one of four directions, used by the player when moving
    void FlipGravity(int dir) {
        m_audioSource.PlayOneShot(m_gravSound, 0.09f * m_gameManager.m_volumeScale);
        m_gameManager.m_totalFlips++;
        m_renderer.sprite = m_gameManager.m_holdingBonus ? m_bonusSprites[dir] : m_normalSprites[dir];
        m_flipCooldown = 0.2f;
        // change world gravity
        if (dir == UP) {
            Physics2D.gravity = Vector2.up * m_gravForce;
        } else if (dir == DOWN) {
            Physics2D.gravity = Vector2.down * m_gravForce;
        } else if (dir == LEFT) {
            Physics2D.gravity = Vector2.left * m_gravForce;
        } else if (dir == RIGHT) {
            Physics2D.gravity = Vector2.right * m_gravForce;
        }
        // cancel existing momentum
        m_body.linearVelocity = Vector3.zero;
        // temporarily ignore belts after flipping
        gameObject.layer = 3;
    }

    // check if the player is right on a corner, and pop them around it if so
    void CheckCornerPop(Collision2D coll) {
        ContactPoint2D[] allHitPoints = new ContactPoint2D[10];
        coll.GetContacts(allHitPoints);
        Vector2 gravDir = Physics2D.gravity.normalized;

        string groundTag;
        if (gravDir == Vector2.up) groundTag = "Ceiling";
        else if (gravDir == Vector2.down) groundTag = "Floor";
        else if (gravDir == Vector2.left) groundTag = "LeftWall";
        else if (gravDir == Vector2.right) groundTag = "RightWall";
        else groundTag = "INVALID";

        // find the 2 collision points with the ground
        Vector2 pointA = Vector2.zero;
        Vector2 pointB = Vector2.zero;
        foreach (ContactPoint2D hitPoint in allHitPoints) {
            if (hitPoint.point != Vector2.zero && hitPoint.collider.CompareTag(groundTag)) {
                if (pointA == Vector2.zero) pointA = hitPoint.point;
                else pointB = hitPoint.point;
            }
        }

        // if one or both points cannot be found, player is in midair
        if (pointA == Vector2.zero || pointB == Vector2.zero) return;

        // if distance between the points is less than 0.3, player is on a corner
        if (Vector2.Distance(pointA, pointB) < 0.3f) {
            // subtract closer point from further point to get pop offset
            Vector3 offset;
            if (Vector3.Distance((Vector3)pointA, transform.position) < Vector3.Distance((Vector3)pointB, transform.position)) {
                offset = pointA - pointB;
            } else {
                offset = pointB - pointA;
            }
            // pop the player around the corner, then reset their velocity
            transform.position = transform.position + (offset * 1.05f);
            m_body.linearVelocity = m_lastVel;
        }
    }

    // die and restart the level
    IEnumerator Die() {
        m_deathBurst.Burst();
        transform.position = new Vector3(100,100,0);
        m_audioSource.PlayOneShot(m_deathSound, 0.3f * m_gameManager.m_volumeScale);
        m_gameManager.AddDeath();
        yield return new WaitForSeconds(0.5f);
        m_gameManager.ResetLevel();
    }

    // set gravity to downward, reset the player's position, visually drop the bonus
    void Respawn() {
        Physics2D.gravity = Vector2.down * m_gravForce;
        m_body.linearVelocity = Vector2.zero;
        m_body.rotation = 0;
        for (int i = 0; i < 4; i++) m_grounded[i] = false;
        m_renderer.sprite = m_normalSprites[1];
        m_bonusAura.SetActive(false);
        transform.position = m_startPos;
    }

    
}
