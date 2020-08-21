using UnityEngine;

public class PatrollerVision : MonoBehaviour
{
    public float viewDistance;
    public float viewAngle;
    public LayerMask layerMask;

    private Transform m_PlayerTransform;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag(Constants.PLAYER_TAG);
        m_PlayerTransform = playerObject.transform;
    }

    // Update is called once per frame
    public bool IsPlayerInVision()
    {
        Vector3 toPlayerVector = m_PlayerTransform.position - transform.position;
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
                if (Physics.Raycast(transform.position, toPlayerVector, out hitInfo, viewDistance, layerMask))
                {
                    if (hitInfo.collider.gameObject.tag == Constants.PLAYER_TAG)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
