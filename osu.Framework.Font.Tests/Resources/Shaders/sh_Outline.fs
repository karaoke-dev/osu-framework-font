varying mediump vec2 v_TexCoord;
uniform lowp sampler2D m_Sampler;

const float offset = 1.0 / 256.0;

void main(void)
{
	vec4 col = texture2D(m_Sampler, v_TexCoord);
	if (col.a > 0.5)
		gl_FragColor = col;
	else {
		float a = texture2D(m_Sampler, vec2(v_TexCoord.x + offset, v_TexCoord.y)).a +
			texture2D(m_Sampler, vec2(v_TexCoord.x, v_TexCoord.y - offset)).a +
			texture2D(m_Sampler, vec2(v_TexCoord.x - offset, v_TexCoord.y)).a +
			texture2D(m_Sampler, vec2(v_TexCoord.x, v_TexCoord.y + offset)).a;
		if (col.a < 1.0 && a > 0.0)
			gl_FragColor = vec4(0.0, 0.0, 1.0, 0.8);
		else
			gl_FragColor = col;
	}
}
