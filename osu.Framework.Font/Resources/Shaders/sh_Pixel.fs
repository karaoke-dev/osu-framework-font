// see the demo: https://www.geeks3d.com/20101029/shader-library-pixelation-post-processing-effect-glsl/
#include "sh_Utils.h"

varying mediump vec2 v_TexCoord;

uniform lowp sampler2D m_Sampler;

uniform mediump vec2 g_TexSize;
uniform mediump vec2 g_Size;
uniform float g_InflationPercentage;


void main(void) 
{ 
	vec2 separaorParts = g_TexSize / (g_Size * g_InflationPercentage);
	vec2 uv = v_TexCoord;
	uv = uv * separaorParts;
    uv = floor(uv);
    uv = uv / separaorParts;
    gl_FragColor = toSRGB(texture2D(m_Sampler, uv));
}