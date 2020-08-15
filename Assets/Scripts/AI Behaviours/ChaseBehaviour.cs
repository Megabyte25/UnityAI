using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : MonoBehaviour
{
    public delegate void PlayerLostEvent();
    public event PlayerLostEvent OnPlayerLostEvent;

    public float viewDistance;
    public float viewAngle;
    public float moveSpeed = 5f;
    public float angularSpeed = 360f;

    public GameObject alertLights; // Assigned in Editor
    private GameObject m_Player;
    private NavMeshAgent m_Agent;
    private bool m_pendingReset = false;

    private void Awake()
    {
        m_Player = GameObject.FindGameObjectWithTag(Constants.PLAYER_TAG);
        m_Agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        m_pendingReset = true;
        alertLights.SetActive(true);
    }

    private void OnDisable()
    {
        alertLights.SetActive(false);
    }

    void Update()
    {
        if (m_pendingReset)
        {
            m_pendingReset = false;
            m_Agent.speed = moveSpeed;
            m_Agent.angularSpeed = angularSpeed;
        }

        m_Agent.SetDestination(m_Player.transform.position);
    }
}
