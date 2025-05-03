using UnityEngine;

public class ParticleBurstController : MonoBehaviour {
    [Header("Spawner Settings")]
    public GameObject m_particle;
    public float m_burstForce;
    public int m_minSpawnCount;
    public int m_maxSpawnCount;
    [Header("Particle Settings")]
    public Color m_color;
    public float m_shrinkFactor;
    public float m_initialScale;

    public void Burst() {
        int toSpawn = Random.Range(m_minSpawnCount, m_maxSpawnCount + 1);
        for (int i = 0; i < toSpawn; i++) {
            Quaternion spawnRot = Quaternion.Euler(0, 0, Random.value * 360);
            float spawnScale = Mathf.Lerp(m_initialScale - 0.02f, m_initialScale + 0.02f, Random.value);

            GameObject newParticle = Instantiate(m_particle, transform.position, spawnRot);
            newParticle.GetComponent<SpriteRenderer>().color = m_color;
            newParticle.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            newParticle.GetComponent<SpriteRenderer>().sortingOrder = 1;
            newParticle.GetComponent<ParticleController>().m_motion = Random.insideUnitCircle * m_burstForce;
            newParticle.GetComponent<ParticleController>().m_shrinkFactor = m_shrinkFactor;
            newParticle.transform.localScale = new Vector3(spawnScale, spawnScale, 1);
        }
    }
}
