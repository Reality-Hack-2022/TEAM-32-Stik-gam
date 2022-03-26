
/// Thanks to https://www.martinpalko.com/triplanar-mapping/
half4 Toon_SampleTriplanar(sampler2D tex, float3 positionWS, float3 normalWS, float textureScale, float triplanarSharpness)
{
    half2 yUV = positionWS.xz / textureScale;
    half2 xUV = positionWS.zy / textureScale;
    half2 zUV = positionWS.xy / textureScale;
    // Now do texture samples from our diffuse map with each of the 3 UV set's we've just made.
    half4 yDiff = tex2D (tex, yUV);
    half4 xDiff = tex2D (tex, xUV);
    half4 zDiff = tex2D (tex, zUV);
    // Get the absolute value of the world normal.
    // Put the blend weights to the power of BlendSharpness, the higher the value,
    // the sharper the transition between the planar maps will be.
    half3 blendWeights = pow (abs(normalWS), triplanarSharpness);
    // Divide our blend mask by the sum of it's components, this will make x+y+z=1
    blendWeights = blendWeights / (blendWeights.x + blendWeights.y + blendWeights.z);
    // Finally, blend together all three samples based on the blend mask.
    return xDiff * blendWeights.x + yDiff * blendWeights.y + zDiff * blendWeights.z;
}
