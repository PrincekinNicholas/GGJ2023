using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_DialogueBubble : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI dialogue;
    [SerializeField] private TextMeshProUGUI nameTag;
    [SerializeField] private TextMeshProUGUI continueMarkTag;
    [SerializeField] private float dialogueFadeSpeed = 2;
    [SerializeField] private float lifeTime;
    public bool TimeUp { get { return lifeTime <= 0; } }
[Header("Style")]
    [SerializeField] private Image contentImage;
    [SerializeField] private Image continueMark;
    [SerializeField] private Image nameTagBackground;
    public bool autoFade { get; protected set; } = true;

    IEnumerator coroutineFade;

    public void InitiateContent(string speakerName, string speakContent, Vector3 uiPos, float lifeTime, DialogueBubble_Sytle style)
    {
        rectTransform.localPosition = uiPos;
        nameTag.text = speakerName;
        dialogue.text = speakContent;

        //continueMarkTag.color = style.TextColor;
        //nameTag.color  = style.TextColor;
        //dialogue.color = style.TextColor;

        //nameTagBackground.color = style.NameColor;
        //contentImage.color = style.ContentColor;
        //continueMark.color = style.ContinueMarkColor;

        this.lifeTime = lifeTime;
        if (lifeTime <= -1) autoFade = false;
    }
    public void UpdateDialoguePos(Vector3 uiPos)
    {
        rectTransform.localPosition = uiPos;
    }
    public void DialogueUpdate()
    {
        if(autoFade) lifeTime -= Time.deltaTime;
    }
    public void FadeContent(bool isFadeIn)
    {
        StartCoroutine(coroutineFadeContent(isFadeIn));
    }
    public void KillDialogue()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
    public IEnumerator coroutineFadeContent(bool isFadeIn)
    {
        float initAlpha = canvasGroup.alpha;
        float targetAlpha = isFadeIn ? 1 : 0;
        for (float t = 0; t < 1; t += Time.deltaTime * dialogueFadeSpeed)
        {
            canvasGroup.alpha = Mathf.Lerp(initAlpha, targetAlpha, EasingFunc.Easing.QuadEaseOut(t));
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
    }
}
