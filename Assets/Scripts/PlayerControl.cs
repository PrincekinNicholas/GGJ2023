using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator), typeof(PlayerInput))]
public class PlayerControl : MonoBehaviour{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runningSpeed;
    private bool isRunning = false;

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
        transform.position += Vector3.right * direction * (isRunning? runningSpeed:moveSpeed) * Time.deltaTime;
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

    }
    void OnJump(InputValue value){

    }
#endregion
}
