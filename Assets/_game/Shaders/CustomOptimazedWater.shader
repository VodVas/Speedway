Shader "Custom/OptimizedWaterWithEdge" 
{
    Properties {
        _MainColor ("Water Color", Color) = (0.2, 0.6, 1, 0.5)
        _DepthColor ("Depth Color", Color) = (0.1, 0.3, 0.6, 1)
        _WaveSpeed ("Wave Speed", Range(0, 2)) = 0.5
        _WaveScale ("Wave Scale", Range(0, 0.5)) = 0.1
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _RippleFrequency ("Ripple Frequency", Range(1, 10)) = 5
        _FoamStrength ("Foam Strength", Range(0, 1)) = 0.3
        _FoamThreshold ("Foam Threshold", Range(0, 1)) = 0.8
        _VolumeStrength ("Volume Strength", Range(0, 1)) = 0.5
        
        // Добавленные параметры для окаймления
        [Header(Edge Settings)]
        _EdgeColor("Цвет кромки", Color) = (1,1,1,1)
        _EdgeDistance("Дистанция кромки", Range(0, 2)) = 0.5
        _EdgeFalloff("Сглаживание", Range(0.01, 1)) = 0.2
    }

    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 100

        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile_fog
            
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                half2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                UNITY_FOG_COORDS(2)
                half3 viewDir : TEXCOORD3;
                half3 worldPos : TEXCOORD4;
                half waveValue : TEXCOORD5;
            };

            sampler2D _NormalMap;
            sampler2D _CameraDepthTexture;
            float4 _NormalMap_ST;
            fixed4 _MainColor;
            fixed4 _DepthColor;
            fixed4 _EdgeColor;
            half _WaveSpeed;
            half _WaveScale;
            half _RippleFrequency;
            half _FoamStrength;
            half _FoamThreshold;
            half _VolumeStrength;
            half _EdgeDistance;
            half _EdgeFalloff;

            v2f vert (appdata v) {
                v2f o;
                
                half timeFactor = _Time.y * 2;
                half posFactor = v.vertex.x * 10;
                half wave = sin(timeFactor + posFactor) * _WaveScale;
                v.vertex.y += wave;
                
                half timeFactor2 = _Time.y * 3 + 1.57;
                half posFactor2 = v.vertex.z * 8;
                half wave2 = sin(timeFactor2 + posFactor2) * (_WaveScale * 0.6);
                v.vertex.y += wave2;
                                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _NormalMap);
                o.screenPos = ComputeScreenPos(o.vertex);
                o.waveValue = (wave + wave2) / (_WaveScale * 1.6);
                
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.viewDir = normalize(UnityWorldSpaceViewDir(mul(unity_ObjectToWorld, v.vertex)));
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // Эффект кромки
                float2 screenUV = i.screenPos.xy / i.screenPos.w;
                float sceneDepth = LinearEyeDepth(tex2D(_CameraDepthTexture, screenUV));
                float surfaceDepth = i.screenPos.w;
                float depthDifference = sceneDepth - surfaceDepth;
                
                half edge = 1 - smoothstep(_EdgeDistance, _EdgeDistance + _EdgeFalloff, depthDifference);
                fixed4 edgeColor = _EdgeColor * edge;

                // Остальная логика
                half2 rippleOffset1 = half2(_Time.x * _WaveSpeed * 0.8, _Time.x * _WaveSpeed * 0.6);
                half2 rippleOffset2 = half2(_Time.x * _WaveSpeed * -0.7, _Time.x * _WaveSpeed * 0.9);
                
                half2 uv1 = (i.uv * _RippleFrequency) + rippleOffset1;
                half2 uv2 = (i.uv * (_RippleFrequency * 0.66)) * half2(-1,1) + rippleOffset2;
                
                half3 normal1 = UnpackNormal(tex2D(_NormalMap, uv1));
                half3 normal2 = UnpackNormal(tex2D(_NormalMap, uv2));
                half3 combinedNormal = normalize(half3(normal1.xy + normal2.xy, normal1.z * normal2.z));
                
                half3 dx = ddx(i.worldPos);
                half3 dy = ddy(i.worldPos);
                half3 volumeNormal = normalize(cross(dy, dx));
                half3 finalNormal = normalize(combinedNormal + volumeNormal * _VolumeStrength);
                
                half3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                half diff = saturate(dot(finalNormal, lightDir) * 0.5 + 0.5);
                
                half foam = smoothstep(_FoamThreshold, 1.0, i.waveValue);
                foam *= _FoamStrength;
                
                half fresnel = 1.0 - saturate(dot(finalNormal, i.viewDir));
                fresnel = pow(fresnel, 4) * saturate(length(combinedNormal.xy) * 2.0);
                
                fixed4 waterColor = lerp(_MainColor, _DepthColor, fresnel * 1.2);
                waterColor.rgb = lerp(waterColor.rgb, fixed3(1,1,1), foam);
                waterColor.rgb *= diff;

                // Смешивание с кромкой
                waterColor.rgb = lerp(waterColor.rgb, edgeColor.rgb, edgeColor.a);
                waterColor.a = _MainColor.a * (0.8 + fresnel * 0.4 + foam * 0.3);
                
                UNITY_APPLY_FOG(i.fogCoord, waterColor);
                return waterColor;
            }
            ENDCG
        }
    }
    FallBack "Mobile/Transparent/Vertex Color"
}