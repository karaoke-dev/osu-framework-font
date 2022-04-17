varying mediump vec2 v_TexCoord;

uniform lowp sampler2D m_Sampler;

uniform mediump vec2 g_TexSize;
uniform vec4 g_Colour;
uniform vec2 g_Offset;

lowp vec4 shadow(sampler2D tex, mediump vec2 texCoord, mediump vec2 texSize, mediump vec4 colour, mediump vec2 offset)
{
    return texture2D(tex, texCoord + offset / texSize).a * colour;
}

void main(void)
{
	lowp vec4 texture = texture2D(m_Sampler, v_TexCoord);
	lowp vec4 shadow = shadow(m_Sampler, v_TexCoord, g_TexSize, g_Colour, g_Offset);
	gl_FragColor = mix(shadow, texture, texture.a);
}