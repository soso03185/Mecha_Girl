Shader "Custom/OutLineShader"
{
   Properties 
   {  
     _OutlineColor("Test Color", color) = (1, 1, 1, 1)
	 _OutlineWidth("Range Sample", Range(0, 0.1)) = 0.04
	 _MainTex("Main Texture", 2D) = "white" {}
   }  
           
   SubShader
   {  	
	    Tags
        {
	        "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"          
            "Queue"="Geometry"
        }

        // Outline Pass
        Pass
    	{  		
            Name "Outline"
            Tags {"LightMode" = "UniversalForward"}
            
            Cull Front
            ZWrite Off
            
HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag       	       	
         
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            float _OutlineWidth;
            float4 _OutlineColor;

            v2f vert (appdata v)
            { 
                v2f o;
                float3 normal = normalize(v.normal);
                o.vertex = TransformObjectToHClip(v.vertex + normal * _OutlineWidth);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            } 
        ENDHLSL
        }
        
        // Main Pass
        Pass
        {
            Name "Universal Forward"
            Tags {"LightMode" = "UniversalForward"}

            Cull Back
            ZWrite Off
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;

            v2f vert (appdata v)
            { 
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.texcoord);
            }

            ENDHLSL
        }

        // shadow caster
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

    FallBack "Diffuse"
}
