Shader "Custom/TerrainShader" {
    Properties {
        [MainTexture] _MainTex("Texture",2DArray) = "" {}
    }
    SubShader {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        Pass {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes {
                float4 position : POSITION;
                float3 uv : TEXCOORD0;
            };

            struct Varyings {
                float4 vertex : SV_POSITION;
                float3 uv : TEXCOORD0;
                float4 shadowCoords : TEXCOORD3;
            };

            TEXTURE2D_ARRAY(_MainTex);
            SAMPLER(sampler_MainTex);

            Varyings vert(Attributes IN) {
                Varyings OUT;
                OUT.vertex = TransformObjectToHClip(IN.position.xyz);
                OUT.uv = IN.uv;

                VertexPositionInputs positionsInputs = GetVertexPositionInputs(IN.position.xyz);
                OUT.shadowCoords = GetShadowCoord(positionsInputs);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target {
                half shadowAmount = MainLightRealtimeShadow(IN.shadowCoords);
                return SAMPLE_TEXTURE2D_ARRAY(_MainTex,sampler_MainTex,IN.uv.xy,IN.uv.z) * (shadowAmount <= 0.3 ? 0.3 : shadowAmount);
            }

            ENDHLSL
        }
    }
    FallBack "Diffuse"
}