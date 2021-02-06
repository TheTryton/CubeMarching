Shader "Terrain/VoxelTerrain3D"
{
    Properties
    {
        _Tex1("Texture 1", 2D) = "white" {}
        _Tex1Bump("Texture 1 Bump Map", 2D) = "white" {}
        _Tex2("Texture 2", 2D) = "white" {}
        _Tex2Bump("Texture 2 Bump Map", 2D) = "white" {}

        _Bright("Brightness", Float) = 3.0
        _Sharpness("Sharpness", Float) = 1.0

        _LO("LO", Float) = 0.45
        _HI("HI", Float) = 0.55
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        CGPROGRAM
        #pragma surface surf Lambert 

        struct Input
        {
            float3 worldPos;
            float3 worldNormal;// INTERNAL_DATA

            float2 uv_Tex1;
        };

        sampler2D _Tex1;
        sampler2D _Tex1Bump;
        sampler2D _Tex2;
        sampler2D _Tex2Bump;
        float _Bright;
        float _Sharpness;
        float _LO;
        float _HI;

        float3 GetBlendCoeff(Input i)
        {
            //float3 blendCoeff = pow(abs(WorldNormalVector(i, float3(0, 0, 1))), _Sharpness);
            float3 blendCoeff = pow(abs(i.worldNormal), _Sharpness);
            blendCoeff = blendCoeff / (blendCoeff.x + blendCoeff.y + blendCoeff.z);
            return blendCoeff;
        }

        float4 GetTriplanarMappedT1Albedo(Input i)
        {
            float3 localPos = i.worldPos - mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;

            float4 col1 = tex2D(_Tex1, i.worldPos.xy);
            float4 col2 = tex2D(_Tex1, i.worldPos.yz);
            float4 col3 = tex2D(_Tex1, i.worldPos.zx);

            float coeff = smoothstep(_LO, _HI, i.uv_Tex1.x);

            float3 blendCoeff = GetBlendCoeff(i);

            return (col1 * _Bright * blendCoeff.z + col2 * _Bright * blendCoeff.x + col3 * _Bright * blendCoeff.y) / 3.0 * coeff;
        }

        float4 GetTriplanarMappedT2Albedo(Input i)
        {
            float4 col1 = tex2D(_Tex2, i.worldPos.xy);
            float4 col2 = tex2D(_Tex2, i.worldPos.yz);
            float4 col3 = tex2D(_Tex2, i.worldPos.zx);

            float coeff = smoothstep(_LO, _HI, 1.0 - i.uv_Tex1.x);

            float3 blendCoeff = GetBlendCoeff(i);

            return (col1 * _Bright * blendCoeff.z + col2 * _Bright * blendCoeff.x + col3 * _Bright * blendCoeff.y) / 3.0 * coeff;
        }

        /*half3 GetTriplanarMappedT1Normal(Input i)
        {
            float4 c1 = tex2D(_Tex1Bump, i.worldPos.xy);
            float4 c2 = tex2D(_Tex1Bump, i.worldPos.yz);
            float4 c3 = tex2D(_Tex1Bump, i.worldPos.zx);

            float coeff = smoothstep(_LO, _HI, i.uv_Tex1.x);

            float3 blendCoeff = GetBlendCoeff(i);

            return UnpackNormal((c1 * blendCoeff.z + c2 * blendCoeff.x + c3 * blendCoeff.y) / 3.0 * coeff);
        }

        half3 GetTriplanarMappedT2Normal(Input i)
        {
            float4 c1 = tex2D(_Tex2Bump, i.worldPos.xy);
            float4 c2 = tex2D(_Tex2Bump, i.worldPos.yz);
            float4 c3 = tex2D(_Tex2Bump, i.worldPos.zx);

            float3 blendCoeff = GetBlendCoeff(i);

            float coeff = smoothstep(_LO, _HI, 1.0 - i.uv_Tex1.x);

            return UnpackNormal((c1 * blendCoeff.z + c2 * blendCoeff.x + c3 * blendCoeff.y) / 3.0 * coeff);
        }*/

        void surf(Input i, inout SurfaceOutput o)
        {
            o.Albedo = (GetTriplanarMappedT1Albedo(i) + GetTriplanarMappedT2Albedo(i)) / 2.0;
            //o.Normal = (GetTriplanarMappedT1Normal(i) + GetTriplanarMappedT2Normal(i)) / 2.0;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
