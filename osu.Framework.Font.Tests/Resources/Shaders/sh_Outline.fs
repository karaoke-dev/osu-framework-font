varying mediump vec2 v_TexCoord;
uniform lowp sampler2D m_Sampler;
uniform float g_outlineRadius;
uniform vec4 g_outlineColour;

void main(void)
{
	vec4 col = texture2D(m_Sampler, v_TexCoord);
	if (col.a > 0.5)
		gl_FragColor = col;
	else {
		float a = texture2D(m_Sampler, vec2(v_TexCoord.x + g_outlineRadius, v_TexCoord.y)).a +
			texture2D(m_Sampler, vec2(v_TexCoord.x, v_TexCoord.y - g_outlineRadius)).a +
			texture2D(m_Sampler, vec2(v_TexCoord.x - g_outlineRadius, v_TexCoord.y)).a +
			texture2D(m_Sampler, vec2(v_TexCoord.x, v_TexCoord.y + g_outlineRadius)).a;
		if (col.a < 1.0 && a > 0.0)
			gl_FragColor = g_outlineColour;
		else
			gl_FragColor = col;
	}
}
