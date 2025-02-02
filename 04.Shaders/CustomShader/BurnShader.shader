
Shader "Study/BurnShader_Shadow"
{
   Properties 
   {
	 _MainTex("Main Texture", 2D) = "white" {}
	 _MainTex02("Main Texture02", 2D) = "white" {}
   }  

   SubShader
   {  	
	    Tags
        {
	        "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"          
            "Queue"="Geometry"
        }
        Pass
    	{  		
     	    Name "Universal Forward"
            Tags {"LightMode" = "UniversalForward"}

HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma vertex vert
            #pragma fragment frag       	       	
       	            
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma multi_compile_fog

            // Recieve Shadow
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT

            CBUFFER_START(UnityPerMaterial)

            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            half4 _MainTex_ST;

    struct appdata
    {
        float4 vertex : POSITION;
        float2 uv : TEXCOORD0;
        float3 normal : NORMAL;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct v2f
    {
        float4 vertex : SV_POSITION;
        float2 uv : TEXCOORD0;
        float fogCoord : TEXCOORD1;
        float3 normal : NORMAL;
        float4 shadowCoord : TEXCOORD2;
        UNITY_VERTEX_INPUT_INSTANCE_ID
        UNITY_VERTEX_OUTPUT_STEREO
    };

    v2f vert (appdata v)
    {
       v2f o;
 //    UNITY_SETUP_INSTANCE_ID(v);
 //    UNITY_TRANSFER_INTANCE_ID(v,o);
       UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

       o.vertex = TransformObjectToHClip(v.vertex.xyz);
       o.uv = TRANSFORM_TEX(v.uv, _MainTex);
       o.normal = TransformObjectToWorldNormal(v.normal);
       o.fogCoord = ComputeFogFactor(o.vertex.z);

       // #ifdef _MAIN_LIGHT_SHADOWS
       VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
       o.shadowCoord = GetShadowCoord(vertexInput);
       // #endif

       return o;
    }

    half4 frag(v2f i) : SV_Target
    {
//     UNITY_SETUP_INSTANCE_ID(v);
       UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

       Light mainLight = GetMainLight(i.shadowCoord);

       float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

       float NdotL = saturate(dot(_MainLightPosition.xyz, i.normal));
       half3 ambient = SampleSH(i.normal);

       col.rgb *=  NdotL * _MainLightColor.rgb * mainLight.shadowAttenuation * mainLight.distanceAttenuation + ambient;
       col.rgb  = MixFog(col.rgb, i.fogCoord);

       return col;
    }

    ENDHLSL
    }

           Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }
            // -------------------------------------
            // Render State Commands
            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            #pragma target 2.0

            // -------------------------------------
            // Shader Stages
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"

            // -------------------------------------
            // Universal Pipeline keywords

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE

            // This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW

            // -------------------------------------
            // Includes
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }
     }
}
