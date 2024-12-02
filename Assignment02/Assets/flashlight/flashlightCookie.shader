Shader "Custom/flashlightCookie"
{
    Properties
    {
        _CookieColor ("Cookie Color", Color) = (1, 1, 1, 1) // Light color
        _CookieRange ("Cookie Range", Float) = 5.0         // Distance the cookie affects
        _SoftEdge ("Soft Edge", Float) = 1.0               // Softness of cookie edge
        _SpotStrength ("Spot Strength", Float) = 1.0       // Intensity of the spotlight
    }
    SubShader
    {
        Tags { "LightMode"="ForwardBase" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _CookieColor;
            float _CookieRange;
            float _SoftEdge;
            float _SpotStrength;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // Center UV coordinates to simulate radial falloff
                float2 uv = i.uv - 0.5;
                float distance = length(uv); // Radial distance from the center

                // Compute cookie intensity with soft edge falloff
                float cookie = saturate(1.0 - distance / _CookieRange);
                cookie = pow(cookie, _SoftEdge); // Control softness of the edges

                // Combine cookie intensity with spot strength and color
                float intensity = _SpotStrength * cookie;
                return float4(_CookieColor.rgb * intensity, intensity);
            }
            ENDCG
        }
    }
    FallBack "Unlit/Transparent"
}
