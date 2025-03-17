using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public int current_level_collectables;

    private int[] max_collectables = new int[3];
    private int[] acheived_collectables = new int[3];

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        current_level_collectables = 0;
        max_collectables = new int[] { 0, 1, 2 }; //change to reflect how many collectables are in each scene 
    }

    public void storeCollectableCount(int level_id)
    {
        acheived_collectables[level_id] = current_level_collectables;
    }
}
