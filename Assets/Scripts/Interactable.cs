using UnityEngine;

public class Interactable : MonoBehaviour {

    [SerializeField]
    private string m_DialogueNode;

    public void Trigger() {
        Debug.Log($"Triggering Interactable {name} with dialogue {m_DialogueNode}");
        DialogueManager.Instance.StartDialogue(m_DialogueNode);
    }
}
