
#ifndef H_TOON_LIGHTING
#define H_TOON_LIGHTING


/*
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariablesGlobal.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightDefinition.cs.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Shadow/HDShadowContext.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/HDShadow.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Builtin/BuiltinData.hlsl"
*/
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

Light Toon_GetLight(float3 positionWS)
{
	float4 shadowCoord = TransformWorldToShadowCoord(positionWS);
	return GetMainLight(shadowCoord);
}

float Toon_GetShadowStrength(float3 positionWS)
{
	Light mainLight = GetMainLight(TransformWorldToShadowCoord(positionWS));
	return mainLight.shadowAttenuation * mainLight.distanceAttenuation;
	//return MainLightRealtimeShadow(TransformWorldToShadowCoord(positionWS));
}

float Toon_GetSpecularStrength(float3 lightDirection, float3 eyePos, float3 positionWS, float3 normalWS)
{
	float3 vertexToEye = normalize(eyePos - positionWS);
	float diff = dot(normalWS, lightDirection);
	// Ensure specular happens on one side only
	if (diff > 0)
	{
		float3 reflect = normalize((2 * diff * normalWS) - lightDirection);

		float specularStrength = dot(reflect, vertexToEye);
		// specularStrength = clamp(specularStrength, 0, 1);
		return specularStrength;
	}
	return 0;
}

void Toon_GetLightInfo(Light light, float2 positionSS, float3 positionWS, float3 normalWS, out float3 lightDir, out float attenuation, out float specularPower)
{
	lightDir = light.direction;
	attenuation = Toon_GetShadowStrength(positionWS);
	specularPower = Toon_GetSpecularStrength(lightDir, _WorldSpaceCameraPos, positionWS, normalWS);
}


void Toon_GetLightData(float3 positionCS, float3 positionWS, float2 uv, float3 normalWS, out float attenuation, out float normalShadow, out float specularPower)
{
	Light light = Toon_GetLight(positionWS);

	float3 lightDirection;
	Toon_GetLightInfo(light, positionCS.xy, positionWS, normalWS, lightDirection, attenuation, specularPower);
	lightDirection = normalize(lightDirection);
	normalShadow = 0.5 - 0.5 * clamp(dot(lightDirection, normalWS), -1, 1);

	attenuation = 1 - attenuation;
	//normalShadow = 1 - normalShadow;
}


#endif
