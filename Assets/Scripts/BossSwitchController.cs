using UnityEngine;

public class BossSwitchController : SwitchController {
    ParticleBurstController m_burst;

    protected override void Start() {
        base.Start();
        m_burst = gameObject.GetComponentInChildren<ParticleBurstController>();
    }

    protected override void Activate() {
        base.Activate();
        m_burst.Burst();
    }
}