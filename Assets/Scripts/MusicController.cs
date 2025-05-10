using UnityEngine;

public class MusicController : MonoBehaviour {
    public AudioClip m_menuMusic;
    public AudioClip m_levelMusic;
    public AudioClip m_bossMusic;

    AudioSource m_audioSource;
    
    void Awake() {
        // make sure there's only ever one music object
        GameObject[] musics = GameObject.FindGameObjectsWithTag("Music");
        if (musics.Length > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);

        // normal setup
        m_audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void ChangeMusicTo(int index) {
        m_audioSource.Stop();
        if (index == 0) {
            m_audioSource.resource = m_menuMusic;
        } else if (index == 1) {
            m_audioSource.resource = m_levelMusic;
        } else if (index == 2) {
            m_audioSource.resource = m_bossMusic;
        }
        m_audioSource.Play();
    }
}
