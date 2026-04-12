using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Felix : MonoBehaviour {

    [Header("Felix takes action")]
    [SerializeField]
    private FelixInputActions m_Actions;

    [Header("Felix walks around")]

    [SerializeField]
    private NavMeshAgent m_Agent;

    private Interactable m_TargetInteractable;

    public void Start() {
        Debug.Log($"Hello, I am {name}!");
    }

    public void Update() {
        if (DialogueManager.Instance.Runner.IsDialogueRunning) {
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        if (m_Actions.Click.action.WasPerformedThisFrame()) {
            SetTarget();
            m_TargetInteractable = DialogueManager.Instance.CastMouseRay();
            Debug.Log($"{name} wants the {m_TargetInteractable}!");
        }
        if (m_TargetInteractable != null && NavUtils.AreWeThereYet(m_Agent)) {
            m_TargetInteractable.Trigger();
            m_TargetInteractable = null;
        }
    }

    public void SetTarget() {
        var p = RayUtils.CastMouseRayToWorld(~0);  // mask: everything
        if (p != null) {
            var target = RayUtils.CastRayDown(p.Value.point + Vector3.up);
            if (target != null) {
                m_Agent.destination = target.Value;
            }
        }
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.purple;
        Gizmos.DrawSphere(transform.position, 0.2f);

        Gizmos.color = Color.orange;
        Gizmos.DrawSphere(m_Agent.destination, 0.2f);
    }

}
