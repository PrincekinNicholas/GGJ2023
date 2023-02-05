using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CST_MeetStrongBoss : CutSceneTrigger
{
    [SerializeField] private DialogueCommand bossWords;
    [SerializeField] private string hintMessage;
    [SerializeField] private float killplayer_Delay;
    [SerializeField] private float hint_Duration;
    protected override IEnumerator coroutineCutscene(PlayerControl player)
    {
        EventHandler.Call_OnEnterCutscene();
        EventHandler.Call_UI_OnShowDialogueBubble(bossWords);
        yield return new WaitForSeconds(killplayer_Delay);
        EventHandler.Call_UI_OnHideDialougeBubble(bossWords);
        player.Kill(false);
        EventHandler.Call_UI_OnSubtitle(hintMessage);
        yield return new WaitForSeconds(hint_Duration);
        EventHandler.Call_UI_OnSubtitle(null);
        EventHandler.Call_OnExitCutscene();
        GameManager.Instance.RestartLevel();
    }
}
