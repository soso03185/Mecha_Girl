Shader "Custom/DimFocusShader"
{
    Properties
    {
        _Color("Color", Color) = (0,0,0,0.6)
      //  _Center("Focus Center", Vector) = (0.5, 0.5, 0, 0) // 포커스 중심
        _Radius("Focus Radius", Float) = 0.2 // 포커스 반경
        _ScreenSize("Screen Size", Vector) = (1080, 1920, 0, 0) // Screen Size
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" }
        Pass
        {
            ZTest Always
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _Color;
          //  float2 _Center;
            float _Radius;
            float2 _ScreenSize;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

           fixed4 frag(v2f i) : SV_Target
            {
                // UV 좌표를 스크린 좌표로 변환
                float2 uv = i.uv * _ScreenSize;

                // 포커스 중심을 스크린 좌표로 변환
                float2 focusCenter = (0.5, 0.5) * _ScreenSize;

                // 포커스 중심과 텍스처 좌표의 거리 계산
                float dist = distance(uv, focusCenter);

                // 반경을 픽셀 단위로 설정하여 반경 내는 투명하게 설정
                clip(dist - _Radius);

                return _Color; // 나머지 영역은 딤처리 색상
            }
            ENDCG
        }
    }
}
