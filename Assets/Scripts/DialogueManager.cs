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
        var parent = transform.parent.gameObject;
        if (Instance != null && Instance != this) {
            Destroy(parent);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(parent);
    }

    public void StartDialogue(string node) {
        Runner.StartDialogue(node);
    }

    // the patient needs more mouse rays...
    public Interactable CastMouseRay() {
        var mask_interactable = 1 << LayerMask.NameToLayer("Interactable");

        var hit = RayUtils.CastMouseRayToWorld(mask_interactable);
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
