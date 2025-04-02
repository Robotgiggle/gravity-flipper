using UnityEngine;
using UnityEngine.UI;

public class LoopingBackground : MonoBehaviour
{
    public Material m_material;
    public float m_scrollSpeed = 0.05f;

    GameManager m_gameManager;

    void Start() {
        m_gameManager = GameManager.TheInstance;
    }

    void Update() {
        if (m_gameManager.m_scrollBG) {
            float offset = Time.deltaTime * m_scrollSpeed;
            m_material.mainTextureOffset += new Vector2(offset, 0);
        }
    }
}
