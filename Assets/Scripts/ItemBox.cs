using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ItemBoxUI : MonoBehaviour
{
    private Animator anim;
    public float totalTime = 10f; // Total time to control the animation speed
    public float gamblingTime = 3.0f;

    private float gamblingTimer = 0f; // Internal timer to track elapsed time

    private float timer = 0f; // Internal timer to track elapsed time
    private bool isGambling = false; // Whether the animation is active
    private List<Sprite> itemsList;
    private int itemIndex = 0;

    // public int currentItemID = 0;

    private Image nextItem;

    private int lastId = 0;
    private Image currentItem;

    private GameObject player; // The player attached to an item box instance

    // public NetworkVariable<int> CurrentItemID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    // public NetworkVariable<bool> IsGambling = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    // Animator speed milestones
    private Dictionary<float, float> speedChanges = new Dictionary<float, float>
    {
        { 8f, 3f }, // At 8 seconds, speed is 3
        { 5f, 2f }, // At 5 seconds, speed is 2
        { 3f, 1f }, // At 3 seconds, speed is 1
        { 0f, 0f }  // At 0 seconds, set ItemActive to true
    };

    public void SetPlayer(GameObject playerObject)
    {
        player = playerObject;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        // isGambling = anim.GetBool("IsGambling");
        itemsList = new List<Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Items");
        itemsList.AddRange(sprites);
        Debug.Log($"Loaded {itemsList.Count} sprites for the ItemBox.");

        nextItem = transform.Find("ItemBoxBottom/ItemBox_Object_Next").GetComponent<Image>();
        currentItem = transform.Find("ItemBoxTop/ItemBox_Object_Current").GetComponent<Image>();

        if (itemsList.Count > 0)
        {
            nextItem.sprite = itemsList[(itemIndex + 1) % itemsList.Count];
            currentItem.sprite = itemsList[itemIndex % itemsList.Count];
        }
        anim.speed = 1f; // Default speed
    }

    void Update()
    {
        Debug.Log("Player is " + player);
        if (player != null)
        {
            isGambling = anim.GetBool("IsGambling");
            if (isGambling && itemsList.Count != 0)
                UpdateSprites();
            Debug.Log("timer " + timer + " " + totalTime + " " + gamblingTimer + " " + gamblingTime);
            if (isGambling && timer < totalTime && gamblingTimer < gamblingTime)
            {
                timer += Time.deltaTime;
                gamblingTimer += Time.deltaTime;
                HandleSpeedChanges();
            }
            else
            {
                anim.SetBool("IsGambling", false);
            }
        }
    }


    public void StartGambling(int id = 0)
    {
        lastId = id;
        if (!isGambling)
        {
            anim.SetBool("IsGambling", true);
            gamblingTimer = 0f;
        }
    }

    public bool isGamblingActive()
    {
        if (anim)
            return anim.GetBool("IsGambling");
        return false;
    }

    public bool isItemActive()
    {
        if (anim)
            return anim.GetBool("ItemActive");
        return false;
    }

    public void SetItemActive(bool value)
    {
        anim.SetBool("ItemActive", value);
    }

    private void UpdateSprites()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime % 1 >= 0.0f && stateInfo.normalizedTime % 1 < 0.5f && stateInfo.normalizedTime % 1 < 0.0f + Time.deltaTime)
        {
            itemIndex++;
            if (itemIndex >= itemsList.Count)
            {
                itemIndex = 0;
            }
            if (gamblingTime - gamblingTimer < 1f)
            {
                itemIndex = lastId;
            }

            currentItem.sprite = itemsList[(itemIndex + 1) % itemsList.Count];
            player.GetComponent<Player>().currentItemId = itemIndex;

        } else if (stateInfo.normalizedTime % 1 >= 0.5f && stateInfo.normalizedTime % 1 < 0.5f + Time.deltaTime)
        {
            itemIndex++;
            if (itemIndex >= itemsList.Count)
            {
                itemIndex = 0;
            }
            if (gamblingTime - gamblingTimer < 1f)
            {
                itemIndex = lastId;
            }

            nextItem.sprite = itemsList[(itemIndex + 1) % itemsList.Count];
            player.GetComponent<Player>().currentItemId = itemIndex;
        }
    }

    private void HandleSpeedChanges()
    {
        // Loop through milestones and check if the timer has reached any
        foreach (var milestone in speedChanges)
        {
            if (timer >= milestone.Key && milestone.Value != 0f)
            {
                anim.speed = milestone.Value; // Update speed
                Debug.Log($"Speed changed to {milestone.Value} at time {milestone.Key} seconds.");
                break;
            }
            else if (timer >= milestone.Key && milestone.Value == 0f)
            {
                // Set ItemActive to true when timer hits 0 milestone
                anim.SetBool("ItemActive", true);
                isGambling = false; // Disable further updates
                timer = 0f;
                break;
            }
        }
    }
}

