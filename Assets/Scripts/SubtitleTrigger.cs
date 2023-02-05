using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtitleTrigger : MonoBehaviour
{
    [SerializeField] private string content;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Service.playerTag)
        {
            EventHandler.Call_UI_OnSubtitle(content);
            Destroy(this);
        }
    }
}
