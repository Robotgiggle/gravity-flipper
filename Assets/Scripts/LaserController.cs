using UnityEngine;
using System.Collections;

public class LaserController : MonoBehaviour
{
    public float m_uptime;
    public float m_downtime;
    public float m_offset;

    GameObject m_laserObj;
    
    void Start() {
        m_laserObj = transform.GetChild(0).gameObject;
        m_laserObj.SetActive(false);
        StartCoroutine(FireLaser());
    }

    IEnumerator FireLaser() {
        // Offset initial firing to allow for interesting timing
        yield return new WaitForSeconds(m_offset);

        while (true) {
            // Activate laser for specified interval
            m_laserObj.SetActive(true);
            yield return new WaitForSeconds(m_uptime);

            // Deactivate laser for specified interval
            m_laserObj.SetActive(false);
            yield return new WaitForSeconds(m_downtime);
        }
    }
}
