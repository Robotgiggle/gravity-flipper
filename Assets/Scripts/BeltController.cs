using UnityEngine;
using System.Collections.Generic;

public class BeltController : MonoBehaviour {
    public GameObject m_beltLight;
    public float m_lightSpeed;
    public bool m_vertical;

    List<GameObject> m_activeLights;
    Vector3 m_startPos;
    float m_maxDist;
    float m_timer;
    
    void Start() {
        m_activeLights = new List<GameObject>();
        float offset;
        if (m_vertical) offset = GetComponent<SpriteRenderer>().bounds.size.y;
        else offset = GetComponent<SpriteRenderer>().bounds.size.x;
        m_startPos = transform.position + transform.right * (-offset/2 + 0.19f);
        m_maxDist = offset - 0.38f;
        
        Vector3 spawnPos = m_startPos;
        while (m_lightSpeed != 0) {
            GameObject newLight = Instantiate(m_beltLight, spawnPos, Quaternion.identity);
            newLight.transform.SetParent(transform);
            newLight.GetComponent<SpriteRenderer>().color = new Color(1, 0.9f, 0);
            m_activeLights.Add(newLight);

            spawnPos += transform.right * m_lightSpeed * 0.3f;
            if (Vector3.Distance(spawnPos, m_startPos) > m_maxDist) break;
        }
    }

    void Update() {
        // increment spawn timer
        m_timer += Time.deltaTime;

        // move existing lights
        for (int i = m_activeLights.Count; i > 0; i--) {
            GameObject light = m_activeLights[i-1];
            if (Vector3.Distance(light.transform.position, m_startPos) > m_maxDist) {
                m_activeLights.RemoveAt(i-1);
                Destroy(light);
            } else {
                light.transform.Translate(transform.right * m_lightSpeed * Time.deltaTime);
            }
        }

        // spawn a new light
        if (m_timer > 0.3) {
            m_timer = 0;
            GameObject newLight = Instantiate(m_beltLight, m_startPos, Quaternion.identity);
            newLight.transform.SetParent(transform);
            newLight.GetComponent<SpriteRenderer>().color = new Color(1, 0.9f, 0);
            m_activeLights.Add(newLight);
        }
    }
}
