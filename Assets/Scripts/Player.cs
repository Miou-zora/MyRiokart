using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public int currentCheckpoint = 0;
    public int currentLap = 0;
    public int currentItemId = 0;

    private GameObject player;
    private LapCounterUI lapCounterUiPrefab;
    private ItemBoxUI itemBoxUiPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure this runs only for the local player.
        if (!IsLocalPlayer) return;

        player = gameObject;

        // Find the Canvas in the scene
        Canvas raceUi = GameObject.FindGameObjectWithTag("GameUI").GetComponent<Canvas>();
        if (raceUi == null)
        {
            Debug.LogError("Race_UI Canvas not found!");
            return;
        }

        // Load LapCounterUI prefab
        lapCounterUiPrefab = Resources.Load<LapCounterUI>("UI/LapCount");
        if (lapCounterUiPrefab != null)
        {
            // Instantiate LapCounterUI
            LapCounterUI localLapCounter = Instantiate(lapCounterUiPrefab, raceUi.transform); // Parent directly to Canvas
            localLapCounter.SetPlayer(player); // Set player reference
        }
        else
        {
            Debug.LogError("LapCounterUI prefab could not be loaded!");
        }

        itemBoxUiPrefab = Resources.Load<ItemBoxUI>("UI/ItemBoxUi Variant");
        if (itemBoxUiPrefab != null)
        {
            // Instantiate the ItemBoxUI for the local player only
            ItemBoxUI localItemBox = Instantiate(itemBoxUiPrefab, raceUi.transform);
            localItemBox.SetPlayer(player);
        }
        else
        {
            Debug.LogError("ItemBoxUI prefab could not be loaded!");
            return;
        }

    }
}
