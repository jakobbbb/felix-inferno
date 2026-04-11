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
        var cam_pos = Camera.main.transform.position;
        var dist_cam_now = (cam_pos - transform.position).magnitude;
        var dist_cam_prev = (cam_pos - m_PrevPos).magnitude;
        var dist_cam_delta = dist_cam_now - dist_cam_prev;

        Directions d = Directions.Neutral;
        bool front = true;
        if (Mathf.Abs(ppos_delta.x) > 0.1312f) {
            if (ppos_delta.x < 0) {
                d = Directions.Left;
            } else {
                d = Directions.Right;
            }
            front = false;
        } else {
            if (dist_cam_delta > 0.1f) {
                front = false;
            }
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
