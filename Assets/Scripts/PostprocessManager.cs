using System;
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
        Color c = new Color(UnityEngine.Random.Range(0.0f, 1.0f),
                            UnityEngine.Random.Range(0.0f, 1.0f),
                            UnityEngine.Random.Range(0.0f, 1.0f));
        Bloom bloom;
        if (Volume.profile.TryGet<Bloom>(out bloom))
        {
            bloom.tint.value = c;
        }
    }

    public void QuantumChange(Action completed)
    {
        StartCoroutine(QuantumChangeCor(completed));
    }

    private IEnumerator QuantumChangeCor(Action completed)
    {
        const float duration = 1.0f;
        float t = 0.0f;
        Bloom bloom;
        if (Volume.profile.TryGet<Bloom>(out bloom))
        {
            float previousIntensity = bloom.intensity.value;
            while (t < duration)
            {
                bloom.intensity.value = previousIntensity + (1000 * Mathf.Pow(t / duration, 3));
                t += Time.deltaTime;
                yield return null;
            }
            bloom.intensity.value = previousIntensity;
        }
        completed.Invoke();
    }

    public void ColorWhite()
    {
        Color c = Color.white;
        Bloom bloom;
        if (Volume.profile.TryGet<Bloom>(out bloom))
        {
            bloom.tint.value = c;
        }
    }
}
