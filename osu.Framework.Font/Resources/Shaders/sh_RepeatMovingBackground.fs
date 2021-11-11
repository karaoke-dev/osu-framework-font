// see : https://github.com/kiwipxl/GLSL-shaders/blob/master/repeat.glsl

varying vec2 v_TexCoord;
varying vec4 v_Colour;
varying mediump vec4 v_TexRect;

uniform lowp sampler2D m_Sampler;

uniform mediump vec2 g_TexSize;
uniform lowp sampler2D g_RepeatSample;
uniform vec2 g_RepeatSampleCoord;
uniform vec2 g_RepeatSampleSize;
uniform vec2 g_DisplaySize;
uniform vec2 g_DisplayBorder;
uniform vec2 g_Speed;
uniform float g_Time;
uniform float g_Mix;

float mod(float a, int b) {
    return a - (float(b) * floor(a/float(b)));
}

void main(void) {
    // calculate how many times texture should be repeated.
    vec2 repeat = g_TexSize / (g_DisplaySize + g_DisplayBorder);

    // get the repeat texture coordinate.
    float repeatTexCoordX = mod(v_TexCoord.x * repeat.x + g_Speed.x * g_Time, 1);
    float repeatTexCoordY = mod(v_TexCoord.y * repeat.y + g_Speed.y * g_Time, 1);
    vec2 repeatTexCoord = vec2(repeatTexCoordX, repeatTexCoordY) / g_DisplaySize * (g_DisplaySize + g_DisplayBorder);

    // because repeat texture will be the size of 1024*1024, so should make a conversion to get the target area of the texture.
    vec2 fixedTexCoord = repeatTexCoord * g_RepeatSampleSize + g_RepeatSampleCoord;

    // get point colour from sample.
    vec4 texColor = texture2D(m_Sampler, v_TexCoord);
    vec4 repeatSampleColor = v_Colour * vec4(texture2D(g_RepeatSample, fixedTexCoord).xyz, texColor.a);
    gl_FragColor = mix(texColor, repeatSampleColor, g_Mix);
}
