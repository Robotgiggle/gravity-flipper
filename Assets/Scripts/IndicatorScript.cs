using UnityEngine;

public class IndicatorScript : MonoBehaviour
{
    GameObject player;
    public GameObject[] m_indicators = new GameObject[4];
    PlayerController playerScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);

        string[] directions = {"up", "down", "left", "right"};
        for(int i = 0; i<4; i++){
            if(playerScript.CanFlip(directions[i])){
                m_indicators[i].SetActive(true);
            }
            else{
                m_indicators[i].SetActive(false);
            }
        }
    }
}
