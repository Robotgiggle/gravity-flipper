using UnityEngine;

public class PopupTriggerController : MonoBehaviour {
    public GameObject m_popup;

    void OnTriggerEnter2D() {
        m_popup.SetActive(true);
    }

    void OnTriggerExit2D() {
        if (m_popup == null) return;
        m_popup.SetActive(false);
    }
}
