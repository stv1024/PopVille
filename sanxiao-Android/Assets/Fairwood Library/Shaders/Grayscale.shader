// unlit, vertex colour, alpha blended
// cull off

Shader "Fairwood/Grayscale" 
{
	Properties 
	{
		_Grayscale ("Grayscale", Range(0,1)) = 1
	}

	SubShader
	{
		Tags {"IgnoreProjector"="True" "RenderType"="Opaque"}
		Blend Off Lighting Off Cull Off Fog { Mode Off }
		LOD 110
		
		Pass 
		{
			Color (0,0,1,0)
		} 
	}
 
	SubShader 
	{
		Tags {"IgnoreProjector"="True" "RenderType"="Opaque"}
		Blend Off Cull Off Fog { Mode Off }
		LOD 100
		
		BindChannels 
		{
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
			Bind "Color", color
		}

		Pass 
		{
			Lighting Off
			SetTexture [_MainTex] { combine texture * primary } 
		}
	}
}
