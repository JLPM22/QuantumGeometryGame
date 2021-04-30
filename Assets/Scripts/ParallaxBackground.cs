using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform Player;
    [Range(0.0f, 1.0f)]
    public float RelativeVelocity;

    private Vector3 LastPlayerPos;
    private Material Material;

    private void Start()
    {
        LastPlayerPos = Player.transform.position;

        Material = GetComponent<SpriteRenderer>().material;
        Material.SetVector("OriginalPos", transform.position);
        Material.SetVector("TilingOffset", new Vector2(Random.Range(0, 100), Random.Range(0, 100)));
    }

    private void Update()
    {
        float diffX = Player.transform.position.x - LastPlayerPos.x;

        transform.position += transform.right * diffX * RelativeVelocity;

        LastPlayerPos = Player.transform.position;
    }

    private void OnDestroy()
    {
        if (Material != null)
        {
            Destroy(Material);
        }
    }
}
