Shader "Custom/HyperbolaShader" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
       	_I ("Intensity [0,1]", Range(0,1)) = 0
       	_R ("Radius [0,1]", Range(0,1)) = 1
       	_Cx ("CenterX", Range(0,1)) = 0.5
       	_Cy ("CenterY", Range(0,1)) = 0.5
    }
    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            uniform sampler2D _MainTex;
            uniform fixed _I; // Intensity
            uniform fixed _Cx; // CenterX
            uniform fixed _Cy; // CenterY
            uniform fixed _R; // Radius of circle
            fixed2 _P; // Current fragment (point)
            fixed2 _C; // Center of texture
            fixed _D; // Distance between fragment and center

            float4 frag(v2f_img i) : COLOR {
            	_P = i.uv;
	        	_C = fixed2(_Cx,_Cy);
				_D = distance(_P, _C);
				_R *= 0.2;
				_I *= 9.8;
				
				if (_D > _R)
				{
					return tex2D(_MainTex, i.uv);
				}
				else
				{
	            	return tex2D(_MainTex, _P + pow(_R - _D,2) * (_P - _C) / (_D * (10-_I)) ); // p + (r-d)^2 * (p-c)/(d(10-i))
        		}
            }
            ENDCG
        }
    }
}