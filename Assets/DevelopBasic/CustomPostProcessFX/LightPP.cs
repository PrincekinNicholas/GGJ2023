using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(LightPPRenderer), PostProcessEvent.BeforeStack, "Custom/LightPP")]
public class LightPP : PostProcessEffectSettings
{
    [Range(0f, 100f), Tooltip("Grayscale effect intensity.")]
    public FloatParameter intensity = new FloatParameter { value = 0f };
    [Range(0f, 1f)]
    public FloatParameter darkness = new FloatParameter { value = 0f };
    public override bool IsEnabledAndSupported(PostProcessRenderContext context){
        return enabled.value;
    }
}
public sealed class LightPPRenderer : PostProcessEffectRenderer<LightPP>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/LightPP"));
        sheet.properties.SetFloat("_Intensity", settings.intensity);
        sheet.properties.SetFloat("_Darkness", settings.darkness);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
