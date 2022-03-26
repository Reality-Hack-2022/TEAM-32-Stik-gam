Shader "Unlit/ToonURP"
{
    // Many credits go to https://github.com/FlowingCrescent/CelShadingWithFringeShadow_URP/blob/master/Assets/Shaders/CelShadingWithFringeShadow.shader
    // for help with shadow casting + clip space outline
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _SSS ("SSS (shadow colors) Texture", 2D) = "white" {}
        _ILM ("ILM (Specular, Ambient Occlusion, Highlights) Texture", 2D) = "white" {}

    	_MainColorMultiply ("Main Color Multiply", Color) = (1,1,1,1)
    	_SSSColorMultiply ("SSS (shadow) Color Multiply", Color) = (1,1,1,1)
    	_ILMColorMultiply ("ILM (specular/ambient occlusion) Color Multiply", Color) = (1,1,1,1)

    	_ShadowPosterizeSteps ("Shadow Posterize Steps", int) = 2
        _ShadowNormalMultiplier ("Shadow Normal Multiplier (based on direction)", float) = 1.0
    	_ShadowAttenuationMultiplier ("Shadow Attenuation Multiplier (cast onto us)", float) = 0.5

    	_SpecularColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 0.5)
        _SpecularPower ("Specular Power", float) = 15.0
        _SpecularThreshold ("Cell Specular Threshold", float) = 0.5

		_OutlineWidth ("Outline Width", float) = 0.0
		_OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)

        [Toggle] _GridTexture ("World Grid Texture", int) = 0
        _GridSize ("World Grid Size", float) = 1
    	[Toggle] _GridUseLocal ("World Grid Use Local Coordinates", int) = 0

    }
    SubShader
    {
        Tags{"RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque"}

		Pass {
            Name "Inverted Hull Outline"

            Cull Front
            //ZTest Always
            //ZWrite Off
            HLSLPROGRAM

            float _OutlineWidth;
            half4 _OutlineColor;

            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            inline float3 projectOnPlane( float3 vec, float3 normal )
			{
			    return vec - normal * ( dot( vec, normal ) / dot( normal, normal ) );
			}

            struct appdata
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
				float4 vertexColor	: COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            struct v2f
            {
                float4 vertexColor				: COLOR;
                float3 normalWS                 : NORMAL;
                float4 positionCS               : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };
            v2f Vert(appdata input)
            {
                v2f output = (v2f)0;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                // Convert to world
                float3 positionWS = mul(UNITY_MATRIX_M, float4(input.positionOS.xyz, 1));
				float3 normalWS = normalize(mul(input.normalOS, (float3x3)UNITY_MATRIX_I_M));

            	// Get camera info
                float3 cameraDelta = _WorldSpaceCameraPos - positionWS;
                float distanceToCamera = length(cameraDelta) / 1000.0;
                distanceToCamera = pow(distanceToCamera, 0.5);


                float3 normalCS = TransformWorldToHClipDir(normalWS);

                VertexPositionInputs positionInputs = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = positionInputs.positionCS + float4(normalCS.xy * 0.025 * _OutlineWidth * positionInputs.positionCS.w, 0, 0);//mul(UNITY_MATRIX_VP, float4(positionWS, 1));
                output.normalWS = normalWS;
                // Transfer to world
                return output;
            }
            half4 Frag (v2f input) : SV_Target
            {
            	if (_OutlineWidth <= 0)
            	{
            		discard;
            	}
                return _OutlineColor;
            }


            ENDHLSL
        }


        //this Pass copy from https://github.com/ColinLeung-NiloCat/UnityURPToonLitShaderExample
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            //we don't care about color, we just write to depth
            ColorMask 0

            HLSLPROGRAM

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            #pragma vertex ShadowCasterPassVertex
            #pragma fragment ShadowCasterPassFragment

            struct Attributes
            {
                float3 positionOS: POSITION;
                half3 normalOS: NORMAL;
                half4 tangentOS: TANGENT;
                float2 uv: TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv: TEXCOORD0;
                float4 positionWSAndFogFactor: TEXCOORD2; // xyz: positionWS, w: vertex fog factor
                half3 normalWS: TEXCOORD3;

                #ifdef _MAIN_LIGHT_SHADOWS
                    float4 shadowCoord: TEXCOORD6; // compute shadow coord per-vertex for the main light
                #endif
                float4 positionCS: SV_POSITION;
            };

            // Textures
            sampler2D _MainTex;
            float4 _MainTex_ST;

            Varyings ShadowCasterPassVertex(Attributes input)
            {
                Varyings output;

                // VertexPositionInputs contains position in multiple spaces (world, view, homogeneous clip space)
                // Our compiler will strip all unused references (say you don't use view space).
                // Therefore there is more flexibility at no additional cost with this struct.
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS);

                // Similar to VertexPositionInputs, VertexNormalInputs will contain normal, tangent and bitangent
                // in world space. If not used it will be stripped.
                VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

                // Computes fog factor per-vertex.
                float fogFactor = ComputeFogFactor(vertexInput.positionCS.z);

                // TRANSFORM_TEX is the same as the old shader library.
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);

                // packing posWS.xyz & fog into a vector4
                output.positionWSAndFogFactor = float4(vertexInput.positionWS, fogFactor);
                output.normalWS = vertexNormalInput.normalWS;

                #ifdef _MAIN_LIGHT_SHADOWS
                    // shadow coord for the light is computed in vertex.
                    // After URP 7.21, URP will always resolve shadows in light space, no more screen space resolve.
                    // In this case shadowCoord will be the vertex position in light space.
                    output.shadowCoord = GetShadowCoord(vertexInput);
                #endif

                // Here comes the flexibility of the input structs.
                // We just use the homogeneous clip position from the vertex input
                output.positionCS = vertexInput.positionCS;

                // ShadowCaster pass needs special process to clipPos, else shadow artifact will appear
                //--------------------------------------------------------------------------------------

                //see GetShadowPositionHClip() in URP/Shaders/ShadowCasterPass.hlsl
                float3 positionWS = vertexInput.positionWS;
                float3 normalWS = vertexNormalInput.normalWS;


                Light light = GetMainLight();
                float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, light.direction));

                #if UNITY_REVERSED_Z
                    positionCS.z = min(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE);
                #else
                    positionCS.z = max(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE);
                #endif
                output.positionCS = positionCS;

                //--------------------------------------------------------------------------------------

                return output;
            }

            half4 ShadowCasterPassFragment(Varyings input): SV_TARGET
            {
                return 0;
            }

            ENDHLSL
        }

        Pass
        {
            Name "ForwardLit"
            Tags {"LightMode"="UniversalForward"}



            HLSLPROGRAM
            #pragma only_renderers d3d11 playstation xboxone vulkan metal switch

            #pragma target 4.5

            #pragma multi_compile_fwdbase

            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            // #pragma multi_compile_fog

            // Receive shadows
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE

            #pragma multi_compile SHADOW_LOW SHADOW_MEDIUM SHADOW_HIGH

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/VolumeRendering.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/AreaLighting.hlsl"

            #include "./Lighting.hlsl"
            #include "./TriplanarHelper.hlsl"

            struct appdata
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float2 uv           : TEXCOORD0;
				float4 vertexColor	: COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertexColor				: COLOR;
                float4 positionCS               : SV_POSITION;
                float2 uv                       : TEXCOORD0;
                float3 positionWS               : TEXCOORD1;
            	float3 positionMS				: TEXCOORD2;
                float3 normalWS                 : TEXCOORD3;
                float3 normalMS                 : TEXCOORD4;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            // Textures
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _SSS;
            float4 _SSS_ST;
            sampler2D _ILM;
            float4 _ILM_ST;

            // Unifroms
            uniform float _ShadowNormalMultiplier;
            uniform float _ShadowAttenuationMultiplier;
            half4 _MainColorMultiply;
            half4 _SSSColorMultiply;
            half4 _ILMColorMultiply;
            half4 _SpecularColor;
            float _SpecularThreshold;
            float _SpecularPower;
            int _ShadowPosterizeSteps;
            int _GridTexture;
            float _GridSize;
            int _GridUseLocal;

			float posterize(float x, float steps) {
			    return clamp(floor(x / (1 / steps)) * (1 / (steps - 1)), 0, 1);
			}
            float posterizeMin(float x, float min, float steps)
			{
				float prog = (x - min) / min;
				return clamp(floor(( ( (steps-1)/steps) * prog + (1 / steps)) / (1 / steps)) * (1 / (steps - 1)), 0, 1);
			}

            // Simple vertex shader.
            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID (v);
	            UNITY_TRANSFER_INSTANCE_ID(v, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                // Standard veretex shader stuff.
                o.positionWS = mul(UNITY_MATRIX_M, float4(v.positionOS.xyz, 1));
                o.positionCS = mul(UNITY_MATRIX_VP, float4(o.positionWS, 1));
				o.positionMS = o.positionWS - mul(UNITY_MATRIX_M, float4(0,0,0, 1));
                o.normalWS = normalize(mul(v.normalOS, (float3x3)UNITY_MATRIX_I_M));
				o.normalMS = o.normalWS;//normalize(mul(v.normalOS, (float3x3)UNITY_MATRIX_IT_MV));
			    o.uv = v.uv;
                return o;
            }

            float4 doSample(sampler2D tex, v2f i)
			{
			    if (_GridTexture) {
			    	float3 pos = _GridUseLocal? i.positionMS : i.positionWS;
			    	float3 normal = _GridUseLocal? i.normalMS : i.normalWS;
			        return (float4)Toon_SampleTriplanar(tex, pos, normal, _GridSize, 1);
			    }
			    return tex2D(tex, i.uv);
			}

            float4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
	            UNITY_SETUP_INSTANCE_ID (i);

                // sample the texture
                float4 colBase = doSample(_MainTex, i) * _MainColorMultiply;
                float4 colSSS = doSample(_SSS, i) * _SSSColorMultiply;
                float4 colILM = doSample(_ILM, i) * _ILMColorMultiply;

            	// ILM Stuff
                float specular = colILM.r;
                float ambientOcclusion = 0.5;//colILM.g;

                // Get lighting data
                float normalShadow, attenuationShadow, specularIntensity;
                Toon_GetLightData(i.positionCS, i.positionWS, i.uv, i.normalWS, attenuationShadow, normalShadow, specularIntensity);

            	float shadowStrength = _ShadowNormalMultiplier * normalShadow + _ShadowAttenuationMultiplier * attenuationShadow;
            	shadowStrength = clamp(shadowStrength, 0, 1);

            	float shadowPosterized = posterizeMin(shadowStrength, ambientOcclusion, _ShadowPosterizeSteps);
            	float4 col = lerp(colBase, colSSS, shadowPosterized);

            	// Add Specular
                if (specular > 0 && specularIntensity > 0) {
                	specularIntensity = pow(specularIntensity, _SpecularPower);
                	specularIntensity = posterize(specularIntensity, _ShadowPosterizeSteps);//(specularIntensity > _SpecularThreshold)? 1 : 0;
                    specularIntensity *= specular;
                    col += specularIntensity * half4(_SpecularColor.rgb, 0) * _SpecularColor.a;
                }
                return col;
            }
            ENDHLSL
        }
    }
}
