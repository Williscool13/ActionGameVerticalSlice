Shader "Custom/ExperimentShader"
{
	Properties
	{
		[Header(Surface options)] 
        [MainColor] _ColorTint("Tint", Color) = (1, 1, 1, 1)
		[MainTexture] _MainTex("Albedo (RGB)", 2D) = "white" {}

		_Smoothness("Smoothness", Range(0, 1)) = 0
	}

	SubShader
	{
		Tags{"RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque"}// "RenderType" = "Transparent" "Queue" = "Transparent"}

		Pass
		{
			Name "ForwardLit"
			Tags {"LightMode" = "UniversalForward"}

			//Blend SrcAlpha OneMinusSrcAlpha
			//ZWrite Off
			//Blend 1 0
			//ZWrite On

			HLSLPROGRAM
            #define _SPECULAR_COLOR
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile_fragment _ _SHADOWS_SOFT

			#pragma vertex vert
			#pragma fragment frag

			

			#include "ExperimentShaderForwardLitPass.hlsl"
			ENDHLSL

		}

		Pass 
		{
			name "ShadowCaster"
			Tags {"LightMode" = "ShadowCaster"}

			ColorMask 0

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "ExperimentShaderShadowCasterPass.hlsl"


			ENDHLSL
		}
	}
	
}