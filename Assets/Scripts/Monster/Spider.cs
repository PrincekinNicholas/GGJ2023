using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField] private SPIDER_STATE monsterState = SPIDER_STATE.GUARD;
    [SerializeField] private Transform spiderTrans;
    [SerializeField] private Animator spiderRotateAnime;
    [SerializeField, Tooltip("蜘蛛掉落的高度")] private float fallRange;
    [SerializeField, Tooltip("蜘蛛回升的高度")] private float BackHeight;
    [SerializeField] private float fallTime;
    [SerializeField] private float backTime;
    [SerializeField] private float shakeTime;
    [SerializeField] private AnimationCurve fallCurve;
    private bool shake = false;
    private Vector3 targetPos, initPos, backPos;
    private float fallDelta, shakeDelta;
    private void Awake()
    {
        targetPos = spiderTrans.localPosition + Vector3.down * fallRange;
        backPos = spiderTrans.localPosition + Vector3.down * fallRange / 2;
        initPos = spiderTrans.localPosition;
    }
    private void Update()
    {
        switch (monsterState)
        {
            case SPIDER_STATE.GUARD:
                break;
            case SPIDER_STATE.ALERT:
                if (!shake)
                {
                    fallDelta += Time.deltaTime / fallTime;
                    spiderTrans.localPosition = Vector3.LerpUnclamped(initPos, targetPos, fallCurve.Evaluate(fallDelta));
                    if (fallDelta >= 1) shake = true;
                }
                if (shake)
                {
                    shakeDelta += Time.deltaTime / shakeTime;
                    if (shakeDelta >= 1)
                    {
                        fallDelta = 0;
                        monsterState = SPIDER_STATE.BACK;
                        spiderRotateAnime.SetTrigger("StopShake");
                    }
                }
                break;
            case SPIDER_STATE.BACK:
                fallDelta += Time.deltaTime / backTime;
                spiderTrans.localPosition = Vector3.LerpUnclamped(targetPos, backPos, fallCurve.Evaluate(fallDelta));
                if (fallDelta >= 1)
                {
                    initPos = backPos;
                    monsterState = SPIDER_STATE.GUARD;
                    ResetParam();
                }
                break;
        }
    }
    void ResetParam()
    {
        shake = false;
        fallDelta = 0;
        shakeDelta = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (monsterState == SPIDER_STATE.GUARD)
        {
            fallDelta = 0;
            monsterState = SPIDER_STATE.ALERT;
            spiderRotateAnime.SetTrigger("Rotate");
        }
    }
}
