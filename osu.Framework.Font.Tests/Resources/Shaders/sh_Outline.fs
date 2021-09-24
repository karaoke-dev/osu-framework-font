#include "sh_Utils.h"

#define INV_SQRT_2PI 0.39894

varying mediump vec2 v_TexCoord;

uniform lowp sampler2D m_Sampler;

uniform mediump vec2 g_TexSize;
uniform int g_Radius;
uniform vec4 g_Colour;

lowp vec4 outline(sampler2D tex, int radius, mediump vec2 texCoord, mediump vec2 texSize, mediump vec4 colour)
{
	mediump vec4 sum = texture2D(tex, texCoord);

	for (int size = 2; size <= 200; size += 2)
	{
		// draw the circle outline with target size
		for(int degree = 0; degree < 360; degree += 5)
		{
			mediump float xx = sin(degree);
			mediump float yy = cos(degree);

			// create sample with target position.
			mediump vec4 texture = texture2D(tex, texCoord + vec2( xx, yy ) * size / texSize).a * colour;
			sum += texture;
		}
		if (size >= radius)
			break;
	}

	// draw origin texture2D in center
	sum += texture2D(tex, texCoord);

    return sum;
}

void main(void)
{
	gl_FragColor = outline(m_Sampler, g_Radius, v_TexCoord, g_TexSize, g_Colour);
}