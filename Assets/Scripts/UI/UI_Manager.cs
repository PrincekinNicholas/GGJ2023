using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
[Header("Dialogue Bubble")]
    [SerializeField] private GameObject dialogueBubblePrefab;
    [SerializeField] private DialogueBubble_Style_SO bubbleStyle;
    [SerializeField] private Transform dialogueBubblePanel;
[Header("Subtitle")]
    [SerializeField] private TextMeshProUGUI subtitleText;
[Header("Root Amount")]
    [SerializeField] private TextMeshProUGUI rootAmount;
    private Dictionary<DialogueCommand, UI_DialogueBubble> spawnedBubbleDict;
    private void Awake()
    {
        EventHandler.E_UI_RefreshRootCount += RefreshRootAmount;
        EventHandler.E_UI_OnSubtitle += ShowSubtitle;
        EventHandler.E_UI_OnShowDialogueBubble += ShowDialogueBubble;
        EventHandler.E_UI_OnHideDialogueBubble += HideDialogueBubble;
        EventHandler.E_OnBeforeSceneUnload += CleanUpDialogues;
    }
    private void OnDestroy()
    {
        EventHandler.E_UI_RefreshRootCount -= RefreshRootAmount;
        EventHandler.E_UI_OnSubtitle -= ShowSubtitle;
        EventHandler.E_UI_OnShowDialogueBubble -= ShowDialogueBubble;
        EventHandler.E_UI_OnHideDialogueBubble -= HideDialogueBubble;
        EventHandler.E_OnBeforeSceneUnload -= CleanUpDialogues;
    }
    [ContextMenu("Test Spawn Dialogue")]
    void SpawnDialogue()
    {
        EventHandler.Call_UI_OnShowDialogueBubble(new DialogueCommand("TEST", "CAT", Vector2.zero));
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
    void CleanUpDialogues()
    {
        if (spawnedBubbleDict == null) return;
        foreach (var dialogue in spawnedBubbleDict)
        {
            Destroy(dialogue.Value.gameObject);
            spawnedBubbleDict.Remove(dialogue.Key);
        }
        spawnedBubbleDict = null;
    }
    void ShowDialogueBubble(DialogueCommand dialogue)
    {
        if (spawnedBubbleDict == null) spawnedBubbleDict = new Dictionary<DialogueCommand, UI_DialogueBubble>();

        if (!spawnedBubbleDict.ContainsKey(dialogue))
        {
            UI_DialogueBubble dialogueGroup = GameObject.Instantiate(dialogueBubblePrefab, dialogueBubblePanel).GetComponent<UI_DialogueBubble>();
            spawnedBubbleDict.Add(dialogue, dialogueGroup);
        }
        spawnedBubbleDict[dialogue].InitiateContent(dialogue.speakerName, dialogue.content, dialogue.speakerPos, bubbleStyle.GetStyle(dialogue.style));
        StartCoroutine(coroutineFadeInHeardDialogue(dialogue));
    }
    void HideDialogueBubble(DialogueCommand dialogue)
    {
        if (spawnedBubbleDict.ContainsKey(dialogue))
        {
            StartCoroutine(coroutineFadeOutHeardDialogue(dialogue));
        }
    }
    void RefreshRootAmount(int amount) => rootAmount.text = $"x {amount}";

    IEnumerator coroutineFadeInHeardDialogue(DialogueCommand dialogue)
    {
        yield return spawnedBubbleDict[dialogue].coroutineFadeContent(true);
        yield return null;
    }
    IEnumerator coroutineFadeOutHeardDialogue(DialogueCommand dialogue)
    {
        yield return spawnedBubbleDict[dialogue].coroutineFadeContent(false);
        yield return null;
    }
}
