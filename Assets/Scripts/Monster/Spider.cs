using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField] private MONSTER_STATE monsterState = MONSTER_STATE.GUARD;
    [SerializeField] private float fallRange;
    [SerializeField] private float fallTime;
    [SerializeField] private AnimationCurve fallCurve;
    private void Update()
    {
        switch (monsterState)
        {
            case MONSTER_STATE.GUARD:
                break;
            case MONSTER_STATE.ALERT:

                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        monsterState = MONSTER_STATE.ALERT;
    }
}
