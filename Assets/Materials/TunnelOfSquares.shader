Shader "Custom/SpawnedGrowingOutlinesInfiniteGrowOffscreenAlpha"
{
    Properties
    {
        _Thickness("Outline Thickness", Range(0.01, 0.2)) = 0.06
        _Blur("Blur Amount", Range(0.001, 0.1)) = 0.02
        _SpawnPeriod("Time Between Spawns", Range(0.1, 2)) = 0.35
        _GrowSpeed("Scale speed", Range(0.2, 2)) = 0.55
        _MaxActiveSquares("Max Simultaneous On Screen", Int) = 8
        _Alpha("Squares Alpha", Range(0,1)) = 0.7
    }
    SubShader
    {
        // Important: Transparent/Alpha blending
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float _Thickness;
            float _Blur;
            float _SpawnPeriod;
            float _GrowSpeed;
            int _MaxActiveSquares;
            float _Alpha;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 centeredUV = i.uv * 2.0 - 1.0;
                float outlineSum = 0;

                float now = _Time.y;
                float growTo = sqrt(2.0) * 2.0; // fade range
                float initialScale = 0.01;
                float maxTimeOnScreen = (growTo - initialScale) / _GrowSpeed;
                int numToCheck = int(ceil(maxTimeOnScreen / _SpawnPeriod)) + 2;
                numToCheck = min(numToCheck, _MaxActiveSquares);

                float baseIndex = floor(now / _SpawnPeriod);

                for (int k=0; k<numToCheck; ++k)
                {
                    float j = baseIndex - k;
                    float spawnT = j * _SpawnPeriod;
                    float age = now - spawnT;
                    if (age < 0) continue;

                    // Growing: scale = initial + speed * age
                    float scale = initialScale + age * _GrowSpeed;

                    // Out fade: after square exceeds boundary
                    float fadeOut = smoothstep(growTo, growTo - 0.2, scale);

                    // Don't bother if faded completely and large
                    if (fadeOut <= 0 && scale > growTo) continue;

                    // "Random" rotation per pulse (change to arithmetic if wanted)
                    float rotSeed = frac(sin(j + 108.123)*43758.5453);
                    float rotSpeed = lerp(-1.0, 1.0, rotSeed);  // -1 to 1 rad/sec
                    float angle = rotSpeed * age * 2;

                    float sA = sin(angle);
                    float cA = cos(angle);
                    float2 rotatedUV = float2(
                        centeredUV.x * cA - centeredUV.y * sA,
                        centeredUV.x * sA + centeredUV.y * cA
                    ) / scale;

                    float dist = max(abs(rotatedUV.x), abs(rotatedUV.y));
                    float outer = 0.5;
                    float inner = outer - _Thickness;

                    float squareOutline = smoothstep(inner - _Blur, inner + _Blur, dist)
                                       - smoothstep(outer - _Blur, outer + _Blur, dist);

                    // Fade in effect (at birth)
                    float prog = (scale - initialScale) / (growTo - initialScale);
                    float fadeIn = smoothstep(0.01, 0.12, prog);

                    float final = squareOutline * fadeIn * fadeOut;

                    outlineSum = max(outlineSum, final);
                }

                // Output: white with controllable alpha, black background
                return float4(1,1,1, outlineSum * _Alpha);
            }
            ENDCG
        }
    }
}