using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullPieces : MonoBehaviour
{
    [SerializeField] private float direction;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jiggleDegree;
    [SerializeField] private float jiggleFreq;
    [Header("Obstacle Checking")]
    [SerializeField] private float detectStep;
    [SerializeField] private float detectRadius;
    [SerializeField] private Vector2 overlapSize;
    [SerializeField] private LayerMask platformLayer;
    private SpriteRenderer m_sprite;
    private Collider2D m_collider;
    private float initHeight;
    private float moveTime;
    private float speed;
    private float detectTime;
    void Awake()
    {
        m_sprite = GetComponent<SpriteRenderer>();
        m_collider = GetComponent<Collider2D>();    
    }
    void Start()
    {
        initHeight = transform.position.y;
        speed = 0;
        detectTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        moveTime += Time.deltaTime * jiggleFreq;
        Vector3 pos = transform.position;
        speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * 3);
        pos += Vector3.right * direction * speed * Time.deltaTime;
        pos.y = initHeight + Mathf.Sin(moveTime)*jiggleDegree;
        transform.position = pos;
        if(Physics2D.OverlapBox(transform.position, overlapSize, 0, platformLayer) && Time.time - detectTime > detectStep)
        {
            detectTime = Time.time;
            pos -= 2 * Vector3.right * direction * maxSpeed * Time.deltaTime;
            direction *= -1;
            speed = maxSpeed*0.5f;
            m_sprite.flipX = !m_sprite.flipX;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(transform.position, overlapSize);
    }
}
