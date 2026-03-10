Shader "EGA/Particles/FireSphere_URP"
{
    Properties
    {
        _MainTex("Main Tex", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _Emission("Emission", Float) = 2
        _StartFrequency("Start Frequency", Float) = 4
        _Frequency("Frequency", Float) = 10
        _Amplitude("Amplitude", Float) = 1
        
        [Toggle] _Usedepth("Use depth?", Float) = 0
        _Depthpower("Depth power", Float) = 1
        
        [Toggle] _Useblack("Use black", Float) = 0
        _Opacity("Opacity", Float) = 1
        
        [HideInInspector] _tex3coord("", 2D) = "white" {}
        [HideInInspector] __dirty("", Int) = 1
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent+0" 
            "IgnoreProjector" = "True" 
            "RenderPipeline" = "UniversalPipeline"
        }
        
        LOD 100
        Cull Back
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha // Alpha Fade Blending

        Pass
        {
            Name "Unlit"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            
            // Required URP libraries
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float4 color        : COLOR;
                float3 uv           : TEXCOORD0; // Using float3 to match ASE generation
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float4 color        : COLOR;
                float3 uv           : TEXCOORD0;
                float4 screenPos    : TEXCOORD1;
            };

            // Texture Declarations
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            // SRP Batcher compatibility for properties
            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float _Emission;
                float _StartFrequency;
                float _Frequency;
                float _Amplitude;
                float _Usedepth;
                float _Depthpower;
                float _Useblack;
                float _Opacity;
                float4 _MainTex_ST;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                
                // standard URP transform
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.color = input.color;
                output.uv = input.uv;
                
                // Calculate screen position for depth fading
                output.screenPos = ComputeScreenPos(output.positionCS);
                
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                // -- ASE NOISE & UV MATH RECREATION --
                
                float4 temp_output_121_0 = (_Emission * _Color * input.color);
                
                float2 temp_output_8_0 = ((float2(0.2, 0) * _Time.y) + input.uv.xy + input.uv.z) * _StartFrequency;
                float2 break18 = floor(temp_output_8_0);
                float temp_output_21_0 = (break18.x + (break18.y * 57.0));
                
                float2 temp_output_10_0 = frac(temp_output_8_0);
                float2 temp_cast_1 = float2(3.0, 3.0);
                float2 break17 = (temp_output_10_0 * temp_output_10_0 * (temp_cast_1 - (temp_output_10_0 * 2.0)));
                
                float lerpResult39 = lerp(frac((473.5 * sin(temp_output_21_0))), frac((473.5 * sin((1.0 + temp_output_21_0)))), break17.x);
                float lerpResult38 = lerp(frac((473.5 * sin((57.0 + temp_output_21_0)))), frac((473.5 * sin((58.0 + temp_output_21_0)))), break17.x);
                float lerpResult40 = lerp(lerpResult39, lerpResult38, break17.y);
                
                float3 temp_output_51_0 = ((float3((float2(0.5, 0.5) * _Time.y), 0.0) + (input.uv * (lerpResult40 * _Amplitude)) + input.uv.z) * _Frequency);
                float3 break87 = floor(temp_output_51_0);
                float temp_output_90_0 = (break87.x + (break87.y * 57.0));
                
                float3 temp_output_52_0 = frac(temp_output_51_0);
                float3 temp_cast_3 = float3(3.0, 3.0, 3.0);
                float3 break110 = (temp_output_52_0 * temp_output_52_0 * (temp_cast_3 - (temp_output_52_0 * 2.0)));
                
                float lerpResult109 = lerp(frac((473.5 * sin(temp_output_90_0))), frac((473.5 * sin((1.0 + temp_output_90_0)))), break110.x);
                float lerpResult105 = lerp(frac((473.5 * sin((57.0 + temp_output_90_0)))), frac((473.5 * sin((58.0 + temp_output_90_0)))), break110.x);
                float lerpResult106 = lerp(lerpResult109, lerpResult105, break110.y);
                
                float Amp114 = _Amplitude;
                float2 finalUV = input.uv.xy + (0.2 * (lerpResult106 * Amp114));
                
                // Sample main texture
                float4 tex2DNode117 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, finalUV);
                
                // -- EMISSION COLOR --
                float3 finalEmission = lerp(temp_output_121_0, (temp_output_121_0 * tex2DNode117), _Useblack).rgb;
                
                // -- BASE ALPHA --
                float clampResult132 = clamp((input.color.a * tex2DNode117.r * _Opacity), 0.0, 1.0);
                
                // -- URP DEPTH FADE LOGIC --
                float4 ase_screenPosNorm = input.screenPos / (input.screenPos.w + 0.00000000001);
                
                // Fetch the screen depth from URP Depth buffer
                float rawDepth = SampleSceneDepth(ase_screenPosNorm.xy);
                float screenDepth137 = LinearEyeDepth(rawDepth, _ZBufferParams);
                
                // The current pixel's depth is stored in screenPos.w
                float distanceDepth137 = abs((screenDepth137 - input.screenPos.w) / max(_Depthpower, 0.0001));
                float clampResult136 = clamp(distanceDepth137, 0.0, 1.0);
                
                // Apply depth fade if toggled
                float finalAlpha = lerp(clampResult132, (clampResult132 * clampResult136), _Usedepth);

                // Output RGB (Emission) and A (Alpha)
                return float4(finalEmission, finalAlpha);
            }
            ENDHLSL
        }
    }
}