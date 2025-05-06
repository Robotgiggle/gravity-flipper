using UnityEngine;
using System.Collections;

public class BossSwitchController : SwitchController {
    public GameObject[] m_tentacleSegments;
    public ParticleBurstController m_switchBurst;
    public ParticleBurstController m_deathBurst;

    protected override void Start() {
        base.Start();
    }

    protected override void Activate() {
        base.Activate();
        m_switchBurst.Burst();
    }

    protected override void Reset() {
        if (m_renderer.enabled) base.Reset();
    }

    public IEnumerator Vanish() {
        float delay = 1.2f / m_tentacleSegments.Length;
        foreach (GameObject segment in m_tentacleSegments) {
            m_deathBurst.BurstPos(segment.transform.position);
            segment.SetActive(false);
            yield return new WaitForSeconds(delay);
        }
        m_deathBurst.Burst();
        base.m_renderer.enabled = false;
    }
}