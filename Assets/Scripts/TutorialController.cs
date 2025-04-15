using UnityEngine;

public class TutorialController : MonoBehaviour {
    public GameObject[] m_popups = new GameObject[4];
    public PauseMenuController m_PMC;
    
    GameManager m_gameManager;
    
    void Start() {
        m_gameManager = GameManager.TheInstance;
    }

    public void ShowPopup(int index) {
        StartCoroutine(m_PMC.FadeDarkness(true));
        m_gameManager.m_inputsLocked = true;
        m_popups[index].SetActive(true);
    }

    public void HidePopup(int index) {
        StartCoroutine(m_PMC.FadeDarkness(false));
        m_gameManager.m_inputsLocked = false;
        m_popups[index].SetActive(false);
    }
}
