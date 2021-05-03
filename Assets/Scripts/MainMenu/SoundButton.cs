using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundButton : MonoBehaviour
{
    public AudioClip Clip;

    private AudioSource AudioSource;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    public void Sound()
    {
        if (Clip != null)
        {
            AudioSource.PlayOneShot(Clip);
        }
    }
}
