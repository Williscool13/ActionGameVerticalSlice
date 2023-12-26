#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"


struct app
{
    float3 positionOS : POSITION;
    float3 normalOS : NORMAL;
};
    
struct v2f
{
    float4 positionCS : SV_POSITION;
    //float3 positionWS : TEXCOORD1;
    //float3 normalWS : TEXCOORD2;
};

float3 _LightDirection;

float4 GetShadowCasterPositionCS(float3 positionWS, float3 normalWS)
{
    float3 lightDirectionWS = _LightDirection;
    float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));

#if UNITY_REVERSED_Z
    positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
#else
    positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
#endif
    return positionCS;
}


v2f vert(app v)
{
    v2f o;
    o.positionCS = TransformObjectToHClip(v.positionOS); // https://xibanya.github.io/URPShaderViewer/Library/Core/SpaceTransforms.html#TransformObjectToHClip

    // removes "Shadow Acne"
    o.positionCS = GetShadowCasterPositionCS(TransformObjectToWorld(v.positionOS), TransformObjectToWorldNormal(v.normalOS));
    
    return o; 
}

float4 frag(v2f i) : SV_Target
{
    return 0;
}
