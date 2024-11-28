using UnityEngine;
using TMPro;

public class LapCounterUI : MonoBehaviour
{
    public TextMeshProUGUI lapCounter;
    public GameObject player;

    private int lastLapCheck = 0;

    // Start is called before the first frame update
    void Start()
    {
        lapCounter.text = "Lap: " + (player.GetComponent<Player>().currentLap + 1).ToString() + "/3";
    }

    // Update is called once per frame
    void Update()
    {
        if (lastLapCheck != player.GetComponent<Player>().currentLap)
        {
            lapCounter.text = "Lap: " + (player.GetComponent<Player>().currentLap + 1).ToString() + "/3";
            lastLapCheck = player.GetComponent<Player>().currentLap;
        }
    }
}
