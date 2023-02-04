using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCommand
{
    public string content;
    public string speakerName;
    public float y_Offset;
    public Transform speakerTrans;
    public DialogueCommand(string _content, string _speakerName, float _offset, Transform _speakerTrans)
    {
        this.content = _content;
        this.speakerName = _speakerName;
        this.y_Offset = _offset;
        this.speakerTrans = _speakerTrans;
    }
    public static DialogueCommand EmptyDialgue = new DialogueCommand(string.Empty, string.Empty, 0, null);
}
