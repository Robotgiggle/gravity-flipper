using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DoorController : MonoBehaviour {
    public AudioClip m_unlockSound;
    public AudioClip m_teleportSound;
    public Sprite[] m_sprites = new Sprite[2];
    public string m_nextScene;
    public bool m_open = false;

    AudioSource m_audioSource;
    SpriteRenderer m_renderer;
    SwitchController[] m_switchCons;
    GameObject resourceManager;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        resourceManager = GameObject.Find("ResourceManager");

        m_audioSource = gameObject.GetComponent<AudioSource>();
        m_renderer = gameObject.GetComponent<SpriteRenderer>();
        GameObject[] switches = GameObject.FindGameObjectsWithTag("Switch");
        m_switchCons = new SwitchController[switches.Length];
        for (int i = 0; i < switches.Length; i++) {
            m_switchCons[i] = switches[i].GetComponent<SwitchController>();
        }
    }

    // when colliding with the player, move to the next level if the door is open
    void OnTriggerEnter2D(Collider2D coll) {
        if (m_open) {
            coll.gameObject.SetActive(false);
            StartCoroutine(LevelTransition());
        }
    }

    // play sound, change scenes once it's done
    IEnumerator LevelTransition() {
        m_audioSource.PlayOneShot(m_teleportSound);
        ScoreTracker.timer += Time.timeSinceLevelLoad;
        yield return new WaitForSeconds(m_teleportSound.length);
        resourceManager.GetComponent<ResourceManager>().storeCollectableCount(SceneManager.GetActiveScene().buildIndex - 1);
        SceneManager.LoadScene(m_nextScene);
    }

    // open the door if all the switches are active; called whenever a switch is flipped
    public void CheckSwitches() {
        foreach (SwitchController switchCon in m_switchCons) {
            if (switchCon.m_active == false) return;
        }
        m_audioSource.PlayOneShot(m_unlockSound);
        m_renderer.sprite = m_sprites[1];
        m_open = true;
    }
}
