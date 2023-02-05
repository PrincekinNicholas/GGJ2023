using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CST_MeetWeakBoss : CutSceneTrigger
{
    [SerializeField] private DialogueCommand bossWords;
    [SerializeField] private Animation bossAnime;
    [SerializeField] private float escape_Delay;
    protected override IEnumerator coroutineCutscene(PlayerControl player)
    {
        EventHandler.Call_OnEnterCutscene();
        EventHandler.Call_UI_OnShowDialogueBubble(bossWords);
        yield return new WaitForSeconds(escape_Delay);
        EventHandler.Call_UI_OnHideDialougeBubble(bossWords);
        bossAnime.Play();
        yield return null; 
        EventHandler.Call_OnExitCutscene();
    }
}
