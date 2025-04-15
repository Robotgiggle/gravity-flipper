using UnityEngine;

public class PopupTriggerController : MonoBehaviour {
    public TutorialController m_tutorialController;
    public int m_index;

    bool m_activated;

    void OnTriggerEnter2D() {
        if (!m_activated) {
            m_tutorialController.ShowPopup(m_index);
            m_activated = true;
        }
    }
}
