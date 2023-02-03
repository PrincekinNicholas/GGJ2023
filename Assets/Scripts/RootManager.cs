using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootManager : MonoBehaviour
{
    [SerializeField] private int rootCount = 0;
    private void Awake()
    {
        EventHandler.E_OnBreakRoot += BreakOneRoot;
    }
    private void OnDestroy()
    {
        EventHandler.E_OnBreakRoot -= BreakOneRoot;
    }
    public void BreakOneRoot(){
        rootCount ++;
        EventHandler.Call_UI_RefreshRootCount(rootCount);
    }
}
