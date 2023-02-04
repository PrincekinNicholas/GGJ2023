using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCommand
{
    public string content;
    public string speakerName;
    public Vector2 speakerPos;
    public DialogueCommand(string _content, string _speakerName, Vector2 _speakerPos)
    {
        this.content = _content;
        this.speakerName = _speakerName;
        this.speakerPos = _speakerPos;
    }
    public static DialogueCommand EmptyDialgue = new DialogueCommand(string.Empty, string.Empty, Vector2.one);
}
