#include "sh_Utils.h"

#define PI 3.14159265359
#define SAMPLES 128
#define STEP_SAMPLES 2

varying mediump vec2 v_TexCoord;

uniform lowp sampler2D m_Sampler;

uniform mediump vec2 g_TexSize;
uniform int g_Radius;
uniform vec4 g_Colour;

lowp vec4 outline(sampler2D tex, int radius, mediump vec2 texCoord, mediump vec2 texSize, mediump vec4 colour)
{
	float angle = 0.0;
	float outlineAlpha = 0.0;
	
	for (int i = 0; i < SAMPLES; i++)
	{
		angle += 1.0 / (float(SAMPLES) / 2.0) * PI;

		// todo: might need to adjust step samples amount to fill the inner side.
		// but it will cause lots of performance issue if make step samples larger.
		// so should find a better algorithm to fill inner colour.
		for (int j = 1; j <= STEP_SAMPLES; j++)
		{
			vec2 testPoint = texCoord - vec2(sin(angle), cos(angle)) * (float(radius) * (1.0 / j)) / texSize;
			float sampledAlpha = texture2D(tex, testPoint).a;
			outlineAlpha = max(outlineAlpha, sampledAlpha);
		}
	}

	mediump vec4 ogCol = texture2D(tex, texCoord);
	vec4 outlineCol = mix(vec4(0.0), colour, outlineAlpha);

	return mix(outlineCol, ogCol, ogCol.a);
}

void main(void)
{
	gl_FragColor = outline(m_Sampler, g_Radius, v_TexCoord, g_TexSize, g_Colour);
}