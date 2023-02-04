using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(LightPPRenderer), PostProcessEvent.AfterStack, "Custom/LightPP")]
public class LightPP : PostProcessEffectSettings
{
    [Range(0f, 1f), Tooltip("Grayscale effect intensity.")]
    public FloatParameter intensity = new FloatParameter { value = 0f };
    [Range(0f, 1f)]
    public FloatParameter darkness = new FloatParameter { value = 0f };
    [Range(0, 2f)]
    public FloatParameter blurAmount = new FloatParameter { value = 0f };
    [Range(15, 100)]
    public IntParameter sampleAmount = new IntParameter { value = 15 };
    public override bool IsEnabledAndSupported(PostProcessRenderContext context){
        return enabled.value;
    }
}
public sealed class LightPPRenderer : PostProcessEffectRenderer<LightPP>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/LightPP"));
        sheet.properties.SetInt("_Sample", settings.sampleAmount);
        sheet.properties.SetFloat("_Itensity", settings.intensity);
        sheet.properties.SetFloat("_Darkness", settings.darkness);
        sheet.properties.SetFloat("_BlurSize", settings.blurAmount);

        var tempTex = RenderTexture.GetTemporary(context.width, context.height);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
