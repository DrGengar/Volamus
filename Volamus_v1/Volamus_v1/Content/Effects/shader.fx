#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#endif

#define MAX_POINT_LIGHTS 4

//-----------------------------------------------------------------------------
// Globals.
//-----------------------------------------------------------------------------

float4x4 worldMatrix;
float4x4 worldInverseTransposeMatrix;
float4x4 worldViewProjectionMatrix;

float3 cameraPos;
float4 globalAmbient;
int numLights;

float3 PointLightpos[MAX_POINT_LIGHTS];
float4 PointLightambient[MAX_POINT_LIGHTS];
float4 PointLightdiffuse[MAX_POINT_LIGHTS];
float4 PointLightspecular[MAX_POINT_LIGHTS];
float PointLightradius[MAX_POINT_LIGHTS];

float4 Materialambient;
float4 Materialdiffuse;
float4 Materialemissive;
float4 Materialspecular;
float Materialshininess;

//-----------------------------------------------------------------------------
// Textures.
//-----------------------------------------------------------------------------

texture colorMapTexture;

sampler2D colorMap = sampler_state
{
	Texture = <colorMapTexture>;
	MagFilter = Linear;
	MinFilter = Anisotropic;
	MipFilter = Linear;
	MaxAnisotropy = 16;
};

//-----------------------------------------------------------------------------
// Vertex Shaders.
//-----------------------------------------------------------------------------

struct VS_INPUT
{
	float3 position : POSITION;
	float2 texCoord : TEXCOORD0;
	float3 normal : NORMAL;
};

struct VS_OUTPUT
{
	float4 position : POSITION;
	float3 worldPos : TEXCOORD0;
	float2 texCoord : TEXCOORD1;
	float3 viewDir : TEXCOORD2;
	float3 normal : TEXCOORD3;
};

VS_OUTPUT VS_PointLighting(VS_INPUT IN)
{
	VS_OUTPUT OUT;

	OUT.position = mul(float4(IN.position, 1.0f), worldViewProjectionMatrix);
	OUT.worldPos = mul(float4(IN.position, 1.0f), worldMatrix).xyz;
	OUT.texCoord = IN.texCoord;
	OUT.viewDir = cameraPos - OUT.worldPos;
	OUT.normal = mul(IN.normal, (float3x3)worldInverseTransposeMatrix);

	return OUT;
}

//-----------------------------------------------------------------------------
// Pixel Shaders.
//-----------------------------------------------------------------------------

float4 PS_PointLighting(VS_OUTPUT IN) : COLOR
{
	float4 color = float4(0.0f, 0.0f, 0.0f, 0.0f);

	float3 n = normalize(IN.normal);
	float3 v = normalize(IN.viewDir);
	float3 l = float3(0.0f, 0.0f, 0.0f);
	float3 h = float3(0.0f, 0.0f, 0.0f);

	float atten = 0.0f;
	float nDotL = 0.0f;
	float nDotH = 0.0f;
	float power = 0.0f;

	for (int i = 0; i < numLights; ++i)
	{
		l = (PointLightpos[i] - IN.worldPos) / PointLightradius[i];
		atten = saturate(1.0f - dot(l, l));

		l = normalize(l);
		h = normalize(l + v);

		nDotL = saturate(dot(n, l));
		nDotH = saturate(dot(n, h));
		power = (nDotL == 0.0f) ? 0.0f : pow(nDotH, Materialshininess);

		color += (Materialambient * (globalAmbient + (atten * PointLightambient[i]))) +
			(Materialdiffuse * PointLightdiffuse[i] * nDotL * atten) +
			(Materialspecular * PointLightspecular[i] * power * atten);
	}

	return color * tex2D(colorMap, IN.texCoord);
}

//-----------------------------------------------------------------------------
// Techniques.
//-----------------------------------------------------------------------------

technique PerPixelPointLighting
{
	pass
	{
		VertexShader = compile VS_SHADERMODEL VS_PointLighting();
		PixelShader = compile PS_SHADERMODEL PS_PointLighting();
	}
}