// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Spelonko/IceCrystal"
{
	Properties
	{
		_MainTex("Albdeo", 2D) = "white" {}
		_OffsetTex("Refraction Map", 2D) = "white" {}
		_AmbientTex("Ambient Reflections", 2D) = "white" {}
		_RefractionMag("Refraction Magnitude", Float) = 0.3
		_Visibility("Visibility", Float) = 0.02
		_Local("Is local (1 if true)", Int) = 0



	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"PreviewType" = "Plane"
			"DisableBatching" = "True"
		}

		Pass
		{
			ZWrite Off
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				half4 color : COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 screenuv : TEXCOORD1;
				half4 color : COLOR;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.screenuv = ((o.vertex.xy / o.vertex.w) + 1) * 0.5;
				o.color = v.color;
				return o;
			}

			float2 safemul(float4x4 M, float4 v)
			{
				float2 r;

				r.x = dot(M._m00_m01_m02, v);
				r.y = dot(M._m10_m11_m12, v);

				return r;
			}

			sampler2D _MainTex;
			sampler2D _OffsetTex;
			sampler2D _AmbientTex;
			float _RefractionMag;
			float _Visibility;
			int _Local;

			uniform sampler2D _GlobalRefractionTexGlobal;
			uniform sampler2D _GlobalRefractionTexLocal;

			float4 frag(v2f i) : SV_Target
			{

				i.screenuv.y = 1 - i.screenuv.y;
				float4 color = tex2D(_MainTex, i.uv) * i.color;
				float2 offset = safemul(unity_ObjectToWorld, tex2D(_OffsetTex, i.uv) * 2 - 1);
				float4 worldRefl;
				if (_Local == 1) {
					float4 ambient = tex2D(_AmbientTex, (i.screenuv + offset * _RefractionMag * 5) * 2);
					worldRefl = tex2D(_GlobalRefractionTexLocal, i.screenuv + offset.xy * _RefractionMag);
					color.rgb = (color.rgb + ambient.rgb) * (1.0 - worldRefl.a * _Visibility)
						+ worldRefl.rgb * worldRefl.a * _Visibility;

					return color;
				
				}
				else {
					worldRefl = tex2D(_GlobalRefractionTexGlobal, i.screenuv + offset.xy * _RefractionMag);
					color.rgb = (color.rgb) * (1.0 - worldRefl.a * _Visibility)
						+ worldRefl.rgb * worldRefl.a * _Visibility;


					return color;
				}
				
			}
			ENDCG
		}
	}

	Fallback "Sprites/Default"
}