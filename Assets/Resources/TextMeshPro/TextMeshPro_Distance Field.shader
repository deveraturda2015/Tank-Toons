Shader "TextMeshPro/Distance Field"
{
  Properties
  {
    _FaceTex ("Face Texture", 2D) = "white" {}
    _FaceUVSpeedX ("Face UV Speed X", Range(-5, 5)) = 0
    _FaceUVSpeedY ("Face UV Speed Y", Range(-5, 5)) = 0
    _FaceColor ("Face Color", Color) = (1,1,1,1)
    _FaceDilate ("Face Dilate", Range(-1, 1)) = 0
    _OutlineColor ("Outline Color", Color) = (0,0,0,1)
    _OutlineTex ("Outline Texture", 2D) = "white" {}
    _OutlineUVSpeedX ("Outline UV Speed X", Range(-5, 5)) = 0
    _OutlineUVSpeedY ("Outline UV Speed Y", Range(-5, 5)) = 0
    _OutlineWidth ("Outline Thickness", Range(0, 1)) = 0
    _OutlineSoftness ("Outline Softness", Range(-1, 1)) = 0
    _Bevel ("Bevel", Range(0, 1)) = 0.5
    _BevelOffset ("Bevel Offset", Range(-0.5, 0.5)) = 0
    _BevelWidth ("Bevel Width", Range(-0.5, 0.5)) = 0
    _BevelClamp ("Bevel Clamp", Range(0, 1)) = 0
    _BevelRoundness ("Bevel Roundness", Range(0, 1)) = 0
    _LightAngle ("Light Angle", Range(0, 6.283185)) = 3.1416
    _SpecularColor ("Specular", Color) = (1,1,1,1)
    _SpecularPower ("Specular", Range(0, 4)) = 2
    _Reflectivity ("Reflectivity", Range(5, 15)) = 10
    _Diffuse ("Diffuse", Range(0, 1)) = 0.5
    _Ambient ("Ambient", Range(1, 0)) = 0.5
    _BumpMap ("Normal map", 2D) = "bump" {}
    _BumpOutline ("Bump Outline", Range(0, 1)) = 0
    _BumpFace ("Bump Face", Range(0, 1)) = 0
    _ReflectFaceColor ("Reflection Color", Color) = (0,0,0,1)
    _ReflectOutlineColor ("Reflection Color", Color) = (0,0,0,1)
    _Cube ("Reflection Cubemap", Cube) = "black" {}
    _EnvMatrixRotation ("Texture Rotation", Vector) = (0,0,0,0)
    _UnderlayColor ("Border Color", Color) = (0,0,0,0.5)
    _UnderlayOffsetX ("Border OffsetX", Range(-1, 1)) = 0
    _UnderlayOffsetY ("Border OffsetY", Range(-1, 1)) = 0
    _UnderlayDilate ("Border Dilate", Range(-1, 1)) = 0
    _UnderlaySoftness ("Border Softness", Range(0, 1)) = 0
    _GlowColor ("Color", Color) = (0,1,0,0.5)
    _GlowOffset ("Offset", Range(-1, 1)) = 0
    _GlowInner ("Inner", Range(0, 1)) = 0.05
    _GlowOuter ("Outer", Range(0, 1)) = 0.05
    _GlowPower ("Falloff", Range(1, 0)) = 0.75
    _WeightNormal ("Weight Normal", float) = 0
    _WeightBold ("Weight Bold", float) = 0.5
    _ShaderFlags ("Flags", float) = 0
    _ScaleRatioA ("Scale RatioA", float) = 1
    _ScaleRatioB ("Scale RatioB", float) = 1
    _ScaleRatioC ("Scale RatioC", float) = 1
    _MainTex ("Font Atlas", 2D) = "white" {}
    _TextureWidth ("Texture Width", float) = 512
    _TextureHeight ("Texture Height", float) = 512
    _GradientScale ("Gradient Scale", float) = 5
    _ScaleX ("Scale X", float) = 1
    _ScaleY ("Scale Y", float) = 1
    _PerspectiveFilter ("Perspective Correction", Range(0, 1)) = 0.875
    _VertexOffsetX ("Vertex OffsetX", float) = 0
    _VertexOffsetY ("Vertex OffsetY", float) = 0
    _MaskCoord ("Mask Coordinates", Vector) = (0,0,32767,32767)
    _ClipRect ("Clip Rect", Vector) = (-32767,-32767,32767,32767)
    _MaskSoftnessX ("Mask SoftnessX", float) = 0
    _MaskSoftnessY ("Mask SoftnessY", float) = 0
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
      Blend One OneMinusSrcAlpha
      ColorMask 0
      GpuProgramID 57834
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
          attribute vec3 _glesNormal;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _ScreenParams;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp float _FaceDilate;
          uniform highp float _OutlineSoftness;
          uniform highp float _OutlineWidth;
          uniform highp mat4 _EnvMatrix;
          uniform highp float _WeightNormal;
          uniform highp float _WeightBold;
          uniform highp float _ScaleRatioA;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp vec4 _ClipRect;
          uniform highp float _MaskSoftnessX;
          uniform highp float _MaskSoftnessY;
          uniform highp float _GradientScale;
          uniform highp float _ScaleX;
          uniform highp float _ScaleY;
          uniform highp float _PerspectiveFilter;
          uniform highp vec4 _FaceTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec4 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD5;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = _glesColor;
            highp vec2 tmpvar_2;
            tmpvar_2 = _glesMultiTexCoord0.xy;
            highp float weight_3;
            highp float scale_4;
            highp vec2 pixelSize_5;
            highp vec4 vert_6;
            highp float tmpvar_7;
            tmpvar_7 = float((0.0 >= _glesMultiTexCoord1.y));
            vert_6.zw = _glesVertex.zw;
            vert_6.x = (_glesVertex.x + _VertexOffsetX);
            vert_6.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_8;
            highp vec4 tmpvar_9;
            tmpvar_9.w = 1.0;
            tmpvar_9.xyz = vert_6.xyz;
            tmpvar_8 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_9));
            highp vec2 tmpvar_10;
            tmpvar_10.x = _ScaleX;
            tmpvar_10.y = _ScaleY;
            highp mat2 tmpvar_11;
            tmpvar_11[0] = glstate_matrix_projection[0].xy;
            tmpvar_11[1] = glstate_matrix_projection[1].xy;
            pixelSize_5 = (tmpvar_8.ww / (tmpvar_10 * abs(
              (tmpvar_11 * _ScreenParams.xy)
            )));
            scale_4 = (inversesqrt(dot (pixelSize_5, pixelSize_5)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            if ((glstate_matrix_projection[3].w == 0.0)) {
              highp mat3 tmpvar_12;
              tmpvar_12[0] = unity_WorldToObject[0].xyz;
              tmpvar_12[1] = unity_WorldToObject[1].xyz;
              tmpvar_12[2] = unity_WorldToObject[2].xyz;
              scale_4 = mix ((abs(scale_4) * (1.0 - _PerspectiveFilter)), scale_4, abs(dot (
                normalize((_glesNormal * tmpvar_12))
              , 
                normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz))
              )));
            };
            weight_3 = (((
              (mix (_WeightNormal, _WeightBold, tmpvar_7) / 4.0)
             + _FaceDilate) * _ScaleRatioA) * 0.5);
            highp vec4 tmpvar_13;
            tmpvar_13 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_14;
            highp vec2 xlat_varoutput_15;
            xlat_varoutput_15.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_15.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_15.x));
            tmpvar_14 = (xlat_varoutput_15 * 0.001953125);
            highp vec4 tmpvar_16;
            tmpvar_16.x = (((
              ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
             / 2.0) - (0.5 / scale_4)) - weight_3);
            tmpvar_16.y = scale_4;
            tmpvar_16.z = ((0.5 - weight_3) + (0.5 / scale_4));
            tmpvar_16.w = weight_3;
            highp vec2 tmpvar_17;
            tmpvar_17.x = _MaskSoftnessX;
            tmpvar_17.y = _MaskSoftnessY;
            highp vec4 tmpvar_18;
            tmpvar_18.xy = (((vert_6.xy * 2.0) - tmpvar_13.xy) - tmpvar_13.zw);
            tmpvar_18.zw = (0.25 / ((0.25 * tmpvar_17) + pixelSize_5));
            highp mat3 tmpvar_19;
            tmpvar_19[0] = _EnvMatrix[0].xyz;
            tmpvar_19[1] = _EnvMatrix[1].xyz;
            tmpvar_19[2] = _EnvMatrix[2].xyz;
            highp vec4 tmpvar_20;
            tmpvar_20.xy = ((tmpvar_14 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            tmpvar_20.zw = ((tmpvar_14 * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            gl_Position = tmpvar_8;
            xlv_COLOR = tmpvar_1;
            xlv_TEXCOORD0 = tmpvar_2;
            xlv_TEXCOORD1 = tmpvar_16;
            xlv_TEXCOORD2 = tmpvar_18;
            xlv_TEXCOORD3 = (tmpvar_19 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz));
            xlv_TEXCOORD5 = tmpvar_20;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform sampler2D _FaceTex;
          uniform highp float _FaceUVSpeedX;
          uniform highp float _FaceUVSpeedY;
          uniform lowp vec4 _FaceColor;
          uniform highp float _OutlineSoftness;
          uniform sampler2D _OutlineTex;
          uniform highp float _OutlineUVSpeedX;
          uniform highp float _OutlineUVSpeedY;
          uniform lowp vec4 _OutlineColor;
          uniform highp float _OutlineWidth;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec4 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD5;
          void main ()
          {
            lowp vec4 tmpvar_1;
            mediump vec4 outlineColor_2;
            mediump vec4 faceColor_3;
            highp float c_4;
            lowp float tmpvar_5;
            tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0).w;
            c_4 = tmpvar_5;
            highp float x_6;
            x_6 = (c_4 - xlv_TEXCOORD1.x);
            if ((x_6 < 0.0)) {
              discard;
            };
            highp float tmpvar_7;
            tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
            highp float tmpvar_8;
            tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
            highp float tmpvar_9;
            tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
            faceColor_3 = _FaceColor;
            outlineColor_2 = _OutlineColor;
            faceColor_3.xyz = (faceColor_3.xyz * xlv_COLOR.xyz);
            highp vec2 tmpvar_10;
            tmpvar_10.x = _FaceUVSpeedX;
            tmpvar_10.y = _FaceUVSpeedY;
            lowp vec4 tmpvar_11;
            highp vec2 P_12;
            P_12 = (xlv_TEXCOORD5.xy + (tmpvar_10 * _Time.y));
            tmpvar_11 = texture2D (_FaceTex, P_12);
            faceColor_3 = (faceColor_3 * tmpvar_11);
            highp vec2 tmpvar_13;
            tmpvar_13.x = _OutlineUVSpeedX;
            tmpvar_13.y = _OutlineUVSpeedY;
            lowp vec4 tmpvar_14;
            highp vec2 P_15;
            P_15 = (xlv_TEXCOORD5.zw + (tmpvar_13 * _Time.y));
            tmpvar_14 = texture2D (_OutlineTex, P_15);
            outlineColor_2 = (outlineColor_2 * tmpvar_14);
            mediump float d_16;
            d_16 = tmpvar_7;
            lowp vec4 faceColor_17;
            faceColor_17 = faceColor_3;
            lowp vec4 outlineColor_18;
            outlineColor_18 = outlineColor_2;
            mediump float outline_19;
            outline_19 = tmpvar_8;
            mediump float softness_20;
            softness_20 = tmpvar_9;
            mediump float tmpvar_21;
            tmpvar_21 = (1.0 - clamp ((
              ((d_16 - (outline_19 * 0.5)) + (softness_20 * 0.5))
             / 
              (1.0 + softness_20)
            ), 0.0, 1.0));
            faceColor_17.xyz = (faceColor_17.xyz * faceColor_17.w);
            outlineColor_18.xyz = (outlineColor_18.xyz * outlineColor_18.w);
            mediump vec4 tmpvar_22;
            tmpvar_22 = mix (faceColor_17, outlineColor_18, vec4((clamp (
              (d_16 + (outline_19 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_19)
            ))));
            faceColor_17 = tmpvar_22;
            faceColor_17 = (faceColor_17 * tmpvar_21);
            faceColor_3 = faceColor_17;
            tmpvar_1 = (faceColor_3 * xlv_COLOR.w);
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
          attribute vec3 _glesNormal;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _ScreenParams;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp float _FaceDilate;
          uniform highp float _OutlineSoftness;
          uniform highp float _OutlineWidth;
          uniform highp mat4 _EnvMatrix;
          uniform highp float _WeightNormal;
          uniform highp float _WeightBold;
          uniform highp float _ScaleRatioA;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp vec4 _ClipRect;
          uniform highp float _MaskSoftnessX;
          uniform highp float _MaskSoftnessY;
          uniform highp float _GradientScale;
          uniform highp float _ScaleX;
          uniform highp float _ScaleY;
          uniform highp float _PerspectiveFilter;
          uniform highp vec4 _FaceTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec4 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD5;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = _glesColor;
            highp vec2 tmpvar_2;
            tmpvar_2 = _glesMultiTexCoord0.xy;
            highp float weight_3;
            highp float scale_4;
            highp vec2 pixelSize_5;
            highp vec4 vert_6;
            highp float tmpvar_7;
            tmpvar_7 = float((0.0 >= _glesMultiTexCoord1.y));
            vert_6.zw = _glesVertex.zw;
            vert_6.x = (_glesVertex.x + _VertexOffsetX);
            vert_6.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_8;
            highp vec4 tmpvar_9;
            tmpvar_9.w = 1.0;
            tmpvar_9.xyz = vert_6.xyz;
            tmpvar_8 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_9));
            highp vec2 tmpvar_10;
            tmpvar_10.x = _ScaleX;
            tmpvar_10.y = _ScaleY;
            highp mat2 tmpvar_11;
            tmpvar_11[0] = glstate_matrix_projection[0].xy;
            tmpvar_11[1] = glstate_matrix_projection[1].xy;
            pixelSize_5 = (tmpvar_8.ww / (tmpvar_10 * abs(
              (tmpvar_11 * _ScreenParams.xy)
            )));
            scale_4 = (inversesqrt(dot (pixelSize_5, pixelSize_5)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            if ((glstate_matrix_projection[3].w == 0.0)) {
              highp mat3 tmpvar_12;
              tmpvar_12[0] = unity_WorldToObject[0].xyz;
              tmpvar_12[1] = unity_WorldToObject[1].xyz;
              tmpvar_12[2] = unity_WorldToObject[2].xyz;
              scale_4 = mix ((abs(scale_4) * (1.0 - _PerspectiveFilter)), scale_4, abs(dot (
                normalize((_glesNormal * tmpvar_12))
              , 
                normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz))
              )));
            };
            weight_3 = (((
              (mix (_WeightNormal, _WeightBold, tmpvar_7) / 4.0)
             + _FaceDilate) * _ScaleRatioA) * 0.5);
            highp vec4 tmpvar_13;
            tmpvar_13 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_14;
            highp vec2 xlat_varoutput_15;
            xlat_varoutput_15.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_15.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_15.x));
            tmpvar_14 = (xlat_varoutput_15 * 0.001953125);
            highp vec4 tmpvar_16;
            tmpvar_16.x = (((
              ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
             / 2.0) - (0.5 / scale_4)) - weight_3);
            tmpvar_16.y = scale_4;
            tmpvar_16.z = ((0.5 - weight_3) + (0.5 / scale_4));
            tmpvar_16.w = weight_3;
            highp vec2 tmpvar_17;
            tmpvar_17.x = _MaskSoftnessX;
            tmpvar_17.y = _MaskSoftnessY;
            highp vec4 tmpvar_18;
            tmpvar_18.xy = (((vert_6.xy * 2.0) - tmpvar_13.xy) - tmpvar_13.zw);
            tmpvar_18.zw = (0.25 / ((0.25 * tmpvar_17) + pixelSize_5));
            highp mat3 tmpvar_19;
            tmpvar_19[0] = _EnvMatrix[0].xyz;
            tmpvar_19[1] = _EnvMatrix[1].xyz;
            tmpvar_19[2] = _EnvMatrix[2].xyz;
            highp vec4 tmpvar_20;
            tmpvar_20.xy = ((tmpvar_14 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            tmpvar_20.zw = ((tmpvar_14 * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            gl_Position = tmpvar_8;
            xlv_COLOR = tmpvar_1;
            xlv_TEXCOORD0 = tmpvar_2;
            xlv_TEXCOORD1 = tmpvar_16;
            xlv_TEXCOORD2 = tmpvar_18;
            xlv_TEXCOORD3 = (tmpvar_19 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz));
            xlv_TEXCOORD5 = tmpvar_20;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform sampler2D _FaceTex;
          uniform highp float _FaceUVSpeedX;
          uniform highp float _FaceUVSpeedY;
          uniform lowp vec4 _FaceColor;
          uniform highp float _OutlineSoftness;
          uniform sampler2D _OutlineTex;
          uniform highp float _OutlineUVSpeedX;
          uniform highp float _OutlineUVSpeedY;
          uniform lowp vec4 _OutlineColor;
          uniform highp float _OutlineWidth;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec4 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD5;
          void main ()
          {
            lowp vec4 tmpvar_1;
            mediump vec4 outlineColor_2;
            mediump vec4 faceColor_3;
            highp float c_4;
            lowp float tmpvar_5;
            tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0).w;
            c_4 = tmpvar_5;
            highp float x_6;
            x_6 = (c_4 - xlv_TEXCOORD1.x);
            if ((x_6 < 0.0)) {
              discard;
            };
            highp float tmpvar_7;
            tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
            highp float tmpvar_8;
            tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
            highp float tmpvar_9;
            tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
            faceColor_3 = _FaceColor;
            outlineColor_2 = _OutlineColor;
            faceColor_3.xyz = (faceColor_3.xyz * xlv_COLOR.xyz);
            highp vec2 tmpvar_10;
            tmpvar_10.x = _FaceUVSpeedX;
            tmpvar_10.y = _FaceUVSpeedY;
            lowp vec4 tmpvar_11;
            highp vec2 P_12;
            P_12 = (xlv_TEXCOORD5.xy + (tmpvar_10 * _Time.y));
            tmpvar_11 = texture2D (_FaceTex, P_12);
            faceColor_3 = (faceColor_3 * tmpvar_11);
            highp vec2 tmpvar_13;
            tmpvar_13.x = _OutlineUVSpeedX;
            tmpvar_13.y = _OutlineUVSpeedY;
            lowp vec4 tmpvar_14;
            highp vec2 P_15;
            P_15 = (xlv_TEXCOORD5.zw + (tmpvar_13 * _Time.y));
            tmpvar_14 = texture2D (_OutlineTex, P_15);
            outlineColor_2 = (outlineColor_2 * tmpvar_14);
            mediump float d_16;
            d_16 = tmpvar_7;
            lowp vec4 faceColor_17;
            faceColor_17 = faceColor_3;
            lowp vec4 outlineColor_18;
            outlineColor_18 = outlineColor_2;
            mediump float outline_19;
            outline_19 = tmpvar_8;
            mediump float softness_20;
            softness_20 = tmpvar_9;
            mediump float tmpvar_21;
            tmpvar_21 = (1.0 - clamp ((
              ((d_16 - (outline_19 * 0.5)) + (softness_20 * 0.5))
             / 
              (1.0 + softness_20)
            ), 0.0, 1.0));
            faceColor_17.xyz = (faceColor_17.xyz * faceColor_17.w);
            outlineColor_18.xyz = (outlineColor_18.xyz * outlineColor_18.w);
            mediump vec4 tmpvar_22;
            tmpvar_22 = mix (faceColor_17, outlineColor_18, vec4((clamp (
              (d_16 + (outline_19 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_19)
            ))));
            faceColor_17 = tmpvar_22;
            faceColor_17 = (faceColor_17 * tmpvar_21);
            faceColor_3 = faceColor_17;
            tmpvar_1 = (faceColor_3 * xlv_COLOR.w);
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
          attribute vec3 _glesNormal;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _ScreenParams;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp float _FaceDilate;
          uniform highp float _OutlineSoftness;
          uniform highp float _OutlineWidth;
          uniform highp mat4 _EnvMatrix;
          uniform highp float _WeightNormal;
          uniform highp float _WeightBold;
          uniform highp float _ScaleRatioA;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp vec4 _ClipRect;
          uniform highp float _MaskSoftnessX;
          uniform highp float _MaskSoftnessY;
          uniform highp float _GradientScale;
          uniform highp float _ScaleX;
          uniform highp float _ScaleY;
          uniform highp float _PerspectiveFilter;
          uniform highp vec4 _FaceTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec4 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD5;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = _glesColor;
            highp vec2 tmpvar_2;
            tmpvar_2 = _glesMultiTexCoord0.xy;
            highp float weight_3;
            highp float scale_4;
            highp vec2 pixelSize_5;
            highp vec4 vert_6;
            highp float tmpvar_7;
            tmpvar_7 = float((0.0 >= _glesMultiTexCoord1.y));
            vert_6.zw = _glesVertex.zw;
            vert_6.x = (_glesVertex.x + _VertexOffsetX);
            vert_6.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_8;
            highp vec4 tmpvar_9;
            tmpvar_9.w = 1.0;
            tmpvar_9.xyz = vert_6.xyz;
            tmpvar_8 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_9));
            highp vec2 tmpvar_10;
            tmpvar_10.x = _ScaleX;
            tmpvar_10.y = _ScaleY;
            highp mat2 tmpvar_11;
            tmpvar_11[0] = glstate_matrix_projection[0].xy;
            tmpvar_11[1] = glstate_matrix_projection[1].xy;
            pixelSize_5 = (tmpvar_8.ww / (tmpvar_10 * abs(
              (tmpvar_11 * _ScreenParams.xy)
            )));
            scale_4 = (inversesqrt(dot (pixelSize_5, pixelSize_5)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            if ((glstate_matrix_projection[3].w == 0.0)) {
              highp mat3 tmpvar_12;
              tmpvar_12[0] = unity_WorldToObject[0].xyz;
              tmpvar_12[1] = unity_WorldToObject[1].xyz;
              tmpvar_12[2] = unity_WorldToObject[2].xyz;
              scale_4 = mix ((abs(scale_4) * (1.0 - _PerspectiveFilter)), scale_4, abs(dot (
                normalize((_glesNormal * tmpvar_12))
              , 
                normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz))
              )));
            };
            weight_3 = (((
              (mix (_WeightNormal, _WeightBold, tmpvar_7) / 4.0)
             + _FaceDilate) * _ScaleRatioA) * 0.5);
            highp vec4 tmpvar_13;
            tmpvar_13 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_14;
            highp vec2 xlat_varoutput_15;
            xlat_varoutput_15.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_15.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_15.x));
            tmpvar_14 = (xlat_varoutput_15 * 0.001953125);
            highp vec4 tmpvar_16;
            tmpvar_16.x = (((
              ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
             / 2.0) - (0.5 / scale_4)) - weight_3);
            tmpvar_16.y = scale_4;
            tmpvar_16.z = ((0.5 - weight_3) + (0.5 / scale_4));
            tmpvar_16.w = weight_3;
            highp vec2 tmpvar_17;
            tmpvar_17.x = _MaskSoftnessX;
            tmpvar_17.y = _MaskSoftnessY;
            highp vec4 tmpvar_18;
            tmpvar_18.xy = (((vert_6.xy * 2.0) - tmpvar_13.xy) - tmpvar_13.zw);
            tmpvar_18.zw = (0.25 / ((0.25 * tmpvar_17) + pixelSize_5));
            highp mat3 tmpvar_19;
            tmpvar_19[0] = _EnvMatrix[0].xyz;
            tmpvar_19[1] = _EnvMatrix[1].xyz;
            tmpvar_19[2] = _EnvMatrix[2].xyz;
            highp vec4 tmpvar_20;
            tmpvar_20.xy = ((tmpvar_14 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            tmpvar_20.zw = ((tmpvar_14 * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            gl_Position = tmpvar_8;
            xlv_COLOR = tmpvar_1;
            xlv_TEXCOORD0 = tmpvar_2;
            xlv_TEXCOORD1 = tmpvar_16;
            xlv_TEXCOORD2 = tmpvar_18;
            xlv_TEXCOORD3 = (tmpvar_19 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz));
            xlv_TEXCOORD5 = tmpvar_20;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform sampler2D _FaceTex;
          uniform highp float _FaceUVSpeedX;
          uniform highp float _FaceUVSpeedY;
          uniform lowp vec4 _FaceColor;
          uniform highp float _OutlineSoftness;
          uniform sampler2D _OutlineTex;
          uniform highp float _OutlineUVSpeedX;
          uniform highp float _OutlineUVSpeedY;
          uniform lowp vec4 _OutlineColor;
          uniform highp float _OutlineWidth;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec4 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD5;
          void main ()
          {
            lowp vec4 tmpvar_1;
            mediump vec4 outlineColor_2;
            mediump vec4 faceColor_3;
            highp float c_4;
            lowp float tmpvar_5;
            tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0).w;
            c_4 = tmpvar_5;
            highp float x_6;
            x_6 = (c_4 - xlv_TEXCOORD1.x);
            if ((x_6 < 0.0)) {
              discard;
            };
            highp float tmpvar_7;
            tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
            highp float tmpvar_8;
            tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
            highp float tmpvar_9;
            tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
            faceColor_3 = _FaceColor;
            outlineColor_2 = _OutlineColor;
            faceColor_3.xyz = (faceColor_3.xyz * xlv_COLOR.xyz);
            highp vec2 tmpvar_10;
            tmpvar_10.x = _FaceUVSpeedX;
            tmpvar_10.y = _FaceUVSpeedY;
            lowp vec4 tmpvar_11;
            highp vec2 P_12;
            P_12 = (xlv_TEXCOORD5.xy + (tmpvar_10 * _Time.y));
            tmpvar_11 = texture2D (_FaceTex, P_12);
            faceColor_3 = (faceColor_3 * tmpvar_11);
            highp vec2 tmpvar_13;
            tmpvar_13.x = _OutlineUVSpeedX;
            tmpvar_13.y = _OutlineUVSpeedY;
            lowp vec4 tmpvar_14;
            highp vec2 P_15;
            P_15 = (xlv_TEXCOORD5.zw + (tmpvar_13 * _Time.y));
            tmpvar_14 = texture2D (_OutlineTex, P_15);
            outlineColor_2 = (outlineColor_2 * tmpvar_14);
            mediump float d_16;
            d_16 = tmpvar_7;
            lowp vec4 faceColor_17;
            faceColor_17 = faceColor_3;
            lowp vec4 outlineColor_18;
            outlineColor_18 = outlineColor_2;
            mediump float outline_19;
            outline_19 = tmpvar_8;
            mediump float softness_20;
            softness_20 = tmpvar_9;
            mediump float tmpvar_21;
            tmpvar_21 = (1.0 - clamp ((
              ((d_16 - (outline_19 * 0.5)) + (softness_20 * 0.5))
             / 
              (1.0 + softness_20)
            ), 0.0, 1.0));
            faceColor_17.xyz = (faceColor_17.xyz * faceColor_17.w);
            outlineColor_18.xyz = (outlineColor_18.xyz * outlineColor_18.w);
            mediump vec4 tmpvar_22;
            tmpvar_22 = mix (faceColor_17, outlineColor_18, vec4((clamp (
              (d_16 + (outline_19 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_19)
            ))));
            faceColor_17 = tmpvar_22;
            faceColor_17 = (faceColor_17 * tmpvar_21);
            faceColor_3 = faceColor_17;
            tmpvar_1 = (faceColor_3 * xlv_COLOR.w);
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
          attribute vec3 _glesNormal;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _ScreenParams;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp float _FaceDilate;
          uniform highp float _OutlineSoftness;
          uniform highp float _OutlineWidth;
          uniform highp mat4 _EnvMatrix;
          uniform highp float _WeightNormal;
          uniform highp float _WeightBold;
          uniform highp float _ScaleRatioA;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp vec4 _ClipRect;
          uniform highp float _MaskSoftnessX;
          uniform highp float _MaskSoftnessY;
          uniform highp float _GradientScale;
          uniform highp float _ScaleX;
          uniform highp float _ScaleY;
          uniform highp float _PerspectiveFilter;
          uniform highp vec4 _FaceTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec4 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD5;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = _glesColor;
            highp vec2 tmpvar_2;
            tmpvar_2 = _glesMultiTexCoord0.xy;
            highp float weight_3;
            highp float scale_4;
            highp vec2 pixelSize_5;
            highp vec4 vert_6;
            highp float tmpvar_7;
            tmpvar_7 = float((0.0 >= _glesMultiTexCoord1.y));
            vert_6.zw = _glesVertex.zw;
            vert_6.x = (_glesVertex.x + _VertexOffsetX);
            vert_6.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_8;
            highp vec4 tmpvar_9;
            tmpvar_9.w = 1.0;
            tmpvar_9.xyz = vert_6.xyz;
            tmpvar_8 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_9));
            highp vec2 tmpvar_10;
            tmpvar_10.x = _ScaleX;
            tmpvar_10.y = _ScaleY;
            highp mat2 tmpvar_11;
            tmpvar_11[0] = glstate_matrix_projection[0].xy;
            tmpvar_11[1] = glstate_matrix_projection[1].xy;
            pixelSize_5 = (tmpvar_8.ww / (tmpvar_10 * abs(
              (tmpvar_11 * _ScreenParams.xy)
            )));
            scale_4 = (inversesqrt(dot (pixelSize_5, pixelSize_5)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            if ((glstate_matrix_projection[3].w == 0.0)) {
              highp mat3 tmpvar_12;
              tmpvar_12[0] = unity_WorldToObject[0].xyz;
              tmpvar_12[1] = unity_WorldToObject[1].xyz;
              tmpvar_12[2] = unity_WorldToObject[2].xyz;
              scale_4 = mix ((abs(scale_4) * (1.0 - _PerspectiveFilter)), scale_4, abs(dot (
                normalize((_glesNormal * tmpvar_12))
              , 
                normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz))
              )));
            };
            weight_3 = (((
              (mix (_WeightNormal, _WeightBold, tmpvar_7) / 4.0)
             + _FaceDilate) * _ScaleRatioA) * 0.5);
            highp vec4 tmpvar_13;
            tmpvar_13 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_14;
            highp vec2 xlat_varoutput_15;
            xlat_varoutput_15.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_15.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_15.x));
            tmpvar_14 = (xlat_varoutput_15 * 0.001953125);
            highp vec4 tmpvar_16;
            tmpvar_16.x = (((
              ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
             / 2.0) - (0.5 / scale_4)) - weight_3);
            tmpvar_16.y = scale_4;
            tmpvar_16.z = ((0.5 - weight_3) + (0.5 / scale_4));
            tmpvar_16.w = weight_3;
            highp vec2 tmpvar_17;
            tmpvar_17.x = _MaskSoftnessX;
            tmpvar_17.y = _MaskSoftnessY;
            highp vec4 tmpvar_18;
            tmpvar_18.xy = (((vert_6.xy * 2.0) - tmpvar_13.xy) - tmpvar_13.zw);
            tmpvar_18.zw = (0.25 / ((0.25 * tmpvar_17) + pixelSize_5));
            highp mat3 tmpvar_19;
            tmpvar_19[0] = _EnvMatrix[0].xyz;
            tmpvar_19[1] = _EnvMatrix[1].xyz;
            tmpvar_19[2] = _EnvMatrix[2].xyz;
            highp vec4 tmpvar_20;
            tmpvar_20.xy = ((tmpvar_14 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            tmpvar_20.zw = ((tmpvar_14 * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            gl_Position = tmpvar_8;
            xlv_COLOR = tmpvar_1;
            xlv_TEXCOORD0 = tmpvar_2;
            xlv_TEXCOORD1 = tmpvar_16;
            xlv_TEXCOORD2 = tmpvar_18;
            xlv_TEXCOORD3 = (tmpvar_19 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz));
            xlv_TEXCOORD5 = tmpvar_20;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform sampler2D _FaceTex;
          uniform highp float _FaceUVSpeedX;
          uniform highp float _FaceUVSpeedY;
          uniform lowp vec4 _FaceColor;
          uniform highp float _OutlineSoftness;
          uniform sampler2D _OutlineTex;
          uniform highp float _OutlineUVSpeedX;
          uniform highp float _OutlineUVSpeedY;
          uniform lowp vec4 _OutlineColor;
          uniform highp float _OutlineWidth;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec4 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD5;
          void main ()
          {
            lowp vec4 tmpvar_1;
            mediump vec4 outlineColor_2;
            mediump vec4 faceColor_3;
            highp float c_4;
            lowp float tmpvar_5;
            tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0).w;
            c_4 = tmpvar_5;
            highp float x_6;
            x_6 = (c_4 - xlv_TEXCOORD1.x);
            if ((x_6 < 0.0)) {
              discard;
            };
            highp float tmpvar_7;
            tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
            highp float tmpvar_8;
            tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
            highp float tmpvar_9;
            tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
            faceColor_3 = _FaceColor;
            outlineColor_2 = _OutlineColor;
            faceColor_3.xyz = (faceColor_3.xyz * xlv_COLOR.xyz);
            highp vec2 tmpvar_10;
            tmpvar_10.x = _FaceUVSpeedX;
            tmpvar_10.y = _FaceUVSpeedY;
            lowp vec4 tmpvar_11;
            highp vec2 P_12;
            P_12 = (xlv_TEXCOORD5.xy + (tmpvar_10 * _Time.y));
            tmpvar_11 = texture2D (_FaceTex, P_12);
            faceColor_3 = (faceColor_3 * tmpvar_11);
            highp vec2 tmpvar_13;
            tmpvar_13.x = _OutlineUVSpeedX;
            tmpvar_13.y = _OutlineUVSpeedY;
            lowp vec4 tmpvar_14;
            highp vec2 P_15;
            P_15 = (xlv_TEXCOORD5.zw + (tmpvar_13 * _Time.y));
            tmpvar_14 = texture2D (_OutlineTex, P_15);
            outlineColor_2 = (outlineColor_2 * tmpvar_14);
            mediump float d_16;
            d_16 = tmpvar_7;
            lowp vec4 faceColor_17;
            faceColor_17 = faceColor_3;
            lowp vec4 outlineColor_18;
            outlineColor_18 = outlineColor_2;
            mediump float outline_19;
            outline_19 = tmpvar_8;
            mediump float softness_20;
            softness_20 = tmpvar_9;
            mediump float tmpvar_21;
            tmpvar_21 = (1.0 - clamp ((
              ((d_16 - (outline_19 * 0.5)) + (softness_20 * 0.5))
             / 
              (1.0 + softness_20)
            ), 0.0, 1.0));
            faceColor_17.xyz = (faceColor_17.xyz * faceColor_17.w);
            outlineColor_18.xyz = (outlineColor_18.xyz * outlineColor_18.w);
            mediump vec4 tmpvar_22;
            tmpvar_22 = mix (faceColor_17, outlineColor_18, vec4((clamp (
              (d_16 + (outline_19 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_19)
            ))));
            faceColor_17 = tmpvar_22;
            faceColor_17 = (faceColor_17 * tmpvar_21);
            faceColor_3 = faceColor_17;
            mediump float x_23;
            x_23 = (faceColor_3.w - 0.001);
            if ((x_23 < 0.0)) {
              discard;
            };
            tmpvar_1 = (faceColor_3 * xlv_COLOR.w);
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
          attribute vec3 _glesNormal;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _ScreenParams;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp float _FaceDilate;
          uniform highp float _OutlineSoftness;
          uniform highp float _OutlineWidth;
          uniform highp mat4 _EnvMatrix;
          uniform highp float _WeightNormal;
          uniform highp float _WeightBold;
          uniform highp float _ScaleRatioA;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp vec4 _ClipRect;
          uniform highp float _MaskSoftnessX;
          uniform highp float _MaskSoftnessY;
          uniform highp float _GradientScale;
          uniform highp float _ScaleX;
          uniform highp float _ScaleY;
          uniform highp float _PerspectiveFilter;
          uniform highp vec4 _FaceTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec4 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD5;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = _glesColor;
            highp vec2 tmpvar_2;
            tmpvar_2 = _glesMultiTexCoord0.xy;
            highp float weight_3;
            highp float scale_4;
            highp vec2 pixelSize_5;
            highp vec4 vert_6;
            highp float tmpvar_7;
            tmpvar_7 = float((0.0 >= _glesMultiTexCoord1.y));
            vert_6.zw = _glesVertex.zw;
            vert_6.x = (_glesVertex.x + _VertexOffsetX);
            vert_6.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_8;
            highp vec4 tmpvar_9;
            tmpvar_9.w = 1.0;
            tmpvar_9.xyz = vert_6.xyz;
            tmpvar_8 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_9));
            highp vec2 tmpvar_10;
            tmpvar_10.x = _ScaleX;
            tmpvar_10.y = _ScaleY;
            highp mat2 tmpvar_11;
            tmpvar_11[0] = glstate_matrix_projection[0].xy;
            tmpvar_11[1] = glstate_matrix_projection[1].xy;
            pixelSize_5 = (tmpvar_8.ww / (tmpvar_10 * abs(
              (tmpvar_11 * _ScreenParams.xy)
            )));
            scale_4 = (inversesqrt(dot (pixelSize_5, pixelSize_5)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            if ((glstate_matrix_projection[3].w == 0.0)) {
              highp mat3 tmpvar_12;
              tmpvar_12[0] = unity_WorldToObject[0].xyz;
              tmpvar_12[1] = unity_WorldToObject[1].xyz;
              tmpvar_12[2] = unity_WorldToObject[2].xyz;
              scale_4 = mix ((abs(scale_4) * (1.0 - _PerspectiveFilter)), scale_4, abs(dot (
                normalize((_glesNormal * tmpvar_12))
              , 
                normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz))
              )));
            };
            weight_3 = (((
              (mix (_WeightNormal, _WeightBold, tmpvar_7) / 4.0)
             + _FaceDilate) * _ScaleRatioA) * 0.5);
            highp vec4 tmpvar_13;
            tmpvar_13 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_14;
            highp vec2 xlat_varoutput_15;
            xlat_varoutput_15.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_15.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_15.x));
            tmpvar_14 = (xlat_varoutput_15 * 0.001953125);
            highp vec4 tmpvar_16;
            tmpvar_16.x = (((
              ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
             / 2.0) - (0.5 / scale_4)) - weight_3);
            tmpvar_16.y = scale_4;
            tmpvar_16.z = ((0.5 - weight_3) + (0.5 / scale_4));
            tmpvar_16.w = weight_3;
            highp vec2 tmpvar_17;
            tmpvar_17.x = _MaskSoftnessX;
            tmpvar_17.y = _MaskSoftnessY;
            highp vec4 tmpvar_18;
            tmpvar_18.xy = (((vert_6.xy * 2.0) - tmpvar_13.xy) - tmpvar_13.zw);
            tmpvar_18.zw = (0.25 / ((0.25 * tmpvar_17) + pixelSize_5));
            highp mat3 tmpvar_19;
            tmpvar_19[0] = _EnvMatrix[0].xyz;
            tmpvar_19[1] = _EnvMatrix[1].xyz;
            tmpvar_19[2] = _EnvMatrix[2].xyz;
            highp vec4 tmpvar_20;
            tmpvar_20.xy = ((tmpvar_14 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            tmpvar_20.zw = ((tmpvar_14 * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            gl_Position = tmpvar_8;
            xlv_COLOR = tmpvar_1;
            xlv_TEXCOORD0 = tmpvar_2;
            xlv_TEXCOORD1 = tmpvar_16;
            xlv_TEXCOORD2 = tmpvar_18;
            xlv_TEXCOORD3 = (tmpvar_19 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz));
            xlv_TEXCOORD5 = tmpvar_20;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform sampler2D _FaceTex;
          uniform highp float _FaceUVSpeedX;
          uniform highp float _FaceUVSpeedY;
          uniform lowp vec4 _FaceColor;
          uniform highp float _OutlineSoftness;
          uniform sampler2D _OutlineTex;
          uniform highp float _OutlineUVSpeedX;
          uniform highp float _OutlineUVSpeedY;
          uniform lowp vec4 _OutlineColor;
          uniform highp float _OutlineWidth;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec4 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD5;
          void main ()
          {
            lowp vec4 tmpvar_1;
            mediump vec4 outlineColor_2;
            mediump vec4 faceColor_3;
            highp float c_4;
            lowp float tmpvar_5;
            tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0).w;
            c_4 = tmpvar_5;
            highp float x_6;
            x_6 = (c_4 - xlv_TEXCOORD1.x);
            if ((x_6 < 0.0)) {
              discard;
            };
            highp float tmpvar_7;
            tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
            highp float tmpvar_8;
            tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
            highp float tmpvar_9;
            tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
            faceColor_3 = _FaceColor;
            outlineColor_2 = _OutlineColor;
            faceColor_3.xyz = (faceColor_3.xyz * xlv_COLOR.xyz);
            highp vec2 tmpvar_10;
            tmpvar_10.x = _FaceUVSpeedX;
            tmpvar_10.y = _FaceUVSpeedY;
            lowp vec4 tmpvar_11;
            highp vec2 P_12;
            P_12 = (xlv_TEXCOORD5.xy + (tmpvar_10 * _Time.y));
            tmpvar_11 = texture2D (_FaceTex, P_12);
            faceColor_3 = (faceColor_3 * tmpvar_11);
            highp vec2 tmpvar_13;
            tmpvar_13.x = _OutlineUVSpeedX;
            tmpvar_13.y = _OutlineUVSpeedY;
            lowp vec4 tmpvar_14;
            highp vec2 P_15;
            P_15 = (xlv_TEXCOORD5.zw + (tmpvar_13 * _Time.y));
            tmpvar_14 = texture2D (_OutlineTex, P_15);
            outlineColor_2 = (outlineColor_2 * tmpvar_14);
            mediump float d_16;
            d_16 = tmpvar_7;
            lowp vec4 faceColor_17;
            faceColor_17 = faceColor_3;
            lowp vec4 outlineColor_18;
            outlineColor_18 = outlineColor_2;
            mediump float outline_19;
            outline_19 = tmpvar_8;
            mediump float softness_20;
            softness_20 = tmpvar_9;
            mediump float tmpvar_21;
            tmpvar_21 = (1.0 - clamp ((
              ((d_16 - (outline_19 * 0.5)) + (softness_20 * 0.5))
             / 
              (1.0 + softness_20)
            ), 0.0, 1.0));
            faceColor_17.xyz = (faceColor_17.xyz * faceColor_17.w);
            outlineColor_18.xyz = (outlineColor_18.xyz * outlineColor_18.w);
            mediump vec4 tmpvar_22;
            tmpvar_22 = mix (faceColor_17, outlineColor_18, vec4((clamp (
              (d_16 + (outline_19 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_19)
            ))));
            faceColor_17 = tmpvar_22;
            faceColor_17 = (faceColor_17 * tmpvar_21);
            faceColor_3 = faceColor_17;
            mediump float x_23;
            x_23 = (faceColor_3.w - 0.001);
            if ((x_23 < 0.0)) {
              discard;
            };
            tmpvar_1 = (faceColor_3 * xlv_COLOR.w);
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
          attribute vec3 _glesNormal;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _ScreenParams;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp float _FaceDilate;
          uniform highp float _OutlineSoftness;
          uniform highp float _OutlineWidth;
          uniform highp mat4 _EnvMatrix;
          uniform highp float _WeightNormal;
          uniform highp float _WeightBold;
          uniform highp float _ScaleRatioA;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp vec4 _ClipRect;
          uniform highp float _MaskSoftnessX;
          uniform highp float _MaskSoftnessY;
          uniform highp float _GradientScale;
          uniform highp float _ScaleX;
          uniform highp float _ScaleY;
          uniform highp float _PerspectiveFilter;
          uniform highp vec4 _FaceTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec4 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD5;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = _glesColor;
            highp vec2 tmpvar_2;
            tmpvar_2 = _glesMultiTexCoord0.xy;
            highp float weight_3;
            highp float scale_4;
            highp vec2 pixelSize_5;
            highp vec4 vert_6;
            highp float tmpvar_7;
            tmpvar_7 = float((0.0 >= _glesMultiTexCoord1.y));
            vert_6.zw = _glesVertex.zw;
            vert_6.x = (_glesVertex.x + _VertexOffsetX);
            vert_6.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_8;
            highp vec4 tmpvar_9;
            tmpvar_9.w = 1.0;
            tmpvar_9.xyz = vert_6.xyz;
            tmpvar_8 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_9));
            highp vec2 tmpvar_10;
            tmpvar_10.x = _ScaleX;
            tmpvar_10.y = _ScaleY;
            highp mat2 tmpvar_11;
            tmpvar_11[0] = glstate_matrix_projection[0].xy;
            tmpvar_11[1] = glstate_matrix_projection[1].xy;
            pixelSize_5 = (tmpvar_8.ww / (tmpvar_10 * abs(
              (tmpvar_11 * _ScreenParams.xy)
            )));
            scale_4 = (inversesqrt(dot (pixelSize_5, pixelSize_5)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            if ((glstate_matrix_projection[3].w == 0.0)) {
              highp mat3 tmpvar_12;
              tmpvar_12[0] = unity_WorldToObject[0].xyz;
              tmpvar_12[1] = unity_WorldToObject[1].xyz;
              tmpvar_12[2] = unity_WorldToObject[2].xyz;
              scale_4 = mix ((abs(scale_4) * (1.0 - _PerspectiveFilter)), scale_4, abs(dot (
                normalize((_glesNormal * tmpvar_12))
              , 
                normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz))
              )));
            };
            weight_3 = (((
              (mix (_WeightNormal, _WeightBold, tmpvar_7) / 4.0)
             + _FaceDilate) * _ScaleRatioA) * 0.5);
            highp vec4 tmpvar_13;
            tmpvar_13 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_14;
            highp vec2 xlat_varoutput_15;
            xlat_varoutput_15.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_15.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_15.x));
            tmpvar_14 = (xlat_varoutput_15 * 0.001953125);
            highp vec4 tmpvar_16;
            tmpvar_16.x = (((
              ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
             / 2.0) - (0.5 / scale_4)) - weight_3);
            tmpvar_16.y = scale_4;
            tmpvar_16.z = ((0.5 - weight_3) + (0.5 / scale_4));
            tmpvar_16.w = weight_3;
            highp vec2 tmpvar_17;
            tmpvar_17.x = _MaskSoftnessX;
            tmpvar_17.y = _MaskSoftnessY;
            highp vec4 tmpvar_18;
            tmpvar_18.xy = (((vert_6.xy * 2.0) - tmpvar_13.xy) - tmpvar_13.zw);
            tmpvar_18.zw = (0.25 / ((0.25 * tmpvar_17) + pixelSize_5));
            highp mat3 tmpvar_19;
            tmpvar_19[0] = _EnvMatrix[0].xyz;
            tmpvar_19[1] = _EnvMatrix[1].xyz;
            tmpvar_19[2] = _EnvMatrix[2].xyz;
            highp vec4 tmpvar_20;
            tmpvar_20.xy = ((tmpvar_14 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            tmpvar_20.zw = ((tmpvar_14 * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            gl_Position = tmpvar_8;
            xlv_COLOR = tmpvar_1;
            xlv_TEXCOORD0 = tmpvar_2;
            xlv_TEXCOORD1 = tmpvar_16;
            xlv_TEXCOORD2 = tmpvar_18;
            xlv_TEXCOORD3 = (tmpvar_19 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz));
            xlv_TEXCOORD5 = tmpvar_20;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform sampler2D _FaceTex;
          uniform highp float _FaceUVSpeedX;
          uniform highp float _FaceUVSpeedY;
          uniform lowp vec4 _FaceColor;
          uniform highp float _OutlineSoftness;
          uniform sampler2D _OutlineTex;
          uniform highp float _OutlineUVSpeedX;
          uniform highp float _OutlineUVSpeedY;
          uniform lowp vec4 _OutlineColor;
          uniform highp float _OutlineWidth;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec4 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD5;
          void main ()
          {
            lowp vec4 tmpvar_1;
            mediump vec4 outlineColor_2;
            mediump vec4 faceColor_3;
            highp float c_4;
            lowp float tmpvar_5;
            tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0).w;
            c_4 = tmpvar_5;
            highp float x_6;
            x_6 = (c_4 - xlv_TEXCOORD1.x);
            if ((x_6 < 0.0)) {
              discard;
            };
            highp float tmpvar_7;
            tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
            highp float tmpvar_8;
            tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
            highp float tmpvar_9;
            tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
            faceColor_3 = _FaceColor;
            outlineColor_2 = _OutlineColor;
            faceColor_3.xyz = (faceColor_3.xyz * xlv_COLOR.xyz);
            highp vec2 tmpvar_10;
            tmpvar_10.x = _FaceUVSpeedX;
            tmpvar_10.y = _FaceUVSpeedY;
            lowp vec4 tmpvar_11;
            highp vec2 P_12;
            P_12 = (xlv_TEXCOORD5.xy + (tmpvar_10 * _Time.y));
            tmpvar_11 = texture2D (_FaceTex, P_12);
            faceColor_3 = (faceColor_3 * tmpvar_11);
            highp vec2 tmpvar_13;
            tmpvar_13.x = _OutlineUVSpeedX;
            tmpvar_13.y = _OutlineUVSpeedY;
            lowp vec4 tmpvar_14;
            highp vec2 P_15;
            P_15 = (xlv_TEXCOORD5.zw + (tmpvar_13 * _Time.y));
            tmpvar_14 = texture2D (_OutlineTex, P_15);
            outlineColor_2 = (outlineColor_2 * tmpvar_14);
            mediump float d_16;
            d_16 = tmpvar_7;
            lowp vec4 faceColor_17;
            faceColor_17 = faceColor_3;
            lowp vec4 outlineColor_18;
            outlineColor_18 = outlineColor_2;
            mediump float outline_19;
            outline_19 = tmpvar_8;
            mediump float softness_20;
            softness_20 = tmpvar_9;
            mediump float tmpvar_21;
            tmpvar_21 = (1.0 - clamp ((
              ((d_16 - (outline_19 * 0.5)) + (softness_20 * 0.5))
             / 
              (1.0 + softness_20)
            ), 0.0, 1.0));
            faceColor_17.xyz = (faceColor_17.xyz * faceColor_17.w);
            outlineColor_18.xyz = (outlineColor_18.xyz * outlineColor_18.w);
            mediump vec4 tmpvar_22;
            tmpvar_22 = mix (faceColor_17, outlineColor_18, vec4((clamp (
              (d_16 + (outline_19 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_19)
            ))));
            faceColor_17 = tmpvar_22;
            faceColor_17 = (faceColor_17 * tmpvar_21);
            faceColor_3 = faceColor_17;
            mediump float x_23;
            x_23 = (faceColor_3.w - 0.001);
            if ((x_23 < 0.0)) {
              discard;
            };
            tmpvar_1 = (faceColor_3 * xlv_COLOR.w);
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
          attribute vec3 _glesNormal;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _ScreenParams;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp float _FaceDilate;
          uniform highp float _OutlineSoftness;
          uniform highp float _OutlineWidth;
          uniform highp mat4 _EnvMatrix;
          uniform highp float _WeightNormal;
          uniform highp float _WeightBold;
          uniform highp float _ScaleRatioA;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp vec4 _ClipRect;
          uniform highp float _MaskSoftnessX;
          uniform highp float _MaskSoftnessY;
          uniform highp float _GradientScale;
          uniform highp float _ScaleX;
          uniform highp float _ScaleY;
          uniform highp float _PerspectiveFilter;
          uniform highp vec4 _FaceTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec4 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD5;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = _glesColor;
            highp vec2 tmpvar_2;
            tmpvar_2 = _glesMultiTexCoord0.xy;
            highp float weight_3;
            highp float scale_4;
            highp vec2 pixelSize_5;
            highp vec4 vert_6;
            highp float tmpvar_7;
            tmpvar_7 = float((0.0 >= _glesMultiTexCoord1.y));
            vert_6.zw = _glesVertex.zw;
            vert_6.x = (_glesVertex.x + _VertexOffsetX);
            vert_6.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_8;
            highp vec4 tmpvar_9;
            tmpvar_9.w = 1.0;
            tmpvar_9.xyz = vert_6.xyz;
            tmpvar_8 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_9));
            highp vec2 tmpvar_10;
            tmpvar_10.x = _ScaleX;
            tmpvar_10.y = _ScaleY;
            highp mat2 tmpvar_11;
            tmpvar_11[0] = glstate_matrix_projection[0].xy;
            tmpvar_11[1] = glstate_matrix_projection[1].xy;
            pixelSize_5 = (tmpvar_8.ww / (tmpvar_10 * abs(
              (tmpvar_11 * _ScreenParams.xy)
            )));
            scale_4 = (inversesqrt(dot (pixelSize_5, pixelSize_5)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            if ((glstate_matrix_projection[3].w == 0.0)) {
              highp mat3 tmpvar_12;
              tmpvar_12[0] = unity_WorldToObject[0].xyz;
              tmpvar_12[1] = unity_WorldToObject[1].xyz;
              tmpvar_12[2] = unity_WorldToObject[2].xyz;
              scale_4 = mix ((abs(scale_4) * (1.0 - _PerspectiveFilter)), scale_4, abs(dot (
                normalize((_glesNormal * tmpvar_12))
              , 
                normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz))
              )));
            };
            weight_3 = (((
              (mix (_WeightNormal, _WeightBold, tmpvar_7) / 4.0)
             + _FaceDilate) * _ScaleRatioA) * 0.5);
            highp vec4 tmpvar_13;
            tmpvar_13 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_14;
            highp vec2 xlat_varoutput_15;
            xlat_varoutput_15.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_15.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_15.x));
            tmpvar_14 = (xlat_varoutput_15 * 0.001953125);
            highp vec4 tmpvar_16;
            tmpvar_16.x = (((
              ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
             / 2.0) - (0.5 / scale_4)) - weight_3);
            tmpvar_16.y = scale_4;
            tmpvar_16.z = ((0.5 - weight_3) + (0.5 / scale_4));
            tmpvar_16.w = weight_3;
            highp vec2 tmpvar_17;
            tmpvar_17.x = _MaskSoftnessX;
            tmpvar_17.y = _MaskSoftnessY;
            highp vec4 tmpvar_18;
            tmpvar_18.xy = (((vert_6.xy * 2.0) - tmpvar_13.xy) - tmpvar_13.zw);
            tmpvar_18.zw = (0.25 / ((0.25 * tmpvar_17) + pixelSize_5));
            highp mat3 tmpvar_19;
            tmpvar_19[0] = _EnvMatrix[0].xyz;
            tmpvar_19[1] = _EnvMatrix[1].xyz;
            tmpvar_19[2] = _EnvMatrix[2].xyz;
            highp vec4 tmpvar_20;
            tmpvar_20.xy = ((tmpvar_14 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            tmpvar_20.zw = ((tmpvar_14 * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            gl_Position = tmpvar_8;
            xlv_COLOR = tmpvar_1;
            xlv_TEXCOORD0 = tmpvar_2;
            xlv_TEXCOORD1 = tmpvar_16;
            xlv_TEXCOORD2 = tmpvar_18;
            xlv_TEXCOORD3 = (tmpvar_19 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz));
            xlv_TEXCOORD5 = tmpvar_20;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform sampler2D _FaceTex;
          uniform highp float _FaceUVSpeedX;
          uniform highp float _FaceUVSpeedY;
          uniform lowp vec4 _FaceColor;
          uniform highp float _OutlineSoftness;
          uniform sampler2D _OutlineTex;
          uniform highp float _OutlineUVSpeedX;
          uniform highp float _OutlineUVSpeedY;
          uniform lowp vec4 _OutlineColor;
          uniform highp float _OutlineWidth;
          uniform highp float _ScaleRatioA;
          uniform highp vec4 _ClipRect;
          uniform sampler2D _MainTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec4 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec4 xlv_TEXCOORD5;
          void main ()
          {
            lowp vec4 tmpvar_1;
            mediump vec4 outlineColor_2;
            mediump vec4 faceColor_3;
            highp float c_4;
            lowp float tmpvar_5;
            tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0).w;
            c_4 = tmpvar_5;
            highp float x_6;
            x_6 = (c_4 - xlv_TEXCOORD1.x);
            if ((x_6 < 0.0)) {
              discard;
            };
            highp float tmpvar_7;
            tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
            highp float tmpvar_8;
            tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
            highp float tmpvar_9;
            tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
            faceColor_3 = _FaceColor;
            outlineColor_2 = _OutlineColor;
            faceColor_3.xyz = (faceColor_3.xyz * xlv_COLOR.xyz);
            highp vec2 tmpvar_10;
            tmpvar_10.x = _FaceUVSpeedX;
            tmpvar_10.y = _FaceUVSpeedY;
            lowp vec4 tmpvar_11;
            highp vec2 P_12;
            P_12 = (xlv_TEXCOORD5.xy + (tmpvar_10 * _Time.y));
            tmpvar_11 = texture2D (_FaceTex, P_12);
            faceColor_3 = (faceColor_3 * tmpvar_11);
            highp vec2 tmpvar_13;
            tmpvar_13.x = _OutlineUVSpeedX;
            tmpvar_13.y = _OutlineUVSpeedY;
            lowp vec4 tmpvar_14;
            highp vec2 P_15;
            P_15 = (xlv_TEXCOORD5.zw + (tmpvar_13 * _Time.y));
            tmpvar_14 = texture2D (_OutlineTex, P_15);
            outlineColor_2 = (outlineColor_2 * tmpvar_14);
            mediump float d_16;
            d_16 = tmpvar_7;
            lowp vec4 faceColor_17;
            faceColor_17 = faceColor_3;
            lowp vec4 outlineColor_18;
            outlineColor_18 = outlineColor_2;
            mediump float outline_19;
            outline_19 = tmpvar_8;
            mediump float softness_20;
            softness_20 = tmpvar_9;
            mediump float tmpvar_21;
            tmpvar_21 = (1.0 - clamp ((
              ((d_16 - (outline_19 * 0.5)) + (softness_20 * 0.5))
             / 
              (1.0 + softness_20)
            ), 0.0, 1.0));
            faceColor_17.xyz = (faceColor_17.xyz * faceColor_17.w);
            outlineColor_18.xyz = (outlineColor_18.xyz * outlineColor_18.w);
            mediump vec4 tmpvar_22;
            tmpvar_22 = mix (faceColor_17, outlineColor_18, vec4((clamp (
              (d_16 + (outline_19 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_19)
            ))));
            faceColor_17 = tmpvar_22;
            faceColor_17 = (faceColor_17 * tmpvar_21);
            faceColor_3 = faceColor_17;
            mediump vec2 tmpvar_23;
            highp vec2 tmpvar_24;
            tmpvar_24 = clamp (((
              (_ClipRect.zw - _ClipRect.xy)
             - 
              abs(xlv_TEXCOORD2.xy)
            ) * xlv_TEXCOORD2.zw), 0.0, 1.0);
            tmpvar_23 = tmpvar_24;
            faceColor_3 = (faceColor_3 * (tmpvar_23.x * tmpvar_23.y));
            tmpvar_1 = (faceColor_3 * xlv_COLOR.w);
            gl_FragData[0] = tmpvar_1;
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
          attribute vec3 _glesNormal;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _ScreenParams;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp float _FaceDilate;
          uniform highp float _OutlineSoftness;
          uniform highp float _OutlineWidth;
          uniform highp mat4 _EnvMatrix;
          uniform highp float _WeightNormal;
          uniform highp float _WeightBold;
          uniform highp float _ScaleRatioA;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp vec4 _ClipRect;
          uniform highp float _MaskSoftnessX;
          uniform highp float _MaskSoftnessY;
          uniform highp float _GradientScale;
          uniform highp float _ScaleX;
          uniform highp float _ScaleY;
          uniform highp float _PerspectiveFilter;
          uniform highp vec4 _FaceTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec4 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD5;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = _glesColor;
            highp vec2 tmpvar_2;
            tmpvar_2 = _glesMultiTexCoord0.xy;
            highp float weight_3;
            highp float scale_4;
            highp vec2 pixelSize_5;
            highp vec4 vert_6;
            highp float tmpvar_7;
            tmpvar_7 = float((0.0 >= _glesMultiTexCoord1.y));
            vert_6.zw = _glesVertex.zw;
            vert_6.x = (_glesVertex.x + _VertexOffsetX);
            vert_6.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_8;
            highp vec4 tmpvar_9;
            tmpvar_9.w = 1.0;
            tmpvar_9.xyz = vert_6.xyz;
            tmpvar_8 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_9));
            highp vec2 tmpvar_10;
            tmpvar_10.x = _ScaleX;
            tmpvar_10.y = _ScaleY;
            highp mat2 tmpvar_11;
            tmpvar_11[0] = glstate_matrix_projection[0].xy;
            tmpvar_11[1] = glstate_matrix_projection[1].xy;
            pixelSize_5 = (tmpvar_8.ww / (tmpvar_10 * abs(
              (tmpvar_11 * _ScreenParams.xy)
            )));
            scale_4 = (inversesqrt(dot (pixelSize_5, pixelSize_5)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            if ((glstate_matrix_projection[3].w == 0.0)) {
              highp mat3 tmpvar_12;
              tmpvar_12[0] = unity_WorldToObject[0].xyz;
              tmpvar_12[1] = unity_WorldToObject[1].xyz;
              tmpvar_12[2] = unity_WorldToObject[2].xyz;
              scale_4 = mix ((abs(scale_4) * (1.0 - _PerspectiveFilter)), scale_4, abs(dot (
                normalize((_glesNormal * tmpvar_12))
              , 
                normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz))
              )));
            };
            weight_3 = (((
              (mix (_WeightNormal, _WeightBold, tmpvar_7) / 4.0)
             + _FaceDilate) * _ScaleRatioA) * 0.5);
            highp vec4 tmpvar_13;
            tmpvar_13 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_14;
            highp vec2 xlat_varoutput_15;
            xlat_varoutput_15.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_15.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_15.x));
            tmpvar_14 = (xlat_varoutput_15 * 0.001953125);
            highp vec4 tmpvar_16;
            tmpvar_16.x = (((
              ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
             / 2.0) - (0.5 / scale_4)) - weight_3);
            tmpvar_16.y = scale_4;
            tmpvar_16.z = ((0.5 - weight_3) + (0.5 / scale_4));
            tmpvar_16.w = weight_3;
            highp vec2 tmpvar_17;
            tmpvar_17.x = _MaskSoftnessX;
            tmpvar_17.y = _MaskSoftnessY;
            highp vec4 tmpvar_18;
            tmpvar_18.xy = (((vert_6.xy * 2.0) - tmpvar_13.xy) - tmpvar_13.zw);
            tmpvar_18.zw = (0.25 / ((0.25 * tmpvar_17) + pixelSize_5));
            highp mat3 tmpvar_19;
            tmpvar_19[0] = _EnvMatrix[0].xyz;
            tmpvar_19[1] = _EnvMatrix[1].xyz;
            tmpvar_19[2] = _EnvMatrix[2].xyz;
            highp vec4 tmpvar_20;
            tmpvar_20.xy = ((tmpvar_14 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            tmpvar_20.zw = ((tmpvar_14 * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            gl_Position = tmpvar_8;
            xlv_COLOR = tmpvar_1;
            xlv_TEXCOORD0 = tmpvar_2;
            xlv_TEXCOORD1 = tmpvar_16;
            xlv_TEXCOORD2 = tmpvar_18;
            xlv_TEXCOORD3 = (tmpvar_19 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz));
            xlv_TEXCOORD5 = tmpvar_20;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform sampler2D _FaceTex;
          uniform highp float _FaceUVSpeedX;
          uniform highp float _FaceUVSpeedY;
          uniform lowp vec4 _FaceColor;
          uniform highp float _OutlineSoftness;
          uniform sampler2D _OutlineTex;
          uniform highp float _OutlineUVSpeedX;
          uniform highp float _OutlineUVSpeedY;
          uniform lowp vec4 _OutlineColor;
          uniform highp float _OutlineWidth;
          uniform highp float _ScaleRatioA;
          uniform highp vec4 _ClipRect;
          uniform sampler2D _MainTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec4 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec4 xlv_TEXCOORD5;
          void main ()
          {
            lowp vec4 tmpvar_1;
            mediump vec4 outlineColor_2;
            mediump vec4 faceColor_3;
            highp float c_4;
            lowp float tmpvar_5;
            tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0).w;
            c_4 = tmpvar_5;
            highp float x_6;
            x_6 = (c_4 - xlv_TEXCOORD1.x);
            if ((x_6 < 0.0)) {
              discard;
            };
            highp float tmpvar_7;
            tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
            highp float tmpvar_8;
            tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
            highp float tmpvar_9;
            tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
            faceColor_3 = _FaceColor;
            outlineColor_2 = _OutlineColor;
            faceColor_3.xyz = (faceColor_3.xyz * xlv_COLOR.xyz);
            highp vec2 tmpvar_10;
            tmpvar_10.x = _FaceUVSpeedX;
            tmpvar_10.y = _FaceUVSpeedY;
            lowp vec4 tmpvar_11;
            highp vec2 P_12;
            P_12 = (xlv_TEXCOORD5.xy + (tmpvar_10 * _Time.y));
            tmpvar_11 = texture2D (_FaceTex, P_12);
            faceColor_3 = (faceColor_3 * tmpvar_11);
            highp vec2 tmpvar_13;
            tmpvar_13.x = _OutlineUVSpeedX;
            tmpvar_13.y = _OutlineUVSpeedY;
            lowp vec4 tmpvar_14;
            highp vec2 P_15;
            P_15 = (xlv_TEXCOORD5.zw + (tmpvar_13 * _Time.y));
            tmpvar_14 = texture2D (_OutlineTex, P_15);
            outlineColor_2 = (outlineColor_2 * tmpvar_14);
            mediump float d_16;
            d_16 = tmpvar_7;
            lowp vec4 faceColor_17;
            faceColor_17 = faceColor_3;
            lowp vec4 outlineColor_18;
            outlineColor_18 = outlineColor_2;
            mediump float outline_19;
            outline_19 = tmpvar_8;
            mediump float softness_20;
            softness_20 = tmpvar_9;
            mediump float tmpvar_21;
            tmpvar_21 = (1.0 - clamp ((
              ((d_16 - (outline_19 * 0.5)) + (softness_20 * 0.5))
             / 
              (1.0 + softness_20)
            ), 0.0, 1.0));
            faceColor_17.xyz = (faceColor_17.xyz * faceColor_17.w);
            outlineColor_18.xyz = (outlineColor_18.xyz * outlineColor_18.w);
            mediump vec4 tmpvar_22;
            tmpvar_22 = mix (faceColor_17, outlineColor_18, vec4((clamp (
              (d_16 + (outline_19 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_19)
            ))));
            faceColor_17 = tmpvar_22;
            faceColor_17 = (faceColor_17 * tmpvar_21);
            faceColor_3 = faceColor_17;
            mediump vec2 tmpvar_23;
            highp vec2 tmpvar_24;
            tmpvar_24 = clamp (((
              (_ClipRect.zw - _ClipRect.xy)
             - 
              abs(xlv_TEXCOORD2.xy)
            ) * xlv_TEXCOORD2.zw), 0.0, 1.0);
            tmpvar_23 = tmpvar_24;
            faceColor_3 = (faceColor_3 * (tmpvar_23.x * tmpvar_23.y));
            tmpvar_1 = (faceColor_3 * xlv_COLOR.w);
            gl_FragData[0] = tmpvar_1;
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
          attribute vec3 _glesNormal;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _ScreenParams;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp float _FaceDilate;
          uniform highp float _OutlineSoftness;
          uniform highp float _OutlineWidth;
          uniform highp mat4 _EnvMatrix;
          uniform highp float _WeightNormal;
          uniform highp float _WeightBold;
          uniform highp float _ScaleRatioA;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp vec4 _ClipRect;
          uniform highp float _MaskSoftnessX;
          uniform highp float _MaskSoftnessY;
          uniform highp float _GradientScale;
          uniform highp float _ScaleX;
          uniform highp float _ScaleY;
          uniform highp float _PerspectiveFilter;
          uniform highp vec4 _FaceTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec4 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD5;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = _glesColor;
            highp vec2 tmpvar_2;
            tmpvar_2 = _glesMultiTexCoord0.xy;
            highp float weight_3;
            highp float scale_4;
            highp vec2 pixelSize_5;
            highp vec4 vert_6;
            highp float tmpvar_7;
            tmpvar_7 = float((0.0 >= _glesMultiTexCoord1.y));
            vert_6.zw = _glesVertex.zw;
            vert_6.x = (_glesVertex.x + _VertexOffsetX);
            vert_6.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_8;
            highp vec4 tmpvar_9;
            tmpvar_9.w = 1.0;
            tmpvar_9.xyz = vert_6.xyz;
            tmpvar_8 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_9));
            highp vec2 tmpvar_10;
            tmpvar_10.x = _ScaleX;
            tmpvar_10.y = _ScaleY;
            highp mat2 tmpvar_11;
            tmpvar_11[0] = glstate_matrix_projection[0].xy;
            tmpvar_11[1] = glstate_matrix_projection[1].xy;
            pixelSize_5 = (tmpvar_8.ww / (tmpvar_10 * abs(
              (tmpvar_11 * _ScreenParams.xy)
            )));
            scale_4 = (inversesqrt(dot (pixelSize_5, pixelSize_5)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            if ((glstate_matrix_projection[3].w == 0.0)) {
              highp mat3 tmpvar_12;
              tmpvar_12[0] = unity_WorldToObject[0].xyz;
              tmpvar_12[1] = unity_WorldToObject[1].xyz;
              tmpvar_12[2] = unity_WorldToObject[2].xyz;
              scale_4 = mix ((abs(scale_4) * (1.0 - _PerspectiveFilter)), scale_4, abs(dot (
                normalize((_glesNormal * tmpvar_12))
              , 
                normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz))
              )));
            };
            weight_3 = (((
              (mix (_WeightNormal, _WeightBold, tmpvar_7) / 4.0)
             + _FaceDilate) * _ScaleRatioA) * 0.5);
            highp vec4 tmpvar_13;
            tmpvar_13 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_14;
            highp vec2 xlat_varoutput_15;
            xlat_varoutput_15.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_15.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_15.x));
            tmpvar_14 = (xlat_varoutput_15 * 0.001953125);
            highp vec4 tmpvar_16;
            tmpvar_16.x = (((
              ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
             / 2.0) - (0.5 / scale_4)) - weight_3);
            tmpvar_16.y = scale_4;
            tmpvar_16.z = ((0.5 - weight_3) + (0.5 / scale_4));
            tmpvar_16.w = weight_3;
            highp vec2 tmpvar_17;
            tmpvar_17.x = _MaskSoftnessX;
            tmpvar_17.y = _MaskSoftnessY;
            highp vec4 tmpvar_18;
            tmpvar_18.xy = (((vert_6.xy * 2.0) - tmpvar_13.xy) - tmpvar_13.zw);
            tmpvar_18.zw = (0.25 / ((0.25 * tmpvar_17) + pixelSize_5));
            highp mat3 tmpvar_19;
            tmpvar_19[0] = _EnvMatrix[0].xyz;
            tmpvar_19[1] = _EnvMatrix[1].xyz;
            tmpvar_19[2] = _EnvMatrix[2].xyz;
            highp vec4 tmpvar_20;
            tmpvar_20.xy = ((tmpvar_14 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            tmpvar_20.zw = ((tmpvar_14 * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            gl_Position = tmpvar_8;
            xlv_COLOR = tmpvar_1;
            xlv_TEXCOORD0 = tmpvar_2;
            xlv_TEXCOORD1 = tmpvar_16;
            xlv_TEXCOORD2 = tmpvar_18;
            xlv_TEXCOORD3 = (tmpvar_19 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz));
            xlv_TEXCOORD5 = tmpvar_20;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform sampler2D _FaceTex;
          uniform highp float _FaceUVSpeedX;
          uniform highp float _FaceUVSpeedY;
          uniform lowp vec4 _FaceColor;
          uniform highp float _OutlineSoftness;
          uniform sampler2D _OutlineTex;
          uniform highp float _OutlineUVSpeedX;
          uniform highp float _OutlineUVSpeedY;
          uniform lowp vec4 _OutlineColor;
          uniform highp float _OutlineWidth;
          uniform highp float _ScaleRatioA;
          uniform highp vec4 _ClipRect;
          uniform sampler2D _MainTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec4 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec4 xlv_TEXCOORD5;
          void main ()
          {
            lowp vec4 tmpvar_1;
            mediump vec4 outlineColor_2;
            mediump vec4 faceColor_3;
            highp float c_4;
            lowp float tmpvar_5;
            tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0).w;
            c_4 = tmpvar_5;
            highp float x_6;
            x_6 = (c_4 - xlv_TEXCOORD1.x);
            if ((x_6 < 0.0)) {
              discard;
            };
            highp float tmpvar_7;
            tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
            highp float tmpvar_8;
            tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
            highp float tmpvar_9;
            tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
            faceColor_3 = _FaceColor;
            outlineColor_2 = _OutlineColor;
            faceColor_3.xyz = (faceColor_3.xyz * xlv_COLOR.xyz);
            highp vec2 tmpvar_10;
            tmpvar_10.x = _FaceUVSpeedX;
            tmpvar_10.y = _FaceUVSpeedY;
            lowp vec4 tmpvar_11;
            highp vec2 P_12;
            P_12 = (xlv_TEXCOORD5.xy + (tmpvar_10 * _Time.y));
            tmpvar_11 = texture2D (_FaceTex, P_12);
            faceColor_3 = (faceColor_3 * tmpvar_11);
            highp vec2 tmpvar_13;
            tmpvar_13.x = _OutlineUVSpeedX;
            tmpvar_13.y = _OutlineUVSpeedY;
            lowp vec4 tmpvar_14;
            highp vec2 P_15;
            P_15 = (xlv_TEXCOORD5.zw + (tmpvar_13 * _Time.y));
            tmpvar_14 = texture2D (_OutlineTex, P_15);
            outlineColor_2 = (outlineColor_2 * tmpvar_14);
            mediump float d_16;
            d_16 = tmpvar_7;
            lowp vec4 faceColor_17;
            faceColor_17 = faceColor_3;
            lowp vec4 outlineColor_18;
            outlineColor_18 = outlineColor_2;
            mediump float outline_19;
            outline_19 = tmpvar_8;
            mediump float softness_20;
            softness_20 = tmpvar_9;
            mediump float tmpvar_21;
            tmpvar_21 = (1.0 - clamp ((
              ((d_16 - (outline_19 * 0.5)) + (softness_20 * 0.5))
             / 
              (1.0 + softness_20)
            ), 0.0, 1.0));
            faceColor_17.xyz = (faceColor_17.xyz * faceColor_17.w);
            outlineColor_18.xyz = (outlineColor_18.xyz * outlineColor_18.w);
            mediump vec4 tmpvar_22;
            tmpvar_22 = mix (faceColor_17, outlineColor_18, vec4((clamp (
              (d_16 + (outline_19 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_19)
            ))));
            faceColor_17 = tmpvar_22;
            faceColor_17 = (faceColor_17 * tmpvar_21);
            faceColor_3 = faceColor_17;
            mediump vec2 tmpvar_23;
            highp vec2 tmpvar_24;
            tmpvar_24 = clamp (((
              (_ClipRect.zw - _ClipRect.xy)
             - 
              abs(xlv_TEXCOORD2.xy)
            ) * xlv_TEXCOORD2.zw), 0.0, 1.0);
            tmpvar_23 = tmpvar_24;
            faceColor_3 = (faceColor_3 * (tmpvar_23.x * tmpvar_23.y));
            tmpvar_1 = (faceColor_3 * xlv_COLOR.w);
            gl_FragData[0] = tmpvar_1;
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
          attribute vec3 _glesNormal;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _ScreenParams;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp float _FaceDilate;
          uniform highp float _OutlineSoftness;
          uniform highp float _OutlineWidth;
          uniform highp mat4 _EnvMatrix;
          uniform highp float _WeightNormal;
          uniform highp float _WeightBold;
          uniform highp float _ScaleRatioA;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp vec4 _ClipRect;
          uniform highp float _MaskSoftnessX;
          uniform highp float _MaskSoftnessY;
          uniform highp float _GradientScale;
          uniform highp float _ScaleX;
          uniform highp float _ScaleY;
          uniform highp float _PerspectiveFilter;
          uniform highp vec4 _FaceTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec4 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD5;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = _glesColor;
            highp vec2 tmpvar_2;
            tmpvar_2 = _glesMultiTexCoord0.xy;
            highp float weight_3;
            highp float scale_4;
            highp vec2 pixelSize_5;
            highp vec4 vert_6;
            highp float tmpvar_7;
            tmpvar_7 = float((0.0 >= _glesMultiTexCoord1.y));
            vert_6.zw = _glesVertex.zw;
            vert_6.x = (_glesVertex.x + _VertexOffsetX);
            vert_6.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_8;
            highp vec4 tmpvar_9;
            tmpvar_9.w = 1.0;
            tmpvar_9.xyz = vert_6.xyz;
            tmpvar_8 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_9));
            highp vec2 tmpvar_10;
            tmpvar_10.x = _ScaleX;
            tmpvar_10.y = _ScaleY;
            highp mat2 tmpvar_11;
            tmpvar_11[0] = glstate_matrix_projection[0].xy;
            tmpvar_11[1] = glstate_matrix_projection[1].xy;
            pixelSize_5 = (tmpvar_8.ww / (tmpvar_10 * abs(
              (tmpvar_11 * _ScreenParams.xy)
            )));
            scale_4 = (inversesqrt(dot (pixelSize_5, pixelSize_5)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            if ((glstate_matrix_projection[3].w == 0.0)) {
              highp mat3 tmpvar_12;
              tmpvar_12[0] = unity_WorldToObject[0].xyz;
              tmpvar_12[1] = unity_WorldToObject[1].xyz;
              tmpvar_12[2] = unity_WorldToObject[2].xyz;
              scale_4 = mix ((abs(scale_4) * (1.0 - _PerspectiveFilter)), scale_4, abs(dot (
                normalize((_glesNormal * tmpvar_12))
              , 
                normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz))
              )));
            };
            weight_3 = (((
              (mix (_WeightNormal, _WeightBold, tmpvar_7) / 4.0)
             + _FaceDilate) * _ScaleRatioA) * 0.5);
            highp vec4 tmpvar_13;
            tmpvar_13 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_14;
            highp vec2 xlat_varoutput_15;
            xlat_varoutput_15.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_15.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_15.x));
            tmpvar_14 = (xlat_varoutput_15 * 0.001953125);
            highp vec4 tmpvar_16;
            tmpvar_16.x = (((
              ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
             / 2.0) - (0.5 / scale_4)) - weight_3);
            tmpvar_16.y = scale_4;
            tmpvar_16.z = ((0.5 - weight_3) + (0.5 / scale_4));
            tmpvar_16.w = weight_3;
            highp vec2 tmpvar_17;
            tmpvar_17.x = _MaskSoftnessX;
            tmpvar_17.y = _MaskSoftnessY;
            highp vec4 tmpvar_18;
            tmpvar_18.xy = (((vert_6.xy * 2.0) - tmpvar_13.xy) - tmpvar_13.zw);
            tmpvar_18.zw = (0.25 / ((0.25 * tmpvar_17) + pixelSize_5));
            highp mat3 tmpvar_19;
            tmpvar_19[0] = _EnvMatrix[0].xyz;
            tmpvar_19[1] = _EnvMatrix[1].xyz;
            tmpvar_19[2] = _EnvMatrix[2].xyz;
            highp vec4 tmpvar_20;
            tmpvar_20.xy = ((tmpvar_14 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            tmpvar_20.zw = ((tmpvar_14 * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            gl_Position = tmpvar_8;
            xlv_COLOR = tmpvar_1;
            xlv_TEXCOORD0 = tmpvar_2;
            xlv_TEXCOORD1 = tmpvar_16;
            xlv_TEXCOORD2 = tmpvar_18;
            xlv_TEXCOORD3 = (tmpvar_19 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz));
            xlv_TEXCOORD5 = tmpvar_20;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform sampler2D _FaceTex;
          uniform highp float _FaceUVSpeedX;
          uniform highp float _FaceUVSpeedY;
          uniform lowp vec4 _FaceColor;
          uniform highp float _OutlineSoftness;
          uniform sampler2D _OutlineTex;
          uniform highp float _OutlineUVSpeedX;
          uniform highp float _OutlineUVSpeedY;
          uniform lowp vec4 _OutlineColor;
          uniform highp float _OutlineWidth;
          uniform highp float _ScaleRatioA;
          uniform highp vec4 _ClipRect;
          uniform sampler2D _MainTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec4 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec4 xlv_TEXCOORD5;
          void main ()
          {
            lowp vec4 tmpvar_1;
            mediump vec4 outlineColor_2;
            mediump vec4 faceColor_3;
            highp float c_4;
            lowp float tmpvar_5;
            tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0).w;
            c_4 = tmpvar_5;
            highp float x_6;
            x_6 = (c_4 - xlv_TEXCOORD1.x);
            if ((x_6 < 0.0)) {
              discard;
            };
            highp float tmpvar_7;
            tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
            highp float tmpvar_8;
            tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
            highp float tmpvar_9;
            tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
            faceColor_3 = _FaceColor;
            outlineColor_2 = _OutlineColor;
            faceColor_3.xyz = (faceColor_3.xyz * xlv_COLOR.xyz);
            highp vec2 tmpvar_10;
            tmpvar_10.x = _FaceUVSpeedX;
            tmpvar_10.y = _FaceUVSpeedY;
            lowp vec4 tmpvar_11;
            highp vec2 P_12;
            P_12 = (xlv_TEXCOORD5.xy + (tmpvar_10 * _Time.y));
            tmpvar_11 = texture2D (_FaceTex, P_12);
            faceColor_3 = (faceColor_3 * tmpvar_11);
            highp vec2 tmpvar_13;
            tmpvar_13.x = _OutlineUVSpeedX;
            tmpvar_13.y = _OutlineUVSpeedY;
            lowp vec4 tmpvar_14;
            highp vec2 P_15;
            P_15 = (xlv_TEXCOORD5.zw + (tmpvar_13 * _Time.y));
            tmpvar_14 = texture2D (_OutlineTex, P_15);
            outlineColor_2 = (outlineColor_2 * tmpvar_14);
            mediump float d_16;
            d_16 = tmpvar_7;
            lowp vec4 faceColor_17;
            faceColor_17 = faceColor_3;
            lowp vec4 outlineColor_18;
            outlineColor_18 = outlineColor_2;
            mediump float outline_19;
            outline_19 = tmpvar_8;
            mediump float softness_20;
            softness_20 = tmpvar_9;
            mediump float tmpvar_21;
            tmpvar_21 = (1.0 - clamp ((
              ((d_16 - (outline_19 * 0.5)) + (softness_20 * 0.5))
             / 
              (1.0 + softness_20)
            ), 0.0, 1.0));
            faceColor_17.xyz = (faceColor_17.xyz * faceColor_17.w);
            outlineColor_18.xyz = (outlineColor_18.xyz * outlineColor_18.w);
            mediump vec4 tmpvar_22;
            tmpvar_22 = mix (faceColor_17, outlineColor_18, vec4((clamp (
              (d_16 + (outline_19 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_19)
            ))));
            faceColor_17 = tmpvar_22;
            faceColor_17 = (faceColor_17 * tmpvar_21);
            faceColor_3 = faceColor_17;
            mediump vec2 tmpvar_23;
            highp vec2 tmpvar_24;
            tmpvar_24 = clamp (((
              (_ClipRect.zw - _ClipRect.xy)
             - 
              abs(xlv_TEXCOORD2.xy)
            ) * xlv_TEXCOORD2.zw), 0.0, 1.0);
            tmpvar_23 = tmpvar_24;
            faceColor_3 = (faceColor_3 * (tmpvar_23.x * tmpvar_23.y));
            mediump float x_25;
            x_25 = (faceColor_3.w - 0.001);
            if ((x_25 < 0.0)) {
              discard;
            };
            tmpvar_1 = (faceColor_3 * xlv_COLOR.w);
            gl_FragData[0] = tmpvar_1;
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
          attribute vec3 _glesNormal;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _ScreenParams;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp float _FaceDilate;
          uniform highp float _OutlineSoftness;
          uniform highp float _OutlineWidth;
          uniform highp mat4 _EnvMatrix;
          uniform highp float _WeightNormal;
          uniform highp float _WeightBold;
          uniform highp float _ScaleRatioA;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp vec4 _ClipRect;
          uniform highp float _MaskSoftnessX;
          uniform highp float _MaskSoftnessY;
          uniform highp float _GradientScale;
          uniform highp float _ScaleX;
          uniform highp float _ScaleY;
          uniform highp float _PerspectiveFilter;
          uniform highp vec4 _FaceTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec4 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD5;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = _glesColor;
            highp vec2 tmpvar_2;
            tmpvar_2 = _glesMultiTexCoord0.xy;
            highp float weight_3;
            highp float scale_4;
            highp vec2 pixelSize_5;
            highp vec4 vert_6;
            highp float tmpvar_7;
            tmpvar_7 = float((0.0 >= _glesMultiTexCoord1.y));
            vert_6.zw = _glesVertex.zw;
            vert_6.x = (_glesVertex.x + _VertexOffsetX);
            vert_6.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_8;
            highp vec4 tmpvar_9;
            tmpvar_9.w = 1.0;
            tmpvar_9.xyz = vert_6.xyz;
            tmpvar_8 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_9));
            highp vec2 tmpvar_10;
            tmpvar_10.x = _ScaleX;
            tmpvar_10.y = _ScaleY;
            highp mat2 tmpvar_11;
            tmpvar_11[0] = glstate_matrix_projection[0].xy;
            tmpvar_11[1] = glstate_matrix_projection[1].xy;
            pixelSize_5 = (tmpvar_8.ww / (tmpvar_10 * abs(
              (tmpvar_11 * _ScreenParams.xy)
            )));
            scale_4 = (inversesqrt(dot (pixelSize_5, pixelSize_5)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            if ((glstate_matrix_projection[3].w == 0.0)) {
              highp mat3 tmpvar_12;
              tmpvar_12[0] = unity_WorldToObject[0].xyz;
              tmpvar_12[1] = unity_WorldToObject[1].xyz;
              tmpvar_12[2] = unity_WorldToObject[2].xyz;
              scale_4 = mix ((abs(scale_4) * (1.0 - _PerspectiveFilter)), scale_4, abs(dot (
                normalize((_glesNormal * tmpvar_12))
              , 
                normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz))
              )));
            };
            weight_3 = (((
              (mix (_WeightNormal, _WeightBold, tmpvar_7) / 4.0)
             + _FaceDilate) * _ScaleRatioA) * 0.5);
            highp vec4 tmpvar_13;
            tmpvar_13 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_14;
            highp vec2 xlat_varoutput_15;
            xlat_varoutput_15.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_15.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_15.x));
            tmpvar_14 = (xlat_varoutput_15 * 0.001953125);
            highp vec4 tmpvar_16;
            tmpvar_16.x = (((
              ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
             / 2.0) - (0.5 / scale_4)) - weight_3);
            tmpvar_16.y = scale_4;
            tmpvar_16.z = ((0.5 - weight_3) + (0.5 / scale_4));
            tmpvar_16.w = weight_3;
            highp vec2 tmpvar_17;
            tmpvar_17.x = _MaskSoftnessX;
            tmpvar_17.y = _MaskSoftnessY;
            highp vec4 tmpvar_18;
            tmpvar_18.xy = (((vert_6.xy * 2.0) - tmpvar_13.xy) - tmpvar_13.zw);
            tmpvar_18.zw = (0.25 / ((0.25 * tmpvar_17) + pixelSize_5));
            highp mat3 tmpvar_19;
            tmpvar_19[0] = _EnvMatrix[0].xyz;
            tmpvar_19[1] = _EnvMatrix[1].xyz;
            tmpvar_19[2] = _EnvMatrix[2].xyz;
            highp vec4 tmpvar_20;
            tmpvar_20.xy = ((tmpvar_14 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            tmpvar_20.zw = ((tmpvar_14 * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            gl_Position = tmpvar_8;
            xlv_COLOR = tmpvar_1;
            xlv_TEXCOORD0 = tmpvar_2;
            xlv_TEXCOORD1 = tmpvar_16;
            xlv_TEXCOORD2 = tmpvar_18;
            xlv_TEXCOORD3 = (tmpvar_19 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz));
            xlv_TEXCOORD5 = tmpvar_20;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform sampler2D _FaceTex;
          uniform highp float _FaceUVSpeedX;
          uniform highp float _FaceUVSpeedY;
          uniform lowp vec4 _FaceColor;
          uniform highp float _OutlineSoftness;
          uniform sampler2D _OutlineTex;
          uniform highp float _OutlineUVSpeedX;
          uniform highp float _OutlineUVSpeedY;
          uniform lowp vec4 _OutlineColor;
          uniform highp float _OutlineWidth;
          uniform highp float _ScaleRatioA;
          uniform highp vec4 _ClipRect;
          uniform sampler2D _MainTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec4 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec4 xlv_TEXCOORD5;
          void main ()
          {
            lowp vec4 tmpvar_1;
            mediump vec4 outlineColor_2;
            mediump vec4 faceColor_3;
            highp float c_4;
            lowp float tmpvar_5;
            tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0).w;
            c_4 = tmpvar_5;
            highp float x_6;
            x_6 = (c_4 - xlv_TEXCOORD1.x);
            if ((x_6 < 0.0)) {
              discard;
            };
            highp float tmpvar_7;
            tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
            highp float tmpvar_8;
            tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
            highp float tmpvar_9;
            tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
            faceColor_3 = _FaceColor;
            outlineColor_2 = _OutlineColor;
            faceColor_3.xyz = (faceColor_3.xyz * xlv_COLOR.xyz);
            highp vec2 tmpvar_10;
            tmpvar_10.x = _FaceUVSpeedX;
            tmpvar_10.y = _FaceUVSpeedY;
            lowp vec4 tmpvar_11;
            highp vec2 P_12;
            P_12 = (xlv_TEXCOORD5.xy + (tmpvar_10 * _Time.y));
            tmpvar_11 = texture2D (_FaceTex, P_12);
            faceColor_3 = (faceColor_3 * tmpvar_11);
            highp vec2 tmpvar_13;
            tmpvar_13.x = _OutlineUVSpeedX;
            tmpvar_13.y = _OutlineUVSpeedY;
            lowp vec4 tmpvar_14;
            highp vec2 P_15;
            P_15 = (xlv_TEXCOORD5.zw + (tmpvar_13 * _Time.y));
            tmpvar_14 = texture2D (_OutlineTex, P_15);
            outlineColor_2 = (outlineColor_2 * tmpvar_14);
            mediump float d_16;
            d_16 = tmpvar_7;
            lowp vec4 faceColor_17;
            faceColor_17 = faceColor_3;
            lowp vec4 outlineColor_18;
            outlineColor_18 = outlineColor_2;
            mediump float outline_19;
            outline_19 = tmpvar_8;
            mediump float softness_20;
            softness_20 = tmpvar_9;
            mediump float tmpvar_21;
            tmpvar_21 = (1.0 - clamp ((
              ((d_16 - (outline_19 * 0.5)) + (softness_20 * 0.5))
             / 
              (1.0 + softness_20)
            ), 0.0, 1.0));
            faceColor_17.xyz = (faceColor_17.xyz * faceColor_17.w);
            outlineColor_18.xyz = (outlineColor_18.xyz * outlineColor_18.w);
            mediump vec4 tmpvar_22;
            tmpvar_22 = mix (faceColor_17, outlineColor_18, vec4((clamp (
              (d_16 + (outline_19 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_19)
            ))));
            faceColor_17 = tmpvar_22;
            faceColor_17 = (faceColor_17 * tmpvar_21);
            faceColor_3 = faceColor_17;
            mediump vec2 tmpvar_23;
            highp vec2 tmpvar_24;
            tmpvar_24 = clamp (((
              (_ClipRect.zw - _ClipRect.xy)
             - 
              abs(xlv_TEXCOORD2.xy)
            ) * xlv_TEXCOORD2.zw), 0.0, 1.0);
            tmpvar_23 = tmpvar_24;
            faceColor_3 = (faceColor_3 * (tmpvar_23.x * tmpvar_23.y));
            mediump float x_25;
            x_25 = (faceColor_3.w - 0.001);
            if ((x_25 < 0.0)) {
              discard;
            };
            tmpvar_1 = (faceColor_3 * xlv_COLOR.w);
            gl_FragData[0] = tmpvar_1;
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
          attribute vec3 _glesNormal;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _ScreenParams;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp float _FaceDilate;
          uniform highp float _OutlineSoftness;
          uniform highp float _OutlineWidth;
          uniform highp mat4 _EnvMatrix;
          uniform highp float _WeightNormal;
          uniform highp float _WeightBold;
          uniform highp float _ScaleRatioA;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp vec4 _ClipRect;
          uniform highp float _MaskSoftnessX;
          uniform highp float _MaskSoftnessY;
          uniform highp float _GradientScale;
          uniform highp float _ScaleX;
          uniform highp float _ScaleY;
          uniform highp float _PerspectiveFilter;
          uniform highp vec4 _FaceTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec4 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD5;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = _glesColor;
            highp vec2 tmpvar_2;
            tmpvar_2 = _glesMultiTexCoord0.xy;
            highp float weight_3;
            highp float scale_4;
            highp vec2 pixelSize_5;
            highp vec4 vert_6;
            highp float tmpvar_7;
            tmpvar_7 = float((0.0 >= _glesMultiTexCoord1.y));
            vert_6.zw = _glesVertex.zw;
            vert_6.x = (_glesVertex.x + _VertexOffsetX);
            vert_6.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_8;
            highp vec4 tmpvar_9;
            tmpvar_9.w = 1.0;
            tmpvar_9.xyz = vert_6.xyz;
            tmpvar_8 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_9));
            highp vec2 tmpvar_10;
            tmpvar_10.x = _ScaleX;
            tmpvar_10.y = _ScaleY;
            highp mat2 tmpvar_11;
            tmpvar_11[0] = glstate_matrix_projection[0].xy;
            tmpvar_11[1] = glstate_matrix_projection[1].xy;
            pixelSize_5 = (tmpvar_8.ww / (tmpvar_10 * abs(
              (tmpvar_11 * _ScreenParams.xy)
            )));
            scale_4 = (inversesqrt(dot (pixelSize_5, pixelSize_5)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            if ((glstate_matrix_projection[3].w == 0.0)) {
              highp mat3 tmpvar_12;
              tmpvar_12[0] = unity_WorldToObject[0].xyz;
              tmpvar_12[1] = unity_WorldToObject[1].xyz;
              tmpvar_12[2] = unity_WorldToObject[2].xyz;
              scale_4 = mix ((abs(scale_4) * (1.0 - _PerspectiveFilter)), scale_4, abs(dot (
                normalize((_glesNormal * tmpvar_12))
              , 
                normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz))
              )));
            };
            weight_3 = (((
              (mix (_WeightNormal, _WeightBold, tmpvar_7) / 4.0)
             + _FaceDilate) * _ScaleRatioA) * 0.5);
            highp vec4 tmpvar_13;
            tmpvar_13 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_14;
            highp vec2 xlat_varoutput_15;
            xlat_varoutput_15.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_15.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_15.x));
            tmpvar_14 = (xlat_varoutput_15 * 0.001953125);
            highp vec4 tmpvar_16;
            tmpvar_16.x = (((
              ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
             / 2.0) - (0.5 / scale_4)) - weight_3);
            tmpvar_16.y = scale_4;
            tmpvar_16.z = ((0.5 - weight_3) + (0.5 / scale_4));
            tmpvar_16.w = weight_3;
            highp vec2 tmpvar_17;
            tmpvar_17.x = _MaskSoftnessX;
            tmpvar_17.y = _MaskSoftnessY;
            highp vec4 tmpvar_18;
            tmpvar_18.xy = (((vert_6.xy * 2.0) - tmpvar_13.xy) - tmpvar_13.zw);
            tmpvar_18.zw = (0.25 / ((0.25 * tmpvar_17) + pixelSize_5));
            highp mat3 tmpvar_19;
            tmpvar_19[0] = _EnvMatrix[0].xyz;
            tmpvar_19[1] = _EnvMatrix[1].xyz;
            tmpvar_19[2] = _EnvMatrix[2].xyz;
            highp vec4 tmpvar_20;
            tmpvar_20.xy = ((tmpvar_14 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            tmpvar_20.zw = ((tmpvar_14 * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            gl_Position = tmpvar_8;
            xlv_COLOR = tmpvar_1;
            xlv_TEXCOORD0 = tmpvar_2;
            xlv_TEXCOORD1 = tmpvar_16;
            xlv_TEXCOORD2 = tmpvar_18;
            xlv_TEXCOORD3 = (tmpvar_19 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_6).xyz));
            xlv_TEXCOORD5 = tmpvar_20;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform sampler2D _FaceTex;
          uniform highp float _FaceUVSpeedX;
          uniform highp float _FaceUVSpeedY;
          uniform lowp vec4 _FaceColor;
          uniform highp float _OutlineSoftness;
          uniform sampler2D _OutlineTex;
          uniform highp float _OutlineUVSpeedX;
          uniform highp float _OutlineUVSpeedY;
          uniform lowp vec4 _OutlineColor;
          uniform highp float _OutlineWidth;
          uniform highp float _ScaleRatioA;
          uniform highp vec4 _ClipRect;
          uniform sampler2D _MainTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec2 xlv_TEXCOORD0;
          varying highp vec4 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec4 xlv_TEXCOORD5;
          void main ()
          {
            lowp vec4 tmpvar_1;
            mediump vec4 outlineColor_2;
            mediump vec4 faceColor_3;
            highp float c_4;
            lowp float tmpvar_5;
            tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0).w;
            c_4 = tmpvar_5;
            highp float x_6;
            x_6 = (c_4 - xlv_TEXCOORD1.x);
            if ((x_6 < 0.0)) {
              discard;
            };
            highp float tmpvar_7;
            tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
            highp float tmpvar_8;
            tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
            highp float tmpvar_9;
            tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
            faceColor_3 = _FaceColor;
            outlineColor_2 = _OutlineColor;
            faceColor_3.xyz = (faceColor_3.xyz * xlv_COLOR.xyz);
            highp vec2 tmpvar_10;
            tmpvar_10.x = _FaceUVSpeedX;
            tmpvar_10.y = _FaceUVSpeedY;
            lowp vec4 tmpvar_11;
            highp vec2 P_12;
            P_12 = (xlv_TEXCOORD5.xy + (tmpvar_10 * _Time.y));
            tmpvar_11 = texture2D (_FaceTex, P_12);
            faceColor_3 = (faceColor_3 * tmpvar_11);
            highp vec2 tmpvar_13;
            tmpvar_13.x = _OutlineUVSpeedX;
            tmpvar_13.y = _OutlineUVSpeedY;
            lowp vec4 tmpvar_14;
            highp vec2 P_15;
            P_15 = (xlv_TEXCOORD5.zw + (tmpvar_13 * _Time.y));
            tmpvar_14 = texture2D (_OutlineTex, P_15);
            outlineColor_2 = (outlineColor_2 * tmpvar_14);
            mediump float d_16;
            d_16 = tmpvar_7;
            lowp vec4 faceColor_17;
            faceColor_17 = faceColor_3;
            lowp vec4 outlineColor_18;
            outlineColor_18 = outlineColor_2;
            mediump float outline_19;
            outline_19 = tmpvar_8;
            mediump float softness_20;
            softness_20 = tmpvar_9;
            mediump float tmpvar_21;
            tmpvar_21 = (1.0 - clamp ((
              ((d_16 - (outline_19 * 0.5)) + (softness_20 * 0.5))
             / 
              (1.0 + softness_20)
            ), 0.0, 1.0));
            faceColor_17.xyz = (faceColor_17.xyz * faceColor_17.w);
            outlineColor_18.xyz = (outlineColor_18.xyz * outlineColor_18.w);
            mediump vec4 tmpvar_22;
            tmpvar_22 = mix (faceColor_17, outlineColor_18, vec4((clamp (
              (d_16 + (outline_19 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_19)
            ))));
            faceColor_17 = tmpvar_22;
            faceColor_17 = (faceColor_17 * tmpvar_21);
            faceColor_3 = faceColor_17;
            mediump vec2 tmpvar_23;
            highp vec2 tmpvar_24;
            tmpvar_24 = clamp (((
              (_ClipRect.zw - _ClipRect.xy)
             - 
              abs(xlv_TEXCOORD2.xy)
            ) * xlv_TEXCOORD2.zw), 0.0, 1.0);
            tmpvar_23 = tmpvar_24;
            faceColor_3 = (faceColor_3 * (tmpvar_23.x * tmpvar_23.y));
            mediump float x_25;
            x_25 = (faceColor_3.w - 0.001);
            if ((x_25 < 0.0)) {
              discard;
            };
            tmpvar_1 = (faceColor_3 * xlv_COLOR.w);
            gl_FragData[0] = tmpvar_1;
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
  FallBack "TextMeshPro/Mobile/Distance Field"
}
