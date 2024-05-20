using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class RaceCar : MonoBehaviour
{
    public Transform[] waypoints; // Array to store waypoint transforms
    private int currentWaypointIndex = 0; // Index of the current waypoint
    private NavMeshAgent navAgent;

    public float maxSpeed = 15.0f; // Meters per second (adjustable)
    public float waypointThreshold = 0.5f; // Distance threshold to consider the waypoint reached

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();

        // Find all game objects with the "Target" tag and sort them by name
        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag("Target");
        waypoints = waypointObjects.OrderBy(obj => obj.name).Select(obj => obj.transform).ToArray();

        // Set the initial destination
        if (waypoints.Length > 0)
        {
            navAgent.SetDestination(waypoints[currentWaypointIndex].position);
            navAgent.speed = maxSpeed; // Set the agent speed
            navAgent.autoBraking = false; // Disable auto braking to ensure continuous movement
        }
    }

    void Update()
    {
        // Check if the car has reached the current waypoint
        if (!navAgent.pathPending && navAgent.remainingDistance < waypointThreshold)
        {
            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            navAgent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }
}
