using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAudioSource : MonoBehaviour
{
    [SerializeField] private float maxVolume = 0.3f;
    [SerializeField] private float musicFadeTime = 2;
    [SerializeField] private AudioSource m_audio;
    private void Awake()
    {
        EventHandler.E_OnBeforeSceneUnload += FadeOutMusic;
    }
    private void OnDestroy()
    {
        EventHandler.E_OnBeforeSceneUnload -= FadeOutMusic;
    }
    private void Start()
    {
        FadeInMusic();
    }
    void FadeOutMusic()
    {
        StartCoroutine(coroutineFadeMusic(false));
    }
    void FadeInMusic()
    {
        StartCoroutine(coroutineFadeMusic(true));
    }
    IEnumerator coroutineFadeMusic(bool isFadingIn)
    {
        float initVolume = m_audio.volume;
        float targetVolume = isFadingIn ? maxVolume : 0;
        if (isFadingIn) m_audio.Play();
        for (float t=0; t<1; t+=Time.deltaTime/ musicFadeTime)
        {
            m_audio.volume = Mathf.Lerp(initVolume, targetVolume, EasingFunc.Easing.SmoothInOut(t));
            yield return null;
        }
        m_audio.volume = targetVolume;
        if (!isFadingIn) m_audio.Stop();
    }
}
