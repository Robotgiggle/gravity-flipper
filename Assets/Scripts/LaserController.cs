using UnityEngine;
using System.Collections;

public class LaserController : MonoBehaviour
{
    public float m_uptime;
    public float m_downtime;
    public float m_offset;
    public float m_telegraph;
    
    GameManager m_gameManager;
    GameObject m_laserObj;
    GameObject m_laserTeleObj;
    Coroutine m_firingCoroutine;
    
    void Start() {
        m_gameManager = GameManager.TheInstance;
        m_laserObj = transform.GetChild(0).gameObject;
        m_laserTeleObj = transform.GetChild(1).gameObject;
        m_laserObj.SetActive(false);
        m_laserTeleObj.SetActive(false);
        m_gameManager.m_resetLevelEvent.AddListener(RestartFiring);
        
        m_firingCoroutine = StartCoroutine(FireLaser());
    }

    // this is used to reset the timing when the level resets
    void RestartFiring() {
        StopCoroutine(m_firingCoroutine);
        m_laserObj.SetActive(false);
        m_laserTeleObj.SetActive(false);
        m_firingCoroutine = StartCoroutine(FireLaser());
    }

    IEnumerator FireLaser() {
        // offset initial firing to allow for interesting timing
        yield return new WaitForSeconds(m_offset);

        while (true) {
            // activate the telegraph for a certain amount of time
            m_laserTeleObj.SetActive(true);
            yield return new WaitForSeconds(m_telegraph);
            
            // activate laser for specified interval
            m_laserObj.SetActive(true);
            m_laserTeleObj.SetActive(false);
            yield return new WaitForSeconds(m_uptime);

            // deactivate laser for specified interval
            m_laserObj.SetActive(false);
            yield return new WaitForSeconds(m_downtime - m_telegraph);
        }
    }
}
