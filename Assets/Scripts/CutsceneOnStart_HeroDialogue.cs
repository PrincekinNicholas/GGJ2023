using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CutsceneOnStart_HeroDialogue : MonoBehaviour
{
    [SerializeField] private Image blackScreen;
    [SerializeField] private string[] subtitiles;
    [SerializeField] private DialogueCommand heroWords;
    [SerializeField] private float startDelay = 1;
    [SerializeField] private float each_subtitileDuration;
    [SerializeField] private float dialogueDelay;
    [SerializeField] private float dialogueDuration;
    void Start()
    {
        blackScreen.color = Color.black;
        StartCoroutine(coroutineCutScene());
    }
    IEnumerator coroutineCutScene()
    {
        EventHandler.Call_OnEnterCutscene();
        yield return new WaitForSeconds(startDelay);
        //²¥·Å×ÖÄ»
        EventHandler.Call_UI_OnSubtitle(subtitiles[0], true);
        for (int i=1; i<subtitiles.Length; i++)
        {
            yield return new WaitForSeconds(each_subtitileDuration);
            EventHandler.Call_UI_OnSubtitle(subtitiles[i], true);
        }
    //µ­³öºÚÆÁ
        for(float t=0; t<1; t += Time.deltaTime * 2)
        {
            blackScreen.color = Color.Lerp(Color.black, Color.clear, EasingFunc.Easing.SmoothInOut(t));
            yield return null;
        }
        blackScreen.color = Color.clear;
    //Ó¢ÐÛËµ»°
        yield return new WaitForSeconds(dialogueDelay);
        EventHandler.Call_UI_OnShowDialogueBubble(heroWords);
        yield return new WaitForSeconds(dialogueDuration);
        EventHandler.Call_UI_OnHideDialougeBubble(heroWords);
        EventHandler.Call_OnExitCutscene();
    }
}
