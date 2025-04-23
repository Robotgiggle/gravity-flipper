using UnityEngine;

public class TutorialController : MonoBehaviour {
    public GameObject[] m_popups = new GameObject[4];
    public PauseMenuController m_PMC;
    
    GameManager m_gameManager;
    int m_activePopup = -1;
    
    void Start() {
        m_gameManager = GameManager.TheInstance;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && m_gameManager.m_inputsLocked) {
            HidePopup();
        }
    }

    public void ShowPopup(int index) {
        StartCoroutine(m_PMC.FadeDarkness(true));
        m_gameManager.m_inputsLocked = true;
        m_popups[index].SetActive(true);
        m_activePopup = index;
    }

    public void HidePopup() {
        StartCoroutine(m_PMC.FadeDarkness(false));
        m_gameManager.m_inputsLocked = false;
        m_popups[m_activePopup].SetActive(false);
        m_activePopup = -1;
    }
}
