Shader "Custom/SimpleFogShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        _FogColor ("Fog Color", Color) = (0.5, 0.5, 0.5, 1)
        _FogStartDistance ("Fog Start Distance", Float) = 10.0
        _FogEndDistance ("Fog End Distance", Float) = 50.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _BaseColor;
            float4 _FogColor;
            float _FogStartDistance;
            float _FogEndDistance;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // Compute the distance from the camera to the fragment
                float3 viewPos = i.worldPos - _WorldSpaceCameraPos;
                float distance = length(viewPos);

                // Compute fog factor
                float fogFactor = saturate((distance - _FogStartDistance) / (_FogEndDistance - _FogStartDistance));

                // Blend base color with fog color
                return lerp(_BaseColor, _FogColor, fogFactor);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
