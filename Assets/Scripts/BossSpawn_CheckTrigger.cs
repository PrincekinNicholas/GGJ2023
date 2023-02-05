using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn_CheckTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Service.playerTag)
        {

        }
    }
}
