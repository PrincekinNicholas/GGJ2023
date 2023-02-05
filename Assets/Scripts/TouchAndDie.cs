using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchAndDie : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
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
