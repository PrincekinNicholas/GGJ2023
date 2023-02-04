using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resin : MonoBehaviour
{
    [SerializeField, Range(0,1)] private float slowdownFactor = 0.2f;
    [SerializeField] private ParticleSystem m_vfx_mudDot;
[Header("Audio")]
    [SerializeField] private AudioSource resinSource;
    List<ISlowable> slowableList = new List<ISlowable>();
    PlayerControl control;
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
                control = collision.gameObject.GetComponent<PlayerControl>();
                m_vfx_mudDot.Play();
            }
        }
    }
    private void Update()
    {
        if(control != null)
        {
            Vector3 pos = m_vfx_mudDot.transform.position;
            pos.x = control.transform.position.x;
            m_vfx_mudDot.transform.position = pos;
            if (control)
            {
                if(!resinSource.isPlaying) resinSource.Play();
                //resinSource.volume = Mathf.Lerp();
            }
            if (control == null && resinSource.isPlaying) resinSource.Stop();
        }
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
            if (collision.tag == Service.playerTag)
            {
                control = null;
                m_vfx_mudDot.Stop();
            }
        }
    }
}