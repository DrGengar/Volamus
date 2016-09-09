#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif
float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 WorldInverseTranspose;

float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0.1;

float3 DiffuseLightDirection = float3(-60, -50, 15);
float4 DiffuseColor = float4(1, 1, 1, 1);
float DiffuseIntensity = 1.0;

float3 DiffuseLightDirection2 = float3(60, 50, 15);
float4 DiffuseColor2 = float4(1, 1, 1, 1);
float DiffuseIntensity2 = 1.0;

float Shininess = 200;
float4 SpecularColor = float4(1, 1, 1, 1);
float SpecularIntensity = 1;

float3 ViewVector = float3(1, 0, 0);

texture ModelTexture;
sampler2D textureSampler = sampler_state {
	Texture = (ModelTexture);
	MinFilter = Linear;
	MagFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Normal : NORMAL0;
	float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
	float3 Normal : TEXCOORD0;
	float2 TextureCoordinate : TEXCOORD1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
		float4 viewPosition = mul(worldPosition, View);
		output.Position = mul(viewPosition, Projection);

	float4 normal = normalize(mul(input.Normal, WorldInverseTranspose));
		float lightIntensity = dot(normal, DiffuseLightDirection);
	float lightIntensity2 = dot(normal, DiffuseLightDirection2);
	float4 color = saturate(DiffuseColor * DiffuseIntensity * lightIntensity);
		float color2 = saturate(DiffuseColor2 * DiffuseIntensity2 * lightIntensity2);

	output.Color = saturate(color + color2);

	output.Normal = normal;

	output.TextureCoordinate = input.TextureCoordinate;
	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR
{
	float3 light = normalize(DiffuseLightDirection);
	float3 normal = normalize(input.Normal);
	float3 r = normalize(2 * dot(light, normal) * normal - light);
	float3 v = normalize(mul(normalize(ViewVector), World));
	float dotProduct = dot(r, v);

	float4 specular = SpecularIntensity * SpecularColor * max(pow(dotProduct, Shininess), 0) * length(input.Color);
		specular.a = 1;

	float4 textureColor = tex2D(textureSampler, input.TextureCoordinate);
		textureColor.a = 1;

	float4 color1 = saturate(textureColor * input.Color + AmbientColor * AmbientIntensity + specular);

		light = normalize(DiffuseLightDirection2);
	normal = normalize(input.Normal);
	r = normalize(2 * dot(light, normal) * normal - light);
	v = normalize(mul(normalize(ViewVector), World));
	dotProduct = dot(r, v);

	specular = SpecularIntensity * SpecularColor * max(pow(dotProduct, Shininess), 0) * length(input.Color);

	float4 color2 = saturate(textureColor * input.Color + AmbientColor * AmbientIntensity + specular);

		float4 color = 0.5f * (color1 + color2);
		color.a = 1.0;
	return color;
}

technique Textured
{
	pass Pass1
	{
		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
}