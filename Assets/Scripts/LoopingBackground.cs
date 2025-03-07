using UnityEngine;
using UnityEngine.UI;

public class LoopingBackground : MonoBehaviour
{
    public Material m_material;
    public float m_scrollSpeed = 0.05f;

    void Update()
    {
        float offset = Time.time * m_scrollSpeed;
        m_material.mainTextureOffset = new Vector2(offset, 0);
    }
}
