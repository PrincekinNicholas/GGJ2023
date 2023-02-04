using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour{
    [SerializeField] private Sprite wormStretch;
    [SerializeField] private Sprite wormSquish;

    [SerializeField] private GameObject stretchCollider;
    [SerializeField] private GameObject squishCollider;

    [SerializeField] private float detectRange;
    [SerializeField] private float loseSightRange;

    [SerializeField] private float guardRange;
    [SerializeField] private float moveStep;
    [SerializeField] private float moveFreq;
    [SerializeField] private float alertMoveFreq;
    private bool squish = false;

    private float direction = 1;
    private float delta;
    private Vector3 guardPos;

    [SerializeField] private WORM_STATE monsterState = WORM_STATE.GUARD;

    private SpriteRenderer m_sprite;
    private Transform m_playerTrans;
    private void Awake(){
        guardPos = transform.position;
        m_sprite = GetComponent<SpriteRenderer>();
        delta = 0;
    }
    void Update(){
        switch (monsterState) {
            case WORM_STATE.GUARD:
                Move(moveFreq);
                FaceDirection();

                break;
            case WORM_STATE.ALERT:
                Move(alertMoveFreq);
                FacePlayer();

                break;
        }
    }
    void FaceDirection()
    {
        if (transform.position.x > guardPos.x + guardRange)
        {
            direction = -1;
            m_sprite.flipX = true;
        }
        if (transform.position.x < guardPos.x - guardRange)
        {
            direction = 1;
            m_sprite.flipX = false;
        }
    }
    void FacePlayer()
    {
        if(transform.position.x - m_playerTrans.position.x < 0)
        {
            direction = 1;
            m_sprite.flipX = false;
        }
        else
        {
            direction = -1;
            m_sprite.flipX = true;
        }
    }
    void Move(float freq)
    {
        delta += Time.deltaTime * freq;
        if (delta >= 1){
            delta = 0;
            squish = !squish;
            if (squish) m_sprite.sprite = wormSquish;
            else m_sprite.sprite = wormStretch;
        }
        transform.position += Vector3.right * direction * moveStep * freq * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        monsterState = WORM_STATE.ALERT;
        stretchCollider.SetActive(false);
        squishCollider.SetActive(true);
        m_playerTrans = collision.transform;
        GetComponent<CircleCollider2D>().radius = loseSightRange;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        monsterState = WORM_STATE.GUARD;
        stretchCollider.SetActive(true);
        squishCollider.SetActive(false);
        GetComponent<CircleCollider2D>().radius = detectRange;
        guardPos = transform.position;
    }
}
