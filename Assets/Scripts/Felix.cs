using UnityEngine;
using UnityEngine.AI;

public class Felix : MonoBehaviour {

    private NavMeshAgent m_Agent;

    public void Start() {
        Debug.Log($"Hello, I am {name}!");
        m_Agent = GetComponent<NavMeshAgent>();
    }

    public void Update() {
        SetTarget();
    }

    public void SetTarget() {
        var p = RayUtils.CastMouseRayToWorld();
        var target = RayUtils.CastRayDown(p + Vector3.up);

        m_Agent.destination = target;
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.purple;
        Gizmos.DrawSphere(transform.position, 1.2f);

        Gizmos.color = Color.green;
        var p = RayUtils.CastMouseRayToWorld() + Vector3.up;
        Gizmos.DrawSphere(p, 1.0f);

        Gizmos.color = Color.orange;
        Gizmos.DrawSphere(m_Agent.destination, 0.8f);
    }

}
