Shader "Custom/FullFlashlight"
{
    Properties
    {
        _FlashlightColor ("Flashlight Color", Color) = (1, 1, 1, 1) // Light color
        _FlashlightIntensity ("Intensity", Float) = 1.0           // Light intensity
        _CookieTexture ("Cookie Texture", 2D) = "white" {}        // Optional cookie texture
        _Range ("Range", Float) = 10.0                           // Range of flashlight
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float2 uv : TEXCOORD2;
            };

            float4 _FlashlightColor;
            float _FlashlightIntensity;
            float _Range;
            sampler2D _CookieTexture;

            float3 _LightPosition;   // World-space position of the flashlight
            float3 _LightDirection;  // World-space direction of the flashlight

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = mul((float3x3)unity_ObjectToWorld, v.normal);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // Calculate vector from light to fragment
                float3 toLight = normalize(_LightPosition - i.worldPos);

                // Compute the angle between light direction and surface normal
                float3 normal = normalize(i.worldNormal);
                float ndotl = saturate(dot(normal, toLight));

                // Attenuate based on distance
                float distance = length(_LightPosition - i.worldPos);
                float attenuation = saturate(1.0 - (distance / _Range));

                // Cookie effect (optional texture pattern)
                float cookie = tex2D(_CookieTexture, i.uv).r;

                // Combine lighting components
                float intensity = ndotl * attenuation * _FlashlightIntensity * cookie;
                float4 color = _FlashlightColor * intensity;

                return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
