sampler2D _MainTex;
fixed4 _ColorTint;
fixed4 _RimColor;
fixed _EmissionPower;
fixed _RimPower;
float _R1;//zz

struct Input {
	float2 uv_MainTex;
	float3 viewDir;
};
void vert(inout appdata_full v)
{
	float3 normalDirection = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
	float3 ND = normalize(normalDirection + float3(1, 0.5, 0.5));
	float L=sin(v.color.b*3.1415 + _Time.y);
	v.vertex.x += _R1*ND*L*v.color.r;
}



half4 LightingCustomLambert (SurfaceOutput s, fixed3 lightDir, fixed atten){	  
	half pi = 3.14159265;		
	fixed diff = max (0, dot (s.Normal, lightDir));						
	half3 nrmDiff = ((diff*s.Albedo*_LightColor0.rgb)/pi)*(atten*2);
		
	half4 c;
	c.rgb = nrmDiff;
	c.a = s.Alpha;
	return c;		
}


void surf_default (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
	fixed rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
						
	o.Albedo = c.rgb * _ColorTint.rgb;
	o.Emission = _RimColor.rgb * pow (rim, _RimPower) + c.rgb* _EmissionPower;
	o.Alpha = c.a;
}

void surf_no_tint (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
	fixed rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
						
	o.Albedo = c.rgb;
	o.Emission = _RimColor.rgb * pow (rim, _RimPower) + c.rgb* _EmissionPower;
	o.Alpha = c.a;
}