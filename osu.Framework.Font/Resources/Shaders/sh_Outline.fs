#define PI 3.14159265359
#define SAMPLES 128
#define STEP_SAMPLES 2

varying mediump vec2 v_TexCoord;

uniform lowp sampler2D m_Sampler;

uniform mediump vec2 g_TexSize;
uniform int g_Radius;
uniform vec4 g_Colour;

const lowp vec2 angelPosition[SAMPLES] = lowp vec2[](
    lowp vec2(0.05, 1.00),
    lowp vec2(0.10, 1.00),
    lowp vec2(0.15, 0.99),
    lowp vec2(0.20, 0.98),
    lowp vec2(0.24, 0.97),
    lowp vec2(0.29, 0.96),
    lowp vec2(0.34, 0.94),
    lowp vec2(0.38, 0.92),
    lowp vec2(0.43, 0.90),
    lowp vec2(0.47, 0.88),
    lowp vec2(0.51, 0.86),
    lowp vec2(0.56, 0.83),
    lowp vec2(0.60, 0.80),
    lowp vec2(0.63, 0.77),
    lowp vec2(0.67, 0.74),
    lowp vec2(0.71, 0.71),
    lowp vec2(0.74, 0.67),
    lowp vec2(0.77, 0.63),
    lowp vec2(0.80, 0.60),
    lowp vec2(0.83, 0.56),
    lowp vec2(0.86, 0.51),
    lowp vec2(0.88, 0.47),
    lowp vec2(0.90, 0.43),
    lowp vec2(0.92, 0.38),
    lowp vec2(0.94, 0.34),
    lowp vec2(0.96, 0.29),
    lowp vec2(0.97, 0.24),
    lowp vec2(0.98, 0.20),
    lowp vec2(0.99, 0.15),
    lowp vec2(1.00, 0.10),
    lowp vec2(1.00, 0.05),
    lowp vec2(1.00, -0.00),
    lowp vec2(1.00, -0.05),
    lowp vec2(1.00, -0.10),
    lowp vec2(0.99, -0.15),
    lowp vec2(0.98, -0.20),
    lowp vec2(0.97, -0.24),
    lowp vec2(0.96, -0.29),
    lowp vec2(0.94, -0.34),
    lowp vec2(0.92, -0.38),
    lowp vec2(0.90, -0.43),
    lowp vec2(0.88, -0.47),
    lowp vec2(0.86, -0.51),
    lowp vec2(0.83, -0.56),
    lowp vec2(0.80, -0.60),
    lowp vec2(0.77, -0.63),
    lowp vec2(0.74, -0.67),
    lowp vec2(0.71, -0.71),
    lowp vec2(0.67, -0.74),
    lowp vec2(0.63, -0.77),
    lowp vec2(0.60, -0.80),
    lowp vec2(0.56, -0.83),
    lowp vec2(0.51, -0.86),
    lowp vec2(0.47, -0.88),
    lowp vec2(0.43, -0.90),
    lowp vec2(0.38, -0.92),
    lowp vec2(0.34, -0.94),
    lowp vec2(0.29, -0.96),
    lowp vec2(0.24, -0.97),
    lowp vec2(0.20, -0.98),
    lowp vec2(0.15, -0.99),
    lowp vec2(0.10, -1.00),
    lowp vec2(0.05, -1.00),
    lowp vec2(-0.00, -1.00),
    lowp vec2(-0.05, -1.00),
    lowp vec2(-0.10, -1.00),
    lowp vec2(-0.15, -0.99),
    lowp vec2(-0.20, -0.98),
    lowp vec2(-0.24, -0.97),
    lowp vec2(-0.29, -0.96),
    lowp vec2(-0.34, -0.94),
    lowp vec2(-0.38, -0.92),
    lowp vec2(-0.43, -0.90),
    lowp vec2(-0.47, -0.88),
    lowp vec2(-0.51, -0.86),
    lowp vec2(-0.56, -0.83),
    lowp vec2(-0.60, -0.80),
    lowp vec2(-0.63, -0.77),
    lowp vec2(-0.67, -0.74),
    lowp vec2(-0.71, -0.71),
    lowp vec2(-0.74, -0.67),
    lowp vec2(-0.77, -0.63),
    lowp vec2(-0.80, -0.60),
    lowp vec2(-0.83, -0.56),
    lowp vec2(-0.86, -0.51),
    lowp vec2(-0.88, -0.47),
    lowp vec2(-0.90, -0.43),
    lowp vec2(-0.92, -0.38),
    lowp vec2(-0.94, -0.34),
    lowp vec2(-0.96, -0.29),
    lowp vec2(-0.97, -0.24),
    lowp vec2(-0.98, -0.20),
    lowp vec2(-0.99, -0.15),
    lowp vec2(-1.00, -0.10),
    lowp vec2(-1.00, -0.05),
    lowp vec2(-1.00, 0.00),
    lowp vec2(-1.00, 0.05),
    lowp vec2(-1.00, 0.10),
    lowp vec2(-0.99, 0.15),
    lowp vec2(-0.98, 0.20),
    lowp vec2(-0.97, 0.24),
    lowp vec2(-0.96, 0.29),
    lowp vec2(-0.94, 0.34),
    lowp vec2(-0.92, 0.38),
    lowp vec2(-0.90, 0.43),
    lowp vec2(-0.88, 0.47),
    lowp vec2(-0.86, 0.51),
    lowp vec2(-0.83, 0.56),
    lowp vec2(-0.80, 0.60),
    lowp vec2(-0.77, 0.63),
    lowp vec2(-0.74, 0.67),
    lowp vec2(-0.71, 0.71),
    lowp vec2(-0.67, 0.74),
    lowp vec2(-0.63, 0.77),
    lowp vec2(-0.60, 0.80),
    lowp vec2(-0.56, 0.83),
    lowp vec2(-0.51, 0.86),
    lowp vec2(-0.47, 0.88),
    lowp vec2(-0.43, 0.90),
    lowp vec2(-0.38, 0.92),
    lowp vec2(-0.34, 0.94),
    lowp vec2(-0.29, 0.96),
    lowp vec2(-0.24, 0.97),
    lowp vec2(-0.20, 0.98),
    lowp vec2(-0.15, 0.99),
    lowp vec2(-0.10, 1.00),
    lowp vec2(-0.05, 1.00),
    lowp vec2(0.00, 1.00)
);

lowp vec4 outline(sampler2D tex, int radius, mediump vec2 texCoord, mediump vec2 texSize, mediump vec4 colour)
{
	float outlineAlpha = 0.0;
	
	for (int i = 0; i < SAMPLES; i++)
	{
		// todo: might need to adjust step samples amount to fill the inner side.
		// but it will cause lots of performance issue if make step samples larger.
		// so should find a better algorithm to fill inner colour.
		for (int j = 1; j <= STEP_SAMPLES; j++)
		{
			if(outlineAlpha < 0.99) 
			{
				vec2 testPoint = texCoord - angelPosition[i] * (float(radius) * (1.0 / j)) / texSize;
				float sampledAlpha = texture2D(tex, testPoint).a;
				outlineAlpha = max(outlineAlpha, sampledAlpha);
			}
		}
	}

	mediump vec4 ogCol = texture2D(tex, texCoord);
	vec4 outlineCol = mix(vec4(0.0), colour, outlineAlpha);

	return mix(outlineCol, ogCol, ogCol.a);
}

void main(void)
{
	gl_FragColor = outline(m_Sampler, g_Radius, v_TexCoord, g_TexSize, g_Colour);
}