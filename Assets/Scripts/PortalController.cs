using UnityEngine;
using System.Collections.Generic;

public class PortalController : MonoBehaviour {
    public GameObject m_pair;
    public GameObject m_particle;

    PortalController m_pairController;
    SpriteRenderer m_renderer;
    List<GameObject> m_particles;
    float m_particleCooldown;
    float m_teleCooldown;

    void Start() {
        m_pairController = m_pair.GetComponent<PortalController>();
        m_renderer = gameObject.GetComponent<SpriteRenderer>();
        m_particles = new List<GameObject>();
    }

    void Update() {
        // decrement cooldowns
        if (m_particleCooldown > 0) m_particleCooldown -= Time.deltaTime;
        if (m_teleCooldown > 0) m_teleCooldown -= Time.deltaTime;

        // spawn particles
        if (m_particleCooldown <= 0) {
            for (int i = 0; i < 2; i++) {
                Vector3 offsetVert = Vector3.Project(m_renderer.bounds.extents, transform.up) * Random.Range(-1f, 1f);
                Vector3 offsetHoriz = transform.right * 0.35f * Random.Range(-1f, 1f);
                Vector3 spawnPos = transform.position + (transform.right * 0.85f) + offsetVert + offsetHoriz;
                GameObject newParticle = Instantiate(m_particle, spawnPos, Quaternion.identity);

                newParticle.GetComponent<SpriteRenderer>().color = m_renderer.color;
                newParticle.GetComponent<ParticleController>().m_motion = transform.right * -0.65f;
                newParticle.GetComponent<ParticleController>().m_shrinkFactor = 0.985f;
                newParticle.GetComponent<ParticleController>().m_spinRate = 50;
                newParticle.transform.localScale = new Vector3(0.18f, 0.18f, 1);

                m_particles.Add(newParticle);
            }
            m_particleCooldown = 0.1f;
        }

        // delete particles
        for (int i = m_particles.Count; i > 0; i--) {
            GameObject particle = m_particles[i-1];
            if (particle == null) continue;
            if (Vector3.Project(particle.transform.position - transform.position, transform.right).sqrMagnitude < 0.1f) {
                m_particles.RemoveAt(i-1);
                Destroy(particle);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (m_teleCooldown <= 0) {
            // enable cooldown on output portal
            m_pairController.m_teleCooldown = 0.2f;
            // teleport the player
            Vector3 entryOffset = Vector3.Project(collider.transform.position - transform.position, transform.up);
            Quaternion angleDifference = Quaternion.Euler(0, 0, 180 + Vector3.SignedAngle(transform.up, m_pair.transform.up, Vector3.forward));
            collider.transform.position = m_pair.transform.position;
            collider.transform.Translate(angleDifference * entryOffset);
            collider.transform.Translate(0.7f * m_pair.transform.right);
            // change gravity if necessary
            collider.GetComponent<PlayerController>().ChangeGravityTo(m_pair.transform.right);
        }
    }
}
