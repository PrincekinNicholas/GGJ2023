using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resin : MonoBehaviour
{
    [SerializeField, Range(0,1)] private float slowdownFactor = 0.2f;
    [SerializeField] private ParticleSystem m_vfx_mudDot;
    List<ISlowable> slowableList = new List<ISlowable>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var slowable = collision.GetComponent<ISlowable>();
        if (slowable != null)
        {
            if (!slowableList.Contains(slowable))
            {
                slowableList.Add(slowable);
                slowable.SlowDown(slowdownFactor);
            }
            if(collision.tag == Service.playerTag)
            {
                m_vfx_mudDot.Play();
            }
        }
    }
    private void Update()
    {
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var slowable = collision.GetComponent<ISlowable>();
        if(slowable != null)
        {
            if (slowableList.Contains(slowable))
            {
                slowableList.Remove(slowable);
                slowable.Recover();
            }
        }
    }
}