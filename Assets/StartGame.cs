using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene("Game");
        SceneManager.LoadScene("Stage0", LoadSceneMode.Additive);
    }
}
