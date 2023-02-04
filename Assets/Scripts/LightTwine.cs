using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTwine : MonoBehaviour
{
    [SerializeField] private float noiseScale = 0.1f;
    [SerializeField] private float noiseFreq = 1;
    private float timer;
    private void Awake()
    {
        timer = 0;
    }
    void Update()
    {
        timer += Time.deltaTime*noiseFreq;
        transform.localScale = Vector3.one * (1+Mathf.PerlinNoise(timer, 0.21351436f)*noiseScale);
    }
}
