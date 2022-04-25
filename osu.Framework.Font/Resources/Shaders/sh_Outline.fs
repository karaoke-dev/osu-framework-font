#include "sh_Utils.h"

#define STEP_SAMPLES 2

varying mediump vec2 v_TexCoord;

uniform lowp sampler2D m_Sampler;

uniform mediump vec2 g_TexSize;
uniform vec4 g_Colour;
uniform int g_Radius;
uniform vec4 g_OutlineColour;

lowp vec4 outline(sampler2D tex, int radius, mediump vec2 texCoord, mediump vec2 texSize, mediump vec4 colour)
{
    mediump vec2 offset = mediump vec2(float(radius)) / texSize;
    float alpha = 0.0;

    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.05, 1.00) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.10, 1.00) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.15, 0.99) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.20, 0.98) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.24, 0.97) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.29, 0.96) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.34, 0.94) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.38, 0.92) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.43, 0.90) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.47, 0.88) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.51, 0.86) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.56, 0.83) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.60, 0.80) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.63, 0.77) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.67, 0.74) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.71, 0.71) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.74, 0.67) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.77, 0.63) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.80, 0.60) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.83, 0.56) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.86, 0.51) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.88, 0.47) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.90, 0.43) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.92, 0.38) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.94, 0.34) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.96, 0.29) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.97, 0.24) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.98, 0.20) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.99, 0.15) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(1.00, 0.10) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(1.00, 0.05) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(1.00, -0.00) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(1.00, -0.05) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(1.00, -0.10) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.99, -0.15) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.98, -0.20) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.97, -0.24) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.96, -0.29) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.94, -0.34) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.92, -0.38) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.90, -0.43) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.88, -0.47) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.86, -0.51) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.83, -0.56) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.80, -0.60) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.77, -0.63) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.74, -0.67) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.71, -0.71) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.67, -0.74) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.63, -0.77) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.60, -0.80) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.56, -0.83) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.51, -0.86) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.47, -0.88) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.43, -0.90) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.38, -0.92) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.34, -0.94) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.29, -0.96) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.24, -0.97) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.20, -0.98) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.15, -0.99) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.10, -1.00) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.05, -1.00) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.00, -1.00) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.05, -1.00) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.10, -1.00) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.15, -0.99) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.20, -0.98) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.24, -0.97) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.29, -0.96) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.34, -0.94) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.38, -0.92) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.43, -0.90) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.47, -0.88) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.51, -0.86) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.56, -0.83) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.60, -0.80) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.63, -0.77) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.67, -0.74) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.71, -0.71) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.74, -0.67) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.77, -0.63) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.80, -0.60) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.83, -0.56) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.86, -0.51) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.88, -0.47) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.90, -0.43) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.92, -0.38) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.94, -0.34) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.96, -0.29) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.97, -0.24) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.98, -0.20) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.99, -0.15) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-1.00, -0.10) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-1.00, -0.05) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-1.00, 0.00) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-1.00, 0.05) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-1.00, 0.10) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.99, 0.15) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.98, 0.20) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.97, 0.24) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.96, 0.29) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.94, 0.34) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.92, 0.38) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.90, 0.43) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.88, 0.47) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.86, 0.51) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.83, 0.56) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.80, 0.60) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.77, 0.63) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.74, 0.67) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.71, 0.71) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.67, 0.74) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.63, 0.77) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.60, 0.80) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.56, 0.83) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.51, 0.86) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.47, 0.88) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.43, 0.90) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.38, 0.92) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.34, 0.94) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.29, 0.96) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.24, 0.97) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.20, 0.98) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.15, 0.99) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.10, 1.00) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.05, 1.00) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.00, 1.00) * offset).a);
    return mix(vec4(0.0), colour, alpha);
}

void main(void)
{
	lowp vec4 sample = toSRGB(texture2D(m_Sampler, v_TexCoord));
	lowp vec4 originColur = vec4(mix(sample.rgb, g_Colour.rgb, g_Colour.a), sample.a);
	lowp vec4 outlineColour = outline(m_Sampler, g_Radius, v_TexCoord, g_TexSize, g_OutlineColour);

	gl_FragColor = mix(outlineColour, originColur, originColur.a);
}