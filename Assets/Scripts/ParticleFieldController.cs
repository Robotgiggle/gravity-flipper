using UnityEngine;

public class ParticleFieldController : MonoBehaviour {
    [Header("Spawner Settings")]
    public GameObject m_particle;
    public float m_spawnRate;
    public bool m_varyRate;
    public int m_minSpawnCount;
    public int m_maxSpawnCount;
    [Header("Particle Settings")]
    public Color m_color;
    public float m_spinRate;
    public float m_shrinkFactor;
    public float m_initialScale;

    Rect m_area;
    float m_timer;

    void Start() {
        m_area = gameObject.GetComponent<RectTransform>().rect;
    }

    // Update is called once per frame
    void Update() {
        // increment spawn timer
        m_timer += Time.deltaTime;

        // spawn a new light at a random position in the area
        if (m_timer > m_spawnRate) {
            m_timer = m_varyRate ? Random.value - 0.5f : 0;

            int toSpawn = Random.Range(m_minSpawnCount, m_maxSpawnCount + 1);
            for (int i = 0; i < toSpawn; i++) {
                float spawnX = Mathf.Lerp(m_area.xMin, m_area.xMax, Random.value);
                float spawnY = Mathf.Lerp(m_area.yMin, m_area.yMax, Random.value);
                Vector3 spawnPos = transform.position + (transform.parent.localRotation * Vector3.Scale(new Vector3(spawnX, spawnY, 0), transform.parent.localScale));

                Vector3 spawnMotion = 0.5f * (Vector3)Random.insideUnitCircle.normalized;
                float spawnSpin = (Random.value > 0.5)? m_spinRate : -m_spinRate;
                float spawnScale = Mathf.Lerp(m_initialScale - 0.02f, m_initialScale + 0.02f, Random.value);

                GameObject newParticle = Instantiate(m_particle, spawnPos, Quaternion.identity);
                newParticle.GetComponent<SpriteRenderer>().color = m_color;
                newParticle.GetComponent<ParticleController>().m_motion = spawnMotion;
                newParticle.GetComponent<ParticleController>().m_shrinkFactor = m_shrinkFactor;
                newParticle.GetComponent<ParticleController>().m_spinRate = spawnSpin;
                newParticle.transform.localScale = new Vector3(spawnScale, spawnScale, 1);
            }
        }
    }
}
