Shader "Hidden/Custom/LightPP"
{
  HLSLINCLUDE
// StdLib.hlsl holds pre-configured vertex shaders (VertDefault), varying structs (VaryingsDefault), and most of the data you need to write common effects.
      #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
      #include "Assets/Shaders/DitherHelper.cginc"
      TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);

// Lerp the pixel color with the luminance using the _Blend uniform.
      uniform sampler2D _LightPPMaskTex;
      uniform sampler2D _DitherTex;
      float _Intensity;
      float _Darkness;

      float4 Frag(VaryingsDefault i) : SV_Target
      {
          float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
          float mask = tex2D(_LightPPMaskTex, i.texcoord).r;

          mask = floor(mask * 4) / 4;
          float dither = tex2D(_DitherTex, i.texcoord).r;
          mask = isDithered(i.texcoord, mask);

          color.rgb = lerp(color.rgb*_Darkness, color.rgb*(1+_Intensity), mask);

// Return the result
          return color;
      }
  ENDHLSL
  SubShader
  {
      Cull Off ZWrite Off ZTest Always
      Pass
      {
          HLSLPROGRAM
              #pragma vertex VertDefault
              #pragma fragment Frag
          ENDHLSL
      }
  }
}