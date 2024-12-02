Shader "Custom/flashlightBeam"
{
    Properties
    {
        _BeamColor ("Beam Color", Color) = (1, 1, 1, 1)
        _BeamRange ("Beam Range", Float) = 5.0
        _BeamFalloff ("Beam Falloff", Float) = 2.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
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

            float4 _BeamColor;
            float _BeamRange;
            float _BeamFalloff;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv - 0.5; // Centering UV coordinates
                float distance = length(uv); // Calculate distance from center
                float intensity = saturate(1.0 - (distance * _BeamFalloff / _BeamRange)); // Simple radial falloff
                return float4(_BeamColor.rgb * intensity, intensity); // Combine color and transparency
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
}
