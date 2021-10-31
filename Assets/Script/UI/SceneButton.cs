using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneButton : MonoBehaviour
{
    public void LoadStageScene(int StageNum)
    {
        SceneManager.LoadScene("Game");
        SceneManager.LoadScene(StageNum + 1, LoadSceneMode.Additive);
    }
}
