using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public void GotoTheScene(string theNextLevelName)
    {
        GameManager.Instance.SwitchingScene(theNextLevelName);
    }

    public void OnExitGame()
    {
    #if UNITY_EDITOR    //在编辑器模式下退出
        UnityEditor.EditorApplication.isPlaying = false;
    #else   //发布后退出
        Application.Quit();
    #endif
    }
}
