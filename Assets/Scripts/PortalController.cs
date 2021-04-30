using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public bool NextQuantum;
    public int QuantumIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (NextQuantum)
        {
            GameManager.Instance.NextQuantum(QuantumIndex);
        }
        else
        {
            GameManager.Instance.Next(QuantumIndex);
        }
    }
}
