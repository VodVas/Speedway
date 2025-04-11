Shader "Custom/RustEnergy" {
    Properties {
        // Основные текстуры PBR
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _PBRMap ("Occlusion (R), Roughness (G), Metallic (B)", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        
        // Параметры энергии
        _EnergyTex ("Energy Pattern (RGB) + Mask (A)", 2D) = "black" {}
        _EnergyColor ("Energy Color", Color) = (1,0.5,0,1)
        _Speed ("Energy Speed", Range(0,5)) = 1
        _PulseFreq ("Pulse Frequency", Range(0,3)) = 1
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        
        CGPROGRAM
        #pragma surface surf Standard
        #pragma target 3.0

        struct Input {
            float2 uv_MainTex;
            float2 uv_energy;
        };

        sampler2D _MainTex, _PBRMap, _NormalMap, _EnergyTex;
        float4 _EnergyColor;
        float _Speed, _PulseFreq;

        void surf (Input IN, inout SurfaceOutputStandard o) {
            // 1. PBR параметры
            float4 albedo = tex2D(_MainTex, IN.uv_MainTex);
            float4 pbr = tex2D(_PBRMap, IN.uv_MainTex);
            
            // 2. Корректное присвоение свойств
            o.Albedo = albedo.rgb;
            o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
            o.Occlusion = pbr.r; // Ambient Occlusion из R-канала
            o.Metallic = pbr.b; // Metallic из B-канала
            o.Smoothness = 1.0 - pbr.g; // Конвертация Roughness (G-канал) в Smoothness

            // 3. Эмиссия энергии
            float2 uvOffset = float2(_Time.y * _Speed, 0);
            float4 energyData = tex2D(_EnergyTex, IN.uv_energy + uvOffset);
            
            // 4. Пульсация с шумом для нестабильности
            float pulse = (sin(_Time.y * _PulseFreq) * 0.5 + 0.5) * energyData.a;
            
            // 5. Финальная эмиссия
            o.Emission = energyData.rgb * _EnergyColor * pulse
                        * smoothstep(0.2, 0.8, energyData.a);
        }
        ENDCG
    }
    FallBack "Diffuse"
}