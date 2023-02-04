using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightPP_RT : MonoBehaviour
{
    [SerializeField] private int DownGrade = 1;
    private Camera cam;
    private Camera mainCam;
    private RenderTexture tempTex;
    private string lightPPMaskName = "_LightPPMaskTex";
    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
    void OnEnable()
    {
        mainCam = Camera.main;
        if (tempTex == null) tempTex = RenderTexture.GetTemporary(cam.pixelWidth / DownGrade, cam.pixelHeight / DownGrade, 0, RenderTextureFormat.R8);
        cam.targetTexture = tempTex;
        Shader.SetGlobalTexture(lightPPMaskName, cam.targetTexture);
    }
    private void LateUpdate()
    {
        cam.orthographicSize = mainCam.orthographicSize;
    }
    void OnDisable()
    {
        if (tempTex != null)
        {
            cam.targetTexture = null;
            RenderTexture.ReleaseTemporary(tempTex);
        }
    }
    void OnDestroy()
    {
        if (tempTex != null)
        {
            cam.targetTexture = null;
            RenderTexture.ReleaseTemporary(tempTex);
        }
    }
}
