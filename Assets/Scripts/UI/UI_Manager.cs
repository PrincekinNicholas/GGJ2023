using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
[Header("Dialogue Bubble")]
    
[Header("Subtitle")]
    [SerializeField] private TextMeshProUGUI subtitleText;
[Header("Root Amount")]
    [SerializeField] private TextMeshProUGUI rootAmount;

    private void Awake()
    {
        EventHandler.E_UI_RefreshRootCount += RefreshRootAmount;
        EventHandler.E_UI_OnSubtitle += ShowSubtitle;
    }
    private void OnDestroy()
    {
        EventHandler.E_UI_RefreshRootCount -= RefreshRootAmount;
        EventHandler.E_UI_OnSubtitle -= ShowSubtitle;
    }
    void ShowSubtitle(string content)
    {
        if(content == null || content == string.Empty)
        {
            subtitleText.text = string.Empty;
        }
        else
        {
            subtitleText.text = content;
        }
    }
    void RefreshRootAmount(int amount) => rootAmount.text = $"x {amount}";

}
