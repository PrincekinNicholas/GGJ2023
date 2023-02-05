using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CST_Cutscene3 : CutSceneTrigger
{
    [SerializeField] private DialogueCommand bossWords;
    [SerializeField] private float dialogueDuration = 2;
    [SerializeField] private Animation cam_rotate_anime;
    [SerializeField] private string nextScene;
    protected override IEnumerator coroutineCutscene(PlayerControl player)
    {
        EventHandler.Call_OnEnterCutscene();
        yield return new WaitForSeconds(0.2f);
        EventHandler.Call_UI_OnShowDialogueBubble(bossWords);
        yield return new WaitForSeconds(dialogueDuration);
        EventHandler.Call_UI_OnHideDialougeBubble(bossWords);
        yield return null;
        cam_rotate_anime.Play();
        yield return new WaitForSeconds(cam_rotate_anime.clip.length);
        GameManager.Instance.SwitchingScene(nextScene);
    }
}
