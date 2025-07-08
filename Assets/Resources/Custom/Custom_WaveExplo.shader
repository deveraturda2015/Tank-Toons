Shader "Custom/WaveExplo"
{
  Properties
  {
    _MainTex ("", 2D) = "white" {}
    _CenterX ("CenterX", Range(-1, 2)) = 0.5
    _CenterY ("CenterY", Range(-1, 2)) = 0.5
    _Radius ("Radius", Range(-1, 1)) = 0.2
    _Amplitude ("Amplitude", Range(-10, 10)) = 0.05
    _Aspect ("Aspect", Range(-10, 10)) = 1
  }
  SubShader
  {
    Tags
    { 
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
      }
      ZTest Always
      ZWrite Off
      Cull Off
      Fog
      { 
        Mode  Off
      } 
      GpuProgramID 64722
      // m_ProgramMask = 6
      !!! *******************************************************************************************
      !!! Allow restore shader as UnityLab format - only available for DevX GameRecovery license type
      !!! *******************************************************************************************
      Program "vp"
      {
        SubProgram "gles hw_tier00"
        {
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesVertex;
          attribute vec4 _glesMultiTexCoord0;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_MatrixVP;
          varying mediump vec2 xlv_TEXCOORD0;
          void main ()
          {
            mediump vec2 tmpvar_1;
            tmpvar_1 = _glesMultiTexCoord0.xy;
            mediump vec2 tmpvar_2;
            highp vec4 tmpvar_3;
            tmpvar_3.w = 1.0;
            tmpvar_3.xyz = _glesVertex.xyz;
            highp vec2 tmpvar_4;
            highp vec2 inUV_5;
            inUV_5 = tmpvar_1;
            highp vec4 tmpvar_6;
            tmpvar_6.zw = vec2(0.0, 0.0);
            tmpvar_6.xy = inUV_5;
            tmpvar_4 = (mat4(1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0) * tmpvar_6).xy;
            tmpvar_2 = tmpvar_4;
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_3));
            xlv_TEXCOORD0 = tmpvar_2;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp float _CenterX;
          uniform highp float _CenterY;
          uniform highp float _Radius;
          uniform highp float _Amplitude;
          uniform highp float _Aspect;
          uniform sampler2D _MainTex;
          varying mediump vec2 xlv_TEXCOORD0;
          void main ()
          {
            highp vec2 uv_displaced_1;
            highp vec2 tmpvar_2;
            tmpvar_2.x = ((xlv_TEXCOORD0.x - _CenterX) * _Aspect);
            tmpvar_2.y = (xlv_TEXCOORD0.y - _CenterY);
            highp float tmpvar_3;
            tmpvar_3 = sqrt(((tmpvar_2.x * tmpvar_2.x) + (tmpvar_2.y * tmpvar_2.y)));
            uv_displaced_1 = xlv_TEXCOORD0;
            if (((tmpvar_3 > _Radius) && (tmpvar_3 < (_Radius + 0.2)))) {
              uv_displaced_1 = (uv_displaced_1 - ((
                (vec2(((1.0 - cos(
                  ((6.283186 * (tmpvar_3 - _Radius)) / 0.2)
                )) * 0.5)) * tmpvar_2)
               * vec2(_Amplitude)) / vec2(tmpvar_3)));
            };
            lowp vec4 tmpvar_4;
            tmpvar_4 = texture2D (_MainTex, uv_displaced_1);
            gl_FragData[0] = tmpvar_4;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier01"
        {
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesVertex;
          attribute vec4 _glesMultiTexCoord0;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_MatrixVP;
          varying mediump vec2 xlv_TEXCOORD0;
          void main ()
          {
            mediump vec2 tmpvar_1;
            tmpvar_1 = _glesMultiTexCoord0.xy;
            mediump vec2 tmpvar_2;
            highp vec4 tmpvar_3;
            tmpvar_3.w = 1.0;
            tmpvar_3.xyz = _glesVertex.xyz;
            highp vec2 tmpvar_4;
            highp vec2 inUV_5;
            inUV_5 = tmpvar_1;
            highp vec4 tmpvar_6;
            tmpvar_6.zw = vec2(0.0, 0.0);
            tmpvar_6.xy = inUV_5;
            tmpvar_4 = (mat4(1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0) * tmpvar_6).xy;
            tmpvar_2 = tmpvar_4;
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_3));
            xlv_TEXCOORD0 = tmpvar_2;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp float _CenterX;
          uniform highp float _CenterY;
          uniform highp float _Radius;
          uniform highp float _Amplitude;
          uniform highp float _Aspect;
          uniform sampler2D _MainTex;
          varying mediump vec2 xlv_TEXCOORD0;
          void main ()
          {
            highp vec2 uv_displaced_1;
            highp vec2 tmpvar_2;
            tmpvar_2.x = ((xlv_TEXCOORD0.x - _CenterX) * _Aspect);
            tmpvar_2.y = (xlv_TEXCOORD0.y - _CenterY);
            highp float tmpvar_3;
            tmpvar_3 = sqrt(((tmpvar_2.x * tmpvar_2.x) + (tmpvar_2.y * tmpvar_2.y)));
            uv_displaced_1 = xlv_TEXCOORD0;
            if (((tmpvar_3 > _Radius) && (tmpvar_3 < (_Radius + 0.2)))) {
              uv_displaced_1 = (uv_displaced_1 - ((
                (vec2(((1.0 - cos(
                  ((6.283186 * (tmpvar_3 - _Radius)) / 0.2)
                )) * 0.5)) * tmpvar_2)
               * vec2(_Amplitude)) / vec2(tmpvar_3)));
            };
            lowp vec4 tmpvar_4;
            tmpvar_4 = texture2D (_MainTex, uv_displaced_1);
            gl_FragData[0] = tmpvar_4;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier02"
        {
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesVertex;
          attribute vec4 _glesMultiTexCoord0;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_MatrixVP;
          varying mediump vec2 xlv_TEXCOORD0;
          void main ()
          {
            mediump vec2 tmpvar_1;
            tmpvar_1 = _glesMultiTexCoord0.xy;
            mediump vec2 tmpvar_2;
            highp vec4 tmpvar_3;
            tmpvar_3.w = 1.0;
            tmpvar_3.xyz = _glesVertex.xyz;
            highp vec2 tmpvar_4;
            highp vec2 inUV_5;
            inUV_5 = tmpvar_1;
            highp vec4 tmpvar_6;
            tmpvar_6.zw = vec2(0.0, 0.0);
            tmpvar_6.xy = inUV_5;
            tmpvar_4 = (mat4(1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0) * tmpvar_6).xy;
            tmpvar_2 = tmpvar_4;
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_3));
            xlv_TEXCOORD0 = tmpvar_2;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp float _CenterX;
          uniform highp float _CenterY;
          uniform highp float _Radius;
          uniform highp float _Amplitude;
          uniform highp float _Aspect;
          uniform sampler2D _MainTex;
          varying mediump vec2 xlv_TEXCOORD0;
          void main ()
          {
            highp vec2 uv_displaced_1;
            highp vec2 tmpvar_2;
            tmpvar_2.x = ((xlv_TEXCOORD0.x - _CenterX) * _Aspect);
            tmpvar_2.y = (xlv_TEXCOORD0.y - _CenterY);
            highp float tmpvar_3;
            tmpvar_3 = sqrt(((tmpvar_2.x * tmpvar_2.x) + (tmpvar_2.y * tmpvar_2.y)));
            uv_displaced_1 = xlv_TEXCOORD0;
            if (((tmpvar_3 > _Radius) && (tmpvar_3 < (_Radius + 0.2)))) {
              uv_displaced_1 = (uv_displaced_1 - ((
                (vec2(((1.0 - cos(
                  ((6.283186 * (tmpvar_3 - _Radius)) / 0.2)
                )) * 0.5)) * tmpvar_2)
               * vec2(_Amplitude)) / vec2(tmpvar_3)));
            };
            lowp vec4 tmpvar_4;
            tmpvar_4 = texture2D (_MainTex, uv_displaced_1);
            gl_FragData[0] = tmpvar_4;
          }
          
          
          #endif
          
          "
        }
      }
      Program "fp"
      {
        SubProgram "gles hw_tier00"
        {
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier01"
        {
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier02"
        {
          
          "!!!!GLES
          
          
          "
        }
      }
      
    } // end phase
  }
  FallBack "Diffuse"
}
