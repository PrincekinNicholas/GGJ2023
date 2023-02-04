using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBubble : MonoBehaviour
{
    private bool alreadyTriggerDialogue;
    [SerializeField] private GameObject speaker;
    private DialogueCommand bossSpeaking = new DialogueCommand("Hello World", "Boss", Vector2.zero);
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Service.playerTag && !alreadyTriggerDialogue)
        {
            alreadyTriggerDialogue = true;
            EventHandler.Call_UI_OnShowDialogueBubble(bossSpeaking);
        }
        else if(alreadyTriggerDialogue)
        {
            //Temp solution
            EventHandler.Call_UI_OnHideDialougeBubble(bossSpeaking);
        }        
    }
}
