using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GGJ2023/DialogueBubble_Style_SO")]
public class DialogueBubble_Style_SO : ScriptableObject
{
    [SerializeField] private List<DialogueBubble_Sytle> dialogueStyles;
    public DialogueBubble_Sytle GetStyle(DIALOGUE_STYLE style)
    {
        return dialogueStyles.Find(x => x.style == style);
    }
}

[System.Serializable]
public class DialogueBubble_Sytle
{
    public DIALOGUE_STYLE style;
    public Color NameColor;
    public Color ContentColor;
    public Color ContinueMarkColor;
    public Color TextColor;
}
