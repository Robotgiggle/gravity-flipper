using UnityEngine;
using System.Collections;

public class BossSwitchController : SwitchController {
    public GameObject[] m_tentacleSegments;
    public ParticleBurstController m_switchBurst;
    public ParticleBurstController m_deathBurst;
    public bool m_saved;

    Coroutine m_vanishCR;

    protected override void Start() {
        base.Start();
    }

    protected override void Activate() {
        base.Activate();
        m_switchBurst.Burst();
    }

    protected override void Reset() {
        if (!m_saved) {
            if (m_vanishCR != null) StopCoroutine(m_vanishCR);
            base.Reset();
            foreach (GameObject segment in m_tentacleSegments) segment.SetActive(true);
            base.m_renderer.enabled = true;
        }
    }

    public void Vanish() {
        m_vanishCR = StartCoroutine(VanishCoroutine());
    }

    public IEnumerator VanishCoroutine() {
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
}