using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBox : MonoBehaviour
{
    Animator anim;
    public bool isActive = false; // Whether the animation is active
    public float totalTime = 10f; // Total time to control the animation speed
    private float timer = 0f; // Internal timer to track elapsed time
    public float targetAnimationTime = 0.5f;
    public float gamblingTime = 10.0f;

    private List<Sprite> itemsList;
    private int currentIndex = 0;

    private Image nextItem;

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
        itemsList = new List<Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Items");
        itemsList.AddRange(sprites);
        Debug.Log($"Loaded {itemsList.Count} sprites.");

        nextItem = transform.Find("ItemBoxBottom/ItemBox_Object_Next").GetComponent<Image>();

        if (itemsList.Count > 0)
        {
            nextItem.sprite = itemsList[(currentIndex + 1) % itemsList.Count];
        }
        anim.speed = 4f; // Default speed
    }

    void Update()
    {
        if (!isActive || itemsList.Count == 0) return;
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime % 1 >= targetAnimationTime && stateInfo.normalizedTime % 1 < targetAnimationTime + Time.deltaTime)
        {
            UpdateSprites();
        }
        if (isActive && timer < totalTime)
        {
            timer += Time.deltaTime;
            HandleSpeedChanges();
        }
    }

    private void UpdateSprites()
    {
        // Update the sprite index
        currentIndex++;
        if (currentIndex >= itemsList.Count)
        {
            currentIndex = 0;
        }

        nextItem.sprite = itemsList[(currentIndex + 1) % itemsList.Count];

        // Debug.Log($"Sprites updated to index {currentIndex}.");
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
                Debug.Log("ItemActive set to true.");
                isActive = false; // Disable further updates
                break;
            }
        }
    }
}

