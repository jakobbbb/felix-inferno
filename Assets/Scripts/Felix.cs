using UnityEngine;
using UnityEngine.AI;

public class Felix : MonoBehaviour {

    private NavMeshAgent m_Agent;

    public void Start() {
        Debug.Log($"Hello, I am {name}!");
        m_Agent = GetComponent<NavMeshAgent>();
    }

    public void SetTarget() {
        var p = RayUtils.CastMouseRayToWorld();
        var target = RayUtils.CastRayDown(p);

        m_Agent.destination = target;
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.purple;
        Gizmos.DrawSphere(transform.position, 1.2f);
        Gizmos.DrawSphere(m_Agent.destination, 0.8f);
    }

}
