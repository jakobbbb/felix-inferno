using UnityEngine;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour {

    public static DialogueManager Instance = null;

    [SerializeField]
    private DialogueRunner m_Runner;

    public DialogueRunner Runner {
        get => m_Runner;
    }

    public void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(transform.parent.gameObject);
    }

    public void StartDialogue(string node) {
        Runner.StartDialogue(node);
    }

    // the patient needs more mouse rays...
    public Interactable CastMouseRay() {
        var hit = RayUtils.CastMouseRayToWorld(1 << LayerMask.NameToLayer("Interactable"));
        if (hit == null) {
            return null;
        }

        var ia = hit.Value.collider.GetComponent<Interactable>();
        if (ia == null) {
            return null;
        }

        return ia;
    }
}
