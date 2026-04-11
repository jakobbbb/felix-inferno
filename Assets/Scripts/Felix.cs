using UnityEngine;
using UnityEngine.AI;

public class Felix : MonoBehaviour {

    [Header("Felix takes action")]
    [SerializeField]
    private FelixInputActions m_Actions;


    [Header("Felix walks around")]

    [SerializeField]
    private NavMeshAgent m_Agent;

    public void Start() {
        Debug.Log($"Hello, I am {name}!");
    }

    public void Update() {
        if (m_Actions.Click.action.WasPerformedThisFrame()) {
            SetTarget();
        }
    }

    public void SetTarget() {
        var p = RayUtils.CastMouseRayToWorld();
        if (p != null) {
            var target = RayUtils.CastRayDown(p.Value + Vector3.up);
            if (target != null) {
                m_Agent.destination = target.Value;
            }
        }
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.purple;
        Gizmos.DrawSphere(transform.position, 1.2f);

        Gizmos.color = Color.orange;
        Gizmos.DrawSphere(m_Agent.destination, 0.8f);
    }

}
