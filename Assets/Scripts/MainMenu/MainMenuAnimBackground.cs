using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnimBackground : MonoBehaviour
{
    public float Speed = 1.0f;

    private Vector3 InitPos;

    private void Start()
    {
        InitPos = transform.position;
    }

    private void Update()
    {
        const float maxOffset = 50.0f;

        float t = Mathf.PingPong(Time.unscaledTime, 2 * maxOffset);

        transform.position = InitPos + (new Vector3(1.0f, 1.0f, 0.0f)).normalized * (t - maxOffset);
    }
}
