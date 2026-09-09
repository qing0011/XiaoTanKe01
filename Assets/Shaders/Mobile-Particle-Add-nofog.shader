// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)


Shader "Mobile/Particles/Additive nofog" {
Properties {
    _MainTex ("Particle Texture", 2D) = "white" {}
}

Category {
    Tags { "Queue"="Transparent" "RenderType"="Transparent" "PreviewType"="Plane" }
    Blend SrcAlpha One
    ColorMask RGB
    Cull Off Lighting Off ZWrite Off Fog{Mode Off}

    SubShader {
        Pass {
            SetTexture[_MainTex] {
                combine texture * primary
            }
        }
    }
}
}
