Shader "Hidden/Custom/LightPP"
{
  HLSLINCLUDE
// StdLib.hlsl holds pre-configured vertex shaders (VertDefault), varying structs (VaryingsDefault), and most of the data you need to write common effects.
      #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

// Lerp the pixel color with the luminance using the _Blend uniform.
      uniform sampler2D _LightPPMaskTex;
      uniform sampler2D _MainTex;
      float4 _MainTex_ST;

      half _BlurSize;
      half _Sample;

      float _Intensity;
      float _Darkness;

       float4 Frag_Vertical(VaryingsDefault i) : SV_Target{
           float4 col = 0;

           for(float index = 0; index < _Sample; index++){
               float2 uv = i.texcoord + float2(0, (index/(_Sample-1) - 0.5) * _BlurSize);
               col += tex2D(_MainTex, uv);
           }
           col /= _Sample;
           return col;
       }
       float4 Frag_Horizontal(VaryingsDefault i) : SV_Target{
           float invAspect = _ScreenParams.y/_ScreenParams.x;
           float4 col = 0;

           for(float index = 0; index < _Sample; index++){
               float2 uv = i.texcoord + float2((index/(_Sample-1) - 0.5) * _BlurSize * invAspect, 0);
               col += tex2D(_MainTex, uv);
           }
           col = col/_Sample;
           return col;
       }
      float4 Frag(VaryingsDefault i) : SV_Target
      {
          float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
          float mask = tex2D(_LightPPMaskTex, i.texcoord).r;
          
          color.rgb = lerp(color.rgb*_Darkness, color.rgb*(1+_Intensity), mask);
          color.a = lerp(color.a, color.a * (1 + _Intensity), mask);
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