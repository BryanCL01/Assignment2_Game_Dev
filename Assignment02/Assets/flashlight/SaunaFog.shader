Shader "Custom/SaunaFog"
{
    Properties
    {
        _FogColor ("Fog Color", Color) = (0.8, 0.8, 0.8, 1)
        _FogDensity ("Fog Density", Float) = 0.3 // Fog density controls opacity
        _FogSpeed ("Fog Speed", Float) = 0.1
        _NoiseScale ("Noise Scale", Float) = 1.0
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
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _FogColor;
            float _FogDensity;
            float _FogSpeed;
            float _NoiseScale;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            // Simple 3D noise function (based on Perlin noise)
            float hash(float n) { return frac(sin(n) * 43758.5453); }

            float noise(float3 p)
            {
                float3 i = floor(p);
                float3 f = frac(p);
                f = f * f * (3.0 - 2.0 * f);

                return lerp(lerp(lerp(hash(i.x + i.y * 57.0 + i.z * 113.0), 
                                    hash(i.x + 1.0 + i.y * 57.0 + i.z * 113.0), f.x),
                               lerp(hash(i.x + (i.y + 1.0) * 57.0 + i.z * 113.0), 
                                    hash(i.x + 1.0 + (i.y + 1.0) * 57.0 + i.z * 113.0), f.x), f.y),
                          lerp(lerp(hash(i.x + i.y * 57.0 + (i.z + 1.0) * 113.0), 
                                    hash(i.x + 1.0 + i.y * 57.0 + (i.z + 1.0) * 113.0), f.x),
                               lerp(hash(i.x + (i.y + 1.0) * 57.0 + (i.z + 1.0) * 113.0), 
                                    hash(i.x + 1.0 + (i.y + 1.0) * 57.0 + (i.z + 1.0) * 113.0), f.x), f.y), f.z);
            }

            half4 frag (v2f i) : SV_Target
            {
                float timeOffset = _Time.y * _FogSpeed; // Use Unity's built-in _Time variable

                // Scale and animate the noise
                float fogFactor = noise(float3(i.worldPos * _NoiseScale + timeOffset));

                // Adjust fog density
                fogFactor = saturate(fogFactor * _FogDensity);

                // Return fog color directly (no base color blending)
                return float4(_FogColor.rgb, fogFactor);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
