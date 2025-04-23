using UnityEngine;

public class MusicController : MonoBehaviour {
    void Awake() {
        // make sure there's only ever one music object
        GameObject[] musics = GameObject.FindGameObjectsWithTag("Music");
        if (musics.Length > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);
    }
}
