#if OPENGL
    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
#else
    #define VS_SHADERMODEL vs_4_0_level_9_1
    #define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// Pixelation parameters
float PixelSize;
float ScreenWidth;
float ScreenHeight;
float TransitionProgress; // 0.0 = fully pixelated, 1.0 = not pixelated
float2 CircleCenter; // Center of the unpixelation circle in UV coordinates (0-1)

// The texture to be pixelated
sampler2D InputTexture : register(s0);

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float2 uv = input.TextureCoordinates;

    // Calculate distance from the center point (in UV space)
    float2 centerUV = CircleCenter;
    float distFromCenter = distance(uv, centerUV);

    // Normalize the distance (0 to 1) based on the furthest possible distance
    // The furthest possible distance in UV space is from a corner to the center
    float maxDist = distance(float2(0, 0), float2(0.5, 0.5)) * 1.2; // Add some margin to avoid sharp cutoff
    float normalizedDist = distFromCenter / maxDist;

    // Smooth transition function instead of a linear one
    // Using smoothstep for a more natural transition curve
    float circleProgress = TransitionProgress * 1.5; // Extend the transition range
    float localTransition = smoothstep(0.0, 1.0, saturate(circleProgress - normalizedDist));

    // Smooth the interpolation between pixel sizes
    float minPixelSize = 1.0; // No pixelation
    float maxPixelSize = PixelSize; // Maximum pixelation
    float effectivePixelSize = lerp(maxPixelSize, minPixelSize, localTransition);

    // If effectivePixelSize is very close to 1.0, just use the original UVs
    float2 finalUV;

    if (effectivePixelSize <= 1.01)
    {
        finalUV = uv;
    }
    else
    {
        // Apply pixelation by snapping UV coordinates to a grid
        float2 pixelGrid;
        pixelGrid.x = ScreenWidth / effectivePixelSize;
        pixelGrid.y = ScreenHeight / effectivePixelSize;
        finalUV = floor(uv * pixelGrid) / pixelGrid + (0.5 / pixelGrid);
    }

    // Sample the texture with the pixelated/unpixelated UVs
    float4 color = tex2D(InputTexture, finalUV);

    return color * input.Color;
}

technique Pixelate
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
}
