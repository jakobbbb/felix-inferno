using UnityEngine;

public class BillboardSprite : MonoBehaviour {
    public void Update() {
        transform.LookAt(Camera.main.transform.position);
        var eul = transform.eulerAngles;
        eul.x = 0.0f;
        eul.z = 0.0f;
        transform.eulerAngles = eul;
    }
}
