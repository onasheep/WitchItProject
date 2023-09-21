Shader "Custom/Outline"
{
    Properties
    {
        _MainTex ( "Albedo (RGB)", 2D) = "white" {}
        _BumpMap ("NormalMap" , 2D) = "bump" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        cull back

        CGPROGRAM
        #pragma surface surf Toon

        sampler2D _MainTex;
        sampler2D _BumpMap;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
        };


        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Normal = UnpackNormal(tex2D (_BumpMap, IN.uv_BumpMap));
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }

        float4 LightingToon ( SurfaceOutput s, float3 lightDir, float viewDir , float atten)
        {
            float ndot1 = dot(s.Normal,lightDir) * 0.5 + 0.5;
            if(ndot1 > 0.7)
            {
                ndot1 = 1;
            }
            else
            {
                ndot1 = 0.3;
            }

            float rim = abs(dot(s.Normal, viewDir));
            if (rim > 0.3)
            {
                rim = 1;
            }
            else
            {
                rim = -1;
            }


            float4 final;

            final.rgb = s.Albedo * ndot1 * _LightColor0.rgb ;
            final.a = s.Alpha;
            return rim;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
