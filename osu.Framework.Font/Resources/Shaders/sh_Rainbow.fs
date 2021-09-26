// see the demo: https://developer.amazon.com/es/blogs/appstore/post/acefafad-29ba-4f31-8dae-00805fda3f58/intro-to-shaders-and-surfaces-with-gamemaker-studio-2
#include "sh_Utils.h"

varying vec2 v_TexCoord;
varying vec4 v_Colour;

uniform lowp sampler2D m_Sampler;

uniform vec2 u_uv;
uniform float u_speed;
uniform float u_time;
uniform float u_saturation;
uniform float u_brightness;
uniform float u_section;
uniform float u_mix;

void main(void)
{
    float pos = (v_TexCoord.y - u_uv[0]) / (u_uv[1] - u_uv[0]);
    vec4 texColor = texture2D(m_Sampler, v_TexCoord);
    
    vec4 col = vec4(u_section * ((u_time * u_speed) + pos), u_saturation, u_brightness, 1);
    vec4 finalCol = mix(texColor, vec4(hsv2rgb(col).xyz, texColor.a), u_mix);
    
    gl_FragColor = finalCol;
}