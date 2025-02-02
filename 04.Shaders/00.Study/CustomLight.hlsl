// Ŀ���Ҷ���Ʈ �Լ�
void CustomLight_File_float(out float3 Direction, out float3 Color)
{   
#ifdef SHADERGRAPH_PREVIEW
    Direction = float3(1,1,1);
    Color = float3(1,1,1);
#else
    Light light = GetMainLight();
    Direction = light.direction;
    Color = light.color;
#endif
} 
// Ŀ���� ����Ʈ ������ �Լ�
void CustimLight_Shadow_float(float3 worldPos, out float ShadowAtten)
{
#ifdef SHADERGRAPH_PREVIEW
    ShadowAtten = 1.0f;
#else
    // shadow Coord �����
#if defined(_MAIN_LIGHT_SHADOWS_SCREEN) && !defined(_SURFACE_TYPE_TRANSPARENT)
    half4 clipPos = TransformWorldToHClip(worldPos);
    half4 shadowCoord = ComputeScreenPos(clipPos);
#else
    half4 shadowCoord = TransformWorldToShadowCoord(worldPos);
#endif

    Light light = GetMainLight();
    // ���ζ���Ʈ�� ���ų� �޴� ������ ���� �ɼ��� �Ǿ� ���� ���� �׸��ڸ� ���ش�.
#if !defined(_MAIN_LIGHT_SHADOWS) || defined(_RECEIVE_SHADOWS_OFF)
    ShadowAtten = 1.0f;
#endif
    
    //ShadowAtten �޾ƿͼ� �����
#if SHADOWS_SCREEN
    ShadowAtten = SampleScreenSpaceShadowmap(shadowCoord);
#else
    ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
    half shadowStrength = GetMainLightShadowStrength();
    ShadowAtten = SampleShadowmap(shadowCoord, TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture),
    shadowSamplingData, shadowStrength, false);
#endif
    
#endif
}