using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCommand
{
    public string content;
    public string speakerName;
    public Vector2 speakerPos;
    public DIALOGUE_STYLE style;
    public float lifeTime;
    public DialogueCommand(string _content, string _speakerName, Vector2 _speakerPos, float lifeTime = 2, DIALOGUE_STYLE style = DIALOGUE_STYLE.HERO)
    {
        this.content = _content;
        this.speakerName = _speakerName;
        this.speakerPos = _speakerPos;
        this.style = style;
        this.lifeTime = lifeTime;
    }
    public static DialogueCommand EmptyDialgue = new DialogueCommand(string.Empty, string.Empty, Vector2.one, -1, DIALOGUE_STYLE.HERO);
}
