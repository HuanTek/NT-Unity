Shader "Hidden/SolidBody" {
	Properties {
		_Color ("Color", Color) = (0.5,1,1,1)
	}
	SubShader {
		Pass
		{
			Material
			{
				Diffuse[_Color]
			}
		}
	}
}
