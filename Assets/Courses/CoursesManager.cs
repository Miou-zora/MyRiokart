using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoursesManager : MonoBehaviour
{
    public static CoursesManager Instance { get; private set; }
    public GameObject startingPositions;
    public List<GameObject> players;
    public float maxStartTime = 4.0f;
    public float startTimeElapsed = 0.0f;
    public float timeElapsed = 0.0f;
    public bool gameStarted = false;

    private void Awake()
    {
        // Ensure only one instance of CoursesManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayers();
    }

    void SpawnPlayers()
    {
        if (startingPositions.transform.childCount < players.Count)
        {
            Debug.LogError("Not enough starting positions for all players");
            return;
        }
        for (int i = 0; i < players.Count; i++)
        {
            players[i].transform.position = startingPositions.transform.GetChild(i).position;
        }
    }
    
    void Update()
    {
        if (!gameStarted) {
            // print each seconds elapsed
            float interval = startTimeElapsed;
            startTimeElapsed += Time.deltaTime;
            if (startTimeElapsed >= maxStartTime) {
                gameStarted = true;
                Debug.Log("Game started!");
                // Start the game (enable car movement)
            } else {
                if (Mathf.Floor(startTimeElapsed) > Mathf.Floor(interval)) {
                    Debug.Log("Time elapsed: " + (4 - Mathf.Floor(startTimeElapsed)));
                }
                return;
            }
        }
        timeElapsed += Time.deltaTime;
    }
}
