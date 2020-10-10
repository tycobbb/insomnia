Shader "Custom/WavyShader" {
    // -- fields --
    Properties {
        // -- fields/color
        _MeshColor ("Color", Color) = (1,1,1,1)
        _VertColorAlpha ("Vertex Color Alpha", Range(0, 1)) = 0.05

        // -- fields/animation
        _AnimSpeedX ("Distrotion Speed.X", Range(0, 4)) = 1.1
        _AnimSpeedY ("Distrotion Speed.Y", Range(0, 4)) = 1.4
        _AnimAmplitude ("Distortion Amplitude", Range(0, 1)) = 0.03
        _AnimFrequency ("Distortion Frequency", Range(0, 20)) = 3
    }

    // -- shader --
    SubShader {
        Tags {
          "RenderType"="Opaque"
        }

        LOD 100

        // -- program --
        Pass {
            CGPROGRAM

            // -- config --
            #pragma vertex RenderVert
            #pragma fragment RenderFrag

            // -- includes --
            #include "UnityCG.cginc"

            // -- types --
            struct Vert {
                float4 pos : POSITION;
                float4 tangent : TANGENT;
                float3 normal : NORMAL;
                // float4 texcoord : TEXCOORD0;
                // float4 texcoord1 : TEXCOORD1;
                // float4 texcoord2 : TEXCOORD2;
                // float4 texcoord3 : TEXCOORD3;
                fixed4 vcolor : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Frag {
                float4 pos : SV_POSITION;
                fixed3 color : COLOR0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            // -- props --
            // -- props/color
            fixed4 _MeshColor;
            half _VertColorAlpha;

            // -- props/animation
            float _AnimSpeedX;
            float _AnimSpeedY;
            float _AnimAmplitude;
            float _AnimFrequency;

            // -- impls --
            Frag RenderVert (Vert v) {
                Frag f;
                // f.color = sin(3 * v.pos.y + _Time.z) * 0.5 + 0.5;
                // v.pos.xyz += v.normal * (sin(5 * v.pos.y + _Time.z) * 0.5 + 0.5) * 0.01;
                // v.pos.xyz += v.normal * (sin(v.pos.y + _Time.z) * 0.5 + 0.5) * 0.01;
                // v.pos.xyz += (v.normal * (sin(v.pos.y + _Time.y) * 0.5 + 0.5)) * 0.01;
                // f.pos = UnityObjectToClipPos(v.pos);

                v.pos.x += sin((v.pos.x + v.pos.y) * _AnimFrequency + _Time.y * _AnimSpeedX) * _AnimAmplitude;
                v.pos.y += cos((v.pos.x - v.pos.y) * _AnimFrequency + _Time.y * _AnimSpeedY) * _AnimAmplitude;

                f.pos = UnityObjectToClipPos(v.pos);
                f.color = _MeshColor + v.vcolor * _VertColorAlpha;

                return f;
            }

            fixed4 RenderFrag (Frag f) : SV_Target {
                return fixed4 (f.color, 1);
            }

            ENDCG
        }
    }
}
