Shader "Custom/DimFocusShader"
{
    Properties
    {
        _Color("Color", Color) = (0,0,0,0.6)
      //  _Center("Focus Center", Vector) = (0.5, 0.5, 0, 0) // ��Ŀ�� �߽�
        _Radius("Focus Radius", Float) = 0.2 // ��Ŀ�� �ݰ�
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
                // UV ��ǥ�� ��ũ�� ��ǥ�� ��ȯ
                float2 uv = i.uv * _ScreenSize;

                // ��Ŀ�� �߽��� ��ũ�� ��ǥ�� ��ȯ
                float2 focusCenter = (0.5, 0.5) * _ScreenSize;

                // ��Ŀ�� �߽ɰ� �ؽ�ó ��ǥ�� �Ÿ� ���
                float dist = distance(uv, focusCenter);

                // �ݰ��� �ȼ� ������ �����Ͽ� �ݰ� ���� �����ϰ� ����
                clip(dist - _Radius);

                return _Color; // ������ ������ ��ó�� ����
            }
            ENDCG
        }
    }
}
