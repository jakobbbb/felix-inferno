using UnityEngine;

public class FelixAnimator : MonoBehaviour {

    [SerializeField]
    private Animator m_Animator;

    public enum Triggers {
        Jump,
        Happy,
        Sad,
        Hocke,
        Dap,
    }
    public enum Directions {
        Left = 1,
        Neutral = 0,
        Right = -1,
    }

    private void Trigger(Triggers t, bool reset = false) {
        if (reset) {
            m_Animator.ResetTrigger(t.ToString());
        } else {
            m_Animator.SetTrigger(t.ToString());
        }
    }

    private void SetDirection(Directions d, bool front) {
        m_Animator.SetInteger("Direction", (int)d);
        m_Animator.SetBool("FacingFront", front);
    }

    private Vector3 m_PrevPos;
    private Vector3 m_PrevPosScreenSpace;

    public void Update() {
        var dir = (transform.position - m_PrevPos) / Time.deltaTime;
        var ppos = Camera.main.WorldToScreenPoint(transform.position);
        var ppos_delta = ppos - m_PrevPosScreenSpace;

        Directions d = Directions.Neutral;
        bool front = false;
        if (Mathf.Abs(ppos_delta.x) > 0.1312f) {
            if (ppos_delta.x < 0) {
                d = Directions.Left;
            } else {
                d = Directions.Right;
            }
            front = false;
        } else {
            var cam = Camera.main.transform.position;
            var dot = Vector3.Dot(transform.right, cam - transform.position);
            Debug.Log(dot);
        }
        SetDirection(d, front);

        if (dir.y > 0.01f) {
            Trigger(Triggers.Jump);
        } else if (Mathf.Abs(dir.y) < 0.01f) {
            Trigger(Triggers.Jump, reset: true);
        }

        m_PrevPos = transform.position;
        m_PrevPosScreenSpace = ppos;
    }

}
