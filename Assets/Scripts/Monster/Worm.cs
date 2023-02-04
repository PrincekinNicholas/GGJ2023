using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour, ISlowable{
    public float slowFactor { get; set; } = 1;
    [SerializeField] private bool RightAsStart = true;
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

    [SerializeField] private float detectDistance;
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private float collisionDetectionStep;
    private bool squish = false;

    private float direction = 1;
    private float delta;
    private float collisionTime;
    private Vector3 guardPos;

    [SerializeField] private WORM_STATE monsterState = WORM_STATE.GUARD;

    private SpriteRenderer m_sprite;
    private Transform m_playerTrans;
    private void Awake(){
        guardPos = transform.position;
        m_sprite = GetComponent<SpriteRenderer>();
        collisionTime = Time.time;
        direction = RightAsStart ? 1 : -1;
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
    public void SlowDown(float factor)
    {
        StartCoroutine(coroutineSlowingDown(factor));
    }
    IEnumerator coroutineSlowingDown(float targetFactor)
    {
        float initFactor = slowFactor;
        for (float t = 0; t < 1; t += Time.deltaTime * 2f)
        {
            slowFactor = Mathf.Lerp(initFactor, targetFactor, EasingFunc.Easing.SmoothInOut(t));
            yield return null;
        }
        slowFactor = targetFactor;
    }
    public void Recover()
    {
        slowFactor = 1;
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
        delta += Time.deltaTime * freq * slowFactor;
        if (delta >= 1){
            CollisionDetection();
            delta = 0;
            squish = !squish;
            if (squish) m_sprite.sprite = wormSquish;
            else m_sprite.sprite = wormStretch;
            transform.position += Vector3.right * direction * moveStep;
        }
    }
    void CollisionDetection()
    {
        if(Physics2D.Raycast(transform.position, Vector2.right * direction, detectDistance, collisionLayer) && Time.time - collisionTime > collisionDetectionStep)
        {
            collisionTime = Time.time;
            transform.position -= Vector3.right * direction * moveStep * Time.deltaTime;
            direction *= -1;
            m_sprite.flipX = !m_sprite.flipX;
            monsterState = WORM_STATE.GUARD;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawRay(transform.position, Vector2.right * direction * detectDistance);
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
