using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntactSkull : MonoBehaviour
{
    [SerializeField] private bool RightAsStart = true;
    [SerializeField] private float floatScale;
    [SerializeField] private float floatFreq;
    [SerializeField] private Transform skullRoot;
[Header("Look Direction Rnandom")]
    [SerializeField] private SpriteRenderer m_head;
    [SerializeField] private SpriteRenderer m_jaw;
[Header("Separate")]
    [SerializeField] private Animation separate_Animation;
    [SerializeField] private SkullPieces headPiece;
    [SerializeField] private SkullPieces jawPieces;
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip alertClip;
    private float direction = 1;
    private float floatDelta;
    private float jawTurningDelta;
    private bool separated = false;
    // Start is called before the first frame update
    void Start(){
        floatDelta = 0;
        direction = RightAsStart ? 1 : -1;
    }

    // Update is called once per frame
    void Update()
    {
        floatDelta += Time.deltaTime * floatFreq;
        skullRoot.localPosition = new Vector3(floatScale*(2*Mathf.PerlinNoise(floatDelta+0.2251f, 0.3251f)-1), floatScale * (2 *Mathf.PerlinNoise(floatDelta, 0.7525f)-1), 0);

        jawTurningDelta += Time.deltaTime * 0.25f;
        if (jawTurningDelta >= 1)
        {
            jawTurningDelta = 0;
            direction *= -1;
            skullRoot.localScale = new Vector3(direction, 1, 1);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == Service.playerTag)
        {
        //如果骷髅面朝玩家
            if(direction * (collision.transform.position.x - transform.position.x)>0)
            {
                if (!separated)
                {
                    separated = true;
                    this.enabled = false;
                    audioSource.PlayOneShot(alertClip);
                    StartCoroutine(coroutineSeparate());
                    GetComponent<Collider2D>().enabled = false;
                }
            }
        }
    }
    IEnumerator coroutineSeparate()
    {
        separate_Animation.Play();

        Vector3 initPos = skullRoot.position;
        Vector3 targetPos = skullRoot.position - Vector3.right * direction + Vector3.up * 0.2f;

        float length = separate_Animation.clip.length;
        for (float t=0; t<1; t += Time.deltaTime / length)
        {
            skullRoot.position = Vector3.Lerp(initPos, targetPos, EasingFunc.Easing.QuadEaseOut(t));
            yield return null;
        }
        skullRoot.position = targetPos;
        headPiece.enabled = true;
        jawPieces.enabled = true;
    }
}
