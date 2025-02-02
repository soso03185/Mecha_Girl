
Shader "Study/RimLight"
{
     Properties 
     {
        [HDR]_RimColor("Rim Color", color) = (1,1,1,1)
        _RimPower("Rim Power", Range(0.01, 0.1)) = 0.1
        _RimInten("Rim Intensity", Range(0.01, 100)) = 1
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
	            float3 normal : NORMAL;
           		float2 uv 	  : TEXCOORD1;
            };

	        struct VertexOutput
            {
                float4 vertex  	: SV_POSITION;
	            float3 normal   : NORMAL;			
	            float3 WorldSpaceViewDirection : TEXCOORD0;
           		float2 uv 	  : TEXCOORD1;
      	    };

	        float _RimPower, _RimInten;
	        half4 _RimColor;	

            Texture2D _MainTex; //, _Flowmap;
		    float4 _MainTex_ST;
		    SamplerState sampler_MainTex;	

	        VertexOutput vert(VertexInput v)
        	{
	            VertexOutput o;      
          	    o.vertex = TransformObjectToHClip(v.vertex.xyz); 				         	
	            o.normal = TransformObjectToWorldNormal(v.normal);
	       
                // ������ ��ü ���� ���� ��ġ���� ī�޶� �������� ���� ���� ������ ����ϰ� ����ȭ �� ������� ī�޶� ��ǥ - ����������ؽ� ��ǥ
	            o.WorldSpaceViewDirection = normalize(_WorldSpaceCameraPos.xyz - TransformObjectToWorld(v.vertex.xyz));
                // o.WorldSpaceViewDirection = normalize(_WorldSpaceCameraPos.xyz - mul(GetObjectToWorldMatrix(), float4(v.vertex.xyz, 1.0)).xyz);
           
                o.uv = v.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw;
         	    return o;
        	}					

	        half4 frag(VertexOutput i) : SV_Target
        	{	  	
	            float3 light = _MainLightPosition.xyz;
          	    float4 color = float4(0.5, 0.5, 0.5, 1);			
			
	            half3 ambient = SampleSH(i.normal);
                //���� ī�޶� ���Ϳ� ����� ������ ���⿡ ���� ���� ���մϴ�. �ٶ󺸴� ������ ���� 1(����) 90���� 0(��ο�)�� �˴ϴ�.	      
	            half face = saturate(dot(i.WorldSpaceViewDirection, i.normal));
                
	            half3 rim = 1 - (pow(face, _RimPower));
                
                //emissive term	
	            color.rgb *= saturate(dot(i.normal, light)) * _MainLightColor.rgb + ambient ;	
	
                //emissive term	
	            color.rgb += rim * _RimInten * _RimColor;

                float4 TexColor = _MainTex.Sample(sampler_MainTex, i.uv);
	            TexColor.rgb *= color;
                TexColor.rgb *= 2;
	            return TexColor;
        	}

	ENDHLSL  
      }
   }
}
