// see the demo: https://developer.amazon.com/es/blogs/appstore/post/acefafad-29ba-4f31-8dae-00805fda3f58/intro-to-shaders-and-surfaces-with-gamemaker-studio-2
#include "sh_Utils.h"

varying vec2 v_TexCoord;
varying vec4 v_Colour;

uniform lowp sampler2D m_Sampler;

uniform vec2 g_Uv;
uniform float g_Speed;
uniform float g_Time;
uniform float g_Saturation;
uniform float g_Brightness;
uniform float g_Section;
uniform float g_Mix;

void main(void)
{
    float pos = (v_TexCoord.y - g_Uv[0]) / (g_Uv[1] - g_Uv[0]);
    vec4 texColor = toSRGB(texture2D(m_Sampler, v_TexCoord));
    
    vec4 col = vec4(g_Section * ((g_Time * g_Speed) + pos), g_Saturation, g_Brightness, 1);
    vec4 finalCol = mix(texColor, vec4(hsv2rgb(col).xyz, texColor.a), g_Mix);
    
    gl_FragColor = finalCol;
}