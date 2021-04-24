using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<GameObject> Maps;
    public float OffsetMaps = 1000.0f;

    private int Current;
    private bool Quantum;

    private void Awake()
    {
        Debug.Assert(Instance == null, "Singleton");
        Instance = this;
    }

    public void NextQuantum()
    {
        Advance();



        Quantum = true;
    }

    public void Next()
    {
        Advance();

        Maps[Current].SetActive(true);
        Maps[Current].GetComponentInChildren<PlayerMovement>().SetMapIndex(Current);

        Quantum = false;
    }

    public void NotifyDead(int mapIndex)
    {
        if (Quantum)
        {

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
            for (int i = Current; i < 4; ++i)
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