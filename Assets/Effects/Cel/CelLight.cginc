#ifndef CEL_LIGHT_CGINC
#define CEL_LIGHT_CGINC

// -- includes --
#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "AutoLight.cginc"

// -- types --
struct Vert {
    float4 pos : POSITION;
    float3 normal : NORMAL;
};

struct Frag {
    float4 pos : SV_POSITION;
    float3 normal : NORMAL;
    float3 view : TEXCOORD1;
    float3 world : TEXCOORD2;
    SHADOW_COORDS(2)
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
    f.world = mul(unity_ObjectToWorld, v.pos);
    f.view = UnityWorldSpaceViewDir(f.world);
    f.normal = UnityObjectToWorldNormal(v.normal);
    TRANSFER_SHADOW(f);
    return f;
}

fixed4 RenderFrag (Frag f) : SV_Target {
#ifdef DIRECTIONAL
    float3 iLight = _WorldSpaceLightPos0.xyz;
#else
    float3 iLight = _WorldSpaceLightPos0.xyz - f.world;
#endif

    // capture normalized vectors for lighting calculations
    float3 dNormal = normalize(f.normal);
    float3 dView = normalize(f.view);
    float3 dLight = normalize(iLight);
    float3 dSpecular = normalize(dLight + dView);

    // calculate light intensity based on angle between normal and light dir,
    // attenuating point lights and shadows
    float lightR = dot(dNormal, dLight);
    float light = lightR;
    light = light * (1.0f / (1.0f + dot(iLight, iLight))) * SHADOW_ATTENUATION(f);
    light = smoothstep(0.0f, 0.1f, light);

    // use flat shadow color if this area has shadows
    if (light <= 0.0f) {
        return _ShadowColor;
    }

    if (_HasHighlights == 0.0f) {
        return _Color * _LightColor0;
    }

    // calculate specular intensity, increasing sized based on glossiness
    float specular = dot(dNormal, dSpecular);
    specular = pow(specular * light, _SpecularGloss * _SpecularGloss);
    specular = smoothstep(0.005f, 0.01f, specular);
    float4 specularColor = specular * _SpecularColor;

    // calculate rim light intensity, increasing size based on threshold
    float rim = 1.0f - dot(dView, dNormal);
    rim = rim * pow(lightR, _RimThreshold);
    rim = smoothstep(_RimAmount - 0.01f, _RimAmount + 0.01f, rim);
    float4 rimColor = rim * _RimColor;

    // apply lighting effects to lit areas
    return _Color * (_LightColor0 + specularColor + rimColor);
}

#endif
