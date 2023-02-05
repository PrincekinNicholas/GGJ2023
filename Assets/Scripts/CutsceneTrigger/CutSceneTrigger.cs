using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Service.playerTag)
        {
            StartCoroutine(coroutineCutscene(collision.GetComponent<PlayerControl>()));
            GetComponent<Collider2D>().enabled = false;
        }
    }
    protected virtual IEnumerator coroutineCutscene(PlayerControl player) {
        yield return null;
    }
}
