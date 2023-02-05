using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneOnStart_HeroDialogue : MonoBehaviour
{
    [SerializeField] private DialogueCommand heroWords;
    [SerializeField] private float dialogueDelay;
    [SerializeField] private float dialogueDuration;
    void Start()
    {
        StartCoroutine(coroutineCutScene());
    }
    IEnumerator coroutineCutScene()
    {
        EventHandler.Call_OnEnterCutscene();
        yield return new WaitForSeconds(dialogueDelay);
        EventHandler.Call_UI_OnShowDialogueBubble(heroWords);
        yield return new WaitForSeconds(dialogueDuration);
        EventHandler.Call_UI_OnHideDialougeBubble(heroWords);
        EventHandler.Call_OnExitCutscene();
    }
}
