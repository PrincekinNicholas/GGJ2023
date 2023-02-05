using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBossAnimation : MonoBehaviour
{
    [SerializeField] private Animation bossRunning;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Service.playerTag)
        {
            bossRunning.Play();
        }
    }
}
