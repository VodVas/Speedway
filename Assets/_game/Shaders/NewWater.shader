Shader "Custom/StylizedWater_Optimized"
{
    Properties
    {
        [Header(Base Colors)]
        _ShallowColor("Мелководье", Color) = (0.32, 0.68, 0.85, 0.8)
        _DeepColor("Глубина", Color) = (0.12, 0.35, 0.6, 0.9)
        _DepthContrast("Контраст глубины", Range(0.5, 3)) = 1.5

        [Header(Wave Settings)]
        _WaveSpeed("Скорость волн", Float) = 0.4
        _WaveScale("Высота волн", Range(0, 0.2)) = 0.1
        _NormalMap("Нормальная карта", 2D) = "bump" {}
        _RippleFrequency("Частота ряби", Range(1, 10)) = 5

        [Header(Effects)]
        _CausticsTex("Каустики", 2D) = "white" {}
        _FoamRamp("Градиент пены", 2D) = "white" {}
        [HDR]_FoamColor("Цвет пены", Color) = (1,1,1,1)
        _FoamThreshold("Порог пены", Range(0,1)) = 0.3
        _FoamSpeed("Скорость пены", Float) = 0.6
        _FresnelPower("Сила Френеля", Range(0,5)) = 2.5
    }

    SubShader
    {
        Tags { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent+300" 
            "IgnoreProjector" = "True" 
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma target 3.0

            #include "UnityCG.cginc"

            uniform half4 _ShallowColor, _DeepColor, _FoamColor;
            uniform sampler2D _CameraDepthTexture, _CausticsTex, _FoamRamp, _NormalMap;
            uniform half _DepthContrast, _WaveSpeed, _FoamSpeed, _FoamThreshold, _FresnelPower;
            uniform half _WaveScale, _RippleFrequency;
            uniform float4 _NormalMap_ST;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 screenPos : TEXCOORD0;
                half3 worldPos : TEXCOORD1;
                half2 noiseUV : TEXCOORD2;
                half2 foamUV : TEXCOORD3;
                half3 tSpace0 : TEXCOORD4;
                half3 tSpace1 : TEXCOORD5;
                half3 tSpace2 : TEXCOORD6;
                UNITY_FOG_COORDS(7)
            };

            v2f vert(appdata_full v)
            {
                v2f o;
                
                half time = _Time.y;
                half wave1 = sin(time * 2 + v.vertex.x * 10) * _WaveScale;
                half wave2 = sin(time * 3 + v.vertex.z * 8) * _WaveScale * 0.6;
                v.vertex.y += (wave1 + wave2) * 0.5;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.pos);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                
                o.noiseUV = v.texcoord * _NormalMap_ST.xy + _Time.x * _WaveSpeed;
                o.foamUV = o.worldPos.xz * 0.25 + _Time.x * _FoamSpeed;
                
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
                half3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w;
                o.tSpace0 = half3(worldTangent.x, worldBinormal.x, worldNormal.x);
                o.tSpace1 = half3(worldTangent.y, worldBinormal.y, worldNormal.y);
                o.tSpace2 = half3(worldTangent.z, worldBinormal.z, worldNormal.z);

                UNITY_TRANSFER_FOG(o, o.pos);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 screenUV = i.screenPos.xy / i.screenPos.w;
                float sceneDepth = LinearEyeDepth(tex2D(_CameraDepthTexture, screenUV).r);
                float surfaceDepth = i.screenPos.w;
                half depth = saturate((sceneDepth - surfaceDepth) * _DepthContrast);

                half3 waterColor = lerp(_ShallowColor.rgb, _DeepColor.rgb, depth);

                half3 normalMap = UnpackNormal(tex2D(_NormalMap, i.noiseUV * _RippleFrequency));
                half3 worldNormal;
                worldNormal.x = dot(i.tSpace0, normalMap);
                worldNormal.y = dot(i.tSpace1, normalMap);
                worldNormal.z = dot(i.tSpace2, normalMap);
                worldNormal = normalize(worldNormal);

                half3 lightDir = _WorldSpaceLightPos0.xyz;
                // Исправленная строка:
                half diff = saturate(dot(worldNormal, lightDir) * 0.5 + 0.5);
                waterColor *= diff;

                half3 caustics = tex2D(_CausticsTex, i.noiseUV).rgb;
                waterColor += caustics * (1 - depth) * 0.5;

                half foamPattern = tex2D(_FoamRamp, i.foamUV).r;
                half foamIntensity = saturate((1 - depth * 2) * foamPattern);
                half foam = step(_FoamThreshold, foamIntensity);
                waterColor = lerp(waterColor, _FoamColor.rgb, foam * _FoamColor.a);

                half3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                half fresnel = pow(1.0 - saturate(dot(worldNormal, viewDir)), _FresnelPower);
                waterColor += fresnel * 0.2;

                fixed4 finalColor = fixed4(waterColor, _ShallowColor.a);
                UNITY_APPLY_FOG(i.fogCoord, finalColor);
                
                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Mobile/Transparent/Diffuse"
}