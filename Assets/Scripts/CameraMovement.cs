using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform Target;
    public Vector2 Offset;

    private void LateUpdate()
    {
        transform.position = Target.transform.position + new Vector3(Offset.x, Offset.y, -10.0f);
    }
}
