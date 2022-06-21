Shader "Unlit/Curved"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Curvature("Curvature", Range(0,.02)) = 0.001
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

       CGPROGRAM
       #pragma surface surf Lambert vertex:vert addshadow
       uniform sampler2D _MainTex;
       uniform float _Curvature;
       fixed4 _Color;

        struct Input
        {
            float2 uv_MainTex;
        };

        void vert(inout appdata_full v)
        {
            float4 worldSpace = mul(unity_ObjectToWorld, v.vertex);
            worldSpace.xyz -= _WorldSpaceCameraPos.xyz;
            worldSpace = float4(0.0f, (worldSpace.z*worldSpace.z)* -_Curvature, 0.0f, 0.0f);

            v.vertex += mul(unity_WorldToObject, worldSpace);
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            //fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            fixed4 c = _Color;
            o.Albedo = c.rgb;
            //o.Albedo = c.a;
        }

       ENDCG
    }
    FallBack "Mobile/Diffuse"
}
