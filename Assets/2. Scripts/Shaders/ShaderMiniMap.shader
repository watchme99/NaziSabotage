Shader "Custom/ShaderMiniMap" 
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_Mask("Mask Texture", 2D) = "white" {}
	}
	SubShader
	{
		Lighting On
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			Tags{ "LightMode" = "Vertex" }
			Fog{ Mode Off }
			SetTexture[_Mask] {combine texture}
			SetTexture[_MainTex] {combine texture, previous}
		}
	}
}
