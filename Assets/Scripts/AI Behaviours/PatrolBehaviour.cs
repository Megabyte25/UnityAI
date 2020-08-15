using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PatrolBehaviour : MonoBehaviour
{
    public delegate void PlayerFoundEvent();
    public event PlayerFoundEvent OnPlayerFoundEvent;

    public float viewDistance;
    public float viewAngle;
    public float moveSpeed = 2f;
    public float angularSpeed = 270f;

    [SerializeField]
    private Vector3 m_CurrentWaypoint;
    private List<Vector3> m_Waypoints;
    private NavMeshAgent m_Agent;
    private GameObject m_Player;
    private bool m_pendingReset = false;

    void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        // Initialization
        m_Waypoints = new List<Vector3>();
        m_Player = GameObject.FindGameObjectWithTag(Constants.PLAYER_TAG);

        // Populate waypoint list
        SetupWaypoints();
    }

    private void OnEnable()
    {
        m_pendingReset = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_pendingReset)
        {
            m_pendingReset = false;
            m_Agent.speed = moveSpeed;
            m_Agent.angularSpeed = angularSpeed;
            SetRandomWaypoint();
        }

        if (m_Agent.remainingDistance < Constants.ARRIVAL_DISTANCE)
        {
            // If arrived, set a new waypoint
            SetRandomWaypoint();
        }

        CheckForPlayer();
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

    private void CheckForPlayer()
    {
        Vector3 toPlayerVector = m_Player.transform.position - transform.position;
        float distanceFromPlayer = toPlayerVector.magnitude;
        
        // Is player within view distance?
        if (distanceFromPlayer < viewDistance)
        {
            // Is player within view angle?
            float angle = Vector3.Angle(transform.forward, toPlayerVector);
            if (angle < (viewAngle / 2))
            {
                // Is player in line of sight?
                RaycastHit hitInfo;
                if (Physics.Raycast(transform.position, toPlayerVector, out hitInfo, viewDistance))
                {
                    if (hitInfo.collider.gameObject.tag == Constants.PLAYER_TAG)
                    {
                        OnPlayerFoundEvent?.Invoke();
                    }
                }
            }
        }
    }
}
