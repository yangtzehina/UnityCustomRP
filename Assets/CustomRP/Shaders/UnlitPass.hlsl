#ifndef CUSTOM_UNLIT_PASS_INCLUDED
#define CUSTOM_UNLIT_PASS_INCLUDED

#include "../ShaderLibrary/Common.hlsl"

//CBUFFER_START(UnityPerMaterial)
//    float4 _BaseColor;
//CBUFFER_END
UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
    UNITY_DEFINE_INSTANCED_PROP(float4,_BaseColor)
UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)


struct Attributes{
    float3 positionOS : POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};


float4 UnlitPassVertex (Attributes input) : SV_POSITION {
    UNITY_SETUP_INSTANCE_ID(input);
    float3 positionWS = TransformObjectToWorld(input.positionOS);
    return TransformWorldToHClip(positionWS);
}

float4 UnlitPassFragment (): SV_TARGET {
	return _BaseColor;
}
#endif

