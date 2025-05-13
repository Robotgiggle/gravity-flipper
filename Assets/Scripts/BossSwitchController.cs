using UnityEngine;
using System.Collections;

public class BossSwitchController : SwitchController {
    public GameObject[] m_tentacleSegments;
    public ParticleBurstController m_switchBurst;
    public ParticleBurstController m_deathBurst;
    public bool m_saved;

    Coroutine m_vanishCR;
    float m_pulseCooldown;
    int m_whichSegment;

    protected override void Start() {
        base.Start();
    }

    void Update() {
        if (m_pulseCooldown > 0) m_pulseCooldown -= Time.deltaTime;
        else {
            StartCoroutine(PulseSegment(m_whichSegment));
            m_whichSegment = (m_whichSegment + 1) % m_tentacleSegments.Length;
            m_pulseCooldown = 0.5f;
        }
    }

    protected override void Activate() {
        base.Activate();
        m_switchBurst.Burst();
    }

    protected override void Reset() {
        if (!m_saved) {
            if (m_vanishCR != null) StopCoroutine(m_vanishCR);
            m_whichSegment = 0;
            m_pulseCooldown = 0;
            foreach (GameObject segment in m_tentacleSegments) segment.SetActive(true);
            base.Reset();
            base.m_renderer.enabled = true;
        }
    }

    public void Vanish() {
        m_vanishCR = StartCoroutine(VanishCoroutine());
    }

    IEnumerator VanishCoroutine() {
        // wait for boss damage animation
        yield return new WaitForSeconds(0.5f);
        // explode the tentacle
        float delay = 1.2f / m_tentacleSegments.Length;
        foreach (GameObject segment in m_tentacleSegments) {
            m_deathBurst.BurstPos(segment.transform.position);
            segment.SetActive(false);
            yield return new WaitForSeconds(delay);
        }
        // explode self
        m_deathBurst.Burst();
        base.m_renderer.enabled = false;
    }

    IEnumerator PulseSegment(int which) {
        for (int i = 0; i < 5; i++) {
            m_tentacleSegments[which].transform.localScale += new Vector3(0, 0.016f, 0);
            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0; i < 5; i++) {
            m_tentacleSegments[which].transform.localScale -= new Vector3(0, 0.016f, 0);
            yield return new WaitForSeconds(0.1f);
        }
    }
}