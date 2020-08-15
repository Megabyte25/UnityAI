using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class ClickDestination : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Get mouse position in screen space
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector3 mousePositionVec3 = new Vector3(mousePosition.x, mousePosition.y, 0f);

            // Perform a raycast in world space and check what the ray hits
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePositionVec3), out hit, 100f))
            {
                // Set that raycast hit point as the next destination for the AI agent
                agent.SetDestination(hit.point);
            }
        }
    }
}
