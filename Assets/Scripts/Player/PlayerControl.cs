using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(PlayerInput))]
public class PlayerControl : MonoBehaviour, ISlowable{
    public float slowFactor { get; set; } = 1;
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
    [SerializeField] private float MaxjumpForce;
    [SerializeField] private float groundCheckRadius = 0.1f;
[Header("Crouch")]
    [SerializeField] private bool holdCrouch = false;
    [SerializeField] private bool headBlocked = false;
[Header("Sound")]
    [SerializeField] private AudioSource playerSource;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip deadClip;
    private float airdriftSpeed;
    private float jumpForce;
    private bool isRunning = false;
    private bool isDead = false;

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

    public Vector3 velocity { get { return m_rigid.velocity; } }
    private void Awake(){
        m_sprite   = GetComponentInChildren<SpriteRenderer>();
        m_collider = GetComponent<CircleCollider2D>();
        m_rigid    = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_input    = GetComponent<PlayerInput>();
        jumpForce  = MaxjumpForce;
    }
    void OnEnable(){
        EventHandler.E_OnBeforeSceneUnload += FreezeControl;
    }
    void OnDisable(){
        EventHandler.E_OnBeforeSceneUnload -= FreezeControl;
    }
    void Update(){
        GroundCheck();
        if(playerState == PLAYER_STATE.CROUCH)
        {
            CrouchHeadCheck();
        }
    }
    void FixedUpdate() {
        switch (playerState)
        {
            case PLAYER_STATE.DEFAULT:
                Move(isRunning ? runningSpeed : moveSpeed);
                break;
            case PLAYER_STATE.CROUCH:
                Move(crouchSpeed);
                if (!headBlocked && !holdCrouch)
                {
                    playerState = PLAYER_STATE.DEFAULT;
                    m_animator.SetTrigger(standTrigger);
                    ScaleCollisionBox(0.4f);
                }
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
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + Vector3.up * 0.62f, Vector3.up * 0.1f);
    }
    void FreezeControl()=>m_input.DeactivateInput();
    void GroundCheck(){
        if(Physics2D.OverlapCircle(transform.position, groundCheckRadius, platformLayer) != null) onGround = true;
        else onGround = false;
    }
    void CrouchHeadCheck()
    {
        if(Physics2D.Raycast(transform.position + Vector3.up*0.62f, Vector3.up, 0.1f, platformLayer))headBlocked = true;
        else headBlocked = false;
    }
    void Move(float speed) {
        var vel = m_rigid.velocity;
        vel.x = direction * speed * slowFactor;
        m_rigid.velocity = vel;
    }
    void AirDrift(float speed) {
        var vel = m_rigid.velocity;
        vel.x = Mathf.Lerp(vel.x, direction * speed, Time.fixedDeltaTime * speedSmooth);
        m_rigid.velocity = vel;
    }
    public void Kill(){
        if (!isDead){
            isDead = true;
            StartCoroutine(CoroutinePlayDead());
        }
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

        playerSource.PlayOneShot(deadClip);
        Time.timeScale = 0.15f;
        yield return new WaitForSecondsRealtime(.35f);

        Time.timeScale = 0.5f;
        m_animator.SetTrigger("Dead");
        yield return new WaitForSecondsRealtime(1f);

        Time.timeScale = 1f;
        GameManager.Instance.RestartLevel();
        yield return null;
    }
    #region Interaface
    public void SlowDown(float factor) {
        StartCoroutine(coroutineSlowingDown(factor));
        jumpForce = 0;
    }
    public void Recover()
    {
        slowFactor = 1;
        jumpForce = MaxjumpForce;
    }
    IEnumerator coroutineSlowingDown(float targetFactor)
    {
        float initFactor = slowFactor;
        for(float t=0; t<1; t += Time.deltaTime*2f)
        {
            slowFactor = Mathf.Lerp(initFactor, targetFactor, EasingFunc.Easing.SmoothInOut(t));
            yield return null;
        }
        slowFactor = targetFactor;
    }
    #endregion
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
            holdCrouch = true;
            if(playerState == PLAYER_STATE.DEFAULT && onGround){
                playerState = PLAYER_STATE.CROUCH;
                m_animator.SetTrigger(crouchTrigger);
                ScaleCollisionBox(0.3f);
            }
        }
        else {
            holdCrouch = false;
            if (playerState == PLAYER_STATE.CROUCH && !headBlocked)
            {
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

            playerSource.PlayOneShot(jumpClip);
        }
    }
#endregion
}
