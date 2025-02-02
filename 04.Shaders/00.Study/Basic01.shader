
Shader "Study/URPBasic01"
{
   Properties 
   {
     _TintColor("Test Color", color) = (1, 1, 1, 1)
	 _Intensity("Range Sample", Range(0, 1)) = 0.5
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

       	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
           
	    half4 _TintColor;
	    float _Intensity;
        sampler2D _MainTex;
        sampler2D _MainTex02;

        float4 _MainTex_ST;
        float4 _MainTex02_ST;

    struct VertexInput
    {
	    float4 vertex       : POSITION;
	    float2 uv           : TEXCOORD0;
    };

    struct VertexOutput
    {
        float4 vertex   	: SV_POSITION;
        float2 uv       	: TEXCOORD0;           	
    };

    VertexOutput vert(VertexInput v)
    {
        VertexOutput o;
        o.vertex = TransformObjectToHClip(v.vertex.xyz);            
        o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw * _Time.y;
        return o;
    }

    half4 frag(VertexOutput i) : SV_Target
    {
        float2 uv  = i.uv.xy  * _MainTex_ST.xy + _MainTex_ST.zw;	
        float2 uv2 = i.uv.xy  * _MainTex02_ST.xy + _MainTex02_ST.zw;	
        
        half4 col01 = tex2D(_MainTex, uv);
        half4 col02 = tex2D(_MainTex02, uv2); 
        half4 color = col01 + col02;

        return color;
    }

ENDHLSL
    	}
     }
}
