// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/WaterBlur" {
    Properties
    {
	    _blurSizeXY("BlurSizeXY", Range(0,10)) = 0
    }
    SubShader 
    {
        // Draw ourselves after all opaque geometry
        Tags { "Queue" = "Transparent" }

        // Grab the screen behind the object into _GrabTexture
        GrabPass { }

        // Render the object with the texture generated above
        Pass 
        {
            CGPROGRAM
            #pragma debug
            #pragma vertex vert
            #pragma fragment frag 
            #ifndef SHADER_API_D3D11

            #pragma target 3.0

            #else

            #pragma target 4.0

            #endif

            sampler2D _GrabTexture : register(s0);
            float _blurSizeXY;

            struct data 
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 position : POSITION;
                float4 screenPos : TEXCOORD0;
            };

            v2f vert(data i)
            {
                v2f o;
                o.position = UnityObjectToClipPos(i.vertex);
                o.screenPos = o.position;
                return o;
            }

            half4 frag( v2f i ) : COLOR
            {
                float2 screenPos = i.screenPos.xy / i.screenPos.w;
	            float depth= _blurSizeXY*0.0005;

                screenPos.x = (screenPos.x + 1) * 0.5;

                screenPos.y = 1-(screenPos.y + 1) * 0.5;

                half4 sum = half4(0.0h,0.0h,0.0h,0.0h);  
                sampler2D grab = _GrabTexture;
    
                sum += tex2D(grab, screenPos - 3.0 * depth) * 0.1;
                sum += tex2D(grab, screenPos) * 0.64;
                sum += tex2D(grab, screenPos + 3.0 * depth) * 0.6;

	            return sum*0.5;

            }
ENDCG
        }
    }

Fallback Off
} 