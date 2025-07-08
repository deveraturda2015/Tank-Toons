Shader "TextMeshPro/Bitmap"
{
  Properties
  {
    _MainTex ("Font Atlas", 2D) = "white" {}
    _FaceTex ("Font Texture", 2D) = "white" {}
    _FaceColor ("Text Color", Color) = (1,1,1,1)
    _VertexOffsetX ("Vertex OffsetX", float) = 0
    _VertexOffsetY ("Vertex OffsetY", float) = 0
    _MaskSoftnessX ("Mask SoftnessX", float) = 0
    _MaskSoftnessY ("Mask SoftnessY", float) = 0
    _ClipRect ("Clip Rect", Vector) = (-32767,-32767,32767,32767)
    _StencilComp ("Stencil Comparison", float) = 8
    _Stencil ("Stencil ID", float) = 0
    _StencilOp ("Stencil Operation", float) = 0
    _StencilWriteMask ("Stencil Write Mask", float) = 255
    _StencilReadMask ("Stencil Read Mask", float) = 255
    _ColorMask ("Color Mask", float) = 15
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      ZWrite Off
      Cull Off
      Stencil
      { 
        Ref 0
        ReadMask 0
        WriteMask 0
        Pass Keep
        Fail Keep
        ZFail Keep
        PassFront Keep
        FailFront Keep
        ZFailFront Keep
        PassBack Keep
        FailBack Keep
        ZFailBack Keep
      } 
      Fog
      { 
        Mode  Off
      } 
      Blend SrcAlpha OneMinusSrcAlpha
      ColorMask 0
      GpuProgramID 62192
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
          attribute vec4 _glesColor;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec4 _ScreenParams;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp vec4 _FaceTex_ST;
          uniform lowp vec4 _FaceColor;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp vec4 _ClipRect;
          uniform highp float _MaskSoftnessX;
          uniform highp float _MaskSoftnessY;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          void main ()
          {
            highp vec4 vert_1;
            vert_1.zw = _glesVertex.zw;
            vert_1.x = (_glesVertex.x + _VertexOffsetX);
            vert_1.y = (_glesVertex.y + _VertexOffsetY);
            vert_1.xy = (vert_1.xy + ((_glesVertex.w * 0.5) / _ScreenParams.xy));
            highp vec4 tmpvar_2;
            highp vec4 tmpvar_3;
            tmpvar_3.w = 1.0;
            tmpvar_3.xyz = vert_1.xyz;
            tmpvar_2 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_3));
            highp vec4 pos_4;
            pos_4.zw = tmpvar_2.zw;
            highp vec2 tmpvar_5;
            tmpvar_5 = (_ScreenParams.xy * 0.5);
            pos_4.xy = ((floor(
              (((tmpvar_2.xy / tmpvar_2.w) * tmpvar_5) + vec2(0.5, 0.5))
            ) / tmpvar_5) * tmpvar_2.w);
            highp vec2 xlat_varoutput_6;
            xlat_varoutput_6.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_6.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_6.x));
            highp vec2 tmpvar_7;
            tmpvar_7.x = (_ScreenParams.x * glstate_matrix_projection[0].x);
            tmpvar_7.y = (_ScreenParams.y * glstate_matrix_projection[1].y);
            highp vec4 tmpvar_8;
            tmpvar_8 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_9;
            tmpvar_9.x = _MaskSoftnessX;
            tmpvar_9.y = _MaskSoftnessY;
            highp vec4 tmpvar_10;
            tmpvar_10.xy = (((vert_1.xy * 2.0) - tmpvar_8.xy) - tmpvar_8.zw);
            tmpvar_10.zw = (0.25 / ((0.25 * tmpvar_9) + (tmpvar_2.ww / 
              abs(tmpvar_7)
            )));
            gl_Position = pos_4;
            xlv_COLOR = (_glesColor * _FaceColor);
            xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
            xlv_TEXCOORD1 = (((xlat_varoutput_6 * 0.001953125) * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_10;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform sampler2D _MainTex;
          uniform sampler2D _FaceTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1.xyz = (texture2D (_FaceTex, xlv_TEXCOORD1).xyz * xlv_COLOR.xyz);
            tmpvar_1.w = (xlv_COLOR.w * texture2D (_MainTex, xlv_TEXCOORD0).w);
            gl_FragData[0] = tmpvar_1;
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
          attribute vec4 _glesColor;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec4 _ScreenParams;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp vec4 _FaceTex_ST;
          uniform lowp vec4 _FaceColor;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp vec4 _ClipRect;
          uniform highp float _MaskSoftnessX;
          uniform highp float _MaskSoftnessY;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          void main ()
          {
            highp vec4 vert_1;
            vert_1.zw = _glesVertex.zw;
            vert_1.x = (_glesVertex.x + _VertexOffsetX);
            vert_1.y = (_glesVertex.y + _VertexOffsetY);
            vert_1.xy = (vert_1.xy + ((_glesVertex.w * 0.5) / _ScreenParams.xy));
            highp vec4 tmpvar_2;
            highp vec4 tmpvar_3;
            tmpvar_3.w = 1.0;
            tmpvar_3.xyz = vert_1.xyz;
            tmpvar_2 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_3));
            highp vec4 pos_4;
            pos_4.zw = tmpvar_2.zw;
            highp vec2 tmpvar_5;
            tmpvar_5 = (_ScreenParams.xy * 0.5);
            pos_4.xy = ((floor(
              (((tmpvar_2.xy / tmpvar_2.w) * tmpvar_5) + vec2(0.5, 0.5))
            ) / tmpvar_5) * tmpvar_2.w);
            highp vec2 xlat_varoutput_6;
            xlat_varoutput_6.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_6.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_6.x));
            highp vec2 tmpvar_7;
            tmpvar_7.x = (_ScreenParams.x * glstate_matrix_projection[0].x);
            tmpvar_7.y = (_ScreenParams.y * glstate_matrix_projection[1].y);
            highp vec4 tmpvar_8;
            tmpvar_8 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_9;
            tmpvar_9.x = _MaskSoftnessX;
            tmpvar_9.y = _MaskSoftnessY;
            highp vec4 tmpvar_10;
            tmpvar_10.xy = (((vert_1.xy * 2.0) - tmpvar_8.xy) - tmpvar_8.zw);
            tmpvar_10.zw = (0.25 / ((0.25 * tmpvar_9) + (tmpvar_2.ww / 
              abs(tmpvar_7)
            )));
            gl_Position = pos_4;
            xlv_COLOR = (_glesColor * _FaceColor);
            xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
            xlv_TEXCOORD1 = (((xlat_varoutput_6 * 0.001953125) * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_10;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform sampler2D _MainTex;
          uniform sampler2D _FaceTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1.xyz = (texture2D (_FaceTex, xlv_TEXCOORD1).xyz * xlv_COLOR.xyz);
            tmpvar_1.w = (xlv_COLOR.w * texture2D (_MainTex, xlv_TEXCOORD0).w);
            gl_FragData[0] = tmpvar_1;
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
          attribute vec4 _glesColor;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec4 _ScreenParams;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp vec4 _FaceTex_ST;
          uniform lowp vec4 _FaceColor;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp vec4 _ClipRect;
          uniform highp float _MaskSoftnessX;
          uniform highp float _MaskSoftnessY;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          void main ()
          {
            highp vec4 vert_1;
            vert_1.zw = _glesVertex.zw;
            vert_1.x = (_glesVertex.x + _VertexOffsetX);
            vert_1.y = (_glesVertex.y + _VertexOffsetY);
            vert_1.xy = (vert_1.xy + ((_glesVertex.w * 0.5) / _ScreenParams.xy));
            highp vec4 tmpvar_2;
            highp vec4 tmpvar_3;
            tmpvar_3.w = 1.0;
            tmpvar_3.xyz = vert_1.xyz;
            tmpvar_2 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_3));
            highp vec4 pos_4;
            pos_4.zw = tmpvar_2.zw;
            highp vec2 tmpvar_5;
            tmpvar_5 = (_ScreenParams.xy * 0.5);
            pos_4.xy = ((floor(
              (((tmpvar_2.xy / tmpvar_2.w) * tmpvar_5) + vec2(0.5, 0.5))
            ) / tmpvar_5) * tmpvar_2.w);
            highp vec2 xlat_varoutput_6;
            xlat_varoutput_6.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_6.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_6.x));
            highp vec2 tmpvar_7;
            tmpvar_7.x = (_ScreenParams.x * glstate_matrix_projection[0].x);
            tmpvar_7.y = (_ScreenParams.y * glstate_matrix_projection[1].y);
            highp vec4 tmpvar_8;
            tmpvar_8 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_9;
            tmpvar_9.x = _MaskSoftnessX;
            tmpvar_9.y = _MaskSoftnessY;
            highp vec4 tmpvar_10;
            tmpvar_10.xy = (((vert_1.xy * 2.0) - tmpvar_8.xy) - tmpvar_8.zw);
            tmpvar_10.zw = (0.25 / ((0.25 * tmpvar_9) + (tmpvar_2.ww / 
              abs(tmpvar_7)
            )));
            gl_Position = pos_4;
            xlv_COLOR = (_glesColor * _FaceColor);
            xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
            xlv_TEXCOORD1 = (((xlat_varoutput_6 * 0.001953125) * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_10;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform sampler2D _MainTex;
          uniform sampler2D _FaceTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1.xyz = (texture2D (_FaceTex, xlv_TEXCOORD1).xyz * xlv_COLOR.xyz);
            tmpvar_1.w = (xlv_COLOR.w * texture2D (_MainTex, xlv_TEXCOORD0).w);
            gl_FragData[0] = tmpvar_1;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier00"
        {
           Keywords { "UNITY_UI_ALPHACLIP" }
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesVertex;
          attribute vec4 _glesColor;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec4 _ScreenParams;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp vec4 _FaceTex_ST;
          uniform lowp vec4 _FaceColor;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp vec4 _ClipRect;
          uniform highp float _MaskSoftnessX;
          uniform highp float _MaskSoftnessY;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          void main ()
          {
            highp vec4 vert_1;
            vert_1.zw = _glesVertex.zw;
            vert_1.x = (_glesVertex.x + _VertexOffsetX);
            vert_1.y = (_glesVertex.y + _VertexOffsetY);
            vert_1.xy = (vert_1.xy + ((_glesVertex.w * 0.5) / _ScreenParams.xy));
            highp vec4 tmpvar_2;
            highp vec4 tmpvar_3;
            tmpvar_3.w = 1.0;
            tmpvar_3.xyz = vert_1.xyz;
            tmpvar_2 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_3));
            highp vec4 pos_4;
            pos_4.zw = tmpvar_2.zw;
            highp vec2 tmpvar_5;
            tmpvar_5 = (_ScreenParams.xy * 0.5);
            pos_4.xy = ((floor(
              (((tmpvar_2.xy / tmpvar_2.w) * tmpvar_5) + vec2(0.5, 0.5))
            ) / tmpvar_5) * tmpvar_2.w);
            highp vec2 xlat_varoutput_6;
            xlat_varoutput_6.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_6.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_6.x));
            highp vec2 tmpvar_7;
            tmpvar_7.x = (_ScreenParams.x * glstate_matrix_projection[0].x);
            tmpvar_7.y = (_ScreenParams.y * glstate_matrix_projection[1].y);
            highp vec4 tmpvar_8;
            tmpvar_8 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_9;
            tmpvar_9.x = _MaskSoftnessX;
            tmpvar_9.y = _MaskSoftnessY;
            highp vec4 tmpvar_10;
            tmpvar_10.xy = (((vert_1.xy * 2.0) - tmpvar_8.xy) - tmpvar_8.zw);
            tmpvar_10.zw = (0.25 / ((0.25 * tmpvar_9) + (tmpvar_2.ww / 
              abs(tmpvar_7)
            )));
            gl_Position = pos_4;
            xlv_COLOR = (_glesColor * _FaceColor);
            xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
            xlv_TEXCOORD1 = (((xlat_varoutput_6 * 0.001953125) * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_10;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform sampler2D _MainTex;
          uniform sampler2D _FaceTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1.xyz = (texture2D (_FaceTex, xlv_TEXCOORD1).xyz * xlv_COLOR.xyz);
            tmpvar_1.w = (xlv_COLOR.w * texture2D (_MainTex, xlv_TEXCOORD0).w);
            lowp float x_2;
            x_2 = (tmpvar_1.w - 0.001);
            if ((x_2 < 0.0)) {
              discard;
            };
            gl_FragData[0] = tmpvar_1;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier01"
        {
           Keywords { "UNITY_UI_ALPHACLIP" }
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesVertex;
          attribute vec4 _glesColor;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec4 _ScreenParams;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp vec4 _FaceTex_ST;
          uniform lowp vec4 _FaceColor;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp vec4 _ClipRect;
          uniform highp float _MaskSoftnessX;
          uniform highp float _MaskSoftnessY;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          void main ()
          {
            highp vec4 vert_1;
            vert_1.zw = _glesVertex.zw;
            vert_1.x = (_glesVertex.x + _VertexOffsetX);
            vert_1.y = (_glesVertex.y + _VertexOffsetY);
            vert_1.xy = (vert_1.xy + ((_glesVertex.w * 0.5) / _ScreenParams.xy));
            highp vec4 tmpvar_2;
            highp vec4 tmpvar_3;
            tmpvar_3.w = 1.0;
            tmpvar_3.xyz = vert_1.xyz;
            tmpvar_2 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_3));
            highp vec4 pos_4;
            pos_4.zw = tmpvar_2.zw;
            highp vec2 tmpvar_5;
            tmpvar_5 = (_ScreenParams.xy * 0.5);
            pos_4.xy = ((floor(
              (((tmpvar_2.xy / tmpvar_2.w) * tmpvar_5) + vec2(0.5, 0.5))
            ) / tmpvar_5) * tmpvar_2.w);
            highp vec2 xlat_varoutput_6;
            xlat_varoutput_6.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_6.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_6.x));
            highp vec2 tmpvar_7;
            tmpvar_7.x = (_ScreenParams.x * glstate_matrix_projection[0].x);
            tmpvar_7.y = (_ScreenParams.y * glstate_matrix_projection[1].y);
            highp vec4 tmpvar_8;
            tmpvar_8 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_9;
            tmpvar_9.x = _MaskSoftnessX;
            tmpvar_9.y = _MaskSoftnessY;
            highp vec4 tmpvar_10;
            tmpvar_10.xy = (((vert_1.xy * 2.0) - tmpvar_8.xy) - tmpvar_8.zw);
            tmpvar_10.zw = (0.25 / ((0.25 * tmpvar_9) + (tmpvar_2.ww / 
              abs(tmpvar_7)
            )));
            gl_Position = pos_4;
            xlv_COLOR = (_glesColor * _FaceColor);
            xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
            xlv_TEXCOORD1 = (((xlat_varoutput_6 * 0.001953125) * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_10;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform sampler2D _MainTex;
          uniform sampler2D _FaceTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1.xyz = (texture2D (_FaceTex, xlv_TEXCOORD1).xyz * xlv_COLOR.xyz);
            tmpvar_1.w = (xlv_COLOR.w * texture2D (_MainTex, xlv_TEXCOORD0).w);
            lowp float x_2;
            x_2 = (tmpvar_1.w - 0.001);
            if ((x_2 < 0.0)) {
              discard;
            };
            gl_FragData[0] = tmpvar_1;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier02"
        {
           Keywords { "UNITY_UI_ALPHACLIP" }
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesVertex;
          attribute vec4 _glesColor;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec4 _ScreenParams;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp vec4 _FaceTex_ST;
          uniform lowp vec4 _FaceColor;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp vec4 _ClipRect;
          uniform highp float _MaskSoftnessX;
          uniform highp float _MaskSoftnessY;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          void main ()
          {
            highp vec4 vert_1;
            vert_1.zw = _glesVertex.zw;
            vert_1.x = (_glesVertex.x + _VertexOffsetX);
            vert_1.y = (_glesVertex.y + _VertexOffsetY);
            vert_1.xy = (vert_1.xy + ((_glesVertex.w * 0.5) / _ScreenParams.xy));
            highp vec4 tmpvar_2;
            highp vec4 tmpvar_3;
            tmpvar_3.w = 1.0;
            tmpvar_3.xyz = vert_1.xyz;
            tmpvar_2 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_3));
            highp vec4 pos_4;
            pos_4.zw = tmpvar_2.zw;
            highp vec2 tmpvar_5;
            tmpvar_5 = (_ScreenParams.xy * 0.5);
            pos_4.xy = ((floor(
              (((tmpvar_2.xy / tmpvar_2.w) * tmpvar_5) + vec2(0.5, 0.5))
            ) / tmpvar_5) * tmpvar_2.w);
            highp vec2 xlat_varoutput_6;
            xlat_varoutput_6.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_6.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_6.x));
            highp vec2 tmpvar_7;
            tmpvar_7.x = (_ScreenParams.x * glstate_matrix_projection[0].x);
            tmpvar_7.y = (_ScreenParams.y * glstate_matrix_projection[1].y);
            highp vec4 tmpvar_8;
            tmpvar_8 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_9;
            tmpvar_9.x = _MaskSoftnessX;
            tmpvar_9.y = _MaskSoftnessY;
            highp vec4 tmpvar_10;
            tmpvar_10.xy = (((vert_1.xy * 2.0) - tmpvar_8.xy) - tmpvar_8.zw);
            tmpvar_10.zw = (0.25 / ((0.25 * tmpvar_9) + (tmpvar_2.ww / 
              abs(tmpvar_7)
            )));
            gl_Position = pos_4;
            xlv_COLOR = (_glesColor * _FaceColor);
            xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
            xlv_TEXCOORD1 = (((xlat_varoutput_6 * 0.001953125) * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_10;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform sampler2D _MainTex;
          uniform sampler2D _FaceTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1.xyz = (texture2D (_FaceTex, xlv_TEXCOORD1).xyz * xlv_COLOR.xyz);
            tmpvar_1.w = (xlv_COLOR.w * texture2D (_MainTex, xlv_TEXCOORD0).w);
            lowp float x_2;
            x_2 = (tmpvar_1.w - 0.001);
            if ((x_2 < 0.0)) {
              discard;
            };
            gl_FragData[0] = tmpvar_1;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier00"
        {
           Keywords { "UNITY_UI_CLIP_RECT" }
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesVertex;
          attribute vec4 _glesColor;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec4 _ScreenParams;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp vec4 _FaceTex_ST;
          uniform lowp vec4 _FaceColor;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp vec4 _ClipRect;
          uniform highp float _MaskSoftnessX;
          uniform highp float _MaskSoftnessY;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          void main ()
          {
            highp vec4 vert_1;
            vert_1.zw = _glesVertex.zw;
            vert_1.x = (_glesVertex.x + _VertexOffsetX);
            vert_1.y = (_glesVertex.y + _VertexOffsetY);
            vert_1.xy = (vert_1.xy + ((_glesVertex.w * 0.5) / _ScreenParams.xy));
            highp vec4 tmpvar_2;
            highp vec4 tmpvar_3;
            tmpvar_3.w = 1.0;
            tmpvar_3.xyz = vert_1.xyz;
            tmpvar_2 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_3));
            highp vec4 pos_4;
            pos_4.zw = tmpvar_2.zw;
            highp vec2 tmpvar_5;
            tmpvar_5 = (_ScreenParams.xy * 0.5);
            pos_4.xy = ((floor(
              (((tmpvar_2.xy / tmpvar_2.w) * tmpvar_5) + vec2(0.5, 0.5))
            ) / tmpvar_5) * tmpvar_2.w);
            highp vec2 xlat_varoutput_6;
            xlat_varoutput_6.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_6.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_6.x));
            highp vec2 tmpvar_7;
            tmpvar_7.x = (_ScreenParams.x * glstate_matrix_projection[0].x);
            tmpvar_7.y = (_ScreenParams.y * glstate_matrix_projection[1].y);
            highp vec4 tmpvar_8;
            tmpvar_8 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_9;
            tmpvar_9.x = _MaskSoftnessX;
            tmpvar_9.y = _MaskSoftnessY;
            highp vec4 tmpvar_10;
            tmpvar_10.xy = (((vert_1.xy * 2.0) - tmpvar_8.xy) - tmpvar_8.zw);
            tmpvar_10.zw = (0.25 / ((0.25 * tmpvar_9) + (tmpvar_2.ww / 
              abs(tmpvar_7)
            )));
            gl_Position = pos_4;
            xlv_COLOR = (_glesColor * _FaceColor);
            xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
            xlv_TEXCOORD1 = (((xlat_varoutput_6 * 0.001953125) * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_10;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform sampler2D _MainTex;
          uniform sampler2D _FaceTex;
          uniform highp vec4 _ClipRect;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 color_1;
            lowp vec4 tmpvar_2;
            tmpvar_2.xyz = (texture2D (_FaceTex, xlv_TEXCOORD1).xyz * xlv_COLOR.xyz);
            tmpvar_2.w = (xlv_COLOR.w * texture2D (_MainTex, xlv_TEXCOORD0).w);
            mediump vec2 tmpvar_3;
            highp vec2 tmpvar_4;
            tmpvar_4 = clamp (((
              (_ClipRect.zw - _ClipRect.xy)
             - 
              abs(xlv_TEXCOORD2.xy)
            ) * xlv_TEXCOORD2.zw), 0.0, 1.0);
            tmpvar_3 = tmpvar_4;
            color_1 = (tmpvar_2 * (tmpvar_3.x * tmpvar_3.y));
            gl_FragData[0] = color_1;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier01"
        {
           Keywords { "UNITY_UI_CLIP_RECT" }
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesVertex;
          attribute vec4 _glesColor;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec4 _ScreenParams;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp vec4 _FaceTex_ST;
          uniform lowp vec4 _FaceColor;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp vec4 _ClipRect;
          uniform highp float _MaskSoftnessX;
          uniform highp float _MaskSoftnessY;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          void main ()
          {
            highp vec4 vert_1;
            vert_1.zw = _glesVertex.zw;
            vert_1.x = (_glesVertex.x + _VertexOffsetX);
            vert_1.y = (_glesVertex.y + _VertexOffsetY);
            vert_1.xy = (vert_1.xy + ((_glesVertex.w * 0.5) / _ScreenParams.xy));
            highp vec4 tmpvar_2;
            highp vec4 tmpvar_3;
            tmpvar_3.w = 1.0;
            tmpvar_3.xyz = vert_1.xyz;
            tmpvar_2 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_3));
            highp vec4 pos_4;
            pos_4.zw = tmpvar_2.zw;
            highp vec2 tmpvar_5;
            tmpvar_5 = (_ScreenParams.xy * 0.5);
            pos_4.xy = ((floor(
              (((tmpvar_2.xy / tmpvar_2.w) * tmpvar_5) + vec2(0.5, 0.5))
            ) / tmpvar_5) * tmpvar_2.w);
            highp vec2 xlat_varoutput_6;
            xlat_varoutput_6.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_6.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_6.x));
            highp vec2 tmpvar_7;
            tmpvar_7.x = (_ScreenParams.x * glstate_matrix_projection[0].x);
            tmpvar_7.y = (_ScreenParams.y * glstate_matrix_projection[1].y);
            highp vec4 tmpvar_8;
            tmpvar_8 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_9;
            tmpvar_9.x = _MaskSoftnessX;
            tmpvar_9.y = _MaskSoftnessY;
            highp vec4 tmpvar_10;
            tmpvar_10.xy = (((vert_1.xy * 2.0) - tmpvar_8.xy) - tmpvar_8.zw);
            tmpvar_10.zw = (0.25 / ((0.25 * tmpvar_9) + (tmpvar_2.ww / 
              abs(tmpvar_7)
            )));
            gl_Position = pos_4;
            xlv_COLOR = (_glesColor * _FaceColor);
            xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
            xlv_TEXCOORD1 = (((xlat_varoutput_6 * 0.001953125) * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_10;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform sampler2D _MainTex;
          uniform sampler2D _FaceTex;
          uniform highp vec4 _ClipRect;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 color_1;
            lowp vec4 tmpvar_2;
            tmpvar_2.xyz = (texture2D (_FaceTex, xlv_TEXCOORD1).xyz * xlv_COLOR.xyz);
            tmpvar_2.w = (xlv_COLOR.w * texture2D (_MainTex, xlv_TEXCOORD0).w);
            mediump vec2 tmpvar_3;
            highp vec2 tmpvar_4;
            tmpvar_4 = clamp (((
              (_ClipRect.zw - _ClipRect.xy)
             - 
              abs(xlv_TEXCOORD2.xy)
            ) * xlv_TEXCOORD2.zw), 0.0, 1.0);
            tmpvar_3 = tmpvar_4;
            color_1 = (tmpvar_2 * (tmpvar_3.x * tmpvar_3.y));
            gl_FragData[0] = color_1;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier02"
        {
           Keywords { "UNITY_UI_CLIP_RECT" }
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesVertex;
          attribute vec4 _glesColor;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec4 _ScreenParams;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp vec4 _FaceTex_ST;
          uniform lowp vec4 _FaceColor;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp vec4 _ClipRect;
          uniform highp float _MaskSoftnessX;
          uniform highp float _MaskSoftnessY;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          void main ()
          {
            highp vec4 vert_1;
            vert_1.zw = _glesVertex.zw;
            vert_1.x = (_glesVertex.x + _VertexOffsetX);
            vert_1.y = (_glesVertex.y + _VertexOffsetY);
            vert_1.xy = (vert_1.xy + ((_glesVertex.w * 0.5) / _ScreenParams.xy));
            highp vec4 tmpvar_2;
            highp vec4 tmpvar_3;
            tmpvar_3.w = 1.0;
            tmpvar_3.xyz = vert_1.xyz;
            tmpvar_2 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_3));
            highp vec4 pos_4;
            pos_4.zw = tmpvar_2.zw;
            highp vec2 tmpvar_5;
            tmpvar_5 = (_ScreenParams.xy * 0.5);
            pos_4.xy = ((floor(
              (((tmpvar_2.xy / tmpvar_2.w) * tmpvar_5) + vec2(0.5, 0.5))
            ) / tmpvar_5) * tmpvar_2.w);
            highp vec2 xlat_varoutput_6;
            xlat_varoutput_6.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_6.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_6.x));
            highp vec2 tmpvar_7;
            tmpvar_7.x = (_ScreenParams.x * glstate_matrix_projection[0].x);
            tmpvar_7.y = (_ScreenParams.y * glstate_matrix_projection[1].y);
            highp vec4 tmpvar_8;
            tmpvar_8 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_9;
            tmpvar_9.x = _MaskSoftnessX;
            tmpvar_9.y = _MaskSoftnessY;
            highp vec4 tmpvar_10;
            tmpvar_10.xy = (((vert_1.xy * 2.0) - tmpvar_8.xy) - tmpvar_8.zw);
            tmpvar_10.zw = (0.25 / ((0.25 * tmpvar_9) + (tmpvar_2.ww / 
              abs(tmpvar_7)
            )));
            gl_Position = pos_4;
            xlv_COLOR = (_glesColor * _FaceColor);
            xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
            xlv_TEXCOORD1 = (((xlat_varoutput_6 * 0.001953125) * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_10;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform sampler2D _MainTex;
          uniform sampler2D _FaceTex;
          uniform highp vec4 _ClipRect;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 color_1;
            lowp vec4 tmpvar_2;
            tmpvar_2.xyz = (texture2D (_FaceTex, xlv_TEXCOORD1).xyz * xlv_COLOR.xyz);
            tmpvar_2.w = (xlv_COLOR.w * texture2D (_MainTex, xlv_TEXCOORD0).w);
            mediump vec2 tmpvar_3;
            highp vec2 tmpvar_4;
            tmpvar_4 = clamp (((
              (_ClipRect.zw - _ClipRect.xy)
             - 
              abs(xlv_TEXCOORD2.xy)
            ) * xlv_TEXCOORD2.zw), 0.0, 1.0);
            tmpvar_3 = tmpvar_4;
            color_1 = (tmpvar_2 * (tmpvar_3.x * tmpvar_3.y));
            gl_FragData[0] = color_1;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier00"
        {
           Keywords { "UNITY_UI_CLIP_RECT" "UNITY_UI_ALPHACLIP" }
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesVertex;
          attribute vec4 _glesColor;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec4 _ScreenParams;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp vec4 _FaceTex_ST;
          uniform lowp vec4 _FaceColor;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp vec4 _ClipRect;
          uniform highp float _MaskSoftnessX;
          uniform highp float _MaskSoftnessY;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          void main ()
          {
            highp vec4 vert_1;
            vert_1.zw = _glesVertex.zw;
            vert_1.x = (_glesVertex.x + _VertexOffsetX);
            vert_1.y = (_glesVertex.y + _VertexOffsetY);
            vert_1.xy = (vert_1.xy + ((_glesVertex.w * 0.5) / _ScreenParams.xy));
            highp vec4 tmpvar_2;
            highp vec4 tmpvar_3;
            tmpvar_3.w = 1.0;
            tmpvar_3.xyz = vert_1.xyz;
            tmpvar_2 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_3));
            highp vec4 pos_4;
            pos_4.zw = tmpvar_2.zw;
            highp vec2 tmpvar_5;
            tmpvar_5 = (_ScreenParams.xy * 0.5);
            pos_4.xy = ((floor(
              (((tmpvar_2.xy / tmpvar_2.w) * tmpvar_5) + vec2(0.5, 0.5))
            ) / tmpvar_5) * tmpvar_2.w);
            highp vec2 xlat_varoutput_6;
            xlat_varoutput_6.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_6.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_6.x));
            highp vec2 tmpvar_7;
            tmpvar_7.x = (_ScreenParams.x * glstate_matrix_projection[0].x);
            tmpvar_7.y = (_ScreenParams.y * glstate_matrix_projection[1].y);
            highp vec4 tmpvar_8;
            tmpvar_8 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_9;
            tmpvar_9.x = _MaskSoftnessX;
            tmpvar_9.y = _MaskSoftnessY;
            highp vec4 tmpvar_10;
            tmpvar_10.xy = (((vert_1.xy * 2.0) - tmpvar_8.xy) - tmpvar_8.zw);
            tmpvar_10.zw = (0.25 / ((0.25 * tmpvar_9) + (tmpvar_2.ww / 
              abs(tmpvar_7)
            )));
            gl_Position = pos_4;
            xlv_COLOR = (_glesColor * _FaceColor);
            xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
            xlv_TEXCOORD1 = (((xlat_varoutput_6 * 0.001953125) * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_10;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform sampler2D _MainTex;
          uniform sampler2D _FaceTex;
          uniform highp vec4 _ClipRect;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 color_1;
            lowp vec4 tmpvar_2;
            tmpvar_2.xyz = (texture2D (_FaceTex, xlv_TEXCOORD1).xyz * xlv_COLOR.xyz);
            tmpvar_2.w = (xlv_COLOR.w * texture2D (_MainTex, xlv_TEXCOORD0).w);
            mediump vec2 tmpvar_3;
            highp vec2 tmpvar_4;
            tmpvar_4 = clamp (((
              (_ClipRect.zw - _ClipRect.xy)
             - 
              abs(xlv_TEXCOORD2.xy)
            ) * xlv_TEXCOORD2.zw), 0.0, 1.0);
            tmpvar_3 = tmpvar_4;
            color_1 = (tmpvar_2 * (tmpvar_3.x * tmpvar_3.y));
            lowp float x_5;
            x_5 = (color_1.w - 0.001);
            if ((x_5 < 0.0)) {
              discard;
            };
            gl_FragData[0] = color_1;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier01"
        {
           Keywords { "UNITY_UI_CLIP_RECT" "UNITY_UI_ALPHACLIP" }
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesVertex;
          attribute vec4 _glesColor;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec4 _ScreenParams;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp vec4 _FaceTex_ST;
          uniform lowp vec4 _FaceColor;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp vec4 _ClipRect;
          uniform highp float _MaskSoftnessX;
          uniform highp float _MaskSoftnessY;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          void main ()
          {
            highp vec4 vert_1;
            vert_1.zw = _glesVertex.zw;
            vert_1.x = (_glesVertex.x + _VertexOffsetX);
            vert_1.y = (_glesVertex.y + _VertexOffsetY);
            vert_1.xy = (vert_1.xy + ((_glesVertex.w * 0.5) / _ScreenParams.xy));
            highp vec4 tmpvar_2;
            highp vec4 tmpvar_3;
            tmpvar_3.w = 1.0;
            tmpvar_3.xyz = vert_1.xyz;
            tmpvar_2 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_3));
            highp vec4 pos_4;
            pos_4.zw = tmpvar_2.zw;
            highp vec2 tmpvar_5;
            tmpvar_5 = (_ScreenParams.xy * 0.5);
            pos_4.xy = ((floor(
              (((tmpvar_2.xy / tmpvar_2.w) * tmpvar_5) + vec2(0.5, 0.5))
            ) / tmpvar_5) * tmpvar_2.w);
            highp vec2 xlat_varoutput_6;
            xlat_varoutput_6.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_6.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_6.x));
            highp vec2 tmpvar_7;
            tmpvar_7.x = (_ScreenParams.x * glstate_matrix_projection[0].x);
            tmpvar_7.y = (_ScreenParams.y * glstate_matrix_projection[1].y);
            highp vec4 tmpvar_8;
            tmpvar_8 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_9;
            tmpvar_9.x = _MaskSoftnessX;
            tmpvar_9.y = _MaskSoftnessY;
            highp vec4 tmpvar_10;
            tmpvar_10.xy = (((vert_1.xy * 2.0) - tmpvar_8.xy) - tmpvar_8.zw);
            tmpvar_10.zw = (0.25 / ((0.25 * tmpvar_9) + (tmpvar_2.ww / 
              abs(tmpvar_7)
            )));
            gl_Position = pos_4;
            xlv_COLOR = (_glesColor * _FaceColor);
            xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
            xlv_TEXCOORD1 = (((xlat_varoutput_6 * 0.001953125) * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_10;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform sampler2D _MainTex;
          uniform sampler2D _FaceTex;
          uniform highp vec4 _ClipRect;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 color_1;
            lowp vec4 tmpvar_2;
            tmpvar_2.xyz = (texture2D (_FaceTex, xlv_TEXCOORD1).xyz * xlv_COLOR.xyz);
            tmpvar_2.w = (xlv_COLOR.w * texture2D (_MainTex, xlv_TEXCOORD0).w);
            mediump vec2 tmpvar_3;
            highp vec2 tmpvar_4;
            tmpvar_4 = clamp (((
              (_ClipRect.zw - _ClipRect.xy)
             - 
              abs(xlv_TEXCOORD2.xy)
            ) * xlv_TEXCOORD2.zw), 0.0, 1.0);
            tmpvar_3 = tmpvar_4;
            color_1 = (tmpvar_2 * (tmpvar_3.x * tmpvar_3.y));
            lowp float x_5;
            x_5 = (color_1.w - 0.001);
            if ((x_5 < 0.0)) {
              discard;
            };
            gl_FragData[0] = color_1;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier02"
        {
           Keywords { "UNITY_UI_CLIP_RECT" "UNITY_UI_ALPHACLIP" }
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesVertex;
          attribute vec4 _glesColor;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec4 _ScreenParams;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp vec4 _FaceTex_ST;
          uniform lowp vec4 _FaceColor;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp vec4 _ClipRect;
          uniform highp float _MaskSoftnessX;
          uniform highp float _MaskSoftnessY;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          void main ()
          {
            highp vec4 vert_1;
            vert_1.zw = _glesVertex.zw;
            vert_1.x = (_glesVertex.x + _VertexOffsetX);
            vert_1.y = (_glesVertex.y + _VertexOffsetY);
            vert_1.xy = (vert_1.xy + ((_glesVertex.w * 0.5) / _ScreenParams.xy));
            highp vec4 tmpvar_2;
            highp vec4 tmpvar_3;
            tmpvar_3.w = 1.0;
            tmpvar_3.xyz = vert_1.xyz;
            tmpvar_2 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_3));
            highp vec4 pos_4;
            pos_4.zw = tmpvar_2.zw;
            highp vec2 tmpvar_5;
            tmpvar_5 = (_ScreenParams.xy * 0.5);
            pos_4.xy = ((floor(
              (((tmpvar_2.xy / tmpvar_2.w) * tmpvar_5) + vec2(0.5, 0.5))
            ) / tmpvar_5) * tmpvar_2.w);
            highp vec2 xlat_varoutput_6;
            xlat_varoutput_6.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_6.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_6.x));
            highp vec2 tmpvar_7;
            tmpvar_7.x = (_ScreenParams.x * glstate_matrix_projection[0].x);
            tmpvar_7.y = (_ScreenParams.y * glstate_matrix_projection[1].y);
            highp vec4 tmpvar_8;
            tmpvar_8 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_9;
            tmpvar_9.x = _MaskSoftnessX;
            tmpvar_9.y = _MaskSoftnessY;
            highp vec4 tmpvar_10;
            tmpvar_10.xy = (((vert_1.xy * 2.0) - tmpvar_8.xy) - tmpvar_8.zw);
            tmpvar_10.zw = (0.25 / ((0.25 * tmpvar_9) + (tmpvar_2.ww / 
              abs(tmpvar_7)
            )));
            gl_Position = pos_4;
            xlv_COLOR = (_glesColor * _FaceColor);
            xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
            xlv_TEXCOORD1 = (((xlat_varoutput_6 * 0.001953125) * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_10;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform sampler2D _MainTex;
          uniform sampler2D _FaceTex;
          uniform highp vec4 _ClipRect;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 color_1;
            lowp vec4 tmpvar_2;
            tmpvar_2.xyz = (texture2D (_FaceTex, xlv_TEXCOORD1).xyz * xlv_COLOR.xyz);
            tmpvar_2.w = (xlv_COLOR.w * texture2D (_MainTex, xlv_TEXCOORD0).w);
            mediump vec2 tmpvar_3;
            highp vec2 tmpvar_4;
            tmpvar_4 = clamp (((
              (_ClipRect.zw - _ClipRect.xy)
             - 
              abs(xlv_TEXCOORD2.xy)
            ) * xlv_TEXCOORD2.zw), 0.0, 1.0);
            tmpvar_3 = tmpvar_4;
            color_1 = (tmpvar_2 * (tmpvar_3.x * tmpvar_3.y));
            lowp float x_5;
            x_5 = (color_1.w - 0.001);
            if ((x_5 < 0.0)) {
              discard;
            };
            gl_FragData[0] = color_1;
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
        SubProgram "gles hw_tier00"
        {
           Keywords { "UNITY_UI_ALPHACLIP" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier01"
        {
           Keywords { "UNITY_UI_ALPHACLIP" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier02"
        {
           Keywords { "UNITY_UI_ALPHACLIP" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier00"
        {
           Keywords { "UNITY_UI_CLIP_RECT" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier01"
        {
           Keywords { "UNITY_UI_CLIP_RECT" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier02"
        {
           Keywords { "UNITY_UI_CLIP_RECT" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier00"
        {
           Keywords { "UNITY_UI_CLIP_RECT" "UNITY_UI_ALPHACLIP" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier01"
        {
           Keywords { "UNITY_UI_CLIP_RECT" "UNITY_UI_ALPHACLIP" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier02"
        {
           Keywords { "UNITY_UI_CLIP_RECT" "UNITY_UI_ALPHACLIP" }
          
          "!!!!GLES
          
          
          "
        }
      }
      
    } // end phase
  }
  FallBack Off
}
