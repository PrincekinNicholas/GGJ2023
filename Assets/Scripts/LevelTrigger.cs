using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    [SerializeField] private string nextLevelName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(nextLevelName);
        GameManager.Instance.SwitchingScene(nextLevelName);
    }
}
