
void MainLight_float(float3 WorldPos, out float3 Direction, out float3 Color, 
    out float DistanceAtten, out float ShadowAtten)
{
    #ifdef SHADERGRAPH_PREVIEW
    Direction = normalize(float3(0.5f, 0.5f, 0.25f));
    Color = float3(1.0f, 1.0f, 1.0f);
    DistanceAtten = 1.0f;
    ShadowAtten = 1.0f;
    
    #else
    float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
    Light mainLight = GetMainLight(shadowCoord);
    
    Direction = mainLight.direction;
    Color = mainLight.color;
    DistanceAtten = mainLight.distanceAttenuation;
    ShadowAtten = mainLight.shadowAttenuation;
    
    #endif    
}

void AdditionalLight_float(float3 WorldPos, int Index, out float3 Direction, out float3 Color,
    out float DistanceAtten, out float ShadowAtten)
{
    Direction = normalize(float3(0.5f, 0.5f, 0.25f));
    Color = float3(0.0f, 0.0f, 0.0f);
    DistanceAtten = 0.0f;
    ShadowAtten = 0.0f;
    
    #ifndef SHADERGRAPH_PREVIEW
    int pixelLightCount = GetAdditionalLightsCount();
    if (Index < pixelLightCount)
    {
        Light light = GetAdditionalLight(Index, WorldPos);
        
        Direction = light.direction;
        Color = light.color;
        DistanceAtten = light.distanceAttenuation;
        ShadowAtten = light.shadowAttenuation;
    }
    #endif
}