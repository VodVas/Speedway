Shader "Custom/Advanced Fog"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _FogColorStart ("Near Color", Color) = (0.44, 0.72, 1, 1)
        _FogColorEnd ("Far Color", Color) = (0.89, 1.44, 2, 1)
        _FogDistanceStart ("Distance Start", Float) = 0
        _FogDistanceEnd ("Distance End", Float) = 100
        _FogHeightStart ("Height Start", Float) = 0
        _FogHeightEnd ("Height End", Float) = 50
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _NoiseScale ("Noise Scale", Range(0.01, 1)) = 0.1
        _NoiseSpeed ("Noise Speed", Vector) = (0.5, 0.5, 0, 0)
        [Toggle]_UseNoise ("Enable Noise", Float) = 1
    }

    SubShader
    {
        Tags { 
            "RenderType" = "Overlay" 
            "Queue" = "Overlay+1000" 
        }

        Pass
        {
            Cull Off
            ZWrite Off
            ZTest Always

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile __ _USENOISE_ON

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
            float4 _MainTex_ST;
            
            half4 _FogColorStart;
            half4 _FogColorEnd;
            half _FogDistanceStart;
            half _FogDistanceEnd;
            half _FogHeightStart;
            half _FogHeightEnd;
            sampler2D _NoiseTex;
            half _NoiseScale;
            half2 _NoiseSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                // Calculate world position from depth
                float depth = UNITY_SAMPLE_DEPTH(tex2Dlod(_CameraDepthTexture, float4(o.uv, 0, 0)));
                float4 pos = mul(unity_CameraToWorld, float4(o.uv * 2.0 - 1.0, depth * 2.0 - 1.0, 1.0));
                o.worldPos = pos.xyz / pos.w;
                
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // Base scene color
                half4 sceneColor = tex2D(_MainTex, i.uv);
                
                // Distance calculation
                half dist = distance(i.worldPos, _WorldSpaceCameraPos);
                half fogDist = saturate((dist - _FogDistanceStart) / (_FogDistanceEnd - _FogDistanceStart));
                
                // Height calculation
                half height = i.worldPos.y;
                half fogHeight = saturate((height - _FogHeightStart) / (_FogHeightEnd - _FogHeightStart));
                
                // Combined fog factor
                half fogFactor = fogDist * fogHeight;
                
                // Noise calculation
                #ifdef _USENOISE_ON
                    half2 noiseUV = i.worldPos.xz * _NoiseScale + _Time.y * _NoiseSpeed;
                    half noise = tex2D(_NoiseTex, noiseUV).r;
                    fogFactor *= noise;
                #endif
                
                // Final color blending
                half3 fogColor = lerp(_FogColorStart.rgb, _FogColorEnd.rgb, fogDist);
                half3 finalColor = lerp(sceneColor.rgb, fogColor, fogFactor);
                
                return half4(finalColor, sceneColor.a);
            }
            ENDCG
        }
    }
}