using UnityEngine;

public class PopupTriggerController : MonoBehaviour {
    public GameObject m_popup;

    void OnTriggerEnter2D() {
        m_popup.SetActive(true);
    }

    void OnTriggerExit2D() {
        m_popup.SetActive(false);
    }
}
