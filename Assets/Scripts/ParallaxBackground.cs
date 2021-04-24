using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform Player;
    [Range(0.0f, 1.0f)]
    public float RelativeVelocity;

    private Vector3 LastPlayerPos;

    private void Start()
    {
        LastPlayerPos = Player.transform.position;
    }

    private void Update()
    {
        float diffX = Player.transform.position.x - LastPlayerPos.x;

        transform.position += transform.right * diffX * RelativeVelocity;

        LastPlayerPos = Player.transform.position;
    }
}
