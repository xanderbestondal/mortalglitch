Shader "Custom/standard_blood"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BloodTex ("blood (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_NormalStrength("Normal Strength", Range(0, 1)) = 0.9
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BloodTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BloodTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
		half _NormalStrength;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            //o.Albedo = c.rgb;
			
			float2 bloodUv = IN.uv_MainTex*float2(.5, 1); // half because I scaled plane double with... fix later with proper tiling ...

			fixed4 b = tex2D(_BloodTex, bloodUv);
			o.Albedo = lerp(c.rgb, fixed3(1, 0, 0), b.r);

            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic * b.r;
            o.Smoothness = _Glossiness * b.r* b.r;


			float3 duv = float3(.001,.001, 0);
			half v1 = tex2D(_BloodTex, bloodUv - duv.xz).x;
			half v2 = tex2D(_BloodTex, bloodUv + duv.xz).x;
			half v3 = tex2D(_BloodTex, bloodUv - duv.zy).x;
			half v4 = tex2D(_BloodTex, bloodUv + duv.zy).x;
			o.Normal = normalize(float3(v1 - v2, v3 - v4, 1 - _NormalStrength));

            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
