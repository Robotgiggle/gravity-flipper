using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float m_followSpeed;
    public float m_yOffset;
    public Transform m_target;

    // Update is called once per frame
    void Update() {
        Vector3 newPos = new Vector3(m_target.position.x, m_target.position.y + m_yOffset, -10.0f);
        if ((newPos - transform.position).sqrMagnitude < 0.05) return;
        transform.position = Vector3.Lerp(transform.position, newPos, m_followSpeed * Time.deltaTime);
    }
}
