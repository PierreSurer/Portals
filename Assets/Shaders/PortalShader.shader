Shader "Unlit/PortalShader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _GradientMap("GradientMap", 2D) = "black" {}
        _FlowMap("FlowMap", 2D) = "white" {}
        _PortalEffect("Portal effect", Range(0, 10)) = 10.0
        _FlowSpeed("Flow speed", Range(0, 100)) = 1.0
        _Falloff("Falloff", Range(0, 100)) = 50.0

        _Border("Border", Range(0, 2)) = 0.0
        
    }
    SubShader
    {
        Tags{ "RenderType" = "Transparent" "Queue" = "Transparent"}
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            sampler2D _GradientMap;
            sampler2D _FlowMap;
            float _Falloff;
            float _PortalEffect;
            float _FlowSpeed;
            float _Border;

            fixed4 _Color;

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 img = tex2D(_MainTex, i.uv);

                float time1 = frac(_Time * _FlowSpeed);
                float time2 = frac(time1 + 0.5);
                float mixed = abs((time1 - 0.5) * 2.0);

                fixed2 flow = tex2D(_FlowMap, i.uv).xy;
                flow = (flow - 0.5f) * 2.0f;

                fixed4 border1 = (tex2D(_GradientMap, i.uv + (flow * time1)) / 2.0) + 0.75;
                fixed4 border2 = (tex2D(_GradientMap, i.uv + (flow * time2)) / 2.0) + 0.75;
                fixed4 finalBorder = (border1 * (1.0f - mixed) + border2 * mixed) * _Color;

                float x = i.uv.x - 0.5f;
                float y = i.uv.y - 0.5f;
                float dist = (x * x + y * y) * 4.0f;

                img.xyz = lerp(finalBorder, img.xyz, clamp(1 - pow(dist, _PortalEffect), 0.0f, 1.0f));

                
                dist = max(dist + _Border * ((border1 * (1.0f - mixed) + border2 * mixed) - 0.5f), 0.0f);
                
                img.a = clamp(1 - pow(dist, _Falloff), 0.0f, 1.0f);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, img);
                return img;
            }
            ENDCG
        }
    }
}
