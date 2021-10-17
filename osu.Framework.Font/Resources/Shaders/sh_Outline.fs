#include "sh_Utils.h"

varying mediump vec2 v_TexCoord;

uniform lowp sampler2D m_Sampler;

uniform mediump vec2 g_TexSize;
uniform int g_Radius;
uniform vec4 g_Colour;

lowp vec4 outline(sampler2D tex, int radius, mediump vec2 texCoord, mediump vec2 texSize, mediump vec4 colour)
{
	mediump vec4 sum = texture2D(tex, texCoord);

	for (int size = 2; size <= radius; size += 2)
	{
		// draw the circle outline with target size
		for(int degree = 0; degree < 360; degree += 5)
		{
			mediump vec2 offset = vec2(sin(degree), cos(degree)) * size / texSize;

			// create sample with target position.
			sum += texture2D(tex, texCoord + offset).a * colour;
		}
	}

	// draw origin texture2D in center
	sum += texture2D(tex, texCoord);

    return sum;
}

void main(void)
{
	gl_FragColor = outline(m_Sampler, g_Radius, v_TexCoord, g_TexSize, g_Colour);
}