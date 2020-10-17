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
            "RenderType" = "Opaque"
        }

        LOD 100

        Pass {
            Tags {
                "LightMode" = "ForwardBase"
            }

            CGPROGRAM
            // -- config --
            #pragma vertex RenderVert
            #pragma fragment RenderFrag
            #pragma multi_compile_fwdbase

            // -- includes --
            #include "CelLight.cginc"

            ENDCG
        }

        Pass {
            Tags {
                "LightMode" = "ForwardAdd"
            }

            CGPROGRAM
            // -- config --
            #pragma multi_compile_fwdadd_fullshadows
            #pragma vertex RenderVert
            #pragma fragment RenderFrag

            // -- includes --
            #include "CelLight.cginc"

            ENDCG
        }

        // use built-in cast shadows
        UsePass "VertexLit/SHADOWCASTER"
    }
}
