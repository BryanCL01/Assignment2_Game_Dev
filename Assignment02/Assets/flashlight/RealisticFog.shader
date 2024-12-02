Shader "Custom/RealisticFog"
{
    Properties
    {
        _FogColor ("Fog Color", Color) = (0.7, 0.8, 1.0, 1.0) // Fog color
        _FogDensity ("Fog Density", Float) = 0.02             // Density of the fog
        _HeightFalloff ("Height Falloff", Float) = 0.5        // Attenuation based on height
        _FogStartHeight ("Fog Start Height", Float) = 0.0     // Height where fog starts
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest LEqual
            Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float4 _FogColor;
            float _FogDensity;
            float _HeightFalloff;
            float _FogStartHeight;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // Calculate distance from the camera to the fragment
                float distance = length(_WorldSpaceCameraPos - i.worldPos);

                // Height-based attenuation
                float height = i.worldPos.y - _FogStartHeight;
                float heightFactor = exp(-_HeightFalloff * height);

                // Exponential fog calculation
                float fogFactor = 1.0 - exp(-_FogDensity * distance * heightFactor);

                // Blend fog color with the background
                float4 fogColor = _FogColor;
                return lerp(float4(0, 0, 0, 1), fogColor, fogFactor);
            }
            ENDCG
        }
    }
    FallBack "Unlit/Transparent"
}
