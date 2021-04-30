using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<GameObject> Maps;

    private int Current;
    private bool Quantum;
    private int QuantumAlive;
    private int LastQuantumIndex = -1;

    private void Awake()
    {
        Debug.Assert(Instance == null, "Singleton");
        Instance = this;
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
    }

    public void Next(int quantumIndex)
    {
        if (quantumIndex <= LastQuantumIndex) return;

        Advance();

        Maps[Current].SetActive(true);
        Maps[Current].GetComponentInChildren<PlayerMovement>().SetMapIndex(Current);

        Quantum = false;
        LastQuantumIndex = quantumIndex;
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
}