using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    [SerializeField] private string nextLevelName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Service.playerTag)
        {
            GameManager.Instance.SwitchingScene(nextLevelName);
        }
    }
}
