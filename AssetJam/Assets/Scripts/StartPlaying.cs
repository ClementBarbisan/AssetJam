using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPlaying : MonoBehaviour
{
    public void StartPlayingChess()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
