using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {
    public AudioClip m_menuMusic;
    public AudioClip m_levelMusic;
    public AudioClip m_bossMusic;

    public float Volume {
        get { return m_trueVolume; }
        set { m_trueVolume = value; m_audioSource.volume = value; }
    }

    AudioSource m_audioSource;
    float m_trueVolume = 1;
    
    void Awake() {
        // make sure there's only ever one music object
        GameObject[] musics = GameObject.FindGameObjectsWithTag("Music");
        if (musics.Length > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);

        // normal setup
        m_audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void ChangeMusicTo(string which, bool fade) {
        if (which == "Menu") {
            if (m_audioSource.clip == m_menuMusic) return;
            else if (fade) StartCoroutine(CrossFadeTo(m_menuMusic));
            else StartCoroutine(ChangeTo(m_menuMusic));
        } else if (which == "Level") {
            if (m_audioSource.clip == m_levelMusic) return;
            else if (fade) StartCoroutine(CrossFadeTo(m_levelMusic));
            else StartCoroutine(ChangeTo(m_levelMusic));
        } else if (which == "Boss") {
            if (m_audioSource.clip == m_bossMusic) return;
            else if (fade) StartCoroutine(CrossFadeTo(m_bossMusic));
            else StartCoroutine(ChangeTo(m_bossMusic));
        }
    }

    IEnumerator ChangeTo(AudioClip newClip) {
        // change to new audio clip
        m_audioSource.Stop();
        m_audioSource.clip = newClip;
        m_audioSource.Play();

        // set volume to 40% of current value, then scale back up
        float vol = m_trueVolume * 0.4f;
        while (vol < 0.98f) {
            vol = Mathf.Lerp(vol, 1f, Time.deltaTime);
            m_audioSource.volume = Mathf.Lerp(0f, m_trueVolume, vol);
            yield return null;
        }
        m_audioSource.volume = m_trueVolume;
    }

    IEnumerator CrossFadeTo(AudioClip newClip) {
        // copy current audio parameters into new audiosource
        AudioSource fadeOutSource = gameObject.AddComponent<AudioSource>();
        fadeOutSource.clip = m_audioSource.clip;
        fadeOutSource.time = m_audioSource.time;
        fadeOutSource.volume = m_audioSource.volume;
        fadeOutSource.Play();

        // set original audiosource to new clip at 0% volume
        m_audioSource.volume = 0f;
        m_audioSource.clip = newClip;
        m_audioSource.Play();

        // fade in updated original audiosource while fading out new audiosource with old clip
        float t = 0;
        while (t < 0.98f) {
            t = Mathf.Lerp(t, 1f, Time.deltaTime);
            fadeOutSource.volume = Mathf.Lerp(m_trueVolume, 0f, t);
            m_audioSource.volume = Mathf.Lerp(0f, m_trueVolume, t);
            yield return null;
        }
        m_audioSource.volume = m_trueVolume;

        //destroy the fading audiosource
        Destroy(fadeOutSource);
    }
}
