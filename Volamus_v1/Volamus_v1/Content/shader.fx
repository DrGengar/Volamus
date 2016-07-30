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

float4 AmbientColor = float4(0, 0, 0, 1);  //white
float AmbientIntensity = 1.0f;
float3 DiffuseLightDirection = float3(0, 1, 1);  
float4 DiffuseColor = float4(1, 0, 0, 1);
float DiffuseIntensity = 1.0f;
float Shininess = 200;
float4 SpecularColor = float4(0, 1, 1, 1);
float SpecularIntensity = 1.0f;
float3 ViewVector = float3(1, 0, 0);

//Input des Vertex Shaders
struct VertexShaderInput {
	float4 Position : POSITION0;
	float4 Normal : NORMAL0;
};

// Output des Vertex Shaders = Input des Pixel Shaders
struct VertexShaderOutput {
	float4 Position : POSITION0;
	float4 Color : COLOR0 ;
	float3 Normal : TEXCOORD0; 
};

//Vertices werden transformiert
VertexShaderOutput VertexShaderFunction(VertexShaderInput input) {

	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	float4 normal = mul(input.Normal, WorldInverseTranspose);
	float lightIntensity = dot(normal, DiffuseLightDirection);
	output.Color = saturate(DiffuseColor * DiffuseIntensity * lightIntensity);

	output.Normal = normal;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0{
	float3 light = normalize(DiffuseLightDirection);
	float3 normal = normalize(input.Normal);
	float3 r = normalize(2 * dot(light, normal) * normal - light);
	float3 v = normalize(mul(normalize(ViewVector), World));

	float dotProduct = dot(r, v);
	float4 specular = SpecularIntensity * SpecularColor * max(pow(dotProduct, Shininess), 1.0f) * length(input.Color);

	return saturate(input.Color + AmbientColor * AmbientIntensity + specular);
}


// dass es von unserem Spiel genutzt werden kann
technique Ambient {
	pass Pass1 {
		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
}