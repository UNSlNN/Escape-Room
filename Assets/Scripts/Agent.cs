using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    [Header("WayPoint")]
    public List<Transform> waypoints = new List<Transform>();
    public float radius = 1.5f;
    private NavMeshAgent agent;
    private Transform target;
    private Transform playerTarget = null;
    private int targetWayPointIndex;
    private float minDistance = 0.1f;
    private int lastWayPointIndex;

    [Header("LightSystem")]
    public Light[] findLight;
    public List<Light> lights = new List<Light>();
    private List<Transform> targetLight = new List<Transform>();    // Light turn to transform, and Get to list
    public bool isLightOff = false;                               // Check if any light is off
    public bool isPlayerFound = false;            // Flag to track if the player is found
    void Start()
    {
        findLight = FindObjectsOfType<Light>();                     // Find light objects in scene
        playerTarget = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        target = waypoints[targetWayPointIndex];                    // waypoint position
        lastWayPointIndex = waypoints.Count - 1;
        ShuffleWaypoints();                                         // Random waypoint in first time

        lights.AddRange(findLight);                                 // Store light objects to list when start
    }

    void Update()
    {
        // Raycast for check player has passed thorough
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitinfo, 15f))
        {
            if (hitinfo.collider.gameObject.name == "Player")
            {
                isPlayerFound = true;
                //Debug.Log("Player has found!");
            }
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitinfo.distance, Color.red);
        }
        else
        {
            isPlayerFound = false;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10f, Color.green);
        }

        foreach (Light light in lights) // Turn Light Info
        {
            if (isPlayerFound)
            {
                agent.SetDestination(playerTarget.position);
            }
            else{
                if (!light.enabled)
                {
                    targetLight.Add(light.transform);               // Find the light tranform, Which one is turn off
                    isLightOff = true;
                }

                float playerDistance = Vector3.Distance(playerTarget.position, transform.position); // Check if player near in agent sight
                if (isLightOff)
                {
                    foreach (Transform t in targetLight)
                    {
                        float distanceToLight = Vector3.Distance(agent.transform.position, t.position);
                        if (distanceToLight <= 1f)
                        {
                            light.enabled = true;
                            isLightOff = false;
                        }
                        agent.SetDestination(t.position);           // Follow the Light position
                    }
                }
                else if (playerDistance < radius)
                {
                    agent.SetDestination(playerTarget.position);
                }
                else
                {
                    float distance = Vector3.Distance(transform.position, target.position); // Agent's distance
                    CheckDistanceTowayPoint(distance);

                    target = waypoints[targetWayPointIndex];
                    agent.SetDestination(target.position);          // Follow the waypoint position
                }
            }
        }
    }
    void CheckDistanceTowayPoint(float cuurenDistance)              // Continue waypoint
    {
        if (cuurenDistance <= minDistance)
        {
            targetWayPointIndex++;
            UpdateTargetWayPoint();
        }
    }
    void UpdateTargetWayPoint()                                     // Loop waypoint 
    {
        if (targetWayPointIndex > lastWayPointIndex)
        {
            targetWayPointIndex = 0;
        }
        target = waypoints[targetWayPointIndex];

    }
    void ShuffleWaypoints()                                         // randomly the waypoints List
    {
        for (int i = waypoints.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Transform terandomTarget = waypoints[i];
            waypoints[i] = waypoints[randomIndex];
            waypoints[randomIndex] = terandomTarget;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
