using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI PercentageText;
    public List<GameObject> Maps;

    private int Current;
    private bool Quantum;
    private int QuantumAlive;
    private int LastQuantumIndex = -1;

    public int NumberDeads { get; private set; }

    private void Awake()
    {
        Debug.Assert(Instance == null, "Singleton");
        Instance = this;

        Application.targetFrameRate = 60;
    }

    public void NextQuantum(int quantumIndex)
    {
        if (quantumIndex <= LastQuantumIndex) return;

        Advance();

        for (int i = Current; i < Current + 4; ++i)
        {
            Maps[i].SetActive(true);
            Maps[i].GetComponentInChildren<PlayerMovement>().SetMapIndex(i);
        }

        QuantumAlive = 4;
        Quantum = true;
        LastQuantumIndex = quantumIndex;
        UpdatePercentageText();
    }

    public void Next(int quantumIndex)
    {
        if (quantumIndex <= LastQuantumIndex) return;

        Advance();

        Maps[Current].SetActive(true);
        Maps[Current].GetComponentInChildren<PlayerMovement>().SetMapIndex(Current);

        Quantum = false;
        LastQuantumIndex = quantumIndex;
        UpdatePercentageText();
    }

    public void NotifyDead(int mapIndex)
    {
        if (Quantum)
        {
            // Turn off Camera
            // Camera camera = Maps[mapIndex].GetComponentInChildren<Camera>();
            // camera.backgroundColor = Color.black;
            // camera.cullingMask = 0;
            // Compute probability to die
            float p = Random.Range(0.0f, 1.0f);
            if (p < (1.0f / QuantumAlive))
            {
                for (int i = Current; i < Current + 4; ++i)
                {
                    PlayerMovement player = Maps[i].GetComponentInChildren<PlayerMovement>();
                    if (!player.Dead) player.Die();
                }
                StartCoroutine(ReloadScene());
            }
            QuantumAlive -= 1;
            NumberDeads += 1;
            UpdatePercentageText();
        }
        else
        {
            StartCoroutine(ReloadScene());
        }
    }

    private IEnumerator ReloadScene()
    {
        const string sceneName = "MainScene";
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(sceneName);
    }

    private void Advance()
    {
        if (Quantum)
        {
            for (int i = Current; i < Current + 4; ++i)
            {
                Maps[i].SetActive(false);
            }
            Current += 4;
        }
        else
        {
            Maps[Current].SetActive(false);
            Current += 1;
        }
    }

    private void UpdatePercentageText()
    {
        if (Quantum)
        {
            if (QuantumAlive == 4)
            {
                PercentageText.text = "25%";
            }
            else if (QuantumAlive == 3)
            {
                PercentageText.text = "33%";
            }
            else if (QuantumAlive == 2)
            {
                PercentageText.text = "50%";
            }
            else if (QuantumAlive == 1)
            {
                PercentageText.text = "100%";
            }
        }
        else
        {
            PercentageText.text = "100%";
        }
    }
}