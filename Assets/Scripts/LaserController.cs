using UnityEngine;
using System.Collections;

public class LaserController : MonoBehaviour
{
    public float m_uptime;
    public float m_downtime;
    public float m_offset;
    public float m_telegraph;
    
    GameObject m_laserObj;
    GameObject m_laserTeleObj;
    
    
    void Start() {
        m_laserObj = transform.GetChild(0).gameObject;
        m_laserTeleObj = transform.GetChild(1).gameObject;
        m_laserObj.SetActive(false);
        m_laserTeleObj.SetActive(false);
        
        StartCoroutine(FireLaser());
    }

    IEnumerator FireLaser() {
        // Offset initial firing to allow for interesting timing
        yield return new WaitForSeconds(m_offset);

        while (true) {
            // Activate the telegraph for a certain amount of time
            m_laserTeleObj.SetActive(true);
            yield return new WaitForSeconds(m_telegraph);
            
            // Activate laser for specified interval
            m_laserObj.SetActive(true);
            m_laserTeleObj.SetActive(false);
            yield return new WaitForSeconds(m_uptime);

            // Deactivate laser for specified interval
            m_laserObj.SetActive(false);
            yield return new WaitForSeconds(m_downtime - m_telegraph);
        }
    }
}
