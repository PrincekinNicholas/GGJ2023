using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(PlayerInput))]
public class PlayerControl : MonoBehaviour{
    [SerializeField] private PLAYER_STATE playerState = PLAYER_STATE.DEFAULT;
[Header("Basic Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float minAirdriftSpeed;
    [SerializeField] private float speedSmooth;
[Header("Jump")]
    [SerializeField] private bool onGround = true;
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundCheckRadius = 0.1f;
    private float airdriftSpeed;
    private bool isRunning = false;

    private string crouchTrigger = "Crouch";
    private string standTrigger  = "Stand";
    private string moveBoolean   = "Moving";
    private string jumpTrigger   = "Jump";

    private Rigidbody2D m_rigid;
    private CircleCollider2D m_collider;
    private SpriteRenderer m_sprite;
    private Animator m_animator;
    private PlayerInput m_input;
    private float direction = 0;
    private void Awake(){
        m_sprite   = GetComponentInChildren<SpriteRenderer>();
        m_collider = GetComponent<CircleCollider2D>();
        m_rigid    = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_input    = GetComponent<PlayerInput>();
    }
    void OnEnable(){
        EventHandler.E_OnBeforeSceneUnload += FreezeControl;
    }
    void OnDisable(){
        EventHandler.E_OnBeforeSceneUnload -= FreezeControl;
    }
    void Update(){
        GroundCheck();
    }
    void FixedUpdate() {
        switch (playerState)
        {
            case PLAYER_STATE.DEFAULT:
                Move(isRunning ? runningSpeed : moveSpeed);
                break;
            case PLAYER_STATE.CROUCH:
                Move(crouchSpeed);
                break;
            case PLAYER_STATE.JUMP:
                AirDrift(airdriftSpeed);
                if (onGround) {
                    playerState = PLAYER_STATE.DEFAULT;
                    m_animator.SetTrigger(standTrigger);
                }
                break;
        }
    }
    void OnDrawGizmos(){
        DebugExtension.DrawCircle(transform.position, Vector3.forward, Color.green, groundCheckRadius);
    }
    void FreezeControl()=>m_input.DeactivateInput();
    void GroundCheck(){
        if(Physics2D.OverlapCircle(transform.position, groundCheckRadius, platformLayer) != null) {
            onGround = true;
        }
        else {
            onGround = false;
        }
    }
    void Move(float speed) {
        var vel = m_rigid.velocity;
        vel.x = direction * speed;
        m_rigid.velocity = vel;
    }
    void AirDrift(float speed) {
        var vel = m_rigid.velocity;
        vel.x = Mathf.Lerp(vel.x, direction * speed, Time.fixedDeltaTime * speedSmooth);
        m_rigid.velocity = vel;
    }
    public void Kill(){
        StartCoroutine(CoroutinePlayDead());
    }
    void ScaleCollisionBox(float scale)
    {
        m_collider.offset = Vector2.up * scale;
        m_collider.radius = scale;
    }
    IEnumerator CoroutinePlayDead() {
        m_rigid.isKinematic = true;
        m_rigid.velocity = Vector2.zero;
        m_input.DeactivateInput();
        GetComponent<Collider2D>().enabled = false;

        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(.5f);

        Time.timeScale = 0.5f;
        m_animator.SetTrigger("Dead");
        yield return new WaitForSecondsRealtime(1f);

        Time.timeScale = 1f;
        GameManager.Instance.RestartLevel();
        yield return null;
    }
    #region Input Action
    void OnMove(InputValue value){
        var moveAxis = value.Get<float>();
    //Flip sprite
        if(moveAxis != 0){
            m_animator.SetBool(moveBoolean, true);
            if (moveAxis < 0) m_sprite.flipX = false;
            else m_sprite.flipX = true;
        }
        else {
            m_animator.SetBool(moveBoolean, false);
        }

        direction = moveAxis;
    }
    void OnSprint(InputValue value)=>isRunning = value.isPressed;
    void OnCrouch(InputValue value){
        if (value.isPressed) {
            if(playerState == PLAYER_STATE.DEFAULT && onGround){
                playerState = PLAYER_STATE.CROUCH;
                m_animator.SetTrigger(crouchTrigger);
                ScaleCollisionBox(0.3f);
            }
        }
        else {
            if(playerState == PLAYER_STATE.CROUCH){
                playerState = PLAYER_STATE.DEFAULT;
                m_animator.SetTrigger(standTrigger);
                ScaleCollisionBox(0.4f);
            }
        }
    }
    void OnJump(InputValue value){
        if(playerState == PLAYER_STATE.DEFAULT && onGround) {
            airdriftSpeed = Mathf.Max(Mathf.Abs(m_rigid.velocity.x), minAirdriftSpeed);//以玩家起跳时的速度和最低的airdrift速度做比较，得到最后可行的速度
            transform.position += Vector3.up * (groundCheckRadius + 0.01f); //将玩家抬起来一点，以避免跳起的瞬间触发落地检测
            m_rigid.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            m_animator.SetTrigger(jumpTrigger);
            playerState = PLAYER_STATE.JUMP;
        }
    }
#endregion
}
