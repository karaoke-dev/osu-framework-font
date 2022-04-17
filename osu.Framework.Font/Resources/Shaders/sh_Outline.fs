#define PI 3.14159265359
#define SAMPLES 128
#define STEP_SAMPLES 2

varying mediump vec2 v_TexCoord;

uniform lowp sampler2D m_Sampler;

uniform mediump vec2 g_TexSize;
uniform int g_Radius;
uniform vec4 g_OutlineColour;

lowp vec2 angelPosition[128];

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
				vec2 testPoint = texCoord - angelPosition[i] * (float(radius) * (1.0 / float(j))) / texSize;
				float sampledAlpha = texture2D(tex, testPoint).a;
				outlineAlpha = max(outlineAlpha, sampledAlpha);
			}
		}
	}

	return mix(vec4(0.0), colour, outlineAlpha);
}

void main(void)
{
	angelPosition[0] = lowp vec2(0.05, 1.00);
	angelPosition[1] = lowp vec2(0.10, 1.00);
	angelPosition[2] = lowp vec2(0.15, 0.99);
	angelPosition[3] = lowp vec2(0.20, 0.98);
	angelPosition[4] = lowp vec2(0.24, 0.97);
	angelPosition[5] = lowp vec2(0.29, 0.96);
	angelPosition[6] = lowp vec2(0.34, 0.94);
	angelPosition[7] = lowp vec2(0.38, 0.92);
	angelPosition[8] = lowp vec2(0.43, 0.90);
	angelPosition[9] = lowp vec2(0.47, 0.88);
	angelPosition[10] = lowp vec2(0.51, 0.86);
	angelPosition[11] = lowp vec2(0.56, 0.83);
	angelPosition[12] = lowp vec2(0.60, 0.80);
	angelPosition[13] = lowp vec2(0.63, 0.77);
	angelPosition[14] = lowp vec2(0.67, 0.74);
	angelPosition[15] = lowp vec2(0.71, 0.71);
	angelPosition[16] = lowp vec2(0.74, 0.67);
	angelPosition[17] = lowp vec2(0.77, 0.63);
	angelPosition[18] = lowp vec2(0.80, 0.60);
	angelPosition[19] = lowp vec2(0.83, 0.56);
	angelPosition[20] = lowp vec2(0.86, 0.51);
	angelPosition[21] = lowp vec2(0.88, 0.47);
	angelPosition[22] = lowp vec2(0.90, 0.43);
	angelPosition[23] = lowp vec2(0.92, 0.38);
	angelPosition[24] = lowp vec2(0.94, 0.34);
	angelPosition[25] = lowp vec2(0.96, 0.29);
	angelPosition[26] = lowp vec2(0.97, 0.24);
	angelPosition[27] = lowp vec2(0.98, 0.20);
	angelPosition[28] = lowp vec2(0.99, 0.15);
	angelPosition[29] = lowp vec2(1.00, 0.10);
	angelPosition[30] = lowp vec2(1.00, 0.05);
	angelPosition[31] = lowp vec2(1.00, -0.00);
	angelPosition[32] = lowp vec2(1.00, -0.05);
	angelPosition[33] = lowp vec2(1.00, -0.10);
	angelPosition[34] = lowp vec2(0.99, -0.15);
	angelPosition[35] = lowp vec2(0.98, -0.20);
	angelPosition[36] = lowp vec2(0.97, -0.24);
	angelPosition[37] = lowp vec2(0.96, -0.29);
	angelPosition[38] = lowp vec2(0.94, -0.34);
	angelPosition[39] = lowp vec2(0.92, -0.38);
	angelPosition[40] = lowp vec2(0.90, -0.43);
	angelPosition[41] = lowp vec2(0.88, -0.47);
	angelPosition[42] = lowp vec2(0.86, -0.51);
	angelPosition[43] = lowp vec2(0.83, -0.56);
	angelPosition[44] = lowp vec2(0.80, -0.60);
	angelPosition[45] = lowp vec2(0.77, -0.63);
	angelPosition[46] = lowp vec2(0.74, -0.67);
	angelPosition[47] = lowp vec2(0.71, -0.71);
	angelPosition[48] = lowp vec2(0.67, -0.74);
	angelPosition[49] = lowp vec2(0.63, -0.77);
	angelPosition[50] = lowp vec2(0.60, -0.80);
	angelPosition[51] = lowp vec2(0.56, -0.83);
	angelPosition[52] = lowp vec2(0.51, -0.86);
	angelPosition[53] = lowp vec2(0.47, -0.88);
	angelPosition[54] = lowp vec2(0.43, -0.90);
	angelPosition[55] = lowp vec2(0.38, -0.92);
	angelPosition[56] = lowp vec2(0.34, -0.94);
	angelPosition[57] = lowp vec2(0.29, -0.96);
	angelPosition[58] = lowp vec2(0.24, -0.97);
	angelPosition[59] = lowp vec2(0.20, -0.98);
	angelPosition[60] = lowp vec2(0.15, -0.99);
	angelPosition[61] = lowp vec2(0.10, -1.00);
	angelPosition[62] = lowp vec2(0.05, -1.00);
	angelPosition[63] = lowp vec2(-0.00, -1.00);
	angelPosition[64] = lowp vec2(-0.05, -1.00);
	angelPosition[65] = lowp vec2(-0.10, -1.00);
	angelPosition[66] = lowp vec2(-0.15, -0.99);
	angelPosition[67] = lowp vec2(-0.20, -0.98);
	angelPosition[68] = lowp vec2(-0.24, -0.97);
	angelPosition[69] = lowp vec2(-0.29, -0.96);
	angelPosition[70] = lowp vec2(-0.34, -0.94);
	angelPosition[71] = lowp vec2(-0.38, -0.92);
	angelPosition[72] = lowp vec2(-0.43, -0.90);
	angelPosition[73] = lowp vec2(-0.47, -0.88);
	angelPosition[74] = lowp vec2(-0.51, -0.86);
	angelPosition[75] = lowp vec2(-0.56, -0.83);
	angelPosition[76] = lowp vec2(-0.60, -0.80);
	angelPosition[77] = lowp vec2(-0.63, -0.77);
	angelPosition[78] = lowp vec2(-0.67, -0.74);
	angelPosition[79] = lowp vec2(-0.71, -0.71);
	angelPosition[80] = lowp vec2(-0.74, -0.67);
	angelPosition[81] = lowp vec2(-0.77, -0.63);
	angelPosition[82] = lowp vec2(-0.80, -0.60);
	angelPosition[83] = lowp vec2(-0.83, -0.56);
	angelPosition[84] = lowp vec2(-0.86, -0.51);
	angelPosition[85] = lowp vec2(-0.88, -0.47);
	angelPosition[86] = lowp vec2(-0.90, -0.43);
	angelPosition[87] = lowp vec2(-0.92, -0.38);
	angelPosition[88] = lowp vec2(-0.94, -0.34);
	angelPosition[89] = lowp vec2(-0.96, -0.29);
	angelPosition[90] = lowp vec2(-0.97, -0.24);
	angelPosition[91] = lowp vec2(-0.98, -0.20);
	angelPosition[92] = lowp vec2(-0.99, -0.15);
	angelPosition[93] = lowp vec2(-1.00, -0.10);
	angelPosition[94] = lowp vec2(-1.00, -0.05);
	angelPosition[95] = lowp vec2(-1.00, 0.00);
	angelPosition[96] = lowp vec2(-1.00, 0.05);
	angelPosition[97] = lowp vec2(-1.00, 0.10);
	angelPosition[98] = lowp vec2(-0.99, 0.15);
	angelPosition[99] = lowp vec2(-0.98, 0.20);
	angelPosition[100] = lowp vec2(-0.97, 0.24);
	angelPosition[101] = lowp vec2(-0.96, 0.29);
	angelPosition[102] = lowp vec2(-0.94, 0.34);
	angelPosition[103] = lowp vec2(-0.92, 0.38);
	angelPosition[104] = lowp vec2(-0.90, 0.43);
	angelPosition[105] = lowp vec2(-0.88, 0.47);
	angelPosition[106] = lowp vec2(-0.86, 0.51);
	angelPosition[107] = lowp vec2(-0.83, 0.56);
	angelPosition[108] = lowp vec2(-0.80, 0.60);
	angelPosition[109] = lowp vec2(-0.77, 0.63);
	angelPosition[110] = lowp vec2(-0.74, 0.67);
	angelPosition[111] = lowp vec2(-0.71, 0.71);
	angelPosition[112] = lowp vec2(-0.67, 0.74);
	angelPosition[113] = lowp vec2(-0.63, 0.77);
	angelPosition[114] = lowp vec2(-0.60, 0.80);
	angelPosition[115] = lowp vec2(-0.56, 0.83);
	angelPosition[116] = lowp vec2(-0.51, 0.86);
	angelPosition[117] = lowp vec2(-0.47, 0.88);
	angelPosition[118] = lowp vec2(-0.43, 0.90);
	angelPosition[119] = lowp vec2(-0.38, 0.92);
	angelPosition[120] = lowp vec2(-0.34, 0.94);
	angelPosition[121] = lowp vec2(-0.29, 0.96);
	angelPosition[122] = lowp vec2(-0.24, 0.97);
	angelPosition[123] = lowp vec2(-0.20, 0.98);
	angelPosition[124] = lowp vec2(-0.15, 0.99);
	angelPosition[125] = lowp vec2(-0.10, 1.00);
	angelPosition[126] = lowp vec2(-0.05, 1.00);
	angelPosition[127] = lowp vec2(0.00, 1.00);

	mediump vec4 originColur = texture2D(m_Sampler, v_TexCoord);
	lowp vec4 outlineColour = outline(m_Sampler, g_Radius, v_TexCoord, g_TexSize, g_OutlineColour);

	gl_FragColor = mix(outlineColour, originColur, originColur.a);
}