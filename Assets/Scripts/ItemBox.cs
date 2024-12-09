using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.UI;

public class ItemBoxUI : MonoBehaviour
{
    Animator anim;
    public float totalTime = 10f; // Total time to control the animation speed
    public float gamblingTime = 3.0f;

    private float gamblingTimer = 0f; // Internal timer to track elapsed time

    private float timer = 0f; // Internal timer to track elapsed time
    private bool isGambling = false; // Whether the animation is active
    private List<Sprite> itemsList;
    private int itemIndex = 0;

    public int currentItemID = 0;

    private Image nextItem;

    private int lastId = 0;
    private Image currentItem;

    // Animator speed milestones
    private Dictionary<float, float> speedChanges = new Dictionary<float, float>
    {
        { 8f, 3f }, // At 8 seconds, speed is 3
        { 5f, 2f }, // At 5 seconds, speed is 2
        { 3f, 1f }, // At 3 seconds, speed is 1
        { 0f, 0f }  // At 0 seconds, set ItemActive to true
    };

    void Start()
    {
        anim = GetComponent<Animator>();
        isGambling = anim.GetBool("IsGambling");
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
        isGambling = anim.GetBool("IsGambling");
        if (isGambling && itemsList.Count != 0)
            UpdateSprites();
        if (isGambling && timer < totalTime && gamblingTimer < gamblingTime) {
            timer += Time.deltaTime;
            gamblingTimer += Time.deltaTime;
            HandleSpeedChanges();
            Debug.Log($"Timer: {gamblingTimer} seconds and time: {gamblingTime}.");
        } else {
            anim.SetBool("IsGambling", false);
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
        return isGambling;
    }

    public bool isItemActive()
    {
        return anim.GetBool("ItemActive");
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
            if (gamblingTime - gamblingTimer < 0.6f)
            {
                itemIndex = lastId;
            }

            currentItem.sprite = itemsList[(itemIndex + 1) % itemsList.Count];
            currentItemID = itemIndex;

        } else if (stateInfo.normalizedTime % 1 >= 0.5f && stateInfo.normalizedTime % 1 < 0.5f + Time.deltaTime)
        {
            itemIndex++;
            if (itemIndex >= itemsList.Count)
            {
                itemIndex = 0;
            }
            if (gamblingTime - gamblingTimer < 0.6f)
            {
                itemIndex = lastId;
            }

            nextItem.sprite = itemsList[(itemIndex + 1) % itemsList.Count];
            currentItemID = itemIndex;
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

