using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator), typeof(PlayerInput))]
public class PlayerControl : MonoBehaviour{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runningSpeed;
    private bool isRunning = false;
    private PLAYER_STATE playerState = PLAYER_STATE.ON_GROUND;

    private SpriteRenderer m_sprite;
    private Animator m_animator;
    private PlayerInput m_input;
    private float direction = 0;
    private void Awake(){
        m_sprite   = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
        m_input    = GetComponent<PlayerInput>();
    }
    void Update(){
        switch (playerState) {
            case PLAYER_STATE.ON_GROUND:
                transform.position += Vector3.right * direction * (isRunning? runningSpeed:moveSpeed) * Time.deltaTime;
                break;
            case PLAYER_STATE.CROUCH:

                break;
            case PLAYER_STATE.JUMP:

                break;
        }
    }
    void GroundCheck(){
        
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
        if(playerState == PLAYER_STATE.ON_GROUND){
            playerState = PLAYER_STATE.CROUCH;
        }
    }
    void OnJump(InputValue value){
        if(playerState == PLAYER_STATE.ON_GROUND) {
            playerState = PLAYER_STATE.JUMP;
        }
    }
#endregion
}
