Shader "Custom/MobileFire" {
    Properties {
        _MainTex ("Fire Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _Color ("Fire Color", Color) = (1,0.3,0,1)
        _Speed ("Animation Speed", Range(0, 5)) = 1
        _Intensity ("Fire Intensity", Range(0, 2)) = 1
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
    }

    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True"}
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv_main : TEXCOORD0;
                float2 uv_noise : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _MainTex_ST;
            float4 _NoiseTex_ST;
            fixed4 _Color;
            half _Speed;
            half _Intensity;
            half _Cutoff;

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                
                o.uv_main = TRANSFORM_TEX(v.uv, _MainTex) + float2(0, _Time.x * _Speed);
                o.uv_noise = TRANSFORM_TEX(v.uv, _NoiseTex) + float2(0, _Time.x * _Speed * 0.5);
                
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 fire = tex2D(_MainTex, i.uv_main);
                fixed noise = tex2D(_NoiseTex, i.uv_noise).r;
                
                half alpha = fire.a * noise * _Intensity;
                clip(alpha - _Cutoff);
                
                fixed4 col = _Color * fire;
                col.a = alpha;
                
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Mobile/Particles/Alpha Blended"
}