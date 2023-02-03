using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator), typeof(PlayerInput))]
public class PlayerControl : MonoBehaviour{
    [SerializeField] private PLAYER_STATE playerState = PLAYER_STATE.DEFAULT;
[Header("Basic Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float crouchSpeed;
[Header("Jump")]
    [SerializeField] private bool onGround = true;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private float groundCheckRadius = 0.1f;
    private bool isRunning = false;
    
    private Rigidbody2D m_rigid;
    private SpriteRenderer m_sprite;
    private Animator m_animator;
    private PlayerInput m_input;
    private float direction = 0;
    private void Awake(){
        m_rigid    = GetComponent<Rigidbody2D>();
        m_sprite   = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
        m_input    = GetComponent<PlayerInput>();
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
                if (onGround) {
                    playerState = PLAYER_STATE.DEFAULT;
                }
                break;
        }
    }
    void OnDrawGizmos(){
        DebugExtension.DrawCircle(transform.position, Vector3.forward, Color.green, groundCheckRadius);
    }
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
#region Input Action
    void OnMove(InputValue value){
        var moveAxis = value.Get<float>();
    //Flip sprite
        if(moveAxis != 0){
            if (moveAxis < 0) m_sprite.flipX = false;
            else m_sprite.flipX = true;
        }

        direction = moveAxis;
    }
    void OnSprint(InputValue value)=>isRunning = value.isPressed;
    void OnCrouch(InputValue value){
        if (value.isPressed) {
            if(playerState == PLAYER_STATE.DEFAULT && onGround){
                playerState = PLAYER_STATE.CROUCH;
                transform.localScale = new Vector3(1, 0.8f, 1);
            }
        }
        else {
            if(playerState == PLAYER_STATE.CROUCH){
                playerState = PLAYER_STATE.DEFAULT;
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }
    void OnJump(InputValue value){
        if(playerState == PLAYER_STATE.DEFAULT && onGround) {
            playerState = PLAYER_STATE.JUMP;
            m_rigid.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }
    }
#endregion
}
