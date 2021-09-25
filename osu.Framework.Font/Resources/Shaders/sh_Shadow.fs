#include "sh_Utils.h"

varying mediump vec2 v_TexCoord;

uniform lowp sampler2D m_Sampler;

uniform mediump vec2 g_TexSize;
uniform vec4 g_Colour;
uniform vec2 g_Offset;

lowp vec4 outline(sampler2D tex, mediump vec2 texCoord, mediump vec2 texSize, mediump vec4 colour, mediump vec2 offset)
{
	mediump vec4 sum = texture2D(tex, texCoord);

	// draw shader with target offset and colour.
	sum += texture2D(tex, texCoord + offset / texSize).a * colour;

	// draw origin texture2D in center
	sum += texture2D(tex, texCoord);

    return sum;
}

void main(void)
{
	gl_FragColor = outline(m_Sampler, v_TexCoord, g_TexSize, g_Colour, g_Offset);
}