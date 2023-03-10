using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableRoot : MonoBehaviour{
    [SerializeField] private SpriteRenderer rootSprite;
    [SerializeField] private Animation rootAnimation;
[Header("Break root")]
    [SerializeField] private GameObject breakedBranch;
    [SerializeField] private bool canBreak = true;
    [SerializeField] private float MinHitVelocity = 6;
    [SerializeField] private int MaxHitAllowed = 3;
[Header("Audio")]
    [SerializeField] private AudioSource rootSource;
    [SerializeField] private AudioClip rootClip;
    private string stepOnRoot_Anim_Name = "StepOnRoot";
    private string stepOnRootHeavy_Anim_Name = "StepOnRoot Heavy";
    private int hitCount = 0;
    private bool alreadyBroken = false;
    void OnCollisionEnter2D(Collision2D collision){
        if (collision.collider.tag == Service.playerTag) {
            if(Mathf.Abs(collision.relativeVelocity.y) > MinHitVelocity) { 
                Debug.Log("??????????????");
                if(canBreak)hitCount++;
                rootAnimation.Play(stepOnRootHeavy_Anim_Name);
                if (hitCount == MaxHitAllowed) {
                    rootSource.PlayOneShot(rootClip, 1f);

                    rootSprite.gameObject.SetActive(false);
                    breakedBranch.gameObject.SetActive(true);
                    this.GetComponent<Collider2D>().enabled = false;
                    alreadyBroken = true;

                    EventHandler.Call_OnBreakRoot();
                }
                else{
                    rootSource.PlayOneShot(rootClip, 0.1f);
                }
            }
            else {
                rootAnimation.Play(stepOnRoot_Anim_Name);
            }
        }
    }

    public bool GetAlreadyBorkenValue()
    {
        rootSource.PlayOneShot(rootClip, 1);
        return alreadyBroken;
    }
}
