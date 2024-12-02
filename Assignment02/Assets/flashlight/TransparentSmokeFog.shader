Shader "Custom/TransparentSmokeFog"
{
    Properties
    {
        _FogColor ("Fog Color", Color) = (0.8, 0.8, 0.8, 1)
        _FogIntensity ("Fog Intensity", Float) = 0.5 // Controls visibility of the fog
        _NoiseScale ("Noise Scale", Float) = 1.0 // Scale of noise patterns
        _FogSpeed ("Fog Speed", Float) = 0.1 // Speed of noise animation
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha // Enable transparency blending
        ZWrite Off // Prevents writing to the depth buffer for correct transparency

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

            float4 _FogColor;
            float _FogIntensity;
            float _NoiseScale;
            float _FogSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            // Simple Perlin-like noise function
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
                float timeOffset = _Time.y * _FogSpeed;

                // Generate animated noise
                float fogNoise = noise(float3(i.worldPos.xy * _NoiseScale, timeOffset));
                fogNoise = saturate(fogNoise * _FogIntensity);

                // Apply the noise effect to the transparency
                float alpha = fogNoise; // Use noise to control transparency

                // Return fog color with alpha channel
                return float4(_FogColor.rgb, alpha);
            }
            ENDCG
        }
    }
}
