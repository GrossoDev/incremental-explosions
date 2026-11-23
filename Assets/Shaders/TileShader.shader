Shader "Custom/TileShader"
{
    Properties
    {
        _DarkColor ("Dark Color", Color) = (1,1,1,1)
        _LightColor ("Light Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _OffsetX ("Offset X", Range(0,1)) = 0.0
        _OffsetY ("Offset Y", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        float _OffsetX;
        float _OffsetY;
        fixed4 _DarkColor;
        fixed4 _LightColor;

        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 tile = floor(IN.worldPos.xz + float2(_OffsetX, _OffsetY));
            float checker = frac((tile.x + tile.y) * 0.5);
            fixed4 tileColor = checker > 0.25 ? _LightColor : _DarkColor;
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * tileColor;
            o.Albedo = c.rgb;

            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
