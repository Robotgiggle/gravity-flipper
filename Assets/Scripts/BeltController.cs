using UnityEngine;
using System.Collections.Generic;

public class BeltController : MonoBehaviour {
    public GameObject m_beltLight;
    public float m_lightSpeed;
    public bool m_vertical;

    GameManager m_gameManager;
    List<GameObject> m_activeLights;
    SurfaceEffector2D m_effector;
    Vector3 m_startPos;
    float m_sparkCooldown;
    float m_maxDist;
    float m_timer;
    
    void Start() {
        m_gameManager = GameManager.TheInstance;
        m_activeLights = new List<GameObject>();
        m_effector = gameObject.GetComponent<SurfaceEffector2D>();
        float offset;
        if (m_vertical) offset = GetComponent<SpriteRenderer>().bounds.size.y;
        else offset = GetComponent<SpriteRenderer>().bounds.size.x;
        m_startPos = transform.position + transform.right * (-offset/2 + 0.19f);
        m_maxDist = offset - 0.38f;

        if (m_gameManager.m_hardMode) m_effector.forceScale = 0.17f;
        
        Vector3 spawnPos = m_startPos;
        while (m_lightSpeed != 0) {
            SpawnLight(spawnPos);
            spawnPos += transform.right * m_lightSpeed * 0.3f;
            if (Vector3.Distance(spawnPos, m_startPos) > m_maxDist) break;
        }
    }

    void Update() {
        // increment spawn timer
        m_timer += Time.deltaTime;

        // remove lights once they reach the end
        for (int i = m_activeLights.Count; i > 0; i--) {
            GameObject light = m_activeLights[i-1];
            if (Vector3.Distance(light.transform.position, m_startPos) > m_maxDist) {
                m_activeLights.RemoveAt(i-1);
                Destroy(light);
            }
        }

        // spawn a new light
        if (m_timer > 0.3) {
            m_timer = 0;
            SpawnLight(m_startPos);
        }
    }

    // set momentum to zero when leaving if gravity doesn't align with the belt
    void OnCollisionExit2D(Collision2D collision) {
        if (Physics2D.gravity.normalized != (Vector2)transform.right)
            collision.rigidbody.linearVelocity = Vector3.zero;
    }

    void OnCollisionStay2D(Collision2D collision) {
        // if moving against the belt, make sparks
        Vector3 grav3D = (Vector3)Physics2D.gravity.normalized;
        if (grav3D == -transform.right && m_sparkCooldown <= 0) {
            Vector3 offset = Vector3.Project(transform.position - collision.transform.position, transform.up).normalized * 0.5f;
            Vector3 randomize = (Random.Range(-0.55f, -0.1f) * transform.right) + (Random.Range(-0.09f, 0.09f) * transform.up);
            Vector3 sparkPos = collision.transform.position + offset + randomize;

            GameObject newSpark = Instantiate(m_beltLight, sparkPos, Quaternion.identity);
            newSpark.GetComponent<SpriteRenderer>().color = new Color(1, 0.7f, 0);
            newSpark.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            newSpark.GetComponent<SpriteRenderer>().sortingOrder = 3;
            newSpark.GetComponent<ParticleController>().m_shrinkFactor = 0.98f;
            newSpark.GetComponent<ParticleController>().m_spinRate = 30;
            newSpark.GetComponent<ParticleController>().m_motion = transform.right * 1.5f;
            newSpark.transform.localScale = new Vector3(0.2f, 0.2f, 1);
            m_sparkCooldown = 0.025f;
        }
        if (m_sparkCooldown > 0) m_sparkCooldown -= Time.deltaTime;
    }

    void SpawnLight(Vector3 pos) {
        GameObject newLight = Instantiate(m_beltLight, pos, Quaternion.identity);
        newLight.GetComponent<SpriteRenderer>().color = new Color(1, 0.9f, 0);
        newLight.GetComponent<SpriteRenderer>().sortingOrder = 3;
        newLight.GetComponent<ParticleController>().m_motion = transform.right * m_lightSpeed;
        newLight.transform.localScale = new Vector3(0.15f, 0.15f, 1);
        m_activeLights.Add(newLight);
    }
}
