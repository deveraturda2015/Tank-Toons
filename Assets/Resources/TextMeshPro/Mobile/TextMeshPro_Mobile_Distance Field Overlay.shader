Shader "TextMeshPro/Mobile/Distance Field Overlay"
{
  Properties
  {
    _FaceColor ("Face Color", Color) = (1,1,1,1)
    _FaceDilate ("Face Dilate", Range(-1, 1)) = 0
    _OutlineColor ("Outline Color", Color) = (0,0,0,1)
    _OutlineWidth ("Outline Thickness", Range(0, 1)) = 0
    _OutlineSoftness ("Outline Softness", Range(0, 1)) = 0
    _UnderlayColor ("Border Color", Color) = (0,0,0,0.5)
    _UnderlayOffsetX ("Border OffsetX", Range(-1, 1)) = 0
    _UnderlayOffsetY ("Border OffsetY", Range(-1, 1)) = 0
    _UnderlayDilate ("Border Dilate", Range(-1, 1)) = 0
    _UnderlaySoftness ("Border Softness", Range(0, 1)) = 0
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
      "QUEUE" = "Overlay"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "QUEUE" = "Overlay"
        "RenderType" = "Transparent"
      }
      ZTest Always
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
      GpuProgramID 53104
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
          uniform lowp vec4 _FaceColor;
          uniform highp float _FaceDilate;
          uniform highp float _OutlineSoftness;
          uniform lowp vec4 _OutlineColor;
          uniform highp float _OutlineWidth;
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
          varying lowp vec4 xlv_COLOR;
          varying lowp vec4 xlv_COLOR1;
          varying highp vec4 xlv_TEXCOORD0;
          varying mediump vec4 xlv_TEXCOORD1;
          varying mediump vec4 xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = _glesColor;
            highp vec2 tmpvar_2;
            tmpvar_2 = _glesMultiTexCoord0.xy;
            lowp vec4 outlineColor_3;
            lowp vec4 faceColor_4;
            highp float opacity_5;
            highp float scale_6;
            highp vec2 pixelSize_7;
            highp vec4 vert_8;
            highp float tmpvar_9;
            tmpvar_9 = float((0.0 >= _glesMultiTexCoord1.y));
            vert_8.zw = _glesVertex.zw;
            vert_8.x = (_glesVertex.x + _VertexOffsetX);
            vert_8.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_10;
            highp vec4 tmpvar_11;
            tmpvar_11.w = 1.0;
            tmpvar_11.xyz = vert_8.xyz;
            tmpvar_10 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_11));
            highp vec2 tmpvar_12;
            tmpvar_12.x = _ScaleX;
            tmpvar_12.y = _ScaleY;
            highp mat2 tmpvar_13;
            tmpvar_13[0] = glstate_matrix_projection[0].xy;
            tmpvar_13[1] = glstate_matrix_projection[1].xy;
            pixelSize_7 = (tmpvar_10.ww / (tmpvar_12 * abs(
              (tmpvar_13 * _ScreenParams.xy)
            )));
            scale_6 = (inversesqrt(dot (pixelSize_7, pixelSize_7)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            if ((glstate_matrix_projection[3].w == 0.0)) {
              highp mat3 tmpvar_14;
              tmpvar_14[0] = unity_WorldToObject[0].xyz;
              tmpvar_14[1] = unity_WorldToObject[1].xyz;
              tmpvar_14[2] = unity_WorldToObject[2].xyz;
              scale_6 = mix ((abs(scale_6) * (1.0 - _PerspectiveFilter)), scale_6, abs(dot (
                normalize((_glesNormal * tmpvar_14))
              , 
                normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_8).xyz))
              )));
            };
            scale_6 = (scale_6 / (1.0 + (
              (_OutlineSoftness * _ScaleRatioA)
             * scale_6)));
            highp float tmpvar_15;
            tmpvar_15 = (((0.5 - 
              ((((
                mix (_WeightNormal, _WeightBold, tmpvar_9)
               / 4.0) + _FaceDilate) * _ScaleRatioA) * 0.5)
            ) * scale_6) - 0.5);
            highp float tmpvar_16;
            tmpvar_16 = ((_OutlineWidth * _ScaleRatioA) * (0.5 * scale_6));
            lowp float tmpvar_17;
            tmpvar_17 = tmpvar_1.w;
            opacity_5 = tmpvar_17;
            highp vec4 tmpvar_18;
            tmpvar_18.xyz = tmpvar_1.xyz;
            tmpvar_18.w = opacity_5;
            highp vec4 tmpvar_19;
            tmpvar_19 = (tmpvar_18 * _FaceColor);
            faceColor_4 = tmpvar_19;
            faceColor_4.xyz = (faceColor_4.xyz * faceColor_4.w);
            outlineColor_3.xyz = _OutlineColor.xyz;
            outlineColor_3.w = (_OutlineColor.w * opacity_5);
            outlineColor_3.xyz = (_OutlineColor.xyz * outlineColor_3.w);
            highp vec4 tmpvar_20;
            tmpvar_20 = mix (faceColor_4, outlineColor_3, vec4(sqrt(min (1.0, 
              (tmpvar_16 * 2.0)
            ))));
            outlineColor_3 = tmpvar_20;
            highp vec4 tmpvar_21;
            tmpvar_21 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_22;
            tmpvar_22 = ((vert_8.xy - tmpvar_21.xy) / (tmpvar_21.zw - tmpvar_21.xy));
            highp vec4 tmpvar_23;
            tmpvar_23.xy = tmpvar_2;
            tmpvar_23.z = tmpvar_22.x;
            tmpvar_23.w = tmpvar_22.y;
            highp vec4 tmpvar_24;
            tmpvar_24.x = scale_6;
            tmpvar_24.y = (tmpvar_15 - tmpvar_16);
            tmpvar_24.z = (tmpvar_15 + tmpvar_16);
            tmpvar_24.w = tmpvar_15;
            highp vec2 tmpvar_25;
            tmpvar_25.x = _MaskSoftnessX;
            tmpvar_25.y = _MaskSoftnessY;
            highp vec4 tmpvar_26;
            tmpvar_26.xy = (((vert_8.xy * 2.0) - tmpvar_21.xy) - tmpvar_21.zw);
            tmpvar_26.zw = (0.25 / ((0.25 * tmpvar_25) + pixelSize_7));
            mediump vec4 tmpvar_27;
            mediump vec4 tmpvar_28;
            tmpvar_27 = tmpvar_24;
            tmpvar_28 = tmpvar_26;
            gl_Position = tmpvar_10;
            xlv_COLOR = faceColor_4;
            xlv_COLOR1 = outlineColor_3;
            xlv_TEXCOORD0 = tmpvar_23;
            xlv_TEXCOORD1 = tmpvar_27;
            xlv_TEXCOORD2 = tmpvar_28;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform sampler2D _MainTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec4 xlv_TEXCOORD0;
          varying mediump vec4 xlv_TEXCOORD1;
          void main ()
          {
            lowp vec4 tmpvar_1;
            mediump vec4 c_2;
            lowp vec4 tmpvar_3;
            tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
            mediump float tmpvar_4;
            tmpvar_4 = clamp (((tmpvar_3.w * xlv_TEXCOORD1.x) - xlv_TEXCOORD1.w), 0.0, 1.0);
            lowp vec4 tmpvar_5;
            tmpvar_5 = (xlv_COLOR * tmpvar_4);
            c_2 = tmpvar_5;
            tmpvar_1 = c_2;
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
          uniform lowp vec4 _FaceColor;
          uniform highp float _FaceDilate;
          uniform highp float _OutlineSoftness;
          uniform lowp vec4 _OutlineColor;
          uniform highp float _OutlineWidth;
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
          varying lowp vec4 xlv_COLOR;
          varying lowp vec4 xlv_COLOR1;
          varying highp vec4 xlv_TEXCOORD0;
          varying mediump vec4 xlv_TEXCOORD1;
          varying mediump vec4 xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = _glesColor;
            highp vec2 tmpvar_2;
            tmpvar_2 = _glesMultiTexCoord0.xy;
            lowp vec4 outlineColor_3;
            lowp vec4 faceColor_4;
            highp float opacity_5;
            highp float scale_6;
            highp vec2 pixelSize_7;
            highp vec4 vert_8;
            highp float tmpvar_9;
            tmpvar_9 = float((0.0 >= _glesMultiTexCoord1.y));
            vert_8.zw = _glesVertex.zw;
            vert_8.x = (_glesVertex.x + _VertexOffsetX);
            vert_8.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_10;
            highp vec4 tmpvar_11;
            tmpvar_11.w = 1.0;
            tmpvar_11.xyz = vert_8.xyz;
            tmpvar_10 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_11));
            highp vec2 tmpvar_12;
            tmpvar_12.x = _ScaleX;
            tmpvar_12.y = _ScaleY;
            highp mat2 tmpvar_13;
            tmpvar_13[0] = glstate_matrix_projection[0].xy;
            tmpvar_13[1] = glstate_matrix_projection[1].xy;
            pixelSize_7 = (tmpvar_10.ww / (tmpvar_12 * abs(
              (tmpvar_13 * _ScreenParams.xy)
            )));
            scale_6 = (inversesqrt(dot (pixelSize_7, pixelSize_7)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            if ((glstate_matrix_projection[3].w == 0.0)) {
              highp mat3 tmpvar_14;
              tmpvar_14[0] = unity_WorldToObject[0].xyz;
              tmpvar_14[1] = unity_WorldToObject[1].xyz;
              tmpvar_14[2] = unity_WorldToObject[2].xyz;
              scale_6 = mix ((abs(scale_6) * (1.0 - _PerspectiveFilter)), scale_6, abs(dot (
                normalize((_glesNormal * tmpvar_14))
              , 
                normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_8).xyz))
              )));
            };
            scale_6 = (scale_6 / (1.0 + (
              (_OutlineSoftness * _ScaleRatioA)
             * scale_6)));
            highp float tmpvar_15;
            tmpvar_15 = (((0.5 - 
              ((((
                mix (_WeightNormal, _WeightBold, tmpvar_9)
               / 4.0) + _FaceDilate) * _ScaleRatioA) * 0.5)
            ) * scale_6) - 0.5);
            highp float tmpvar_16;
            tmpvar_16 = ((_OutlineWidth * _ScaleRatioA) * (0.5 * scale_6));
            lowp float tmpvar_17;
            tmpvar_17 = tmpvar_1.w;
            opacity_5 = tmpvar_17;
            highp vec4 tmpvar_18;
            tmpvar_18.xyz = tmpvar_1.xyz;
            tmpvar_18.w = opacity_5;
            highp vec4 tmpvar_19;
            tmpvar_19 = (tmpvar_18 * _FaceColor);
            faceColor_4 = tmpvar_19;
            faceColor_4.xyz = (faceColor_4.xyz * faceColor_4.w);
            outlineColor_3.xyz = _OutlineColor.xyz;
            outlineColor_3.w = (_OutlineColor.w * opacity_5);
            outlineColor_3.xyz = (_OutlineColor.xyz * outlineColor_3.w);
            highp vec4 tmpvar_20;
            tmpvar_20 = mix (faceColor_4, outlineColor_3, vec4(sqrt(min (1.0, 
              (tmpvar_16 * 2.0)
            ))));
            outlineColor_3 = tmpvar_20;
            highp vec4 tmpvar_21;
            tmpvar_21 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_22;
            tmpvar_22 = ((vert_8.xy - tmpvar_21.xy) / (tmpvar_21.zw - tmpvar_21.xy));
            highp vec4 tmpvar_23;
            tmpvar_23.xy = tmpvar_2;
            tmpvar_23.z = tmpvar_22.x;
            tmpvar_23.w = tmpvar_22.y;
            highp vec4 tmpvar_24;
            tmpvar_24.x = scale_6;
            tmpvar_24.y = (tmpvar_15 - tmpvar_16);
            tmpvar_24.z = (tmpvar_15 + tmpvar_16);
            tmpvar_24.w = tmpvar_15;
            highp vec2 tmpvar_25;
            tmpvar_25.x = _MaskSoftnessX;
            tmpvar_25.y = _MaskSoftnessY;
            highp vec4 tmpvar_26;
            tmpvar_26.xy = (((vert_8.xy * 2.0) - tmpvar_21.xy) - tmpvar_21.zw);
            tmpvar_26.zw = (0.25 / ((0.25 * tmpvar_25) + pixelSize_7));
            mediump vec4 tmpvar_27;
            mediump vec4 tmpvar_28;
            tmpvar_27 = tmpvar_24;
            tmpvar_28 = tmpvar_26;
            gl_Position = tmpvar_10;
            xlv_COLOR = faceColor_4;
            xlv_COLOR1 = outlineColor_3;
            xlv_TEXCOORD0 = tmpvar_23;
            xlv_TEXCOORD1 = tmpvar_27;
            xlv_TEXCOORD2 = tmpvar_28;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform sampler2D _MainTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec4 xlv_TEXCOORD0;
          varying mediump vec4 xlv_TEXCOORD1;
          void main ()
          {
            lowp vec4 tmpvar_1;
            mediump vec4 c_2;
            lowp vec4 tmpvar_3;
            tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
            mediump float tmpvar_4;
            tmpvar_4 = clamp (((tmpvar_3.w * xlv_TEXCOORD1.x) - xlv_TEXCOORD1.w), 0.0, 1.0);
            lowp vec4 tmpvar_5;
            tmpvar_5 = (xlv_COLOR * tmpvar_4);
            c_2 = tmpvar_5;
            tmpvar_1 = c_2;
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
          uniform lowp vec4 _FaceColor;
          uniform highp float _FaceDilate;
          uniform highp float _OutlineSoftness;
          uniform lowp vec4 _OutlineColor;
          uniform highp float _OutlineWidth;
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
          varying lowp vec4 xlv_COLOR;
          varying lowp vec4 xlv_COLOR1;
          varying highp vec4 xlv_TEXCOORD0;
          varying mediump vec4 xlv_TEXCOORD1;
          varying mediump vec4 xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = _glesColor;
            highp vec2 tmpvar_2;
            tmpvar_2 = _glesMultiTexCoord0.xy;
            lowp vec4 outlineColor_3;
            lowp vec4 faceColor_4;
            highp float opacity_5;
            highp float scale_6;
            highp vec2 pixelSize_7;
            highp vec4 vert_8;
            highp float tmpvar_9;
            tmpvar_9 = float((0.0 >= _glesMultiTexCoord1.y));
            vert_8.zw = _glesVertex.zw;
            vert_8.x = (_glesVertex.x + _VertexOffsetX);
            vert_8.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_10;
            highp vec4 tmpvar_11;
            tmpvar_11.w = 1.0;
            tmpvar_11.xyz = vert_8.xyz;
            tmpvar_10 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_11));
            highp vec2 tmpvar_12;
            tmpvar_12.x = _ScaleX;
            tmpvar_12.y = _ScaleY;
            highp mat2 tmpvar_13;
            tmpvar_13[0] = glstate_matrix_projection[0].xy;
            tmpvar_13[1] = glstate_matrix_projection[1].xy;
            pixelSize_7 = (tmpvar_10.ww / (tmpvar_12 * abs(
              (tmpvar_13 * _ScreenParams.xy)
            )));
            scale_6 = (inversesqrt(dot (pixelSize_7, pixelSize_7)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            if ((glstate_matrix_projection[3].w == 0.0)) {
              highp mat3 tmpvar_14;
              tmpvar_14[0] = unity_WorldToObject[0].xyz;
              tmpvar_14[1] = unity_WorldToObject[1].xyz;
              tmpvar_14[2] = unity_WorldToObject[2].xyz;
              scale_6 = mix ((abs(scale_6) * (1.0 - _PerspectiveFilter)), scale_6, abs(dot (
                normalize((_glesNormal * tmpvar_14))
              , 
                normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_8).xyz))
              )));
            };
            scale_6 = (scale_6 / (1.0 + (
              (_OutlineSoftness * _ScaleRatioA)
             * scale_6)));
            highp float tmpvar_15;
            tmpvar_15 = (((0.5 - 
              ((((
                mix (_WeightNormal, _WeightBold, tmpvar_9)
               / 4.0) + _FaceDilate) * _ScaleRatioA) * 0.5)
            ) * scale_6) - 0.5);
            highp float tmpvar_16;
            tmpvar_16 = ((_OutlineWidth * _ScaleRatioA) * (0.5 * scale_6));
            lowp float tmpvar_17;
            tmpvar_17 = tmpvar_1.w;
            opacity_5 = tmpvar_17;
            highp vec4 tmpvar_18;
            tmpvar_18.xyz = tmpvar_1.xyz;
            tmpvar_18.w = opacity_5;
            highp vec4 tmpvar_19;
            tmpvar_19 = (tmpvar_18 * _FaceColor);
            faceColor_4 = tmpvar_19;
            faceColor_4.xyz = (faceColor_4.xyz * faceColor_4.w);
            outlineColor_3.xyz = _OutlineColor.xyz;
            outlineColor_3.w = (_OutlineColor.w * opacity_5);
            outlineColor_3.xyz = (_OutlineColor.xyz * outlineColor_3.w);
            highp vec4 tmpvar_20;
            tmpvar_20 = mix (faceColor_4, outlineColor_3, vec4(sqrt(min (1.0, 
              (tmpvar_16 * 2.0)
            ))));
            outlineColor_3 = tmpvar_20;
            highp vec4 tmpvar_21;
            tmpvar_21 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_22;
            tmpvar_22 = ((vert_8.xy - tmpvar_21.xy) / (tmpvar_21.zw - tmpvar_21.xy));
            highp vec4 tmpvar_23;
            tmpvar_23.xy = tmpvar_2;
            tmpvar_23.z = tmpvar_22.x;
            tmpvar_23.w = tmpvar_22.y;
            highp vec4 tmpvar_24;
            tmpvar_24.x = scale_6;
            tmpvar_24.y = (tmpvar_15 - tmpvar_16);
            tmpvar_24.z = (tmpvar_15 + tmpvar_16);
            tmpvar_24.w = tmpvar_15;
            highp vec2 tmpvar_25;
            tmpvar_25.x = _MaskSoftnessX;
            tmpvar_25.y = _MaskSoftnessY;
            highp vec4 tmpvar_26;
            tmpvar_26.xy = (((vert_8.xy * 2.0) - tmpvar_21.xy) - tmpvar_21.zw);
            tmpvar_26.zw = (0.25 / ((0.25 * tmpvar_25) + pixelSize_7));
            mediump vec4 tmpvar_27;
            mediump vec4 tmpvar_28;
            tmpvar_27 = tmpvar_24;
            tmpvar_28 = tmpvar_26;
            gl_Position = tmpvar_10;
            xlv_COLOR = faceColor_4;
            xlv_COLOR1 = outlineColor_3;
            xlv_TEXCOORD0 = tmpvar_23;
            xlv_TEXCOORD1 = tmpvar_27;
            xlv_TEXCOORD2 = tmpvar_28;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform sampler2D _MainTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec4 xlv_TEXCOORD0;
          varying mediump vec4 xlv_TEXCOORD1;
          void main ()
          {
            lowp vec4 tmpvar_1;
            mediump vec4 c_2;
            lowp vec4 tmpvar_3;
            tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
            mediump float tmpvar_4;
            tmpvar_4 = clamp (((tmpvar_3.w * xlv_TEXCOORD1.x) - xlv_TEXCOORD1.w), 0.0, 1.0);
            lowp vec4 tmpvar_5;
            tmpvar_5 = (xlv_COLOR * tmpvar_4);
            c_2 = tmpvar_5;
            tmpvar_1 = c_2;
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
          uniform lowp vec4 _FaceColor;
          uniform highp float _FaceDilate;
          uniform highp float _OutlineSoftness;
          uniform lowp vec4 _OutlineColor;
          uniform highp float _OutlineWidth;
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
          varying lowp vec4 xlv_COLOR;
          varying lowp vec4 xlv_COLOR1;
          varying highp vec4 xlv_TEXCOORD0;
          varying mediump vec4 xlv_TEXCOORD1;
          varying mediump vec4 xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = _glesColor;
            highp vec2 tmpvar_2;
            tmpvar_2 = _glesMultiTexCoord0.xy;
            lowp vec4 outlineColor_3;
            lowp vec4 faceColor_4;
            highp float opacity_5;
            highp float scale_6;
            highp vec2 pixelSize_7;
            highp vec4 vert_8;
            highp float tmpvar_9;
            tmpvar_9 = float((0.0 >= _glesMultiTexCoord1.y));
            vert_8.zw = _glesVertex.zw;
            vert_8.x = (_glesVertex.x + _VertexOffsetX);
            vert_8.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_10;
            highp vec4 tmpvar_11;
            tmpvar_11.w = 1.0;
            tmpvar_11.xyz = vert_8.xyz;
            tmpvar_10 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_11));
            highp vec2 tmpvar_12;
            tmpvar_12.x = _ScaleX;
            tmpvar_12.y = _ScaleY;
            highp mat2 tmpvar_13;
            tmpvar_13[0] = glstate_matrix_projection[0].xy;
            tmpvar_13[1] = glstate_matrix_projection[1].xy;
            pixelSize_7 = (tmpvar_10.ww / (tmpvar_12 * abs(
              (tmpvar_13 * _ScreenParams.xy)
            )));
            scale_6 = (inversesqrt(dot (pixelSize_7, pixelSize_7)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            if ((glstate_matrix_projection[3].w == 0.0)) {
              highp mat3 tmpvar_14;
              tmpvar_14[0] = unity_WorldToObject[0].xyz;
              tmpvar_14[1] = unity_WorldToObject[1].xyz;
              tmpvar_14[2] = unity_WorldToObject[2].xyz;
              scale_6 = mix ((abs(scale_6) * (1.0 - _PerspectiveFilter)), scale_6, abs(dot (
                normalize((_glesNormal * tmpvar_14))
              , 
                normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_8).xyz))
              )));
            };
            scale_6 = (scale_6 / (1.0 + (
              (_OutlineSoftness * _ScaleRatioA)
             * scale_6)));
            highp float tmpvar_15;
            tmpvar_15 = (((0.5 - 
              ((((
                mix (_WeightNormal, _WeightBold, tmpvar_9)
               / 4.0) + _FaceDilate) * _ScaleRatioA) * 0.5)
            ) * scale_6) - 0.5);
            highp float tmpvar_16;
            tmpvar_16 = ((_OutlineWidth * _ScaleRatioA) * (0.5 * scale_6));
            lowp float tmpvar_17;
            tmpvar_17 = tmpvar_1.w;
            opacity_5 = tmpvar_17;
            highp vec4 tmpvar_18;
            tmpvar_18.xyz = tmpvar_1.xyz;
            tmpvar_18.w = opacity_5;
            highp vec4 tmpvar_19;
            tmpvar_19 = (tmpvar_18 * _FaceColor);
            faceColor_4 = tmpvar_19;
            faceColor_4.xyz = (faceColor_4.xyz * faceColor_4.w);
            outlineColor_3.xyz = _OutlineColor.xyz;
            outlineColor_3.w = (_OutlineColor.w * opacity_5);
            outlineColor_3.xyz = (_OutlineColor.xyz * outlineColor_3.w);
            highp vec4 tmpvar_20;
            tmpvar_20 = mix (faceColor_4, outlineColor_3, vec4(sqrt(min (1.0, 
              (tmpvar_16 * 2.0)
            ))));
            outlineColor_3 = tmpvar_20;
            highp vec4 tmpvar_21;
            tmpvar_21 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_22;
            tmpvar_22 = ((vert_8.xy - tmpvar_21.xy) / (tmpvar_21.zw - tmpvar_21.xy));
            highp vec4 tmpvar_23;
            tmpvar_23.xy = tmpvar_2;
            tmpvar_23.z = tmpvar_22.x;
            tmpvar_23.w = tmpvar_22.y;
            highp vec4 tmpvar_24;
            tmpvar_24.x = scale_6;
            tmpvar_24.y = (tmpvar_15 - tmpvar_16);
            tmpvar_24.z = (tmpvar_15 + tmpvar_16);
            tmpvar_24.w = tmpvar_15;
            highp vec2 tmpvar_25;
            tmpvar_25.x = _MaskSoftnessX;
            tmpvar_25.y = _MaskSoftnessY;
            highp vec4 tmpvar_26;
            tmpvar_26.xy = (((vert_8.xy * 2.0) - tmpvar_21.xy) - tmpvar_21.zw);
            tmpvar_26.zw = (0.25 / ((0.25 * tmpvar_25) + pixelSize_7));
            mediump vec4 tmpvar_27;
            mediump vec4 tmpvar_28;
            tmpvar_27 = tmpvar_24;
            tmpvar_28 = tmpvar_26;
            gl_Position = tmpvar_10;
            xlv_COLOR = faceColor_4;
            xlv_COLOR1 = outlineColor_3;
            xlv_TEXCOORD0 = tmpvar_23;
            xlv_TEXCOORD1 = tmpvar_27;
            xlv_TEXCOORD2 = tmpvar_28;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform sampler2D _MainTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec4 xlv_TEXCOORD0;
          varying mediump vec4 xlv_TEXCOORD1;
          void main ()
          {
            lowp vec4 tmpvar_1;
            mediump vec4 c_2;
            lowp vec4 tmpvar_3;
            tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
            mediump float tmpvar_4;
            tmpvar_4 = clamp (((tmpvar_3.w * xlv_TEXCOORD1.x) - xlv_TEXCOORD1.w), 0.0, 1.0);
            lowp vec4 tmpvar_5;
            tmpvar_5 = (xlv_COLOR * tmpvar_4);
            c_2 = tmpvar_5;
            mediump float x_6;
            x_6 = (c_2.w - 0.001);
            if ((x_6 < 0.0)) {
              discard;
            };
            tmpvar_1 = c_2;
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
          uniform lowp vec4 _FaceColor;
          uniform highp float _FaceDilate;
          uniform highp float _OutlineSoftness;
          uniform lowp vec4 _OutlineColor;
          uniform highp float _OutlineWidth;
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
          varying lowp vec4 xlv_COLOR;
          varying lowp vec4 xlv_COLOR1;
          varying highp vec4 xlv_TEXCOORD0;
          varying mediump vec4 xlv_TEXCOORD1;
          varying mediump vec4 xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = _glesColor;
            highp vec2 tmpvar_2;
            tmpvar_2 = _glesMultiTexCoord0.xy;
            lowp vec4 outlineColor_3;
            lowp vec4 faceColor_4;
            highp float opacity_5;
            highp float scale_6;
            highp vec2 pixelSize_7;
            highp vec4 vert_8;
            highp float tmpvar_9;
            tmpvar_9 = float((0.0 >= _glesMultiTexCoord1.y));
            vert_8.zw = _glesVertex.zw;
            vert_8.x = (_glesVertex.x + _VertexOffsetX);
            vert_8.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_10;
            highp vec4 tmpvar_11;
            tmpvar_11.w = 1.0;
            tmpvar_11.xyz = vert_8.xyz;
            tmpvar_10 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_11));
            highp vec2 tmpvar_12;
            tmpvar_12.x = _ScaleX;
            tmpvar_12.y = _ScaleY;
            highp mat2 tmpvar_13;
            tmpvar_13[0] = glstate_matrix_projection[0].xy;
            tmpvar_13[1] = glstate_matrix_projection[1].xy;
            pixelSize_7 = (tmpvar_10.ww / (tmpvar_12 * abs(
              (tmpvar_13 * _ScreenParams.xy)
            )));
            scale_6 = (inversesqrt(dot (pixelSize_7, pixelSize_7)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            if ((glstate_matrix_projection[3].w == 0.0)) {
              highp mat3 tmpvar_14;
              tmpvar_14[0] = unity_WorldToObject[0].xyz;
              tmpvar_14[1] = unity_WorldToObject[1].xyz;
              tmpvar_14[2] = unity_WorldToObject[2].xyz;
              scale_6 = mix ((abs(scale_6) * (1.0 - _PerspectiveFilter)), scale_6, abs(dot (
                normalize((_glesNormal * tmpvar_14))
              , 
                normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_8).xyz))
              )));
            };
            scale_6 = (scale_6 / (1.0 + (
              (_OutlineSoftness * _ScaleRatioA)
             * scale_6)));
            highp float tmpvar_15;
            tmpvar_15 = (((0.5 - 
              ((((
                mix (_WeightNormal, _WeightBold, tmpvar_9)
               / 4.0) + _FaceDilate) * _ScaleRatioA) * 0.5)
            ) * scale_6) - 0.5);
            highp float tmpvar_16;
            tmpvar_16 = ((_OutlineWidth * _ScaleRatioA) * (0.5 * scale_6));
            lowp float tmpvar_17;
            tmpvar_17 = tmpvar_1.w;
            opacity_5 = tmpvar_17;
            highp vec4 tmpvar_18;
            tmpvar_18.xyz = tmpvar_1.xyz;
            tmpvar_18.w = opacity_5;
            highp vec4 tmpvar_19;
            tmpvar_19 = (tmpvar_18 * _FaceColor);
            faceColor_4 = tmpvar_19;
            faceColor_4.xyz = (faceColor_4.xyz * faceColor_4.w);
            outlineColor_3.xyz = _OutlineColor.xyz;
            outlineColor_3.w = (_OutlineColor.w * opacity_5);
            outlineColor_3.xyz = (_OutlineColor.xyz * outlineColor_3.w);
            highp vec4 tmpvar_20;
            tmpvar_20 = mix (faceColor_4, outlineColor_3, vec4(sqrt(min (1.0, 
              (tmpvar_16 * 2.0)
            ))));
            outlineColor_3 = tmpvar_20;
            highp vec4 tmpvar_21;
            tmpvar_21 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_22;
            tmpvar_22 = ((vert_8.xy - tmpvar_21.xy) / (tmpvar_21.zw - tmpvar_21.xy));
            highp vec4 tmpvar_23;
            tmpvar_23.xy = tmpvar_2;
            tmpvar_23.z = tmpvar_22.x;
            tmpvar_23.w = tmpvar_22.y;
            highp vec4 tmpvar_24;
            tmpvar_24.x = scale_6;
            tmpvar_24.y = (tmpvar_15 - tmpvar_16);
            tmpvar_24.z = (tmpvar_15 + tmpvar_16);
            tmpvar_24.w = tmpvar_15;
            highp vec2 tmpvar_25;
            tmpvar_25.x = _MaskSoftnessX;
            tmpvar_25.y = _MaskSoftnessY;
            highp vec4 tmpvar_26;
            tmpvar_26.xy = (((vert_8.xy * 2.0) - tmpvar_21.xy) - tmpvar_21.zw);
            tmpvar_26.zw = (0.25 / ((0.25 * tmpvar_25) + pixelSize_7));
            mediump vec4 tmpvar_27;
            mediump vec4 tmpvar_28;
            tmpvar_27 = tmpvar_24;
            tmpvar_28 = tmpvar_26;
            gl_Position = tmpvar_10;
            xlv_COLOR = faceColor_4;
            xlv_COLOR1 = outlineColor_3;
            xlv_TEXCOORD0 = tmpvar_23;
            xlv_TEXCOORD1 = tmpvar_27;
            xlv_TEXCOORD2 = tmpvar_28;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform sampler2D _MainTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec4 xlv_TEXCOORD0;
          varying mediump vec4 xlv_TEXCOORD1;
          void main ()
          {
            lowp vec4 tmpvar_1;
            mediump vec4 c_2;
            lowp vec4 tmpvar_3;
            tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
            mediump float tmpvar_4;
            tmpvar_4 = clamp (((tmpvar_3.w * xlv_TEXCOORD1.x) - xlv_TEXCOORD1.w), 0.0, 1.0);
            lowp vec4 tmpvar_5;
            tmpvar_5 = (xlv_COLOR * tmpvar_4);
            c_2 = tmpvar_5;
            mediump float x_6;
            x_6 = (c_2.w - 0.001);
            if ((x_6 < 0.0)) {
              discard;
            };
            tmpvar_1 = c_2;
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
          uniform lowp vec4 _FaceColor;
          uniform highp float _FaceDilate;
          uniform highp float _OutlineSoftness;
          uniform lowp vec4 _OutlineColor;
          uniform highp float _OutlineWidth;
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
          varying lowp vec4 xlv_COLOR;
          varying lowp vec4 xlv_COLOR1;
          varying highp vec4 xlv_TEXCOORD0;
          varying mediump vec4 xlv_TEXCOORD1;
          varying mediump vec4 xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = _glesColor;
            highp vec2 tmpvar_2;
            tmpvar_2 = _glesMultiTexCoord0.xy;
            lowp vec4 outlineColor_3;
            lowp vec4 faceColor_4;
            highp float opacity_5;
            highp float scale_6;
            highp vec2 pixelSize_7;
            highp vec4 vert_8;
            highp float tmpvar_9;
            tmpvar_9 = float((0.0 >= _glesMultiTexCoord1.y));
            vert_8.zw = _glesVertex.zw;
            vert_8.x = (_glesVertex.x + _VertexOffsetX);
            vert_8.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_10;
            highp vec4 tmpvar_11;
            tmpvar_11.w = 1.0;
            tmpvar_11.xyz = vert_8.xyz;
            tmpvar_10 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_11));
            highp vec2 tmpvar_12;
            tmpvar_12.x = _ScaleX;
            tmpvar_12.y = _ScaleY;
            highp mat2 tmpvar_13;
            tmpvar_13[0] = glstate_matrix_projection[0].xy;
            tmpvar_13[1] = glstate_matrix_projection[1].xy;
            pixelSize_7 = (tmpvar_10.ww / (tmpvar_12 * abs(
              (tmpvar_13 * _ScreenParams.xy)
            )));
            scale_6 = (inversesqrt(dot (pixelSize_7, pixelSize_7)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            if ((glstate_matrix_projection[3].w == 0.0)) {
              highp mat3 tmpvar_14;
              tmpvar_14[0] = unity_WorldToObject[0].xyz;
              tmpvar_14[1] = unity_WorldToObject[1].xyz;
              tmpvar_14[2] = unity_WorldToObject[2].xyz;
              scale_6 = mix ((abs(scale_6) * (1.0 - _PerspectiveFilter)), scale_6, abs(dot (
                normalize((_glesNormal * tmpvar_14))
              , 
                normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_8).xyz))
              )));
            };
            scale_6 = (scale_6 / (1.0 + (
              (_OutlineSoftness * _ScaleRatioA)
             * scale_6)));
            highp float tmpvar_15;
            tmpvar_15 = (((0.5 - 
              ((((
                mix (_WeightNormal, _WeightBold, tmpvar_9)
               / 4.0) + _FaceDilate) * _ScaleRatioA) * 0.5)
            ) * scale_6) - 0.5);
            highp float tmpvar_16;
            tmpvar_16 = ((_OutlineWidth * _ScaleRatioA) * (0.5 * scale_6));
            lowp float tmpvar_17;
            tmpvar_17 = tmpvar_1.w;
            opacity_5 = tmpvar_17;
            highp vec4 tmpvar_18;
            tmpvar_18.xyz = tmpvar_1.xyz;
            tmpvar_18.w = opacity_5;
            highp vec4 tmpvar_19;
            tmpvar_19 = (tmpvar_18 * _FaceColor);
            faceColor_4 = tmpvar_19;
            faceColor_4.xyz = (faceColor_4.xyz * faceColor_4.w);
            outlineColor_3.xyz = _OutlineColor.xyz;
            outlineColor_3.w = (_OutlineColor.w * opacity_5);
            outlineColor_3.xyz = (_OutlineColor.xyz * outlineColor_3.w);
            highp vec4 tmpvar_20;
            tmpvar_20 = mix (faceColor_4, outlineColor_3, vec4(sqrt(min (1.0, 
              (tmpvar_16 * 2.0)
            ))));
            outlineColor_3 = tmpvar_20;
            highp vec4 tmpvar_21;
            tmpvar_21 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_22;
            tmpvar_22 = ((vert_8.xy - tmpvar_21.xy) / (tmpvar_21.zw - tmpvar_21.xy));
            highp vec4 tmpvar_23;
            tmpvar_23.xy = tmpvar_2;
            tmpvar_23.z = tmpvar_22.x;
            tmpvar_23.w = tmpvar_22.y;
            highp vec4 tmpvar_24;
            tmpvar_24.x = scale_6;
            tmpvar_24.y = (tmpvar_15 - tmpvar_16);
            tmpvar_24.z = (tmpvar_15 + tmpvar_16);
            tmpvar_24.w = tmpvar_15;
            highp vec2 tmpvar_25;
            tmpvar_25.x = _MaskSoftnessX;
            tmpvar_25.y = _MaskSoftnessY;
            highp vec4 tmpvar_26;
            tmpvar_26.xy = (((vert_8.xy * 2.0) - tmpvar_21.xy) - tmpvar_21.zw);
            tmpvar_26.zw = (0.25 / ((0.25 * tmpvar_25) + pixelSize_7));
            mediump vec4 tmpvar_27;
            mediump vec4 tmpvar_28;
            tmpvar_27 = tmpvar_24;
            tmpvar_28 = tmpvar_26;
            gl_Position = tmpvar_10;
            xlv_COLOR = faceColor_4;
            xlv_COLOR1 = outlineColor_3;
            xlv_TEXCOORD0 = tmpvar_23;
            xlv_TEXCOORD1 = tmpvar_27;
            xlv_TEXCOORD2 = tmpvar_28;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform sampler2D _MainTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec4 xlv_TEXCOORD0;
          varying mediump vec4 xlv_TEXCOORD1;
          void main ()
          {
            lowp vec4 tmpvar_1;
            mediump vec4 c_2;
            lowp vec4 tmpvar_3;
            tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
            mediump float tmpvar_4;
            tmpvar_4 = clamp (((tmpvar_3.w * xlv_TEXCOORD1.x) - xlv_TEXCOORD1.w), 0.0, 1.0);
            lowp vec4 tmpvar_5;
            tmpvar_5 = (xlv_COLOR * tmpvar_4);
            c_2 = tmpvar_5;
            mediump float x_6;
            x_6 = (c_2.w - 0.001);
            if ((x_6 < 0.0)) {
              discard;
            };
            tmpvar_1 = c_2;
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
          uniform lowp vec4 _FaceColor;
          uniform highp float _FaceDilate;
          uniform highp float _OutlineSoftness;
          uniform lowp vec4 _OutlineColor;
          uniform highp float _OutlineWidth;
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
          varying lowp vec4 xlv_COLOR;
          varying lowp vec4 xlv_COLOR1;
          varying highp vec4 xlv_TEXCOORD0;
          varying mediump vec4 xlv_TEXCOORD1;
          varying mediump vec4 xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = _glesColor;
            highp vec2 tmpvar_2;
            tmpvar_2 = _glesMultiTexCoord0.xy;
            lowp vec4 outlineColor_3;
            lowp vec4 faceColor_4;
            highp float opacity_5;
            highp float scale_6;
            highp vec2 pixelSize_7;
            highp vec4 vert_8;
            highp float tmpvar_9;
            tmpvar_9 = float((0.0 >= _glesMultiTexCoord1.y));
            vert_8.zw = _glesVertex.zw;
            vert_8.x = (_glesVertex.x + _VertexOffsetX);
            vert_8.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_10;
            highp vec4 tmpvar_11;
            tmpvar_11.w = 1.0;
            tmpvar_11.xyz = vert_8.xyz;
            tmpvar_10 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_11));
            highp vec2 tmpvar_12;
            tmpvar_12.x = _ScaleX;
            tmpvar_12.y = _ScaleY;
            highp mat2 tmpvar_13;
            tmpvar_13[0] = glstate_matrix_projection[0].xy;
            tmpvar_13[1] = glstate_matrix_projection[1].xy;
            pixelSize_7 = (tmpvar_10.ww / (tmpvar_12 * abs(
              (tmpvar_13 * _ScreenParams.xy)
            )));
            scale_6 = (inversesqrt(dot (pixelSize_7, pixelSize_7)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            if ((glstate_matrix_projection[3].w == 0.0)) {
              highp mat3 tmpvar_14;
              tmpvar_14[0] = unity_WorldToObject[0].xyz;
              tmpvar_14[1] = unity_WorldToObject[1].xyz;
              tmpvar_14[2] = unity_WorldToObject[2].xyz;
              scale_6 = mix ((abs(scale_6) * (1.0 - _PerspectiveFilter)), scale_6, abs(dot (
                normalize((_glesNormal * tmpvar_14))
              , 
                normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_8).xyz))
              )));
            };
            scale_6 = (scale_6 / (1.0 + (
              (_OutlineSoftness * _ScaleRatioA)
             * scale_6)));
            highp float tmpvar_15;
            tmpvar_15 = (((0.5 - 
              ((((
                mix (_WeightNormal, _WeightBold, tmpvar_9)
               / 4.0) + _FaceDilate) * _ScaleRatioA) * 0.5)
            ) * scale_6) - 0.5);
            highp float tmpvar_16;
            tmpvar_16 = ((_OutlineWidth * _ScaleRatioA) * (0.5 * scale_6));
            lowp float tmpvar_17;
            tmpvar_17 = tmpvar_1.w;
            opacity_5 = tmpvar_17;
            highp vec4 tmpvar_18;
            tmpvar_18.xyz = tmpvar_1.xyz;
            tmpvar_18.w = opacity_5;
            highp vec4 tmpvar_19;
            tmpvar_19 = (tmpvar_18 * _FaceColor);
            faceColor_4 = tmpvar_19;
            faceColor_4.xyz = (faceColor_4.xyz * faceColor_4.w);
            outlineColor_3.xyz = _OutlineColor.xyz;
            outlineColor_3.w = (_OutlineColor.w * opacity_5);
            outlineColor_3.xyz = (_OutlineColor.xyz * outlineColor_3.w);
            highp vec4 tmpvar_20;
            tmpvar_20 = mix (faceColor_4, outlineColor_3, vec4(sqrt(min (1.0, 
              (tmpvar_16 * 2.0)
            ))));
            outlineColor_3 = tmpvar_20;
            highp vec4 tmpvar_21;
            tmpvar_21 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_22;
            tmpvar_22 = ((vert_8.xy - tmpvar_21.xy) / (tmpvar_21.zw - tmpvar_21.xy));
            highp vec4 tmpvar_23;
            tmpvar_23.xy = tmpvar_2;
            tmpvar_23.z = tmpvar_22.x;
            tmpvar_23.w = tmpvar_22.y;
            highp vec4 tmpvar_24;
            tmpvar_24.x = scale_6;
            tmpvar_24.y = (tmpvar_15 - tmpvar_16);
            tmpvar_24.z = (tmpvar_15 + tmpvar_16);
            tmpvar_24.w = tmpvar_15;
            highp vec2 tmpvar_25;
            tmpvar_25.x = _MaskSoftnessX;
            tmpvar_25.y = _MaskSoftnessY;
            highp vec4 tmpvar_26;
            tmpvar_26.xy = (((vert_8.xy * 2.0) - tmpvar_21.xy) - tmpvar_21.zw);
            tmpvar_26.zw = (0.25 / ((0.25 * tmpvar_25) + pixelSize_7));
            mediump vec4 tmpvar_27;
            mediump vec4 tmpvar_28;
            tmpvar_27 = tmpvar_24;
            tmpvar_28 = tmpvar_26;
            gl_Position = tmpvar_10;
            xlv_COLOR = faceColor_4;
            xlv_COLOR1 = outlineColor_3;
            xlv_TEXCOORD0 = tmpvar_23;
            xlv_TEXCOORD1 = tmpvar_27;
            xlv_TEXCOORD2 = tmpvar_28;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _ClipRect;
          uniform sampler2D _MainTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec4 xlv_TEXCOORD0;
          varying mediump vec4 xlv_TEXCOORD1;
          varying mediump vec4 xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 tmpvar_1;
            mediump vec4 c_2;
            lowp vec4 tmpvar_3;
            tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
            mediump float tmpvar_4;
            tmpvar_4 = clamp (((tmpvar_3.w * xlv_TEXCOORD1.x) - xlv_TEXCOORD1.w), 0.0, 1.0);
            lowp vec4 tmpvar_5;
            tmpvar_5 = (xlv_COLOR * tmpvar_4);
            c_2 = tmpvar_5;
            mediump vec2 tmpvar_6;
            tmpvar_6 = abs(xlv_TEXCOORD2.xy);
            mediump vec2 tmpvar_7;
            highp vec2 tmpvar_8;
            tmpvar_8 = clamp (((
              (_ClipRect.zw - _ClipRect.xy)
             - tmpvar_6) * xlv_TEXCOORD2.zw), 0.0, 1.0);
            tmpvar_7 = tmpvar_8;
            c_2 = (c_2 * (tmpvar_7.x * tmpvar_7.y));
            tmpvar_1 = c_2;
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
          uniform lowp vec4 _FaceColor;
          uniform highp float _FaceDilate;
          uniform highp float _OutlineSoftness;
          uniform lowp vec4 _OutlineColor;
          uniform highp float _OutlineWidth;
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
          varying lowp vec4 xlv_COLOR;
          varying lowp vec4 xlv_COLOR1;
          varying highp vec4 xlv_TEXCOORD0;
          varying mediump vec4 xlv_TEXCOORD1;
          varying mediump vec4 xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = _glesColor;
            highp vec2 tmpvar_2;
            tmpvar_2 = _glesMultiTexCoord0.xy;
            lowp vec4 outlineColor_3;
            lowp vec4 faceColor_4;
            highp float opacity_5;
            highp float scale_6;
            highp vec2 pixelSize_7;
            highp vec4 vert_8;
            highp float tmpvar_9;
            tmpvar_9 = float((0.0 >= _glesMultiTexCoord1.y));
            vert_8.zw = _glesVertex.zw;
            vert_8.x = (_glesVertex.x + _VertexOffsetX);
            vert_8.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_10;
            highp vec4 tmpvar_11;
            tmpvar_11.w = 1.0;
            tmpvar_11.xyz = vert_8.xyz;
            tmpvar_10 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_11));
            highp vec2 tmpvar_12;
            tmpvar_12.x = _ScaleX;
            tmpvar_12.y = _ScaleY;
            highp mat2 tmpvar_13;
            tmpvar_13[0] = glstate_matrix_projection[0].xy;
            tmpvar_13[1] = glstate_matrix_projection[1].xy;
            pixelSize_7 = (tmpvar_10.ww / (tmpvar_12 * abs(
              (tmpvar_13 * _ScreenParams.xy)
            )));
            scale_6 = (inversesqrt(dot (pixelSize_7, pixelSize_7)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            if ((glstate_matrix_projection[3].w == 0.0)) {
              highp mat3 tmpvar_14;
              tmpvar_14[0] = unity_WorldToObject[0].xyz;
              tmpvar_14[1] = unity_WorldToObject[1].xyz;
              tmpvar_14[2] = unity_WorldToObject[2].xyz;
              scale_6 = mix ((abs(scale_6) * (1.0 - _PerspectiveFilter)), scale_6, abs(dot (
                normalize((_glesNormal * tmpvar_14))
              , 
                normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_8).xyz))
              )));
            };
            scale_6 = (scale_6 / (1.0 + (
              (_OutlineSoftness * _ScaleRatioA)
             * scale_6)));
            highp float tmpvar_15;
            tmpvar_15 = (((0.5 - 
              ((((
                mix (_WeightNormal, _WeightBold, tmpvar_9)
               / 4.0) + _FaceDilate) * _ScaleRatioA) * 0.5)
            ) * scale_6) - 0.5);
            highp float tmpvar_16;
            tmpvar_16 = ((_OutlineWidth * _ScaleRatioA) * (0.5 * scale_6));
            lowp float tmpvar_17;
            tmpvar_17 = tmpvar_1.w;
            opacity_5 = tmpvar_17;
            highp vec4 tmpvar_18;
            tmpvar_18.xyz = tmpvar_1.xyz;
            tmpvar_18.w = opacity_5;
            highp vec4 tmpvar_19;
            tmpvar_19 = (tmpvar_18 * _FaceColor);
            faceColor_4 = tmpvar_19;
            faceColor_4.xyz = (faceColor_4.xyz * faceColor_4.w);
            outlineColor_3.xyz = _OutlineColor.xyz;
            outlineColor_3.w = (_OutlineColor.w * opacity_5);
            outlineColor_3.xyz = (_OutlineColor.xyz * outlineColor_3.w);
            highp vec4 tmpvar_20;
            tmpvar_20 = mix (faceColor_4, outlineColor_3, vec4(sqrt(min (1.0, 
              (tmpvar_16 * 2.0)
            ))));
            outlineColor_3 = tmpvar_20;
            highp vec4 tmpvar_21;
            tmpvar_21 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_22;
            tmpvar_22 = ((vert_8.xy - tmpvar_21.xy) / (tmpvar_21.zw - tmpvar_21.xy));
            highp vec4 tmpvar_23;
            tmpvar_23.xy = tmpvar_2;
            tmpvar_23.z = tmpvar_22.x;
            tmpvar_23.w = tmpvar_22.y;
            highp vec4 tmpvar_24;
            tmpvar_24.x = scale_6;
            tmpvar_24.y = (tmpvar_15 - tmpvar_16);
            tmpvar_24.z = (tmpvar_15 + tmpvar_16);
            tmpvar_24.w = tmpvar_15;
            highp vec2 tmpvar_25;
            tmpvar_25.x = _MaskSoftnessX;
            tmpvar_25.y = _MaskSoftnessY;
            highp vec4 tmpvar_26;
            tmpvar_26.xy = (((vert_8.xy * 2.0) - tmpvar_21.xy) - tmpvar_21.zw);
            tmpvar_26.zw = (0.25 / ((0.25 * tmpvar_25) + pixelSize_7));
            mediump vec4 tmpvar_27;
            mediump vec4 tmpvar_28;
            tmpvar_27 = tmpvar_24;
            tmpvar_28 = tmpvar_26;
            gl_Position = tmpvar_10;
            xlv_COLOR = faceColor_4;
            xlv_COLOR1 = outlineColor_3;
            xlv_TEXCOORD0 = tmpvar_23;
            xlv_TEXCOORD1 = tmpvar_27;
            xlv_TEXCOORD2 = tmpvar_28;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _ClipRect;
          uniform sampler2D _MainTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec4 xlv_TEXCOORD0;
          varying mediump vec4 xlv_TEXCOORD1;
          varying mediump vec4 xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 tmpvar_1;
            mediump vec4 c_2;
            lowp vec4 tmpvar_3;
            tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
            mediump float tmpvar_4;
            tmpvar_4 = clamp (((tmpvar_3.w * xlv_TEXCOORD1.x) - xlv_TEXCOORD1.w), 0.0, 1.0);
            lowp vec4 tmpvar_5;
            tmpvar_5 = (xlv_COLOR * tmpvar_4);
            c_2 = tmpvar_5;
            mediump vec2 tmpvar_6;
            tmpvar_6 = abs(xlv_TEXCOORD2.xy);
            mediump vec2 tmpvar_7;
            highp vec2 tmpvar_8;
            tmpvar_8 = clamp (((
              (_ClipRect.zw - _ClipRect.xy)
             - tmpvar_6) * xlv_TEXCOORD2.zw), 0.0, 1.0);
            tmpvar_7 = tmpvar_8;
            c_2 = (c_2 * (tmpvar_7.x * tmpvar_7.y));
            tmpvar_1 = c_2;
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
          uniform lowp vec4 _FaceColor;
          uniform highp float _FaceDilate;
          uniform highp float _OutlineSoftness;
          uniform lowp vec4 _OutlineColor;
          uniform highp float _OutlineWidth;
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
          varying lowp vec4 xlv_COLOR;
          varying lowp vec4 xlv_COLOR1;
          varying highp vec4 xlv_TEXCOORD0;
          varying mediump vec4 xlv_TEXCOORD1;
          varying mediump vec4 xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = _glesColor;
            highp vec2 tmpvar_2;
            tmpvar_2 = _glesMultiTexCoord0.xy;
            lowp vec4 outlineColor_3;
            lowp vec4 faceColor_4;
            highp float opacity_5;
            highp float scale_6;
            highp vec2 pixelSize_7;
            highp vec4 vert_8;
            highp float tmpvar_9;
            tmpvar_9 = float((0.0 >= _glesMultiTexCoord1.y));
            vert_8.zw = _glesVertex.zw;
            vert_8.x = (_glesVertex.x + _VertexOffsetX);
            vert_8.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_10;
            highp vec4 tmpvar_11;
            tmpvar_11.w = 1.0;
            tmpvar_11.xyz = vert_8.xyz;
            tmpvar_10 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_11));
            highp vec2 tmpvar_12;
            tmpvar_12.x = _ScaleX;
            tmpvar_12.y = _ScaleY;
            highp mat2 tmpvar_13;
            tmpvar_13[0] = glstate_matrix_projection[0].xy;
            tmpvar_13[1] = glstate_matrix_projection[1].xy;
            pixelSize_7 = (tmpvar_10.ww / (tmpvar_12 * abs(
              (tmpvar_13 * _ScreenParams.xy)
            )));
            scale_6 = (inversesqrt(dot (pixelSize_7, pixelSize_7)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            if ((glstate_matrix_projection[3].w == 0.0)) {
              highp mat3 tmpvar_14;
              tmpvar_14[0] = unity_WorldToObject[0].xyz;
              tmpvar_14[1] = unity_WorldToObject[1].xyz;
              tmpvar_14[2] = unity_WorldToObject[2].xyz;
              scale_6 = mix ((abs(scale_6) * (1.0 - _PerspectiveFilter)), scale_6, abs(dot (
                normalize((_glesNormal * tmpvar_14))
              , 
                normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_8).xyz))
              )));
            };
            scale_6 = (scale_6 / (1.0 + (
              (_OutlineSoftness * _ScaleRatioA)
             * scale_6)));
            highp float tmpvar_15;
            tmpvar_15 = (((0.5 - 
              ((((
                mix (_WeightNormal, _WeightBold, tmpvar_9)
               / 4.0) + _FaceDilate) * _ScaleRatioA) * 0.5)
            ) * scale_6) - 0.5);
            highp float tmpvar_16;
            tmpvar_16 = ((_OutlineWidth * _ScaleRatioA) * (0.5 * scale_6));
            lowp float tmpvar_17;
            tmpvar_17 = tmpvar_1.w;
            opacity_5 = tmpvar_17;
            highp vec4 tmpvar_18;
            tmpvar_18.xyz = tmpvar_1.xyz;
            tmpvar_18.w = opacity_5;
            highp vec4 tmpvar_19;
            tmpvar_19 = (tmpvar_18 * _FaceColor);
            faceColor_4 = tmpvar_19;
            faceColor_4.xyz = (faceColor_4.xyz * faceColor_4.w);
            outlineColor_3.xyz = _OutlineColor.xyz;
            outlineColor_3.w = (_OutlineColor.w * opacity_5);
            outlineColor_3.xyz = (_OutlineColor.xyz * outlineColor_3.w);
            highp vec4 tmpvar_20;
            tmpvar_20 = mix (faceColor_4, outlineColor_3, vec4(sqrt(min (1.0, 
              (tmpvar_16 * 2.0)
            ))));
            outlineColor_3 = tmpvar_20;
            highp vec4 tmpvar_21;
            tmpvar_21 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_22;
            tmpvar_22 = ((vert_8.xy - tmpvar_21.xy) / (tmpvar_21.zw - tmpvar_21.xy));
            highp vec4 tmpvar_23;
            tmpvar_23.xy = tmpvar_2;
            tmpvar_23.z = tmpvar_22.x;
            tmpvar_23.w = tmpvar_22.y;
            highp vec4 tmpvar_24;
            tmpvar_24.x = scale_6;
            tmpvar_24.y = (tmpvar_15 - tmpvar_16);
            tmpvar_24.z = (tmpvar_15 + tmpvar_16);
            tmpvar_24.w = tmpvar_15;
            highp vec2 tmpvar_25;
            tmpvar_25.x = _MaskSoftnessX;
            tmpvar_25.y = _MaskSoftnessY;
            highp vec4 tmpvar_26;
            tmpvar_26.xy = (((vert_8.xy * 2.0) - tmpvar_21.xy) - tmpvar_21.zw);
            tmpvar_26.zw = (0.25 / ((0.25 * tmpvar_25) + pixelSize_7));
            mediump vec4 tmpvar_27;
            mediump vec4 tmpvar_28;
            tmpvar_27 = tmpvar_24;
            tmpvar_28 = tmpvar_26;
            gl_Position = tmpvar_10;
            xlv_COLOR = faceColor_4;
            xlv_COLOR1 = outlineColor_3;
            xlv_TEXCOORD0 = tmpvar_23;
            xlv_TEXCOORD1 = tmpvar_27;
            xlv_TEXCOORD2 = tmpvar_28;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _ClipRect;
          uniform sampler2D _MainTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec4 xlv_TEXCOORD0;
          varying mediump vec4 xlv_TEXCOORD1;
          varying mediump vec4 xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 tmpvar_1;
            mediump vec4 c_2;
            lowp vec4 tmpvar_3;
            tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
            mediump float tmpvar_4;
            tmpvar_4 = clamp (((tmpvar_3.w * xlv_TEXCOORD1.x) - xlv_TEXCOORD1.w), 0.0, 1.0);
            lowp vec4 tmpvar_5;
            tmpvar_5 = (xlv_COLOR * tmpvar_4);
            c_2 = tmpvar_5;
            mediump vec2 tmpvar_6;
            tmpvar_6 = abs(xlv_TEXCOORD2.xy);
            mediump vec2 tmpvar_7;
            highp vec2 tmpvar_8;
            tmpvar_8 = clamp (((
              (_ClipRect.zw - _ClipRect.xy)
             - tmpvar_6) * xlv_TEXCOORD2.zw), 0.0, 1.0);
            tmpvar_7 = tmpvar_8;
            c_2 = (c_2 * (tmpvar_7.x * tmpvar_7.y));
            tmpvar_1 = c_2;
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
          uniform lowp vec4 _FaceColor;
          uniform highp float _FaceDilate;
          uniform highp float _OutlineSoftness;
          uniform lowp vec4 _OutlineColor;
          uniform highp float _OutlineWidth;
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
          varying lowp vec4 xlv_COLOR;
          varying lowp vec4 xlv_COLOR1;
          varying highp vec4 xlv_TEXCOORD0;
          varying mediump vec4 xlv_TEXCOORD1;
          varying mediump vec4 xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = _glesColor;
            highp vec2 tmpvar_2;
            tmpvar_2 = _glesMultiTexCoord0.xy;
            lowp vec4 outlineColor_3;
            lowp vec4 faceColor_4;
            highp float opacity_5;
            highp float scale_6;
            highp vec2 pixelSize_7;
            highp vec4 vert_8;
            highp float tmpvar_9;
            tmpvar_9 = float((0.0 >= _glesMultiTexCoord1.y));
            vert_8.zw = _glesVertex.zw;
            vert_8.x = (_glesVertex.x + _VertexOffsetX);
            vert_8.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_10;
            highp vec4 tmpvar_11;
            tmpvar_11.w = 1.0;
            tmpvar_11.xyz = vert_8.xyz;
            tmpvar_10 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_11));
            highp vec2 tmpvar_12;
            tmpvar_12.x = _ScaleX;
            tmpvar_12.y = _ScaleY;
            highp mat2 tmpvar_13;
            tmpvar_13[0] = glstate_matrix_projection[0].xy;
            tmpvar_13[1] = glstate_matrix_projection[1].xy;
            pixelSize_7 = (tmpvar_10.ww / (tmpvar_12 * abs(
              (tmpvar_13 * _ScreenParams.xy)
            )));
            scale_6 = (inversesqrt(dot (pixelSize_7, pixelSize_7)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            if ((glstate_matrix_projection[3].w == 0.0)) {
              highp mat3 tmpvar_14;
              tmpvar_14[0] = unity_WorldToObject[0].xyz;
              tmpvar_14[1] = unity_WorldToObject[1].xyz;
              tmpvar_14[2] = unity_WorldToObject[2].xyz;
              scale_6 = mix ((abs(scale_6) * (1.0 - _PerspectiveFilter)), scale_6, abs(dot (
                normalize((_glesNormal * tmpvar_14))
              , 
                normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_8).xyz))
              )));
            };
            scale_6 = (scale_6 / (1.0 + (
              (_OutlineSoftness * _ScaleRatioA)
             * scale_6)));
            highp float tmpvar_15;
            tmpvar_15 = (((0.5 - 
              ((((
                mix (_WeightNormal, _WeightBold, tmpvar_9)
               / 4.0) + _FaceDilate) * _ScaleRatioA) * 0.5)
            ) * scale_6) - 0.5);
            highp float tmpvar_16;
            tmpvar_16 = ((_OutlineWidth * _ScaleRatioA) * (0.5 * scale_6));
            lowp float tmpvar_17;
            tmpvar_17 = tmpvar_1.w;
            opacity_5 = tmpvar_17;
            highp vec4 tmpvar_18;
            tmpvar_18.xyz = tmpvar_1.xyz;
            tmpvar_18.w = opacity_5;
            highp vec4 tmpvar_19;
            tmpvar_19 = (tmpvar_18 * _FaceColor);
            faceColor_4 = tmpvar_19;
            faceColor_4.xyz = (faceColor_4.xyz * faceColor_4.w);
            outlineColor_3.xyz = _OutlineColor.xyz;
            outlineColor_3.w = (_OutlineColor.w * opacity_5);
            outlineColor_3.xyz = (_OutlineColor.xyz * outlineColor_3.w);
            highp vec4 tmpvar_20;
            tmpvar_20 = mix (faceColor_4, outlineColor_3, vec4(sqrt(min (1.0, 
              (tmpvar_16 * 2.0)
            ))));
            outlineColor_3 = tmpvar_20;
            highp vec4 tmpvar_21;
            tmpvar_21 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_22;
            tmpvar_22 = ((vert_8.xy - tmpvar_21.xy) / (tmpvar_21.zw - tmpvar_21.xy));
            highp vec4 tmpvar_23;
            tmpvar_23.xy = tmpvar_2;
            tmpvar_23.z = tmpvar_22.x;
            tmpvar_23.w = tmpvar_22.y;
            highp vec4 tmpvar_24;
            tmpvar_24.x = scale_6;
            tmpvar_24.y = (tmpvar_15 - tmpvar_16);
            tmpvar_24.z = (tmpvar_15 + tmpvar_16);
            tmpvar_24.w = tmpvar_15;
            highp vec2 tmpvar_25;
            tmpvar_25.x = _MaskSoftnessX;
            tmpvar_25.y = _MaskSoftnessY;
            highp vec4 tmpvar_26;
            tmpvar_26.xy = (((vert_8.xy * 2.0) - tmpvar_21.xy) - tmpvar_21.zw);
            tmpvar_26.zw = (0.25 / ((0.25 * tmpvar_25) + pixelSize_7));
            mediump vec4 tmpvar_27;
            mediump vec4 tmpvar_28;
            tmpvar_27 = tmpvar_24;
            tmpvar_28 = tmpvar_26;
            gl_Position = tmpvar_10;
            xlv_COLOR = faceColor_4;
            xlv_COLOR1 = outlineColor_3;
            xlv_TEXCOORD0 = tmpvar_23;
            xlv_TEXCOORD1 = tmpvar_27;
            xlv_TEXCOORD2 = tmpvar_28;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _ClipRect;
          uniform sampler2D _MainTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec4 xlv_TEXCOORD0;
          varying mediump vec4 xlv_TEXCOORD1;
          varying mediump vec4 xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 tmpvar_1;
            mediump vec4 c_2;
            lowp vec4 tmpvar_3;
            tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
            mediump float tmpvar_4;
            tmpvar_4 = clamp (((tmpvar_3.w * xlv_TEXCOORD1.x) - xlv_TEXCOORD1.w), 0.0, 1.0);
            lowp vec4 tmpvar_5;
            tmpvar_5 = (xlv_COLOR * tmpvar_4);
            c_2 = tmpvar_5;
            mediump vec2 tmpvar_6;
            tmpvar_6 = abs(xlv_TEXCOORD2.xy);
            mediump vec2 tmpvar_7;
            highp vec2 tmpvar_8;
            tmpvar_8 = clamp (((
              (_ClipRect.zw - _ClipRect.xy)
             - tmpvar_6) * xlv_TEXCOORD2.zw), 0.0, 1.0);
            tmpvar_7 = tmpvar_8;
            c_2 = (c_2 * (tmpvar_7.x * tmpvar_7.y));
            mediump float x_9;
            x_9 = (c_2.w - 0.001);
            if ((x_9 < 0.0)) {
              discard;
            };
            tmpvar_1 = c_2;
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
          uniform lowp vec4 _FaceColor;
          uniform highp float _FaceDilate;
          uniform highp float _OutlineSoftness;
          uniform lowp vec4 _OutlineColor;
          uniform highp float _OutlineWidth;
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
          varying lowp vec4 xlv_COLOR;
          varying lowp vec4 xlv_COLOR1;
          varying highp vec4 xlv_TEXCOORD0;
          varying mediump vec4 xlv_TEXCOORD1;
          varying mediump vec4 xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = _glesColor;
            highp vec2 tmpvar_2;
            tmpvar_2 = _glesMultiTexCoord0.xy;
            lowp vec4 outlineColor_3;
            lowp vec4 faceColor_4;
            highp float opacity_5;
            highp float scale_6;
            highp vec2 pixelSize_7;
            highp vec4 vert_8;
            highp float tmpvar_9;
            tmpvar_9 = float((0.0 >= _glesMultiTexCoord1.y));
            vert_8.zw = _glesVertex.zw;
            vert_8.x = (_glesVertex.x + _VertexOffsetX);
            vert_8.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_10;
            highp vec4 tmpvar_11;
            tmpvar_11.w = 1.0;
            tmpvar_11.xyz = vert_8.xyz;
            tmpvar_10 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_11));
            highp vec2 tmpvar_12;
            tmpvar_12.x = _ScaleX;
            tmpvar_12.y = _ScaleY;
            highp mat2 tmpvar_13;
            tmpvar_13[0] = glstate_matrix_projection[0].xy;
            tmpvar_13[1] = glstate_matrix_projection[1].xy;
            pixelSize_7 = (tmpvar_10.ww / (tmpvar_12 * abs(
              (tmpvar_13 * _ScreenParams.xy)
            )));
            scale_6 = (inversesqrt(dot (pixelSize_7, pixelSize_7)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            if ((glstate_matrix_projection[3].w == 0.0)) {
              highp mat3 tmpvar_14;
              tmpvar_14[0] = unity_WorldToObject[0].xyz;
              tmpvar_14[1] = unity_WorldToObject[1].xyz;
              tmpvar_14[2] = unity_WorldToObject[2].xyz;
              scale_6 = mix ((abs(scale_6) * (1.0 - _PerspectiveFilter)), scale_6, abs(dot (
                normalize((_glesNormal * tmpvar_14))
              , 
                normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_8).xyz))
              )));
            };
            scale_6 = (scale_6 / (1.0 + (
              (_OutlineSoftness * _ScaleRatioA)
             * scale_6)));
            highp float tmpvar_15;
            tmpvar_15 = (((0.5 - 
              ((((
                mix (_WeightNormal, _WeightBold, tmpvar_9)
               / 4.0) + _FaceDilate) * _ScaleRatioA) * 0.5)
            ) * scale_6) - 0.5);
            highp float tmpvar_16;
            tmpvar_16 = ((_OutlineWidth * _ScaleRatioA) * (0.5 * scale_6));
            lowp float tmpvar_17;
            tmpvar_17 = tmpvar_1.w;
            opacity_5 = tmpvar_17;
            highp vec4 tmpvar_18;
            tmpvar_18.xyz = tmpvar_1.xyz;
            tmpvar_18.w = opacity_5;
            highp vec4 tmpvar_19;
            tmpvar_19 = (tmpvar_18 * _FaceColor);
            faceColor_4 = tmpvar_19;
            faceColor_4.xyz = (faceColor_4.xyz * faceColor_4.w);
            outlineColor_3.xyz = _OutlineColor.xyz;
            outlineColor_3.w = (_OutlineColor.w * opacity_5);
            outlineColor_3.xyz = (_OutlineColor.xyz * outlineColor_3.w);
            highp vec4 tmpvar_20;
            tmpvar_20 = mix (faceColor_4, outlineColor_3, vec4(sqrt(min (1.0, 
              (tmpvar_16 * 2.0)
            ))));
            outlineColor_3 = tmpvar_20;
            highp vec4 tmpvar_21;
            tmpvar_21 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_22;
            tmpvar_22 = ((vert_8.xy - tmpvar_21.xy) / (tmpvar_21.zw - tmpvar_21.xy));
            highp vec4 tmpvar_23;
            tmpvar_23.xy = tmpvar_2;
            tmpvar_23.z = tmpvar_22.x;
            tmpvar_23.w = tmpvar_22.y;
            highp vec4 tmpvar_24;
            tmpvar_24.x = scale_6;
            tmpvar_24.y = (tmpvar_15 - tmpvar_16);
            tmpvar_24.z = (tmpvar_15 + tmpvar_16);
            tmpvar_24.w = tmpvar_15;
            highp vec2 tmpvar_25;
            tmpvar_25.x = _MaskSoftnessX;
            tmpvar_25.y = _MaskSoftnessY;
            highp vec4 tmpvar_26;
            tmpvar_26.xy = (((vert_8.xy * 2.0) - tmpvar_21.xy) - tmpvar_21.zw);
            tmpvar_26.zw = (0.25 / ((0.25 * tmpvar_25) + pixelSize_7));
            mediump vec4 tmpvar_27;
            mediump vec4 tmpvar_28;
            tmpvar_27 = tmpvar_24;
            tmpvar_28 = tmpvar_26;
            gl_Position = tmpvar_10;
            xlv_COLOR = faceColor_4;
            xlv_COLOR1 = outlineColor_3;
            xlv_TEXCOORD0 = tmpvar_23;
            xlv_TEXCOORD1 = tmpvar_27;
            xlv_TEXCOORD2 = tmpvar_28;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _ClipRect;
          uniform sampler2D _MainTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec4 xlv_TEXCOORD0;
          varying mediump vec4 xlv_TEXCOORD1;
          varying mediump vec4 xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 tmpvar_1;
            mediump vec4 c_2;
            lowp vec4 tmpvar_3;
            tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
            mediump float tmpvar_4;
            tmpvar_4 = clamp (((tmpvar_3.w * xlv_TEXCOORD1.x) - xlv_TEXCOORD1.w), 0.0, 1.0);
            lowp vec4 tmpvar_5;
            tmpvar_5 = (xlv_COLOR * tmpvar_4);
            c_2 = tmpvar_5;
            mediump vec2 tmpvar_6;
            tmpvar_6 = abs(xlv_TEXCOORD2.xy);
            mediump vec2 tmpvar_7;
            highp vec2 tmpvar_8;
            tmpvar_8 = clamp (((
              (_ClipRect.zw - _ClipRect.xy)
             - tmpvar_6) * xlv_TEXCOORD2.zw), 0.0, 1.0);
            tmpvar_7 = tmpvar_8;
            c_2 = (c_2 * (tmpvar_7.x * tmpvar_7.y));
            mediump float x_9;
            x_9 = (c_2.w - 0.001);
            if ((x_9 < 0.0)) {
              discard;
            };
            tmpvar_1 = c_2;
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
          uniform lowp vec4 _FaceColor;
          uniform highp float _FaceDilate;
          uniform highp float _OutlineSoftness;
          uniform lowp vec4 _OutlineColor;
          uniform highp float _OutlineWidth;
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
          varying lowp vec4 xlv_COLOR;
          varying lowp vec4 xlv_COLOR1;
          varying highp vec4 xlv_TEXCOORD0;
          varying mediump vec4 xlv_TEXCOORD1;
          varying mediump vec4 xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = _glesColor;
            highp vec2 tmpvar_2;
            tmpvar_2 = _glesMultiTexCoord0.xy;
            lowp vec4 outlineColor_3;
            lowp vec4 faceColor_4;
            highp float opacity_5;
            highp float scale_6;
            highp vec2 pixelSize_7;
            highp vec4 vert_8;
            highp float tmpvar_9;
            tmpvar_9 = float((0.0 >= _glesMultiTexCoord1.y));
            vert_8.zw = _glesVertex.zw;
            vert_8.x = (_glesVertex.x + _VertexOffsetX);
            vert_8.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_10;
            highp vec4 tmpvar_11;
            tmpvar_11.w = 1.0;
            tmpvar_11.xyz = vert_8.xyz;
            tmpvar_10 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_11));
            highp vec2 tmpvar_12;
            tmpvar_12.x = _ScaleX;
            tmpvar_12.y = _ScaleY;
            highp mat2 tmpvar_13;
            tmpvar_13[0] = glstate_matrix_projection[0].xy;
            tmpvar_13[1] = glstate_matrix_projection[1].xy;
            pixelSize_7 = (tmpvar_10.ww / (tmpvar_12 * abs(
              (tmpvar_13 * _ScreenParams.xy)
            )));
            scale_6 = (inversesqrt(dot (pixelSize_7, pixelSize_7)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            if ((glstate_matrix_projection[3].w == 0.0)) {
              highp mat3 tmpvar_14;
              tmpvar_14[0] = unity_WorldToObject[0].xyz;
              tmpvar_14[1] = unity_WorldToObject[1].xyz;
              tmpvar_14[2] = unity_WorldToObject[2].xyz;
              scale_6 = mix ((abs(scale_6) * (1.0 - _PerspectiveFilter)), scale_6, abs(dot (
                normalize((_glesNormal * tmpvar_14))
              , 
                normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * vert_8).xyz))
              )));
            };
            scale_6 = (scale_6 / (1.0 + (
              (_OutlineSoftness * _ScaleRatioA)
             * scale_6)));
            highp float tmpvar_15;
            tmpvar_15 = (((0.5 - 
              ((((
                mix (_WeightNormal, _WeightBold, tmpvar_9)
               / 4.0) + _FaceDilate) * _ScaleRatioA) * 0.5)
            ) * scale_6) - 0.5);
            highp float tmpvar_16;
            tmpvar_16 = ((_OutlineWidth * _ScaleRatioA) * (0.5 * scale_6));
            lowp float tmpvar_17;
            tmpvar_17 = tmpvar_1.w;
            opacity_5 = tmpvar_17;
            highp vec4 tmpvar_18;
            tmpvar_18.xyz = tmpvar_1.xyz;
            tmpvar_18.w = opacity_5;
            highp vec4 tmpvar_19;
            tmpvar_19 = (tmpvar_18 * _FaceColor);
            faceColor_4 = tmpvar_19;
            faceColor_4.xyz = (faceColor_4.xyz * faceColor_4.w);
            outlineColor_3.xyz = _OutlineColor.xyz;
            outlineColor_3.w = (_OutlineColor.w * opacity_5);
            outlineColor_3.xyz = (_OutlineColor.xyz * outlineColor_3.w);
            highp vec4 tmpvar_20;
            tmpvar_20 = mix (faceColor_4, outlineColor_3, vec4(sqrt(min (1.0, 
              (tmpvar_16 * 2.0)
            ))));
            outlineColor_3 = tmpvar_20;
            highp vec4 tmpvar_21;
            tmpvar_21 = clamp (_ClipRect, vec4(-2e+10, -2e+10, -2e+10, -2e+10), vec4(2e+10, 2e+10, 2e+10, 2e+10));
            highp vec2 tmpvar_22;
            tmpvar_22 = ((vert_8.xy - tmpvar_21.xy) / (tmpvar_21.zw - tmpvar_21.xy));
            highp vec4 tmpvar_23;
            tmpvar_23.xy = tmpvar_2;
            tmpvar_23.z = tmpvar_22.x;
            tmpvar_23.w = tmpvar_22.y;
            highp vec4 tmpvar_24;
            tmpvar_24.x = scale_6;
            tmpvar_24.y = (tmpvar_15 - tmpvar_16);
            tmpvar_24.z = (tmpvar_15 + tmpvar_16);
            tmpvar_24.w = tmpvar_15;
            highp vec2 tmpvar_25;
            tmpvar_25.x = _MaskSoftnessX;
            tmpvar_25.y = _MaskSoftnessY;
            highp vec4 tmpvar_26;
            tmpvar_26.xy = (((vert_8.xy * 2.0) - tmpvar_21.xy) - tmpvar_21.zw);
            tmpvar_26.zw = (0.25 / ((0.25 * tmpvar_25) + pixelSize_7));
            mediump vec4 tmpvar_27;
            mediump vec4 tmpvar_28;
            tmpvar_27 = tmpvar_24;
            tmpvar_28 = tmpvar_26;
            gl_Position = tmpvar_10;
            xlv_COLOR = faceColor_4;
            xlv_COLOR1 = outlineColor_3;
            xlv_TEXCOORD0 = tmpvar_23;
            xlv_TEXCOORD1 = tmpvar_27;
            xlv_TEXCOORD2 = tmpvar_28;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _ClipRect;
          uniform sampler2D _MainTex;
          varying lowp vec4 xlv_COLOR;
          varying highp vec4 xlv_TEXCOORD0;
          varying mediump vec4 xlv_TEXCOORD1;
          varying mediump vec4 xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 tmpvar_1;
            mediump vec4 c_2;
            lowp vec4 tmpvar_3;
            tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
            mediump float tmpvar_4;
            tmpvar_4 = clamp (((tmpvar_3.w * xlv_TEXCOORD1.x) - xlv_TEXCOORD1.w), 0.0, 1.0);
            lowp vec4 tmpvar_5;
            tmpvar_5 = (xlv_COLOR * tmpvar_4);
            c_2 = tmpvar_5;
            mediump vec2 tmpvar_6;
            tmpvar_6 = abs(xlv_TEXCOORD2.xy);
            mediump vec2 tmpvar_7;
            highp vec2 tmpvar_8;
            tmpvar_8 = clamp (((
              (_ClipRect.zw - _ClipRect.xy)
             - tmpvar_6) * xlv_TEXCOORD2.zw), 0.0, 1.0);
            tmpvar_7 = tmpvar_8;
            c_2 = (c_2 * (tmpvar_7.x * tmpvar_7.y));
            mediump float x_9;
            x_9 = (c_2.w - 0.001);
            if ((x_9 < 0.0)) {
              discard;
            };
            tmpvar_1 = c_2;
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
  FallBack Off
}
