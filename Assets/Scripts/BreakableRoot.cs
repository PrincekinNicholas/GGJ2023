using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableRoot : MonoBehaviour{
    [SerializeField] private SpriteRenderer rootSprite;
    [SerializeField] private Animation rootAnimation;
[Header("Break root")]
    [SerializeField] private GameObject breakedBranch;
    [SerializeField] private float MinHitVelocity = 6;
    [SerializeField] private int MaxHitAllowed = 3;
    private string stepOnRoot_Anim_Name = "StepOnRoot";
    private string stepOnRootHeavy_Anim_Name = "StepOnRoot Heavy";
    private int hitCount = 0;
    void OnCollisionEnter2D(Collision2D collision){
        if (collision.collider.tag == Service.playerTag) {
            if(Mathf.Abs(collision.relativeVelocity.y) > MinHitVelocity) { 
                Debug.Log("±¬²ÈÊ÷¸ù£¡£¡£¡");
                hitCount++;
                rootAnimation.Play(stepOnRootHeavy_Anim_Name);
                if (hitCount == MaxHitAllowed) {
                    rootSprite.gameObject.SetActive(false);
                    breakedBranch.gameObject.SetActive(true);
                    this.GetComponent<Collider2D>().enabled = false;
                }
            }
            else {
                rootAnimation.Play(stepOnRoot_Anim_Name);
            }
        }
    }
}
