using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class KeyBinderController : MonoBehaviour {
    public bool m_active;
    public int m_index;

    GameManager m_gameManager;
    Image m_image;
    TMP_Text m_text;
    string m_boundKey;
    
    void Start() {
        m_gameManager = GameManager.TheInstance;
        m_image = gameObject.GetComponent<Image>();
        m_text = gameObject.GetComponentInChildren<TMP_Text>();
        m_text.text = m_gameManager.m_keyBinds[m_index].ToString();
    }

    void Update() {
        if (Input.anyKeyDown && m_active) {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode))) {
                if (Input.GetKeyDown(key)) {
                    m_gameManager.m_keyBinds[m_index] = key;
                    m_boundKey = key.ToString();
                    m_text.text = key.ToString();
                    ResetStyle();
                    m_active = false;
                }
            }
        }
    }

    public void ToggleActive() {
        if (m_active) {
            m_text.text = m_boundKey;
            ResetStyle();
            m_active = false;
        } else {
            m_text.text = "[Press a key]";
            m_text.fontStyle = FontStyles.Bold;
            m_text.fontSize = 18;
            m_image.color = new Color(0.23f, 0.54f, 1f);
            m_active = true;
        }
    }

    void ResetStyle() {
        m_text.fontStyle = FontStyles.Normal;
        m_text.fontSize = 20;
        m_image.color = new Color(0.34f, 0.58f, 0.87f);
    }
}
