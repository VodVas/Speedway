Shader "Custom/Fog 3.0"
{
    Properties
    {
        _FogColor("Fog Color", Color) = (0.4, 0.7, 1, 1)
        _Density("Density", Range(0,1)) = 0.5
        _HeightFalloff("Height Falloff", Float) = 100
    }

    SubShader
    {
        Tags { "RenderType"="Overlay" "Queue"="Overlay+1000" }
        Cull Front
        ZTest Always
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"

            struct AppData
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct V2F
            {
                float4 pos : SV_POSITION;
                float3 viewRay : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
            float4 _CameraDepthTexture_TexelSize;
            
            // Fog parameters
            float4 _FogColor;
            float _Density;
            float _HeightFalloff;
            float _BaseHeight;

            V2F vert(AppData v)
            {
                V2F o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                
                o.pos = UnityObjectToClipPos(v.vertex);
                float3 viewVector = mul(unity_CameraInvProjection, float4(0,0,1,1)).xyz;
                o.viewRay = mul(unity_CameraToWorld, float4(viewVector,0)).xyz;
                return o;
            }

            half4 frag(V2F i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                
                float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.pos.xy);
                float linearDepth = LinearEyeDepth(depth);
                float3 worldPos = _WorldSpaceCameraPos + linearDepth * i.viewRay;

                float heightFactor = saturate((worldPos.y - _BaseHeight) / _HeightFalloff);
                float fogFactor = exp(-_Density * linearDepth * heightFactor);
                
                half4 color = _FogColor;
                color.a = saturate(1.0 - fogFactor);
                return color;
            }
            ENDCG
        }
    }
}