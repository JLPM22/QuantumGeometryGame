using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PortalController : MonoBehaviour
{
    public bool NextQuantum;
    public int QuantumIndex;
    public bool EndGame;
    [Header("EndGame References")]
    public GameObject EndCanvas;
    public GameObject Player;
    public GameObject MainCanvas;
    public TextMeshProUGUI ScoreText;
    public GameObject Background;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (EndGame)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            Background.SetActive(false);
            Player.SetActive(false);
            MainCanvas.SetActive(false);
            EndCanvas.SetActive(true);
            if (Time.timeScale > 0.6f)
            {
                ScoreText.text = (11 - GameManager.Instance.NumberDeads) + " / 11";
            }
            else
            {
                ScoreText.text = "Practice Mode";
            }
            PostprocessManager.Instance.ColorWhite();
        }
        else
        {
            if (NextQuantum)
            {
                PostprocessManager.Instance.QuantumChange(() => GameManager.Instance.NextQuantum(QuantumIndex));
            }
            else
            {
                PostprocessManager.Instance.QuantumChange(() => GameManager.Instance.Next(QuantumIndex));
            }
        }
    }
}
