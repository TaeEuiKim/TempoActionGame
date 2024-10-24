Shader "Custom/LitOilPainting"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Radius("Radius", Range(0, 20)) = 1
        _Glossiness("Smoothness", Range(0, 1)) = 0.5
        _Metallic("Metallic", Range(0, 1)) = 0.0
        _NormalMap("Normal Map", 2D) = "bump" {}
        _LightColor("Light Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _NormalMap;
            float4 _MainTex_TexelSize;
            int _Radius;
            float _Glossiness;
            float _Metallic;
            float4 _LightColor;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                half2 uv = i.uv;

                float3 mean[4];
                float3 sigma[4];

                for (int k = 0; k < 4; k++)
                {
                    mean[k] = float3(0.0, 0.0, 0.0);
                    sigma[k] = float3(0.0, 0.0, 0.0);
                }

                float2 start[4] = {
                    {-_Radius, -_Radius},
                    {-_Radius, 0},
                    {0, -_Radius},
                    {0, 0}
                };

                float n = pow(_Radius + 1, 2);
                float3 baseColor = tex2D(_MainTex, uv).rgb;

                for (int k = 0; k < 4; k++)
                {
                    for (int i = 0; i <= _Radius; i++)
                    {
                        for (int j = 0; j <= _Radius; j++)
                        {
                            float2 pos = float2(i, j) + start[k];
                            float2 sampleUV = uv + pos * _MainTex_TexelSize.xy;
                            float3 sampleColor = tex2D(_MainTex, sampleUV).rgb;

                            mean[k] += sampleColor;
                            sigma[k] += sampleColor * sampleColor;
                        }
                    }
                    mean[k] /= n;
                    sigma[k] = abs(sigma[k] / n - mean[k] * mean[k]);
                }

                float minVariance = 1e10;
                for (int l = 0; l < 4; l++)
                {
                    float variance = sigma[l].r + sigma[l].g + sigma[l].b;
                    if (variance < minVariance)
                    {
                        minVariance = variance;
                        baseColor = mean[l];
                    }
                }

                // Lambertian lighting calculation
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float3 normal = normalize(i.normal);
                float NdotL = max(0, dot(normal, lightDir));

                float3 lambertColor = baseColor * NdotL * _LightColor.rgb;

                // Final output
                return float4(lambertColor, 1.0);
            }
            ENDCG
        }
    }
}
