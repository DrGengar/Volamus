float4x4 World;
float4x4 View;
float4x4 Projection;

float CurrentLayer; //value between 0 and 1
float MaxHairLength; //maximum hair length

float3 Displacement;



texture FurTexture;
sampler FurSampler = sampler_state
{
    Texture = (FurTexture);
    MinFilter = Point;
    MagFilter = Point;
    MipFilter = Point;
    AddressU = Wrap;
    AddressV = Wrap;
};
texture Texture;
sampler FurColorSampler = sampler_state
{
    Texture = (Texture);
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};


struct VertexShaderInput
{
    float3 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
};

VertexShaderOutput FurVertexShader(VertexShaderInput input)
{
    VertexShaderOutput output;
    float3 pos;
    pos = input.Position + input.Normal * MaxHairLength * CurrentLayer;

    float4 worldPosition = mul(float4(pos,1), World);
    
    //make the displacement non linear, to make it look more like fur
    float displacementFactor = pow(CurrentLayer, 3);
    //apply the displacement
    worldPosition.xyz +=Displacement*displacementFactor ;
    
    
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    output.TexCoord = input.TexCoord;
    return output;
}

float4 FurPixelShader(VertexShaderOutput input) : COLOR0
{
    float4 furData = tex2D(FurSampler, input.TexCoord);
    float4 furColor = tex2D(FurColorSampler, input.TexCoord);
    
    //based on layer depth, choose the amount of shading.
    //we lerp between two values to avoid having the base of the fur pure black.
    float shadow = lerp(0.4,1,CurrentLayer);
    furColor *= shadow;
    
    float furVisibility =(CurrentLayer > furData.r) ? 0 : furData.a;
	furColor.a = (CurrentLayer == 0) ? 1 : furVisibility;
    return furColor;
}

technique Fur
{
    pass Pass1
    {
        AlphaBlendEnable = true;
        SrcBlend = SRCALPHA;
        DestBlend = INVSRCALPHA;
        CullMode = None;

        VertexShader = compile vs_4_0 FurVertexShader();
        PixelShader = compile ps_4_0 FurPixelShader();
    }
}