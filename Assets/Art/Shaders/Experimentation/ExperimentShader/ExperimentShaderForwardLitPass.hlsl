//#include "UnityCG.cginc"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

// these 2 are equivalent
TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
float4 _MainTex_ST;
float4 _ColorTint;
float _Smoothness;

struct app
{
    float3 positionOS : POSITION;
    float2 uv : TEXCOORD0;
    float3 normalOS : NORMAL;
    float4 tangentOS : TANGENT;
    float4 color : COLOR;
};

struct v2f
{
    float4 positionCS : SV_POSITION;
    float2 uv : TEXCOORD0;
    float3 positionWS : TEXCOORD1;
    float3 normalWS : TEXCOORD2;
};


// these 3 are alternative ways of transforming to different spaces - note that normals need to be transformed differently
float4 ObjectToClipPos(float3 pos) { return mul(UNITY_MATRIX_VP, mul(UNITY_MATRIX_M, float4(pos, 1))); }
float3 ObjectToWorldPos(float3 posOS) { return mul(unity_ObjectToWorld, float4(posOS, 1)).xyz; }
float3 ObjectToWorldNormal(float3 normalOS, bool normalized) { 
    if (normalized)
    {
        return normalize(mul(unity_ObjectToWorld, float4(normalOS, 1)).xyz);
    }
    return mul(unity_ObjectToWorld, float4(normalOS, 1)).xyz;
}


v2f vert(app v)
{
    v2f o;
    o.positionCS = TransformObjectToHClip(v.positionOS); // Core.hlsl (Space Transforms)
    o.positionWS = TransformObjectToWorld(v.positionOS); // Core.hlsl (Space Transforms)
    o.normalWS = TransformObjectToWorldNormal(v.normalOS, false); // Core.hlsl (Space Transforms)
    
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    return o; 
}

float4 frag(v2f i) : SV_Target
{
    float4 _t = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
    
    InputData lightingInput = (InputData)0;
    lightingInput.positionWS = i.positionWS;
    lightingInput.normalWS = normalize(i.normalWS);
    lightingInput.viewDirectionWS = GetWorldSpaceNormalizeViewDir(i.positionWS); // ShaderVariablesFunctions.hlsl
    lightingInput.shadowCoord = TransformWorldToShadowCoord(i.positionWS);
    
    SurfaceData surfaceData = (SurfaceData)0;
    surfaceData.albedo = _t.rgb * _ColorTint.rgb;
    surfaceData.alpha = _t.a * _ColorTint.a;
    surfaceData.specular = 1;
    surfaceData.smoothness = _Smoothness;
    
    return UniversalFragmentPBR(lightingInput, surfaceData);
}
