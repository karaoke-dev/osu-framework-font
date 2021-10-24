// see : https://github.com/kiwipxl/GLSL-shaders/blob/master/repeat.glsl

varying vec2 v_TexCoord;
varying vec4 v_Colour;

uniform lowp sampler2D g_RepeatSample;
uniform vec2 g_Repeat;

void main(void) {
    gl_FragColor = v_Colour * texture2D(g_RepeatSample, vec2(mod(v_TexCoord.x * g_Repeat.x, 1), mod(v_TexCoord.y * g_Repeat.y, 1)));
}
