
Shader "Study/URPBasic02"
{
   Properties 
   {
       _AmbientColor("Ambient Color" , color) = (0,0,0,0)
       _Lightwidth("Light Width", Range(0, 255)) = 1
       _LightStep("Light Step", int) = 1
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
      
    struct VertexInput
    {
	    float4 vertex  : POSITION;      
        float3 normal : NORMAL;
    };

    struct VertexOutput
    {
        float4 vertex : SV_POSITION;
        float3 normal : NORMAL;
    };


    half4 _AmbientColor;
    float _Lightwidth, _LightStep;

    VertexOutput vert(VertexInput v)
    {
        VertexOutput o;
        
        o.vertex = TransformObjectToHClip(v.vertex.xyz);
        o.normal = TransformObjectToWorldNormal(v.normal);

        return o;
    }

    half4 frag(VertexOutput i) : SV_Target
    {     
        float4 color = half4(1, 1, 1, 1);
        
        // _MainLightPosition 은 라이트 벡터를 읽어옵니다.
        float3 light = _MainLightPosition.xyz;
        float3 lightcolor = _MainLightColor.rgb;
        float NdotL = saturate(dot(i.normal, light));

        float3 toonlight = ceil(NdotL * _Lightwidth) / _LightStep * lightcolor;
        float3 combine = NdotL > 0 ? toonlight : _AmbientColor;

        color.rgb *= combine;

        return color;
    }

ENDHLSL
    	}
     }
}
