using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField] private MONSTER_STATE monsterState = MONSTER_STATE.GUARD;
    [SerializeField] private Transform spiderTrans;
    [SerializeField] private float fallRange;
    [SerializeField] private float fallTime;
    [SerializeField] private AnimationCurve fallCurve;
    private bool shake = false;
    private Vector3 targetPos, initPos;
    private float fallDelta;
    private void Awake()
    {
        targetPos = spiderTrans.localPosition + Vector3.down * fallRange;
        initPos = spiderTrans.localPosition;
    }
    private void Update()
    {
        switch (monsterState)
        {
            case MONSTER_STATE.GUARD:
                break;
            case MONSTER_STATE.ALERT:
                if (!shake)
                {
                    fallDelta += Time.deltaTime / fallTime;
                    spiderTrans.localPosition = Vector3.LerpUnclamped(initPos, targetPos, fallCurve.Evaluate(fallDelta));
                    if (fallDelta >= 1) shake = true;
                }
                else
                {

                }
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        monsterState = MONSTER_STATE.ALERT;
    }
}
