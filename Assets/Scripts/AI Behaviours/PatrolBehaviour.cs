using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PatrolBehaviour : MonoBehaviour
{
    public delegate void PlayerFoundEvent();
    public event PlayerFoundEvent OnPlayerFoundEvent;

    public float moveSpeed = 2f;
    public float angularSpeed = 270f;

    [SerializeField]
    private Vector3 m_CurrentWaypoint;
    private List<Vector3> m_Waypoints;
    private NavMeshAgent m_Agent;
    private PatrollerVision m_Vision;

    void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Vision = GetComponent<PatrollerVision>();
    }

    void Start()
    {
        // Initialization
        m_Waypoints = new List<Vector3>();

        // Populate waypoint list
        SetupWaypoints();
        SetRandomWaypoint();
    }

    private void OnEnable()
    {
        m_Agent.speed = moveSpeed;
        m_Agent.angularSpeed = angularSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Agent.remainingDistance < Constants.ARRIVAL_DISTANCE)
        {
            // If arrived, set a new waypoint
            SetRandomWaypoint();
        }

        bool isPlayerFound = m_Vision.IsPlayerInVision();
        if (isPlayerFound)
        {
            OnPlayerFoundEvent?.Invoke();
        }
    }

    private void SetupWaypoints()
    {
        GameObject[] wps = GameObject.FindGameObjectsWithTag(Constants.WAYPOINT_TAG);
        foreach (GameObject wp in wps)
        {
            m_Waypoints.Add(wp.transform.position);
        }
    }

    private void SetRandomWaypoint()
    {
        int randomIndex = Random.Range(0, m_Waypoints.Count);
        m_CurrentWaypoint = m_Waypoints[randomIndex];
        m_Agent.SetDestination(m_CurrentWaypoint);
    }
}
