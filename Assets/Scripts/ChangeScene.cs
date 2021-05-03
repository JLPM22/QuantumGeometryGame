using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string SceneName;
    public float TimeScale = 0.8f;

    public void DoChangeScene()
    {
        Time.timeScale = TimeScale;
        SceneManager.LoadScene(SceneName);
    }
}
