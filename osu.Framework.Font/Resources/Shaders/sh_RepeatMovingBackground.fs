// see : https://github.com/kiwipxl/GLSL-shaders/blob/master/repeat.glsl

varying vec2 v_TexCoord;
varying vec4 v_Colour;

uniform lowp sampler2D m_Sampler;

uniform lowp sampler2D g_RepeatSample;
uniform vec2 g_RepeatSampleCoord;
uniform vec2 g_RepeatSampleSize;
uniform vec2 g_TextureSize;
uniform vec2 g_Offset;

void main(void) {
    // get the repeat position
    float repeatTexCoordX = mod(v_TexCoord.x * g_TextureSize.x + g_Offset.x, 1);
    float repeatTexCoordY = mod(v_TexCoord.y * g_TextureSize.y + g_Offset.y, 1);
    vec2 repeatTexCoord = vec2(repeatTexCoordX, repeatTexCoordY);

    // because repeat texture will be the size of 1024*1024, so should make a conversion to get the target area of the texture.
    vec2 fixedTexCoord = repeatTexCoord * g_RepeatSampleSize + g_RepeatSampleCoord;

    // get point colour from sample.
    gl_FragColor = v_Colour * texture2D(g_RepeatSample, fixedTexCoord);
}
