using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform Target;
    public Vector2 Offset;

    private float PosY;

    private void Start()
    {
        PosY = transform.position.y + Offset.y;
    }

    private void LateUpdate()
    {
        Vector3 newPos = Target.transform.position + new Vector3(Offset.x, 0.0f, -10.0f);
        newPos.y = PosY;
        transform.position = newPos;
    }
}
