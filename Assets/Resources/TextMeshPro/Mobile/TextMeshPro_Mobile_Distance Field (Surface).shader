Shader "TextMeshPro/Mobile/Distance Field (Surface)"
{
  Properties
  {
    _FaceTex ("Fill Texture", 2D) = "white" {}
    _FaceColor ("Fill Color", Color) = (1,1,1,1)
    _FaceDilate ("Face Dilate", Range(-1, 1)) = 0
    _OutlineColor ("Outline Color", Color) = (0,0,0,1)
    _OutlineTex ("Outline Texture", 2D) = "white" {}
    _OutlineWidth ("Outline Thickness", Range(0, 1)) = 0
    _OutlineSoftness ("Outline Softness", Range(0, 1)) = 0
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
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    LOD 300
    Pass // ind: 1, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "FORWARDBASE"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 300
      ZWrite Off
      Cull Off
      Blend SrcAlpha OneMinusSrcAlpha
      ColorMask RGB
      GpuProgramID 23212
      // m_ProgramMask = 6
      !!! *******************************************************************************************
      !!! Allow restore shader as UnityLab format - only available for DevX GameRecovery license type
      !!! *******************************************************************************************
      Program "vp"
      {
        SubProgram "gles hw_tier00"
        {
           Keywords { "DIRECTIONAL" }
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesTANGENT;
          attribute vec4 _glesVertex;
          attribute vec4 _glesColor;
          attribute vec3 _glesNormal;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _ScreenParams;
          uniform mediump vec4 unity_SHBr;
          uniform mediump vec4 unity_SHBg;
          uniform mediump vec4 unity_SHBb;
          uniform mediump vec4 unity_SHC;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp vec4 unity_WorldTransformParams;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp float _FaceDilate;
          uniform highp mat4 _EnvMatrix;
          uniform highp float _WeightNormal;
          uniform highp float _WeightBold;
          uniform highp float _ScaleRatioA;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp float _GradientScale;
          uniform highp float _ScaleX;
          uniform highp float _ScaleY;
          uniform highp float _PerspectiveFilter;
          uniform highp vec4 _MainTex_ST;
          uniform highp vec4 _FaceTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec4 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD4;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD5;
          varying highp vec3 xlv_TEXCOORD6;
          varying mediump vec3 xlv_TEXCOORD7;
          void main ()
          {
            lowp vec3 worldBinormal_1;
            lowp float tangentSign_2;
            lowp vec3 worldTangent_3;
            highp vec4 tmpvar_4;
            highp vec4 tmpvar_5;
            highp vec3 tmpvar_6;
            highp vec4 tmpvar_7;
            tmpvar_5.zw = _glesVertex.zw;
            tmpvar_7.zw = _glesMultiTexCoord1.zw;
            highp vec2 tmpvar_8;
            highp float scale_9;
            highp vec2 pixelSize_10;
            tmpvar_5.x = (_glesVertex.x + _VertexOffsetX);
            tmpvar_5.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_11;
            tmpvar_11.w = 1.0;
            tmpvar_11.xyz = _WorldSpaceCameraPos;
            tmpvar_6 = (_glesNormal * sign(dot (_glesNormal, 
              ((unity_WorldToObject * tmpvar_11).xyz - tmpvar_5.xyz)
            )));
            highp vec4 tmpvar_12;
            tmpvar_12.w = 1.0;
            tmpvar_12.xyz = tmpvar_5.xyz;
            highp vec2 tmpvar_13;
            tmpvar_13.x = _ScaleX;
            tmpvar_13.y = _ScaleY;
            highp mat2 tmpvar_14;
            tmpvar_14[0] = glstate_matrix_projection[0].xy;
            tmpvar_14[1] = glstate_matrix_projection[1].xy;
            pixelSize_10 = ((unity_MatrixVP * (unity_ObjectToWorld * tmpvar_12)).ww / (tmpvar_13 * (tmpvar_14 * _ScreenParams.xy)));
            scale_9 = (inversesqrt(dot (pixelSize_10, pixelSize_10)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            highp mat3 tmpvar_15;
            tmpvar_15[0] = unity_WorldToObject[0].xyz;
            tmpvar_15[1] = unity_WorldToObject[1].xyz;
            tmpvar_15[2] = unity_WorldToObject[2].xyz;
            highp float tmpvar_16;
            tmpvar_16 = mix ((scale_9 * (1.0 - _PerspectiveFilter)), scale_9, abs(dot (
              normalize((tmpvar_6 * tmpvar_15))
            , 
              normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz))
            )));
            scale_9 = tmpvar_16;
            tmpvar_8.y = tmpvar_16;
            tmpvar_8.x = (((
              (mix (_WeightNormal, _WeightBold, float((0.0 >= _glesMultiTexCoord1.y))) / 4.0)
             + _FaceDilate) * _ScaleRatioA) * 0.5);
            highp vec2 xlat_varoutput_17;
            xlat_varoutput_17.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_17.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_17.x));
            tmpvar_7.xy = (xlat_varoutput_17 * 0.001953125);
            highp mat3 tmpvar_18;
            tmpvar_18[0] = _EnvMatrix[0].xyz;
            tmpvar_18[1] = _EnvMatrix[1].xyz;
            tmpvar_18[2] = _EnvMatrix[2].xyz;
            highp vec4 tmpvar_19;
            tmpvar_19.w = 1.0;
            tmpvar_19.xyz = tmpvar_5.xyz;
            tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
            tmpvar_4.zw = ((tmpvar_7.xy * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            highp vec3 tmpvar_20;
            tmpvar_20 = (unity_ObjectToWorld * tmpvar_5).xyz;
            highp mat3 tmpvar_21;
            tmpvar_21[0] = unity_WorldToObject[0].xyz;
            tmpvar_21[1] = unity_WorldToObject[1].xyz;
            tmpvar_21[2] = unity_WorldToObject[2].xyz;
            highp vec3 tmpvar_22;
            tmpvar_22 = normalize((tmpvar_6 * tmpvar_21));
            highp mat3 tmpvar_23;
            tmpvar_23[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_23[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_23[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_24;
            tmpvar_24 = normalize((tmpvar_23 * _glesTANGENT.xyz));
            worldTangent_3 = tmpvar_24;
            highp float tmpvar_25;
            tmpvar_25 = (_glesTANGENT.w * unity_WorldTransformParams.w);
            tangentSign_2 = tmpvar_25;
            highp vec3 tmpvar_26;
            tmpvar_26 = (((tmpvar_22.yzx * worldTangent_3.zxy) - (tmpvar_22.zxy * worldTangent_3.yzx)) * tangentSign_2);
            worldBinormal_1 = tmpvar_26;
            highp vec4 tmpvar_27;
            tmpvar_27.x = worldTangent_3.x;
            tmpvar_27.y = worldBinormal_1.x;
            tmpvar_27.z = tmpvar_22.x;
            tmpvar_27.w = tmpvar_20.x;
            highp vec4 tmpvar_28;
            tmpvar_28.x = worldTangent_3.y;
            tmpvar_28.y = worldBinormal_1.y;
            tmpvar_28.z = tmpvar_22.y;
            tmpvar_28.w = tmpvar_20.y;
            highp vec4 tmpvar_29;
            tmpvar_29.x = worldTangent_3.z;
            tmpvar_29.y = worldBinormal_1.z;
            tmpvar_29.z = tmpvar_22.z;
            tmpvar_29.w = tmpvar_20.z;
            mediump vec3 normal_30;
            normal_30 = tmpvar_22;
            mediump vec3 x1_31;
            mediump vec4 tmpvar_32;
            tmpvar_32 = (normal_30.xyzz * normal_30.yzzx);
            x1_31.x = dot (unity_SHBr, tmpvar_32);
            x1_31.y = dot (unity_SHBg, tmpvar_32);
            x1_31.z = dot (unity_SHBb, tmpvar_32);
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_19));
            xlv_TEXCOORD0 = tmpvar_4;
            xlv_TEXCOORD1 = ((tmpvar_7.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_27;
            xlv_TEXCOORD3 = tmpvar_28;
            xlv_TEXCOORD4 = tmpvar_29;
            xlv_COLOR0 = _glesColor;
            xlv_TEXCOORD5 = tmpvar_8;
            xlv_TEXCOORD6 = (tmpvar_18 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz));
            xlv_TEXCOORD7 = (x1_31 + (unity_SHC.xyz * (
              (normal_30.x * normal_30.x)
             - 
              (normal_30.y * normal_30.y)
            )));
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform mediump vec4 _WorldSpaceLightPos0;
          uniform mediump vec4 unity_SHAr;
          uniform mediump vec4 unity_SHAg;
          uniform mediump vec4 unity_SHAb;
          uniform lowp vec4 _LightColor0;
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
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec4 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD4;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD5;
          varying mediump vec3 xlv_TEXCOORD7;
          void main ()
          {
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp vec3 tmpvar_4;
            lowp vec3 lightDir_5;
            lowp vec3 _unity_tbn_2_6;
            lowp vec3 _unity_tbn_1_7;
            lowp vec3 _unity_tbn_0_8;
            highp vec3 tmpvar_9;
            tmpvar_9 = xlv_TEXCOORD2.xyz;
            _unity_tbn_0_8 = tmpvar_9;
            highp vec3 tmpvar_10;
            tmpvar_10 = xlv_TEXCOORD3.xyz;
            _unity_tbn_1_7 = tmpvar_10;
            highp vec3 tmpvar_11;
            tmpvar_11 = xlv_TEXCOORD4.xyz;
            _unity_tbn_2_6 = tmpvar_11;
            mediump vec3 tmpvar_12;
            tmpvar_12 = _WorldSpaceLightPos0.xyz;
            lightDir_5 = tmpvar_12;
            lowp vec3 tmpvar_13;
            lowp float tmpvar_14;
            highp vec4 outlineColor_15;
            highp vec4 faceColor_16;
            highp float c_17;
            lowp float tmpvar_18;
            tmpvar_18 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_17 = tmpvar_18;
            highp float tmpvar_19;
            tmpvar_19 = (((
              (0.5 - c_17)
             - xlv_TEXCOORD5.x) * xlv_TEXCOORD5.y) + 0.5);
            highp float tmpvar_20;
            tmpvar_20 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD5.y);
            highp float tmpvar_21;
            tmpvar_21 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD5.y);
            faceColor_16 = _FaceColor;
            outlineColor_15 = _OutlineColor;
            faceColor_16 = (faceColor_16 * xlv_COLOR0);
            outlineColor_15.w = (outlineColor_15.w * xlv_COLOR0.w);
            highp vec2 tmpvar_22;
            tmpvar_22.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_22.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_23;
            tmpvar_23 = texture2D (_FaceTex, tmpvar_22);
            faceColor_16 = (faceColor_16 * tmpvar_23);
            highp vec2 tmpvar_24;
            tmpvar_24.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_24.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_25;
            tmpvar_25 = texture2D (_OutlineTex, tmpvar_24);
            outlineColor_15 = (outlineColor_15 * tmpvar_25);
            mediump float d_26;
            d_26 = tmpvar_19;
            lowp vec4 faceColor_27;
            faceColor_27 = faceColor_16;
            lowp vec4 outlineColor_28;
            outlineColor_28 = outlineColor_15;
            mediump float outline_29;
            outline_29 = tmpvar_20;
            mediump float softness_30;
            softness_30 = tmpvar_21;
            mediump float tmpvar_31;
            tmpvar_31 = (1.0 - clamp ((
              ((d_26 - (outline_29 * 0.5)) + (softness_30 * 0.5))
             / 
              (1.0 + softness_30)
            ), 0.0, 1.0));
            faceColor_27.xyz = (faceColor_27.xyz * faceColor_27.w);
            outlineColor_28.xyz = (outlineColor_28.xyz * outlineColor_28.w);
            mediump vec4 tmpvar_32;
            tmpvar_32 = mix (faceColor_27, outlineColor_28, vec4((clamp (
              (d_26 + (outline_29 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_29)
            ))));
            faceColor_27 = tmpvar_32;
            faceColor_27 = (faceColor_27 * tmpvar_31);
            faceColor_16 = faceColor_27;
            faceColor_16.xyz = (faceColor_16.xyz / max (faceColor_16.w, 0.0001));
            tmpvar_13 = faceColor_16.xyz;
            tmpvar_14 = faceColor_16.w;
            lowp float tmpvar_33;
            tmpvar_33 = _unity_tbn_0_8.z;
            worldN_3.x = tmpvar_33;
            lowp float tmpvar_34;
            tmpvar_34 = _unity_tbn_1_7.z;
            worldN_3.y = tmpvar_34;
            lowp float tmpvar_35;
            tmpvar_35 = _unity_tbn_2_6.z;
            worldN_3.z = tmpvar_35;
            highp vec3 tmpvar_36;
            tmpvar_36 = normalize(worldN_3);
            worldN_3 = tmpvar_36;
            tmpvar_4 = tmpvar_36;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_5;
            mediump vec3 normalWorld_37;
            normalWorld_37 = tmpvar_4;
            mediump vec4 tmpvar_38;
            tmpvar_38.w = 1.0;
            tmpvar_38.xyz = normalWorld_37;
            mediump vec3 x_39;
            x_39.x = dot (unity_SHAr, tmpvar_38);
            x_39.y = dot (unity_SHAg, tmpvar_38);
            x_39.z = dot (unity_SHAb, tmpvar_38);
            mediump vec3 tmpvar_40;
            tmpvar_40 = max (((1.055 * 
              pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD7 + x_39)), vec3(0.4166667, 0.4166667, 0.4166667))
            ) - 0.055), vec3(0.0, 0.0, 0.0));
            lowp vec4 c_41;
            lowp vec4 c_42;
            lowp float diff_43;
            mediump float tmpvar_44;
            tmpvar_44 = max (0.0, dot (tmpvar_4, tmpvar_2));
            diff_43 = tmpvar_44;
            c_42.xyz = ((tmpvar_13 * tmpvar_1) * diff_43);
            c_42.w = tmpvar_14;
            c_41.w = c_42.w;
            c_41.xyz = (c_42.xyz + (tmpvar_13 * tmpvar_40));
            gl_FragData[0] = c_41;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier01"
        {
           Keywords { "DIRECTIONAL" }
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesTANGENT;
          attribute vec4 _glesVertex;
          attribute vec4 _glesColor;
          attribute vec3 _glesNormal;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _ScreenParams;
          uniform mediump vec4 unity_SHBr;
          uniform mediump vec4 unity_SHBg;
          uniform mediump vec4 unity_SHBb;
          uniform mediump vec4 unity_SHC;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp vec4 unity_WorldTransformParams;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp float _FaceDilate;
          uniform highp mat4 _EnvMatrix;
          uniform highp float _WeightNormal;
          uniform highp float _WeightBold;
          uniform highp float _ScaleRatioA;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp float _GradientScale;
          uniform highp float _ScaleX;
          uniform highp float _ScaleY;
          uniform highp float _PerspectiveFilter;
          uniform highp vec4 _MainTex_ST;
          uniform highp vec4 _FaceTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec4 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD4;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD5;
          varying highp vec3 xlv_TEXCOORD6;
          varying mediump vec3 xlv_TEXCOORD7;
          void main ()
          {
            lowp vec3 worldBinormal_1;
            lowp float tangentSign_2;
            lowp vec3 worldTangent_3;
            highp vec4 tmpvar_4;
            highp vec4 tmpvar_5;
            highp vec3 tmpvar_6;
            highp vec4 tmpvar_7;
            tmpvar_5.zw = _glesVertex.zw;
            tmpvar_7.zw = _glesMultiTexCoord1.zw;
            highp vec2 tmpvar_8;
            highp float scale_9;
            highp vec2 pixelSize_10;
            tmpvar_5.x = (_glesVertex.x + _VertexOffsetX);
            tmpvar_5.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_11;
            tmpvar_11.w = 1.0;
            tmpvar_11.xyz = _WorldSpaceCameraPos;
            tmpvar_6 = (_glesNormal * sign(dot (_glesNormal, 
              ((unity_WorldToObject * tmpvar_11).xyz - tmpvar_5.xyz)
            )));
            highp vec4 tmpvar_12;
            tmpvar_12.w = 1.0;
            tmpvar_12.xyz = tmpvar_5.xyz;
            highp vec2 tmpvar_13;
            tmpvar_13.x = _ScaleX;
            tmpvar_13.y = _ScaleY;
            highp mat2 tmpvar_14;
            tmpvar_14[0] = glstate_matrix_projection[0].xy;
            tmpvar_14[1] = glstate_matrix_projection[1].xy;
            pixelSize_10 = ((unity_MatrixVP * (unity_ObjectToWorld * tmpvar_12)).ww / (tmpvar_13 * (tmpvar_14 * _ScreenParams.xy)));
            scale_9 = (inversesqrt(dot (pixelSize_10, pixelSize_10)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            highp mat3 tmpvar_15;
            tmpvar_15[0] = unity_WorldToObject[0].xyz;
            tmpvar_15[1] = unity_WorldToObject[1].xyz;
            tmpvar_15[2] = unity_WorldToObject[2].xyz;
            highp float tmpvar_16;
            tmpvar_16 = mix ((scale_9 * (1.0 - _PerspectiveFilter)), scale_9, abs(dot (
              normalize((tmpvar_6 * tmpvar_15))
            , 
              normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz))
            )));
            scale_9 = tmpvar_16;
            tmpvar_8.y = tmpvar_16;
            tmpvar_8.x = (((
              (mix (_WeightNormal, _WeightBold, float((0.0 >= _glesMultiTexCoord1.y))) / 4.0)
             + _FaceDilate) * _ScaleRatioA) * 0.5);
            highp vec2 xlat_varoutput_17;
            xlat_varoutput_17.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_17.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_17.x));
            tmpvar_7.xy = (xlat_varoutput_17 * 0.001953125);
            highp mat3 tmpvar_18;
            tmpvar_18[0] = _EnvMatrix[0].xyz;
            tmpvar_18[1] = _EnvMatrix[1].xyz;
            tmpvar_18[2] = _EnvMatrix[2].xyz;
            highp vec4 tmpvar_19;
            tmpvar_19.w = 1.0;
            tmpvar_19.xyz = tmpvar_5.xyz;
            tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
            tmpvar_4.zw = ((tmpvar_7.xy * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            highp vec3 tmpvar_20;
            tmpvar_20 = (unity_ObjectToWorld * tmpvar_5).xyz;
            highp mat3 tmpvar_21;
            tmpvar_21[0] = unity_WorldToObject[0].xyz;
            tmpvar_21[1] = unity_WorldToObject[1].xyz;
            tmpvar_21[2] = unity_WorldToObject[2].xyz;
            highp vec3 tmpvar_22;
            tmpvar_22 = normalize((tmpvar_6 * tmpvar_21));
            highp mat3 tmpvar_23;
            tmpvar_23[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_23[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_23[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_24;
            tmpvar_24 = normalize((tmpvar_23 * _glesTANGENT.xyz));
            worldTangent_3 = tmpvar_24;
            highp float tmpvar_25;
            tmpvar_25 = (_glesTANGENT.w * unity_WorldTransformParams.w);
            tangentSign_2 = tmpvar_25;
            highp vec3 tmpvar_26;
            tmpvar_26 = (((tmpvar_22.yzx * worldTangent_3.zxy) - (tmpvar_22.zxy * worldTangent_3.yzx)) * tangentSign_2);
            worldBinormal_1 = tmpvar_26;
            highp vec4 tmpvar_27;
            tmpvar_27.x = worldTangent_3.x;
            tmpvar_27.y = worldBinormal_1.x;
            tmpvar_27.z = tmpvar_22.x;
            tmpvar_27.w = tmpvar_20.x;
            highp vec4 tmpvar_28;
            tmpvar_28.x = worldTangent_3.y;
            tmpvar_28.y = worldBinormal_1.y;
            tmpvar_28.z = tmpvar_22.y;
            tmpvar_28.w = tmpvar_20.y;
            highp vec4 tmpvar_29;
            tmpvar_29.x = worldTangent_3.z;
            tmpvar_29.y = worldBinormal_1.z;
            tmpvar_29.z = tmpvar_22.z;
            tmpvar_29.w = tmpvar_20.z;
            mediump vec3 normal_30;
            normal_30 = tmpvar_22;
            mediump vec3 x1_31;
            mediump vec4 tmpvar_32;
            tmpvar_32 = (normal_30.xyzz * normal_30.yzzx);
            x1_31.x = dot (unity_SHBr, tmpvar_32);
            x1_31.y = dot (unity_SHBg, tmpvar_32);
            x1_31.z = dot (unity_SHBb, tmpvar_32);
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_19));
            xlv_TEXCOORD0 = tmpvar_4;
            xlv_TEXCOORD1 = ((tmpvar_7.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_27;
            xlv_TEXCOORD3 = tmpvar_28;
            xlv_TEXCOORD4 = tmpvar_29;
            xlv_COLOR0 = _glesColor;
            xlv_TEXCOORD5 = tmpvar_8;
            xlv_TEXCOORD6 = (tmpvar_18 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz));
            xlv_TEXCOORD7 = (x1_31 + (unity_SHC.xyz * (
              (normal_30.x * normal_30.x)
             - 
              (normal_30.y * normal_30.y)
            )));
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform mediump vec4 _WorldSpaceLightPos0;
          uniform mediump vec4 unity_SHAr;
          uniform mediump vec4 unity_SHAg;
          uniform mediump vec4 unity_SHAb;
          uniform lowp vec4 _LightColor0;
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
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec4 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD4;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD5;
          varying mediump vec3 xlv_TEXCOORD7;
          void main ()
          {
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp vec3 tmpvar_4;
            lowp vec3 lightDir_5;
            lowp vec3 _unity_tbn_2_6;
            lowp vec3 _unity_tbn_1_7;
            lowp vec3 _unity_tbn_0_8;
            highp vec3 tmpvar_9;
            tmpvar_9 = xlv_TEXCOORD2.xyz;
            _unity_tbn_0_8 = tmpvar_9;
            highp vec3 tmpvar_10;
            tmpvar_10 = xlv_TEXCOORD3.xyz;
            _unity_tbn_1_7 = tmpvar_10;
            highp vec3 tmpvar_11;
            tmpvar_11 = xlv_TEXCOORD4.xyz;
            _unity_tbn_2_6 = tmpvar_11;
            mediump vec3 tmpvar_12;
            tmpvar_12 = _WorldSpaceLightPos0.xyz;
            lightDir_5 = tmpvar_12;
            lowp vec3 tmpvar_13;
            lowp float tmpvar_14;
            highp vec4 outlineColor_15;
            highp vec4 faceColor_16;
            highp float c_17;
            lowp float tmpvar_18;
            tmpvar_18 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_17 = tmpvar_18;
            highp float tmpvar_19;
            tmpvar_19 = (((
              (0.5 - c_17)
             - xlv_TEXCOORD5.x) * xlv_TEXCOORD5.y) + 0.5);
            highp float tmpvar_20;
            tmpvar_20 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD5.y);
            highp float tmpvar_21;
            tmpvar_21 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD5.y);
            faceColor_16 = _FaceColor;
            outlineColor_15 = _OutlineColor;
            faceColor_16 = (faceColor_16 * xlv_COLOR0);
            outlineColor_15.w = (outlineColor_15.w * xlv_COLOR0.w);
            highp vec2 tmpvar_22;
            tmpvar_22.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_22.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_23;
            tmpvar_23 = texture2D (_FaceTex, tmpvar_22);
            faceColor_16 = (faceColor_16 * tmpvar_23);
            highp vec2 tmpvar_24;
            tmpvar_24.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_24.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_25;
            tmpvar_25 = texture2D (_OutlineTex, tmpvar_24);
            outlineColor_15 = (outlineColor_15 * tmpvar_25);
            mediump float d_26;
            d_26 = tmpvar_19;
            lowp vec4 faceColor_27;
            faceColor_27 = faceColor_16;
            lowp vec4 outlineColor_28;
            outlineColor_28 = outlineColor_15;
            mediump float outline_29;
            outline_29 = tmpvar_20;
            mediump float softness_30;
            softness_30 = tmpvar_21;
            mediump float tmpvar_31;
            tmpvar_31 = (1.0 - clamp ((
              ((d_26 - (outline_29 * 0.5)) + (softness_30 * 0.5))
             / 
              (1.0 + softness_30)
            ), 0.0, 1.0));
            faceColor_27.xyz = (faceColor_27.xyz * faceColor_27.w);
            outlineColor_28.xyz = (outlineColor_28.xyz * outlineColor_28.w);
            mediump vec4 tmpvar_32;
            tmpvar_32 = mix (faceColor_27, outlineColor_28, vec4((clamp (
              (d_26 + (outline_29 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_29)
            ))));
            faceColor_27 = tmpvar_32;
            faceColor_27 = (faceColor_27 * tmpvar_31);
            faceColor_16 = faceColor_27;
            faceColor_16.xyz = (faceColor_16.xyz / max (faceColor_16.w, 0.0001));
            tmpvar_13 = faceColor_16.xyz;
            tmpvar_14 = faceColor_16.w;
            lowp float tmpvar_33;
            tmpvar_33 = _unity_tbn_0_8.z;
            worldN_3.x = tmpvar_33;
            lowp float tmpvar_34;
            tmpvar_34 = _unity_tbn_1_7.z;
            worldN_3.y = tmpvar_34;
            lowp float tmpvar_35;
            tmpvar_35 = _unity_tbn_2_6.z;
            worldN_3.z = tmpvar_35;
            highp vec3 tmpvar_36;
            tmpvar_36 = normalize(worldN_3);
            worldN_3 = tmpvar_36;
            tmpvar_4 = tmpvar_36;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_5;
            mediump vec3 normalWorld_37;
            normalWorld_37 = tmpvar_4;
            mediump vec4 tmpvar_38;
            tmpvar_38.w = 1.0;
            tmpvar_38.xyz = normalWorld_37;
            mediump vec3 x_39;
            x_39.x = dot (unity_SHAr, tmpvar_38);
            x_39.y = dot (unity_SHAg, tmpvar_38);
            x_39.z = dot (unity_SHAb, tmpvar_38);
            mediump vec3 tmpvar_40;
            tmpvar_40 = max (((1.055 * 
              pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD7 + x_39)), vec3(0.4166667, 0.4166667, 0.4166667))
            ) - 0.055), vec3(0.0, 0.0, 0.0));
            lowp vec4 c_41;
            lowp vec4 c_42;
            lowp float diff_43;
            mediump float tmpvar_44;
            tmpvar_44 = max (0.0, dot (tmpvar_4, tmpvar_2));
            diff_43 = tmpvar_44;
            c_42.xyz = ((tmpvar_13 * tmpvar_1) * diff_43);
            c_42.w = tmpvar_14;
            c_41.w = c_42.w;
            c_41.xyz = (c_42.xyz + (tmpvar_13 * tmpvar_40));
            gl_FragData[0] = c_41;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier02"
        {
           Keywords { "DIRECTIONAL" }
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesTANGENT;
          attribute vec4 _glesVertex;
          attribute vec4 _glesColor;
          attribute vec3 _glesNormal;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _ScreenParams;
          uniform mediump vec4 unity_SHBr;
          uniform mediump vec4 unity_SHBg;
          uniform mediump vec4 unity_SHBb;
          uniform mediump vec4 unity_SHC;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp vec4 unity_WorldTransformParams;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp float _FaceDilate;
          uniform highp mat4 _EnvMatrix;
          uniform highp float _WeightNormal;
          uniform highp float _WeightBold;
          uniform highp float _ScaleRatioA;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp float _GradientScale;
          uniform highp float _ScaleX;
          uniform highp float _ScaleY;
          uniform highp float _PerspectiveFilter;
          uniform highp vec4 _MainTex_ST;
          uniform highp vec4 _FaceTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec4 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD4;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD5;
          varying highp vec3 xlv_TEXCOORD6;
          varying mediump vec3 xlv_TEXCOORD7;
          void main ()
          {
            lowp vec3 worldBinormal_1;
            lowp float tangentSign_2;
            lowp vec3 worldTangent_3;
            highp vec4 tmpvar_4;
            highp vec4 tmpvar_5;
            highp vec3 tmpvar_6;
            highp vec4 tmpvar_7;
            tmpvar_5.zw = _glesVertex.zw;
            tmpvar_7.zw = _glesMultiTexCoord1.zw;
            highp vec2 tmpvar_8;
            highp float scale_9;
            highp vec2 pixelSize_10;
            tmpvar_5.x = (_glesVertex.x + _VertexOffsetX);
            tmpvar_5.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_11;
            tmpvar_11.w = 1.0;
            tmpvar_11.xyz = _WorldSpaceCameraPos;
            tmpvar_6 = (_glesNormal * sign(dot (_glesNormal, 
              ((unity_WorldToObject * tmpvar_11).xyz - tmpvar_5.xyz)
            )));
            highp vec4 tmpvar_12;
            tmpvar_12.w = 1.0;
            tmpvar_12.xyz = tmpvar_5.xyz;
            highp vec2 tmpvar_13;
            tmpvar_13.x = _ScaleX;
            tmpvar_13.y = _ScaleY;
            highp mat2 tmpvar_14;
            tmpvar_14[0] = glstate_matrix_projection[0].xy;
            tmpvar_14[1] = glstate_matrix_projection[1].xy;
            pixelSize_10 = ((unity_MatrixVP * (unity_ObjectToWorld * tmpvar_12)).ww / (tmpvar_13 * (tmpvar_14 * _ScreenParams.xy)));
            scale_9 = (inversesqrt(dot (pixelSize_10, pixelSize_10)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            highp mat3 tmpvar_15;
            tmpvar_15[0] = unity_WorldToObject[0].xyz;
            tmpvar_15[1] = unity_WorldToObject[1].xyz;
            tmpvar_15[2] = unity_WorldToObject[2].xyz;
            highp float tmpvar_16;
            tmpvar_16 = mix ((scale_9 * (1.0 - _PerspectiveFilter)), scale_9, abs(dot (
              normalize((tmpvar_6 * tmpvar_15))
            , 
              normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz))
            )));
            scale_9 = tmpvar_16;
            tmpvar_8.y = tmpvar_16;
            tmpvar_8.x = (((
              (mix (_WeightNormal, _WeightBold, float((0.0 >= _glesMultiTexCoord1.y))) / 4.0)
             + _FaceDilate) * _ScaleRatioA) * 0.5);
            highp vec2 xlat_varoutput_17;
            xlat_varoutput_17.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_17.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_17.x));
            tmpvar_7.xy = (xlat_varoutput_17 * 0.001953125);
            highp mat3 tmpvar_18;
            tmpvar_18[0] = _EnvMatrix[0].xyz;
            tmpvar_18[1] = _EnvMatrix[1].xyz;
            tmpvar_18[2] = _EnvMatrix[2].xyz;
            highp vec4 tmpvar_19;
            tmpvar_19.w = 1.0;
            tmpvar_19.xyz = tmpvar_5.xyz;
            tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
            tmpvar_4.zw = ((tmpvar_7.xy * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            highp vec3 tmpvar_20;
            tmpvar_20 = (unity_ObjectToWorld * tmpvar_5).xyz;
            highp mat3 tmpvar_21;
            tmpvar_21[0] = unity_WorldToObject[0].xyz;
            tmpvar_21[1] = unity_WorldToObject[1].xyz;
            tmpvar_21[2] = unity_WorldToObject[2].xyz;
            highp vec3 tmpvar_22;
            tmpvar_22 = normalize((tmpvar_6 * tmpvar_21));
            highp mat3 tmpvar_23;
            tmpvar_23[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_23[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_23[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_24;
            tmpvar_24 = normalize((tmpvar_23 * _glesTANGENT.xyz));
            worldTangent_3 = tmpvar_24;
            highp float tmpvar_25;
            tmpvar_25 = (_glesTANGENT.w * unity_WorldTransformParams.w);
            tangentSign_2 = tmpvar_25;
            highp vec3 tmpvar_26;
            tmpvar_26 = (((tmpvar_22.yzx * worldTangent_3.zxy) - (tmpvar_22.zxy * worldTangent_3.yzx)) * tangentSign_2);
            worldBinormal_1 = tmpvar_26;
            highp vec4 tmpvar_27;
            tmpvar_27.x = worldTangent_3.x;
            tmpvar_27.y = worldBinormal_1.x;
            tmpvar_27.z = tmpvar_22.x;
            tmpvar_27.w = tmpvar_20.x;
            highp vec4 tmpvar_28;
            tmpvar_28.x = worldTangent_3.y;
            tmpvar_28.y = worldBinormal_1.y;
            tmpvar_28.z = tmpvar_22.y;
            tmpvar_28.w = tmpvar_20.y;
            highp vec4 tmpvar_29;
            tmpvar_29.x = worldTangent_3.z;
            tmpvar_29.y = worldBinormal_1.z;
            tmpvar_29.z = tmpvar_22.z;
            tmpvar_29.w = tmpvar_20.z;
            mediump vec3 normal_30;
            normal_30 = tmpvar_22;
            mediump vec3 x1_31;
            mediump vec4 tmpvar_32;
            tmpvar_32 = (normal_30.xyzz * normal_30.yzzx);
            x1_31.x = dot (unity_SHBr, tmpvar_32);
            x1_31.y = dot (unity_SHBg, tmpvar_32);
            x1_31.z = dot (unity_SHBb, tmpvar_32);
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_19));
            xlv_TEXCOORD0 = tmpvar_4;
            xlv_TEXCOORD1 = ((tmpvar_7.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_27;
            xlv_TEXCOORD3 = tmpvar_28;
            xlv_TEXCOORD4 = tmpvar_29;
            xlv_COLOR0 = _glesColor;
            xlv_TEXCOORD5 = tmpvar_8;
            xlv_TEXCOORD6 = (tmpvar_18 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz));
            xlv_TEXCOORD7 = (x1_31 + (unity_SHC.xyz * (
              (normal_30.x * normal_30.x)
             - 
              (normal_30.y * normal_30.y)
            )));
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform mediump vec4 _WorldSpaceLightPos0;
          uniform mediump vec4 unity_SHAr;
          uniform mediump vec4 unity_SHAg;
          uniform mediump vec4 unity_SHAb;
          uniform lowp vec4 _LightColor0;
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
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec4 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD4;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD5;
          varying mediump vec3 xlv_TEXCOORD7;
          void main ()
          {
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp vec3 tmpvar_4;
            lowp vec3 lightDir_5;
            lowp vec3 _unity_tbn_2_6;
            lowp vec3 _unity_tbn_1_7;
            lowp vec3 _unity_tbn_0_8;
            highp vec3 tmpvar_9;
            tmpvar_9 = xlv_TEXCOORD2.xyz;
            _unity_tbn_0_8 = tmpvar_9;
            highp vec3 tmpvar_10;
            tmpvar_10 = xlv_TEXCOORD3.xyz;
            _unity_tbn_1_7 = tmpvar_10;
            highp vec3 tmpvar_11;
            tmpvar_11 = xlv_TEXCOORD4.xyz;
            _unity_tbn_2_6 = tmpvar_11;
            mediump vec3 tmpvar_12;
            tmpvar_12 = _WorldSpaceLightPos0.xyz;
            lightDir_5 = tmpvar_12;
            lowp vec3 tmpvar_13;
            lowp float tmpvar_14;
            highp vec4 outlineColor_15;
            highp vec4 faceColor_16;
            highp float c_17;
            lowp float tmpvar_18;
            tmpvar_18 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_17 = tmpvar_18;
            highp float tmpvar_19;
            tmpvar_19 = (((
              (0.5 - c_17)
             - xlv_TEXCOORD5.x) * xlv_TEXCOORD5.y) + 0.5);
            highp float tmpvar_20;
            tmpvar_20 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD5.y);
            highp float tmpvar_21;
            tmpvar_21 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD5.y);
            faceColor_16 = _FaceColor;
            outlineColor_15 = _OutlineColor;
            faceColor_16 = (faceColor_16 * xlv_COLOR0);
            outlineColor_15.w = (outlineColor_15.w * xlv_COLOR0.w);
            highp vec2 tmpvar_22;
            tmpvar_22.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_22.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_23;
            tmpvar_23 = texture2D (_FaceTex, tmpvar_22);
            faceColor_16 = (faceColor_16 * tmpvar_23);
            highp vec2 tmpvar_24;
            tmpvar_24.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_24.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_25;
            tmpvar_25 = texture2D (_OutlineTex, tmpvar_24);
            outlineColor_15 = (outlineColor_15 * tmpvar_25);
            mediump float d_26;
            d_26 = tmpvar_19;
            lowp vec4 faceColor_27;
            faceColor_27 = faceColor_16;
            lowp vec4 outlineColor_28;
            outlineColor_28 = outlineColor_15;
            mediump float outline_29;
            outline_29 = tmpvar_20;
            mediump float softness_30;
            softness_30 = tmpvar_21;
            mediump float tmpvar_31;
            tmpvar_31 = (1.0 - clamp ((
              ((d_26 - (outline_29 * 0.5)) + (softness_30 * 0.5))
             / 
              (1.0 + softness_30)
            ), 0.0, 1.0));
            faceColor_27.xyz = (faceColor_27.xyz * faceColor_27.w);
            outlineColor_28.xyz = (outlineColor_28.xyz * outlineColor_28.w);
            mediump vec4 tmpvar_32;
            tmpvar_32 = mix (faceColor_27, outlineColor_28, vec4((clamp (
              (d_26 + (outline_29 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_29)
            ))));
            faceColor_27 = tmpvar_32;
            faceColor_27 = (faceColor_27 * tmpvar_31);
            faceColor_16 = faceColor_27;
            faceColor_16.xyz = (faceColor_16.xyz / max (faceColor_16.w, 0.0001));
            tmpvar_13 = faceColor_16.xyz;
            tmpvar_14 = faceColor_16.w;
            lowp float tmpvar_33;
            tmpvar_33 = _unity_tbn_0_8.z;
            worldN_3.x = tmpvar_33;
            lowp float tmpvar_34;
            tmpvar_34 = _unity_tbn_1_7.z;
            worldN_3.y = tmpvar_34;
            lowp float tmpvar_35;
            tmpvar_35 = _unity_tbn_2_6.z;
            worldN_3.z = tmpvar_35;
            highp vec3 tmpvar_36;
            tmpvar_36 = normalize(worldN_3);
            worldN_3 = tmpvar_36;
            tmpvar_4 = tmpvar_36;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_5;
            mediump vec3 normalWorld_37;
            normalWorld_37 = tmpvar_4;
            mediump vec4 tmpvar_38;
            tmpvar_38.w = 1.0;
            tmpvar_38.xyz = normalWorld_37;
            mediump vec3 x_39;
            x_39.x = dot (unity_SHAr, tmpvar_38);
            x_39.y = dot (unity_SHAg, tmpvar_38);
            x_39.z = dot (unity_SHAb, tmpvar_38);
            mediump vec3 tmpvar_40;
            tmpvar_40 = max (((1.055 * 
              pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD7 + x_39)), vec3(0.4166667, 0.4166667, 0.4166667))
            ) - 0.055), vec3(0.0, 0.0, 0.0));
            lowp vec4 c_41;
            lowp vec4 c_42;
            lowp float diff_43;
            mediump float tmpvar_44;
            tmpvar_44 = max (0.0, dot (tmpvar_4, tmpvar_2));
            diff_43 = tmpvar_44;
            c_42.xyz = ((tmpvar_13 * tmpvar_1) * diff_43);
            c_42.w = tmpvar_14;
            c_41.w = c_42.w;
            c_41.xyz = (c_42.xyz + (tmpvar_13 * tmpvar_40));
            gl_FragData[0] = c_41;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier00"
        {
           Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" }
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesTANGENT;
          attribute vec4 _glesVertex;
          attribute vec4 _glesColor;
          attribute vec3 _glesNormal;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _ScreenParams;
          uniform mediump vec4 unity_SHBr;
          uniform mediump vec4 unity_SHBg;
          uniform mediump vec4 unity_SHBb;
          uniform mediump vec4 unity_SHC;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp vec4 unity_WorldTransformParams;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp float _FaceDilate;
          uniform highp mat4 _EnvMatrix;
          uniform highp float _WeightNormal;
          uniform highp float _WeightBold;
          uniform highp float _ScaleRatioA;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp float _GradientScale;
          uniform highp float _ScaleX;
          uniform highp float _ScaleY;
          uniform highp float _PerspectiveFilter;
          uniform highp vec4 _MainTex_ST;
          uniform highp vec4 _FaceTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec4 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD4;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD5;
          varying highp vec3 xlv_TEXCOORD6;
          varying mediump vec3 xlv_TEXCOORD7;
          void main ()
          {
            lowp vec3 worldBinormal_1;
            lowp float tangentSign_2;
            lowp vec3 worldTangent_3;
            highp vec4 tmpvar_4;
            highp vec4 tmpvar_5;
            highp vec3 tmpvar_6;
            highp vec4 tmpvar_7;
            tmpvar_5.zw = _glesVertex.zw;
            tmpvar_7.zw = _glesMultiTexCoord1.zw;
            highp vec2 tmpvar_8;
            highp float scale_9;
            highp vec2 pixelSize_10;
            tmpvar_5.x = (_glesVertex.x + _VertexOffsetX);
            tmpvar_5.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_11;
            tmpvar_11.w = 1.0;
            tmpvar_11.xyz = _WorldSpaceCameraPos;
            tmpvar_6 = (_glesNormal * sign(dot (_glesNormal, 
              ((unity_WorldToObject * tmpvar_11).xyz - tmpvar_5.xyz)
            )));
            highp vec4 tmpvar_12;
            tmpvar_12.w = 1.0;
            tmpvar_12.xyz = tmpvar_5.xyz;
            highp vec2 tmpvar_13;
            tmpvar_13.x = _ScaleX;
            tmpvar_13.y = _ScaleY;
            highp mat2 tmpvar_14;
            tmpvar_14[0] = glstate_matrix_projection[0].xy;
            tmpvar_14[1] = glstate_matrix_projection[1].xy;
            pixelSize_10 = ((unity_MatrixVP * (unity_ObjectToWorld * tmpvar_12)).ww / (tmpvar_13 * (tmpvar_14 * _ScreenParams.xy)));
            scale_9 = (inversesqrt(dot (pixelSize_10, pixelSize_10)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            highp mat3 tmpvar_15;
            tmpvar_15[0] = unity_WorldToObject[0].xyz;
            tmpvar_15[1] = unity_WorldToObject[1].xyz;
            tmpvar_15[2] = unity_WorldToObject[2].xyz;
            highp float tmpvar_16;
            tmpvar_16 = mix ((scale_9 * (1.0 - _PerspectiveFilter)), scale_9, abs(dot (
              normalize((tmpvar_6 * tmpvar_15))
            , 
              normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz))
            )));
            scale_9 = tmpvar_16;
            tmpvar_8.y = tmpvar_16;
            tmpvar_8.x = (((
              (mix (_WeightNormal, _WeightBold, float((0.0 >= _glesMultiTexCoord1.y))) / 4.0)
             + _FaceDilate) * _ScaleRatioA) * 0.5);
            highp vec2 xlat_varoutput_17;
            xlat_varoutput_17.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_17.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_17.x));
            tmpvar_7.xy = (xlat_varoutput_17 * 0.001953125);
            highp mat3 tmpvar_18;
            tmpvar_18[0] = _EnvMatrix[0].xyz;
            tmpvar_18[1] = _EnvMatrix[1].xyz;
            tmpvar_18[2] = _EnvMatrix[2].xyz;
            highp vec4 tmpvar_19;
            tmpvar_19.w = 1.0;
            tmpvar_19.xyz = tmpvar_5.xyz;
            tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
            tmpvar_4.zw = ((tmpvar_7.xy * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            highp vec3 tmpvar_20;
            tmpvar_20 = (unity_ObjectToWorld * tmpvar_5).xyz;
            highp mat3 tmpvar_21;
            tmpvar_21[0] = unity_WorldToObject[0].xyz;
            tmpvar_21[1] = unity_WorldToObject[1].xyz;
            tmpvar_21[2] = unity_WorldToObject[2].xyz;
            highp vec3 tmpvar_22;
            tmpvar_22 = normalize((tmpvar_6 * tmpvar_21));
            highp mat3 tmpvar_23;
            tmpvar_23[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_23[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_23[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_24;
            tmpvar_24 = normalize((tmpvar_23 * _glesTANGENT.xyz));
            worldTangent_3 = tmpvar_24;
            highp float tmpvar_25;
            tmpvar_25 = (_glesTANGENT.w * unity_WorldTransformParams.w);
            tangentSign_2 = tmpvar_25;
            highp vec3 tmpvar_26;
            tmpvar_26 = (((tmpvar_22.yzx * worldTangent_3.zxy) - (tmpvar_22.zxy * worldTangent_3.yzx)) * tangentSign_2);
            worldBinormal_1 = tmpvar_26;
            highp vec4 tmpvar_27;
            tmpvar_27.x = worldTangent_3.x;
            tmpvar_27.y = worldBinormal_1.x;
            tmpvar_27.z = tmpvar_22.x;
            tmpvar_27.w = tmpvar_20.x;
            highp vec4 tmpvar_28;
            tmpvar_28.x = worldTangent_3.y;
            tmpvar_28.y = worldBinormal_1.y;
            tmpvar_28.z = tmpvar_22.y;
            tmpvar_28.w = tmpvar_20.y;
            highp vec4 tmpvar_29;
            tmpvar_29.x = worldTangent_3.z;
            tmpvar_29.y = worldBinormal_1.z;
            tmpvar_29.z = tmpvar_22.z;
            tmpvar_29.w = tmpvar_20.z;
            mediump vec3 normal_30;
            normal_30 = tmpvar_22;
            mediump vec3 x1_31;
            mediump vec4 tmpvar_32;
            tmpvar_32 = (normal_30.xyzz * normal_30.yzzx);
            x1_31.x = dot (unity_SHBr, tmpvar_32);
            x1_31.y = dot (unity_SHBg, tmpvar_32);
            x1_31.z = dot (unity_SHBb, tmpvar_32);
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_19));
            xlv_TEXCOORD0 = tmpvar_4;
            xlv_TEXCOORD1 = ((tmpvar_7.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_27;
            xlv_TEXCOORD3 = tmpvar_28;
            xlv_TEXCOORD4 = tmpvar_29;
            xlv_COLOR0 = _glesColor;
            xlv_TEXCOORD5 = tmpvar_8;
            xlv_TEXCOORD6 = (tmpvar_18 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz));
            xlv_TEXCOORD7 = (x1_31 + (unity_SHC.xyz * (
              (normal_30.x * normal_30.x)
             - 
              (normal_30.y * normal_30.y)
            )));
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform mediump vec4 _WorldSpaceLightPos0;
          uniform mediump vec4 unity_SHAr;
          uniform mediump vec4 unity_SHAg;
          uniform mediump vec4 unity_SHAb;
          uniform lowp vec4 _LightColor0;
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
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec4 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD4;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD5;
          varying mediump vec3 xlv_TEXCOORD7;
          void main ()
          {
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp vec3 tmpvar_4;
            lowp vec3 lightDir_5;
            lowp vec3 _unity_tbn_2_6;
            lowp vec3 _unity_tbn_1_7;
            lowp vec3 _unity_tbn_0_8;
            highp vec3 tmpvar_9;
            tmpvar_9 = xlv_TEXCOORD2.xyz;
            _unity_tbn_0_8 = tmpvar_9;
            highp vec3 tmpvar_10;
            tmpvar_10 = xlv_TEXCOORD3.xyz;
            _unity_tbn_1_7 = tmpvar_10;
            highp vec3 tmpvar_11;
            tmpvar_11 = xlv_TEXCOORD4.xyz;
            _unity_tbn_2_6 = tmpvar_11;
            mediump vec3 tmpvar_12;
            tmpvar_12 = _WorldSpaceLightPos0.xyz;
            lightDir_5 = tmpvar_12;
            lowp vec3 tmpvar_13;
            lowp float tmpvar_14;
            highp vec4 outlineColor_15;
            highp vec4 faceColor_16;
            highp float c_17;
            lowp float tmpvar_18;
            tmpvar_18 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_17 = tmpvar_18;
            highp float tmpvar_19;
            tmpvar_19 = (((
              (0.5 - c_17)
             - xlv_TEXCOORD5.x) * xlv_TEXCOORD5.y) + 0.5);
            highp float tmpvar_20;
            tmpvar_20 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD5.y);
            highp float tmpvar_21;
            tmpvar_21 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD5.y);
            faceColor_16 = _FaceColor;
            outlineColor_15 = _OutlineColor;
            faceColor_16 = (faceColor_16 * xlv_COLOR0);
            outlineColor_15.w = (outlineColor_15.w * xlv_COLOR0.w);
            highp vec2 tmpvar_22;
            tmpvar_22.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_22.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_23;
            tmpvar_23 = texture2D (_FaceTex, tmpvar_22);
            faceColor_16 = (faceColor_16 * tmpvar_23);
            highp vec2 tmpvar_24;
            tmpvar_24.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_24.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_25;
            tmpvar_25 = texture2D (_OutlineTex, tmpvar_24);
            outlineColor_15 = (outlineColor_15 * tmpvar_25);
            mediump float d_26;
            d_26 = tmpvar_19;
            lowp vec4 faceColor_27;
            faceColor_27 = faceColor_16;
            lowp vec4 outlineColor_28;
            outlineColor_28 = outlineColor_15;
            mediump float outline_29;
            outline_29 = tmpvar_20;
            mediump float softness_30;
            softness_30 = tmpvar_21;
            mediump float tmpvar_31;
            tmpvar_31 = (1.0 - clamp ((
              ((d_26 - (outline_29 * 0.5)) + (softness_30 * 0.5))
             / 
              (1.0 + softness_30)
            ), 0.0, 1.0));
            faceColor_27.xyz = (faceColor_27.xyz * faceColor_27.w);
            outlineColor_28.xyz = (outlineColor_28.xyz * outlineColor_28.w);
            mediump vec4 tmpvar_32;
            tmpvar_32 = mix (faceColor_27, outlineColor_28, vec4((clamp (
              (d_26 + (outline_29 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_29)
            ))));
            faceColor_27 = tmpvar_32;
            faceColor_27 = (faceColor_27 * tmpvar_31);
            faceColor_16 = faceColor_27;
            faceColor_16.xyz = (faceColor_16.xyz / max (faceColor_16.w, 0.0001));
            tmpvar_13 = faceColor_16.xyz;
            tmpvar_14 = faceColor_16.w;
            lowp float tmpvar_33;
            tmpvar_33 = _unity_tbn_0_8.z;
            worldN_3.x = tmpvar_33;
            lowp float tmpvar_34;
            tmpvar_34 = _unity_tbn_1_7.z;
            worldN_3.y = tmpvar_34;
            lowp float tmpvar_35;
            tmpvar_35 = _unity_tbn_2_6.z;
            worldN_3.z = tmpvar_35;
            highp vec3 tmpvar_36;
            tmpvar_36 = normalize(worldN_3);
            worldN_3 = tmpvar_36;
            tmpvar_4 = tmpvar_36;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_5;
            mediump vec3 normalWorld_37;
            normalWorld_37 = tmpvar_4;
            mediump vec4 tmpvar_38;
            tmpvar_38.w = 1.0;
            tmpvar_38.xyz = normalWorld_37;
            mediump vec3 x_39;
            x_39.x = dot (unity_SHAr, tmpvar_38);
            x_39.y = dot (unity_SHAg, tmpvar_38);
            x_39.z = dot (unity_SHAb, tmpvar_38);
            mediump vec3 tmpvar_40;
            tmpvar_40 = max (((1.055 * 
              pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD7 + x_39)), vec3(0.4166667, 0.4166667, 0.4166667))
            ) - 0.055), vec3(0.0, 0.0, 0.0));
            lowp vec4 c_41;
            lowp vec4 c_42;
            lowp float diff_43;
            mediump float tmpvar_44;
            tmpvar_44 = max (0.0, dot (tmpvar_4, tmpvar_2));
            diff_43 = tmpvar_44;
            c_42.xyz = ((tmpvar_13 * tmpvar_1) * diff_43);
            c_42.w = tmpvar_14;
            c_41.w = c_42.w;
            c_41.xyz = (c_42.xyz + (tmpvar_13 * tmpvar_40));
            gl_FragData[0] = c_41;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier01"
        {
           Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" }
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesTANGENT;
          attribute vec4 _glesVertex;
          attribute vec4 _glesColor;
          attribute vec3 _glesNormal;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _ScreenParams;
          uniform mediump vec4 unity_SHBr;
          uniform mediump vec4 unity_SHBg;
          uniform mediump vec4 unity_SHBb;
          uniform mediump vec4 unity_SHC;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp vec4 unity_WorldTransformParams;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp float _FaceDilate;
          uniform highp mat4 _EnvMatrix;
          uniform highp float _WeightNormal;
          uniform highp float _WeightBold;
          uniform highp float _ScaleRatioA;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp float _GradientScale;
          uniform highp float _ScaleX;
          uniform highp float _ScaleY;
          uniform highp float _PerspectiveFilter;
          uniform highp vec4 _MainTex_ST;
          uniform highp vec4 _FaceTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec4 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD4;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD5;
          varying highp vec3 xlv_TEXCOORD6;
          varying mediump vec3 xlv_TEXCOORD7;
          void main ()
          {
            lowp vec3 worldBinormal_1;
            lowp float tangentSign_2;
            lowp vec3 worldTangent_3;
            highp vec4 tmpvar_4;
            highp vec4 tmpvar_5;
            highp vec3 tmpvar_6;
            highp vec4 tmpvar_7;
            tmpvar_5.zw = _glesVertex.zw;
            tmpvar_7.zw = _glesMultiTexCoord1.zw;
            highp vec2 tmpvar_8;
            highp float scale_9;
            highp vec2 pixelSize_10;
            tmpvar_5.x = (_glesVertex.x + _VertexOffsetX);
            tmpvar_5.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_11;
            tmpvar_11.w = 1.0;
            tmpvar_11.xyz = _WorldSpaceCameraPos;
            tmpvar_6 = (_glesNormal * sign(dot (_glesNormal, 
              ((unity_WorldToObject * tmpvar_11).xyz - tmpvar_5.xyz)
            )));
            highp vec4 tmpvar_12;
            tmpvar_12.w = 1.0;
            tmpvar_12.xyz = tmpvar_5.xyz;
            highp vec2 tmpvar_13;
            tmpvar_13.x = _ScaleX;
            tmpvar_13.y = _ScaleY;
            highp mat2 tmpvar_14;
            tmpvar_14[0] = glstate_matrix_projection[0].xy;
            tmpvar_14[1] = glstate_matrix_projection[1].xy;
            pixelSize_10 = ((unity_MatrixVP * (unity_ObjectToWorld * tmpvar_12)).ww / (tmpvar_13 * (tmpvar_14 * _ScreenParams.xy)));
            scale_9 = (inversesqrt(dot (pixelSize_10, pixelSize_10)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            highp mat3 tmpvar_15;
            tmpvar_15[0] = unity_WorldToObject[0].xyz;
            tmpvar_15[1] = unity_WorldToObject[1].xyz;
            tmpvar_15[2] = unity_WorldToObject[2].xyz;
            highp float tmpvar_16;
            tmpvar_16 = mix ((scale_9 * (1.0 - _PerspectiveFilter)), scale_9, abs(dot (
              normalize((tmpvar_6 * tmpvar_15))
            , 
              normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz))
            )));
            scale_9 = tmpvar_16;
            tmpvar_8.y = tmpvar_16;
            tmpvar_8.x = (((
              (mix (_WeightNormal, _WeightBold, float((0.0 >= _glesMultiTexCoord1.y))) / 4.0)
             + _FaceDilate) * _ScaleRatioA) * 0.5);
            highp vec2 xlat_varoutput_17;
            xlat_varoutput_17.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_17.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_17.x));
            tmpvar_7.xy = (xlat_varoutput_17 * 0.001953125);
            highp mat3 tmpvar_18;
            tmpvar_18[0] = _EnvMatrix[0].xyz;
            tmpvar_18[1] = _EnvMatrix[1].xyz;
            tmpvar_18[2] = _EnvMatrix[2].xyz;
            highp vec4 tmpvar_19;
            tmpvar_19.w = 1.0;
            tmpvar_19.xyz = tmpvar_5.xyz;
            tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
            tmpvar_4.zw = ((tmpvar_7.xy * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            highp vec3 tmpvar_20;
            tmpvar_20 = (unity_ObjectToWorld * tmpvar_5).xyz;
            highp mat3 tmpvar_21;
            tmpvar_21[0] = unity_WorldToObject[0].xyz;
            tmpvar_21[1] = unity_WorldToObject[1].xyz;
            tmpvar_21[2] = unity_WorldToObject[2].xyz;
            highp vec3 tmpvar_22;
            tmpvar_22 = normalize((tmpvar_6 * tmpvar_21));
            highp mat3 tmpvar_23;
            tmpvar_23[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_23[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_23[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_24;
            tmpvar_24 = normalize((tmpvar_23 * _glesTANGENT.xyz));
            worldTangent_3 = tmpvar_24;
            highp float tmpvar_25;
            tmpvar_25 = (_glesTANGENT.w * unity_WorldTransformParams.w);
            tangentSign_2 = tmpvar_25;
            highp vec3 tmpvar_26;
            tmpvar_26 = (((tmpvar_22.yzx * worldTangent_3.zxy) - (tmpvar_22.zxy * worldTangent_3.yzx)) * tangentSign_2);
            worldBinormal_1 = tmpvar_26;
            highp vec4 tmpvar_27;
            tmpvar_27.x = worldTangent_3.x;
            tmpvar_27.y = worldBinormal_1.x;
            tmpvar_27.z = tmpvar_22.x;
            tmpvar_27.w = tmpvar_20.x;
            highp vec4 tmpvar_28;
            tmpvar_28.x = worldTangent_3.y;
            tmpvar_28.y = worldBinormal_1.y;
            tmpvar_28.z = tmpvar_22.y;
            tmpvar_28.w = tmpvar_20.y;
            highp vec4 tmpvar_29;
            tmpvar_29.x = worldTangent_3.z;
            tmpvar_29.y = worldBinormal_1.z;
            tmpvar_29.z = tmpvar_22.z;
            tmpvar_29.w = tmpvar_20.z;
            mediump vec3 normal_30;
            normal_30 = tmpvar_22;
            mediump vec3 x1_31;
            mediump vec4 tmpvar_32;
            tmpvar_32 = (normal_30.xyzz * normal_30.yzzx);
            x1_31.x = dot (unity_SHBr, tmpvar_32);
            x1_31.y = dot (unity_SHBg, tmpvar_32);
            x1_31.z = dot (unity_SHBb, tmpvar_32);
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_19));
            xlv_TEXCOORD0 = tmpvar_4;
            xlv_TEXCOORD1 = ((tmpvar_7.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_27;
            xlv_TEXCOORD3 = tmpvar_28;
            xlv_TEXCOORD4 = tmpvar_29;
            xlv_COLOR0 = _glesColor;
            xlv_TEXCOORD5 = tmpvar_8;
            xlv_TEXCOORD6 = (tmpvar_18 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz));
            xlv_TEXCOORD7 = (x1_31 + (unity_SHC.xyz * (
              (normal_30.x * normal_30.x)
             - 
              (normal_30.y * normal_30.y)
            )));
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform mediump vec4 _WorldSpaceLightPos0;
          uniform mediump vec4 unity_SHAr;
          uniform mediump vec4 unity_SHAg;
          uniform mediump vec4 unity_SHAb;
          uniform lowp vec4 _LightColor0;
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
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec4 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD4;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD5;
          varying mediump vec3 xlv_TEXCOORD7;
          void main ()
          {
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp vec3 tmpvar_4;
            lowp vec3 lightDir_5;
            lowp vec3 _unity_tbn_2_6;
            lowp vec3 _unity_tbn_1_7;
            lowp vec3 _unity_tbn_0_8;
            highp vec3 tmpvar_9;
            tmpvar_9 = xlv_TEXCOORD2.xyz;
            _unity_tbn_0_8 = tmpvar_9;
            highp vec3 tmpvar_10;
            tmpvar_10 = xlv_TEXCOORD3.xyz;
            _unity_tbn_1_7 = tmpvar_10;
            highp vec3 tmpvar_11;
            tmpvar_11 = xlv_TEXCOORD4.xyz;
            _unity_tbn_2_6 = tmpvar_11;
            mediump vec3 tmpvar_12;
            tmpvar_12 = _WorldSpaceLightPos0.xyz;
            lightDir_5 = tmpvar_12;
            lowp vec3 tmpvar_13;
            lowp float tmpvar_14;
            highp vec4 outlineColor_15;
            highp vec4 faceColor_16;
            highp float c_17;
            lowp float tmpvar_18;
            tmpvar_18 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_17 = tmpvar_18;
            highp float tmpvar_19;
            tmpvar_19 = (((
              (0.5 - c_17)
             - xlv_TEXCOORD5.x) * xlv_TEXCOORD5.y) + 0.5);
            highp float tmpvar_20;
            tmpvar_20 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD5.y);
            highp float tmpvar_21;
            tmpvar_21 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD5.y);
            faceColor_16 = _FaceColor;
            outlineColor_15 = _OutlineColor;
            faceColor_16 = (faceColor_16 * xlv_COLOR0);
            outlineColor_15.w = (outlineColor_15.w * xlv_COLOR0.w);
            highp vec2 tmpvar_22;
            tmpvar_22.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_22.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_23;
            tmpvar_23 = texture2D (_FaceTex, tmpvar_22);
            faceColor_16 = (faceColor_16 * tmpvar_23);
            highp vec2 tmpvar_24;
            tmpvar_24.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_24.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_25;
            tmpvar_25 = texture2D (_OutlineTex, tmpvar_24);
            outlineColor_15 = (outlineColor_15 * tmpvar_25);
            mediump float d_26;
            d_26 = tmpvar_19;
            lowp vec4 faceColor_27;
            faceColor_27 = faceColor_16;
            lowp vec4 outlineColor_28;
            outlineColor_28 = outlineColor_15;
            mediump float outline_29;
            outline_29 = tmpvar_20;
            mediump float softness_30;
            softness_30 = tmpvar_21;
            mediump float tmpvar_31;
            tmpvar_31 = (1.0 - clamp ((
              ((d_26 - (outline_29 * 0.5)) + (softness_30 * 0.5))
             / 
              (1.0 + softness_30)
            ), 0.0, 1.0));
            faceColor_27.xyz = (faceColor_27.xyz * faceColor_27.w);
            outlineColor_28.xyz = (outlineColor_28.xyz * outlineColor_28.w);
            mediump vec4 tmpvar_32;
            tmpvar_32 = mix (faceColor_27, outlineColor_28, vec4((clamp (
              (d_26 + (outline_29 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_29)
            ))));
            faceColor_27 = tmpvar_32;
            faceColor_27 = (faceColor_27 * tmpvar_31);
            faceColor_16 = faceColor_27;
            faceColor_16.xyz = (faceColor_16.xyz / max (faceColor_16.w, 0.0001));
            tmpvar_13 = faceColor_16.xyz;
            tmpvar_14 = faceColor_16.w;
            lowp float tmpvar_33;
            tmpvar_33 = _unity_tbn_0_8.z;
            worldN_3.x = tmpvar_33;
            lowp float tmpvar_34;
            tmpvar_34 = _unity_tbn_1_7.z;
            worldN_3.y = tmpvar_34;
            lowp float tmpvar_35;
            tmpvar_35 = _unity_tbn_2_6.z;
            worldN_3.z = tmpvar_35;
            highp vec3 tmpvar_36;
            tmpvar_36 = normalize(worldN_3);
            worldN_3 = tmpvar_36;
            tmpvar_4 = tmpvar_36;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_5;
            mediump vec3 normalWorld_37;
            normalWorld_37 = tmpvar_4;
            mediump vec4 tmpvar_38;
            tmpvar_38.w = 1.0;
            tmpvar_38.xyz = normalWorld_37;
            mediump vec3 x_39;
            x_39.x = dot (unity_SHAr, tmpvar_38);
            x_39.y = dot (unity_SHAg, tmpvar_38);
            x_39.z = dot (unity_SHAb, tmpvar_38);
            mediump vec3 tmpvar_40;
            tmpvar_40 = max (((1.055 * 
              pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD7 + x_39)), vec3(0.4166667, 0.4166667, 0.4166667))
            ) - 0.055), vec3(0.0, 0.0, 0.0));
            lowp vec4 c_41;
            lowp vec4 c_42;
            lowp float diff_43;
            mediump float tmpvar_44;
            tmpvar_44 = max (0.0, dot (tmpvar_4, tmpvar_2));
            diff_43 = tmpvar_44;
            c_42.xyz = ((tmpvar_13 * tmpvar_1) * diff_43);
            c_42.w = tmpvar_14;
            c_41.w = c_42.w;
            c_41.xyz = (c_42.xyz + (tmpvar_13 * tmpvar_40));
            gl_FragData[0] = c_41;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier02"
        {
           Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" }
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesTANGENT;
          attribute vec4 _glesVertex;
          attribute vec4 _glesColor;
          attribute vec3 _glesNormal;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _ScreenParams;
          uniform mediump vec4 unity_SHBr;
          uniform mediump vec4 unity_SHBg;
          uniform mediump vec4 unity_SHBb;
          uniform mediump vec4 unity_SHC;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp vec4 unity_WorldTransformParams;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp float _FaceDilate;
          uniform highp mat4 _EnvMatrix;
          uniform highp float _WeightNormal;
          uniform highp float _WeightBold;
          uniform highp float _ScaleRatioA;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp float _GradientScale;
          uniform highp float _ScaleX;
          uniform highp float _ScaleY;
          uniform highp float _PerspectiveFilter;
          uniform highp vec4 _MainTex_ST;
          uniform highp vec4 _FaceTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec4 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD4;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD5;
          varying highp vec3 xlv_TEXCOORD6;
          varying mediump vec3 xlv_TEXCOORD7;
          void main ()
          {
            lowp vec3 worldBinormal_1;
            lowp float tangentSign_2;
            lowp vec3 worldTangent_3;
            highp vec4 tmpvar_4;
            highp vec4 tmpvar_5;
            highp vec3 tmpvar_6;
            highp vec4 tmpvar_7;
            tmpvar_5.zw = _glesVertex.zw;
            tmpvar_7.zw = _glesMultiTexCoord1.zw;
            highp vec2 tmpvar_8;
            highp float scale_9;
            highp vec2 pixelSize_10;
            tmpvar_5.x = (_glesVertex.x + _VertexOffsetX);
            tmpvar_5.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_11;
            tmpvar_11.w = 1.0;
            tmpvar_11.xyz = _WorldSpaceCameraPos;
            tmpvar_6 = (_glesNormal * sign(dot (_glesNormal, 
              ((unity_WorldToObject * tmpvar_11).xyz - tmpvar_5.xyz)
            )));
            highp vec4 tmpvar_12;
            tmpvar_12.w = 1.0;
            tmpvar_12.xyz = tmpvar_5.xyz;
            highp vec2 tmpvar_13;
            tmpvar_13.x = _ScaleX;
            tmpvar_13.y = _ScaleY;
            highp mat2 tmpvar_14;
            tmpvar_14[0] = glstate_matrix_projection[0].xy;
            tmpvar_14[1] = glstate_matrix_projection[1].xy;
            pixelSize_10 = ((unity_MatrixVP * (unity_ObjectToWorld * tmpvar_12)).ww / (tmpvar_13 * (tmpvar_14 * _ScreenParams.xy)));
            scale_9 = (inversesqrt(dot (pixelSize_10, pixelSize_10)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            highp mat3 tmpvar_15;
            tmpvar_15[0] = unity_WorldToObject[0].xyz;
            tmpvar_15[1] = unity_WorldToObject[1].xyz;
            tmpvar_15[2] = unity_WorldToObject[2].xyz;
            highp float tmpvar_16;
            tmpvar_16 = mix ((scale_9 * (1.0 - _PerspectiveFilter)), scale_9, abs(dot (
              normalize((tmpvar_6 * tmpvar_15))
            , 
              normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz))
            )));
            scale_9 = tmpvar_16;
            tmpvar_8.y = tmpvar_16;
            tmpvar_8.x = (((
              (mix (_WeightNormal, _WeightBold, float((0.0 >= _glesMultiTexCoord1.y))) / 4.0)
             + _FaceDilate) * _ScaleRatioA) * 0.5);
            highp vec2 xlat_varoutput_17;
            xlat_varoutput_17.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_17.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_17.x));
            tmpvar_7.xy = (xlat_varoutput_17 * 0.001953125);
            highp mat3 tmpvar_18;
            tmpvar_18[0] = _EnvMatrix[0].xyz;
            tmpvar_18[1] = _EnvMatrix[1].xyz;
            tmpvar_18[2] = _EnvMatrix[2].xyz;
            highp vec4 tmpvar_19;
            tmpvar_19.w = 1.0;
            tmpvar_19.xyz = tmpvar_5.xyz;
            tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
            tmpvar_4.zw = ((tmpvar_7.xy * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            highp vec3 tmpvar_20;
            tmpvar_20 = (unity_ObjectToWorld * tmpvar_5).xyz;
            highp mat3 tmpvar_21;
            tmpvar_21[0] = unity_WorldToObject[0].xyz;
            tmpvar_21[1] = unity_WorldToObject[1].xyz;
            tmpvar_21[2] = unity_WorldToObject[2].xyz;
            highp vec3 tmpvar_22;
            tmpvar_22 = normalize((tmpvar_6 * tmpvar_21));
            highp mat3 tmpvar_23;
            tmpvar_23[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_23[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_23[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_24;
            tmpvar_24 = normalize((tmpvar_23 * _glesTANGENT.xyz));
            worldTangent_3 = tmpvar_24;
            highp float tmpvar_25;
            tmpvar_25 = (_glesTANGENT.w * unity_WorldTransformParams.w);
            tangentSign_2 = tmpvar_25;
            highp vec3 tmpvar_26;
            tmpvar_26 = (((tmpvar_22.yzx * worldTangent_3.zxy) - (tmpvar_22.zxy * worldTangent_3.yzx)) * tangentSign_2);
            worldBinormal_1 = tmpvar_26;
            highp vec4 tmpvar_27;
            tmpvar_27.x = worldTangent_3.x;
            tmpvar_27.y = worldBinormal_1.x;
            tmpvar_27.z = tmpvar_22.x;
            tmpvar_27.w = tmpvar_20.x;
            highp vec4 tmpvar_28;
            tmpvar_28.x = worldTangent_3.y;
            tmpvar_28.y = worldBinormal_1.y;
            tmpvar_28.z = tmpvar_22.y;
            tmpvar_28.w = tmpvar_20.y;
            highp vec4 tmpvar_29;
            tmpvar_29.x = worldTangent_3.z;
            tmpvar_29.y = worldBinormal_1.z;
            tmpvar_29.z = tmpvar_22.z;
            tmpvar_29.w = tmpvar_20.z;
            mediump vec3 normal_30;
            normal_30 = tmpvar_22;
            mediump vec3 x1_31;
            mediump vec4 tmpvar_32;
            tmpvar_32 = (normal_30.xyzz * normal_30.yzzx);
            x1_31.x = dot (unity_SHBr, tmpvar_32);
            x1_31.y = dot (unity_SHBg, tmpvar_32);
            x1_31.z = dot (unity_SHBb, tmpvar_32);
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_19));
            xlv_TEXCOORD0 = tmpvar_4;
            xlv_TEXCOORD1 = ((tmpvar_7.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_27;
            xlv_TEXCOORD3 = tmpvar_28;
            xlv_TEXCOORD4 = tmpvar_29;
            xlv_COLOR0 = _glesColor;
            xlv_TEXCOORD5 = tmpvar_8;
            xlv_TEXCOORD6 = (tmpvar_18 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz));
            xlv_TEXCOORD7 = (x1_31 + (unity_SHC.xyz * (
              (normal_30.x * normal_30.x)
             - 
              (normal_30.y * normal_30.y)
            )));
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform mediump vec4 _WorldSpaceLightPos0;
          uniform mediump vec4 unity_SHAr;
          uniform mediump vec4 unity_SHAg;
          uniform mediump vec4 unity_SHAb;
          uniform lowp vec4 _LightColor0;
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
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec4 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD4;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD5;
          varying mediump vec3 xlv_TEXCOORD7;
          void main ()
          {
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp vec3 tmpvar_4;
            lowp vec3 lightDir_5;
            lowp vec3 _unity_tbn_2_6;
            lowp vec3 _unity_tbn_1_7;
            lowp vec3 _unity_tbn_0_8;
            highp vec3 tmpvar_9;
            tmpvar_9 = xlv_TEXCOORD2.xyz;
            _unity_tbn_0_8 = tmpvar_9;
            highp vec3 tmpvar_10;
            tmpvar_10 = xlv_TEXCOORD3.xyz;
            _unity_tbn_1_7 = tmpvar_10;
            highp vec3 tmpvar_11;
            tmpvar_11 = xlv_TEXCOORD4.xyz;
            _unity_tbn_2_6 = tmpvar_11;
            mediump vec3 tmpvar_12;
            tmpvar_12 = _WorldSpaceLightPos0.xyz;
            lightDir_5 = tmpvar_12;
            lowp vec3 tmpvar_13;
            lowp float tmpvar_14;
            highp vec4 outlineColor_15;
            highp vec4 faceColor_16;
            highp float c_17;
            lowp float tmpvar_18;
            tmpvar_18 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_17 = tmpvar_18;
            highp float tmpvar_19;
            tmpvar_19 = (((
              (0.5 - c_17)
             - xlv_TEXCOORD5.x) * xlv_TEXCOORD5.y) + 0.5);
            highp float tmpvar_20;
            tmpvar_20 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD5.y);
            highp float tmpvar_21;
            tmpvar_21 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD5.y);
            faceColor_16 = _FaceColor;
            outlineColor_15 = _OutlineColor;
            faceColor_16 = (faceColor_16 * xlv_COLOR0);
            outlineColor_15.w = (outlineColor_15.w * xlv_COLOR0.w);
            highp vec2 tmpvar_22;
            tmpvar_22.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_22.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_23;
            tmpvar_23 = texture2D (_FaceTex, tmpvar_22);
            faceColor_16 = (faceColor_16 * tmpvar_23);
            highp vec2 tmpvar_24;
            tmpvar_24.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_24.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_25;
            tmpvar_25 = texture2D (_OutlineTex, tmpvar_24);
            outlineColor_15 = (outlineColor_15 * tmpvar_25);
            mediump float d_26;
            d_26 = tmpvar_19;
            lowp vec4 faceColor_27;
            faceColor_27 = faceColor_16;
            lowp vec4 outlineColor_28;
            outlineColor_28 = outlineColor_15;
            mediump float outline_29;
            outline_29 = tmpvar_20;
            mediump float softness_30;
            softness_30 = tmpvar_21;
            mediump float tmpvar_31;
            tmpvar_31 = (1.0 - clamp ((
              ((d_26 - (outline_29 * 0.5)) + (softness_30 * 0.5))
             / 
              (1.0 + softness_30)
            ), 0.0, 1.0));
            faceColor_27.xyz = (faceColor_27.xyz * faceColor_27.w);
            outlineColor_28.xyz = (outlineColor_28.xyz * outlineColor_28.w);
            mediump vec4 tmpvar_32;
            tmpvar_32 = mix (faceColor_27, outlineColor_28, vec4((clamp (
              (d_26 + (outline_29 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_29)
            ))));
            faceColor_27 = tmpvar_32;
            faceColor_27 = (faceColor_27 * tmpvar_31);
            faceColor_16 = faceColor_27;
            faceColor_16.xyz = (faceColor_16.xyz / max (faceColor_16.w, 0.0001));
            tmpvar_13 = faceColor_16.xyz;
            tmpvar_14 = faceColor_16.w;
            lowp float tmpvar_33;
            tmpvar_33 = _unity_tbn_0_8.z;
            worldN_3.x = tmpvar_33;
            lowp float tmpvar_34;
            tmpvar_34 = _unity_tbn_1_7.z;
            worldN_3.y = tmpvar_34;
            lowp float tmpvar_35;
            tmpvar_35 = _unity_tbn_2_6.z;
            worldN_3.z = tmpvar_35;
            highp vec3 tmpvar_36;
            tmpvar_36 = normalize(worldN_3);
            worldN_3 = tmpvar_36;
            tmpvar_4 = tmpvar_36;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_5;
            mediump vec3 normalWorld_37;
            normalWorld_37 = tmpvar_4;
            mediump vec4 tmpvar_38;
            tmpvar_38.w = 1.0;
            tmpvar_38.xyz = normalWorld_37;
            mediump vec3 x_39;
            x_39.x = dot (unity_SHAr, tmpvar_38);
            x_39.y = dot (unity_SHAg, tmpvar_38);
            x_39.z = dot (unity_SHAb, tmpvar_38);
            mediump vec3 tmpvar_40;
            tmpvar_40 = max (((1.055 * 
              pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD7 + x_39)), vec3(0.4166667, 0.4166667, 0.4166667))
            ) - 0.055), vec3(0.0, 0.0, 0.0));
            lowp vec4 c_41;
            lowp vec4 c_42;
            lowp float diff_43;
            mediump float tmpvar_44;
            tmpvar_44 = max (0.0, dot (tmpvar_4, tmpvar_2));
            diff_43 = tmpvar_44;
            c_42.xyz = ((tmpvar_13 * tmpvar_1) * diff_43);
            c_42.w = tmpvar_14;
            c_41.w = c_42.w;
            c_41.xyz = (c_42.xyz + (tmpvar_13 * tmpvar_40));
            gl_FragData[0] = c_41;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier00"
        {
           Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "VERTEXLIGHT_ON" }
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesTANGENT;
          attribute vec4 _glesVertex;
          attribute vec4 _glesColor;
          attribute vec3 _glesNormal;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _ScreenParams;
          uniform highp vec4 unity_4LightPosX0;
          uniform highp vec4 unity_4LightPosY0;
          uniform highp vec4 unity_4LightPosZ0;
          uniform mediump vec4 unity_4LightAtten0;
          uniform mediump vec4 unity_LightColor[8];
          uniform mediump vec4 unity_SHBr;
          uniform mediump vec4 unity_SHBg;
          uniform mediump vec4 unity_SHBb;
          uniform mediump vec4 unity_SHC;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp vec4 unity_WorldTransformParams;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp float _FaceDilate;
          uniform highp mat4 _EnvMatrix;
          uniform highp float _WeightNormal;
          uniform highp float _WeightBold;
          uniform highp float _ScaleRatioA;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp float _GradientScale;
          uniform highp float _ScaleX;
          uniform highp float _ScaleY;
          uniform highp float _PerspectiveFilter;
          uniform highp vec4 _MainTex_ST;
          uniform highp vec4 _FaceTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec4 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD4;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD5;
          varying highp vec3 xlv_TEXCOORD6;
          varying mediump vec3 xlv_TEXCOORD7;
          void main ()
          {
            lowp vec3 worldBinormal_1;
            lowp float tangentSign_2;
            lowp vec3 worldTangent_3;
            highp vec4 tmpvar_4;
            mediump vec3 tmpvar_5;
            highp vec4 tmpvar_6;
            highp vec3 tmpvar_7;
            highp vec4 tmpvar_8;
            tmpvar_6.zw = _glesVertex.zw;
            tmpvar_8.zw = _glesMultiTexCoord1.zw;
            highp vec2 tmpvar_9;
            highp float scale_10;
            highp vec2 pixelSize_11;
            tmpvar_6.x = (_glesVertex.x + _VertexOffsetX);
            tmpvar_6.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_12;
            tmpvar_12.w = 1.0;
            tmpvar_12.xyz = _WorldSpaceCameraPos;
            tmpvar_7 = (_glesNormal * sign(dot (_glesNormal, 
              ((unity_WorldToObject * tmpvar_12).xyz - tmpvar_6.xyz)
            )));
            highp vec4 tmpvar_13;
            tmpvar_13.w = 1.0;
            tmpvar_13.xyz = tmpvar_6.xyz;
            highp vec2 tmpvar_14;
            tmpvar_14.x = _ScaleX;
            tmpvar_14.y = _ScaleY;
            highp mat2 tmpvar_15;
            tmpvar_15[0] = glstate_matrix_projection[0].xy;
            tmpvar_15[1] = glstate_matrix_projection[1].xy;
            pixelSize_11 = ((unity_MatrixVP * (unity_ObjectToWorld * tmpvar_13)).ww / (tmpvar_14 * (tmpvar_15 * _ScreenParams.xy)));
            scale_10 = (inversesqrt(dot (pixelSize_11, pixelSize_11)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            highp mat3 tmpvar_16;
            tmpvar_16[0] = unity_WorldToObject[0].xyz;
            tmpvar_16[1] = unity_WorldToObject[1].xyz;
            tmpvar_16[2] = unity_WorldToObject[2].xyz;
            highp float tmpvar_17;
            tmpvar_17 = mix ((scale_10 * (1.0 - _PerspectiveFilter)), scale_10, abs(dot (
              normalize((tmpvar_7 * tmpvar_16))
            , 
              normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_6).xyz))
            )));
            scale_10 = tmpvar_17;
            tmpvar_9.y = tmpvar_17;
            tmpvar_9.x = (((
              (mix (_WeightNormal, _WeightBold, float((0.0 >= _glesMultiTexCoord1.y))) / 4.0)
             + _FaceDilate) * _ScaleRatioA) * 0.5);
            highp vec2 xlat_varoutput_18;
            xlat_varoutput_18.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_18.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_18.x));
            tmpvar_8.xy = (xlat_varoutput_18 * 0.001953125);
            highp mat3 tmpvar_19;
            tmpvar_19[0] = _EnvMatrix[0].xyz;
            tmpvar_19[1] = _EnvMatrix[1].xyz;
            tmpvar_19[2] = _EnvMatrix[2].xyz;
            highp vec4 tmpvar_20;
            tmpvar_20.w = 1.0;
            tmpvar_20.xyz = tmpvar_6.xyz;
            tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
            tmpvar_4.zw = ((tmpvar_8.xy * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            highp vec3 tmpvar_21;
            tmpvar_21 = (unity_ObjectToWorld * tmpvar_6).xyz;
            highp mat3 tmpvar_22;
            tmpvar_22[0] = unity_WorldToObject[0].xyz;
            tmpvar_22[1] = unity_WorldToObject[1].xyz;
            tmpvar_22[2] = unity_WorldToObject[2].xyz;
            highp vec3 tmpvar_23;
            tmpvar_23 = normalize((tmpvar_7 * tmpvar_22));
            highp mat3 tmpvar_24;
            tmpvar_24[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_24[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_24[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_25;
            tmpvar_25 = normalize((tmpvar_24 * _glesTANGENT.xyz));
            worldTangent_3 = tmpvar_25;
            highp float tmpvar_26;
            tmpvar_26 = (_glesTANGENT.w * unity_WorldTransformParams.w);
            tangentSign_2 = tmpvar_26;
            highp vec3 tmpvar_27;
            tmpvar_27 = (((tmpvar_23.yzx * worldTangent_3.zxy) - (tmpvar_23.zxy * worldTangent_3.yzx)) * tangentSign_2);
            worldBinormal_1 = tmpvar_27;
            highp vec4 tmpvar_28;
            tmpvar_28.x = worldTangent_3.x;
            tmpvar_28.y = worldBinormal_1.x;
            tmpvar_28.z = tmpvar_23.x;
            tmpvar_28.w = tmpvar_21.x;
            highp vec4 tmpvar_29;
            tmpvar_29.x = worldTangent_3.y;
            tmpvar_29.y = worldBinormal_1.y;
            tmpvar_29.z = tmpvar_23.y;
            tmpvar_29.w = tmpvar_21.y;
            highp vec4 tmpvar_30;
            tmpvar_30.x = worldTangent_3.z;
            tmpvar_30.y = worldBinormal_1.z;
            tmpvar_30.z = tmpvar_23.z;
            tmpvar_30.w = tmpvar_21.z;
            highp vec3 lightColor0_31;
            lightColor0_31 = unity_LightColor[0].xyz;
            highp vec3 lightColor1_32;
            lightColor1_32 = unity_LightColor[1].xyz;
            highp vec3 lightColor2_33;
            lightColor2_33 = unity_LightColor[2].xyz;
            highp vec3 lightColor3_34;
            lightColor3_34 = unity_LightColor[3].xyz;
            highp vec4 lightAttenSq_35;
            lightAttenSq_35 = unity_4LightAtten0;
            highp vec3 col_36;
            highp vec4 ndotl_37;
            highp vec4 lengthSq_38;
            highp vec4 tmpvar_39;
            tmpvar_39 = (unity_4LightPosX0 - tmpvar_21.x);
            highp vec4 tmpvar_40;
            tmpvar_40 = (unity_4LightPosY0 - tmpvar_21.y);
            highp vec4 tmpvar_41;
            tmpvar_41 = (unity_4LightPosZ0 - tmpvar_21.z);
            lengthSq_38 = (tmpvar_39 * tmpvar_39);
            lengthSq_38 = (lengthSq_38 + (tmpvar_40 * tmpvar_40));
            lengthSq_38 = (lengthSq_38 + (tmpvar_41 * tmpvar_41));
            highp vec4 tmpvar_42;
            tmpvar_42 = max (lengthSq_38, vec4(1e-6, 1e-6, 1e-6, 1e-6));
            lengthSq_38 = tmpvar_42;
            ndotl_37 = (tmpvar_39 * tmpvar_23.x);
            ndotl_37 = (ndotl_37 + (tmpvar_40 * tmpvar_23.y));
            ndotl_37 = (ndotl_37 + (tmpvar_41 * tmpvar_23.z));
            highp vec4 tmpvar_43;
            tmpvar_43 = max (vec4(0.0, 0.0, 0.0, 0.0), (ndotl_37 * inversesqrt(tmpvar_42)));
            ndotl_37 = tmpvar_43;
            highp vec4 tmpvar_44;
            tmpvar_44 = (tmpvar_43 * (1.0/((1.0 + 
              (tmpvar_42 * lightAttenSq_35)
            ))));
            col_36 = (lightColor0_31 * tmpvar_44.x);
            col_36 = (col_36 + (lightColor1_32 * tmpvar_44.y));
            col_36 = (col_36 + (lightColor2_33 * tmpvar_44.z));
            col_36 = (col_36 + (lightColor3_34 * tmpvar_44.w));
            tmpvar_5 = col_36;
            mediump vec3 normal_45;
            normal_45 = tmpvar_23;
            mediump vec3 ambient_46;
            mediump vec3 x1_47;
            mediump vec4 tmpvar_48;
            tmpvar_48 = (normal_45.xyzz * normal_45.yzzx);
            x1_47.x = dot (unity_SHBr, tmpvar_48);
            x1_47.y = dot (unity_SHBg, tmpvar_48);
            x1_47.z = dot (unity_SHBb, tmpvar_48);
            ambient_46 = ((tmpvar_5 * (
              (tmpvar_5 * ((tmpvar_5 * 0.305306) + 0.6821711))
             + 0.01252288)) + (x1_47 + (unity_SHC.xyz * 
              ((normal_45.x * normal_45.x) - (normal_45.y * normal_45.y))
            )));
            tmpvar_5 = ambient_46;
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_20));
            xlv_TEXCOORD0 = tmpvar_4;
            xlv_TEXCOORD1 = ((tmpvar_8.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_28;
            xlv_TEXCOORD3 = tmpvar_29;
            xlv_TEXCOORD4 = tmpvar_30;
            xlv_COLOR0 = _glesColor;
            xlv_TEXCOORD5 = tmpvar_9;
            xlv_TEXCOORD6 = (tmpvar_19 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_6).xyz));
            xlv_TEXCOORD7 = ambient_46;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform mediump vec4 _WorldSpaceLightPos0;
          uniform mediump vec4 unity_SHAr;
          uniform mediump vec4 unity_SHAg;
          uniform mediump vec4 unity_SHAb;
          uniform lowp vec4 _LightColor0;
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
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec4 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD4;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD5;
          varying mediump vec3 xlv_TEXCOORD7;
          void main ()
          {
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp vec3 tmpvar_4;
            lowp vec3 lightDir_5;
            lowp vec3 _unity_tbn_2_6;
            lowp vec3 _unity_tbn_1_7;
            lowp vec3 _unity_tbn_0_8;
            highp vec3 tmpvar_9;
            tmpvar_9 = xlv_TEXCOORD2.xyz;
            _unity_tbn_0_8 = tmpvar_9;
            highp vec3 tmpvar_10;
            tmpvar_10 = xlv_TEXCOORD3.xyz;
            _unity_tbn_1_7 = tmpvar_10;
            highp vec3 tmpvar_11;
            tmpvar_11 = xlv_TEXCOORD4.xyz;
            _unity_tbn_2_6 = tmpvar_11;
            mediump vec3 tmpvar_12;
            tmpvar_12 = _WorldSpaceLightPos0.xyz;
            lightDir_5 = tmpvar_12;
            lowp vec3 tmpvar_13;
            lowp float tmpvar_14;
            highp vec4 outlineColor_15;
            highp vec4 faceColor_16;
            highp float c_17;
            lowp float tmpvar_18;
            tmpvar_18 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_17 = tmpvar_18;
            highp float tmpvar_19;
            tmpvar_19 = (((
              (0.5 - c_17)
             - xlv_TEXCOORD5.x) * xlv_TEXCOORD5.y) + 0.5);
            highp float tmpvar_20;
            tmpvar_20 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD5.y);
            highp float tmpvar_21;
            tmpvar_21 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD5.y);
            faceColor_16 = _FaceColor;
            outlineColor_15 = _OutlineColor;
            faceColor_16 = (faceColor_16 * xlv_COLOR0);
            outlineColor_15.w = (outlineColor_15.w * xlv_COLOR0.w);
            highp vec2 tmpvar_22;
            tmpvar_22.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_22.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_23;
            tmpvar_23 = texture2D (_FaceTex, tmpvar_22);
            faceColor_16 = (faceColor_16 * tmpvar_23);
            highp vec2 tmpvar_24;
            tmpvar_24.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_24.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_25;
            tmpvar_25 = texture2D (_OutlineTex, tmpvar_24);
            outlineColor_15 = (outlineColor_15 * tmpvar_25);
            mediump float d_26;
            d_26 = tmpvar_19;
            lowp vec4 faceColor_27;
            faceColor_27 = faceColor_16;
            lowp vec4 outlineColor_28;
            outlineColor_28 = outlineColor_15;
            mediump float outline_29;
            outline_29 = tmpvar_20;
            mediump float softness_30;
            softness_30 = tmpvar_21;
            mediump float tmpvar_31;
            tmpvar_31 = (1.0 - clamp ((
              ((d_26 - (outline_29 * 0.5)) + (softness_30 * 0.5))
             / 
              (1.0 + softness_30)
            ), 0.0, 1.0));
            faceColor_27.xyz = (faceColor_27.xyz * faceColor_27.w);
            outlineColor_28.xyz = (outlineColor_28.xyz * outlineColor_28.w);
            mediump vec4 tmpvar_32;
            tmpvar_32 = mix (faceColor_27, outlineColor_28, vec4((clamp (
              (d_26 + (outline_29 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_29)
            ))));
            faceColor_27 = tmpvar_32;
            faceColor_27 = (faceColor_27 * tmpvar_31);
            faceColor_16 = faceColor_27;
            faceColor_16.xyz = (faceColor_16.xyz / max (faceColor_16.w, 0.0001));
            tmpvar_13 = faceColor_16.xyz;
            tmpvar_14 = faceColor_16.w;
            lowp float tmpvar_33;
            tmpvar_33 = _unity_tbn_0_8.z;
            worldN_3.x = tmpvar_33;
            lowp float tmpvar_34;
            tmpvar_34 = _unity_tbn_1_7.z;
            worldN_3.y = tmpvar_34;
            lowp float tmpvar_35;
            tmpvar_35 = _unity_tbn_2_6.z;
            worldN_3.z = tmpvar_35;
            highp vec3 tmpvar_36;
            tmpvar_36 = normalize(worldN_3);
            worldN_3 = tmpvar_36;
            tmpvar_4 = tmpvar_36;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_5;
            mediump vec3 normalWorld_37;
            normalWorld_37 = tmpvar_4;
            mediump vec4 tmpvar_38;
            tmpvar_38.w = 1.0;
            tmpvar_38.xyz = normalWorld_37;
            mediump vec3 x_39;
            x_39.x = dot (unity_SHAr, tmpvar_38);
            x_39.y = dot (unity_SHAg, tmpvar_38);
            x_39.z = dot (unity_SHAb, tmpvar_38);
            mediump vec3 tmpvar_40;
            tmpvar_40 = max (((1.055 * 
              pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD7 + x_39)), vec3(0.4166667, 0.4166667, 0.4166667))
            ) - 0.055), vec3(0.0, 0.0, 0.0));
            lowp vec4 c_41;
            lowp vec4 c_42;
            lowp float diff_43;
            mediump float tmpvar_44;
            tmpvar_44 = max (0.0, dot (tmpvar_4, tmpvar_2));
            diff_43 = tmpvar_44;
            c_42.xyz = ((tmpvar_13 * tmpvar_1) * diff_43);
            c_42.w = tmpvar_14;
            c_41.w = c_42.w;
            c_41.xyz = (c_42.xyz + (tmpvar_13 * tmpvar_40));
            gl_FragData[0] = c_41;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier01"
        {
           Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "VERTEXLIGHT_ON" }
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesTANGENT;
          attribute vec4 _glesVertex;
          attribute vec4 _glesColor;
          attribute vec3 _glesNormal;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _ScreenParams;
          uniform highp vec4 unity_4LightPosX0;
          uniform highp vec4 unity_4LightPosY0;
          uniform highp vec4 unity_4LightPosZ0;
          uniform mediump vec4 unity_4LightAtten0;
          uniform mediump vec4 unity_LightColor[8];
          uniform mediump vec4 unity_SHBr;
          uniform mediump vec4 unity_SHBg;
          uniform mediump vec4 unity_SHBb;
          uniform mediump vec4 unity_SHC;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp vec4 unity_WorldTransformParams;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp float _FaceDilate;
          uniform highp mat4 _EnvMatrix;
          uniform highp float _WeightNormal;
          uniform highp float _WeightBold;
          uniform highp float _ScaleRatioA;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp float _GradientScale;
          uniform highp float _ScaleX;
          uniform highp float _ScaleY;
          uniform highp float _PerspectiveFilter;
          uniform highp vec4 _MainTex_ST;
          uniform highp vec4 _FaceTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec4 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD4;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD5;
          varying highp vec3 xlv_TEXCOORD6;
          varying mediump vec3 xlv_TEXCOORD7;
          void main ()
          {
            lowp vec3 worldBinormal_1;
            lowp float tangentSign_2;
            lowp vec3 worldTangent_3;
            highp vec4 tmpvar_4;
            mediump vec3 tmpvar_5;
            highp vec4 tmpvar_6;
            highp vec3 tmpvar_7;
            highp vec4 tmpvar_8;
            tmpvar_6.zw = _glesVertex.zw;
            tmpvar_8.zw = _glesMultiTexCoord1.zw;
            highp vec2 tmpvar_9;
            highp float scale_10;
            highp vec2 pixelSize_11;
            tmpvar_6.x = (_glesVertex.x + _VertexOffsetX);
            tmpvar_6.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_12;
            tmpvar_12.w = 1.0;
            tmpvar_12.xyz = _WorldSpaceCameraPos;
            tmpvar_7 = (_glesNormal * sign(dot (_glesNormal, 
              ((unity_WorldToObject * tmpvar_12).xyz - tmpvar_6.xyz)
            )));
            highp vec4 tmpvar_13;
            tmpvar_13.w = 1.0;
            tmpvar_13.xyz = tmpvar_6.xyz;
            highp vec2 tmpvar_14;
            tmpvar_14.x = _ScaleX;
            tmpvar_14.y = _ScaleY;
            highp mat2 tmpvar_15;
            tmpvar_15[0] = glstate_matrix_projection[0].xy;
            tmpvar_15[1] = glstate_matrix_projection[1].xy;
            pixelSize_11 = ((unity_MatrixVP * (unity_ObjectToWorld * tmpvar_13)).ww / (tmpvar_14 * (tmpvar_15 * _ScreenParams.xy)));
            scale_10 = (inversesqrt(dot (pixelSize_11, pixelSize_11)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            highp mat3 tmpvar_16;
            tmpvar_16[0] = unity_WorldToObject[0].xyz;
            tmpvar_16[1] = unity_WorldToObject[1].xyz;
            tmpvar_16[2] = unity_WorldToObject[2].xyz;
            highp float tmpvar_17;
            tmpvar_17 = mix ((scale_10 * (1.0 - _PerspectiveFilter)), scale_10, abs(dot (
              normalize((tmpvar_7 * tmpvar_16))
            , 
              normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_6).xyz))
            )));
            scale_10 = tmpvar_17;
            tmpvar_9.y = tmpvar_17;
            tmpvar_9.x = (((
              (mix (_WeightNormal, _WeightBold, float((0.0 >= _glesMultiTexCoord1.y))) / 4.0)
             + _FaceDilate) * _ScaleRatioA) * 0.5);
            highp vec2 xlat_varoutput_18;
            xlat_varoutput_18.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_18.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_18.x));
            tmpvar_8.xy = (xlat_varoutput_18 * 0.001953125);
            highp mat3 tmpvar_19;
            tmpvar_19[0] = _EnvMatrix[0].xyz;
            tmpvar_19[1] = _EnvMatrix[1].xyz;
            tmpvar_19[2] = _EnvMatrix[2].xyz;
            highp vec4 tmpvar_20;
            tmpvar_20.w = 1.0;
            tmpvar_20.xyz = tmpvar_6.xyz;
            tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
            tmpvar_4.zw = ((tmpvar_8.xy * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            highp vec3 tmpvar_21;
            tmpvar_21 = (unity_ObjectToWorld * tmpvar_6).xyz;
            highp mat3 tmpvar_22;
            tmpvar_22[0] = unity_WorldToObject[0].xyz;
            tmpvar_22[1] = unity_WorldToObject[1].xyz;
            tmpvar_22[2] = unity_WorldToObject[2].xyz;
            highp vec3 tmpvar_23;
            tmpvar_23 = normalize((tmpvar_7 * tmpvar_22));
            highp mat3 tmpvar_24;
            tmpvar_24[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_24[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_24[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_25;
            tmpvar_25 = normalize((tmpvar_24 * _glesTANGENT.xyz));
            worldTangent_3 = tmpvar_25;
            highp float tmpvar_26;
            tmpvar_26 = (_glesTANGENT.w * unity_WorldTransformParams.w);
            tangentSign_2 = tmpvar_26;
            highp vec3 tmpvar_27;
            tmpvar_27 = (((tmpvar_23.yzx * worldTangent_3.zxy) - (tmpvar_23.zxy * worldTangent_3.yzx)) * tangentSign_2);
            worldBinormal_1 = tmpvar_27;
            highp vec4 tmpvar_28;
            tmpvar_28.x = worldTangent_3.x;
            tmpvar_28.y = worldBinormal_1.x;
            tmpvar_28.z = tmpvar_23.x;
            tmpvar_28.w = tmpvar_21.x;
            highp vec4 tmpvar_29;
            tmpvar_29.x = worldTangent_3.y;
            tmpvar_29.y = worldBinormal_1.y;
            tmpvar_29.z = tmpvar_23.y;
            tmpvar_29.w = tmpvar_21.y;
            highp vec4 tmpvar_30;
            tmpvar_30.x = worldTangent_3.z;
            tmpvar_30.y = worldBinormal_1.z;
            tmpvar_30.z = tmpvar_23.z;
            tmpvar_30.w = tmpvar_21.z;
            highp vec3 lightColor0_31;
            lightColor0_31 = unity_LightColor[0].xyz;
            highp vec3 lightColor1_32;
            lightColor1_32 = unity_LightColor[1].xyz;
            highp vec3 lightColor2_33;
            lightColor2_33 = unity_LightColor[2].xyz;
            highp vec3 lightColor3_34;
            lightColor3_34 = unity_LightColor[3].xyz;
            highp vec4 lightAttenSq_35;
            lightAttenSq_35 = unity_4LightAtten0;
            highp vec3 col_36;
            highp vec4 ndotl_37;
            highp vec4 lengthSq_38;
            highp vec4 tmpvar_39;
            tmpvar_39 = (unity_4LightPosX0 - tmpvar_21.x);
            highp vec4 tmpvar_40;
            tmpvar_40 = (unity_4LightPosY0 - tmpvar_21.y);
            highp vec4 tmpvar_41;
            tmpvar_41 = (unity_4LightPosZ0 - tmpvar_21.z);
            lengthSq_38 = (tmpvar_39 * tmpvar_39);
            lengthSq_38 = (lengthSq_38 + (tmpvar_40 * tmpvar_40));
            lengthSq_38 = (lengthSq_38 + (tmpvar_41 * tmpvar_41));
            highp vec4 tmpvar_42;
            tmpvar_42 = max (lengthSq_38, vec4(1e-6, 1e-6, 1e-6, 1e-6));
            lengthSq_38 = tmpvar_42;
            ndotl_37 = (tmpvar_39 * tmpvar_23.x);
            ndotl_37 = (ndotl_37 + (tmpvar_40 * tmpvar_23.y));
            ndotl_37 = (ndotl_37 + (tmpvar_41 * tmpvar_23.z));
            highp vec4 tmpvar_43;
            tmpvar_43 = max (vec4(0.0, 0.0, 0.0, 0.0), (ndotl_37 * inversesqrt(tmpvar_42)));
            ndotl_37 = tmpvar_43;
            highp vec4 tmpvar_44;
            tmpvar_44 = (tmpvar_43 * (1.0/((1.0 + 
              (tmpvar_42 * lightAttenSq_35)
            ))));
            col_36 = (lightColor0_31 * tmpvar_44.x);
            col_36 = (col_36 + (lightColor1_32 * tmpvar_44.y));
            col_36 = (col_36 + (lightColor2_33 * tmpvar_44.z));
            col_36 = (col_36 + (lightColor3_34 * tmpvar_44.w));
            tmpvar_5 = col_36;
            mediump vec3 normal_45;
            normal_45 = tmpvar_23;
            mediump vec3 ambient_46;
            mediump vec3 x1_47;
            mediump vec4 tmpvar_48;
            tmpvar_48 = (normal_45.xyzz * normal_45.yzzx);
            x1_47.x = dot (unity_SHBr, tmpvar_48);
            x1_47.y = dot (unity_SHBg, tmpvar_48);
            x1_47.z = dot (unity_SHBb, tmpvar_48);
            ambient_46 = ((tmpvar_5 * (
              (tmpvar_5 * ((tmpvar_5 * 0.305306) + 0.6821711))
             + 0.01252288)) + (x1_47 + (unity_SHC.xyz * 
              ((normal_45.x * normal_45.x) - (normal_45.y * normal_45.y))
            )));
            tmpvar_5 = ambient_46;
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_20));
            xlv_TEXCOORD0 = tmpvar_4;
            xlv_TEXCOORD1 = ((tmpvar_8.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_28;
            xlv_TEXCOORD3 = tmpvar_29;
            xlv_TEXCOORD4 = tmpvar_30;
            xlv_COLOR0 = _glesColor;
            xlv_TEXCOORD5 = tmpvar_9;
            xlv_TEXCOORD6 = (tmpvar_19 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_6).xyz));
            xlv_TEXCOORD7 = ambient_46;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform mediump vec4 _WorldSpaceLightPos0;
          uniform mediump vec4 unity_SHAr;
          uniform mediump vec4 unity_SHAg;
          uniform mediump vec4 unity_SHAb;
          uniform lowp vec4 _LightColor0;
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
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec4 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD4;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD5;
          varying mediump vec3 xlv_TEXCOORD7;
          void main ()
          {
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp vec3 tmpvar_4;
            lowp vec3 lightDir_5;
            lowp vec3 _unity_tbn_2_6;
            lowp vec3 _unity_tbn_1_7;
            lowp vec3 _unity_tbn_0_8;
            highp vec3 tmpvar_9;
            tmpvar_9 = xlv_TEXCOORD2.xyz;
            _unity_tbn_0_8 = tmpvar_9;
            highp vec3 tmpvar_10;
            tmpvar_10 = xlv_TEXCOORD3.xyz;
            _unity_tbn_1_7 = tmpvar_10;
            highp vec3 tmpvar_11;
            tmpvar_11 = xlv_TEXCOORD4.xyz;
            _unity_tbn_2_6 = tmpvar_11;
            mediump vec3 tmpvar_12;
            tmpvar_12 = _WorldSpaceLightPos0.xyz;
            lightDir_5 = tmpvar_12;
            lowp vec3 tmpvar_13;
            lowp float tmpvar_14;
            highp vec4 outlineColor_15;
            highp vec4 faceColor_16;
            highp float c_17;
            lowp float tmpvar_18;
            tmpvar_18 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_17 = tmpvar_18;
            highp float tmpvar_19;
            tmpvar_19 = (((
              (0.5 - c_17)
             - xlv_TEXCOORD5.x) * xlv_TEXCOORD5.y) + 0.5);
            highp float tmpvar_20;
            tmpvar_20 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD5.y);
            highp float tmpvar_21;
            tmpvar_21 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD5.y);
            faceColor_16 = _FaceColor;
            outlineColor_15 = _OutlineColor;
            faceColor_16 = (faceColor_16 * xlv_COLOR0);
            outlineColor_15.w = (outlineColor_15.w * xlv_COLOR0.w);
            highp vec2 tmpvar_22;
            tmpvar_22.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_22.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_23;
            tmpvar_23 = texture2D (_FaceTex, tmpvar_22);
            faceColor_16 = (faceColor_16 * tmpvar_23);
            highp vec2 tmpvar_24;
            tmpvar_24.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_24.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_25;
            tmpvar_25 = texture2D (_OutlineTex, tmpvar_24);
            outlineColor_15 = (outlineColor_15 * tmpvar_25);
            mediump float d_26;
            d_26 = tmpvar_19;
            lowp vec4 faceColor_27;
            faceColor_27 = faceColor_16;
            lowp vec4 outlineColor_28;
            outlineColor_28 = outlineColor_15;
            mediump float outline_29;
            outline_29 = tmpvar_20;
            mediump float softness_30;
            softness_30 = tmpvar_21;
            mediump float tmpvar_31;
            tmpvar_31 = (1.0 - clamp ((
              ((d_26 - (outline_29 * 0.5)) + (softness_30 * 0.5))
             / 
              (1.0 + softness_30)
            ), 0.0, 1.0));
            faceColor_27.xyz = (faceColor_27.xyz * faceColor_27.w);
            outlineColor_28.xyz = (outlineColor_28.xyz * outlineColor_28.w);
            mediump vec4 tmpvar_32;
            tmpvar_32 = mix (faceColor_27, outlineColor_28, vec4((clamp (
              (d_26 + (outline_29 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_29)
            ))));
            faceColor_27 = tmpvar_32;
            faceColor_27 = (faceColor_27 * tmpvar_31);
            faceColor_16 = faceColor_27;
            faceColor_16.xyz = (faceColor_16.xyz / max (faceColor_16.w, 0.0001));
            tmpvar_13 = faceColor_16.xyz;
            tmpvar_14 = faceColor_16.w;
            lowp float tmpvar_33;
            tmpvar_33 = _unity_tbn_0_8.z;
            worldN_3.x = tmpvar_33;
            lowp float tmpvar_34;
            tmpvar_34 = _unity_tbn_1_7.z;
            worldN_3.y = tmpvar_34;
            lowp float tmpvar_35;
            tmpvar_35 = _unity_tbn_2_6.z;
            worldN_3.z = tmpvar_35;
            highp vec3 tmpvar_36;
            tmpvar_36 = normalize(worldN_3);
            worldN_3 = tmpvar_36;
            tmpvar_4 = tmpvar_36;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_5;
            mediump vec3 normalWorld_37;
            normalWorld_37 = tmpvar_4;
            mediump vec4 tmpvar_38;
            tmpvar_38.w = 1.0;
            tmpvar_38.xyz = normalWorld_37;
            mediump vec3 x_39;
            x_39.x = dot (unity_SHAr, tmpvar_38);
            x_39.y = dot (unity_SHAg, tmpvar_38);
            x_39.z = dot (unity_SHAb, tmpvar_38);
            mediump vec3 tmpvar_40;
            tmpvar_40 = max (((1.055 * 
              pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD7 + x_39)), vec3(0.4166667, 0.4166667, 0.4166667))
            ) - 0.055), vec3(0.0, 0.0, 0.0));
            lowp vec4 c_41;
            lowp vec4 c_42;
            lowp float diff_43;
            mediump float tmpvar_44;
            tmpvar_44 = max (0.0, dot (tmpvar_4, tmpvar_2));
            diff_43 = tmpvar_44;
            c_42.xyz = ((tmpvar_13 * tmpvar_1) * diff_43);
            c_42.w = tmpvar_14;
            c_41.w = c_42.w;
            c_41.xyz = (c_42.xyz + (tmpvar_13 * tmpvar_40));
            gl_FragData[0] = c_41;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier02"
        {
           Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "VERTEXLIGHT_ON" }
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesTANGENT;
          attribute vec4 _glesVertex;
          attribute vec4 _glesColor;
          attribute vec3 _glesNormal;
          attribute vec4 _glesMultiTexCoord0;
          attribute vec4 _glesMultiTexCoord1;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _ScreenParams;
          uniform highp vec4 unity_4LightPosX0;
          uniform highp vec4 unity_4LightPosY0;
          uniform highp vec4 unity_4LightPosZ0;
          uniform mediump vec4 unity_4LightAtten0;
          uniform mediump vec4 unity_LightColor[8];
          uniform mediump vec4 unity_SHBr;
          uniform mediump vec4 unity_SHBg;
          uniform mediump vec4 unity_SHBb;
          uniform mediump vec4 unity_SHC;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp vec4 unity_WorldTransformParams;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp float _FaceDilate;
          uniform highp mat4 _EnvMatrix;
          uniform highp float _WeightNormal;
          uniform highp float _WeightBold;
          uniform highp float _ScaleRatioA;
          uniform highp float _VertexOffsetX;
          uniform highp float _VertexOffsetY;
          uniform highp float _GradientScale;
          uniform highp float _ScaleX;
          uniform highp float _ScaleY;
          uniform highp float _PerspectiveFilter;
          uniform highp vec4 _MainTex_ST;
          uniform highp vec4 _FaceTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec4 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD4;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD5;
          varying highp vec3 xlv_TEXCOORD6;
          varying mediump vec3 xlv_TEXCOORD7;
          void main ()
          {
            lowp vec3 worldBinormal_1;
            lowp float tangentSign_2;
            lowp vec3 worldTangent_3;
            highp vec4 tmpvar_4;
            mediump vec3 tmpvar_5;
            highp vec4 tmpvar_6;
            highp vec3 tmpvar_7;
            highp vec4 tmpvar_8;
            tmpvar_6.zw = _glesVertex.zw;
            tmpvar_8.zw = _glesMultiTexCoord1.zw;
            highp vec2 tmpvar_9;
            highp float scale_10;
            highp vec2 pixelSize_11;
            tmpvar_6.x = (_glesVertex.x + _VertexOffsetX);
            tmpvar_6.y = (_glesVertex.y + _VertexOffsetY);
            highp vec4 tmpvar_12;
            tmpvar_12.w = 1.0;
            tmpvar_12.xyz = _WorldSpaceCameraPos;
            tmpvar_7 = (_glesNormal * sign(dot (_glesNormal, 
              ((unity_WorldToObject * tmpvar_12).xyz - tmpvar_6.xyz)
            )));
            highp vec4 tmpvar_13;
            tmpvar_13.w = 1.0;
            tmpvar_13.xyz = tmpvar_6.xyz;
            highp vec2 tmpvar_14;
            tmpvar_14.x = _ScaleX;
            tmpvar_14.y = _ScaleY;
            highp mat2 tmpvar_15;
            tmpvar_15[0] = glstate_matrix_projection[0].xy;
            tmpvar_15[1] = glstate_matrix_projection[1].xy;
            pixelSize_11 = ((unity_MatrixVP * (unity_ObjectToWorld * tmpvar_13)).ww / (tmpvar_14 * (tmpvar_15 * _ScreenParams.xy)));
            scale_10 = (inversesqrt(dot (pixelSize_11, pixelSize_11)) * ((
              abs(_glesMultiTexCoord1.y)
             * _GradientScale) * 1.5));
            highp mat3 tmpvar_16;
            tmpvar_16[0] = unity_WorldToObject[0].xyz;
            tmpvar_16[1] = unity_WorldToObject[1].xyz;
            tmpvar_16[2] = unity_WorldToObject[2].xyz;
            highp float tmpvar_17;
            tmpvar_17 = mix ((scale_10 * (1.0 - _PerspectiveFilter)), scale_10, abs(dot (
              normalize((tmpvar_7 * tmpvar_16))
            , 
              normalize((_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_6).xyz))
            )));
            scale_10 = tmpvar_17;
            tmpvar_9.y = tmpvar_17;
            tmpvar_9.x = (((
              (mix (_WeightNormal, _WeightBold, float((0.0 >= _glesMultiTexCoord1.y))) / 4.0)
             + _FaceDilate) * _ScaleRatioA) * 0.5);
            highp vec2 xlat_varoutput_18;
            xlat_varoutput_18.x = floor((_glesMultiTexCoord1.x / 4096.0));
            xlat_varoutput_18.y = (_glesMultiTexCoord1.x - (4096.0 * xlat_varoutput_18.x));
            tmpvar_8.xy = (xlat_varoutput_18 * 0.001953125);
            highp mat3 tmpvar_19;
            tmpvar_19[0] = _EnvMatrix[0].xyz;
            tmpvar_19[1] = _EnvMatrix[1].xyz;
            tmpvar_19[2] = _EnvMatrix[2].xyz;
            highp vec4 tmpvar_20;
            tmpvar_20.w = 1.0;
            tmpvar_20.xyz = tmpvar_6.xyz;
            tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
            tmpvar_4.zw = ((tmpvar_8.xy * _FaceTex_ST.xy) + _FaceTex_ST.zw);
            highp vec3 tmpvar_21;
            tmpvar_21 = (unity_ObjectToWorld * tmpvar_6).xyz;
            highp mat3 tmpvar_22;
            tmpvar_22[0] = unity_WorldToObject[0].xyz;
            tmpvar_22[1] = unity_WorldToObject[1].xyz;
            tmpvar_22[2] = unity_WorldToObject[2].xyz;
            highp vec3 tmpvar_23;
            tmpvar_23 = normalize((tmpvar_7 * tmpvar_22));
            highp mat3 tmpvar_24;
            tmpvar_24[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_24[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_24[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_25;
            tmpvar_25 = normalize((tmpvar_24 * _glesTANGENT.xyz));
            worldTangent_3 = tmpvar_25;
            highp float tmpvar_26;
            tmpvar_26 = (_glesTANGENT.w * unity_WorldTransformParams.w);
            tangentSign_2 = tmpvar_26;
            highp vec3 tmpvar_27;
            tmpvar_27 = (((tmpvar_23.yzx * worldTangent_3.zxy) - (tmpvar_23.zxy * worldTangent_3.yzx)) * tangentSign_2);
            worldBinormal_1 = tmpvar_27;
            highp vec4 tmpvar_28;
            tmpvar_28.x = worldTangent_3.x;
            tmpvar_28.y = worldBinormal_1.x;
            tmpvar_28.z = tmpvar_23.x;
            tmpvar_28.w = tmpvar_21.x;
            highp vec4 tmpvar_29;
            tmpvar_29.x = worldTangent_3.y;
            tmpvar_29.y = worldBinormal_1.y;
            tmpvar_29.z = tmpvar_23.y;
            tmpvar_29.w = tmpvar_21.y;
            highp vec4 tmpvar_30;
            tmpvar_30.x = worldTangent_3.z;
            tmpvar_30.y = worldBinormal_1.z;
            tmpvar_30.z = tmpvar_23.z;
            tmpvar_30.w = tmpvar_21.z;
            highp vec3 lightColor0_31;
            lightColor0_31 = unity_LightColor[0].xyz;
            highp vec3 lightColor1_32;
            lightColor1_32 = unity_LightColor[1].xyz;
            highp vec3 lightColor2_33;
            lightColor2_33 = unity_LightColor[2].xyz;
            highp vec3 lightColor3_34;
            lightColor3_34 = unity_LightColor[3].xyz;
            highp vec4 lightAttenSq_35;
            lightAttenSq_35 = unity_4LightAtten0;
            highp vec3 col_36;
            highp vec4 ndotl_37;
            highp vec4 lengthSq_38;
            highp vec4 tmpvar_39;
            tmpvar_39 = (unity_4LightPosX0 - tmpvar_21.x);
            highp vec4 tmpvar_40;
            tmpvar_40 = (unity_4LightPosY0 - tmpvar_21.y);
            highp vec4 tmpvar_41;
            tmpvar_41 = (unity_4LightPosZ0 - tmpvar_21.z);
            lengthSq_38 = (tmpvar_39 * tmpvar_39);
            lengthSq_38 = (lengthSq_38 + (tmpvar_40 * tmpvar_40));
            lengthSq_38 = (lengthSq_38 + (tmpvar_41 * tmpvar_41));
            highp vec4 tmpvar_42;
            tmpvar_42 = max (lengthSq_38, vec4(1e-6, 1e-6, 1e-6, 1e-6));
            lengthSq_38 = tmpvar_42;
            ndotl_37 = (tmpvar_39 * tmpvar_23.x);
            ndotl_37 = (ndotl_37 + (tmpvar_40 * tmpvar_23.y));
            ndotl_37 = (ndotl_37 + (tmpvar_41 * tmpvar_23.z));
            highp vec4 tmpvar_43;
            tmpvar_43 = max (vec4(0.0, 0.0, 0.0, 0.0), (ndotl_37 * inversesqrt(tmpvar_42)));
            ndotl_37 = tmpvar_43;
            highp vec4 tmpvar_44;
            tmpvar_44 = (tmpvar_43 * (1.0/((1.0 + 
              (tmpvar_42 * lightAttenSq_35)
            ))));
            col_36 = (lightColor0_31 * tmpvar_44.x);
            col_36 = (col_36 + (lightColor1_32 * tmpvar_44.y));
            col_36 = (col_36 + (lightColor2_33 * tmpvar_44.z));
            col_36 = (col_36 + (lightColor3_34 * tmpvar_44.w));
            tmpvar_5 = col_36;
            mediump vec3 normal_45;
            normal_45 = tmpvar_23;
            mediump vec3 ambient_46;
            mediump vec3 x1_47;
            mediump vec4 tmpvar_48;
            tmpvar_48 = (normal_45.xyzz * normal_45.yzzx);
            x1_47.x = dot (unity_SHBr, tmpvar_48);
            x1_47.y = dot (unity_SHBg, tmpvar_48);
            x1_47.z = dot (unity_SHBb, tmpvar_48);
            ambient_46 = ((tmpvar_5 * (
              (tmpvar_5 * ((tmpvar_5 * 0.305306) + 0.6821711))
             + 0.01252288)) + (x1_47 + (unity_SHC.xyz * 
              ((normal_45.x * normal_45.x) - (normal_45.y * normal_45.y))
            )));
            tmpvar_5 = ambient_46;
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_20));
            xlv_TEXCOORD0 = tmpvar_4;
            xlv_TEXCOORD1 = ((tmpvar_8.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_28;
            xlv_TEXCOORD3 = tmpvar_29;
            xlv_TEXCOORD4 = tmpvar_30;
            xlv_COLOR0 = _glesColor;
            xlv_TEXCOORD5 = tmpvar_9;
            xlv_TEXCOORD6 = (tmpvar_19 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_6).xyz));
            xlv_TEXCOORD7 = ambient_46;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform mediump vec4 _WorldSpaceLightPos0;
          uniform mediump vec4 unity_SHAr;
          uniform mediump vec4 unity_SHAg;
          uniform mediump vec4 unity_SHAb;
          uniform lowp vec4 _LightColor0;
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
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec4 xlv_TEXCOORD2;
          varying highp vec4 xlv_TEXCOORD3;
          varying highp vec4 xlv_TEXCOORD4;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD5;
          varying mediump vec3 xlv_TEXCOORD7;
          void main ()
          {
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp vec3 tmpvar_4;
            lowp vec3 lightDir_5;
            lowp vec3 _unity_tbn_2_6;
            lowp vec3 _unity_tbn_1_7;
            lowp vec3 _unity_tbn_0_8;
            highp vec3 tmpvar_9;
            tmpvar_9 = xlv_TEXCOORD2.xyz;
            _unity_tbn_0_8 = tmpvar_9;
            highp vec3 tmpvar_10;
            tmpvar_10 = xlv_TEXCOORD3.xyz;
            _unity_tbn_1_7 = tmpvar_10;
            highp vec3 tmpvar_11;
            tmpvar_11 = xlv_TEXCOORD4.xyz;
            _unity_tbn_2_6 = tmpvar_11;
            mediump vec3 tmpvar_12;
            tmpvar_12 = _WorldSpaceLightPos0.xyz;
            lightDir_5 = tmpvar_12;
            lowp vec3 tmpvar_13;
            lowp float tmpvar_14;
            highp vec4 outlineColor_15;
            highp vec4 faceColor_16;
            highp float c_17;
            lowp float tmpvar_18;
            tmpvar_18 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_17 = tmpvar_18;
            highp float tmpvar_19;
            tmpvar_19 = (((
              (0.5 - c_17)
             - xlv_TEXCOORD5.x) * xlv_TEXCOORD5.y) + 0.5);
            highp float tmpvar_20;
            tmpvar_20 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD5.y);
            highp float tmpvar_21;
            tmpvar_21 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD5.y);
            faceColor_16 = _FaceColor;
            outlineColor_15 = _OutlineColor;
            faceColor_16 = (faceColor_16 * xlv_COLOR0);
            outlineColor_15.w = (outlineColor_15.w * xlv_COLOR0.w);
            highp vec2 tmpvar_22;
            tmpvar_22.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_22.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_23;
            tmpvar_23 = texture2D (_FaceTex, tmpvar_22);
            faceColor_16 = (faceColor_16 * tmpvar_23);
            highp vec2 tmpvar_24;
            tmpvar_24.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_24.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_25;
            tmpvar_25 = texture2D (_OutlineTex, tmpvar_24);
            outlineColor_15 = (outlineColor_15 * tmpvar_25);
            mediump float d_26;
            d_26 = tmpvar_19;
            lowp vec4 faceColor_27;
            faceColor_27 = faceColor_16;
            lowp vec4 outlineColor_28;
            outlineColor_28 = outlineColor_15;
            mediump float outline_29;
            outline_29 = tmpvar_20;
            mediump float softness_30;
            softness_30 = tmpvar_21;
            mediump float tmpvar_31;
            tmpvar_31 = (1.0 - clamp ((
              ((d_26 - (outline_29 * 0.5)) + (softness_30 * 0.5))
             / 
              (1.0 + softness_30)
            ), 0.0, 1.0));
            faceColor_27.xyz = (faceColor_27.xyz * faceColor_27.w);
            outlineColor_28.xyz = (outlineColor_28.xyz * outlineColor_28.w);
            mediump vec4 tmpvar_32;
            tmpvar_32 = mix (faceColor_27, outlineColor_28, vec4((clamp (
              (d_26 + (outline_29 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_29)
            ))));
            faceColor_27 = tmpvar_32;
            faceColor_27 = (faceColor_27 * tmpvar_31);
            faceColor_16 = faceColor_27;
            faceColor_16.xyz = (faceColor_16.xyz / max (faceColor_16.w, 0.0001));
            tmpvar_13 = faceColor_16.xyz;
            tmpvar_14 = faceColor_16.w;
            lowp float tmpvar_33;
            tmpvar_33 = _unity_tbn_0_8.z;
            worldN_3.x = tmpvar_33;
            lowp float tmpvar_34;
            tmpvar_34 = _unity_tbn_1_7.z;
            worldN_3.y = tmpvar_34;
            lowp float tmpvar_35;
            tmpvar_35 = _unity_tbn_2_6.z;
            worldN_3.z = tmpvar_35;
            highp vec3 tmpvar_36;
            tmpvar_36 = normalize(worldN_3);
            worldN_3 = tmpvar_36;
            tmpvar_4 = tmpvar_36;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_5;
            mediump vec3 normalWorld_37;
            normalWorld_37 = tmpvar_4;
            mediump vec4 tmpvar_38;
            tmpvar_38.w = 1.0;
            tmpvar_38.xyz = normalWorld_37;
            mediump vec3 x_39;
            x_39.x = dot (unity_SHAr, tmpvar_38);
            x_39.y = dot (unity_SHAg, tmpvar_38);
            x_39.z = dot (unity_SHAb, tmpvar_38);
            mediump vec3 tmpvar_40;
            tmpvar_40 = max (((1.055 * 
              pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD7 + x_39)), vec3(0.4166667, 0.4166667, 0.4166667))
            ) - 0.055), vec3(0.0, 0.0, 0.0));
            lowp vec4 c_41;
            lowp vec4 c_42;
            lowp float diff_43;
            mediump float tmpvar_44;
            tmpvar_44 = max (0.0, dot (tmpvar_4, tmpvar_2));
            diff_43 = tmpvar_44;
            c_42.xyz = ((tmpvar_13 * tmpvar_1) * diff_43);
            c_42.w = tmpvar_14;
            c_41.w = c_42.w;
            c_41.xyz = (c_42.xyz + (tmpvar_13 * tmpvar_40));
            gl_FragData[0] = c_41;
          }
          
          
          #endif
          
          "
        }
      }
      Program "fp"
      {
        SubProgram "gles hw_tier00"
        {
           Keywords { "DIRECTIONAL" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier01"
        {
           Keywords { "DIRECTIONAL" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier02"
        {
           Keywords { "DIRECTIONAL" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier00"
        {
           Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier01"
        {
           Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier02"
        {
           Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" }
          
          "!!!!GLES
          
          
          "
        }
      }
      
    } // end phase
    Pass // ind: 2, name: Caster
    {
      Name "Caster"
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "SHADOWCASTER"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
        "SHADOWSUPPORT" = "true"
      }
      LOD 300
      Cull Off
      Offset 1, 1
      Fog
      { 
        Mode  Off
      } 
      ColorMask RGB
      GpuProgramID 74246
      // m_ProgramMask = 6
      !!! *******************************************************************************************
      !!! Allow restore shader as UnityLab format - only available for DevX GameRecovery license type
      !!! *******************************************************************************************
      Program "vp"
      {
        SubProgram "gles hw_tier00"
        {
           Keywords { "SHADOWS_DEPTH" }
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesVertex;
          attribute vec4 _glesMultiTexCoord0;
          uniform highp vec4 unity_LightShadowBias;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_MatrixVP;
          uniform highp vec4 _MainTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          uniform highp float _OutlineWidth;
          uniform highp float _FaceDilate;
          uniform highp float _ScaleRatioA;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec2 xlv_TEXCOORD3;
          varying highp float xlv_TEXCOORD2;
          void main ()
          {
            highp vec4 tmpvar_1;
            highp vec4 tmpvar_2;
            tmpvar_2.w = 1.0;
            tmpvar_2.xyz = _glesVertex.xyz;
            tmpvar_1 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_2));
            highp vec4 clipPos_3;
            clipPos_3.xyw = tmpvar_1.xyw;
            clipPos_3.z = (tmpvar_1.z + clamp ((unity_LightShadowBias.x / tmpvar_1.w), 0.0, 1.0));
            clipPos_3.z = mix (clipPos_3.z, max (clipPos_3.z, -(tmpvar_1.w)), unity_LightShadowBias.y);
            gl_Position = clipPos_3;
            xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
            xlv_TEXCOORD3 = ((_glesMultiTexCoord0.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = (((1.0 - 
              (_OutlineWidth * _ScaleRatioA)
            ) - (_FaceDilate * _ScaleRatioA)) / 2.0);
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform sampler2D _MainTex;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp float xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = texture2D (_MainTex, xlv_TEXCOORD1).wwww;
            highp float x_2;
            x_2 = (tmpvar_1.w - xlv_TEXCOORD2);
            if ((x_2 < 0.0)) {
              discard;
            };
            gl_FragData[0] = vec4(0.0, 0.0, 0.0, 0.0);
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier01"
        {
           Keywords { "SHADOWS_DEPTH" }
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesVertex;
          attribute vec4 _glesMultiTexCoord0;
          uniform highp vec4 unity_LightShadowBias;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_MatrixVP;
          uniform highp vec4 _MainTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          uniform highp float _OutlineWidth;
          uniform highp float _FaceDilate;
          uniform highp float _ScaleRatioA;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec2 xlv_TEXCOORD3;
          varying highp float xlv_TEXCOORD2;
          void main ()
          {
            highp vec4 tmpvar_1;
            highp vec4 tmpvar_2;
            tmpvar_2.w = 1.0;
            tmpvar_2.xyz = _glesVertex.xyz;
            tmpvar_1 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_2));
            highp vec4 clipPos_3;
            clipPos_3.xyw = tmpvar_1.xyw;
            clipPos_3.z = (tmpvar_1.z + clamp ((unity_LightShadowBias.x / tmpvar_1.w), 0.0, 1.0));
            clipPos_3.z = mix (clipPos_3.z, max (clipPos_3.z, -(tmpvar_1.w)), unity_LightShadowBias.y);
            gl_Position = clipPos_3;
            xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
            xlv_TEXCOORD3 = ((_glesMultiTexCoord0.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = (((1.0 - 
              (_OutlineWidth * _ScaleRatioA)
            ) - (_FaceDilate * _ScaleRatioA)) / 2.0);
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform sampler2D _MainTex;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp float xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = texture2D (_MainTex, xlv_TEXCOORD1).wwww;
            highp float x_2;
            x_2 = (tmpvar_1.w - xlv_TEXCOORD2);
            if ((x_2 < 0.0)) {
              discard;
            };
            gl_FragData[0] = vec4(0.0, 0.0, 0.0, 0.0);
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier02"
        {
           Keywords { "SHADOWS_DEPTH" }
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesVertex;
          attribute vec4 _glesMultiTexCoord0;
          uniform highp vec4 unity_LightShadowBias;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_MatrixVP;
          uniform highp vec4 _MainTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          uniform highp float _OutlineWidth;
          uniform highp float _FaceDilate;
          uniform highp float _ScaleRatioA;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec2 xlv_TEXCOORD3;
          varying highp float xlv_TEXCOORD2;
          void main ()
          {
            highp vec4 tmpvar_1;
            highp vec4 tmpvar_2;
            tmpvar_2.w = 1.0;
            tmpvar_2.xyz = _glesVertex.xyz;
            tmpvar_1 = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_2));
            highp vec4 clipPos_3;
            clipPos_3.xyw = tmpvar_1.xyw;
            clipPos_3.z = (tmpvar_1.z + clamp ((unity_LightShadowBias.x / tmpvar_1.w), 0.0, 1.0));
            clipPos_3.z = mix (clipPos_3.z, max (clipPos_3.z, -(tmpvar_1.w)), unity_LightShadowBias.y);
            gl_Position = clipPos_3;
            xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
            xlv_TEXCOORD3 = ((_glesMultiTexCoord0.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = (((1.0 - 
              (_OutlineWidth * _ScaleRatioA)
            ) - (_FaceDilate * _ScaleRatioA)) / 2.0);
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform sampler2D _MainTex;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp float xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = texture2D (_MainTex, xlv_TEXCOORD1).wwww;
            highp float x_2;
            x_2 = (tmpvar_1.w - xlv_TEXCOORD2);
            if ((x_2 < 0.0)) {
              discard;
            };
            gl_FragData[0] = vec4(0.0, 0.0, 0.0, 0.0);
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier00"
        {
           Keywords { "SHADOWS_CUBE" }
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesVertex;
          attribute vec4 _glesMultiTexCoord0;
          uniform highp vec4 _LightPositionRange;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_MatrixVP;
          uniform highp vec4 _MainTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          uniform highp float _OutlineWidth;
          uniform highp float _FaceDilate;
          uniform highp float _ScaleRatioA;
          varying highp vec3 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec2 xlv_TEXCOORD3;
          varying highp float xlv_TEXCOORD2;
          void main ()
          {
            highp vec4 tmpvar_1;
            tmpvar_1.w = 1.0;
            tmpvar_1.xyz = _glesVertex.xyz;
            xlv_TEXCOORD0 = ((unity_ObjectToWorld * _glesVertex).xyz - _LightPositionRange.xyz);
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_1));
            xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
            xlv_TEXCOORD3 = ((_glesMultiTexCoord0.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = (((1.0 - 
              (_OutlineWidth * _ScaleRatioA)
            ) - (_FaceDilate * _ScaleRatioA)) / 2.0);
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _LightPositionRange;
          uniform highp vec4 unity_LightShadowBias;
          uniform sampler2D _MainTex;
          varying highp vec3 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp float xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = texture2D (_MainTex, xlv_TEXCOORD1).wwww;
            highp float x_2;
            x_2 = (tmpvar_1.w - xlv_TEXCOORD2);
            if ((x_2 < 0.0)) {
              discard;
            };
            highp vec4 tmpvar_3;
            tmpvar_3 = fract((vec4(1.0, 255.0, 65025.0, 1.658138e+7) * min (
              ((sqrt(dot (xlv_TEXCOORD0, xlv_TEXCOORD0)) + unity_LightShadowBias.x) * _LightPositionRange.w)
            , 0.999)));
            highp vec4 tmpvar_4;
            tmpvar_4 = (tmpvar_3 - (tmpvar_3.yzww * 0.003921569));
            gl_FragData[0] = tmpvar_4;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier01"
        {
           Keywords { "SHADOWS_CUBE" }
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesVertex;
          attribute vec4 _glesMultiTexCoord0;
          uniform highp vec4 _LightPositionRange;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_MatrixVP;
          uniform highp vec4 _MainTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          uniform highp float _OutlineWidth;
          uniform highp float _FaceDilate;
          uniform highp float _ScaleRatioA;
          varying highp vec3 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec2 xlv_TEXCOORD3;
          varying highp float xlv_TEXCOORD2;
          void main ()
          {
            highp vec4 tmpvar_1;
            tmpvar_1.w = 1.0;
            tmpvar_1.xyz = _glesVertex.xyz;
            xlv_TEXCOORD0 = ((unity_ObjectToWorld * _glesVertex).xyz - _LightPositionRange.xyz);
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_1));
            xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
            xlv_TEXCOORD3 = ((_glesMultiTexCoord0.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = (((1.0 - 
              (_OutlineWidth * _ScaleRatioA)
            ) - (_FaceDilate * _ScaleRatioA)) / 2.0);
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _LightPositionRange;
          uniform highp vec4 unity_LightShadowBias;
          uniform sampler2D _MainTex;
          varying highp vec3 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp float xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = texture2D (_MainTex, xlv_TEXCOORD1).wwww;
            highp float x_2;
            x_2 = (tmpvar_1.w - xlv_TEXCOORD2);
            if ((x_2 < 0.0)) {
              discard;
            };
            highp vec4 tmpvar_3;
            tmpvar_3 = fract((vec4(1.0, 255.0, 65025.0, 1.658138e+7) * min (
              ((sqrt(dot (xlv_TEXCOORD0, xlv_TEXCOORD0)) + unity_LightShadowBias.x) * _LightPositionRange.w)
            , 0.999)));
            highp vec4 tmpvar_4;
            tmpvar_4 = (tmpvar_3 - (tmpvar_3.yzww * 0.003921569));
            gl_FragData[0] = tmpvar_4;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier02"
        {
           Keywords { "SHADOWS_CUBE" }
          
          "!!!!GLES
          #version 100
          
          #ifdef VERTEX
          attribute vec4 _glesVertex;
          attribute vec4 _glesMultiTexCoord0;
          uniform highp vec4 _LightPositionRange;
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_MatrixVP;
          uniform highp vec4 _MainTex_ST;
          uniform highp vec4 _OutlineTex_ST;
          uniform highp float _OutlineWidth;
          uniform highp float _FaceDilate;
          uniform highp float _ScaleRatioA;
          varying highp vec3 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec2 xlv_TEXCOORD3;
          varying highp float xlv_TEXCOORD2;
          void main ()
          {
            highp vec4 tmpvar_1;
            tmpvar_1.w = 1.0;
            tmpvar_1.xyz = _glesVertex.xyz;
            xlv_TEXCOORD0 = ((unity_ObjectToWorld * _glesVertex).xyz - _LightPositionRange.xyz);
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_1));
            xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
            xlv_TEXCOORD3 = ((_glesMultiTexCoord0.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = (((1.0 - 
              (_OutlineWidth * _ScaleRatioA)
            ) - (_FaceDilate * _ScaleRatioA)) / 2.0);
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _LightPositionRange;
          uniform highp vec4 unity_LightShadowBias;
          uniform sampler2D _MainTex;
          varying highp vec3 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp float xlv_TEXCOORD2;
          void main ()
          {
            lowp vec4 tmpvar_1;
            tmpvar_1 = texture2D (_MainTex, xlv_TEXCOORD1).wwww;
            highp float x_2;
            x_2 = (tmpvar_1.w - xlv_TEXCOORD2);
            if ((x_2 < 0.0)) {
              discard;
            };
            highp vec4 tmpvar_3;
            tmpvar_3 = fract((vec4(1.0, 255.0, 65025.0, 1.658138e+7) * min (
              ((sqrt(dot (xlv_TEXCOORD0, xlv_TEXCOORD0)) + unity_LightShadowBias.x) * _LightPositionRange.w)
            , 0.999)));
            highp vec4 tmpvar_4;
            tmpvar_4 = (tmpvar_3 - (tmpvar_3.yzww * 0.003921569));
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
           Keywords { "SHADOWS_DEPTH" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier01"
        {
           Keywords { "SHADOWS_DEPTH" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier02"
        {
           Keywords { "SHADOWS_DEPTH" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier00"
        {
           Keywords { "SHADOWS_CUBE" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier01"
        {
           Keywords { "SHADOWS_CUBE" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier02"
        {
           Keywords { "SHADOWS_CUBE" }
          
          "!!!!GLES
          
          
          "
        }
      }
      
    } // end phase
  }
  FallBack Off
}
