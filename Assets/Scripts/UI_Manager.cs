using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rootAmount;
    private void Awake()
    {
        EventHandler.E_UI_RefreshRootCount += RefreshRootAmount;
    }
    private void OnDestroy()
    {
        EventHandler.E_UI_RefreshRootCount -= RefreshRootAmount;
    }
    void RefreshRootAmount(int amount) => rootAmount.text = $"x {amount}";
}
