// see: https://roystan.net/articles/toon-shader.html
// archived: http://archive.today/2020.10.10-213352/https://roystan.net/articles/toon-shader.html
Shader "Custom/Cel" {
    Properties {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _ShadowColor ("Shadow Color", Color) = (0, 0, 0, 0)
        _HasHighlights ("Has Highlights", Float) = 1
        _SpecularColor ("Specular Color", Color) = (1, 1, 1, 1)
        _SpecularGloss ("Specular Size", Float) = 32
        _RimColor ("Rim Color", Color) = (1, 1, 1, 1)
        _RimAmount ("Rim Amount", Range(0, 1)) = 0.716
        _RimThreshold ("Rim Threshold", Range(0, 1)) = 0.1
    }

    SubShader {
        Tags {
            "LightMode" = "ForwardBase"
            "PassFlags" = "OnlyDirectional"
            "RenderType" = "Opaque"
        }

        LOD 100

        Pass
        {
            CGPROGRAM
            // -- config --
            #pragma vertex RenderVert
            #pragma fragment RenderFrag
            #pragma multi_compile_fwdbase

            // -- includes --
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            // -- types --
            struct Vert {
                float4 pos : POSITION;
                float4 tangent : TANGENT;
                float3 normal : NORMAL;
                fixed4 vcolor : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Frag {
                float4 pos : SV_POSITION;
                float3 normal : NORMAL;
                float3 view : TEXCOORD1;
                SHADOW_COORDS(2)
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            // -- props --
            // -- props/color
            fixed4 _Color;
            fixed4 _ShadowColor;
            half _HasHighlights;
            float4 _SpecularColor;
            float _SpecularGloss;
            float4 _RimColor;
            float _RimAmount;
            float _RimThreshold;

            // -- impls --
            Frag RenderVert (Vert v) {
                Frag f;
                f.pos = UnityObjectToClipPos(v.pos);
                f.normal = UnityObjectToWorldNormal(v.normal);
                f.view = WorldSpaceViewDir(v.pos);
                TRANSFER_SHADOW(f);
                return f;
            }

            fixed4 RenderFrag (Frag f) : SV_Target {
                // capture normalized vectors for lighting calculations
                float3 dNormal = normalize(f.normal);
                float3 dView = normalize(f.view);
                float3 dSpecular = normalize(_WorldSpaceLightPos0 + dView);

                // calculate light intensity based on angle between light and normal,
                // factoring in received shadows
                float lightR = dot(_WorldSpaceLightPos0, dNormal);
                float light = smoothstep(0, 0.1, lightR * SHADOW_ATTENUATION(f));

                // use flat shadow color if this area has shadows
                if (light <= 0) {
                    return _ShadowColor;
                }

                if (_HasHighlights == 0) {
                    return _Color * _LightColor0;
                }

                // calculate specular intensity, increasing sized based on glossiness
                float specular = dot(dNormal, dSpecular);
                specular = pow(specular * light, _SpecularGloss * _SpecularGloss);
                specular = smoothstep(0.005, 0.01, specular);
                float4 specularColor = specular * _SpecularColor;

                // calculate rim light intensity, increasing size based on threshold
                float rim = 1 - dot(dView, dNormal);
                rim = rim * pow(lightR, _RimThreshold);
                rim = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rim);
                float4 rimColor = rim * _RimColor;

                // apply lighting effects to lit areas
                return _Color * (_LightColor0 + specularColor + rimColor);
            }

            ENDCG
        }

        // use built-in cast shadows
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
