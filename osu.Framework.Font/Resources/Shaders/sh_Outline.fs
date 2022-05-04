#include "sh_Utils.h"

varying mediump vec2 v_TexCoord;

uniform lowp sampler2D m_Sampler;

uniform mediump vec2 g_TexSize;
uniform vec4 g_Colour;
uniform float g_Radius;
uniform vec4 g_OutlineColour;
uniform float g_InflationPercentage;

lowp float outlineAlpha(sampler2D tex, float radius, mediump vec2 texCoord, mediump vec2 texSize)
{
    mediump vec2 offset = mediump vec2(radius) / texSize;
    lowp float alpha = 0.0;

    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.20, 0.98) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.38, 0.92) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.56, 0.83) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.71, 0.71) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.83, 0.56) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.92, 0.38) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.98, 0.20) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(1.00, -0.00) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.98, -0.20) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.92, -0.38) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.83, -0.56) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.71, -0.71) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.56, -0.83) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.38, -0.92) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.20, -0.98) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.00, -1.00) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.20, -0.98) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.38, -0.92) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.56, -0.83) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.71, -0.71) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.83, -0.56) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.92, -0.38) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.98, -0.20) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-1.00, 0.00) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.98, 0.20) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.92, 0.38) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.83, 0.56) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.71, 0.71) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.56, 0.83) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.38, 0.92) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(-0.20, 0.98) * offset).a);
    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2(0.00, 1.00) * offset).a);
    return alpha;
}

lowp vec4 outline(sampler2D tex, float radius, mediump vec2 texCoord, mediump vec2 texSize, mediump vec4 colour)
{
	lowp float outlineAlpha = max(outlineAlpha(tex, radius, texCoord, texSize), outlineAlpha(tex, radius / 2.0, texCoord, texSize));
	return mix(vec4(0.0), colour, outlineAlpha);
}

void main(void)
{
	lowp vec4 sample = toSRGB(texture2D(m_Sampler, v_TexCoord));
	lowp vec4 originColur = vec4(mix(sample.rgb, g_Colour.rgb, g_Colour.a), sample.a);
	lowp vec4 outlineColour = outline(m_Sampler, g_Radius * g_InflationPercentage, v_TexCoord, g_TexSize, g_OutlineColour);

	gl_FragColor = mix(outlineColour, originColur, originColur.a);
}