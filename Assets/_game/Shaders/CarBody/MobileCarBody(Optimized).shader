Shader "Mobile/Car Body (Optimized)"
{
    Properties
    {
        // Base
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Diffuse (RGB)", 2D) = "white" {}
        
        // Decal
        _DecalColor("Decal Color", Color) = (1,1,1,1)
        _DecalTex("Decal (RGBA)", 2D) = "white" {}
        _DecalBlend("Decal Blend", Range(0,1)) = 0.8
        
        // Pearl
        _Pearlescent("Pearlescent", Range(0,2)) = 0.5
        _PearlColor("Pearl Color", Color) = (1,1,1,1)
        _FresnelPower("Fresnel Power", Range(1,5)) = 3
        
        // Reflection
        _ReflectionCube("Reflection Cubemap", Cube) = "" {}
        _ReflectionStrength("Reflection Strength", Range(0,1)) = 0.3
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 150

        CGPROGRAM
        #pragma surface surf MobileBlinnPhong exclude_path:prepass nolightmap noforwardadd
        #pragma target 3.0

        // Lighting model optimization
        inline fixed4 LightingMobileBlinnPhong (SurfaceOutput s, fixed3 lightDir, fixed3 halfDir, fixed atten)
        {
            fixed diff = max(0, dot(s.Normal, lightDir));
            fixed nh = max(0, dot(s.Normal, halfDir));
            fixed spec = pow(nh, s.Specular*128) * s.Gloss;
            
            fixed4 c;
            c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * atten;
            c.a = 0;
            return c;
        }

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_DecalTex;
            float3 worldRefl;
            float3 viewDir;
        };

        // Properties
        sampler2D _MainTex;
        sampler2D _DecalTex;
        samplerCUBE _ReflectionCube;
        
        fixed4 _Color;
        fixed4 _DecalColor;
        fixed4 _PearlColor;
        half _Pearlescent;
        half _FresnelPower;
        half _ReflectionStrength;
        half _DecalBlend;

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Base Color & Decal
            fixed4 mainTex = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 decal = tex2D(_DecalTex, IN.uv_DecalTex);
            
            // Albedo Composition
            o.Albedo = lerp(mainTex.rgb * _Color.rgb, 
                          decal.rgb * _DecalColor.rgb, 
                          decal.a * _DecalBlend);

            // Pearlescent Effect
            half fresnel = pow(1.0 - saturate(dot(normalize(IN.viewDir), o.Normal)), _FresnelPower);
            o.Emission = _PearlColor.rgb * fresnel * _Pearlescent;

            // Cubemap Reflection
            fixed3 reflection = texCUBE(_ReflectionCube, IN.worldRefl).rgb;
            o.Emission += reflection * _ReflectionStrength * (1 - decal.a);

            // Specular Setup
            o.Specular = 32; // Контролирует размер блика
            o.Gloss = 0.5;    // Контролирует интенсивность блика
        }
        ENDCG
    }
    FallBack "Mobile/VertexLit"
    CustomEditor "CustomCarShaderEditor"
}