using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

using Bloom = UnityEngine.Rendering.Universal.Bloom;

public class PostprocessManager : MonoBehaviour
{
    public static PostprocessManager Instance;

    private Volume Volume;

    private void Awake()
    {
        Debug.Assert(Instance == null, "Singleton");
        Instance = this;

        Volume = GetComponent<Volume>();
    }

    public void ChangeColor()
    {
        Color c = new Color(Random.Range(0.0f, 1.0f),
                            Random.Range(0.0f, 1.0f),
                            Random.Range(0.0f, 1.0f));
        Bloom bloom;
        if (Volume.profile.TryGet<Bloom>(out bloom))
        {
            bloom.tint.value = c;
        }
    }
}
