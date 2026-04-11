using UnityEngine;

public class NavUtils : MonoBehaviour
{
    public static bool AreWeThereYet(UnityEngine.AI.NavMeshAgent mNavMeshAgent) {
    // Check if we've reached the destination
    // https://discussions.unity.com/t/how-can-i-tell-when-a-navmeshagent-has-reached-its-destination/52403
    if (!mNavMeshAgent.pathPending)
    {
        if (mNavMeshAgent.remainingDistance <= mNavMeshAgent.stoppingDistance)
        {
            if (!mNavMeshAgent.hasPath || mNavMeshAgent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }
    }
    return false;
    }

}
