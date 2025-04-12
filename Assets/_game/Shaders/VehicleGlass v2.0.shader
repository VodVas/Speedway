Shader "Custom/VehicleGlass v2.0"
{
    Properties
    {
        [Header(Base Settings)]
        _Color("Glass Tint", Color) = (0.9, 0.95, 1, 1)
        _Transparency("Transparency", Range(0.3, 1)) = 0.85
        [Space]
        
        [Header(Reflections)]
        _RefIntensity("Reflection Strength", Range(0, 0.8)) = 0.3
        [NoScaleOffset] _Cube("Cubemap (Mip Blurred)", Cube) = "black" {}
        [NoScaleOffset] _RenderedTex("Screen Texture", 2D) = "black" {}
        [Space]
        
        [Header(Advanced Transparency)]
        _FresnelPower("Edge Falloff", Range(0.5, 5)) = 2.5
        _EdgeBrightness("Edge Brightness", Range(0, 1)) = 0.15
    }

    SubShader
    {
        Tags { 
            "Queue" = "Transparent+100"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
            "ForceNoShadowCasting" = "True"
        }

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile __ USE_CUBEMAP_REFLECTIONS
            #pragma multi_compile __ USE_SCREEN_TEXTURE

            #include "UnityCG.cginc"

            #define FIXED_MIP_LEVEL 2
            #define ALPHA_BOOST 1.25

            uniform half4 _Color;
            uniform half _Transparency;
            uniform half _RefIntensity;
            uniform half _FresnelPower;
            uniform half _EdgeBrightness;
            uniform samplerCUBE _Cube;
            uniform sampler2D _RenderedTex;

            struct v2f
            {
                float4 pos : SV_POSITION;
                half3 refl : TEXCOORD0;
                half3 normal : TEXCOORD1;
                half3 viewDir : TEXCOORD2;
                #if USE_SCREEN_TEXTURE
                    half2 screenUV : TEXCOORD3;
                #endif
                UNITY_FOG_COORDS(4)
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, v.vertex).xyz);
                o.refl = reflect(-o.viewDir, o.normal);
                
                #if USE_SCREEN_TEXTURE
                    o.screenUV = ComputeScreenPos(o.pos).xy;
                #endif
                
                UNITY_TRANSFER_FOG(o, o.pos);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                half4 col = _Color;
                col.a = _Transparency * ALPHA_BOOST;
                
                half fresnel = saturate(1.0 - dot(normalize(i.viewDir), normalize(i.normal)));
                fresnel = pow(fresnel, _FresnelPower);
                col.a = lerp(col.a, col.a * 0.7, fresnel);
                
                half3 reflections = 0;
                
                #if USE_CUBEMAP_REFLECTIONS
                    half4 cubemap = texCUBElod(_Cube, half4(i.refl, FIXED_MIP_LEVEL));
                    reflections += cubemap.rgb * _RefIntensity;
                #endif
                
                #if USE_SCREEN_TEXTURE
                    half3 screenRef = tex2D(_RenderedTex, i.screenUV).rgb;
                    reflections += screenRef * (_RefIntensity * 0.6);
                #endif
                
                col.rgb += reflections * (1.0 - col.a);
                col.rgb += fresnel * _EdgeBrightness;
                
                col.a = saturate(col.a);
                
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }

    FallBack "Mobile/Transparent/VertexLit"
    CustomEditor "Apocalypse.VehicleGlassAdvanced_Editor"
}