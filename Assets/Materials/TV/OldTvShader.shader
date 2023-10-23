Shader "Custom/OldTVShader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Distortion ("Distortion", Range(0.0, 1.0)) = 0.05
        _Scanline ("Scanline", Range(0.0, 1.0)) = 0.1
        _Noise ("Noise", Range(0.0, 1.0)) = 0.1
        _Darkness ("Darkness", Range(0.0, 1.0)) = 0.5
        _Contrast ("Contrast", Range(0.0, 2.0)) = 1.0
        _Vignette ("Vignette", Range(0.0, 1.0)) = 0.5
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Distortion;
            float _Scanline;
            float _Noise;
            float _Darkness;
            float _Contrast;
            float _Vignette;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float4 col = tex2D(_MainTex, i.uv);
                col.rgb *= _Darkness; // darken the image

                // Apply contrast adjustment using a lookup table
                float3 lookup = float3(0.5, 0.5, 0.5) + _Contrast * (col.rgb - float3(0.5, 0.5, 0.5));
                col.rgb = lerp(col.rgb, lookup, _Contrast);

                // Add vignette effect
                float dist = distance(i.uv, float2(0.5, 0.5));
                col.rgb *= smoothstep(1.0, 1.0 - _Vignette, dist);

                float noise = (frac(sin(dot(i.uv + _Time.y, float2(12.9898, 78.233))) * 43758.5453) * 2.0 - 1.0) * _Noise;
                float scanline = step(_Scanline, sin(i.uv.y * _Distortion + _Time.y * 5.0) * 0.5 + 0.5);
                col.rgb += noise * scanline;
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
