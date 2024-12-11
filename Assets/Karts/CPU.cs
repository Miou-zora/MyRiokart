using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KartGame.KartSystems;
using Unity.Mathematics;
using System;
using Unity.Netcode;

public class CPU : NetworkBehaviour
{
    GameObject[] waypoints;

    int currentWaypoint = 0;

    private ArcadeKart kart;

    private Vector3 target;

    private float lastX = 0;
    private float lastZ = 0;
    // Start is called before the first frame update
    void Start()
    {
        kart = gameObject.GetComponent<ArcadeKart>();
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        // Sort waypoints by parent name

        System.Array.Sort(waypoints, (x, y) => x.transform.parent.name.CompareTo(y.transform.parent.name));

        for (int i = 0; i < waypoints.Length; i++)
        {
            Debug.Log(waypoints[i].transform.parent.name);
        }

        AddRandomTarget();
    }

    void AddRandomTarget()
    {
        // Obtenir le waypoint actuel
        GameObject waypoint = waypoints[currentWaypoint];

        // Vérifier si le waypoint a un BoxCollider
        BoxCollider boxCollider = waypoint.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            // Obtenir les dimensions locales du BoxCollider
            Vector3 size = boxCollider.size;
            Vector3 center = boxCollider.center;

            // Calculer un point aléatoire dans les limites du BoxCollider
            float randomX = UnityEngine.Random.Range(-size.x / 2, size.x / 2);
            float randomY = 0;
            float randomZ = UnityEngine.Random.Range(-size.z / 2, size.z / 2);
            // make it closer to last point
            if (lastX != 0 && lastZ != 0)
            {
                randomX = Mathf.Lerp(randomX, lastX, 0.75f);
                randomZ = Mathf.Lerp(randomX, lastX, 0.75f);
            }

            lastX = randomX;
            lastZ = randomZ;
            // Convertir le point aléatoire en coordonnées mondiales
            target = waypoint.transform.TransformPoint(center + new Vector3(randomX, randomY, randomZ));
        }
        else
        {
            Debug.LogWarning("Le waypoint n'a pas de BoxCollider !");
            target = waypoint.transform.position; // Utiliser la position par défaut
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer)
        {
            return;
        }
        // print waypoint current waypoint parent name
        InputData input = new InputData();
        input.Accelerate = true;


        // add turn input between -1 and 1 to go to target

        Vector3 direction = target - transform.position;
        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        float desiredTurnInput = Mathf.Clamp(angle / 180f, -1f, 1f);
        desiredTurnInput *= 3;

        Debug.Log(Mathf.Abs(input.TurnInput - desiredTurnInput));
        if (Mathf.Abs(input.TurnInput - desiredTurnInput) < 0.25)
        {
            input.TurnInput = Mathf.Lerp(input.TurnInput, desiredTurnInput, Time.deltaTime * 150);
        } else
        {
            input.TurnInput = desiredTurnInput;
        }

        if (Mathf.Abs(input.TurnInput) < 0.1)
        {
            input.TurnInput = 0;
        }

        // if the target is behind stop accelerating
        if (angle > 90 || angle < -90)
        {
            input.Accelerate = false;
        }
        

        // Show the target
        Debug.DrawLine(transform.position, target, Color.red);
        kart.Input = input;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Waypoint"))
        {
            if (other.gameObject != waypoints[currentWaypoint])
            {
                return;
            }
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }
            AddRandomTarget();
        }
    }
}
