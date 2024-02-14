Shader "Custom/2DOutlineShader"
{
    Properties
    {
        _Color("Main Color", Color) = (1,1,1,1)
        _Outline("Outline color", Color) = (0,0,0,1)
        _MainTex("Base (RGB)", 2D) = "white" { }
        _OutlineWidth("Outline width", Range(.002, 0.03)) = .005
    }

        CGINCLUDE
#include "UnityCG.cginc"

            struct appdata
        {
            float4 vertex : POSITION;
        };

        struct v2f
        {
            float4 pos : POSITION;
            float4 color : COLOR;
        };

        uniform float _OutlineWidth;

        v2f vert(appdata v)
        {
            // just make a copy of incoming vertex data but scaled according to normal direction
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            float normSign = (o.pos.z > 0) ? 1.0 : -1.0;
            o.pos.xy += normSign * o.pos.xy * _OutlineWidth;
            return o;
        }
        ENDCG

            SubShader
        {
            Tags {"Queue" = "Overlay" }
            Pass
            {
                Name "OUTLINE"
                Tags {"LightMode" = "Always" }
                ZWrite On
                ZTest LEqual
                Blend SrcAlpha OneMinusSrcAlpha
                Cull Front
                ColorMask RGB
                Offset 10,10

                CGPROGRAM
                #pragma vertex vert
                #pragma exclude_renderers gles xbox360 ps3
                ENDCG
            }
        }

            SubShader
        {
            Tags {"Queue" = "Overlay" }
            Pass
            {
                Name "OUTLINE"
                Tags {"LightMode" = "Always" }
                ZWrite On
                ZTest LEqual
                Blend SrcAlpha OneMinusSrcAlpha
                Cull Front
                ColorMask RGB
                Offset 10,10

                ColorMask RGB
                Blend SrcAlpha OneMinusSrcAlpha

                CGPROGRAM
                #pragma vertex vert
                #pragma exclude_renderers gles xbox360 ps3
                ENDCG
            }
        }

            SubShader
        {
            Tags {"Queue" = "Overlay" }
            LOD 100

            CGPROGRAM
            #pragma surface surf Lambert

            sampler2D _MainTex;

            fixed4 LightingLambert(SurfaceOutput s, fixed3 lightDir, fixed atten)
            {
                fixed4 c;
                c.rgb = s.Albedo * 2 * _LightColor0.rgb * (lightDir.x * 0.5 + 0.5);
                c.a = s.Alpha;
                return c;
            }
            ENDCG
        }
            Fallback "Diffuse"
}
