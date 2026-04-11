using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class RayUtils {

    // Cast ray from mouse cursor onto anything
    public static Vector3? CastMouseRayToWorld() {
        var mouse_pos = Mouse.current.position.ReadValue();
        var ray = Camera.main.ScreenPointToRay(mouse_pos);
        bool is_hit = Physics.Raycast(
                ray,
                out RaycastHit hit,
                9999f,
                ~0  // everything
        );

        if (!is_hit) {
            return null;
        }

        return hit.point;
    }

    // Cast ray downwards onto the walkable plane
    public static Vector3? CastRayDown(Vector3 p) {
        var ray = new Ray();
        ray.origin = p;
        ray.direction = Vector3.down;
        Debug.DrawRay(ray.origin, ray.direction);
        bool is_hit = Physics.Raycast(
                ray,
                out RaycastHit hit,
                9999f,
                1 << LayerMask.NameToLayer("Walkable")
        );

        if (!is_hit) {
            return null;
        }

        return hit.point;
    }
}
