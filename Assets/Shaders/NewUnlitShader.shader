Shader "Custom/URP_InvertedHullOutline"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (0,250,0,1)
        _OutlineWidth ("Outline Width", Range(0.0, 0.1)) = 0.03
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry+1" }
        Cull Front
        ZWrite On
        ZTest LEqual

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            float4 _OutlineColor;
            float _OutlineWidth;

            Varyings vert(Attributes input)
            {
                Varyings output;

                // Normalize the normal in object space
                float3 normal = normalize(input.normalOS);

                // Offset position along normal
                float3 posOffset = input.positionOS + normal * _OutlineWidth;

                // Transform to clip space (HClip = Homogeneous Clip Space)
                output.positionCS = TransformObjectToHClip(posOffset);

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                return _OutlineColor;
            }

            ENDHLSL
        }
    }
    FallBack "Universal Forward"
}

