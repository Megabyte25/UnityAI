using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : MonoBehaviour
{
    public delegate void PlayerLostEvent();
    public event PlayerLostEvent OnPlayerLostEvent;

    public float moveSpeed = 5f;
    public float angularSpeed = 360f;

    public GameObject alertLights; // Assigned in Editor
    private GameObject m_Player;
    private NavMeshAgent m_Agent;

    private void Awake()
    {
        m_Player = GameObject.FindGameObjectWithTag(Constants.PLAYER_TAG);
        m_Agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        SetAgentSpeed();
        alertLights.SetActive(true);
    }

    private void OnDisable()
    {
        alertLights.SetActive(false);
    }

    void Update()
    {
        m_Agent.SetDestination(m_Player.transform.position);
    }

    void SetAgentSpeed()
    {
        m_Agent.speed = moveSpeed;
        m_Agent.angularSpeed = angularSpeed;
    }
}
