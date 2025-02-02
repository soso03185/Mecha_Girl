Shader "ImageEffect/ShadowThresholdCustomEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
       //  Cull Off -- ZWrite Off -- ZTest Always
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

     	    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct VertexInput
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct VertexOutput
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            VertexOutput vert (VertexInput v)
            {
                VertexOutput o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            half4 frag (VertexOutput i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                col.rgb = 1 - col.rgb;
                return col;
            }

	ENDHLSL  
        }
    }
}
