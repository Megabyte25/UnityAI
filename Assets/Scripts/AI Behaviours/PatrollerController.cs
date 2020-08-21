using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollerController : MonoBehaviour
{
    // Behaviours
    PatrolBehaviour patrolBehaviour;
    ChaseBehaviour chaseBehaviour;

    private void Awake()
    {
        patrolBehaviour = GetComponent<PatrolBehaviour>();
        chaseBehaviour = GetComponent<ChaseBehaviour>();
        patrolBehaviour.enabled = true;
        chaseBehaviour.enabled = false;
    }

    private void OnEnable()
    {
        patrolBehaviour.OnPlayerFoundEvent += PlayerFound;
        chaseBehaviour.OnPlayerLostEvent += PlayerLost;
    }

    private void OnDisable()
    {
        patrolBehaviour.OnPlayerFoundEvent -= PlayerFound;
        chaseBehaviour.OnPlayerLostEvent -= PlayerLost;
    }

    public void PlayerFound()
    {
        // Alert all patrollers
        GameObject[] allPatrollers = GameObject.FindGameObjectsWithTag(Constants.PATROLLER_TAG);
        foreach (GameObject obj in allPatrollers)
        {
            PatrollerController pc = obj.GetComponent<PatrollerController>();
            pc.SwitchToChaseMode();
        }
    }

    public void PlayerLost()
    {
        // Signal all patrollers
        GameObject[] allPatrollers = GameObject.FindGameObjectsWithTag(Constants.PATROLLER_TAG);
        foreach (GameObject obj in allPatrollers)
        {
            PatrollerController pc = obj.GetComponent<PatrollerController>();
            pc.SwitchToPatrolMode();
        }
    }

    public void SwitchToChaseMode()
    {
        patrolBehaviour.enabled = false;
        chaseBehaviour.enabled = true;
    }

    public void SwitchToPatrolMode()
    {
        patrolBehaviour.enabled = true;
        chaseBehaviour.enabled = false;
    }
}
