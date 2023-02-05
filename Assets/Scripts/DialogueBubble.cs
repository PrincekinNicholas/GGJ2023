using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBubble : MonoBehaviour
{
    private bool alreadyTriggerDialogue;
    [SerializeField] private DialogueCommand bossDialogue;
    [SerializeField] private GameObject BreakableTreeBranch;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(bossDialogue.content == "BossFirstMeetDialogue")
        {
            BossFirstMeetDialogue( collision );
            
        }


        if(collision.tag == Service.playerTag && !alreadyTriggerDialogue)
        {
            alreadyTriggerDialogue = true;
            EventHandler.Call_UI_OnShowDialogueBubble(bossDialogue);
        }
    }

    public void BossFirstMeetDialogue( Collider2D collision )
    {
        string killingSpeakingContent = "离那些树根远点！";     //boss剧情杀死玩家
        string escapeSpeakingContent = "根系是祭司的力量之源，摧毁它们，削弱祭司的力量。";  //boss逃跑

        BreakableRoot breakableRoot = BreakableTreeBranch.GetComponent<BreakableRoot>();
        bool isWeak = breakableRoot.GetAlreadyBorkenValue();
        if (isWeak)
        {
            bossDialogue.content = escapeSpeakingContent;
        }
        else
        {
            bossDialogue.content = killingSpeakingContent;
            if(collision.tag == Service.playerTag)
            {
                PlayerControl pc = collision.GetComponent<PlayerControl>();
                if (!pc.godMode)
                {
                    collision.GetComponent<PlayerControl>().Kill();
                }
            }   
        }
    }
}
