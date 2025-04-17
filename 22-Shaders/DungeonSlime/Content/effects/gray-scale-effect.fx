#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// This is a parameter we can set from our game
float Saturation;

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	// Sample the texture
	float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);

	// Calculate grayscale using luminance method
	float luminance = dot(color.rgb, float3(0.299, 0.587, 0.114));

	// Interpolate between grayscale and original color based on saturation
	color.rgb = lerp(float3(luminance, luminance, luminance), color.rgb, Saturation);

	// Multiply by vertex color (this allows SpriteBatch.Draw's color parameter to work)
	color *= input.Color;

	return color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
