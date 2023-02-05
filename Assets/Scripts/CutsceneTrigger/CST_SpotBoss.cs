using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CST_SpotBoss : CutSceneTrigger
{
    [SerializeField] private DialogueCommand heroWords;
    [SerializeField] private DialogueCommand bossWords;
    [SerializeField] private GameObject heroSuprise;
    [SerializeField] private Animation bossAnimation;
    protected override IEnumerator coroutineCutscene(PlayerControl player)
    {
        EventHandler.Call_OnEnterCutscene();
        yield return new WaitForSeconds(1f);
        heroSuprise.SetActive(true); //ÏÔÊ¾Ò»¸ö¾ªÌ¾ºÅ

        EventHandler.Call_UI_OnShowDialogueBubble(heroWords);
        yield return new WaitForSeconds(2f);
        EventHandler.Call_UI_OnHideDialougeBubble(heroWords);
        yield return new WaitForSeconds(0.5f);
        heroSuprise.SetActive(false);

        EventHandler.Call_UI_OnShowDialogueBubble(bossWords);
        yield return new WaitForSeconds(1f);
        EventHandler.Call_UI_OnHideDialougeBubble(bossWords);
        yield return new WaitForSeconds(0.5f);
        bossAnimation.Play();

        EventHandler.Call_OnExitCutscene();
    }
}