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
    [SerializeField] private TextMeshProUGUI subtitleText_Middle;
[Header("Root Amount")]
    [SerializeField] private GameObject rootUI_Panel;
    [SerializeField] private TextMeshProUGUI rootAmount;
    [SerializeField] private Dictionary<DialogueCommand, UI_DialogueBubble> spawnedBubbleDict;
    [SerializeField] private List<DialogueCommand> playedCommand;
    private void Awake()
    {
        EventHandler.E_UI_RefreshRootCount += RefreshRootAmount;
        EventHandler.E_UI_OnSubtitle += ShowSubtitle;
        EventHandler.E_UI_OnShowDialogueBubble += ShowDialogueBubble;
        EventHandler.E_UI_OnHideDialogueBubble += HideDialogueBubble;
        EventHandler.E_OnBeforeSceneUnload += CleanUpDialogues;
        EventHandler.E_OnAfterSceneUnload += CheckRootUI;

        rootUI_Panel.SetActive(false);
    }
    private void OnDestroy()
    {
        EventHandler.E_UI_RefreshRootCount -= RefreshRootAmount;
        EventHandler.E_UI_OnSubtitle -= ShowSubtitle;
        EventHandler.E_UI_OnShowDialogueBubble -= ShowDialogueBubble;
        EventHandler.E_UI_OnHideDialogueBubble -= HideDialogueBubble;
        EventHandler.E_OnBeforeSceneUnload -= CleanUpDialogues;
        EventHandler.E_OnAfterSceneUnload -= CheckRootUI;
    }
    private void Update()
    {
        if (playedCommand == null || playedCommand.Count == 0) return;
        for(int i=0; i< playedCommand.Count; i++)
        {
            if(spawnedBubbleDict.ContainsKey(playedCommand[i]) && spawnedBubbleDict[playedCommand[i]] != null)
            {
                var dialogue = spawnedBubbleDict[playedCommand[i]];
                dialogue.DialogueUpdate();
                if (dialogue.TimeUp && dialogue.autoFade)
                {
                    dialogue.FadeContent(false);
                }
            }
        }
    }
    void ShowSubtitle(string content, bool isMiddle = false)
    {
        if(content == null || content == string.Empty)
        {
            subtitleText.text = string.Empty;
            subtitleText_Middle.text = string.Empty;
        }
        else
        {
            if (isMiddle) subtitleText_Middle.text = content;
            else subtitleText.text = content;
        }
    }
    void CheckRootUI(string unloadSceneName)
    {
        if(unloadSceneName == "Start Menu")
        {
            rootUI_Panel.SetActive(true);
        }
    }
    void CleanUpDialogues()
    {
        if (playedCommand == null || spawnedBubbleDict == null) return;

        foreach (var dialogue in spawnedBubbleDict)
        {
            dialogue.Value.KillDialogue();
        }
        spawnedBubbleDict.Clear();
        spawnedBubbleDict = null;

        for (int i = playedCommand.Count - 1; i >= 0; i--)
        {
            playedCommand.Remove(playedCommand[i]);
        }
        playedCommand.Clear();
        playedCommand = null;
    }
    void ShowDialogueBubble(DialogueCommand dialogue)
    {
        if (spawnedBubbleDict == null) spawnedBubbleDict = new Dictionary<DialogueCommand, UI_DialogueBubble>();
        if (playedCommand == null) playedCommand = new List<DialogueCommand>();

        if (!spawnedBubbleDict.ContainsKey(dialogue))
        {
            UI_DialogueBubble dialogueGroup = GameObject.Instantiate(dialogueBubblePrefab, dialogueBubblePanel).GetComponent<UI_DialogueBubble>();
            spawnedBubbleDict.Add(dialogue, dialogueGroup);
            playedCommand.Add(dialogue);
        }
        spawnedBubbleDict[dialogue].InitiateContent(dialogue.speakerName, dialogue.content, dialogue.speakerPos, dialogue.lifeTime, bubbleStyle.GetStyle(dialogue.style));
        spawnedBubbleDict[dialogue].FadeContent(true);
    }
    void HideDialogueBubble(DialogueCommand dialogue)
    {
        if (spawnedBubbleDict.ContainsKey(dialogue))
        {
            spawnedBubbleDict[dialogue].FadeContent(false);
        }
    }
    void RefreshRootAmount(int amount) => rootAmount.text = $"x {amount}";
}
