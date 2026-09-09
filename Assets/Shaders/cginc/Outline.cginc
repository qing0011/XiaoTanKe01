#ifndef OUTLINE_SHADERS_CGINC
#define OUTLINE_SHADERS_CGINC
#include "UnityCG.cginc"
void BackFacing(inout appdata_full v, sampler2D _BaseColorRGBOutlineWidthA, float _OutlineWidth, float _OutlineEnabled)
{	
	float2 uv = v.texcoord;
	float4 tex2DNode76 = tex2Dlod( _BaseColorRGBOutlineWidthA, float4( uv, 0, 0.0) );
	float outlineVar = ( _OutlineWidth * _OutlineEnabled * tex2DNode76.a );
	v.vertex.xyz += ( v.normal * outlineVar );
}
void BackFacingTangent(inout appdata_full v, sampler2D _BaseColorRGBOutlineWidthA, float _OutlineWidth, float _OutlineEnabled)
{
	float2 uv = v.texcoord;
	float4 tex2DNode76 = tex2Dlod(_BaseColorRGBOutlineWidthA, float4(uv, 0, 0.0));
	float outlineVar = (_OutlineWidth * _OutlineEnabled * tex2DNode76.a);
	v.vertex.xyz += (v.tangent * outlineVar);
}


void BackFacingNDC(inout appdata_full v, sampler2D BaseColorRGBOutlineWidthA, float outlineWidth, float enabled)
{	
	float2 uv = v.texcoord;
	float4 pos = UnityObjectToClipPos(v.vertex);
	float3 viewNormal = mul((float3x3)UNITY_MATRIX_IT_MV, v.tangent.xyz);
	float3 ndcNormal = normalize(TransformViewToProjection(viewNormal.xyz))* pos.w;
	float4 nearUpperRight = mul(unity_CameraInvProjection, float4(1, 1, UNITY_NEAR_CLIP_VALUE, _ProjectionParams.y));
	float aspect = abs(nearUpperRight.y / nearUpperRight.x);
	ndcNormal.x *= aspect;
	
	float4 col = tex2Dlod(BaseColorRGBOutlineWidthA, float4(uv, 0, 0.0));
	float outlineVar = (outlineWidth * enabled * col.a);
	pos.xy += 0.01 * (ndcNormal.xy * outlineVar);
	v.vertex.xyz = pos.xyz;// (v.normal * outlineVar);
}
#endif