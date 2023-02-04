using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBubble : MonoBehaviour
{
    private bool alreadyTriggerDialogue;
    [SerializeField] private GameObject speaker;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Service.playerTag && !alreadyTriggerDialogue)
        {
            alreadyTriggerDialogue = true;
            EventHandler.Call_UI_OnShowDialogueBubble(new DialogueCommand("Hello World", "Boss", Vector2.zero));
        }
    }
}