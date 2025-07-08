Shader "TextMeshPro/Distance Field (Surface)"
{
  Properties
  {
    _FaceTex ("Fill Texture", 2D) = "white" {}
    _FaceUVSpeedX ("Face UV Speed X", Range(-5, 5)) = 0
    _FaceUVSpeedY ("Face UV Speed Y", Range(-5, 5)) = 0
    _FaceColor ("Fill Color", Color) = (1,1,1,1)
    _FaceDilate ("Face Dilate", Range(-1, 1)) = 0
    _OutlineColor ("Outline Color", Color) = (0,0,0,1)
    _OutlineTex ("Outline Texture", 2D) = "white" {}
    _OutlineUVSpeedX ("Outline UV Speed X", Range(-5, 5)) = 0
    _OutlineUVSpeedY ("Outline UV Speed Y", Range(-5, 5)) = 0
    _OutlineWidth ("Outline Thickness", Range(0, 1)) = 0
    _OutlineSoftness ("Outline Softness", Range(0, 1)) = 0
    _Bevel ("Bevel", Range(0, 1)) = 0.5
    _BevelOffset ("Bevel Offset", Range(-0.5, 0.5)) = 0
    _BevelWidth ("Bevel Width", Range(-0.5, 0.5)) = 0
    _BevelClamp ("Bevel Clamp", Range(0, 1)) = 0
    _BevelRoundness ("Bevel Roundness", Range(0, 1)) = 0
    _BumpMap ("Normalmap", 2D) = "bump" {}
    _BumpOutline ("Bump Outline", Range(0, 1)) = 0.5
    _BumpFace ("Bump Face", Range(0, 1)) = 0.5
    _ReflectFaceColor ("Face Color", Color) = (0,0,0,1)
    _ReflectOutlineColor ("Outline Color", Color) = (0,0,0,1)
    _Cube ("Reflection Cubemap", Cube) = "black" {}
    _EnvMatrixRotation ("Texture Rotation", Vector) = (0,0,0,0)
    _SpecColor ("Specular Color", Color) = (0,0,0,1)
    _FaceShininess ("Face Shininess", Range(0, 1)) = 0
    _OutlineShininess ("Outline Shininess", Range(0, 1)) = 0
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
      GpuProgramID 57641
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
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform mediump vec4 _WorldSpaceLightPos0;
          uniform mediump vec4 unity_SHAr;
          uniform mediump vec4 unity_SHAg;
          uniform mediump vec4 unity_SHAb;
          uniform highp mat4 unity_ObjectToWorld;
          uniform lowp vec4 _LightColor0;
          uniform lowp vec4 _SpecColor;
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
          uniform highp float _Bevel;
          uniform highp float _BevelOffset;
          uniform highp float _BevelWidth;
          uniform highp float _BevelClamp;
          uniform highp float _BevelRoundness;
          uniform sampler2D _BumpMap;
          uniform highp float _BumpOutline;
          uniform highp float _BumpFace;
          uniform lowp samplerCube _Cube;
          uniform lowp vec4 _ReflectFaceColor;
          uniform lowp vec4 _ReflectOutlineColor;
          uniform highp float _ShaderFlags;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          uniform highp float _TextureWidth;
          uniform highp float _TextureHeight;
          uniform highp float _GradientScale;
          uniform mediump float _FaceShininess;
          uniform mediump float _OutlineShininess;
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
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp vec4 c_4;
            lowp vec3 tmpvar_5;
            lowp vec3 tmpvar_6;
            lowp vec3 tmpvar_7;
            lowp float tmpvar_8;
            lowp float tmpvar_9;
            highp vec3 worldViewDir_10;
            lowp vec3 lightDir_11;
            lowp vec3 _unity_tbn_2_12;
            lowp vec3 _unity_tbn_1_13;
            lowp vec3 _unity_tbn_0_14;
            highp vec3 tmpvar_15;
            tmpvar_15 = xlv_TEXCOORD2.xyz;
            _unity_tbn_0_14 = tmpvar_15;
            highp vec3 tmpvar_16;
            tmpvar_16 = xlv_TEXCOORD3.xyz;
            _unity_tbn_1_13 = tmpvar_16;
            highp vec3 tmpvar_17;
            tmpvar_17 = xlv_TEXCOORD4.xyz;
            _unity_tbn_2_12 = tmpvar_17;
            highp vec3 tmpvar_18;
            tmpvar_18.x = xlv_TEXCOORD2.w;
            tmpvar_18.y = xlv_TEXCOORD3.w;
            tmpvar_18.z = xlv_TEXCOORD4.w;
            mediump vec3 tmpvar_19;
            tmpvar_19 = _WorldSpaceLightPos0.xyz;
            lightDir_11 = tmpvar_19;
            worldViewDir_10 = normalize((_WorldSpaceCameraPos - tmpvar_18));
            tmpvar_5 = vec3(0.0, 0.0, 0.0);
            tmpvar_7 = vec3(0.0, 0.0, 0.0);
            tmpvar_9 = 0.0;
            tmpvar_8 = 0.0;
            tmpvar_6 = vec3(0.0, 0.0, 1.0);
            lowp vec3 tmpvar_20;
            lowp vec3 tmpvar_21;
            lowp vec3 tmpvar_22;
            lowp float tmpvar_23;
            lowp float tmpvar_24;
            tmpvar_20 = tmpvar_5;
            tmpvar_21 = tmpvar_6;
            tmpvar_22 = tmpvar_7;
            tmpvar_23 = tmpvar_8;
            tmpvar_24 = tmpvar_9;
            highp vec3 bump_25;
            highp vec4 outlineColor_26;
            highp vec4 faceColor_27;
            highp float c_28;
            highp vec4 smp4x_29;
            highp vec3 tmpvar_30;
            tmpvar_30.z = 0.0;
            tmpvar_30.x = (1.0/(_TextureWidth));
            tmpvar_30.y = (1.0/(_TextureHeight));
            highp vec2 P_31;
            P_31 = (xlv_TEXCOORD0.xy - tmpvar_30.xz);
            highp vec2 P_32;
            P_32 = (xlv_TEXCOORD0.xy + tmpvar_30.xz);
            highp vec2 P_33;
            P_33 = (xlv_TEXCOORD0.xy - tmpvar_30.zy);
            highp vec2 P_34;
            P_34 = (xlv_TEXCOORD0.xy + tmpvar_30.zy);
            lowp vec4 tmpvar_35;
            tmpvar_35.x = texture2D (_MainTex, P_31).w;
            tmpvar_35.y = texture2D (_MainTex, P_32).w;
            tmpvar_35.z = texture2D (_MainTex, P_33).w;
            tmpvar_35.w = texture2D (_MainTex, P_34).w;
            smp4x_29 = tmpvar_35;
            lowp float tmpvar_36;
            tmpvar_36 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_28 = tmpvar_36;
            highp float tmpvar_37;
            tmpvar_37 = (((
              (0.5 - c_28)
             - xlv_TEXCOORD5.x) * xlv_TEXCOORD5.y) + 0.5);
            highp float tmpvar_38;
            tmpvar_38 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD5.y);
            highp float tmpvar_39;
            tmpvar_39 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD5.y);
            faceColor_27 = _FaceColor;
            outlineColor_26 = _OutlineColor;
            faceColor_27 = (faceColor_27 * xlv_COLOR0);
            outlineColor_26.w = (outlineColor_26.w * xlv_COLOR0.w);
            highp vec2 tmpvar_40;
            tmpvar_40.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_40.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_41;
            tmpvar_41 = texture2D (_FaceTex, tmpvar_40);
            faceColor_27 = (faceColor_27 * tmpvar_41);
            highp vec2 tmpvar_42;
            tmpvar_42.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_42.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_43;
            tmpvar_43 = texture2D (_OutlineTex, tmpvar_42);
            outlineColor_26 = (outlineColor_26 * tmpvar_43);
            mediump float d_44;
            d_44 = tmpvar_37;
            lowp vec4 faceColor_45;
            faceColor_45 = faceColor_27;
            lowp vec4 outlineColor_46;
            outlineColor_46 = outlineColor_26;
            mediump float outline_47;
            outline_47 = tmpvar_38;
            mediump float softness_48;
            softness_48 = tmpvar_39;
            mediump float tmpvar_49;
            tmpvar_49 = (1.0 - clamp ((
              ((d_44 - (outline_47 * 0.5)) + (softness_48 * 0.5))
             / 
              (1.0 + softness_48)
            ), 0.0, 1.0));
            faceColor_45.xyz = (faceColor_45.xyz * faceColor_45.w);
            outlineColor_46.xyz = (outlineColor_46.xyz * outlineColor_46.w);
            mediump vec4 tmpvar_50;
            tmpvar_50 = mix (faceColor_45, outlineColor_46, vec4((clamp (
              (d_44 + (outline_47 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_47)
            ))));
            faceColor_45 = tmpvar_50;
            faceColor_45 = (faceColor_45 * tmpvar_49);
            faceColor_27 = faceColor_45;
            faceColor_27.xyz = (faceColor_27.xyz / max (faceColor_27.w, 0.0001));
            highp vec4 h_51;
            h_51 = smp4x_29;
            highp float tmpvar_52;
            tmpvar_52 = (_ShaderFlags / 2.0);
            highp float tmpvar_53;
            tmpvar_53 = (fract(abs(tmpvar_52)) * 2.0);
            highp float tmpvar_54;
            if ((tmpvar_52 >= 0.0)) {
              tmpvar_54 = tmpvar_53;
            } else {
              tmpvar_54 = -(tmpvar_53);
            };
            h_51 = (smp4x_29 + (xlv_TEXCOORD5.x + _BevelOffset));
            highp float tmpvar_55;
            tmpvar_55 = max (0.01, (_OutlineWidth + _BevelWidth));
            h_51 = (h_51 - 0.5);
            h_51 = (h_51 / tmpvar_55);
            highp vec4 tmpvar_56;
            tmpvar_56 = clamp ((h_51 + 0.5), 0.0, 1.0);
            h_51 = tmpvar_56;
            if (bool(float((tmpvar_54 >= 1.0)))) {
              h_51 = (1.0 - abs((
                (tmpvar_56 * 2.0)
               - 1.0)));
            };
            h_51 = (min (mix (h_51, 
              sin(((h_51 * 3.141592) / 2.0))
            , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((_Bevel * tmpvar_55) * (_GradientScale * -2.0)));
            highp vec3 tmpvar_57;
            tmpvar_57.xy = vec2(1.0, 0.0);
            tmpvar_57.z = (h_51.y - h_51.x);
            highp vec3 tmpvar_58;
            tmpvar_58 = normalize(tmpvar_57);
            highp vec3 tmpvar_59;
            tmpvar_59.xy = vec2(0.0, -1.0);
            tmpvar_59.z = (h_51.w - h_51.z);
            highp vec3 tmpvar_60;
            tmpvar_60 = normalize(tmpvar_59);
            lowp vec3 tmpvar_61;
            tmpvar_61 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
            bump_25 = tmpvar_61;
            bump_25 = (bump_25 * mix (_BumpFace, _BumpOutline, clamp (
              (tmpvar_37 + (tmpvar_38 * 0.5))
            , 0.0, 1.0)));
            highp vec3 tmpvar_62;
            tmpvar_62 = mix (vec3(0.0, 0.0, 1.0), bump_25, faceColor_27.www);
            bump_25 = tmpvar_62;
            highp vec3 tmpvar_63;
            tmpvar_63 = normalize(((
              (tmpvar_58.yzx * tmpvar_60.zxy)
             - 
              (tmpvar_58.zxy * tmpvar_60.yzx)
            ) - tmpvar_62));
            highp mat3 tmpvar_64;
            tmpvar_64[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_64[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_64[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_65;
            highp vec3 N_66;
            N_66 = (tmpvar_64 * tmpvar_63);
            tmpvar_65 = (xlv_TEXCOORD6 - (2.0 * (
              dot (N_66, xlv_TEXCOORD6)
             * N_66)));
            lowp vec4 tmpvar_67;
            tmpvar_67 = textureCube (_Cube, tmpvar_65);
            highp float tmpvar_68;
            tmpvar_68 = clamp ((tmpvar_37 + (tmpvar_38 * 0.5)), 0.0, 1.0);
            lowp vec3 tmpvar_69;
            tmpvar_69 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_68));
            highp vec3 tmpvar_70;
            tmpvar_70 = ((tmpvar_67.xyz * tmpvar_69) * faceColor_27.w);
            tmpvar_20 = faceColor_27.xyz;
            tmpvar_21 = -(tmpvar_63);
            tmpvar_22 = tmpvar_70;
            highp float tmpvar_71;
            tmpvar_71 = clamp ((tmpvar_37 + (tmpvar_38 * 0.5)), 0.0, 1.0);
            tmpvar_23 = 1.0;
            tmpvar_24 = faceColor_27.w;
            tmpvar_5 = tmpvar_20;
            tmpvar_7 = tmpvar_22;
            tmpvar_8 = tmpvar_23;
            tmpvar_9 = tmpvar_24;
            lowp float tmpvar_72;
            tmpvar_72 = dot (_unity_tbn_0_14, tmpvar_21);
            worldN_3.x = tmpvar_72;
            lowp float tmpvar_73;
            tmpvar_73 = dot (_unity_tbn_1_13, tmpvar_21);
            worldN_3.y = tmpvar_73;
            lowp float tmpvar_74;
            tmpvar_74 = dot (_unity_tbn_2_12, tmpvar_21);
            worldN_3.z = tmpvar_74;
            highp vec3 tmpvar_75;
            tmpvar_75 = normalize(worldN_3);
            worldN_3 = tmpvar_75;
            tmpvar_6 = tmpvar_75;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_11;
            mediump vec3 normalWorld_76;
            normalWorld_76 = tmpvar_6;
            mediump vec4 tmpvar_77;
            tmpvar_77.w = 1.0;
            tmpvar_77.xyz = normalWorld_76;
            mediump vec3 x_78;
            x_78.x = dot (unity_SHAr, tmpvar_77);
            x_78.y = dot (unity_SHAg, tmpvar_77);
            x_78.z = dot (unity_SHAb, tmpvar_77);
            mediump vec3 tmpvar_79;
            tmpvar_79 = max (((1.055 * 
              pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD7 + x_78)), vec3(0.4166667, 0.4166667, 0.4166667))
            ) - 0.055), vec3(0.0, 0.0, 0.0));
            mediump vec3 viewDir_80;
            viewDir_80 = worldViewDir_10;
            lowp vec4 c_81;
            lowp vec4 c_82;
            highp float nh_83;
            lowp float diff_84;
            mediump float tmpvar_85;
            tmpvar_85 = max (0.0, dot (tmpvar_6, tmpvar_2));
            diff_84 = tmpvar_85;
            mediump float tmpvar_86;
            tmpvar_86 = max (0.0, dot (tmpvar_6, normalize(
              (tmpvar_2 + viewDir_80)
            )));
            nh_83 = tmpvar_86;
            mediump float y_87;
            y_87 = (mix (_FaceShininess, _OutlineShininess, tmpvar_71) * 128.0);
            highp float tmpvar_88;
            tmpvar_88 = pow (nh_83, y_87);
            c_82.xyz = (((tmpvar_20 * tmpvar_1) * diff_84) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_88));
            c_82.w = tmpvar_24;
            c_81.w = c_82.w;
            c_81.xyz = (c_82.xyz + (tmpvar_20 * tmpvar_79));
            c_4.w = c_81.w;
            c_4.xyz = (c_81.xyz + tmpvar_22);
            gl_FragData[0] = c_4;
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
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform mediump vec4 _WorldSpaceLightPos0;
          uniform mediump vec4 unity_SHAr;
          uniform mediump vec4 unity_SHAg;
          uniform mediump vec4 unity_SHAb;
          uniform highp mat4 unity_ObjectToWorld;
          uniform lowp vec4 _LightColor0;
          uniform lowp vec4 _SpecColor;
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
          uniform highp float _Bevel;
          uniform highp float _BevelOffset;
          uniform highp float _BevelWidth;
          uniform highp float _BevelClamp;
          uniform highp float _BevelRoundness;
          uniform sampler2D _BumpMap;
          uniform highp float _BumpOutline;
          uniform highp float _BumpFace;
          uniform lowp samplerCube _Cube;
          uniform lowp vec4 _ReflectFaceColor;
          uniform lowp vec4 _ReflectOutlineColor;
          uniform highp float _ShaderFlags;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          uniform highp float _TextureWidth;
          uniform highp float _TextureHeight;
          uniform highp float _GradientScale;
          uniform mediump float _FaceShininess;
          uniform mediump float _OutlineShininess;
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
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp vec4 c_4;
            lowp vec3 tmpvar_5;
            lowp vec3 tmpvar_6;
            lowp vec3 tmpvar_7;
            lowp float tmpvar_8;
            lowp float tmpvar_9;
            highp vec3 worldViewDir_10;
            lowp vec3 lightDir_11;
            lowp vec3 _unity_tbn_2_12;
            lowp vec3 _unity_tbn_1_13;
            lowp vec3 _unity_tbn_0_14;
            highp vec3 tmpvar_15;
            tmpvar_15 = xlv_TEXCOORD2.xyz;
            _unity_tbn_0_14 = tmpvar_15;
            highp vec3 tmpvar_16;
            tmpvar_16 = xlv_TEXCOORD3.xyz;
            _unity_tbn_1_13 = tmpvar_16;
            highp vec3 tmpvar_17;
            tmpvar_17 = xlv_TEXCOORD4.xyz;
            _unity_tbn_2_12 = tmpvar_17;
            highp vec3 tmpvar_18;
            tmpvar_18.x = xlv_TEXCOORD2.w;
            tmpvar_18.y = xlv_TEXCOORD3.w;
            tmpvar_18.z = xlv_TEXCOORD4.w;
            mediump vec3 tmpvar_19;
            tmpvar_19 = _WorldSpaceLightPos0.xyz;
            lightDir_11 = tmpvar_19;
            worldViewDir_10 = normalize((_WorldSpaceCameraPos - tmpvar_18));
            tmpvar_5 = vec3(0.0, 0.0, 0.0);
            tmpvar_7 = vec3(0.0, 0.0, 0.0);
            tmpvar_9 = 0.0;
            tmpvar_8 = 0.0;
            tmpvar_6 = vec3(0.0, 0.0, 1.0);
            lowp vec3 tmpvar_20;
            lowp vec3 tmpvar_21;
            lowp vec3 tmpvar_22;
            lowp float tmpvar_23;
            lowp float tmpvar_24;
            tmpvar_20 = tmpvar_5;
            tmpvar_21 = tmpvar_6;
            tmpvar_22 = tmpvar_7;
            tmpvar_23 = tmpvar_8;
            tmpvar_24 = tmpvar_9;
            highp vec3 bump_25;
            highp vec4 outlineColor_26;
            highp vec4 faceColor_27;
            highp float c_28;
            highp vec4 smp4x_29;
            highp vec3 tmpvar_30;
            tmpvar_30.z = 0.0;
            tmpvar_30.x = (1.0/(_TextureWidth));
            tmpvar_30.y = (1.0/(_TextureHeight));
            highp vec2 P_31;
            P_31 = (xlv_TEXCOORD0.xy - tmpvar_30.xz);
            highp vec2 P_32;
            P_32 = (xlv_TEXCOORD0.xy + tmpvar_30.xz);
            highp vec2 P_33;
            P_33 = (xlv_TEXCOORD0.xy - tmpvar_30.zy);
            highp vec2 P_34;
            P_34 = (xlv_TEXCOORD0.xy + tmpvar_30.zy);
            lowp vec4 tmpvar_35;
            tmpvar_35.x = texture2D (_MainTex, P_31).w;
            tmpvar_35.y = texture2D (_MainTex, P_32).w;
            tmpvar_35.z = texture2D (_MainTex, P_33).w;
            tmpvar_35.w = texture2D (_MainTex, P_34).w;
            smp4x_29 = tmpvar_35;
            lowp float tmpvar_36;
            tmpvar_36 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_28 = tmpvar_36;
            highp float tmpvar_37;
            tmpvar_37 = (((
              (0.5 - c_28)
             - xlv_TEXCOORD5.x) * xlv_TEXCOORD5.y) + 0.5);
            highp float tmpvar_38;
            tmpvar_38 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD5.y);
            highp float tmpvar_39;
            tmpvar_39 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD5.y);
            faceColor_27 = _FaceColor;
            outlineColor_26 = _OutlineColor;
            faceColor_27 = (faceColor_27 * xlv_COLOR0);
            outlineColor_26.w = (outlineColor_26.w * xlv_COLOR0.w);
            highp vec2 tmpvar_40;
            tmpvar_40.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_40.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_41;
            tmpvar_41 = texture2D (_FaceTex, tmpvar_40);
            faceColor_27 = (faceColor_27 * tmpvar_41);
            highp vec2 tmpvar_42;
            tmpvar_42.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_42.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_43;
            tmpvar_43 = texture2D (_OutlineTex, tmpvar_42);
            outlineColor_26 = (outlineColor_26 * tmpvar_43);
            mediump float d_44;
            d_44 = tmpvar_37;
            lowp vec4 faceColor_45;
            faceColor_45 = faceColor_27;
            lowp vec4 outlineColor_46;
            outlineColor_46 = outlineColor_26;
            mediump float outline_47;
            outline_47 = tmpvar_38;
            mediump float softness_48;
            softness_48 = tmpvar_39;
            mediump float tmpvar_49;
            tmpvar_49 = (1.0 - clamp ((
              ((d_44 - (outline_47 * 0.5)) + (softness_48 * 0.5))
             / 
              (1.0 + softness_48)
            ), 0.0, 1.0));
            faceColor_45.xyz = (faceColor_45.xyz * faceColor_45.w);
            outlineColor_46.xyz = (outlineColor_46.xyz * outlineColor_46.w);
            mediump vec4 tmpvar_50;
            tmpvar_50 = mix (faceColor_45, outlineColor_46, vec4((clamp (
              (d_44 + (outline_47 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_47)
            ))));
            faceColor_45 = tmpvar_50;
            faceColor_45 = (faceColor_45 * tmpvar_49);
            faceColor_27 = faceColor_45;
            faceColor_27.xyz = (faceColor_27.xyz / max (faceColor_27.w, 0.0001));
            highp vec4 h_51;
            h_51 = smp4x_29;
            highp float tmpvar_52;
            tmpvar_52 = (_ShaderFlags / 2.0);
            highp float tmpvar_53;
            tmpvar_53 = (fract(abs(tmpvar_52)) * 2.0);
            highp float tmpvar_54;
            if ((tmpvar_52 >= 0.0)) {
              tmpvar_54 = tmpvar_53;
            } else {
              tmpvar_54 = -(tmpvar_53);
            };
            h_51 = (smp4x_29 + (xlv_TEXCOORD5.x + _BevelOffset));
            highp float tmpvar_55;
            tmpvar_55 = max (0.01, (_OutlineWidth + _BevelWidth));
            h_51 = (h_51 - 0.5);
            h_51 = (h_51 / tmpvar_55);
            highp vec4 tmpvar_56;
            tmpvar_56 = clamp ((h_51 + 0.5), 0.0, 1.0);
            h_51 = tmpvar_56;
            if (bool(float((tmpvar_54 >= 1.0)))) {
              h_51 = (1.0 - abs((
                (tmpvar_56 * 2.0)
               - 1.0)));
            };
            h_51 = (min (mix (h_51, 
              sin(((h_51 * 3.141592) / 2.0))
            , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((_Bevel * tmpvar_55) * (_GradientScale * -2.0)));
            highp vec3 tmpvar_57;
            tmpvar_57.xy = vec2(1.0, 0.0);
            tmpvar_57.z = (h_51.y - h_51.x);
            highp vec3 tmpvar_58;
            tmpvar_58 = normalize(tmpvar_57);
            highp vec3 tmpvar_59;
            tmpvar_59.xy = vec2(0.0, -1.0);
            tmpvar_59.z = (h_51.w - h_51.z);
            highp vec3 tmpvar_60;
            tmpvar_60 = normalize(tmpvar_59);
            lowp vec3 tmpvar_61;
            tmpvar_61 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
            bump_25 = tmpvar_61;
            bump_25 = (bump_25 * mix (_BumpFace, _BumpOutline, clamp (
              (tmpvar_37 + (tmpvar_38 * 0.5))
            , 0.0, 1.0)));
            highp vec3 tmpvar_62;
            tmpvar_62 = mix (vec3(0.0, 0.0, 1.0), bump_25, faceColor_27.www);
            bump_25 = tmpvar_62;
            highp vec3 tmpvar_63;
            tmpvar_63 = normalize(((
              (tmpvar_58.yzx * tmpvar_60.zxy)
             - 
              (tmpvar_58.zxy * tmpvar_60.yzx)
            ) - tmpvar_62));
            highp mat3 tmpvar_64;
            tmpvar_64[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_64[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_64[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_65;
            highp vec3 N_66;
            N_66 = (tmpvar_64 * tmpvar_63);
            tmpvar_65 = (xlv_TEXCOORD6 - (2.0 * (
              dot (N_66, xlv_TEXCOORD6)
             * N_66)));
            lowp vec4 tmpvar_67;
            tmpvar_67 = textureCube (_Cube, tmpvar_65);
            highp float tmpvar_68;
            tmpvar_68 = clamp ((tmpvar_37 + (tmpvar_38 * 0.5)), 0.0, 1.0);
            lowp vec3 tmpvar_69;
            tmpvar_69 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_68));
            highp vec3 tmpvar_70;
            tmpvar_70 = ((tmpvar_67.xyz * tmpvar_69) * faceColor_27.w);
            tmpvar_20 = faceColor_27.xyz;
            tmpvar_21 = -(tmpvar_63);
            tmpvar_22 = tmpvar_70;
            highp float tmpvar_71;
            tmpvar_71 = clamp ((tmpvar_37 + (tmpvar_38 * 0.5)), 0.0, 1.0);
            tmpvar_23 = 1.0;
            tmpvar_24 = faceColor_27.w;
            tmpvar_5 = tmpvar_20;
            tmpvar_7 = tmpvar_22;
            tmpvar_8 = tmpvar_23;
            tmpvar_9 = tmpvar_24;
            lowp float tmpvar_72;
            tmpvar_72 = dot (_unity_tbn_0_14, tmpvar_21);
            worldN_3.x = tmpvar_72;
            lowp float tmpvar_73;
            tmpvar_73 = dot (_unity_tbn_1_13, tmpvar_21);
            worldN_3.y = tmpvar_73;
            lowp float tmpvar_74;
            tmpvar_74 = dot (_unity_tbn_2_12, tmpvar_21);
            worldN_3.z = tmpvar_74;
            highp vec3 tmpvar_75;
            tmpvar_75 = normalize(worldN_3);
            worldN_3 = tmpvar_75;
            tmpvar_6 = tmpvar_75;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_11;
            mediump vec3 normalWorld_76;
            normalWorld_76 = tmpvar_6;
            mediump vec4 tmpvar_77;
            tmpvar_77.w = 1.0;
            tmpvar_77.xyz = normalWorld_76;
            mediump vec3 x_78;
            x_78.x = dot (unity_SHAr, tmpvar_77);
            x_78.y = dot (unity_SHAg, tmpvar_77);
            x_78.z = dot (unity_SHAb, tmpvar_77);
            mediump vec3 tmpvar_79;
            tmpvar_79 = max (((1.055 * 
              pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD7 + x_78)), vec3(0.4166667, 0.4166667, 0.4166667))
            ) - 0.055), vec3(0.0, 0.0, 0.0));
            mediump vec3 viewDir_80;
            viewDir_80 = worldViewDir_10;
            lowp vec4 c_81;
            lowp vec4 c_82;
            highp float nh_83;
            lowp float diff_84;
            mediump float tmpvar_85;
            tmpvar_85 = max (0.0, dot (tmpvar_6, tmpvar_2));
            diff_84 = tmpvar_85;
            mediump float tmpvar_86;
            tmpvar_86 = max (0.0, dot (tmpvar_6, normalize(
              (tmpvar_2 + viewDir_80)
            )));
            nh_83 = tmpvar_86;
            mediump float y_87;
            y_87 = (mix (_FaceShininess, _OutlineShininess, tmpvar_71) * 128.0);
            highp float tmpvar_88;
            tmpvar_88 = pow (nh_83, y_87);
            c_82.xyz = (((tmpvar_20 * tmpvar_1) * diff_84) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_88));
            c_82.w = tmpvar_24;
            c_81.w = c_82.w;
            c_81.xyz = (c_82.xyz + (tmpvar_20 * tmpvar_79));
            c_4.w = c_81.w;
            c_4.xyz = (c_81.xyz + tmpvar_22);
            gl_FragData[0] = c_4;
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
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform mediump vec4 _WorldSpaceLightPos0;
          uniform mediump vec4 unity_SHAr;
          uniform mediump vec4 unity_SHAg;
          uniform mediump vec4 unity_SHAb;
          uniform highp mat4 unity_ObjectToWorld;
          uniform lowp vec4 _LightColor0;
          uniform lowp vec4 _SpecColor;
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
          uniform highp float _Bevel;
          uniform highp float _BevelOffset;
          uniform highp float _BevelWidth;
          uniform highp float _BevelClamp;
          uniform highp float _BevelRoundness;
          uniform sampler2D _BumpMap;
          uniform highp float _BumpOutline;
          uniform highp float _BumpFace;
          uniform lowp samplerCube _Cube;
          uniform lowp vec4 _ReflectFaceColor;
          uniform lowp vec4 _ReflectOutlineColor;
          uniform highp float _ShaderFlags;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          uniform highp float _TextureWidth;
          uniform highp float _TextureHeight;
          uniform highp float _GradientScale;
          uniform mediump float _FaceShininess;
          uniform mediump float _OutlineShininess;
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
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp vec4 c_4;
            lowp vec3 tmpvar_5;
            lowp vec3 tmpvar_6;
            lowp vec3 tmpvar_7;
            lowp float tmpvar_8;
            lowp float tmpvar_9;
            highp vec3 worldViewDir_10;
            lowp vec3 lightDir_11;
            lowp vec3 _unity_tbn_2_12;
            lowp vec3 _unity_tbn_1_13;
            lowp vec3 _unity_tbn_0_14;
            highp vec3 tmpvar_15;
            tmpvar_15 = xlv_TEXCOORD2.xyz;
            _unity_tbn_0_14 = tmpvar_15;
            highp vec3 tmpvar_16;
            tmpvar_16 = xlv_TEXCOORD3.xyz;
            _unity_tbn_1_13 = tmpvar_16;
            highp vec3 tmpvar_17;
            tmpvar_17 = xlv_TEXCOORD4.xyz;
            _unity_tbn_2_12 = tmpvar_17;
            highp vec3 tmpvar_18;
            tmpvar_18.x = xlv_TEXCOORD2.w;
            tmpvar_18.y = xlv_TEXCOORD3.w;
            tmpvar_18.z = xlv_TEXCOORD4.w;
            mediump vec3 tmpvar_19;
            tmpvar_19 = _WorldSpaceLightPos0.xyz;
            lightDir_11 = tmpvar_19;
            worldViewDir_10 = normalize((_WorldSpaceCameraPos - tmpvar_18));
            tmpvar_5 = vec3(0.0, 0.0, 0.0);
            tmpvar_7 = vec3(0.0, 0.0, 0.0);
            tmpvar_9 = 0.0;
            tmpvar_8 = 0.0;
            tmpvar_6 = vec3(0.0, 0.0, 1.0);
            lowp vec3 tmpvar_20;
            lowp vec3 tmpvar_21;
            lowp vec3 tmpvar_22;
            lowp float tmpvar_23;
            lowp float tmpvar_24;
            tmpvar_20 = tmpvar_5;
            tmpvar_21 = tmpvar_6;
            tmpvar_22 = tmpvar_7;
            tmpvar_23 = tmpvar_8;
            tmpvar_24 = tmpvar_9;
            highp vec3 bump_25;
            highp vec4 outlineColor_26;
            highp vec4 faceColor_27;
            highp float c_28;
            highp vec4 smp4x_29;
            highp vec3 tmpvar_30;
            tmpvar_30.z = 0.0;
            tmpvar_30.x = (1.0/(_TextureWidth));
            tmpvar_30.y = (1.0/(_TextureHeight));
            highp vec2 P_31;
            P_31 = (xlv_TEXCOORD0.xy - tmpvar_30.xz);
            highp vec2 P_32;
            P_32 = (xlv_TEXCOORD0.xy + tmpvar_30.xz);
            highp vec2 P_33;
            P_33 = (xlv_TEXCOORD0.xy - tmpvar_30.zy);
            highp vec2 P_34;
            P_34 = (xlv_TEXCOORD0.xy + tmpvar_30.zy);
            lowp vec4 tmpvar_35;
            tmpvar_35.x = texture2D (_MainTex, P_31).w;
            tmpvar_35.y = texture2D (_MainTex, P_32).w;
            tmpvar_35.z = texture2D (_MainTex, P_33).w;
            tmpvar_35.w = texture2D (_MainTex, P_34).w;
            smp4x_29 = tmpvar_35;
            lowp float tmpvar_36;
            tmpvar_36 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_28 = tmpvar_36;
            highp float tmpvar_37;
            tmpvar_37 = (((
              (0.5 - c_28)
             - xlv_TEXCOORD5.x) * xlv_TEXCOORD5.y) + 0.5);
            highp float tmpvar_38;
            tmpvar_38 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD5.y);
            highp float tmpvar_39;
            tmpvar_39 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD5.y);
            faceColor_27 = _FaceColor;
            outlineColor_26 = _OutlineColor;
            faceColor_27 = (faceColor_27 * xlv_COLOR0);
            outlineColor_26.w = (outlineColor_26.w * xlv_COLOR0.w);
            highp vec2 tmpvar_40;
            tmpvar_40.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_40.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_41;
            tmpvar_41 = texture2D (_FaceTex, tmpvar_40);
            faceColor_27 = (faceColor_27 * tmpvar_41);
            highp vec2 tmpvar_42;
            tmpvar_42.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_42.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_43;
            tmpvar_43 = texture2D (_OutlineTex, tmpvar_42);
            outlineColor_26 = (outlineColor_26 * tmpvar_43);
            mediump float d_44;
            d_44 = tmpvar_37;
            lowp vec4 faceColor_45;
            faceColor_45 = faceColor_27;
            lowp vec4 outlineColor_46;
            outlineColor_46 = outlineColor_26;
            mediump float outline_47;
            outline_47 = tmpvar_38;
            mediump float softness_48;
            softness_48 = tmpvar_39;
            mediump float tmpvar_49;
            tmpvar_49 = (1.0 - clamp ((
              ((d_44 - (outline_47 * 0.5)) + (softness_48 * 0.5))
             / 
              (1.0 + softness_48)
            ), 0.0, 1.0));
            faceColor_45.xyz = (faceColor_45.xyz * faceColor_45.w);
            outlineColor_46.xyz = (outlineColor_46.xyz * outlineColor_46.w);
            mediump vec4 tmpvar_50;
            tmpvar_50 = mix (faceColor_45, outlineColor_46, vec4((clamp (
              (d_44 + (outline_47 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_47)
            ))));
            faceColor_45 = tmpvar_50;
            faceColor_45 = (faceColor_45 * tmpvar_49);
            faceColor_27 = faceColor_45;
            faceColor_27.xyz = (faceColor_27.xyz / max (faceColor_27.w, 0.0001));
            highp vec4 h_51;
            h_51 = smp4x_29;
            highp float tmpvar_52;
            tmpvar_52 = (_ShaderFlags / 2.0);
            highp float tmpvar_53;
            tmpvar_53 = (fract(abs(tmpvar_52)) * 2.0);
            highp float tmpvar_54;
            if ((tmpvar_52 >= 0.0)) {
              tmpvar_54 = tmpvar_53;
            } else {
              tmpvar_54 = -(tmpvar_53);
            };
            h_51 = (smp4x_29 + (xlv_TEXCOORD5.x + _BevelOffset));
            highp float tmpvar_55;
            tmpvar_55 = max (0.01, (_OutlineWidth + _BevelWidth));
            h_51 = (h_51 - 0.5);
            h_51 = (h_51 / tmpvar_55);
            highp vec4 tmpvar_56;
            tmpvar_56 = clamp ((h_51 + 0.5), 0.0, 1.0);
            h_51 = tmpvar_56;
            if (bool(float((tmpvar_54 >= 1.0)))) {
              h_51 = (1.0 - abs((
                (tmpvar_56 * 2.0)
               - 1.0)));
            };
            h_51 = (min (mix (h_51, 
              sin(((h_51 * 3.141592) / 2.0))
            , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((_Bevel * tmpvar_55) * (_GradientScale * -2.0)));
            highp vec3 tmpvar_57;
            tmpvar_57.xy = vec2(1.0, 0.0);
            tmpvar_57.z = (h_51.y - h_51.x);
            highp vec3 tmpvar_58;
            tmpvar_58 = normalize(tmpvar_57);
            highp vec3 tmpvar_59;
            tmpvar_59.xy = vec2(0.0, -1.0);
            tmpvar_59.z = (h_51.w - h_51.z);
            highp vec3 tmpvar_60;
            tmpvar_60 = normalize(tmpvar_59);
            lowp vec3 tmpvar_61;
            tmpvar_61 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
            bump_25 = tmpvar_61;
            bump_25 = (bump_25 * mix (_BumpFace, _BumpOutline, clamp (
              (tmpvar_37 + (tmpvar_38 * 0.5))
            , 0.0, 1.0)));
            highp vec3 tmpvar_62;
            tmpvar_62 = mix (vec3(0.0, 0.0, 1.0), bump_25, faceColor_27.www);
            bump_25 = tmpvar_62;
            highp vec3 tmpvar_63;
            tmpvar_63 = normalize(((
              (tmpvar_58.yzx * tmpvar_60.zxy)
             - 
              (tmpvar_58.zxy * tmpvar_60.yzx)
            ) - tmpvar_62));
            highp mat3 tmpvar_64;
            tmpvar_64[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_64[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_64[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_65;
            highp vec3 N_66;
            N_66 = (tmpvar_64 * tmpvar_63);
            tmpvar_65 = (xlv_TEXCOORD6 - (2.0 * (
              dot (N_66, xlv_TEXCOORD6)
             * N_66)));
            lowp vec4 tmpvar_67;
            tmpvar_67 = textureCube (_Cube, tmpvar_65);
            highp float tmpvar_68;
            tmpvar_68 = clamp ((tmpvar_37 + (tmpvar_38 * 0.5)), 0.0, 1.0);
            lowp vec3 tmpvar_69;
            tmpvar_69 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_68));
            highp vec3 tmpvar_70;
            tmpvar_70 = ((tmpvar_67.xyz * tmpvar_69) * faceColor_27.w);
            tmpvar_20 = faceColor_27.xyz;
            tmpvar_21 = -(tmpvar_63);
            tmpvar_22 = tmpvar_70;
            highp float tmpvar_71;
            tmpvar_71 = clamp ((tmpvar_37 + (tmpvar_38 * 0.5)), 0.0, 1.0);
            tmpvar_23 = 1.0;
            tmpvar_24 = faceColor_27.w;
            tmpvar_5 = tmpvar_20;
            tmpvar_7 = tmpvar_22;
            tmpvar_8 = tmpvar_23;
            tmpvar_9 = tmpvar_24;
            lowp float tmpvar_72;
            tmpvar_72 = dot (_unity_tbn_0_14, tmpvar_21);
            worldN_3.x = tmpvar_72;
            lowp float tmpvar_73;
            tmpvar_73 = dot (_unity_tbn_1_13, tmpvar_21);
            worldN_3.y = tmpvar_73;
            lowp float tmpvar_74;
            tmpvar_74 = dot (_unity_tbn_2_12, tmpvar_21);
            worldN_3.z = tmpvar_74;
            highp vec3 tmpvar_75;
            tmpvar_75 = normalize(worldN_3);
            worldN_3 = tmpvar_75;
            tmpvar_6 = tmpvar_75;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_11;
            mediump vec3 normalWorld_76;
            normalWorld_76 = tmpvar_6;
            mediump vec4 tmpvar_77;
            tmpvar_77.w = 1.0;
            tmpvar_77.xyz = normalWorld_76;
            mediump vec3 x_78;
            x_78.x = dot (unity_SHAr, tmpvar_77);
            x_78.y = dot (unity_SHAg, tmpvar_77);
            x_78.z = dot (unity_SHAb, tmpvar_77);
            mediump vec3 tmpvar_79;
            tmpvar_79 = max (((1.055 * 
              pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD7 + x_78)), vec3(0.4166667, 0.4166667, 0.4166667))
            ) - 0.055), vec3(0.0, 0.0, 0.0));
            mediump vec3 viewDir_80;
            viewDir_80 = worldViewDir_10;
            lowp vec4 c_81;
            lowp vec4 c_82;
            highp float nh_83;
            lowp float diff_84;
            mediump float tmpvar_85;
            tmpvar_85 = max (0.0, dot (tmpvar_6, tmpvar_2));
            diff_84 = tmpvar_85;
            mediump float tmpvar_86;
            tmpvar_86 = max (0.0, dot (tmpvar_6, normalize(
              (tmpvar_2 + viewDir_80)
            )));
            nh_83 = tmpvar_86;
            mediump float y_87;
            y_87 = (mix (_FaceShininess, _OutlineShininess, tmpvar_71) * 128.0);
            highp float tmpvar_88;
            tmpvar_88 = pow (nh_83, y_87);
            c_82.xyz = (((tmpvar_20 * tmpvar_1) * diff_84) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_88));
            c_82.w = tmpvar_24;
            c_81.w = c_82.w;
            c_81.xyz = (c_82.xyz + (tmpvar_20 * tmpvar_79));
            c_4.w = c_81.w;
            c_4.xyz = (c_81.xyz + tmpvar_22);
            gl_FragData[0] = c_4;
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
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform mediump vec4 _WorldSpaceLightPos0;
          uniform mediump vec4 unity_SHAr;
          uniform mediump vec4 unity_SHAg;
          uniform mediump vec4 unity_SHAb;
          uniform highp mat4 unity_ObjectToWorld;
          uniform lowp vec4 _LightColor0;
          uniform lowp vec4 _SpecColor;
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
          uniform highp float _Bevel;
          uniform highp float _BevelOffset;
          uniform highp float _BevelWidth;
          uniform highp float _BevelClamp;
          uniform highp float _BevelRoundness;
          uniform sampler2D _BumpMap;
          uniform highp float _BumpOutline;
          uniform highp float _BumpFace;
          uniform lowp samplerCube _Cube;
          uniform lowp vec4 _ReflectFaceColor;
          uniform lowp vec4 _ReflectOutlineColor;
          uniform highp float _ShaderFlags;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          uniform highp float _TextureWidth;
          uniform highp float _TextureHeight;
          uniform highp float _GradientScale;
          uniform mediump float _FaceShininess;
          uniform mediump float _OutlineShininess;
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
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp vec4 c_4;
            lowp vec3 tmpvar_5;
            lowp vec3 tmpvar_6;
            lowp vec3 tmpvar_7;
            lowp float tmpvar_8;
            lowp float tmpvar_9;
            highp vec3 worldViewDir_10;
            lowp vec3 lightDir_11;
            lowp vec3 _unity_tbn_2_12;
            lowp vec3 _unity_tbn_1_13;
            lowp vec3 _unity_tbn_0_14;
            highp vec3 tmpvar_15;
            tmpvar_15 = xlv_TEXCOORD2.xyz;
            _unity_tbn_0_14 = tmpvar_15;
            highp vec3 tmpvar_16;
            tmpvar_16 = xlv_TEXCOORD3.xyz;
            _unity_tbn_1_13 = tmpvar_16;
            highp vec3 tmpvar_17;
            tmpvar_17 = xlv_TEXCOORD4.xyz;
            _unity_tbn_2_12 = tmpvar_17;
            highp vec3 tmpvar_18;
            tmpvar_18.x = xlv_TEXCOORD2.w;
            tmpvar_18.y = xlv_TEXCOORD3.w;
            tmpvar_18.z = xlv_TEXCOORD4.w;
            mediump vec3 tmpvar_19;
            tmpvar_19 = _WorldSpaceLightPos0.xyz;
            lightDir_11 = tmpvar_19;
            worldViewDir_10 = normalize((_WorldSpaceCameraPos - tmpvar_18));
            tmpvar_5 = vec3(0.0, 0.0, 0.0);
            tmpvar_7 = vec3(0.0, 0.0, 0.0);
            tmpvar_9 = 0.0;
            tmpvar_8 = 0.0;
            tmpvar_6 = vec3(0.0, 0.0, 1.0);
            lowp vec3 tmpvar_20;
            lowp vec3 tmpvar_21;
            lowp vec3 tmpvar_22;
            lowp float tmpvar_23;
            lowp float tmpvar_24;
            tmpvar_20 = tmpvar_5;
            tmpvar_21 = tmpvar_6;
            tmpvar_22 = tmpvar_7;
            tmpvar_23 = tmpvar_8;
            tmpvar_24 = tmpvar_9;
            highp vec3 bump_25;
            highp vec4 outlineColor_26;
            highp vec4 faceColor_27;
            highp float c_28;
            highp vec4 smp4x_29;
            highp vec3 tmpvar_30;
            tmpvar_30.z = 0.0;
            tmpvar_30.x = (1.0/(_TextureWidth));
            tmpvar_30.y = (1.0/(_TextureHeight));
            highp vec2 P_31;
            P_31 = (xlv_TEXCOORD0.xy - tmpvar_30.xz);
            highp vec2 P_32;
            P_32 = (xlv_TEXCOORD0.xy + tmpvar_30.xz);
            highp vec2 P_33;
            P_33 = (xlv_TEXCOORD0.xy - tmpvar_30.zy);
            highp vec2 P_34;
            P_34 = (xlv_TEXCOORD0.xy + tmpvar_30.zy);
            lowp vec4 tmpvar_35;
            tmpvar_35.x = texture2D (_MainTex, P_31).w;
            tmpvar_35.y = texture2D (_MainTex, P_32).w;
            tmpvar_35.z = texture2D (_MainTex, P_33).w;
            tmpvar_35.w = texture2D (_MainTex, P_34).w;
            smp4x_29 = tmpvar_35;
            lowp float tmpvar_36;
            tmpvar_36 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_28 = tmpvar_36;
            highp float tmpvar_37;
            tmpvar_37 = (((
              (0.5 - c_28)
             - xlv_TEXCOORD5.x) * xlv_TEXCOORD5.y) + 0.5);
            highp float tmpvar_38;
            tmpvar_38 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD5.y);
            highp float tmpvar_39;
            tmpvar_39 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD5.y);
            faceColor_27 = _FaceColor;
            outlineColor_26 = _OutlineColor;
            faceColor_27 = (faceColor_27 * xlv_COLOR0);
            outlineColor_26.w = (outlineColor_26.w * xlv_COLOR0.w);
            highp vec2 tmpvar_40;
            tmpvar_40.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_40.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_41;
            tmpvar_41 = texture2D (_FaceTex, tmpvar_40);
            faceColor_27 = (faceColor_27 * tmpvar_41);
            highp vec2 tmpvar_42;
            tmpvar_42.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_42.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_43;
            tmpvar_43 = texture2D (_OutlineTex, tmpvar_42);
            outlineColor_26 = (outlineColor_26 * tmpvar_43);
            mediump float d_44;
            d_44 = tmpvar_37;
            lowp vec4 faceColor_45;
            faceColor_45 = faceColor_27;
            lowp vec4 outlineColor_46;
            outlineColor_46 = outlineColor_26;
            mediump float outline_47;
            outline_47 = tmpvar_38;
            mediump float softness_48;
            softness_48 = tmpvar_39;
            mediump float tmpvar_49;
            tmpvar_49 = (1.0 - clamp ((
              ((d_44 - (outline_47 * 0.5)) + (softness_48 * 0.5))
             / 
              (1.0 + softness_48)
            ), 0.0, 1.0));
            faceColor_45.xyz = (faceColor_45.xyz * faceColor_45.w);
            outlineColor_46.xyz = (outlineColor_46.xyz * outlineColor_46.w);
            mediump vec4 tmpvar_50;
            tmpvar_50 = mix (faceColor_45, outlineColor_46, vec4((clamp (
              (d_44 + (outline_47 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_47)
            ))));
            faceColor_45 = tmpvar_50;
            faceColor_45 = (faceColor_45 * tmpvar_49);
            faceColor_27 = faceColor_45;
            faceColor_27.xyz = (faceColor_27.xyz / max (faceColor_27.w, 0.0001));
            highp vec4 h_51;
            h_51 = smp4x_29;
            highp float tmpvar_52;
            tmpvar_52 = (_ShaderFlags / 2.0);
            highp float tmpvar_53;
            tmpvar_53 = (fract(abs(tmpvar_52)) * 2.0);
            highp float tmpvar_54;
            if ((tmpvar_52 >= 0.0)) {
              tmpvar_54 = tmpvar_53;
            } else {
              tmpvar_54 = -(tmpvar_53);
            };
            h_51 = (smp4x_29 + (xlv_TEXCOORD5.x + _BevelOffset));
            highp float tmpvar_55;
            tmpvar_55 = max (0.01, (_OutlineWidth + _BevelWidth));
            h_51 = (h_51 - 0.5);
            h_51 = (h_51 / tmpvar_55);
            highp vec4 tmpvar_56;
            tmpvar_56 = clamp ((h_51 + 0.5), 0.0, 1.0);
            h_51 = tmpvar_56;
            if (bool(float((tmpvar_54 >= 1.0)))) {
              h_51 = (1.0 - abs((
                (tmpvar_56 * 2.0)
               - 1.0)));
            };
            h_51 = (min (mix (h_51, 
              sin(((h_51 * 3.141592) / 2.0))
            , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((_Bevel * tmpvar_55) * (_GradientScale * -2.0)));
            highp vec3 tmpvar_57;
            tmpvar_57.xy = vec2(1.0, 0.0);
            tmpvar_57.z = (h_51.y - h_51.x);
            highp vec3 tmpvar_58;
            tmpvar_58 = normalize(tmpvar_57);
            highp vec3 tmpvar_59;
            tmpvar_59.xy = vec2(0.0, -1.0);
            tmpvar_59.z = (h_51.w - h_51.z);
            highp vec3 tmpvar_60;
            tmpvar_60 = normalize(tmpvar_59);
            lowp vec3 tmpvar_61;
            tmpvar_61 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
            bump_25 = tmpvar_61;
            bump_25 = (bump_25 * mix (_BumpFace, _BumpOutline, clamp (
              (tmpvar_37 + (tmpvar_38 * 0.5))
            , 0.0, 1.0)));
            highp vec3 tmpvar_62;
            tmpvar_62 = mix (vec3(0.0, 0.0, 1.0), bump_25, faceColor_27.www);
            bump_25 = tmpvar_62;
            highp vec3 tmpvar_63;
            tmpvar_63 = normalize(((
              (tmpvar_58.yzx * tmpvar_60.zxy)
             - 
              (tmpvar_58.zxy * tmpvar_60.yzx)
            ) - tmpvar_62));
            highp mat3 tmpvar_64;
            tmpvar_64[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_64[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_64[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_65;
            highp vec3 N_66;
            N_66 = (tmpvar_64 * tmpvar_63);
            tmpvar_65 = (xlv_TEXCOORD6 - (2.0 * (
              dot (N_66, xlv_TEXCOORD6)
             * N_66)));
            lowp vec4 tmpvar_67;
            tmpvar_67 = textureCube (_Cube, tmpvar_65);
            highp float tmpvar_68;
            tmpvar_68 = clamp ((tmpvar_37 + (tmpvar_38 * 0.5)), 0.0, 1.0);
            lowp vec3 tmpvar_69;
            tmpvar_69 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_68));
            highp vec3 tmpvar_70;
            tmpvar_70 = ((tmpvar_67.xyz * tmpvar_69) * faceColor_27.w);
            tmpvar_20 = faceColor_27.xyz;
            tmpvar_21 = -(tmpvar_63);
            tmpvar_22 = tmpvar_70;
            highp float tmpvar_71;
            tmpvar_71 = clamp ((tmpvar_37 + (tmpvar_38 * 0.5)), 0.0, 1.0);
            tmpvar_23 = 1.0;
            tmpvar_24 = faceColor_27.w;
            tmpvar_5 = tmpvar_20;
            tmpvar_7 = tmpvar_22;
            tmpvar_8 = tmpvar_23;
            tmpvar_9 = tmpvar_24;
            lowp float tmpvar_72;
            tmpvar_72 = dot (_unity_tbn_0_14, tmpvar_21);
            worldN_3.x = tmpvar_72;
            lowp float tmpvar_73;
            tmpvar_73 = dot (_unity_tbn_1_13, tmpvar_21);
            worldN_3.y = tmpvar_73;
            lowp float tmpvar_74;
            tmpvar_74 = dot (_unity_tbn_2_12, tmpvar_21);
            worldN_3.z = tmpvar_74;
            highp vec3 tmpvar_75;
            tmpvar_75 = normalize(worldN_3);
            worldN_3 = tmpvar_75;
            tmpvar_6 = tmpvar_75;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_11;
            mediump vec3 normalWorld_76;
            normalWorld_76 = tmpvar_6;
            mediump vec4 tmpvar_77;
            tmpvar_77.w = 1.0;
            tmpvar_77.xyz = normalWorld_76;
            mediump vec3 x_78;
            x_78.x = dot (unity_SHAr, tmpvar_77);
            x_78.y = dot (unity_SHAg, tmpvar_77);
            x_78.z = dot (unity_SHAb, tmpvar_77);
            mediump vec3 tmpvar_79;
            tmpvar_79 = max (((1.055 * 
              pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD7 + x_78)), vec3(0.4166667, 0.4166667, 0.4166667))
            ) - 0.055), vec3(0.0, 0.0, 0.0));
            mediump vec3 viewDir_80;
            viewDir_80 = worldViewDir_10;
            lowp vec4 c_81;
            lowp vec4 c_82;
            highp float nh_83;
            lowp float diff_84;
            mediump float tmpvar_85;
            tmpvar_85 = max (0.0, dot (tmpvar_6, tmpvar_2));
            diff_84 = tmpvar_85;
            mediump float tmpvar_86;
            tmpvar_86 = max (0.0, dot (tmpvar_6, normalize(
              (tmpvar_2 + viewDir_80)
            )));
            nh_83 = tmpvar_86;
            mediump float y_87;
            y_87 = (mix (_FaceShininess, _OutlineShininess, tmpvar_71) * 128.0);
            highp float tmpvar_88;
            tmpvar_88 = pow (nh_83, y_87);
            c_82.xyz = (((tmpvar_20 * tmpvar_1) * diff_84) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_88));
            c_82.w = tmpvar_24;
            c_81.w = c_82.w;
            c_81.xyz = (c_82.xyz + (tmpvar_20 * tmpvar_79));
            c_4.w = c_81.w;
            c_4.xyz = (c_81.xyz + tmpvar_22);
            gl_FragData[0] = c_4;
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
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform mediump vec4 _WorldSpaceLightPos0;
          uniform mediump vec4 unity_SHAr;
          uniform mediump vec4 unity_SHAg;
          uniform mediump vec4 unity_SHAb;
          uniform highp mat4 unity_ObjectToWorld;
          uniform lowp vec4 _LightColor0;
          uniform lowp vec4 _SpecColor;
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
          uniform highp float _Bevel;
          uniform highp float _BevelOffset;
          uniform highp float _BevelWidth;
          uniform highp float _BevelClamp;
          uniform highp float _BevelRoundness;
          uniform sampler2D _BumpMap;
          uniform highp float _BumpOutline;
          uniform highp float _BumpFace;
          uniform lowp samplerCube _Cube;
          uniform lowp vec4 _ReflectFaceColor;
          uniform lowp vec4 _ReflectOutlineColor;
          uniform highp float _ShaderFlags;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          uniform highp float _TextureWidth;
          uniform highp float _TextureHeight;
          uniform highp float _GradientScale;
          uniform mediump float _FaceShininess;
          uniform mediump float _OutlineShininess;
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
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp vec4 c_4;
            lowp vec3 tmpvar_5;
            lowp vec3 tmpvar_6;
            lowp vec3 tmpvar_7;
            lowp float tmpvar_8;
            lowp float tmpvar_9;
            highp vec3 worldViewDir_10;
            lowp vec3 lightDir_11;
            lowp vec3 _unity_tbn_2_12;
            lowp vec3 _unity_tbn_1_13;
            lowp vec3 _unity_tbn_0_14;
            highp vec3 tmpvar_15;
            tmpvar_15 = xlv_TEXCOORD2.xyz;
            _unity_tbn_0_14 = tmpvar_15;
            highp vec3 tmpvar_16;
            tmpvar_16 = xlv_TEXCOORD3.xyz;
            _unity_tbn_1_13 = tmpvar_16;
            highp vec3 tmpvar_17;
            tmpvar_17 = xlv_TEXCOORD4.xyz;
            _unity_tbn_2_12 = tmpvar_17;
            highp vec3 tmpvar_18;
            tmpvar_18.x = xlv_TEXCOORD2.w;
            tmpvar_18.y = xlv_TEXCOORD3.w;
            tmpvar_18.z = xlv_TEXCOORD4.w;
            mediump vec3 tmpvar_19;
            tmpvar_19 = _WorldSpaceLightPos0.xyz;
            lightDir_11 = tmpvar_19;
            worldViewDir_10 = normalize((_WorldSpaceCameraPos - tmpvar_18));
            tmpvar_5 = vec3(0.0, 0.0, 0.0);
            tmpvar_7 = vec3(0.0, 0.0, 0.0);
            tmpvar_9 = 0.0;
            tmpvar_8 = 0.0;
            tmpvar_6 = vec3(0.0, 0.0, 1.0);
            lowp vec3 tmpvar_20;
            lowp vec3 tmpvar_21;
            lowp vec3 tmpvar_22;
            lowp float tmpvar_23;
            lowp float tmpvar_24;
            tmpvar_20 = tmpvar_5;
            tmpvar_21 = tmpvar_6;
            tmpvar_22 = tmpvar_7;
            tmpvar_23 = tmpvar_8;
            tmpvar_24 = tmpvar_9;
            highp vec3 bump_25;
            highp vec4 outlineColor_26;
            highp vec4 faceColor_27;
            highp float c_28;
            highp vec4 smp4x_29;
            highp vec3 tmpvar_30;
            tmpvar_30.z = 0.0;
            tmpvar_30.x = (1.0/(_TextureWidth));
            tmpvar_30.y = (1.0/(_TextureHeight));
            highp vec2 P_31;
            P_31 = (xlv_TEXCOORD0.xy - tmpvar_30.xz);
            highp vec2 P_32;
            P_32 = (xlv_TEXCOORD0.xy + tmpvar_30.xz);
            highp vec2 P_33;
            P_33 = (xlv_TEXCOORD0.xy - tmpvar_30.zy);
            highp vec2 P_34;
            P_34 = (xlv_TEXCOORD0.xy + tmpvar_30.zy);
            lowp vec4 tmpvar_35;
            tmpvar_35.x = texture2D (_MainTex, P_31).w;
            tmpvar_35.y = texture2D (_MainTex, P_32).w;
            tmpvar_35.z = texture2D (_MainTex, P_33).w;
            tmpvar_35.w = texture2D (_MainTex, P_34).w;
            smp4x_29 = tmpvar_35;
            lowp float tmpvar_36;
            tmpvar_36 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_28 = tmpvar_36;
            highp float tmpvar_37;
            tmpvar_37 = (((
              (0.5 - c_28)
             - xlv_TEXCOORD5.x) * xlv_TEXCOORD5.y) + 0.5);
            highp float tmpvar_38;
            tmpvar_38 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD5.y);
            highp float tmpvar_39;
            tmpvar_39 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD5.y);
            faceColor_27 = _FaceColor;
            outlineColor_26 = _OutlineColor;
            faceColor_27 = (faceColor_27 * xlv_COLOR0);
            outlineColor_26.w = (outlineColor_26.w * xlv_COLOR0.w);
            highp vec2 tmpvar_40;
            tmpvar_40.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_40.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_41;
            tmpvar_41 = texture2D (_FaceTex, tmpvar_40);
            faceColor_27 = (faceColor_27 * tmpvar_41);
            highp vec2 tmpvar_42;
            tmpvar_42.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_42.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_43;
            tmpvar_43 = texture2D (_OutlineTex, tmpvar_42);
            outlineColor_26 = (outlineColor_26 * tmpvar_43);
            mediump float d_44;
            d_44 = tmpvar_37;
            lowp vec4 faceColor_45;
            faceColor_45 = faceColor_27;
            lowp vec4 outlineColor_46;
            outlineColor_46 = outlineColor_26;
            mediump float outline_47;
            outline_47 = tmpvar_38;
            mediump float softness_48;
            softness_48 = tmpvar_39;
            mediump float tmpvar_49;
            tmpvar_49 = (1.0 - clamp ((
              ((d_44 - (outline_47 * 0.5)) + (softness_48 * 0.5))
             / 
              (1.0 + softness_48)
            ), 0.0, 1.0));
            faceColor_45.xyz = (faceColor_45.xyz * faceColor_45.w);
            outlineColor_46.xyz = (outlineColor_46.xyz * outlineColor_46.w);
            mediump vec4 tmpvar_50;
            tmpvar_50 = mix (faceColor_45, outlineColor_46, vec4((clamp (
              (d_44 + (outline_47 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_47)
            ))));
            faceColor_45 = tmpvar_50;
            faceColor_45 = (faceColor_45 * tmpvar_49);
            faceColor_27 = faceColor_45;
            faceColor_27.xyz = (faceColor_27.xyz / max (faceColor_27.w, 0.0001));
            highp vec4 h_51;
            h_51 = smp4x_29;
            highp float tmpvar_52;
            tmpvar_52 = (_ShaderFlags / 2.0);
            highp float tmpvar_53;
            tmpvar_53 = (fract(abs(tmpvar_52)) * 2.0);
            highp float tmpvar_54;
            if ((tmpvar_52 >= 0.0)) {
              tmpvar_54 = tmpvar_53;
            } else {
              tmpvar_54 = -(tmpvar_53);
            };
            h_51 = (smp4x_29 + (xlv_TEXCOORD5.x + _BevelOffset));
            highp float tmpvar_55;
            tmpvar_55 = max (0.01, (_OutlineWidth + _BevelWidth));
            h_51 = (h_51 - 0.5);
            h_51 = (h_51 / tmpvar_55);
            highp vec4 tmpvar_56;
            tmpvar_56 = clamp ((h_51 + 0.5), 0.0, 1.0);
            h_51 = tmpvar_56;
            if (bool(float((tmpvar_54 >= 1.0)))) {
              h_51 = (1.0 - abs((
                (tmpvar_56 * 2.0)
               - 1.0)));
            };
            h_51 = (min (mix (h_51, 
              sin(((h_51 * 3.141592) / 2.0))
            , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((_Bevel * tmpvar_55) * (_GradientScale * -2.0)));
            highp vec3 tmpvar_57;
            tmpvar_57.xy = vec2(1.0, 0.0);
            tmpvar_57.z = (h_51.y - h_51.x);
            highp vec3 tmpvar_58;
            tmpvar_58 = normalize(tmpvar_57);
            highp vec3 tmpvar_59;
            tmpvar_59.xy = vec2(0.0, -1.0);
            tmpvar_59.z = (h_51.w - h_51.z);
            highp vec3 tmpvar_60;
            tmpvar_60 = normalize(tmpvar_59);
            lowp vec3 tmpvar_61;
            tmpvar_61 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
            bump_25 = tmpvar_61;
            bump_25 = (bump_25 * mix (_BumpFace, _BumpOutline, clamp (
              (tmpvar_37 + (tmpvar_38 * 0.5))
            , 0.0, 1.0)));
            highp vec3 tmpvar_62;
            tmpvar_62 = mix (vec3(0.0, 0.0, 1.0), bump_25, faceColor_27.www);
            bump_25 = tmpvar_62;
            highp vec3 tmpvar_63;
            tmpvar_63 = normalize(((
              (tmpvar_58.yzx * tmpvar_60.zxy)
             - 
              (tmpvar_58.zxy * tmpvar_60.yzx)
            ) - tmpvar_62));
            highp mat3 tmpvar_64;
            tmpvar_64[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_64[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_64[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_65;
            highp vec3 N_66;
            N_66 = (tmpvar_64 * tmpvar_63);
            tmpvar_65 = (xlv_TEXCOORD6 - (2.0 * (
              dot (N_66, xlv_TEXCOORD6)
             * N_66)));
            lowp vec4 tmpvar_67;
            tmpvar_67 = textureCube (_Cube, tmpvar_65);
            highp float tmpvar_68;
            tmpvar_68 = clamp ((tmpvar_37 + (tmpvar_38 * 0.5)), 0.0, 1.0);
            lowp vec3 tmpvar_69;
            tmpvar_69 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_68));
            highp vec3 tmpvar_70;
            tmpvar_70 = ((tmpvar_67.xyz * tmpvar_69) * faceColor_27.w);
            tmpvar_20 = faceColor_27.xyz;
            tmpvar_21 = -(tmpvar_63);
            tmpvar_22 = tmpvar_70;
            highp float tmpvar_71;
            tmpvar_71 = clamp ((tmpvar_37 + (tmpvar_38 * 0.5)), 0.0, 1.0);
            tmpvar_23 = 1.0;
            tmpvar_24 = faceColor_27.w;
            tmpvar_5 = tmpvar_20;
            tmpvar_7 = tmpvar_22;
            tmpvar_8 = tmpvar_23;
            tmpvar_9 = tmpvar_24;
            lowp float tmpvar_72;
            tmpvar_72 = dot (_unity_tbn_0_14, tmpvar_21);
            worldN_3.x = tmpvar_72;
            lowp float tmpvar_73;
            tmpvar_73 = dot (_unity_tbn_1_13, tmpvar_21);
            worldN_3.y = tmpvar_73;
            lowp float tmpvar_74;
            tmpvar_74 = dot (_unity_tbn_2_12, tmpvar_21);
            worldN_3.z = tmpvar_74;
            highp vec3 tmpvar_75;
            tmpvar_75 = normalize(worldN_3);
            worldN_3 = tmpvar_75;
            tmpvar_6 = tmpvar_75;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_11;
            mediump vec3 normalWorld_76;
            normalWorld_76 = tmpvar_6;
            mediump vec4 tmpvar_77;
            tmpvar_77.w = 1.0;
            tmpvar_77.xyz = normalWorld_76;
            mediump vec3 x_78;
            x_78.x = dot (unity_SHAr, tmpvar_77);
            x_78.y = dot (unity_SHAg, tmpvar_77);
            x_78.z = dot (unity_SHAb, tmpvar_77);
            mediump vec3 tmpvar_79;
            tmpvar_79 = max (((1.055 * 
              pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD7 + x_78)), vec3(0.4166667, 0.4166667, 0.4166667))
            ) - 0.055), vec3(0.0, 0.0, 0.0));
            mediump vec3 viewDir_80;
            viewDir_80 = worldViewDir_10;
            lowp vec4 c_81;
            lowp vec4 c_82;
            highp float nh_83;
            lowp float diff_84;
            mediump float tmpvar_85;
            tmpvar_85 = max (0.0, dot (tmpvar_6, tmpvar_2));
            diff_84 = tmpvar_85;
            mediump float tmpvar_86;
            tmpvar_86 = max (0.0, dot (tmpvar_6, normalize(
              (tmpvar_2 + viewDir_80)
            )));
            nh_83 = tmpvar_86;
            mediump float y_87;
            y_87 = (mix (_FaceShininess, _OutlineShininess, tmpvar_71) * 128.0);
            highp float tmpvar_88;
            tmpvar_88 = pow (nh_83, y_87);
            c_82.xyz = (((tmpvar_20 * tmpvar_1) * diff_84) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_88));
            c_82.w = tmpvar_24;
            c_81.w = c_82.w;
            c_81.xyz = (c_82.xyz + (tmpvar_20 * tmpvar_79));
            c_4.w = c_81.w;
            c_4.xyz = (c_81.xyz + tmpvar_22);
            gl_FragData[0] = c_4;
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
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform mediump vec4 _WorldSpaceLightPos0;
          uniform mediump vec4 unity_SHAr;
          uniform mediump vec4 unity_SHAg;
          uniform mediump vec4 unity_SHAb;
          uniform highp mat4 unity_ObjectToWorld;
          uniform lowp vec4 _LightColor0;
          uniform lowp vec4 _SpecColor;
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
          uniform highp float _Bevel;
          uniform highp float _BevelOffset;
          uniform highp float _BevelWidth;
          uniform highp float _BevelClamp;
          uniform highp float _BevelRoundness;
          uniform sampler2D _BumpMap;
          uniform highp float _BumpOutline;
          uniform highp float _BumpFace;
          uniform lowp samplerCube _Cube;
          uniform lowp vec4 _ReflectFaceColor;
          uniform lowp vec4 _ReflectOutlineColor;
          uniform highp float _ShaderFlags;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          uniform highp float _TextureWidth;
          uniform highp float _TextureHeight;
          uniform highp float _GradientScale;
          uniform mediump float _FaceShininess;
          uniform mediump float _OutlineShininess;
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
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp vec4 c_4;
            lowp vec3 tmpvar_5;
            lowp vec3 tmpvar_6;
            lowp vec3 tmpvar_7;
            lowp float tmpvar_8;
            lowp float tmpvar_9;
            highp vec3 worldViewDir_10;
            lowp vec3 lightDir_11;
            lowp vec3 _unity_tbn_2_12;
            lowp vec3 _unity_tbn_1_13;
            lowp vec3 _unity_tbn_0_14;
            highp vec3 tmpvar_15;
            tmpvar_15 = xlv_TEXCOORD2.xyz;
            _unity_tbn_0_14 = tmpvar_15;
            highp vec3 tmpvar_16;
            tmpvar_16 = xlv_TEXCOORD3.xyz;
            _unity_tbn_1_13 = tmpvar_16;
            highp vec3 tmpvar_17;
            tmpvar_17 = xlv_TEXCOORD4.xyz;
            _unity_tbn_2_12 = tmpvar_17;
            highp vec3 tmpvar_18;
            tmpvar_18.x = xlv_TEXCOORD2.w;
            tmpvar_18.y = xlv_TEXCOORD3.w;
            tmpvar_18.z = xlv_TEXCOORD4.w;
            mediump vec3 tmpvar_19;
            tmpvar_19 = _WorldSpaceLightPos0.xyz;
            lightDir_11 = tmpvar_19;
            worldViewDir_10 = normalize((_WorldSpaceCameraPos - tmpvar_18));
            tmpvar_5 = vec3(0.0, 0.0, 0.0);
            tmpvar_7 = vec3(0.0, 0.0, 0.0);
            tmpvar_9 = 0.0;
            tmpvar_8 = 0.0;
            tmpvar_6 = vec3(0.0, 0.0, 1.0);
            lowp vec3 tmpvar_20;
            lowp vec3 tmpvar_21;
            lowp vec3 tmpvar_22;
            lowp float tmpvar_23;
            lowp float tmpvar_24;
            tmpvar_20 = tmpvar_5;
            tmpvar_21 = tmpvar_6;
            tmpvar_22 = tmpvar_7;
            tmpvar_23 = tmpvar_8;
            tmpvar_24 = tmpvar_9;
            highp vec3 bump_25;
            highp vec4 outlineColor_26;
            highp vec4 faceColor_27;
            highp float c_28;
            highp vec4 smp4x_29;
            highp vec3 tmpvar_30;
            tmpvar_30.z = 0.0;
            tmpvar_30.x = (1.0/(_TextureWidth));
            tmpvar_30.y = (1.0/(_TextureHeight));
            highp vec2 P_31;
            P_31 = (xlv_TEXCOORD0.xy - tmpvar_30.xz);
            highp vec2 P_32;
            P_32 = (xlv_TEXCOORD0.xy + tmpvar_30.xz);
            highp vec2 P_33;
            P_33 = (xlv_TEXCOORD0.xy - tmpvar_30.zy);
            highp vec2 P_34;
            P_34 = (xlv_TEXCOORD0.xy + tmpvar_30.zy);
            lowp vec4 tmpvar_35;
            tmpvar_35.x = texture2D (_MainTex, P_31).w;
            tmpvar_35.y = texture2D (_MainTex, P_32).w;
            tmpvar_35.z = texture2D (_MainTex, P_33).w;
            tmpvar_35.w = texture2D (_MainTex, P_34).w;
            smp4x_29 = tmpvar_35;
            lowp float tmpvar_36;
            tmpvar_36 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_28 = tmpvar_36;
            highp float tmpvar_37;
            tmpvar_37 = (((
              (0.5 - c_28)
             - xlv_TEXCOORD5.x) * xlv_TEXCOORD5.y) + 0.5);
            highp float tmpvar_38;
            tmpvar_38 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD5.y);
            highp float tmpvar_39;
            tmpvar_39 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD5.y);
            faceColor_27 = _FaceColor;
            outlineColor_26 = _OutlineColor;
            faceColor_27 = (faceColor_27 * xlv_COLOR0);
            outlineColor_26.w = (outlineColor_26.w * xlv_COLOR0.w);
            highp vec2 tmpvar_40;
            tmpvar_40.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_40.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_41;
            tmpvar_41 = texture2D (_FaceTex, tmpvar_40);
            faceColor_27 = (faceColor_27 * tmpvar_41);
            highp vec2 tmpvar_42;
            tmpvar_42.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_42.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_43;
            tmpvar_43 = texture2D (_OutlineTex, tmpvar_42);
            outlineColor_26 = (outlineColor_26 * tmpvar_43);
            mediump float d_44;
            d_44 = tmpvar_37;
            lowp vec4 faceColor_45;
            faceColor_45 = faceColor_27;
            lowp vec4 outlineColor_46;
            outlineColor_46 = outlineColor_26;
            mediump float outline_47;
            outline_47 = tmpvar_38;
            mediump float softness_48;
            softness_48 = tmpvar_39;
            mediump float tmpvar_49;
            tmpvar_49 = (1.0 - clamp ((
              ((d_44 - (outline_47 * 0.5)) + (softness_48 * 0.5))
             / 
              (1.0 + softness_48)
            ), 0.0, 1.0));
            faceColor_45.xyz = (faceColor_45.xyz * faceColor_45.w);
            outlineColor_46.xyz = (outlineColor_46.xyz * outlineColor_46.w);
            mediump vec4 tmpvar_50;
            tmpvar_50 = mix (faceColor_45, outlineColor_46, vec4((clamp (
              (d_44 + (outline_47 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_47)
            ))));
            faceColor_45 = tmpvar_50;
            faceColor_45 = (faceColor_45 * tmpvar_49);
            faceColor_27 = faceColor_45;
            faceColor_27.xyz = (faceColor_27.xyz / max (faceColor_27.w, 0.0001));
            highp vec4 h_51;
            h_51 = smp4x_29;
            highp float tmpvar_52;
            tmpvar_52 = (_ShaderFlags / 2.0);
            highp float tmpvar_53;
            tmpvar_53 = (fract(abs(tmpvar_52)) * 2.0);
            highp float tmpvar_54;
            if ((tmpvar_52 >= 0.0)) {
              tmpvar_54 = tmpvar_53;
            } else {
              tmpvar_54 = -(tmpvar_53);
            };
            h_51 = (smp4x_29 + (xlv_TEXCOORD5.x + _BevelOffset));
            highp float tmpvar_55;
            tmpvar_55 = max (0.01, (_OutlineWidth + _BevelWidth));
            h_51 = (h_51 - 0.5);
            h_51 = (h_51 / tmpvar_55);
            highp vec4 tmpvar_56;
            tmpvar_56 = clamp ((h_51 + 0.5), 0.0, 1.0);
            h_51 = tmpvar_56;
            if (bool(float((tmpvar_54 >= 1.0)))) {
              h_51 = (1.0 - abs((
                (tmpvar_56 * 2.0)
               - 1.0)));
            };
            h_51 = (min (mix (h_51, 
              sin(((h_51 * 3.141592) / 2.0))
            , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((_Bevel * tmpvar_55) * (_GradientScale * -2.0)));
            highp vec3 tmpvar_57;
            tmpvar_57.xy = vec2(1.0, 0.0);
            tmpvar_57.z = (h_51.y - h_51.x);
            highp vec3 tmpvar_58;
            tmpvar_58 = normalize(tmpvar_57);
            highp vec3 tmpvar_59;
            tmpvar_59.xy = vec2(0.0, -1.0);
            tmpvar_59.z = (h_51.w - h_51.z);
            highp vec3 tmpvar_60;
            tmpvar_60 = normalize(tmpvar_59);
            lowp vec3 tmpvar_61;
            tmpvar_61 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
            bump_25 = tmpvar_61;
            bump_25 = (bump_25 * mix (_BumpFace, _BumpOutline, clamp (
              (tmpvar_37 + (tmpvar_38 * 0.5))
            , 0.0, 1.0)));
            highp vec3 tmpvar_62;
            tmpvar_62 = mix (vec3(0.0, 0.0, 1.0), bump_25, faceColor_27.www);
            bump_25 = tmpvar_62;
            highp vec3 tmpvar_63;
            tmpvar_63 = normalize(((
              (tmpvar_58.yzx * tmpvar_60.zxy)
             - 
              (tmpvar_58.zxy * tmpvar_60.yzx)
            ) - tmpvar_62));
            highp mat3 tmpvar_64;
            tmpvar_64[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_64[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_64[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_65;
            highp vec3 N_66;
            N_66 = (tmpvar_64 * tmpvar_63);
            tmpvar_65 = (xlv_TEXCOORD6 - (2.0 * (
              dot (N_66, xlv_TEXCOORD6)
             * N_66)));
            lowp vec4 tmpvar_67;
            tmpvar_67 = textureCube (_Cube, tmpvar_65);
            highp float tmpvar_68;
            tmpvar_68 = clamp ((tmpvar_37 + (tmpvar_38 * 0.5)), 0.0, 1.0);
            lowp vec3 tmpvar_69;
            tmpvar_69 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_68));
            highp vec3 tmpvar_70;
            tmpvar_70 = ((tmpvar_67.xyz * tmpvar_69) * faceColor_27.w);
            tmpvar_20 = faceColor_27.xyz;
            tmpvar_21 = -(tmpvar_63);
            tmpvar_22 = tmpvar_70;
            highp float tmpvar_71;
            tmpvar_71 = clamp ((tmpvar_37 + (tmpvar_38 * 0.5)), 0.0, 1.0);
            tmpvar_23 = 1.0;
            tmpvar_24 = faceColor_27.w;
            tmpvar_5 = tmpvar_20;
            tmpvar_7 = tmpvar_22;
            tmpvar_8 = tmpvar_23;
            tmpvar_9 = tmpvar_24;
            lowp float tmpvar_72;
            tmpvar_72 = dot (_unity_tbn_0_14, tmpvar_21);
            worldN_3.x = tmpvar_72;
            lowp float tmpvar_73;
            tmpvar_73 = dot (_unity_tbn_1_13, tmpvar_21);
            worldN_3.y = tmpvar_73;
            lowp float tmpvar_74;
            tmpvar_74 = dot (_unity_tbn_2_12, tmpvar_21);
            worldN_3.z = tmpvar_74;
            highp vec3 tmpvar_75;
            tmpvar_75 = normalize(worldN_3);
            worldN_3 = tmpvar_75;
            tmpvar_6 = tmpvar_75;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_11;
            mediump vec3 normalWorld_76;
            normalWorld_76 = tmpvar_6;
            mediump vec4 tmpvar_77;
            tmpvar_77.w = 1.0;
            tmpvar_77.xyz = normalWorld_76;
            mediump vec3 x_78;
            x_78.x = dot (unity_SHAr, tmpvar_77);
            x_78.y = dot (unity_SHAg, tmpvar_77);
            x_78.z = dot (unity_SHAb, tmpvar_77);
            mediump vec3 tmpvar_79;
            tmpvar_79 = max (((1.055 * 
              pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD7 + x_78)), vec3(0.4166667, 0.4166667, 0.4166667))
            ) - 0.055), vec3(0.0, 0.0, 0.0));
            mediump vec3 viewDir_80;
            viewDir_80 = worldViewDir_10;
            lowp vec4 c_81;
            lowp vec4 c_82;
            highp float nh_83;
            lowp float diff_84;
            mediump float tmpvar_85;
            tmpvar_85 = max (0.0, dot (tmpvar_6, tmpvar_2));
            diff_84 = tmpvar_85;
            mediump float tmpvar_86;
            tmpvar_86 = max (0.0, dot (tmpvar_6, normalize(
              (tmpvar_2 + viewDir_80)
            )));
            nh_83 = tmpvar_86;
            mediump float y_87;
            y_87 = (mix (_FaceShininess, _OutlineShininess, tmpvar_71) * 128.0);
            highp float tmpvar_88;
            tmpvar_88 = pow (nh_83, y_87);
            c_82.xyz = (((tmpvar_20 * tmpvar_1) * diff_84) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_88));
            c_82.w = tmpvar_24;
            c_81.w = c_82.w;
            c_81.xyz = (c_82.xyz + (tmpvar_20 * tmpvar_79));
            c_4.w = c_81.w;
            c_4.xyz = (c_81.xyz + tmpvar_22);
            gl_FragData[0] = c_4;
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
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform mediump vec4 _WorldSpaceLightPos0;
          uniform mediump vec4 unity_SHAr;
          uniform mediump vec4 unity_SHAg;
          uniform mediump vec4 unity_SHAb;
          uniform highp mat4 unity_ObjectToWorld;
          uniform lowp vec4 _LightColor0;
          uniform lowp vec4 _SpecColor;
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
          uniform highp float _Bevel;
          uniform highp float _BevelOffset;
          uniform highp float _BevelWidth;
          uniform highp float _BevelClamp;
          uniform highp float _BevelRoundness;
          uniform sampler2D _BumpMap;
          uniform highp float _BumpOutline;
          uniform highp float _BumpFace;
          uniform lowp samplerCube _Cube;
          uniform lowp vec4 _ReflectFaceColor;
          uniform lowp vec4 _ReflectOutlineColor;
          uniform highp float _ShaderFlags;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          uniform highp float _TextureWidth;
          uniform highp float _TextureHeight;
          uniform highp float _GradientScale;
          uniform mediump float _FaceShininess;
          uniform mediump float _OutlineShininess;
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
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp vec4 c_4;
            lowp vec3 tmpvar_5;
            lowp vec3 tmpvar_6;
            lowp vec3 tmpvar_7;
            lowp float tmpvar_8;
            lowp float tmpvar_9;
            highp vec3 worldViewDir_10;
            lowp vec3 lightDir_11;
            lowp vec3 _unity_tbn_2_12;
            lowp vec3 _unity_tbn_1_13;
            lowp vec3 _unity_tbn_0_14;
            highp vec3 tmpvar_15;
            tmpvar_15 = xlv_TEXCOORD2.xyz;
            _unity_tbn_0_14 = tmpvar_15;
            highp vec3 tmpvar_16;
            tmpvar_16 = xlv_TEXCOORD3.xyz;
            _unity_tbn_1_13 = tmpvar_16;
            highp vec3 tmpvar_17;
            tmpvar_17 = xlv_TEXCOORD4.xyz;
            _unity_tbn_2_12 = tmpvar_17;
            highp vec3 tmpvar_18;
            tmpvar_18.x = xlv_TEXCOORD2.w;
            tmpvar_18.y = xlv_TEXCOORD3.w;
            tmpvar_18.z = xlv_TEXCOORD4.w;
            mediump vec3 tmpvar_19;
            tmpvar_19 = _WorldSpaceLightPos0.xyz;
            lightDir_11 = tmpvar_19;
            worldViewDir_10 = normalize((_WorldSpaceCameraPos - tmpvar_18));
            tmpvar_5 = vec3(0.0, 0.0, 0.0);
            tmpvar_7 = vec3(0.0, 0.0, 0.0);
            tmpvar_9 = 0.0;
            tmpvar_8 = 0.0;
            tmpvar_6 = vec3(0.0, 0.0, 1.0);
            lowp vec3 tmpvar_20;
            lowp vec3 tmpvar_21;
            lowp vec3 tmpvar_22;
            lowp float tmpvar_23;
            lowp float tmpvar_24;
            tmpvar_20 = tmpvar_5;
            tmpvar_21 = tmpvar_6;
            tmpvar_22 = tmpvar_7;
            tmpvar_23 = tmpvar_8;
            tmpvar_24 = tmpvar_9;
            highp vec3 bump_25;
            highp vec4 outlineColor_26;
            highp vec4 faceColor_27;
            highp float c_28;
            highp vec4 smp4x_29;
            highp vec3 tmpvar_30;
            tmpvar_30.z = 0.0;
            tmpvar_30.x = (1.0/(_TextureWidth));
            tmpvar_30.y = (1.0/(_TextureHeight));
            highp vec2 P_31;
            P_31 = (xlv_TEXCOORD0.xy - tmpvar_30.xz);
            highp vec2 P_32;
            P_32 = (xlv_TEXCOORD0.xy + tmpvar_30.xz);
            highp vec2 P_33;
            P_33 = (xlv_TEXCOORD0.xy - tmpvar_30.zy);
            highp vec2 P_34;
            P_34 = (xlv_TEXCOORD0.xy + tmpvar_30.zy);
            lowp vec4 tmpvar_35;
            tmpvar_35.x = texture2D (_MainTex, P_31).w;
            tmpvar_35.y = texture2D (_MainTex, P_32).w;
            tmpvar_35.z = texture2D (_MainTex, P_33).w;
            tmpvar_35.w = texture2D (_MainTex, P_34).w;
            smp4x_29 = tmpvar_35;
            lowp float tmpvar_36;
            tmpvar_36 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_28 = tmpvar_36;
            highp float tmpvar_37;
            tmpvar_37 = (((
              (0.5 - c_28)
             - xlv_TEXCOORD5.x) * xlv_TEXCOORD5.y) + 0.5);
            highp float tmpvar_38;
            tmpvar_38 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD5.y);
            highp float tmpvar_39;
            tmpvar_39 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD5.y);
            faceColor_27 = _FaceColor;
            outlineColor_26 = _OutlineColor;
            faceColor_27 = (faceColor_27 * xlv_COLOR0);
            outlineColor_26.w = (outlineColor_26.w * xlv_COLOR0.w);
            highp vec2 tmpvar_40;
            tmpvar_40.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_40.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_41;
            tmpvar_41 = texture2D (_FaceTex, tmpvar_40);
            faceColor_27 = (faceColor_27 * tmpvar_41);
            highp vec2 tmpvar_42;
            tmpvar_42.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_42.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_43;
            tmpvar_43 = texture2D (_OutlineTex, tmpvar_42);
            outlineColor_26 = (outlineColor_26 * tmpvar_43);
            mediump float d_44;
            d_44 = tmpvar_37;
            lowp vec4 faceColor_45;
            faceColor_45 = faceColor_27;
            lowp vec4 outlineColor_46;
            outlineColor_46 = outlineColor_26;
            mediump float outline_47;
            outline_47 = tmpvar_38;
            mediump float softness_48;
            softness_48 = tmpvar_39;
            mediump float tmpvar_49;
            tmpvar_49 = (1.0 - clamp ((
              ((d_44 - (outline_47 * 0.5)) + (softness_48 * 0.5))
             / 
              (1.0 + softness_48)
            ), 0.0, 1.0));
            faceColor_45.xyz = (faceColor_45.xyz * faceColor_45.w);
            outlineColor_46.xyz = (outlineColor_46.xyz * outlineColor_46.w);
            mediump vec4 tmpvar_50;
            tmpvar_50 = mix (faceColor_45, outlineColor_46, vec4((clamp (
              (d_44 + (outline_47 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_47)
            ))));
            faceColor_45 = tmpvar_50;
            faceColor_45 = (faceColor_45 * tmpvar_49);
            faceColor_27 = faceColor_45;
            faceColor_27.xyz = (faceColor_27.xyz / max (faceColor_27.w, 0.0001));
            highp vec4 h_51;
            h_51 = smp4x_29;
            highp float tmpvar_52;
            tmpvar_52 = (_ShaderFlags / 2.0);
            highp float tmpvar_53;
            tmpvar_53 = (fract(abs(tmpvar_52)) * 2.0);
            highp float tmpvar_54;
            if ((tmpvar_52 >= 0.0)) {
              tmpvar_54 = tmpvar_53;
            } else {
              tmpvar_54 = -(tmpvar_53);
            };
            h_51 = (smp4x_29 + (xlv_TEXCOORD5.x + _BevelOffset));
            highp float tmpvar_55;
            tmpvar_55 = max (0.01, (_OutlineWidth + _BevelWidth));
            h_51 = (h_51 - 0.5);
            h_51 = (h_51 / tmpvar_55);
            highp vec4 tmpvar_56;
            tmpvar_56 = clamp ((h_51 + 0.5), 0.0, 1.0);
            h_51 = tmpvar_56;
            if (bool(float((tmpvar_54 >= 1.0)))) {
              h_51 = (1.0 - abs((
                (tmpvar_56 * 2.0)
               - 1.0)));
            };
            h_51 = (min (mix (h_51, 
              sin(((h_51 * 3.141592) / 2.0))
            , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((_Bevel * tmpvar_55) * (_GradientScale * -2.0)));
            highp vec3 tmpvar_57;
            tmpvar_57.xy = vec2(1.0, 0.0);
            tmpvar_57.z = (h_51.y - h_51.x);
            highp vec3 tmpvar_58;
            tmpvar_58 = normalize(tmpvar_57);
            highp vec3 tmpvar_59;
            tmpvar_59.xy = vec2(0.0, -1.0);
            tmpvar_59.z = (h_51.w - h_51.z);
            highp vec3 tmpvar_60;
            tmpvar_60 = normalize(tmpvar_59);
            lowp vec3 tmpvar_61;
            tmpvar_61 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
            bump_25 = tmpvar_61;
            bump_25 = (bump_25 * mix (_BumpFace, _BumpOutline, clamp (
              (tmpvar_37 + (tmpvar_38 * 0.5))
            , 0.0, 1.0)));
            highp vec3 tmpvar_62;
            tmpvar_62 = mix (vec3(0.0, 0.0, 1.0), bump_25, faceColor_27.www);
            bump_25 = tmpvar_62;
            highp vec3 tmpvar_63;
            tmpvar_63 = normalize(((
              (tmpvar_58.yzx * tmpvar_60.zxy)
             - 
              (tmpvar_58.zxy * tmpvar_60.yzx)
            ) - tmpvar_62));
            highp mat3 tmpvar_64;
            tmpvar_64[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_64[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_64[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_65;
            highp vec3 N_66;
            N_66 = (tmpvar_64 * tmpvar_63);
            tmpvar_65 = (xlv_TEXCOORD6 - (2.0 * (
              dot (N_66, xlv_TEXCOORD6)
             * N_66)));
            lowp vec4 tmpvar_67;
            tmpvar_67 = textureCube (_Cube, tmpvar_65);
            highp float tmpvar_68;
            tmpvar_68 = clamp ((tmpvar_37 + (tmpvar_38 * 0.5)), 0.0, 1.0);
            lowp vec3 tmpvar_69;
            tmpvar_69 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_68));
            highp vec3 tmpvar_70;
            tmpvar_70 = ((tmpvar_67.xyz * tmpvar_69) * faceColor_27.w);
            tmpvar_20 = faceColor_27.xyz;
            tmpvar_21 = -(tmpvar_63);
            tmpvar_22 = tmpvar_70;
            highp float tmpvar_71;
            tmpvar_71 = clamp ((tmpvar_37 + (tmpvar_38 * 0.5)), 0.0, 1.0);
            tmpvar_23 = 1.0;
            tmpvar_24 = faceColor_27.w;
            tmpvar_5 = tmpvar_20;
            tmpvar_7 = tmpvar_22;
            tmpvar_8 = tmpvar_23;
            tmpvar_9 = tmpvar_24;
            lowp float tmpvar_72;
            tmpvar_72 = dot (_unity_tbn_0_14, tmpvar_21);
            worldN_3.x = tmpvar_72;
            lowp float tmpvar_73;
            tmpvar_73 = dot (_unity_tbn_1_13, tmpvar_21);
            worldN_3.y = tmpvar_73;
            lowp float tmpvar_74;
            tmpvar_74 = dot (_unity_tbn_2_12, tmpvar_21);
            worldN_3.z = tmpvar_74;
            highp vec3 tmpvar_75;
            tmpvar_75 = normalize(worldN_3);
            worldN_3 = tmpvar_75;
            tmpvar_6 = tmpvar_75;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_11;
            mediump vec3 normalWorld_76;
            normalWorld_76 = tmpvar_6;
            mediump vec4 tmpvar_77;
            tmpvar_77.w = 1.0;
            tmpvar_77.xyz = normalWorld_76;
            mediump vec3 x_78;
            x_78.x = dot (unity_SHAr, tmpvar_77);
            x_78.y = dot (unity_SHAg, tmpvar_77);
            x_78.z = dot (unity_SHAb, tmpvar_77);
            mediump vec3 tmpvar_79;
            tmpvar_79 = max (((1.055 * 
              pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD7 + x_78)), vec3(0.4166667, 0.4166667, 0.4166667))
            ) - 0.055), vec3(0.0, 0.0, 0.0));
            mediump vec3 viewDir_80;
            viewDir_80 = worldViewDir_10;
            lowp vec4 c_81;
            lowp vec4 c_82;
            highp float nh_83;
            lowp float diff_84;
            mediump float tmpvar_85;
            tmpvar_85 = max (0.0, dot (tmpvar_6, tmpvar_2));
            diff_84 = tmpvar_85;
            mediump float tmpvar_86;
            tmpvar_86 = max (0.0, dot (tmpvar_6, normalize(
              (tmpvar_2 + viewDir_80)
            )));
            nh_83 = tmpvar_86;
            mediump float y_87;
            y_87 = (mix (_FaceShininess, _OutlineShininess, tmpvar_71) * 128.0);
            highp float tmpvar_88;
            tmpvar_88 = pow (nh_83, y_87);
            c_82.xyz = (((tmpvar_20 * tmpvar_1) * diff_84) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_88));
            c_82.w = tmpvar_24;
            c_81.w = c_82.w;
            c_81.xyz = (c_82.xyz + (tmpvar_20 * tmpvar_79));
            c_4.w = c_81.w;
            c_4.xyz = (c_81.xyz + tmpvar_22);
            gl_FragData[0] = c_4;
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
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform mediump vec4 _WorldSpaceLightPos0;
          uniform mediump vec4 unity_SHAr;
          uniform mediump vec4 unity_SHAg;
          uniform mediump vec4 unity_SHAb;
          uniform highp mat4 unity_ObjectToWorld;
          uniform lowp vec4 _LightColor0;
          uniform lowp vec4 _SpecColor;
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
          uniform highp float _Bevel;
          uniform highp float _BevelOffset;
          uniform highp float _BevelWidth;
          uniform highp float _BevelClamp;
          uniform highp float _BevelRoundness;
          uniform sampler2D _BumpMap;
          uniform highp float _BumpOutline;
          uniform highp float _BumpFace;
          uniform lowp samplerCube _Cube;
          uniform lowp vec4 _ReflectFaceColor;
          uniform lowp vec4 _ReflectOutlineColor;
          uniform highp float _ShaderFlags;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          uniform highp float _TextureWidth;
          uniform highp float _TextureHeight;
          uniform highp float _GradientScale;
          uniform mediump float _FaceShininess;
          uniform mediump float _OutlineShininess;
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
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp vec4 c_4;
            lowp vec3 tmpvar_5;
            lowp vec3 tmpvar_6;
            lowp vec3 tmpvar_7;
            lowp float tmpvar_8;
            lowp float tmpvar_9;
            highp vec3 worldViewDir_10;
            lowp vec3 lightDir_11;
            lowp vec3 _unity_tbn_2_12;
            lowp vec3 _unity_tbn_1_13;
            lowp vec3 _unity_tbn_0_14;
            highp vec3 tmpvar_15;
            tmpvar_15 = xlv_TEXCOORD2.xyz;
            _unity_tbn_0_14 = tmpvar_15;
            highp vec3 tmpvar_16;
            tmpvar_16 = xlv_TEXCOORD3.xyz;
            _unity_tbn_1_13 = tmpvar_16;
            highp vec3 tmpvar_17;
            tmpvar_17 = xlv_TEXCOORD4.xyz;
            _unity_tbn_2_12 = tmpvar_17;
            highp vec3 tmpvar_18;
            tmpvar_18.x = xlv_TEXCOORD2.w;
            tmpvar_18.y = xlv_TEXCOORD3.w;
            tmpvar_18.z = xlv_TEXCOORD4.w;
            mediump vec3 tmpvar_19;
            tmpvar_19 = _WorldSpaceLightPos0.xyz;
            lightDir_11 = tmpvar_19;
            worldViewDir_10 = normalize((_WorldSpaceCameraPos - tmpvar_18));
            tmpvar_5 = vec3(0.0, 0.0, 0.0);
            tmpvar_7 = vec3(0.0, 0.0, 0.0);
            tmpvar_9 = 0.0;
            tmpvar_8 = 0.0;
            tmpvar_6 = vec3(0.0, 0.0, 1.0);
            lowp vec3 tmpvar_20;
            lowp vec3 tmpvar_21;
            lowp vec3 tmpvar_22;
            lowp float tmpvar_23;
            lowp float tmpvar_24;
            tmpvar_20 = tmpvar_5;
            tmpvar_21 = tmpvar_6;
            tmpvar_22 = tmpvar_7;
            tmpvar_23 = tmpvar_8;
            tmpvar_24 = tmpvar_9;
            highp vec3 bump_25;
            highp vec4 outlineColor_26;
            highp vec4 faceColor_27;
            highp float c_28;
            highp vec4 smp4x_29;
            highp vec3 tmpvar_30;
            tmpvar_30.z = 0.0;
            tmpvar_30.x = (1.0/(_TextureWidth));
            tmpvar_30.y = (1.0/(_TextureHeight));
            highp vec2 P_31;
            P_31 = (xlv_TEXCOORD0.xy - tmpvar_30.xz);
            highp vec2 P_32;
            P_32 = (xlv_TEXCOORD0.xy + tmpvar_30.xz);
            highp vec2 P_33;
            P_33 = (xlv_TEXCOORD0.xy - tmpvar_30.zy);
            highp vec2 P_34;
            P_34 = (xlv_TEXCOORD0.xy + tmpvar_30.zy);
            lowp vec4 tmpvar_35;
            tmpvar_35.x = texture2D (_MainTex, P_31).w;
            tmpvar_35.y = texture2D (_MainTex, P_32).w;
            tmpvar_35.z = texture2D (_MainTex, P_33).w;
            tmpvar_35.w = texture2D (_MainTex, P_34).w;
            smp4x_29 = tmpvar_35;
            lowp float tmpvar_36;
            tmpvar_36 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_28 = tmpvar_36;
            highp float tmpvar_37;
            tmpvar_37 = (((
              (0.5 - c_28)
             - xlv_TEXCOORD5.x) * xlv_TEXCOORD5.y) + 0.5);
            highp float tmpvar_38;
            tmpvar_38 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD5.y);
            highp float tmpvar_39;
            tmpvar_39 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD5.y);
            faceColor_27 = _FaceColor;
            outlineColor_26 = _OutlineColor;
            faceColor_27 = (faceColor_27 * xlv_COLOR0);
            outlineColor_26.w = (outlineColor_26.w * xlv_COLOR0.w);
            highp vec2 tmpvar_40;
            tmpvar_40.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_40.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_41;
            tmpvar_41 = texture2D (_FaceTex, tmpvar_40);
            faceColor_27 = (faceColor_27 * tmpvar_41);
            highp vec2 tmpvar_42;
            tmpvar_42.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_42.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_43;
            tmpvar_43 = texture2D (_OutlineTex, tmpvar_42);
            outlineColor_26 = (outlineColor_26 * tmpvar_43);
            mediump float d_44;
            d_44 = tmpvar_37;
            lowp vec4 faceColor_45;
            faceColor_45 = faceColor_27;
            lowp vec4 outlineColor_46;
            outlineColor_46 = outlineColor_26;
            mediump float outline_47;
            outline_47 = tmpvar_38;
            mediump float softness_48;
            softness_48 = tmpvar_39;
            mediump float tmpvar_49;
            tmpvar_49 = (1.0 - clamp ((
              ((d_44 - (outline_47 * 0.5)) + (softness_48 * 0.5))
             / 
              (1.0 + softness_48)
            ), 0.0, 1.0));
            faceColor_45.xyz = (faceColor_45.xyz * faceColor_45.w);
            outlineColor_46.xyz = (outlineColor_46.xyz * outlineColor_46.w);
            mediump vec4 tmpvar_50;
            tmpvar_50 = mix (faceColor_45, outlineColor_46, vec4((clamp (
              (d_44 + (outline_47 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_47)
            ))));
            faceColor_45 = tmpvar_50;
            faceColor_45 = (faceColor_45 * tmpvar_49);
            faceColor_27 = faceColor_45;
            faceColor_27.xyz = (faceColor_27.xyz / max (faceColor_27.w, 0.0001));
            highp vec4 h_51;
            h_51 = smp4x_29;
            highp float tmpvar_52;
            tmpvar_52 = (_ShaderFlags / 2.0);
            highp float tmpvar_53;
            tmpvar_53 = (fract(abs(tmpvar_52)) * 2.0);
            highp float tmpvar_54;
            if ((tmpvar_52 >= 0.0)) {
              tmpvar_54 = tmpvar_53;
            } else {
              tmpvar_54 = -(tmpvar_53);
            };
            h_51 = (smp4x_29 + (xlv_TEXCOORD5.x + _BevelOffset));
            highp float tmpvar_55;
            tmpvar_55 = max (0.01, (_OutlineWidth + _BevelWidth));
            h_51 = (h_51 - 0.5);
            h_51 = (h_51 / tmpvar_55);
            highp vec4 tmpvar_56;
            tmpvar_56 = clamp ((h_51 + 0.5), 0.0, 1.0);
            h_51 = tmpvar_56;
            if (bool(float((tmpvar_54 >= 1.0)))) {
              h_51 = (1.0 - abs((
                (tmpvar_56 * 2.0)
               - 1.0)));
            };
            h_51 = (min (mix (h_51, 
              sin(((h_51 * 3.141592) / 2.0))
            , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((_Bevel * tmpvar_55) * (_GradientScale * -2.0)));
            highp vec3 tmpvar_57;
            tmpvar_57.xy = vec2(1.0, 0.0);
            tmpvar_57.z = (h_51.y - h_51.x);
            highp vec3 tmpvar_58;
            tmpvar_58 = normalize(tmpvar_57);
            highp vec3 tmpvar_59;
            tmpvar_59.xy = vec2(0.0, -1.0);
            tmpvar_59.z = (h_51.w - h_51.z);
            highp vec3 tmpvar_60;
            tmpvar_60 = normalize(tmpvar_59);
            lowp vec3 tmpvar_61;
            tmpvar_61 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
            bump_25 = tmpvar_61;
            bump_25 = (bump_25 * mix (_BumpFace, _BumpOutline, clamp (
              (tmpvar_37 + (tmpvar_38 * 0.5))
            , 0.0, 1.0)));
            highp vec3 tmpvar_62;
            tmpvar_62 = mix (vec3(0.0, 0.0, 1.0), bump_25, faceColor_27.www);
            bump_25 = tmpvar_62;
            highp vec3 tmpvar_63;
            tmpvar_63 = normalize(((
              (tmpvar_58.yzx * tmpvar_60.zxy)
             - 
              (tmpvar_58.zxy * tmpvar_60.yzx)
            ) - tmpvar_62));
            highp mat3 tmpvar_64;
            tmpvar_64[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_64[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_64[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_65;
            highp vec3 N_66;
            N_66 = (tmpvar_64 * tmpvar_63);
            tmpvar_65 = (xlv_TEXCOORD6 - (2.0 * (
              dot (N_66, xlv_TEXCOORD6)
             * N_66)));
            lowp vec4 tmpvar_67;
            tmpvar_67 = textureCube (_Cube, tmpvar_65);
            highp float tmpvar_68;
            tmpvar_68 = clamp ((tmpvar_37 + (tmpvar_38 * 0.5)), 0.0, 1.0);
            lowp vec3 tmpvar_69;
            tmpvar_69 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_68));
            highp vec3 tmpvar_70;
            tmpvar_70 = ((tmpvar_67.xyz * tmpvar_69) * faceColor_27.w);
            tmpvar_20 = faceColor_27.xyz;
            tmpvar_21 = -(tmpvar_63);
            tmpvar_22 = tmpvar_70;
            highp float tmpvar_71;
            tmpvar_71 = clamp ((tmpvar_37 + (tmpvar_38 * 0.5)), 0.0, 1.0);
            tmpvar_23 = 1.0;
            tmpvar_24 = faceColor_27.w;
            tmpvar_5 = tmpvar_20;
            tmpvar_7 = tmpvar_22;
            tmpvar_8 = tmpvar_23;
            tmpvar_9 = tmpvar_24;
            lowp float tmpvar_72;
            tmpvar_72 = dot (_unity_tbn_0_14, tmpvar_21);
            worldN_3.x = tmpvar_72;
            lowp float tmpvar_73;
            tmpvar_73 = dot (_unity_tbn_1_13, tmpvar_21);
            worldN_3.y = tmpvar_73;
            lowp float tmpvar_74;
            tmpvar_74 = dot (_unity_tbn_2_12, tmpvar_21);
            worldN_3.z = tmpvar_74;
            highp vec3 tmpvar_75;
            tmpvar_75 = normalize(worldN_3);
            worldN_3 = tmpvar_75;
            tmpvar_6 = tmpvar_75;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_11;
            mediump vec3 normalWorld_76;
            normalWorld_76 = tmpvar_6;
            mediump vec4 tmpvar_77;
            tmpvar_77.w = 1.0;
            tmpvar_77.xyz = normalWorld_76;
            mediump vec3 x_78;
            x_78.x = dot (unity_SHAr, tmpvar_77);
            x_78.y = dot (unity_SHAg, tmpvar_77);
            x_78.z = dot (unity_SHAb, tmpvar_77);
            mediump vec3 tmpvar_79;
            tmpvar_79 = max (((1.055 * 
              pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD7 + x_78)), vec3(0.4166667, 0.4166667, 0.4166667))
            ) - 0.055), vec3(0.0, 0.0, 0.0));
            mediump vec3 viewDir_80;
            viewDir_80 = worldViewDir_10;
            lowp vec4 c_81;
            lowp vec4 c_82;
            highp float nh_83;
            lowp float diff_84;
            mediump float tmpvar_85;
            tmpvar_85 = max (0.0, dot (tmpvar_6, tmpvar_2));
            diff_84 = tmpvar_85;
            mediump float tmpvar_86;
            tmpvar_86 = max (0.0, dot (tmpvar_6, normalize(
              (tmpvar_2 + viewDir_80)
            )));
            nh_83 = tmpvar_86;
            mediump float y_87;
            y_87 = (mix (_FaceShininess, _OutlineShininess, tmpvar_71) * 128.0);
            highp float tmpvar_88;
            tmpvar_88 = pow (nh_83, y_87);
            c_82.xyz = (((tmpvar_20 * tmpvar_1) * diff_84) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_88));
            c_82.w = tmpvar_24;
            c_81.w = c_82.w;
            c_81.xyz = (c_82.xyz + (tmpvar_20 * tmpvar_79));
            c_4.w = c_81.w;
            c_4.xyz = (c_81.xyz + tmpvar_22);
            gl_FragData[0] = c_4;
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
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform mediump vec4 _WorldSpaceLightPos0;
          uniform mediump vec4 unity_SHAr;
          uniform mediump vec4 unity_SHAg;
          uniform mediump vec4 unity_SHAb;
          uniform highp mat4 unity_ObjectToWorld;
          uniform lowp vec4 _LightColor0;
          uniform lowp vec4 _SpecColor;
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
          uniform highp float _Bevel;
          uniform highp float _BevelOffset;
          uniform highp float _BevelWidth;
          uniform highp float _BevelClamp;
          uniform highp float _BevelRoundness;
          uniform sampler2D _BumpMap;
          uniform highp float _BumpOutline;
          uniform highp float _BumpFace;
          uniform lowp samplerCube _Cube;
          uniform lowp vec4 _ReflectFaceColor;
          uniform lowp vec4 _ReflectOutlineColor;
          uniform highp float _ShaderFlags;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          uniform highp float _TextureWidth;
          uniform highp float _TextureHeight;
          uniform highp float _GradientScale;
          uniform mediump float _FaceShininess;
          uniform mediump float _OutlineShininess;
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
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp vec4 c_4;
            lowp vec3 tmpvar_5;
            lowp vec3 tmpvar_6;
            lowp vec3 tmpvar_7;
            lowp float tmpvar_8;
            lowp float tmpvar_9;
            highp vec3 worldViewDir_10;
            lowp vec3 lightDir_11;
            lowp vec3 _unity_tbn_2_12;
            lowp vec3 _unity_tbn_1_13;
            lowp vec3 _unity_tbn_0_14;
            highp vec3 tmpvar_15;
            tmpvar_15 = xlv_TEXCOORD2.xyz;
            _unity_tbn_0_14 = tmpvar_15;
            highp vec3 tmpvar_16;
            tmpvar_16 = xlv_TEXCOORD3.xyz;
            _unity_tbn_1_13 = tmpvar_16;
            highp vec3 tmpvar_17;
            tmpvar_17 = xlv_TEXCOORD4.xyz;
            _unity_tbn_2_12 = tmpvar_17;
            highp vec3 tmpvar_18;
            tmpvar_18.x = xlv_TEXCOORD2.w;
            tmpvar_18.y = xlv_TEXCOORD3.w;
            tmpvar_18.z = xlv_TEXCOORD4.w;
            mediump vec3 tmpvar_19;
            tmpvar_19 = _WorldSpaceLightPos0.xyz;
            lightDir_11 = tmpvar_19;
            worldViewDir_10 = normalize((_WorldSpaceCameraPos - tmpvar_18));
            tmpvar_5 = vec3(0.0, 0.0, 0.0);
            tmpvar_7 = vec3(0.0, 0.0, 0.0);
            tmpvar_9 = 0.0;
            tmpvar_8 = 0.0;
            tmpvar_6 = vec3(0.0, 0.0, 1.0);
            lowp vec3 tmpvar_20;
            lowp vec3 tmpvar_21;
            lowp vec3 tmpvar_22;
            lowp float tmpvar_23;
            lowp float tmpvar_24;
            tmpvar_20 = tmpvar_5;
            tmpvar_21 = tmpvar_6;
            tmpvar_22 = tmpvar_7;
            tmpvar_23 = tmpvar_8;
            tmpvar_24 = tmpvar_9;
            highp vec3 bump_25;
            highp vec4 outlineColor_26;
            highp vec4 faceColor_27;
            highp float c_28;
            highp vec4 smp4x_29;
            highp vec3 tmpvar_30;
            tmpvar_30.z = 0.0;
            tmpvar_30.x = (1.0/(_TextureWidth));
            tmpvar_30.y = (1.0/(_TextureHeight));
            highp vec2 P_31;
            P_31 = (xlv_TEXCOORD0.xy - tmpvar_30.xz);
            highp vec2 P_32;
            P_32 = (xlv_TEXCOORD0.xy + tmpvar_30.xz);
            highp vec2 P_33;
            P_33 = (xlv_TEXCOORD0.xy - tmpvar_30.zy);
            highp vec2 P_34;
            P_34 = (xlv_TEXCOORD0.xy + tmpvar_30.zy);
            lowp vec4 tmpvar_35;
            tmpvar_35.x = texture2D (_MainTex, P_31).w;
            tmpvar_35.y = texture2D (_MainTex, P_32).w;
            tmpvar_35.z = texture2D (_MainTex, P_33).w;
            tmpvar_35.w = texture2D (_MainTex, P_34).w;
            smp4x_29 = tmpvar_35;
            lowp float tmpvar_36;
            tmpvar_36 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_28 = tmpvar_36;
            highp float tmpvar_37;
            tmpvar_37 = (((
              (0.5 - c_28)
             - xlv_TEXCOORD5.x) * xlv_TEXCOORD5.y) + 0.5);
            highp float tmpvar_38;
            tmpvar_38 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD5.y);
            highp float tmpvar_39;
            tmpvar_39 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD5.y);
            faceColor_27 = _FaceColor;
            outlineColor_26 = _OutlineColor;
            faceColor_27 = (faceColor_27 * xlv_COLOR0);
            outlineColor_26.w = (outlineColor_26.w * xlv_COLOR0.w);
            highp vec2 tmpvar_40;
            tmpvar_40.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_40.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_41;
            tmpvar_41 = texture2D (_FaceTex, tmpvar_40);
            faceColor_27 = (faceColor_27 * tmpvar_41);
            highp vec2 tmpvar_42;
            tmpvar_42.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_42.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_43;
            tmpvar_43 = texture2D (_OutlineTex, tmpvar_42);
            outlineColor_26 = (outlineColor_26 * tmpvar_43);
            mediump float d_44;
            d_44 = tmpvar_37;
            lowp vec4 faceColor_45;
            faceColor_45 = faceColor_27;
            lowp vec4 outlineColor_46;
            outlineColor_46 = outlineColor_26;
            mediump float outline_47;
            outline_47 = tmpvar_38;
            mediump float softness_48;
            softness_48 = tmpvar_39;
            mediump float tmpvar_49;
            tmpvar_49 = (1.0 - clamp ((
              ((d_44 - (outline_47 * 0.5)) + (softness_48 * 0.5))
             / 
              (1.0 + softness_48)
            ), 0.0, 1.0));
            faceColor_45.xyz = (faceColor_45.xyz * faceColor_45.w);
            outlineColor_46.xyz = (outlineColor_46.xyz * outlineColor_46.w);
            mediump vec4 tmpvar_50;
            tmpvar_50 = mix (faceColor_45, outlineColor_46, vec4((clamp (
              (d_44 + (outline_47 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_47)
            ))));
            faceColor_45 = tmpvar_50;
            faceColor_45 = (faceColor_45 * tmpvar_49);
            faceColor_27 = faceColor_45;
            faceColor_27.xyz = (faceColor_27.xyz / max (faceColor_27.w, 0.0001));
            highp vec4 h_51;
            h_51 = smp4x_29;
            highp float tmpvar_52;
            tmpvar_52 = (_ShaderFlags / 2.0);
            highp float tmpvar_53;
            tmpvar_53 = (fract(abs(tmpvar_52)) * 2.0);
            highp float tmpvar_54;
            if ((tmpvar_52 >= 0.0)) {
              tmpvar_54 = tmpvar_53;
            } else {
              tmpvar_54 = -(tmpvar_53);
            };
            h_51 = (smp4x_29 + (xlv_TEXCOORD5.x + _BevelOffset));
            highp float tmpvar_55;
            tmpvar_55 = max (0.01, (_OutlineWidth + _BevelWidth));
            h_51 = (h_51 - 0.5);
            h_51 = (h_51 / tmpvar_55);
            highp vec4 tmpvar_56;
            tmpvar_56 = clamp ((h_51 + 0.5), 0.0, 1.0);
            h_51 = tmpvar_56;
            if (bool(float((tmpvar_54 >= 1.0)))) {
              h_51 = (1.0 - abs((
                (tmpvar_56 * 2.0)
               - 1.0)));
            };
            h_51 = (min (mix (h_51, 
              sin(((h_51 * 3.141592) / 2.0))
            , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((_Bevel * tmpvar_55) * (_GradientScale * -2.0)));
            highp vec3 tmpvar_57;
            tmpvar_57.xy = vec2(1.0, 0.0);
            tmpvar_57.z = (h_51.y - h_51.x);
            highp vec3 tmpvar_58;
            tmpvar_58 = normalize(tmpvar_57);
            highp vec3 tmpvar_59;
            tmpvar_59.xy = vec2(0.0, -1.0);
            tmpvar_59.z = (h_51.w - h_51.z);
            highp vec3 tmpvar_60;
            tmpvar_60 = normalize(tmpvar_59);
            lowp vec3 tmpvar_61;
            tmpvar_61 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
            bump_25 = tmpvar_61;
            bump_25 = (bump_25 * mix (_BumpFace, _BumpOutline, clamp (
              (tmpvar_37 + (tmpvar_38 * 0.5))
            , 0.0, 1.0)));
            highp vec3 tmpvar_62;
            tmpvar_62 = mix (vec3(0.0, 0.0, 1.0), bump_25, faceColor_27.www);
            bump_25 = tmpvar_62;
            highp vec3 tmpvar_63;
            tmpvar_63 = normalize(((
              (tmpvar_58.yzx * tmpvar_60.zxy)
             - 
              (tmpvar_58.zxy * tmpvar_60.yzx)
            ) - tmpvar_62));
            highp mat3 tmpvar_64;
            tmpvar_64[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_64[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_64[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_65;
            highp vec3 N_66;
            N_66 = (tmpvar_64 * tmpvar_63);
            tmpvar_65 = (xlv_TEXCOORD6 - (2.0 * (
              dot (N_66, xlv_TEXCOORD6)
             * N_66)));
            lowp vec4 tmpvar_67;
            tmpvar_67 = textureCube (_Cube, tmpvar_65);
            highp float tmpvar_68;
            tmpvar_68 = clamp ((tmpvar_37 + (tmpvar_38 * 0.5)), 0.0, 1.0);
            lowp vec3 tmpvar_69;
            tmpvar_69 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_68));
            highp vec3 tmpvar_70;
            tmpvar_70 = ((tmpvar_67.xyz * tmpvar_69) * faceColor_27.w);
            tmpvar_20 = faceColor_27.xyz;
            tmpvar_21 = -(tmpvar_63);
            tmpvar_22 = tmpvar_70;
            highp float tmpvar_71;
            tmpvar_71 = clamp ((tmpvar_37 + (tmpvar_38 * 0.5)), 0.0, 1.0);
            tmpvar_23 = 1.0;
            tmpvar_24 = faceColor_27.w;
            tmpvar_5 = tmpvar_20;
            tmpvar_7 = tmpvar_22;
            tmpvar_8 = tmpvar_23;
            tmpvar_9 = tmpvar_24;
            lowp float tmpvar_72;
            tmpvar_72 = dot (_unity_tbn_0_14, tmpvar_21);
            worldN_3.x = tmpvar_72;
            lowp float tmpvar_73;
            tmpvar_73 = dot (_unity_tbn_1_13, tmpvar_21);
            worldN_3.y = tmpvar_73;
            lowp float tmpvar_74;
            tmpvar_74 = dot (_unity_tbn_2_12, tmpvar_21);
            worldN_3.z = tmpvar_74;
            highp vec3 tmpvar_75;
            tmpvar_75 = normalize(worldN_3);
            worldN_3 = tmpvar_75;
            tmpvar_6 = tmpvar_75;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_11;
            mediump vec3 normalWorld_76;
            normalWorld_76 = tmpvar_6;
            mediump vec4 tmpvar_77;
            tmpvar_77.w = 1.0;
            tmpvar_77.xyz = normalWorld_76;
            mediump vec3 x_78;
            x_78.x = dot (unity_SHAr, tmpvar_77);
            x_78.y = dot (unity_SHAg, tmpvar_77);
            x_78.z = dot (unity_SHAb, tmpvar_77);
            mediump vec3 tmpvar_79;
            tmpvar_79 = max (((1.055 * 
              pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD7 + x_78)), vec3(0.4166667, 0.4166667, 0.4166667))
            ) - 0.055), vec3(0.0, 0.0, 0.0));
            mediump vec3 viewDir_80;
            viewDir_80 = worldViewDir_10;
            lowp vec4 c_81;
            lowp vec4 c_82;
            highp float nh_83;
            lowp float diff_84;
            mediump float tmpvar_85;
            tmpvar_85 = max (0.0, dot (tmpvar_6, tmpvar_2));
            diff_84 = tmpvar_85;
            mediump float tmpvar_86;
            tmpvar_86 = max (0.0, dot (tmpvar_6, normalize(
              (tmpvar_2 + viewDir_80)
            )));
            nh_83 = tmpvar_86;
            mediump float y_87;
            y_87 = (mix (_FaceShininess, _OutlineShininess, tmpvar_71) * 128.0);
            highp float tmpvar_88;
            tmpvar_88 = pow (nh_83, y_87);
            c_82.xyz = (((tmpvar_20 * tmpvar_1) * diff_84) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_88));
            c_82.w = tmpvar_24;
            c_81.w = c_82.w;
            c_81.xyz = (c_82.xyz + (tmpvar_20 * tmpvar_79));
            c_4.w = c_81.w;
            c_4.xyz = (c_81.xyz + tmpvar_22);
            gl_FragData[0] = c_4;
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
    Pass // ind: 2, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "FORWARDADD"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 300
      ZWrite Off
      Cull Off
      Blend SrcAlpha One
      ColorMask RGB
      GpuProgramID 100962
      // m_ProgramMask = 6
      !!! *******************************************************************************************
      !!! Allow restore shader as UnityLab format - only available for DevX GameRecovery license type
      !!! *******************************************************************************************
      Program "vp"
      {
        SubProgram "gles hw_tier00"
        {
           Keywords { "POINT" }
          
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
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp vec4 unity_WorldTransformParams;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp mat4 unity_WorldToLight;
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
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          varying highp vec3 xlv_TEXCOORD8;
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
            highp mat3 tmpvar_20;
            tmpvar_20[0] = unity_WorldToObject[0].xyz;
            tmpvar_20[1] = unity_WorldToObject[1].xyz;
            tmpvar_20[2] = unity_WorldToObject[2].xyz;
            highp vec3 tmpvar_21;
            tmpvar_21 = normalize((tmpvar_6 * tmpvar_20));
            highp mat3 tmpvar_22;
            tmpvar_22[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_22[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_22[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_23;
            tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.xyz));
            worldTangent_3 = tmpvar_23;
            highp float tmpvar_24;
            tmpvar_24 = (_glesTANGENT.w * unity_WorldTransformParams.w);
            tangentSign_2 = tmpvar_24;
            highp vec3 tmpvar_25;
            tmpvar_25 = (((tmpvar_21.yzx * worldTangent_3.zxy) - (tmpvar_21.zxy * worldTangent_3.yzx)) * tangentSign_2);
            worldBinormal_1 = tmpvar_25;
            highp vec3 tmpvar_26;
            tmpvar_26.x = worldTangent_3.x;
            tmpvar_26.y = worldBinormal_1.x;
            tmpvar_26.z = tmpvar_21.x;
            highp vec3 tmpvar_27;
            tmpvar_27.x = worldTangent_3.y;
            tmpvar_27.y = worldBinormal_1.y;
            tmpvar_27.z = tmpvar_21.y;
            highp vec3 tmpvar_28;
            tmpvar_28.x = worldTangent_3.z;
            tmpvar_28.y = worldBinormal_1.z;
            tmpvar_28.z = tmpvar_21.z;
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_19));
            xlv_TEXCOORD0 = tmpvar_4;
            xlv_TEXCOORD1 = ((tmpvar_7.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_26;
            xlv_TEXCOORD3 = tmpvar_27;
            xlv_TEXCOORD4 = tmpvar_28;
            xlv_TEXCOORD5 = (unity_ObjectToWorld * tmpvar_5).xyz;
            xlv_COLOR0 = _glesColor;
            xlv_TEXCOORD6 = tmpvar_8;
            xlv_TEXCOORD7 = (tmpvar_18 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz));
            xlv_TEXCOORD8 = (unity_WorldToLight * (unity_ObjectToWorld * tmpvar_5)).xyz;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _WorldSpaceLightPos0;
          uniform highp mat4 unity_ObjectToWorld;
          uniform lowp vec4 _LightColor0;
          uniform lowp vec4 _SpecColor;
          uniform highp sampler2D _LightTexture0;
          uniform highp mat4 unity_WorldToLight;
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
          uniform highp float _Bevel;
          uniform highp float _BevelOffset;
          uniform highp float _BevelWidth;
          uniform highp float _BevelClamp;
          uniform highp float _BevelRoundness;
          uniform sampler2D _BumpMap;
          uniform highp float _BumpOutline;
          uniform highp float _BumpFace;
          uniform lowp samplerCube _Cube;
          uniform lowp vec4 _ReflectFaceColor;
          uniform lowp vec4 _ReflectOutlineColor;
          uniform highp float _ShaderFlags;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          uniform highp float _TextureWidth;
          uniform highp float _TextureHeight;
          uniform highp float _GradientScale;
          uniform mediump float _FaceShininess;
          uniform mediump float _OutlineShininess;
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          void main ()
          {
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp float atten_4;
            highp vec3 lightCoord_5;
            lowp vec3 tmpvar_6;
            lowp vec3 tmpvar_7;
            lowp vec3 tmpvar_8;
            lowp float tmpvar_9;
            lowp float tmpvar_10;
            highp vec3 worldViewDir_11;
            lowp vec3 lightDir_12;
            lowp vec3 _unity_tbn_2_13;
            lowp vec3 _unity_tbn_1_14;
            lowp vec3 _unity_tbn_0_15;
            _unity_tbn_0_15 = xlv_TEXCOORD2;
            _unity_tbn_1_14 = xlv_TEXCOORD3;
            _unity_tbn_2_13 = xlv_TEXCOORD4;
            highp vec3 tmpvar_16;
            tmpvar_16 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD5));
            lightDir_12 = tmpvar_16;
            worldViewDir_11 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD5));
            tmpvar_6 = vec3(0.0, 0.0, 0.0);
            tmpvar_8 = vec3(0.0, 0.0, 0.0);
            tmpvar_10 = 0.0;
            tmpvar_9 = 0.0;
            tmpvar_7 = vec3(0.0, 0.0, 1.0);
            lowp vec3 tmpvar_17;
            lowp vec3 tmpvar_18;
            lowp vec3 tmpvar_19;
            lowp float tmpvar_20;
            lowp float tmpvar_21;
            tmpvar_17 = tmpvar_6;
            tmpvar_18 = tmpvar_7;
            tmpvar_19 = tmpvar_8;
            tmpvar_20 = tmpvar_9;
            tmpvar_21 = tmpvar_10;
            highp vec3 bump_22;
            highp vec4 outlineColor_23;
            highp vec4 faceColor_24;
            highp float c_25;
            highp vec4 smp4x_26;
            highp vec3 tmpvar_27;
            tmpvar_27.z = 0.0;
            tmpvar_27.x = (1.0/(_TextureWidth));
            tmpvar_27.y = (1.0/(_TextureHeight));
            highp vec2 P_28;
            P_28 = (xlv_TEXCOORD0.xy - tmpvar_27.xz);
            highp vec2 P_29;
            P_29 = (xlv_TEXCOORD0.xy + tmpvar_27.xz);
            highp vec2 P_30;
            P_30 = (xlv_TEXCOORD0.xy - tmpvar_27.zy);
            highp vec2 P_31;
            P_31 = (xlv_TEXCOORD0.xy + tmpvar_27.zy);
            lowp vec4 tmpvar_32;
            tmpvar_32.x = texture2D (_MainTex, P_28).w;
            tmpvar_32.y = texture2D (_MainTex, P_29).w;
            tmpvar_32.z = texture2D (_MainTex, P_30).w;
            tmpvar_32.w = texture2D (_MainTex, P_31).w;
            smp4x_26 = tmpvar_32;
            lowp float tmpvar_33;
            tmpvar_33 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_25 = tmpvar_33;
            highp float tmpvar_34;
            tmpvar_34 = (((
              (0.5 - c_25)
             - xlv_TEXCOORD6.x) * xlv_TEXCOORD6.y) + 0.5);
            highp float tmpvar_35;
            tmpvar_35 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD6.y);
            highp float tmpvar_36;
            tmpvar_36 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD6.y);
            faceColor_24 = _FaceColor;
            outlineColor_23 = _OutlineColor;
            faceColor_24 = (faceColor_24 * xlv_COLOR0);
            outlineColor_23.w = (outlineColor_23.w * xlv_COLOR0.w);
            highp vec2 tmpvar_37;
            tmpvar_37.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_37.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_38;
            tmpvar_38 = texture2D (_FaceTex, tmpvar_37);
            faceColor_24 = (faceColor_24 * tmpvar_38);
            highp vec2 tmpvar_39;
            tmpvar_39.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_39.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_40;
            tmpvar_40 = texture2D (_OutlineTex, tmpvar_39);
            outlineColor_23 = (outlineColor_23 * tmpvar_40);
            mediump float d_41;
            d_41 = tmpvar_34;
            lowp vec4 faceColor_42;
            faceColor_42 = faceColor_24;
            lowp vec4 outlineColor_43;
            outlineColor_43 = outlineColor_23;
            mediump float outline_44;
            outline_44 = tmpvar_35;
            mediump float softness_45;
            softness_45 = tmpvar_36;
            mediump float tmpvar_46;
            tmpvar_46 = (1.0 - clamp ((
              ((d_41 - (outline_44 * 0.5)) + (softness_45 * 0.5))
             / 
              (1.0 + softness_45)
            ), 0.0, 1.0));
            faceColor_42.xyz = (faceColor_42.xyz * faceColor_42.w);
            outlineColor_43.xyz = (outlineColor_43.xyz * outlineColor_43.w);
            mediump vec4 tmpvar_47;
            tmpvar_47 = mix (faceColor_42, outlineColor_43, vec4((clamp (
              (d_41 + (outline_44 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_44)
            ))));
            faceColor_42 = tmpvar_47;
            faceColor_42 = (faceColor_42 * tmpvar_46);
            faceColor_24 = faceColor_42;
            faceColor_24.xyz = (faceColor_24.xyz / max (faceColor_24.w, 0.0001));
            highp vec4 h_48;
            h_48 = smp4x_26;
            highp float tmpvar_49;
            tmpvar_49 = (_ShaderFlags / 2.0);
            highp float tmpvar_50;
            tmpvar_50 = (fract(abs(tmpvar_49)) * 2.0);
            highp float tmpvar_51;
            if ((tmpvar_49 >= 0.0)) {
              tmpvar_51 = tmpvar_50;
            } else {
              tmpvar_51 = -(tmpvar_50);
            };
            h_48 = (smp4x_26 + (xlv_TEXCOORD6.x + _BevelOffset));
            highp float tmpvar_52;
            tmpvar_52 = max (0.01, (_OutlineWidth + _BevelWidth));
            h_48 = (h_48 - 0.5);
            h_48 = (h_48 / tmpvar_52);
            highp vec4 tmpvar_53;
            tmpvar_53 = clamp ((h_48 + 0.5), 0.0, 1.0);
            h_48 = tmpvar_53;
            if (bool(float((tmpvar_51 >= 1.0)))) {
              h_48 = (1.0 - abs((
                (tmpvar_53 * 2.0)
               - 1.0)));
            };
            h_48 = (min (mix (h_48, 
              sin(((h_48 * 3.141592) / 2.0))
            , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((_Bevel * tmpvar_52) * (_GradientScale * -2.0)));
            highp vec3 tmpvar_54;
            tmpvar_54.xy = vec2(1.0, 0.0);
            tmpvar_54.z = (h_48.y - h_48.x);
            highp vec3 tmpvar_55;
            tmpvar_55 = normalize(tmpvar_54);
            highp vec3 tmpvar_56;
            tmpvar_56.xy = vec2(0.0, -1.0);
            tmpvar_56.z = (h_48.w - h_48.z);
            highp vec3 tmpvar_57;
            tmpvar_57 = normalize(tmpvar_56);
            lowp vec3 tmpvar_58;
            tmpvar_58 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
            bump_22 = tmpvar_58;
            bump_22 = (bump_22 * mix (_BumpFace, _BumpOutline, clamp (
              (tmpvar_34 + (tmpvar_35 * 0.5))
            , 0.0, 1.0)));
            highp vec3 tmpvar_59;
            tmpvar_59 = mix (vec3(0.0, 0.0, 1.0), bump_22, faceColor_24.www);
            bump_22 = tmpvar_59;
            highp vec3 tmpvar_60;
            tmpvar_60 = normalize(((
              (tmpvar_55.yzx * tmpvar_57.zxy)
             - 
              (tmpvar_55.zxy * tmpvar_57.yzx)
            ) - tmpvar_59));
            highp mat3 tmpvar_61;
            tmpvar_61[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_61[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_61[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_62;
            highp vec3 N_63;
            N_63 = (tmpvar_61 * tmpvar_60);
            tmpvar_62 = (xlv_TEXCOORD7 - (2.0 * (
              dot (N_63, xlv_TEXCOORD7)
             * N_63)));
            lowp vec4 tmpvar_64;
            tmpvar_64 = textureCube (_Cube, tmpvar_62);
            highp float tmpvar_65;
            tmpvar_65 = clamp ((tmpvar_34 + (tmpvar_35 * 0.5)), 0.0, 1.0);
            lowp vec3 tmpvar_66;
            tmpvar_66 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_65));
            highp vec3 tmpvar_67;
            tmpvar_67 = ((tmpvar_64.xyz * tmpvar_66) * faceColor_24.w);
            tmpvar_17 = faceColor_24.xyz;
            tmpvar_18 = -(tmpvar_60);
            tmpvar_19 = tmpvar_67;
            highp float tmpvar_68;
            tmpvar_68 = clamp ((tmpvar_34 + (tmpvar_35 * 0.5)), 0.0, 1.0);
            tmpvar_20 = 1.0;
            tmpvar_21 = faceColor_24.w;
            tmpvar_6 = tmpvar_17;
            tmpvar_8 = tmpvar_19;
            tmpvar_9 = tmpvar_20;
            tmpvar_10 = tmpvar_21;
            highp vec4 tmpvar_69;
            tmpvar_69.w = 1.0;
            tmpvar_69.xyz = xlv_TEXCOORD5;
            lightCoord_5 = (unity_WorldToLight * tmpvar_69).xyz;
            highp float tmpvar_70;
            tmpvar_70 = texture2D (_LightTexture0, vec2(dot (lightCoord_5, lightCoord_5))).x;
            atten_4 = tmpvar_70;
            lowp float tmpvar_71;
            tmpvar_71 = dot (_unity_tbn_0_15, tmpvar_18);
            worldN_3.x = tmpvar_71;
            lowp float tmpvar_72;
            tmpvar_72 = dot (_unity_tbn_1_14, tmpvar_18);
            worldN_3.y = tmpvar_72;
            lowp float tmpvar_73;
            tmpvar_73 = dot (_unity_tbn_2_13, tmpvar_18);
            worldN_3.z = tmpvar_73;
            highp vec3 tmpvar_74;
            tmpvar_74 = normalize(worldN_3);
            worldN_3 = tmpvar_74;
            tmpvar_7 = tmpvar_74;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_12;
            tmpvar_1 = (tmpvar_1 * atten_4);
            mediump vec3 viewDir_75;
            viewDir_75 = worldViewDir_11;
            lowp vec4 c_76;
            lowp vec4 c_77;
            highp float nh_78;
            lowp float diff_79;
            mediump float tmpvar_80;
            tmpvar_80 = max (0.0, dot (tmpvar_7, tmpvar_2));
            diff_79 = tmpvar_80;
            mediump float tmpvar_81;
            tmpvar_81 = max (0.0, dot (tmpvar_7, normalize(
              (tmpvar_2 + viewDir_75)
            )));
            nh_78 = tmpvar_81;
            mediump float y_82;
            y_82 = (mix (_FaceShininess, _OutlineShininess, tmpvar_68) * 128.0);
            highp float tmpvar_83;
            tmpvar_83 = pow (nh_78, y_82);
            c_77.xyz = (((tmpvar_17 * tmpvar_1) * diff_79) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_83));
            c_77.w = tmpvar_21;
            c_76.w = c_77.w;
            c_76.xyz = c_77.xyz;
            gl_FragData[0] = c_76;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier01"
        {
           Keywords { "POINT" }
          
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
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp vec4 unity_WorldTransformParams;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp mat4 unity_WorldToLight;
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
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          varying highp vec3 xlv_TEXCOORD8;
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
            highp mat3 tmpvar_20;
            tmpvar_20[0] = unity_WorldToObject[0].xyz;
            tmpvar_20[1] = unity_WorldToObject[1].xyz;
            tmpvar_20[2] = unity_WorldToObject[2].xyz;
            highp vec3 tmpvar_21;
            tmpvar_21 = normalize((tmpvar_6 * tmpvar_20));
            highp mat3 tmpvar_22;
            tmpvar_22[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_22[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_22[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_23;
            tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.xyz));
            worldTangent_3 = tmpvar_23;
            highp float tmpvar_24;
            tmpvar_24 = (_glesTANGENT.w * unity_WorldTransformParams.w);
            tangentSign_2 = tmpvar_24;
            highp vec3 tmpvar_25;
            tmpvar_25 = (((tmpvar_21.yzx * worldTangent_3.zxy) - (tmpvar_21.zxy * worldTangent_3.yzx)) * tangentSign_2);
            worldBinormal_1 = tmpvar_25;
            highp vec3 tmpvar_26;
            tmpvar_26.x = worldTangent_3.x;
            tmpvar_26.y = worldBinormal_1.x;
            tmpvar_26.z = tmpvar_21.x;
            highp vec3 tmpvar_27;
            tmpvar_27.x = worldTangent_3.y;
            tmpvar_27.y = worldBinormal_1.y;
            tmpvar_27.z = tmpvar_21.y;
            highp vec3 tmpvar_28;
            tmpvar_28.x = worldTangent_3.z;
            tmpvar_28.y = worldBinormal_1.z;
            tmpvar_28.z = tmpvar_21.z;
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_19));
            xlv_TEXCOORD0 = tmpvar_4;
            xlv_TEXCOORD1 = ((tmpvar_7.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_26;
            xlv_TEXCOORD3 = tmpvar_27;
            xlv_TEXCOORD4 = tmpvar_28;
            xlv_TEXCOORD5 = (unity_ObjectToWorld * tmpvar_5).xyz;
            xlv_COLOR0 = _glesColor;
            xlv_TEXCOORD6 = tmpvar_8;
            xlv_TEXCOORD7 = (tmpvar_18 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz));
            xlv_TEXCOORD8 = (unity_WorldToLight * (unity_ObjectToWorld * tmpvar_5)).xyz;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _WorldSpaceLightPos0;
          uniform highp mat4 unity_ObjectToWorld;
          uniform lowp vec4 _LightColor0;
          uniform lowp vec4 _SpecColor;
          uniform highp sampler2D _LightTexture0;
          uniform highp mat4 unity_WorldToLight;
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
          uniform highp float _Bevel;
          uniform highp float _BevelOffset;
          uniform highp float _BevelWidth;
          uniform highp float _BevelClamp;
          uniform highp float _BevelRoundness;
          uniform sampler2D _BumpMap;
          uniform highp float _BumpOutline;
          uniform highp float _BumpFace;
          uniform lowp samplerCube _Cube;
          uniform lowp vec4 _ReflectFaceColor;
          uniform lowp vec4 _ReflectOutlineColor;
          uniform highp float _ShaderFlags;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          uniform highp float _TextureWidth;
          uniform highp float _TextureHeight;
          uniform highp float _GradientScale;
          uniform mediump float _FaceShininess;
          uniform mediump float _OutlineShininess;
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          void main ()
          {
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp float atten_4;
            highp vec3 lightCoord_5;
            lowp vec3 tmpvar_6;
            lowp vec3 tmpvar_7;
            lowp vec3 tmpvar_8;
            lowp float tmpvar_9;
            lowp float tmpvar_10;
            highp vec3 worldViewDir_11;
            lowp vec3 lightDir_12;
            lowp vec3 _unity_tbn_2_13;
            lowp vec3 _unity_tbn_1_14;
            lowp vec3 _unity_tbn_0_15;
            _unity_tbn_0_15 = xlv_TEXCOORD2;
            _unity_tbn_1_14 = xlv_TEXCOORD3;
            _unity_tbn_2_13 = xlv_TEXCOORD4;
            highp vec3 tmpvar_16;
            tmpvar_16 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD5));
            lightDir_12 = tmpvar_16;
            worldViewDir_11 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD5));
            tmpvar_6 = vec3(0.0, 0.0, 0.0);
            tmpvar_8 = vec3(0.0, 0.0, 0.0);
            tmpvar_10 = 0.0;
            tmpvar_9 = 0.0;
            tmpvar_7 = vec3(0.0, 0.0, 1.0);
            lowp vec3 tmpvar_17;
            lowp vec3 tmpvar_18;
            lowp vec3 tmpvar_19;
            lowp float tmpvar_20;
            lowp float tmpvar_21;
            tmpvar_17 = tmpvar_6;
            tmpvar_18 = tmpvar_7;
            tmpvar_19 = tmpvar_8;
            tmpvar_20 = tmpvar_9;
            tmpvar_21 = tmpvar_10;
            highp vec3 bump_22;
            highp vec4 outlineColor_23;
            highp vec4 faceColor_24;
            highp float c_25;
            highp vec4 smp4x_26;
            highp vec3 tmpvar_27;
            tmpvar_27.z = 0.0;
            tmpvar_27.x = (1.0/(_TextureWidth));
            tmpvar_27.y = (1.0/(_TextureHeight));
            highp vec2 P_28;
            P_28 = (xlv_TEXCOORD0.xy - tmpvar_27.xz);
            highp vec2 P_29;
            P_29 = (xlv_TEXCOORD0.xy + tmpvar_27.xz);
            highp vec2 P_30;
            P_30 = (xlv_TEXCOORD0.xy - tmpvar_27.zy);
            highp vec2 P_31;
            P_31 = (xlv_TEXCOORD0.xy + tmpvar_27.zy);
            lowp vec4 tmpvar_32;
            tmpvar_32.x = texture2D (_MainTex, P_28).w;
            tmpvar_32.y = texture2D (_MainTex, P_29).w;
            tmpvar_32.z = texture2D (_MainTex, P_30).w;
            tmpvar_32.w = texture2D (_MainTex, P_31).w;
            smp4x_26 = tmpvar_32;
            lowp float tmpvar_33;
            tmpvar_33 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_25 = tmpvar_33;
            highp float tmpvar_34;
            tmpvar_34 = (((
              (0.5 - c_25)
             - xlv_TEXCOORD6.x) * xlv_TEXCOORD6.y) + 0.5);
            highp float tmpvar_35;
            tmpvar_35 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD6.y);
            highp float tmpvar_36;
            tmpvar_36 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD6.y);
            faceColor_24 = _FaceColor;
            outlineColor_23 = _OutlineColor;
            faceColor_24 = (faceColor_24 * xlv_COLOR0);
            outlineColor_23.w = (outlineColor_23.w * xlv_COLOR0.w);
            highp vec2 tmpvar_37;
            tmpvar_37.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_37.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_38;
            tmpvar_38 = texture2D (_FaceTex, tmpvar_37);
            faceColor_24 = (faceColor_24 * tmpvar_38);
            highp vec2 tmpvar_39;
            tmpvar_39.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_39.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_40;
            tmpvar_40 = texture2D (_OutlineTex, tmpvar_39);
            outlineColor_23 = (outlineColor_23 * tmpvar_40);
            mediump float d_41;
            d_41 = tmpvar_34;
            lowp vec4 faceColor_42;
            faceColor_42 = faceColor_24;
            lowp vec4 outlineColor_43;
            outlineColor_43 = outlineColor_23;
            mediump float outline_44;
            outline_44 = tmpvar_35;
            mediump float softness_45;
            softness_45 = tmpvar_36;
            mediump float tmpvar_46;
            tmpvar_46 = (1.0 - clamp ((
              ((d_41 - (outline_44 * 0.5)) + (softness_45 * 0.5))
             / 
              (1.0 + softness_45)
            ), 0.0, 1.0));
            faceColor_42.xyz = (faceColor_42.xyz * faceColor_42.w);
            outlineColor_43.xyz = (outlineColor_43.xyz * outlineColor_43.w);
            mediump vec4 tmpvar_47;
            tmpvar_47 = mix (faceColor_42, outlineColor_43, vec4((clamp (
              (d_41 + (outline_44 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_44)
            ))));
            faceColor_42 = tmpvar_47;
            faceColor_42 = (faceColor_42 * tmpvar_46);
            faceColor_24 = faceColor_42;
            faceColor_24.xyz = (faceColor_24.xyz / max (faceColor_24.w, 0.0001));
            highp vec4 h_48;
            h_48 = smp4x_26;
            highp float tmpvar_49;
            tmpvar_49 = (_ShaderFlags / 2.0);
            highp float tmpvar_50;
            tmpvar_50 = (fract(abs(tmpvar_49)) * 2.0);
            highp float tmpvar_51;
            if ((tmpvar_49 >= 0.0)) {
              tmpvar_51 = tmpvar_50;
            } else {
              tmpvar_51 = -(tmpvar_50);
            };
            h_48 = (smp4x_26 + (xlv_TEXCOORD6.x + _BevelOffset));
            highp float tmpvar_52;
            tmpvar_52 = max (0.01, (_OutlineWidth + _BevelWidth));
            h_48 = (h_48 - 0.5);
            h_48 = (h_48 / tmpvar_52);
            highp vec4 tmpvar_53;
            tmpvar_53 = clamp ((h_48 + 0.5), 0.0, 1.0);
            h_48 = tmpvar_53;
            if (bool(float((tmpvar_51 >= 1.0)))) {
              h_48 = (1.0 - abs((
                (tmpvar_53 * 2.0)
               - 1.0)));
            };
            h_48 = (min (mix (h_48, 
              sin(((h_48 * 3.141592) / 2.0))
            , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((_Bevel * tmpvar_52) * (_GradientScale * -2.0)));
            highp vec3 tmpvar_54;
            tmpvar_54.xy = vec2(1.0, 0.0);
            tmpvar_54.z = (h_48.y - h_48.x);
            highp vec3 tmpvar_55;
            tmpvar_55 = normalize(tmpvar_54);
            highp vec3 tmpvar_56;
            tmpvar_56.xy = vec2(0.0, -1.0);
            tmpvar_56.z = (h_48.w - h_48.z);
            highp vec3 tmpvar_57;
            tmpvar_57 = normalize(tmpvar_56);
            lowp vec3 tmpvar_58;
            tmpvar_58 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
            bump_22 = tmpvar_58;
            bump_22 = (bump_22 * mix (_BumpFace, _BumpOutline, clamp (
              (tmpvar_34 + (tmpvar_35 * 0.5))
            , 0.0, 1.0)));
            highp vec3 tmpvar_59;
            tmpvar_59 = mix (vec3(0.0, 0.0, 1.0), bump_22, faceColor_24.www);
            bump_22 = tmpvar_59;
            highp vec3 tmpvar_60;
            tmpvar_60 = normalize(((
              (tmpvar_55.yzx * tmpvar_57.zxy)
             - 
              (tmpvar_55.zxy * tmpvar_57.yzx)
            ) - tmpvar_59));
            highp mat3 tmpvar_61;
            tmpvar_61[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_61[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_61[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_62;
            highp vec3 N_63;
            N_63 = (tmpvar_61 * tmpvar_60);
            tmpvar_62 = (xlv_TEXCOORD7 - (2.0 * (
              dot (N_63, xlv_TEXCOORD7)
             * N_63)));
            lowp vec4 tmpvar_64;
            tmpvar_64 = textureCube (_Cube, tmpvar_62);
            highp float tmpvar_65;
            tmpvar_65 = clamp ((tmpvar_34 + (tmpvar_35 * 0.5)), 0.0, 1.0);
            lowp vec3 tmpvar_66;
            tmpvar_66 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_65));
            highp vec3 tmpvar_67;
            tmpvar_67 = ((tmpvar_64.xyz * tmpvar_66) * faceColor_24.w);
            tmpvar_17 = faceColor_24.xyz;
            tmpvar_18 = -(tmpvar_60);
            tmpvar_19 = tmpvar_67;
            highp float tmpvar_68;
            tmpvar_68 = clamp ((tmpvar_34 + (tmpvar_35 * 0.5)), 0.0, 1.0);
            tmpvar_20 = 1.0;
            tmpvar_21 = faceColor_24.w;
            tmpvar_6 = tmpvar_17;
            tmpvar_8 = tmpvar_19;
            tmpvar_9 = tmpvar_20;
            tmpvar_10 = tmpvar_21;
            highp vec4 tmpvar_69;
            tmpvar_69.w = 1.0;
            tmpvar_69.xyz = xlv_TEXCOORD5;
            lightCoord_5 = (unity_WorldToLight * tmpvar_69).xyz;
            highp float tmpvar_70;
            tmpvar_70 = texture2D (_LightTexture0, vec2(dot (lightCoord_5, lightCoord_5))).x;
            atten_4 = tmpvar_70;
            lowp float tmpvar_71;
            tmpvar_71 = dot (_unity_tbn_0_15, tmpvar_18);
            worldN_3.x = tmpvar_71;
            lowp float tmpvar_72;
            tmpvar_72 = dot (_unity_tbn_1_14, tmpvar_18);
            worldN_3.y = tmpvar_72;
            lowp float tmpvar_73;
            tmpvar_73 = dot (_unity_tbn_2_13, tmpvar_18);
            worldN_3.z = tmpvar_73;
            highp vec3 tmpvar_74;
            tmpvar_74 = normalize(worldN_3);
            worldN_3 = tmpvar_74;
            tmpvar_7 = tmpvar_74;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_12;
            tmpvar_1 = (tmpvar_1 * atten_4);
            mediump vec3 viewDir_75;
            viewDir_75 = worldViewDir_11;
            lowp vec4 c_76;
            lowp vec4 c_77;
            highp float nh_78;
            lowp float diff_79;
            mediump float tmpvar_80;
            tmpvar_80 = max (0.0, dot (tmpvar_7, tmpvar_2));
            diff_79 = tmpvar_80;
            mediump float tmpvar_81;
            tmpvar_81 = max (0.0, dot (tmpvar_7, normalize(
              (tmpvar_2 + viewDir_75)
            )));
            nh_78 = tmpvar_81;
            mediump float y_82;
            y_82 = (mix (_FaceShininess, _OutlineShininess, tmpvar_68) * 128.0);
            highp float tmpvar_83;
            tmpvar_83 = pow (nh_78, y_82);
            c_77.xyz = (((tmpvar_17 * tmpvar_1) * diff_79) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_83));
            c_77.w = tmpvar_21;
            c_76.w = c_77.w;
            c_76.xyz = c_77.xyz;
            gl_FragData[0] = c_76;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier02"
        {
           Keywords { "POINT" }
          
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
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp vec4 unity_WorldTransformParams;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp mat4 unity_WorldToLight;
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
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          varying highp vec3 xlv_TEXCOORD8;
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
            highp mat3 tmpvar_20;
            tmpvar_20[0] = unity_WorldToObject[0].xyz;
            tmpvar_20[1] = unity_WorldToObject[1].xyz;
            tmpvar_20[2] = unity_WorldToObject[2].xyz;
            highp vec3 tmpvar_21;
            tmpvar_21 = normalize((tmpvar_6 * tmpvar_20));
            highp mat3 tmpvar_22;
            tmpvar_22[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_22[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_22[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_23;
            tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.xyz));
            worldTangent_3 = tmpvar_23;
            highp float tmpvar_24;
            tmpvar_24 = (_glesTANGENT.w * unity_WorldTransformParams.w);
            tangentSign_2 = tmpvar_24;
            highp vec3 tmpvar_25;
            tmpvar_25 = (((tmpvar_21.yzx * worldTangent_3.zxy) - (tmpvar_21.zxy * worldTangent_3.yzx)) * tangentSign_2);
            worldBinormal_1 = tmpvar_25;
            highp vec3 tmpvar_26;
            tmpvar_26.x = worldTangent_3.x;
            tmpvar_26.y = worldBinormal_1.x;
            tmpvar_26.z = tmpvar_21.x;
            highp vec3 tmpvar_27;
            tmpvar_27.x = worldTangent_3.y;
            tmpvar_27.y = worldBinormal_1.y;
            tmpvar_27.z = tmpvar_21.y;
            highp vec3 tmpvar_28;
            tmpvar_28.x = worldTangent_3.z;
            tmpvar_28.y = worldBinormal_1.z;
            tmpvar_28.z = tmpvar_21.z;
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_19));
            xlv_TEXCOORD0 = tmpvar_4;
            xlv_TEXCOORD1 = ((tmpvar_7.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_26;
            xlv_TEXCOORD3 = tmpvar_27;
            xlv_TEXCOORD4 = tmpvar_28;
            xlv_TEXCOORD5 = (unity_ObjectToWorld * tmpvar_5).xyz;
            xlv_COLOR0 = _glesColor;
            xlv_TEXCOORD6 = tmpvar_8;
            xlv_TEXCOORD7 = (tmpvar_18 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz));
            xlv_TEXCOORD8 = (unity_WorldToLight * (unity_ObjectToWorld * tmpvar_5)).xyz;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _WorldSpaceLightPos0;
          uniform highp mat4 unity_ObjectToWorld;
          uniform lowp vec4 _LightColor0;
          uniform lowp vec4 _SpecColor;
          uniform highp sampler2D _LightTexture0;
          uniform highp mat4 unity_WorldToLight;
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
          uniform highp float _Bevel;
          uniform highp float _BevelOffset;
          uniform highp float _BevelWidth;
          uniform highp float _BevelClamp;
          uniform highp float _BevelRoundness;
          uniform sampler2D _BumpMap;
          uniform highp float _BumpOutline;
          uniform highp float _BumpFace;
          uniform lowp samplerCube _Cube;
          uniform lowp vec4 _ReflectFaceColor;
          uniform lowp vec4 _ReflectOutlineColor;
          uniform highp float _ShaderFlags;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          uniform highp float _TextureWidth;
          uniform highp float _TextureHeight;
          uniform highp float _GradientScale;
          uniform mediump float _FaceShininess;
          uniform mediump float _OutlineShininess;
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          void main ()
          {
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp float atten_4;
            highp vec3 lightCoord_5;
            lowp vec3 tmpvar_6;
            lowp vec3 tmpvar_7;
            lowp vec3 tmpvar_8;
            lowp float tmpvar_9;
            lowp float tmpvar_10;
            highp vec3 worldViewDir_11;
            lowp vec3 lightDir_12;
            lowp vec3 _unity_tbn_2_13;
            lowp vec3 _unity_tbn_1_14;
            lowp vec3 _unity_tbn_0_15;
            _unity_tbn_0_15 = xlv_TEXCOORD2;
            _unity_tbn_1_14 = xlv_TEXCOORD3;
            _unity_tbn_2_13 = xlv_TEXCOORD4;
            highp vec3 tmpvar_16;
            tmpvar_16 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD5));
            lightDir_12 = tmpvar_16;
            worldViewDir_11 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD5));
            tmpvar_6 = vec3(0.0, 0.0, 0.0);
            tmpvar_8 = vec3(0.0, 0.0, 0.0);
            tmpvar_10 = 0.0;
            tmpvar_9 = 0.0;
            tmpvar_7 = vec3(0.0, 0.0, 1.0);
            lowp vec3 tmpvar_17;
            lowp vec3 tmpvar_18;
            lowp vec3 tmpvar_19;
            lowp float tmpvar_20;
            lowp float tmpvar_21;
            tmpvar_17 = tmpvar_6;
            tmpvar_18 = tmpvar_7;
            tmpvar_19 = tmpvar_8;
            tmpvar_20 = tmpvar_9;
            tmpvar_21 = tmpvar_10;
            highp vec3 bump_22;
            highp vec4 outlineColor_23;
            highp vec4 faceColor_24;
            highp float c_25;
            highp vec4 smp4x_26;
            highp vec3 tmpvar_27;
            tmpvar_27.z = 0.0;
            tmpvar_27.x = (1.0/(_TextureWidth));
            tmpvar_27.y = (1.0/(_TextureHeight));
            highp vec2 P_28;
            P_28 = (xlv_TEXCOORD0.xy - tmpvar_27.xz);
            highp vec2 P_29;
            P_29 = (xlv_TEXCOORD0.xy + tmpvar_27.xz);
            highp vec2 P_30;
            P_30 = (xlv_TEXCOORD0.xy - tmpvar_27.zy);
            highp vec2 P_31;
            P_31 = (xlv_TEXCOORD0.xy + tmpvar_27.zy);
            lowp vec4 tmpvar_32;
            tmpvar_32.x = texture2D (_MainTex, P_28).w;
            tmpvar_32.y = texture2D (_MainTex, P_29).w;
            tmpvar_32.z = texture2D (_MainTex, P_30).w;
            tmpvar_32.w = texture2D (_MainTex, P_31).w;
            smp4x_26 = tmpvar_32;
            lowp float tmpvar_33;
            tmpvar_33 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_25 = tmpvar_33;
            highp float tmpvar_34;
            tmpvar_34 = (((
              (0.5 - c_25)
             - xlv_TEXCOORD6.x) * xlv_TEXCOORD6.y) + 0.5);
            highp float tmpvar_35;
            tmpvar_35 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD6.y);
            highp float tmpvar_36;
            tmpvar_36 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD6.y);
            faceColor_24 = _FaceColor;
            outlineColor_23 = _OutlineColor;
            faceColor_24 = (faceColor_24 * xlv_COLOR0);
            outlineColor_23.w = (outlineColor_23.w * xlv_COLOR0.w);
            highp vec2 tmpvar_37;
            tmpvar_37.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_37.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_38;
            tmpvar_38 = texture2D (_FaceTex, tmpvar_37);
            faceColor_24 = (faceColor_24 * tmpvar_38);
            highp vec2 tmpvar_39;
            tmpvar_39.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_39.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_40;
            tmpvar_40 = texture2D (_OutlineTex, tmpvar_39);
            outlineColor_23 = (outlineColor_23 * tmpvar_40);
            mediump float d_41;
            d_41 = tmpvar_34;
            lowp vec4 faceColor_42;
            faceColor_42 = faceColor_24;
            lowp vec4 outlineColor_43;
            outlineColor_43 = outlineColor_23;
            mediump float outline_44;
            outline_44 = tmpvar_35;
            mediump float softness_45;
            softness_45 = tmpvar_36;
            mediump float tmpvar_46;
            tmpvar_46 = (1.0 - clamp ((
              ((d_41 - (outline_44 * 0.5)) + (softness_45 * 0.5))
             / 
              (1.0 + softness_45)
            ), 0.0, 1.0));
            faceColor_42.xyz = (faceColor_42.xyz * faceColor_42.w);
            outlineColor_43.xyz = (outlineColor_43.xyz * outlineColor_43.w);
            mediump vec4 tmpvar_47;
            tmpvar_47 = mix (faceColor_42, outlineColor_43, vec4((clamp (
              (d_41 + (outline_44 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_44)
            ))));
            faceColor_42 = tmpvar_47;
            faceColor_42 = (faceColor_42 * tmpvar_46);
            faceColor_24 = faceColor_42;
            faceColor_24.xyz = (faceColor_24.xyz / max (faceColor_24.w, 0.0001));
            highp vec4 h_48;
            h_48 = smp4x_26;
            highp float tmpvar_49;
            tmpvar_49 = (_ShaderFlags / 2.0);
            highp float tmpvar_50;
            tmpvar_50 = (fract(abs(tmpvar_49)) * 2.0);
            highp float tmpvar_51;
            if ((tmpvar_49 >= 0.0)) {
              tmpvar_51 = tmpvar_50;
            } else {
              tmpvar_51 = -(tmpvar_50);
            };
            h_48 = (smp4x_26 + (xlv_TEXCOORD6.x + _BevelOffset));
            highp float tmpvar_52;
            tmpvar_52 = max (0.01, (_OutlineWidth + _BevelWidth));
            h_48 = (h_48 - 0.5);
            h_48 = (h_48 / tmpvar_52);
            highp vec4 tmpvar_53;
            tmpvar_53 = clamp ((h_48 + 0.5), 0.0, 1.0);
            h_48 = tmpvar_53;
            if (bool(float((tmpvar_51 >= 1.0)))) {
              h_48 = (1.0 - abs((
                (tmpvar_53 * 2.0)
               - 1.0)));
            };
            h_48 = (min (mix (h_48, 
              sin(((h_48 * 3.141592) / 2.0))
            , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((_Bevel * tmpvar_52) * (_GradientScale * -2.0)));
            highp vec3 tmpvar_54;
            tmpvar_54.xy = vec2(1.0, 0.0);
            tmpvar_54.z = (h_48.y - h_48.x);
            highp vec3 tmpvar_55;
            tmpvar_55 = normalize(tmpvar_54);
            highp vec3 tmpvar_56;
            tmpvar_56.xy = vec2(0.0, -1.0);
            tmpvar_56.z = (h_48.w - h_48.z);
            highp vec3 tmpvar_57;
            tmpvar_57 = normalize(tmpvar_56);
            lowp vec3 tmpvar_58;
            tmpvar_58 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
            bump_22 = tmpvar_58;
            bump_22 = (bump_22 * mix (_BumpFace, _BumpOutline, clamp (
              (tmpvar_34 + (tmpvar_35 * 0.5))
            , 0.0, 1.0)));
            highp vec3 tmpvar_59;
            tmpvar_59 = mix (vec3(0.0, 0.0, 1.0), bump_22, faceColor_24.www);
            bump_22 = tmpvar_59;
            highp vec3 tmpvar_60;
            tmpvar_60 = normalize(((
              (tmpvar_55.yzx * tmpvar_57.zxy)
             - 
              (tmpvar_55.zxy * tmpvar_57.yzx)
            ) - tmpvar_59));
            highp mat3 tmpvar_61;
            tmpvar_61[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_61[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_61[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_62;
            highp vec3 N_63;
            N_63 = (tmpvar_61 * tmpvar_60);
            tmpvar_62 = (xlv_TEXCOORD7 - (2.0 * (
              dot (N_63, xlv_TEXCOORD7)
             * N_63)));
            lowp vec4 tmpvar_64;
            tmpvar_64 = textureCube (_Cube, tmpvar_62);
            highp float tmpvar_65;
            tmpvar_65 = clamp ((tmpvar_34 + (tmpvar_35 * 0.5)), 0.0, 1.0);
            lowp vec3 tmpvar_66;
            tmpvar_66 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_65));
            highp vec3 tmpvar_67;
            tmpvar_67 = ((tmpvar_64.xyz * tmpvar_66) * faceColor_24.w);
            tmpvar_17 = faceColor_24.xyz;
            tmpvar_18 = -(tmpvar_60);
            tmpvar_19 = tmpvar_67;
            highp float tmpvar_68;
            tmpvar_68 = clamp ((tmpvar_34 + (tmpvar_35 * 0.5)), 0.0, 1.0);
            tmpvar_20 = 1.0;
            tmpvar_21 = faceColor_24.w;
            tmpvar_6 = tmpvar_17;
            tmpvar_8 = tmpvar_19;
            tmpvar_9 = tmpvar_20;
            tmpvar_10 = tmpvar_21;
            highp vec4 tmpvar_69;
            tmpvar_69.w = 1.0;
            tmpvar_69.xyz = xlv_TEXCOORD5;
            lightCoord_5 = (unity_WorldToLight * tmpvar_69).xyz;
            highp float tmpvar_70;
            tmpvar_70 = texture2D (_LightTexture0, vec2(dot (lightCoord_5, lightCoord_5))).x;
            atten_4 = tmpvar_70;
            lowp float tmpvar_71;
            tmpvar_71 = dot (_unity_tbn_0_15, tmpvar_18);
            worldN_3.x = tmpvar_71;
            lowp float tmpvar_72;
            tmpvar_72 = dot (_unity_tbn_1_14, tmpvar_18);
            worldN_3.y = tmpvar_72;
            lowp float tmpvar_73;
            tmpvar_73 = dot (_unity_tbn_2_13, tmpvar_18);
            worldN_3.z = tmpvar_73;
            highp vec3 tmpvar_74;
            tmpvar_74 = normalize(worldN_3);
            worldN_3 = tmpvar_74;
            tmpvar_7 = tmpvar_74;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_12;
            tmpvar_1 = (tmpvar_1 * atten_4);
            mediump vec3 viewDir_75;
            viewDir_75 = worldViewDir_11;
            lowp vec4 c_76;
            lowp vec4 c_77;
            highp float nh_78;
            lowp float diff_79;
            mediump float tmpvar_80;
            tmpvar_80 = max (0.0, dot (tmpvar_7, tmpvar_2));
            diff_79 = tmpvar_80;
            mediump float tmpvar_81;
            tmpvar_81 = max (0.0, dot (tmpvar_7, normalize(
              (tmpvar_2 + viewDir_75)
            )));
            nh_78 = tmpvar_81;
            mediump float y_82;
            y_82 = (mix (_FaceShininess, _OutlineShininess, tmpvar_68) * 128.0);
            highp float tmpvar_83;
            tmpvar_83 = pow (nh_78, y_82);
            c_77.xyz = (((tmpvar_17 * tmpvar_1) * diff_79) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_83));
            c_77.w = tmpvar_21;
            c_76.w = c_77.w;
            c_76.xyz = c_77.xyz;
            gl_FragData[0] = c_76;
          }
          
          
          #endif
          
          "
        }
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
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
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
            highp mat3 tmpvar_20;
            tmpvar_20[0] = unity_WorldToObject[0].xyz;
            tmpvar_20[1] = unity_WorldToObject[1].xyz;
            tmpvar_20[2] = unity_WorldToObject[2].xyz;
            highp vec3 tmpvar_21;
            tmpvar_21 = normalize((tmpvar_6 * tmpvar_20));
            highp mat3 tmpvar_22;
            tmpvar_22[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_22[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_22[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_23;
            tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.xyz));
            worldTangent_3 = tmpvar_23;
            highp float tmpvar_24;
            tmpvar_24 = (_glesTANGENT.w * unity_WorldTransformParams.w);
            tangentSign_2 = tmpvar_24;
            highp vec3 tmpvar_25;
            tmpvar_25 = (((tmpvar_21.yzx * worldTangent_3.zxy) - (tmpvar_21.zxy * worldTangent_3.yzx)) * tangentSign_2);
            worldBinormal_1 = tmpvar_25;
            highp vec3 tmpvar_26;
            tmpvar_26.x = worldTangent_3.x;
            tmpvar_26.y = worldBinormal_1.x;
            tmpvar_26.z = tmpvar_21.x;
            highp vec3 tmpvar_27;
            tmpvar_27.x = worldTangent_3.y;
            tmpvar_27.y = worldBinormal_1.y;
            tmpvar_27.z = tmpvar_21.y;
            highp vec3 tmpvar_28;
            tmpvar_28.x = worldTangent_3.z;
            tmpvar_28.y = worldBinormal_1.z;
            tmpvar_28.z = tmpvar_21.z;
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_19));
            xlv_TEXCOORD0 = tmpvar_4;
            xlv_TEXCOORD1 = ((tmpvar_7.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_26;
            xlv_TEXCOORD3 = tmpvar_27;
            xlv_TEXCOORD4 = tmpvar_28;
            xlv_TEXCOORD5 = (unity_ObjectToWorld * tmpvar_5).xyz;
            xlv_COLOR0 = _glesColor;
            xlv_TEXCOORD6 = tmpvar_8;
            xlv_TEXCOORD7 = (tmpvar_18 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz));
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform mediump vec4 _WorldSpaceLightPos0;
          uniform highp mat4 unity_ObjectToWorld;
          uniform lowp vec4 _LightColor0;
          uniform lowp vec4 _SpecColor;
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
          uniform highp float _Bevel;
          uniform highp float _BevelOffset;
          uniform highp float _BevelWidth;
          uniform highp float _BevelClamp;
          uniform highp float _BevelRoundness;
          uniform sampler2D _BumpMap;
          uniform highp float _BumpOutline;
          uniform highp float _BumpFace;
          uniform lowp samplerCube _Cube;
          uniform lowp vec4 _ReflectFaceColor;
          uniform lowp vec4 _ReflectOutlineColor;
          uniform highp float _ShaderFlags;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          uniform highp float _TextureWidth;
          uniform highp float _TextureHeight;
          uniform highp float _GradientScale;
          uniform mediump float _FaceShininess;
          uniform mediump float _OutlineShininess;
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          void main ()
          {
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp vec3 tmpvar_4;
            lowp vec3 tmpvar_5;
            lowp vec3 tmpvar_6;
            lowp float tmpvar_7;
            lowp float tmpvar_8;
            highp vec3 worldViewDir_9;
            lowp vec3 lightDir_10;
            lowp vec3 _unity_tbn_2_11;
            lowp vec3 _unity_tbn_1_12;
            lowp vec3 _unity_tbn_0_13;
            _unity_tbn_0_13 = xlv_TEXCOORD2;
            _unity_tbn_1_12 = xlv_TEXCOORD3;
            _unity_tbn_2_11 = xlv_TEXCOORD4;
            mediump vec3 tmpvar_14;
            tmpvar_14 = _WorldSpaceLightPos0.xyz;
            lightDir_10 = tmpvar_14;
            worldViewDir_9 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD5));
            tmpvar_4 = vec3(0.0, 0.0, 0.0);
            tmpvar_6 = vec3(0.0, 0.0, 0.0);
            tmpvar_8 = 0.0;
            tmpvar_7 = 0.0;
            tmpvar_5 = vec3(0.0, 0.0, 1.0);
            lowp vec3 tmpvar_15;
            lowp vec3 tmpvar_16;
            lowp vec3 tmpvar_17;
            lowp float tmpvar_18;
            lowp float tmpvar_19;
            tmpvar_15 = tmpvar_4;
            tmpvar_16 = tmpvar_5;
            tmpvar_17 = tmpvar_6;
            tmpvar_18 = tmpvar_7;
            tmpvar_19 = tmpvar_8;
            highp vec3 bump_20;
            highp vec4 outlineColor_21;
            highp vec4 faceColor_22;
            highp float c_23;
            highp vec4 smp4x_24;
            highp vec3 tmpvar_25;
            tmpvar_25.z = 0.0;
            tmpvar_25.x = (1.0/(_TextureWidth));
            tmpvar_25.y = (1.0/(_TextureHeight));
            highp vec2 P_26;
            P_26 = (xlv_TEXCOORD0.xy - tmpvar_25.xz);
            highp vec2 P_27;
            P_27 = (xlv_TEXCOORD0.xy + tmpvar_25.xz);
            highp vec2 P_28;
            P_28 = (xlv_TEXCOORD0.xy - tmpvar_25.zy);
            highp vec2 P_29;
            P_29 = (xlv_TEXCOORD0.xy + tmpvar_25.zy);
            lowp vec4 tmpvar_30;
            tmpvar_30.x = texture2D (_MainTex, P_26).w;
            tmpvar_30.y = texture2D (_MainTex, P_27).w;
            tmpvar_30.z = texture2D (_MainTex, P_28).w;
            tmpvar_30.w = texture2D (_MainTex, P_29).w;
            smp4x_24 = tmpvar_30;
            lowp float tmpvar_31;
            tmpvar_31 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_23 = tmpvar_31;
            highp float tmpvar_32;
            tmpvar_32 = (((
              (0.5 - c_23)
             - xlv_TEXCOORD6.x) * xlv_TEXCOORD6.y) + 0.5);
            highp float tmpvar_33;
            tmpvar_33 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD6.y);
            highp float tmpvar_34;
            tmpvar_34 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD6.y);
            faceColor_22 = _FaceColor;
            outlineColor_21 = _OutlineColor;
            faceColor_22 = (faceColor_22 * xlv_COLOR0);
            outlineColor_21.w = (outlineColor_21.w * xlv_COLOR0.w);
            highp vec2 tmpvar_35;
            tmpvar_35.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_35.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_36;
            tmpvar_36 = texture2D (_FaceTex, tmpvar_35);
            faceColor_22 = (faceColor_22 * tmpvar_36);
            highp vec2 tmpvar_37;
            tmpvar_37.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_37.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_38;
            tmpvar_38 = texture2D (_OutlineTex, tmpvar_37);
            outlineColor_21 = (outlineColor_21 * tmpvar_38);
            mediump float d_39;
            d_39 = tmpvar_32;
            lowp vec4 faceColor_40;
            faceColor_40 = faceColor_22;
            lowp vec4 outlineColor_41;
            outlineColor_41 = outlineColor_21;
            mediump float outline_42;
            outline_42 = tmpvar_33;
            mediump float softness_43;
            softness_43 = tmpvar_34;
            mediump float tmpvar_44;
            tmpvar_44 = (1.0 - clamp ((
              ((d_39 - (outline_42 * 0.5)) + (softness_43 * 0.5))
             / 
              (1.0 + softness_43)
            ), 0.0, 1.0));
            faceColor_40.xyz = (faceColor_40.xyz * faceColor_40.w);
            outlineColor_41.xyz = (outlineColor_41.xyz * outlineColor_41.w);
            mediump vec4 tmpvar_45;
            tmpvar_45 = mix (faceColor_40, outlineColor_41, vec4((clamp (
              (d_39 + (outline_42 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_42)
            ))));
            faceColor_40 = tmpvar_45;
            faceColor_40 = (faceColor_40 * tmpvar_44);
            faceColor_22 = faceColor_40;
            faceColor_22.xyz = (faceColor_22.xyz / max (faceColor_22.w, 0.0001));
            highp vec4 h_46;
            h_46 = smp4x_24;
            highp float tmpvar_47;
            tmpvar_47 = (_ShaderFlags / 2.0);
            highp float tmpvar_48;
            tmpvar_48 = (fract(abs(tmpvar_47)) * 2.0);
            highp float tmpvar_49;
            if ((tmpvar_47 >= 0.0)) {
              tmpvar_49 = tmpvar_48;
            } else {
              tmpvar_49 = -(tmpvar_48);
            };
            h_46 = (smp4x_24 + (xlv_TEXCOORD6.x + _BevelOffset));
            highp float tmpvar_50;
            tmpvar_50 = max (0.01, (_OutlineWidth + _BevelWidth));
            h_46 = (h_46 - 0.5);
            h_46 = (h_46 / tmpvar_50);
            highp vec4 tmpvar_51;
            tmpvar_51 = clamp ((h_46 + 0.5), 0.0, 1.0);
            h_46 = tmpvar_51;
            if (bool(float((tmpvar_49 >= 1.0)))) {
              h_46 = (1.0 - abs((
                (tmpvar_51 * 2.0)
               - 1.0)));
            };
            h_46 = (min (mix (h_46, 
              sin(((h_46 * 3.141592) / 2.0))
            , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((_Bevel * tmpvar_50) * (_GradientScale * -2.0)));
            highp vec3 tmpvar_52;
            tmpvar_52.xy = vec2(1.0, 0.0);
            tmpvar_52.z = (h_46.y - h_46.x);
            highp vec3 tmpvar_53;
            tmpvar_53 = normalize(tmpvar_52);
            highp vec3 tmpvar_54;
            tmpvar_54.xy = vec2(0.0, -1.0);
            tmpvar_54.z = (h_46.w - h_46.z);
            highp vec3 tmpvar_55;
            tmpvar_55 = normalize(tmpvar_54);
            lowp vec3 tmpvar_56;
            tmpvar_56 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
            bump_20 = tmpvar_56;
            bump_20 = (bump_20 * mix (_BumpFace, _BumpOutline, clamp (
              (tmpvar_32 + (tmpvar_33 * 0.5))
            , 0.0, 1.0)));
            highp vec3 tmpvar_57;
            tmpvar_57 = mix (vec3(0.0, 0.0, 1.0), bump_20, faceColor_22.www);
            bump_20 = tmpvar_57;
            highp vec3 tmpvar_58;
            tmpvar_58 = normalize(((
              (tmpvar_53.yzx * tmpvar_55.zxy)
             - 
              (tmpvar_53.zxy * tmpvar_55.yzx)
            ) - tmpvar_57));
            highp mat3 tmpvar_59;
            tmpvar_59[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_59[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_59[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_60;
            highp vec3 N_61;
            N_61 = (tmpvar_59 * tmpvar_58);
            tmpvar_60 = (xlv_TEXCOORD7 - (2.0 * (
              dot (N_61, xlv_TEXCOORD7)
             * N_61)));
            lowp vec4 tmpvar_62;
            tmpvar_62 = textureCube (_Cube, tmpvar_60);
            highp float tmpvar_63;
            tmpvar_63 = clamp ((tmpvar_32 + (tmpvar_33 * 0.5)), 0.0, 1.0);
            lowp vec3 tmpvar_64;
            tmpvar_64 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_63));
            highp vec3 tmpvar_65;
            tmpvar_65 = ((tmpvar_62.xyz * tmpvar_64) * faceColor_22.w);
            tmpvar_15 = faceColor_22.xyz;
            tmpvar_16 = -(tmpvar_58);
            tmpvar_17 = tmpvar_65;
            highp float tmpvar_66;
            tmpvar_66 = clamp ((tmpvar_32 + (tmpvar_33 * 0.5)), 0.0, 1.0);
            tmpvar_18 = 1.0;
            tmpvar_19 = faceColor_22.w;
            tmpvar_4 = tmpvar_15;
            tmpvar_6 = tmpvar_17;
            tmpvar_7 = tmpvar_18;
            tmpvar_8 = tmpvar_19;
            lowp float tmpvar_67;
            tmpvar_67 = dot (_unity_tbn_0_13, tmpvar_16);
            worldN_3.x = tmpvar_67;
            lowp float tmpvar_68;
            tmpvar_68 = dot (_unity_tbn_1_12, tmpvar_16);
            worldN_3.y = tmpvar_68;
            lowp float tmpvar_69;
            tmpvar_69 = dot (_unity_tbn_2_11, tmpvar_16);
            worldN_3.z = tmpvar_69;
            highp vec3 tmpvar_70;
            tmpvar_70 = normalize(worldN_3);
            worldN_3 = tmpvar_70;
            tmpvar_5 = tmpvar_70;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_10;
            mediump vec3 viewDir_71;
            viewDir_71 = worldViewDir_9;
            lowp vec4 c_72;
            lowp vec4 c_73;
            highp float nh_74;
            lowp float diff_75;
            mediump float tmpvar_76;
            tmpvar_76 = max (0.0, dot (tmpvar_5, tmpvar_2));
            diff_75 = tmpvar_76;
            mediump float tmpvar_77;
            tmpvar_77 = max (0.0, dot (tmpvar_5, normalize(
              (tmpvar_2 + viewDir_71)
            )));
            nh_74 = tmpvar_77;
            mediump float y_78;
            y_78 = (mix (_FaceShininess, _OutlineShininess, tmpvar_66) * 128.0);
            highp float tmpvar_79;
            tmpvar_79 = pow (nh_74, y_78);
            c_73.xyz = (((tmpvar_15 * tmpvar_1) * diff_75) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_79));
            c_73.w = tmpvar_19;
            c_72.w = c_73.w;
            c_72.xyz = c_73.xyz;
            gl_FragData[0] = c_72;
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
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
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
            highp mat3 tmpvar_20;
            tmpvar_20[0] = unity_WorldToObject[0].xyz;
            tmpvar_20[1] = unity_WorldToObject[1].xyz;
            tmpvar_20[2] = unity_WorldToObject[2].xyz;
            highp vec3 tmpvar_21;
            tmpvar_21 = normalize((tmpvar_6 * tmpvar_20));
            highp mat3 tmpvar_22;
            tmpvar_22[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_22[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_22[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_23;
            tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.xyz));
            worldTangent_3 = tmpvar_23;
            highp float tmpvar_24;
            tmpvar_24 = (_glesTANGENT.w * unity_WorldTransformParams.w);
            tangentSign_2 = tmpvar_24;
            highp vec3 tmpvar_25;
            tmpvar_25 = (((tmpvar_21.yzx * worldTangent_3.zxy) - (tmpvar_21.zxy * worldTangent_3.yzx)) * tangentSign_2);
            worldBinormal_1 = tmpvar_25;
            highp vec3 tmpvar_26;
            tmpvar_26.x = worldTangent_3.x;
            tmpvar_26.y = worldBinormal_1.x;
            tmpvar_26.z = tmpvar_21.x;
            highp vec3 tmpvar_27;
            tmpvar_27.x = worldTangent_3.y;
            tmpvar_27.y = worldBinormal_1.y;
            tmpvar_27.z = tmpvar_21.y;
            highp vec3 tmpvar_28;
            tmpvar_28.x = worldTangent_3.z;
            tmpvar_28.y = worldBinormal_1.z;
            tmpvar_28.z = tmpvar_21.z;
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_19));
            xlv_TEXCOORD0 = tmpvar_4;
            xlv_TEXCOORD1 = ((tmpvar_7.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_26;
            xlv_TEXCOORD3 = tmpvar_27;
            xlv_TEXCOORD4 = tmpvar_28;
            xlv_TEXCOORD5 = (unity_ObjectToWorld * tmpvar_5).xyz;
            xlv_COLOR0 = _glesColor;
            xlv_TEXCOORD6 = tmpvar_8;
            xlv_TEXCOORD7 = (tmpvar_18 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz));
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform mediump vec4 _WorldSpaceLightPos0;
          uniform highp mat4 unity_ObjectToWorld;
          uniform lowp vec4 _LightColor0;
          uniform lowp vec4 _SpecColor;
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
          uniform highp float _Bevel;
          uniform highp float _BevelOffset;
          uniform highp float _BevelWidth;
          uniform highp float _BevelClamp;
          uniform highp float _BevelRoundness;
          uniform sampler2D _BumpMap;
          uniform highp float _BumpOutline;
          uniform highp float _BumpFace;
          uniform lowp samplerCube _Cube;
          uniform lowp vec4 _ReflectFaceColor;
          uniform lowp vec4 _ReflectOutlineColor;
          uniform highp float _ShaderFlags;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          uniform highp float _TextureWidth;
          uniform highp float _TextureHeight;
          uniform highp float _GradientScale;
          uniform mediump float _FaceShininess;
          uniform mediump float _OutlineShininess;
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          void main ()
          {
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp vec3 tmpvar_4;
            lowp vec3 tmpvar_5;
            lowp vec3 tmpvar_6;
            lowp float tmpvar_7;
            lowp float tmpvar_8;
            highp vec3 worldViewDir_9;
            lowp vec3 lightDir_10;
            lowp vec3 _unity_tbn_2_11;
            lowp vec3 _unity_tbn_1_12;
            lowp vec3 _unity_tbn_0_13;
            _unity_tbn_0_13 = xlv_TEXCOORD2;
            _unity_tbn_1_12 = xlv_TEXCOORD3;
            _unity_tbn_2_11 = xlv_TEXCOORD4;
            mediump vec3 tmpvar_14;
            tmpvar_14 = _WorldSpaceLightPos0.xyz;
            lightDir_10 = tmpvar_14;
            worldViewDir_9 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD5));
            tmpvar_4 = vec3(0.0, 0.0, 0.0);
            tmpvar_6 = vec3(0.0, 0.0, 0.0);
            tmpvar_8 = 0.0;
            tmpvar_7 = 0.0;
            tmpvar_5 = vec3(0.0, 0.0, 1.0);
            lowp vec3 tmpvar_15;
            lowp vec3 tmpvar_16;
            lowp vec3 tmpvar_17;
            lowp float tmpvar_18;
            lowp float tmpvar_19;
            tmpvar_15 = tmpvar_4;
            tmpvar_16 = tmpvar_5;
            tmpvar_17 = tmpvar_6;
            tmpvar_18 = tmpvar_7;
            tmpvar_19 = tmpvar_8;
            highp vec3 bump_20;
            highp vec4 outlineColor_21;
            highp vec4 faceColor_22;
            highp float c_23;
            highp vec4 smp4x_24;
            highp vec3 tmpvar_25;
            tmpvar_25.z = 0.0;
            tmpvar_25.x = (1.0/(_TextureWidth));
            tmpvar_25.y = (1.0/(_TextureHeight));
            highp vec2 P_26;
            P_26 = (xlv_TEXCOORD0.xy - tmpvar_25.xz);
            highp vec2 P_27;
            P_27 = (xlv_TEXCOORD0.xy + tmpvar_25.xz);
            highp vec2 P_28;
            P_28 = (xlv_TEXCOORD0.xy - tmpvar_25.zy);
            highp vec2 P_29;
            P_29 = (xlv_TEXCOORD0.xy + tmpvar_25.zy);
            lowp vec4 tmpvar_30;
            tmpvar_30.x = texture2D (_MainTex, P_26).w;
            tmpvar_30.y = texture2D (_MainTex, P_27).w;
            tmpvar_30.z = texture2D (_MainTex, P_28).w;
            tmpvar_30.w = texture2D (_MainTex, P_29).w;
            smp4x_24 = tmpvar_30;
            lowp float tmpvar_31;
            tmpvar_31 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_23 = tmpvar_31;
            highp float tmpvar_32;
            tmpvar_32 = (((
              (0.5 - c_23)
             - xlv_TEXCOORD6.x) * xlv_TEXCOORD6.y) + 0.5);
            highp float tmpvar_33;
            tmpvar_33 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD6.y);
            highp float tmpvar_34;
            tmpvar_34 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD6.y);
            faceColor_22 = _FaceColor;
            outlineColor_21 = _OutlineColor;
            faceColor_22 = (faceColor_22 * xlv_COLOR0);
            outlineColor_21.w = (outlineColor_21.w * xlv_COLOR0.w);
            highp vec2 tmpvar_35;
            tmpvar_35.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_35.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_36;
            tmpvar_36 = texture2D (_FaceTex, tmpvar_35);
            faceColor_22 = (faceColor_22 * tmpvar_36);
            highp vec2 tmpvar_37;
            tmpvar_37.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_37.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_38;
            tmpvar_38 = texture2D (_OutlineTex, tmpvar_37);
            outlineColor_21 = (outlineColor_21 * tmpvar_38);
            mediump float d_39;
            d_39 = tmpvar_32;
            lowp vec4 faceColor_40;
            faceColor_40 = faceColor_22;
            lowp vec4 outlineColor_41;
            outlineColor_41 = outlineColor_21;
            mediump float outline_42;
            outline_42 = tmpvar_33;
            mediump float softness_43;
            softness_43 = tmpvar_34;
            mediump float tmpvar_44;
            tmpvar_44 = (1.0 - clamp ((
              ((d_39 - (outline_42 * 0.5)) + (softness_43 * 0.5))
             / 
              (1.0 + softness_43)
            ), 0.0, 1.0));
            faceColor_40.xyz = (faceColor_40.xyz * faceColor_40.w);
            outlineColor_41.xyz = (outlineColor_41.xyz * outlineColor_41.w);
            mediump vec4 tmpvar_45;
            tmpvar_45 = mix (faceColor_40, outlineColor_41, vec4((clamp (
              (d_39 + (outline_42 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_42)
            ))));
            faceColor_40 = tmpvar_45;
            faceColor_40 = (faceColor_40 * tmpvar_44);
            faceColor_22 = faceColor_40;
            faceColor_22.xyz = (faceColor_22.xyz / max (faceColor_22.w, 0.0001));
            highp vec4 h_46;
            h_46 = smp4x_24;
            highp float tmpvar_47;
            tmpvar_47 = (_ShaderFlags / 2.0);
            highp float tmpvar_48;
            tmpvar_48 = (fract(abs(tmpvar_47)) * 2.0);
            highp float tmpvar_49;
            if ((tmpvar_47 >= 0.0)) {
              tmpvar_49 = tmpvar_48;
            } else {
              tmpvar_49 = -(tmpvar_48);
            };
            h_46 = (smp4x_24 + (xlv_TEXCOORD6.x + _BevelOffset));
            highp float tmpvar_50;
            tmpvar_50 = max (0.01, (_OutlineWidth + _BevelWidth));
            h_46 = (h_46 - 0.5);
            h_46 = (h_46 / tmpvar_50);
            highp vec4 tmpvar_51;
            tmpvar_51 = clamp ((h_46 + 0.5), 0.0, 1.0);
            h_46 = tmpvar_51;
            if (bool(float((tmpvar_49 >= 1.0)))) {
              h_46 = (1.0 - abs((
                (tmpvar_51 * 2.0)
               - 1.0)));
            };
            h_46 = (min (mix (h_46, 
              sin(((h_46 * 3.141592) / 2.0))
            , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((_Bevel * tmpvar_50) * (_GradientScale * -2.0)));
            highp vec3 tmpvar_52;
            tmpvar_52.xy = vec2(1.0, 0.0);
            tmpvar_52.z = (h_46.y - h_46.x);
            highp vec3 tmpvar_53;
            tmpvar_53 = normalize(tmpvar_52);
            highp vec3 tmpvar_54;
            tmpvar_54.xy = vec2(0.0, -1.0);
            tmpvar_54.z = (h_46.w - h_46.z);
            highp vec3 tmpvar_55;
            tmpvar_55 = normalize(tmpvar_54);
            lowp vec3 tmpvar_56;
            tmpvar_56 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
            bump_20 = tmpvar_56;
            bump_20 = (bump_20 * mix (_BumpFace, _BumpOutline, clamp (
              (tmpvar_32 + (tmpvar_33 * 0.5))
            , 0.0, 1.0)));
            highp vec3 tmpvar_57;
            tmpvar_57 = mix (vec3(0.0, 0.0, 1.0), bump_20, faceColor_22.www);
            bump_20 = tmpvar_57;
            highp vec3 tmpvar_58;
            tmpvar_58 = normalize(((
              (tmpvar_53.yzx * tmpvar_55.zxy)
             - 
              (tmpvar_53.zxy * tmpvar_55.yzx)
            ) - tmpvar_57));
            highp mat3 tmpvar_59;
            tmpvar_59[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_59[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_59[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_60;
            highp vec3 N_61;
            N_61 = (tmpvar_59 * tmpvar_58);
            tmpvar_60 = (xlv_TEXCOORD7 - (2.0 * (
              dot (N_61, xlv_TEXCOORD7)
             * N_61)));
            lowp vec4 tmpvar_62;
            tmpvar_62 = textureCube (_Cube, tmpvar_60);
            highp float tmpvar_63;
            tmpvar_63 = clamp ((tmpvar_32 + (tmpvar_33 * 0.5)), 0.0, 1.0);
            lowp vec3 tmpvar_64;
            tmpvar_64 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_63));
            highp vec3 tmpvar_65;
            tmpvar_65 = ((tmpvar_62.xyz * tmpvar_64) * faceColor_22.w);
            tmpvar_15 = faceColor_22.xyz;
            tmpvar_16 = -(tmpvar_58);
            tmpvar_17 = tmpvar_65;
            highp float tmpvar_66;
            tmpvar_66 = clamp ((tmpvar_32 + (tmpvar_33 * 0.5)), 0.0, 1.0);
            tmpvar_18 = 1.0;
            tmpvar_19 = faceColor_22.w;
            tmpvar_4 = tmpvar_15;
            tmpvar_6 = tmpvar_17;
            tmpvar_7 = tmpvar_18;
            tmpvar_8 = tmpvar_19;
            lowp float tmpvar_67;
            tmpvar_67 = dot (_unity_tbn_0_13, tmpvar_16);
            worldN_3.x = tmpvar_67;
            lowp float tmpvar_68;
            tmpvar_68 = dot (_unity_tbn_1_12, tmpvar_16);
            worldN_3.y = tmpvar_68;
            lowp float tmpvar_69;
            tmpvar_69 = dot (_unity_tbn_2_11, tmpvar_16);
            worldN_3.z = tmpvar_69;
            highp vec3 tmpvar_70;
            tmpvar_70 = normalize(worldN_3);
            worldN_3 = tmpvar_70;
            tmpvar_5 = tmpvar_70;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_10;
            mediump vec3 viewDir_71;
            viewDir_71 = worldViewDir_9;
            lowp vec4 c_72;
            lowp vec4 c_73;
            highp float nh_74;
            lowp float diff_75;
            mediump float tmpvar_76;
            tmpvar_76 = max (0.0, dot (tmpvar_5, tmpvar_2));
            diff_75 = tmpvar_76;
            mediump float tmpvar_77;
            tmpvar_77 = max (0.0, dot (tmpvar_5, normalize(
              (tmpvar_2 + viewDir_71)
            )));
            nh_74 = tmpvar_77;
            mediump float y_78;
            y_78 = (mix (_FaceShininess, _OutlineShininess, tmpvar_66) * 128.0);
            highp float tmpvar_79;
            tmpvar_79 = pow (nh_74, y_78);
            c_73.xyz = (((tmpvar_15 * tmpvar_1) * diff_75) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_79));
            c_73.w = tmpvar_19;
            c_72.w = c_73.w;
            c_72.xyz = c_73.xyz;
            gl_FragData[0] = c_72;
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
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
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
            highp mat3 tmpvar_20;
            tmpvar_20[0] = unity_WorldToObject[0].xyz;
            tmpvar_20[1] = unity_WorldToObject[1].xyz;
            tmpvar_20[2] = unity_WorldToObject[2].xyz;
            highp vec3 tmpvar_21;
            tmpvar_21 = normalize((tmpvar_6 * tmpvar_20));
            highp mat3 tmpvar_22;
            tmpvar_22[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_22[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_22[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_23;
            tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.xyz));
            worldTangent_3 = tmpvar_23;
            highp float tmpvar_24;
            tmpvar_24 = (_glesTANGENT.w * unity_WorldTransformParams.w);
            tangentSign_2 = tmpvar_24;
            highp vec3 tmpvar_25;
            tmpvar_25 = (((tmpvar_21.yzx * worldTangent_3.zxy) - (tmpvar_21.zxy * worldTangent_3.yzx)) * tangentSign_2);
            worldBinormal_1 = tmpvar_25;
            highp vec3 tmpvar_26;
            tmpvar_26.x = worldTangent_3.x;
            tmpvar_26.y = worldBinormal_1.x;
            tmpvar_26.z = tmpvar_21.x;
            highp vec3 tmpvar_27;
            tmpvar_27.x = worldTangent_3.y;
            tmpvar_27.y = worldBinormal_1.y;
            tmpvar_27.z = tmpvar_21.y;
            highp vec3 tmpvar_28;
            tmpvar_28.x = worldTangent_3.z;
            tmpvar_28.y = worldBinormal_1.z;
            tmpvar_28.z = tmpvar_21.z;
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_19));
            xlv_TEXCOORD0 = tmpvar_4;
            xlv_TEXCOORD1 = ((tmpvar_7.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_26;
            xlv_TEXCOORD3 = tmpvar_27;
            xlv_TEXCOORD4 = tmpvar_28;
            xlv_TEXCOORD5 = (unity_ObjectToWorld * tmpvar_5).xyz;
            xlv_COLOR0 = _glesColor;
            xlv_TEXCOORD6 = tmpvar_8;
            xlv_TEXCOORD7 = (tmpvar_18 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz));
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform mediump vec4 _WorldSpaceLightPos0;
          uniform highp mat4 unity_ObjectToWorld;
          uniform lowp vec4 _LightColor0;
          uniform lowp vec4 _SpecColor;
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
          uniform highp float _Bevel;
          uniform highp float _BevelOffset;
          uniform highp float _BevelWidth;
          uniform highp float _BevelClamp;
          uniform highp float _BevelRoundness;
          uniform sampler2D _BumpMap;
          uniform highp float _BumpOutline;
          uniform highp float _BumpFace;
          uniform lowp samplerCube _Cube;
          uniform lowp vec4 _ReflectFaceColor;
          uniform lowp vec4 _ReflectOutlineColor;
          uniform highp float _ShaderFlags;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          uniform highp float _TextureWidth;
          uniform highp float _TextureHeight;
          uniform highp float _GradientScale;
          uniform mediump float _FaceShininess;
          uniform mediump float _OutlineShininess;
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          void main ()
          {
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp vec3 tmpvar_4;
            lowp vec3 tmpvar_5;
            lowp vec3 tmpvar_6;
            lowp float tmpvar_7;
            lowp float tmpvar_8;
            highp vec3 worldViewDir_9;
            lowp vec3 lightDir_10;
            lowp vec3 _unity_tbn_2_11;
            lowp vec3 _unity_tbn_1_12;
            lowp vec3 _unity_tbn_0_13;
            _unity_tbn_0_13 = xlv_TEXCOORD2;
            _unity_tbn_1_12 = xlv_TEXCOORD3;
            _unity_tbn_2_11 = xlv_TEXCOORD4;
            mediump vec3 tmpvar_14;
            tmpvar_14 = _WorldSpaceLightPos0.xyz;
            lightDir_10 = tmpvar_14;
            worldViewDir_9 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD5));
            tmpvar_4 = vec3(0.0, 0.0, 0.0);
            tmpvar_6 = vec3(0.0, 0.0, 0.0);
            tmpvar_8 = 0.0;
            tmpvar_7 = 0.0;
            tmpvar_5 = vec3(0.0, 0.0, 1.0);
            lowp vec3 tmpvar_15;
            lowp vec3 tmpvar_16;
            lowp vec3 tmpvar_17;
            lowp float tmpvar_18;
            lowp float tmpvar_19;
            tmpvar_15 = tmpvar_4;
            tmpvar_16 = tmpvar_5;
            tmpvar_17 = tmpvar_6;
            tmpvar_18 = tmpvar_7;
            tmpvar_19 = tmpvar_8;
            highp vec3 bump_20;
            highp vec4 outlineColor_21;
            highp vec4 faceColor_22;
            highp float c_23;
            highp vec4 smp4x_24;
            highp vec3 tmpvar_25;
            tmpvar_25.z = 0.0;
            tmpvar_25.x = (1.0/(_TextureWidth));
            tmpvar_25.y = (1.0/(_TextureHeight));
            highp vec2 P_26;
            P_26 = (xlv_TEXCOORD0.xy - tmpvar_25.xz);
            highp vec2 P_27;
            P_27 = (xlv_TEXCOORD0.xy + tmpvar_25.xz);
            highp vec2 P_28;
            P_28 = (xlv_TEXCOORD0.xy - tmpvar_25.zy);
            highp vec2 P_29;
            P_29 = (xlv_TEXCOORD0.xy + tmpvar_25.zy);
            lowp vec4 tmpvar_30;
            tmpvar_30.x = texture2D (_MainTex, P_26).w;
            tmpvar_30.y = texture2D (_MainTex, P_27).w;
            tmpvar_30.z = texture2D (_MainTex, P_28).w;
            tmpvar_30.w = texture2D (_MainTex, P_29).w;
            smp4x_24 = tmpvar_30;
            lowp float tmpvar_31;
            tmpvar_31 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_23 = tmpvar_31;
            highp float tmpvar_32;
            tmpvar_32 = (((
              (0.5 - c_23)
             - xlv_TEXCOORD6.x) * xlv_TEXCOORD6.y) + 0.5);
            highp float tmpvar_33;
            tmpvar_33 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD6.y);
            highp float tmpvar_34;
            tmpvar_34 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD6.y);
            faceColor_22 = _FaceColor;
            outlineColor_21 = _OutlineColor;
            faceColor_22 = (faceColor_22 * xlv_COLOR0);
            outlineColor_21.w = (outlineColor_21.w * xlv_COLOR0.w);
            highp vec2 tmpvar_35;
            tmpvar_35.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_35.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_36;
            tmpvar_36 = texture2D (_FaceTex, tmpvar_35);
            faceColor_22 = (faceColor_22 * tmpvar_36);
            highp vec2 tmpvar_37;
            tmpvar_37.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_37.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_38;
            tmpvar_38 = texture2D (_OutlineTex, tmpvar_37);
            outlineColor_21 = (outlineColor_21 * tmpvar_38);
            mediump float d_39;
            d_39 = tmpvar_32;
            lowp vec4 faceColor_40;
            faceColor_40 = faceColor_22;
            lowp vec4 outlineColor_41;
            outlineColor_41 = outlineColor_21;
            mediump float outline_42;
            outline_42 = tmpvar_33;
            mediump float softness_43;
            softness_43 = tmpvar_34;
            mediump float tmpvar_44;
            tmpvar_44 = (1.0 - clamp ((
              ((d_39 - (outline_42 * 0.5)) + (softness_43 * 0.5))
             / 
              (1.0 + softness_43)
            ), 0.0, 1.0));
            faceColor_40.xyz = (faceColor_40.xyz * faceColor_40.w);
            outlineColor_41.xyz = (outlineColor_41.xyz * outlineColor_41.w);
            mediump vec4 tmpvar_45;
            tmpvar_45 = mix (faceColor_40, outlineColor_41, vec4((clamp (
              (d_39 + (outline_42 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_42)
            ))));
            faceColor_40 = tmpvar_45;
            faceColor_40 = (faceColor_40 * tmpvar_44);
            faceColor_22 = faceColor_40;
            faceColor_22.xyz = (faceColor_22.xyz / max (faceColor_22.w, 0.0001));
            highp vec4 h_46;
            h_46 = smp4x_24;
            highp float tmpvar_47;
            tmpvar_47 = (_ShaderFlags / 2.0);
            highp float tmpvar_48;
            tmpvar_48 = (fract(abs(tmpvar_47)) * 2.0);
            highp float tmpvar_49;
            if ((tmpvar_47 >= 0.0)) {
              tmpvar_49 = tmpvar_48;
            } else {
              tmpvar_49 = -(tmpvar_48);
            };
            h_46 = (smp4x_24 + (xlv_TEXCOORD6.x + _BevelOffset));
            highp float tmpvar_50;
            tmpvar_50 = max (0.01, (_OutlineWidth + _BevelWidth));
            h_46 = (h_46 - 0.5);
            h_46 = (h_46 / tmpvar_50);
            highp vec4 tmpvar_51;
            tmpvar_51 = clamp ((h_46 + 0.5), 0.0, 1.0);
            h_46 = tmpvar_51;
            if (bool(float((tmpvar_49 >= 1.0)))) {
              h_46 = (1.0 - abs((
                (tmpvar_51 * 2.0)
               - 1.0)));
            };
            h_46 = (min (mix (h_46, 
              sin(((h_46 * 3.141592) / 2.0))
            , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((_Bevel * tmpvar_50) * (_GradientScale * -2.0)));
            highp vec3 tmpvar_52;
            tmpvar_52.xy = vec2(1.0, 0.0);
            tmpvar_52.z = (h_46.y - h_46.x);
            highp vec3 tmpvar_53;
            tmpvar_53 = normalize(tmpvar_52);
            highp vec3 tmpvar_54;
            tmpvar_54.xy = vec2(0.0, -1.0);
            tmpvar_54.z = (h_46.w - h_46.z);
            highp vec3 tmpvar_55;
            tmpvar_55 = normalize(tmpvar_54);
            lowp vec3 tmpvar_56;
            tmpvar_56 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
            bump_20 = tmpvar_56;
            bump_20 = (bump_20 * mix (_BumpFace, _BumpOutline, clamp (
              (tmpvar_32 + (tmpvar_33 * 0.5))
            , 0.0, 1.0)));
            highp vec3 tmpvar_57;
            tmpvar_57 = mix (vec3(0.0, 0.0, 1.0), bump_20, faceColor_22.www);
            bump_20 = tmpvar_57;
            highp vec3 tmpvar_58;
            tmpvar_58 = normalize(((
              (tmpvar_53.yzx * tmpvar_55.zxy)
             - 
              (tmpvar_53.zxy * tmpvar_55.yzx)
            ) - tmpvar_57));
            highp mat3 tmpvar_59;
            tmpvar_59[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_59[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_59[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_60;
            highp vec3 N_61;
            N_61 = (tmpvar_59 * tmpvar_58);
            tmpvar_60 = (xlv_TEXCOORD7 - (2.0 * (
              dot (N_61, xlv_TEXCOORD7)
             * N_61)));
            lowp vec4 tmpvar_62;
            tmpvar_62 = textureCube (_Cube, tmpvar_60);
            highp float tmpvar_63;
            tmpvar_63 = clamp ((tmpvar_32 + (tmpvar_33 * 0.5)), 0.0, 1.0);
            lowp vec3 tmpvar_64;
            tmpvar_64 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_63));
            highp vec3 tmpvar_65;
            tmpvar_65 = ((tmpvar_62.xyz * tmpvar_64) * faceColor_22.w);
            tmpvar_15 = faceColor_22.xyz;
            tmpvar_16 = -(tmpvar_58);
            tmpvar_17 = tmpvar_65;
            highp float tmpvar_66;
            tmpvar_66 = clamp ((tmpvar_32 + (tmpvar_33 * 0.5)), 0.0, 1.0);
            tmpvar_18 = 1.0;
            tmpvar_19 = faceColor_22.w;
            tmpvar_4 = tmpvar_15;
            tmpvar_6 = tmpvar_17;
            tmpvar_7 = tmpvar_18;
            tmpvar_8 = tmpvar_19;
            lowp float tmpvar_67;
            tmpvar_67 = dot (_unity_tbn_0_13, tmpvar_16);
            worldN_3.x = tmpvar_67;
            lowp float tmpvar_68;
            tmpvar_68 = dot (_unity_tbn_1_12, tmpvar_16);
            worldN_3.y = tmpvar_68;
            lowp float tmpvar_69;
            tmpvar_69 = dot (_unity_tbn_2_11, tmpvar_16);
            worldN_3.z = tmpvar_69;
            highp vec3 tmpvar_70;
            tmpvar_70 = normalize(worldN_3);
            worldN_3 = tmpvar_70;
            tmpvar_5 = tmpvar_70;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_10;
            mediump vec3 viewDir_71;
            viewDir_71 = worldViewDir_9;
            lowp vec4 c_72;
            lowp vec4 c_73;
            highp float nh_74;
            lowp float diff_75;
            mediump float tmpvar_76;
            tmpvar_76 = max (0.0, dot (tmpvar_5, tmpvar_2));
            diff_75 = tmpvar_76;
            mediump float tmpvar_77;
            tmpvar_77 = max (0.0, dot (tmpvar_5, normalize(
              (tmpvar_2 + viewDir_71)
            )));
            nh_74 = tmpvar_77;
            mediump float y_78;
            y_78 = (mix (_FaceShininess, _OutlineShininess, tmpvar_66) * 128.0);
            highp float tmpvar_79;
            tmpvar_79 = pow (nh_74, y_78);
            c_73.xyz = (((tmpvar_15 * tmpvar_1) * diff_75) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_79));
            c_73.w = tmpvar_19;
            c_72.w = c_73.w;
            c_72.xyz = c_73.xyz;
            gl_FragData[0] = c_72;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier00"
        {
           Keywords { "SPOT" }
          
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
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp vec4 unity_WorldTransformParams;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp mat4 unity_WorldToLight;
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
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          varying highp vec4 xlv_TEXCOORD8;
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
            highp mat3 tmpvar_20;
            tmpvar_20[0] = unity_WorldToObject[0].xyz;
            tmpvar_20[1] = unity_WorldToObject[1].xyz;
            tmpvar_20[2] = unity_WorldToObject[2].xyz;
            highp vec3 tmpvar_21;
            tmpvar_21 = normalize((tmpvar_6 * tmpvar_20));
            highp mat3 tmpvar_22;
            tmpvar_22[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_22[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_22[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_23;
            tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.xyz));
            worldTangent_3 = tmpvar_23;
            highp float tmpvar_24;
            tmpvar_24 = (_glesTANGENT.w * unity_WorldTransformParams.w);
            tangentSign_2 = tmpvar_24;
            highp vec3 tmpvar_25;
            tmpvar_25 = (((tmpvar_21.yzx * worldTangent_3.zxy) - (tmpvar_21.zxy * worldTangent_3.yzx)) * tangentSign_2);
            worldBinormal_1 = tmpvar_25;
            highp vec3 tmpvar_26;
            tmpvar_26.x = worldTangent_3.x;
            tmpvar_26.y = worldBinormal_1.x;
            tmpvar_26.z = tmpvar_21.x;
            highp vec3 tmpvar_27;
            tmpvar_27.x = worldTangent_3.y;
            tmpvar_27.y = worldBinormal_1.y;
            tmpvar_27.z = tmpvar_21.y;
            highp vec3 tmpvar_28;
            tmpvar_28.x = worldTangent_3.z;
            tmpvar_28.y = worldBinormal_1.z;
            tmpvar_28.z = tmpvar_21.z;
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_19));
            xlv_TEXCOORD0 = tmpvar_4;
            xlv_TEXCOORD1 = ((tmpvar_7.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_26;
            xlv_TEXCOORD3 = tmpvar_27;
            xlv_TEXCOORD4 = tmpvar_28;
            xlv_TEXCOORD5 = (unity_ObjectToWorld * tmpvar_5).xyz;
            xlv_COLOR0 = _glesColor;
            xlv_TEXCOORD6 = tmpvar_8;
            xlv_TEXCOORD7 = (tmpvar_18 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz));
            xlv_TEXCOORD8 = (unity_WorldToLight * (unity_ObjectToWorld * tmpvar_5));
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _WorldSpaceLightPos0;
          uniform highp mat4 unity_ObjectToWorld;
          uniform lowp vec4 _LightColor0;
          uniform lowp vec4 _SpecColor;
          uniform highp sampler2D _LightTexture0;
          uniform highp sampler2D _LightTextureB0;
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
          uniform highp float _Bevel;
          uniform highp float _BevelOffset;
          uniform highp float _BevelWidth;
          uniform highp float _BevelClamp;
          uniform highp float _BevelRoundness;
          uniform sampler2D _BumpMap;
          uniform highp float _BumpOutline;
          uniform highp float _BumpFace;
          uniform lowp samplerCube _Cube;
          uniform lowp vec4 _ReflectFaceColor;
          uniform lowp vec4 _ReflectOutlineColor;
          uniform highp float _ShaderFlags;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          uniform highp float _TextureWidth;
          uniform highp float _TextureHeight;
          uniform highp float _GradientScale;
          uniform mediump float _FaceShininess;
          uniform mediump float _OutlineShininess;
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          varying highp vec4 xlv_TEXCOORD8;
          void main ()
          {
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp float atten_4;
            lowp vec3 tmpvar_5;
            lowp vec3 tmpvar_6;
            lowp vec3 tmpvar_7;
            lowp float tmpvar_8;
            lowp float tmpvar_9;
            highp vec3 worldViewDir_10;
            lowp vec3 lightDir_11;
            lowp vec3 _unity_tbn_2_12;
            lowp vec3 _unity_tbn_1_13;
            lowp vec3 _unity_tbn_0_14;
            _unity_tbn_0_14 = xlv_TEXCOORD2;
            _unity_tbn_1_13 = xlv_TEXCOORD3;
            _unity_tbn_2_12 = xlv_TEXCOORD4;
            highp vec3 tmpvar_15;
            tmpvar_15 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD5));
            lightDir_11 = tmpvar_15;
            worldViewDir_10 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD5));
            tmpvar_5 = vec3(0.0, 0.0, 0.0);
            tmpvar_7 = vec3(0.0, 0.0, 0.0);
            tmpvar_9 = 0.0;
            tmpvar_8 = 0.0;
            tmpvar_6 = vec3(0.0, 0.0, 1.0);
            lowp vec3 tmpvar_16;
            lowp vec3 tmpvar_17;
            lowp vec3 tmpvar_18;
            lowp float tmpvar_19;
            lowp float tmpvar_20;
            tmpvar_16 = tmpvar_5;
            tmpvar_17 = tmpvar_6;
            tmpvar_18 = tmpvar_7;
            tmpvar_19 = tmpvar_8;
            tmpvar_20 = tmpvar_9;
            highp vec3 bump_21;
            highp vec4 outlineColor_22;
            highp vec4 faceColor_23;
            highp float c_24;
            highp vec4 smp4x_25;
            highp vec3 tmpvar_26;
            tmpvar_26.z = 0.0;
            tmpvar_26.x = (1.0/(_TextureWidth));
            tmpvar_26.y = (1.0/(_TextureHeight));
            highp vec2 P_27;
            P_27 = (xlv_TEXCOORD0.xy - tmpvar_26.xz);
            highp vec2 P_28;
            P_28 = (xlv_TEXCOORD0.xy + tmpvar_26.xz);
            highp vec2 P_29;
            P_29 = (xlv_TEXCOORD0.xy - tmpvar_26.zy);
            highp vec2 P_30;
            P_30 = (xlv_TEXCOORD0.xy + tmpvar_26.zy);
            lowp vec4 tmpvar_31;
            tmpvar_31.x = texture2D (_MainTex, P_27).w;
            tmpvar_31.y = texture2D (_MainTex, P_28).w;
            tmpvar_31.z = texture2D (_MainTex, P_29).w;
            tmpvar_31.w = texture2D (_MainTex, P_30).w;
            smp4x_25 = tmpvar_31;
            lowp float tmpvar_32;
            tmpvar_32 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_24 = tmpvar_32;
            highp float tmpvar_33;
            tmpvar_33 = (((
              (0.5 - c_24)
             - xlv_TEXCOORD6.x) * xlv_TEXCOORD6.y) + 0.5);
            highp float tmpvar_34;
            tmpvar_34 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD6.y);
            highp float tmpvar_35;
            tmpvar_35 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD6.y);
            faceColor_23 = _FaceColor;
            outlineColor_22 = _OutlineColor;
            faceColor_23 = (faceColor_23 * xlv_COLOR0);
            outlineColor_22.w = (outlineColor_22.w * xlv_COLOR0.w);
            highp vec2 tmpvar_36;
            tmpvar_36.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_36.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_37;
            tmpvar_37 = texture2D (_FaceTex, tmpvar_36);
            faceColor_23 = (faceColor_23 * tmpvar_37);
            highp vec2 tmpvar_38;
            tmpvar_38.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_38.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_39;
            tmpvar_39 = texture2D (_OutlineTex, tmpvar_38);
            outlineColor_22 = (outlineColor_22 * tmpvar_39);
            mediump float d_40;
            d_40 = tmpvar_33;
            lowp vec4 faceColor_41;
            faceColor_41 = faceColor_23;
            lowp vec4 outlineColor_42;
            outlineColor_42 = outlineColor_22;
            mediump float outline_43;
            outline_43 = tmpvar_34;
            mediump float softness_44;
            softness_44 = tmpvar_35;
            mediump float tmpvar_45;
            tmpvar_45 = (1.0 - clamp ((
              ((d_40 - (outline_43 * 0.5)) + (softness_44 * 0.5))
             / 
              (1.0 + softness_44)
            ), 0.0, 1.0));
            faceColor_41.xyz = (faceColor_41.xyz * faceColor_41.w);
            outlineColor_42.xyz = (outlineColor_42.xyz * outlineColor_42.w);
            mediump vec4 tmpvar_46;
            tmpvar_46 = mix (faceColor_41, outlineColor_42, vec4((clamp (
              (d_40 + (outline_43 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_43)
            ))));
            faceColor_41 = tmpvar_46;
            faceColor_41 = (faceColor_41 * tmpvar_45);
            faceColor_23 = faceColor_41;
            faceColor_23.xyz = (faceColor_23.xyz / max (faceColor_23.w, 0.0001));
            highp vec4 h_47;
            h_47 = smp4x_25;
            highp float tmpvar_48;
            tmpvar_48 = (_ShaderFlags / 2.0);
            highp float tmpvar_49;
            tmpvar_49 = (fract(abs(tmpvar_48)) * 2.0);
            highp float tmpvar_50;
            if ((tmpvar_48 >= 0.0)) {
              tmpvar_50 = tmpvar_49;
            } else {
              tmpvar_50 = -(tmpvar_49);
            };
            h_47 = (smp4x_25 + (xlv_TEXCOORD6.x + _BevelOffset));
            highp float tmpvar_51;
            tmpvar_51 = max (0.01, (_OutlineWidth + _BevelWidth));
            h_47 = (h_47 - 0.5);
            h_47 = (h_47 / tmpvar_51);
            highp vec4 tmpvar_52;
            tmpvar_52 = clamp ((h_47 + 0.5), 0.0, 1.0);
            h_47 = tmpvar_52;
            if (bool(float((tmpvar_50 >= 1.0)))) {
              h_47 = (1.0 - abs((
                (tmpvar_52 * 2.0)
               - 1.0)));
            };
            h_47 = (min (mix (h_47, 
              sin(((h_47 * 3.141592) / 2.0))
            , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((_Bevel * tmpvar_51) * (_GradientScale * -2.0)));
            highp vec3 tmpvar_53;
            tmpvar_53.xy = vec2(1.0, 0.0);
            tmpvar_53.z = (h_47.y - h_47.x);
            highp vec3 tmpvar_54;
            tmpvar_54 = normalize(tmpvar_53);
            highp vec3 tmpvar_55;
            tmpvar_55.xy = vec2(0.0, -1.0);
            tmpvar_55.z = (h_47.w - h_47.z);
            highp vec3 tmpvar_56;
            tmpvar_56 = normalize(tmpvar_55);
            lowp vec3 tmpvar_57;
            tmpvar_57 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
            bump_21 = tmpvar_57;
            bump_21 = (bump_21 * mix (_BumpFace, _BumpOutline, clamp (
              (tmpvar_33 + (tmpvar_34 * 0.5))
            , 0.0, 1.0)));
            highp vec3 tmpvar_58;
            tmpvar_58 = mix (vec3(0.0, 0.0, 1.0), bump_21, faceColor_23.www);
            bump_21 = tmpvar_58;
            highp vec3 tmpvar_59;
            tmpvar_59 = normalize(((
              (tmpvar_54.yzx * tmpvar_56.zxy)
             - 
              (tmpvar_54.zxy * tmpvar_56.yzx)
            ) - tmpvar_58));
            highp mat3 tmpvar_60;
            tmpvar_60[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_60[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_60[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_61;
            highp vec3 N_62;
            N_62 = (tmpvar_60 * tmpvar_59);
            tmpvar_61 = (xlv_TEXCOORD7 - (2.0 * (
              dot (N_62, xlv_TEXCOORD7)
             * N_62)));
            lowp vec4 tmpvar_63;
            tmpvar_63 = textureCube (_Cube, tmpvar_61);
            highp float tmpvar_64;
            tmpvar_64 = clamp ((tmpvar_33 + (tmpvar_34 * 0.5)), 0.0, 1.0);
            lowp vec3 tmpvar_65;
            tmpvar_65 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_64));
            highp vec3 tmpvar_66;
            tmpvar_66 = ((tmpvar_63.xyz * tmpvar_65) * faceColor_23.w);
            tmpvar_16 = faceColor_23.xyz;
            tmpvar_17 = -(tmpvar_59);
            tmpvar_18 = tmpvar_66;
            highp float tmpvar_67;
            tmpvar_67 = clamp ((tmpvar_33 + (tmpvar_34 * 0.5)), 0.0, 1.0);
            tmpvar_19 = 1.0;
            tmpvar_20 = faceColor_23.w;
            tmpvar_5 = tmpvar_16;
            tmpvar_7 = tmpvar_18;
            tmpvar_8 = tmpvar_19;
            tmpvar_9 = tmpvar_20;
            lowp float tmpvar_68;
            highp vec4 tmpvar_69;
            tmpvar_69 = texture2D (_LightTexture0, ((xlv_TEXCOORD8.xy / xlv_TEXCOORD8.w) + 0.5));
            tmpvar_68 = tmpvar_69.w;
            lowp float tmpvar_70;
            highp vec4 tmpvar_71;
            tmpvar_71 = texture2D (_LightTextureB0, vec2(dot (xlv_TEXCOORD8.xyz, xlv_TEXCOORD8.xyz)));
            tmpvar_70 = tmpvar_71.x;
            highp float tmpvar_72;
            tmpvar_72 = ((float(
              (xlv_TEXCOORD8.z > 0.0)
            ) * tmpvar_68) * tmpvar_70);
            atten_4 = tmpvar_72;
            lowp float tmpvar_73;
            tmpvar_73 = dot (_unity_tbn_0_14, tmpvar_17);
            worldN_3.x = tmpvar_73;
            lowp float tmpvar_74;
            tmpvar_74 = dot (_unity_tbn_1_13, tmpvar_17);
            worldN_3.y = tmpvar_74;
            lowp float tmpvar_75;
            tmpvar_75 = dot (_unity_tbn_2_12, tmpvar_17);
            worldN_3.z = tmpvar_75;
            highp vec3 tmpvar_76;
            tmpvar_76 = normalize(worldN_3);
            worldN_3 = tmpvar_76;
            tmpvar_6 = tmpvar_76;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_11;
            tmpvar_1 = (tmpvar_1 * atten_4);
            mediump vec3 viewDir_77;
            viewDir_77 = worldViewDir_10;
            lowp vec4 c_78;
            lowp vec4 c_79;
            highp float nh_80;
            lowp float diff_81;
            mediump float tmpvar_82;
            tmpvar_82 = max (0.0, dot (tmpvar_6, tmpvar_2));
            diff_81 = tmpvar_82;
            mediump float tmpvar_83;
            tmpvar_83 = max (0.0, dot (tmpvar_6, normalize(
              (tmpvar_2 + viewDir_77)
            )));
            nh_80 = tmpvar_83;
            mediump float y_84;
            y_84 = (mix (_FaceShininess, _OutlineShininess, tmpvar_67) * 128.0);
            highp float tmpvar_85;
            tmpvar_85 = pow (nh_80, y_84);
            c_79.xyz = (((tmpvar_16 * tmpvar_1) * diff_81) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_85));
            c_79.w = tmpvar_20;
            c_78.w = c_79.w;
            c_78.xyz = c_79.xyz;
            gl_FragData[0] = c_78;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier01"
        {
           Keywords { "SPOT" }
          
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
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp vec4 unity_WorldTransformParams;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp mat4 unity_WorldToLight;
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
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          varying highp vec4 xlv_TEXCOORD8;
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
            highp mat3 tmpvar_20;
            tmpvar_20[0] = unity_WorldToObject[0].xyz;
            tmpvar_20[1] = unity_WorldToObject[1].xyz;
            tmpvar_20[2] = unity_WorldToObject[2].xyz;
            highp vec3 tmpvar_21;
            tmpvar_21 = normalize((tmpvar_6 * tmpvar_20));
            highp mat3 tmpvar_22;
            tmpvar_22[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_22[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_22[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_23;
            tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.xyz));
            worldTangent_3 = tmpvar_23;
            highp float tmpvar_24;
            tmpvar_24 = (_glesTANGENT.w * unity_WorldTransformParams.w);
            tangentSign_2 = tmpvar_24;
            highp vec3 tmpvar_25;
            tmpvar_25 = (((tmpvar_21.yzx * worldTangent_3.zxy) - (tmpvar_21.zxy * worldTangent_3.yzx)) * tangentSign_2);
            worldBinormal_1 = tmpvar_25;
            highp vec3 tmpvar_26;
            tmpvar_26.x = worldTangent_3.x;
            tmpvar_26.y = worldBinormal_1.x;
            tmpvar_26.z = tmpvar_21.x;
            highp vec3 tmpvar_27;
            tmpvar_27.x = worldTangent_3.y;
            tmpvar_27.y = worldBinormal_1.y;
            tmpvar_27.z = tmpvar_21.y;
            highp vec3 tmpvar_28;
            tmpvar_28.x = worldTangent_3.z;
            tmpvar_28.y = worldBinormal_1.z;
            tmpvar_28.z = tmpvar_21.z;
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_19));
            xlv_TEXCOORD0 = tmpvar_4;
            xlv_TEXCOORD1 = ((tmpvar_7.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_26;
            xlv_TEXCOORD3 = tmpvar_27;
            xlv_TEXCOORD4 = tmpvar_28;
            xlv_TEXCOORD5 = (unity_ObjectToWorld * tmpvar_5).xyz;
            xlv_COLOR0 = _glesColor;
            xlv_TEXCOORD6 = tmpvar_8;
            xlv_TEXCOORD7 = (tmpvar_18 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz));
            xlv_TEXCOORD8 = (unity_WorldToLight * (unity_ObjectToWorld * tmpvar_5));
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _WorldSpaceLightPos0;
          uniform highp mat4 unity_ObjectToWorld;
          uniform lowp vec4 _LightColor0;
          uniform lowp vec4 _SpecColor;
          uniform highp sampler2D _LightTexture0;
          uniform highp sampler2D _LightTextureB0;
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
          uniform highp float _Bevel;
          uniform highp float _BevelOffset;
          uniform highp float _BevelWidth;
          uniform highp float _BevelClamp;
          uniform highp float _BevelRoundness;
          uniform sampler2D _BumpMap;
          uniform highp float _BumpOutline;
          uniform highp float _BumpFace;
          uniform lowp samplerCube _Cube;
          uniform lowp vec4 _ReflectFaceColor;
          uniform lowp vec4 _ReflectOutlineColor;
          uniform highp float _ShaderFlags;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          uniform highp float _TextureWidth;
          uniform highp float _TextureHeight;
          uniform highp float _GradientScale;
          uniform mediump float _FaceShininess;
          uniform mediump float _OutlineShininess;
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          varying highp vec4 xlv_TEXCOORD8;
          void main ()
          {
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp float atten_4;
            lowp vec3 tmpvar_5;
            lowp vec3 tmpvar_6;
            lowp vec3 tmpvar_7;
            lowp float tmpvar_8;
            lowp float tmpvar_9;
            highp vec3 worldViewDir_10;
            lowp vec3 lightDir_11;
            lowp vec3 _unity_tbn_2_12;
            lowp vec3 _unity_tbn_1_13;
            lowp vec3 _unity_tbn_0_14;
            _unity_tbn_0_14 = xlv_TEXCOORD2;
            _unity_tbn_1_13 = xlv_TEXCOORD3;
            _unity_tbn_2_12 = xlv_TEXCOORD4;
            highp vec3 tmpvar_15;
            tmpvar_15 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD5));
            lightDir_11 = tmpvar_15;
            worldViewDir_10 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD5));
            tmpvar_5 = vec3(0.0, 0.0, 0.0);
            tmpvar_7 = vec3(0.0, 0.0, 0.0);
            tmpvar_9 = 0.0;
            tmpvar_8 = 0.0;
            tmpvar_6 = vec3(0.0, 0.0, 1.0);
            lowp vec3 tmpvar_16;
            lowp vec3 tmpvar_17;
            lowp vec3 tmpvar_18;
            lowp float tmpvar_19;
            lowp float tmpvar_20;
            tmpvar_16 = tmpvar_5;
            tmpvar_17 = tmpvar_6;
            tmpvar_18 = tmpvar_7;
            tmpvar_19 = tmpvar_8;
            tmpvar_20 = tmpvar_9;
            highp vec3 bump_21;
            highp vec4 outlineColor_22;
            highp vec4 faceColor_23;
            highp float c_24;
            highp vec4 smp4x_25;
            highp vec3 tmpvar_26;
            tmpvar_26.z = 0.0;
            tmpvar_26.x = (1.0/(_TextureWidth));
            tmpvar_26.y = (1.0/(_TextureHeight));
            highp vec2 P_27;
            P_27 = (xlv_TEXCOORD0.xy - tmpvar_26.xz);
            highp vec2 P_28;
            P_28 = (xlv_TEXCOORD0.xy + tmpvar_26.xz);
            highp vec2 P_29;
            P_29 = (xlv_TEXCOORD0.xy - tmpvar_26.zy);
            highp vec2 P_30;
            P_30 = (xlv_TEXCOORD0.xy + tmpvar_26.zy);
            lowp vec4 tmpvar_31;
            tmpvar_31.x = texture2D (_MainTex, P_27).w;
            tmpvar_31.y = texture2D (_MainTex, P_28).w;
            tmpvar_31.z = texture2D (_MainTex, P_29).w;
            tmpvar_31.w = texture2D (_MainTex, P_30).w;
            smp4x_25 = tmpvar_31;
            lowp float tmpvar_32;
            tmpvar_32 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_24 = tmpvar_32;
            highp float tmpvar_33;
            tmpvar_33 = (((
              (0.5 - c_24)
             - xlv_TEXCOORD6.x) * xlv_TEXCOORD6.y) + 0.5);
            highp float tmpvar_34;
            tmpvar_34 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD6.y);
            highp float tmpvar_35;
            tmpvar_35 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD6.y);
            faceColor_23 = _FaceColor;
            outlineColor_22 = _OutlineColor;
            faceColor_23 = (faceColor_23 * xlv_COLOR0);
            outlineColor_22.w = (outlineColor_22.w * xlv_COLOR0.w);
            highp vec2 tmpvar_36;
            tmpvar_36.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_36.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_37;
            tmpvar_37 = texture2D (_FaceTex, tmpvar_36);
            faceColor_23 = (faceColor_23 * tmpvar_37);
            highp vec2 tmpvar_38;
            tmpvar_38.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_38.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_39;
            tmpvar_39 = texture2D (_OutlineTex, tmpvar_38);
            outlineColor_22 = (outlineColor_22 * tmpvar_39);
            mediump float d_40;
            d_40 = tmpvar_33;
            lowp vec4 faceColor_41;
            faceColor_41 = faceColor_23;
            lowp vec4 outlineColor_42;
            outlineColor_42 = outlineColor_22;
            mediump float outline_43;
            outline_43 = tmpvar_34;
            mediump float softness_44;
            softness_44 = tmpvar_35;
            mediump float tmpvar_45;
            tmpvar_45 = (1.0 - clamp ((
              ((d_40 - (outline_43 * 0.5)) + (softness_44 * 0.5))
             / 
              (1.0 + softness_44)
            ), 0.0, 1.0));
            faceColor_41.xyz = (faceColor_41.xyz * faceColor_41.w);
            outlineColor_42.xyz = (outlineColor_42.xyz * outlineColor_42.w);
            mediump vec4 tmpvar_46;
            tmpvar_46 = mix (faceColor_41, outlineColor_42, vec4((clamp (
              (d_40 + (outline_43 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_43)
            ))));
            faceColor_41 = tmpvar_46;
            faceColor_41 = (faceColor_41 * tmpvar_45);
            faceColor_23 = faceColor_41;
            faceColor_23.xyz = (faceColor_23.xyz / max (faceColor_23.w, 0.0001));
            highp vec4 h_47;
            h_47 = smp4x_25;
            highp float tmpvar_48;
            tmpvar_48 = (_ShaderFlags / 2.0);
            highp float tmpvar_49;
            tmpvar_49 = (fract(abs(tmpvar_48)) * 2.0);
            highp float tmpvar_50;
            if ((tmpvar_48 >= 0.0)) {
              tmpvar_50 = tmpvar_49;
            } else {
              tmpvar_50 = -(tmpvar_49);
            };
            h_47 = (smp4x_25 + (xlv_TEXCOORD6.x + _BevelOffset));
            highp float tmpvar_51;
            tmpvar_51 = max (0.01, (_OutlineWidth + _BevelWidth));
            h_47 = (h_47 - 0.5);
            h_47 = (h_47 / tmpvar_51);
            highp vec4 tmpvar_52;
            tmpvar_52 = clamp ((h_47 + 0.5), 0.0, 1.0);
            h_47 = tmpvar_52;
            if (bool(float((tmpvar_50 >= 1.0)))) {
              h_47 = (1.0 - abs((
                (tmpvar_52 * 2.0)
               - 1.0)));
            };
            h_47 = (min (mix (h_47, 
              sin(((h_47 * 3.141592) / 2.0))
            , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((_Bevel * tmpvar_51) * (_GradientScale * -2.0)));
            highp vec3 tmpvar_53;
            tmpvar_53.xy = vec2(1.0, 0.0);
            tmpvar_53.z = (h_47.y - h_47.x);
            highp vec3 tmpvar_54;
            tmpvar_54 = normalize(tmpvar_53);
            highp vec3 tmpvar_55;
            tmpvar_55.xy = vec2(0.0, -1.0);
            tmpvar_55.z = (h_47.w - h_47.z);
            highp vec3 tmpvar_56;
            tmpvar_56 = normalize(tmpvar_55);
            lowp vec3 tmpvar_57;
            tmpvar_57 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
            bump_21 = tmpvar_57;
            bump_21 = (bump_21 * mix (_BumpFace, _BumpOutline, clamp (
              (tmpvar_33 + (tmpvar_34 * 0.5))
            , 0.0, 1.0)));
            highp vec3 tmpvar_58;
            tmpvar_58 = mix (vec3(0.0, 0.0, 1.0), bump_21, faceColor_23.www);
            bump_21 = tmpvar_58;
            highp vec3 tmpvar_59;
            tmpvar_59 = normalize(((
              (tmpvar_54.yzx * tmpvar_56.zxy)
             - 
              (tmpvar_54.zxy * tmpvar_56.yzx)
            ) - tmpvar_58));
            highp mat3 tmpvar_60;
            tmpvar_60[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_60[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_60[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_61;
            highp vec3 N_62;
            N_62 = (tmpvar_60 * tmpvar_59);
            tmpvar_61 = (xlv_TEXCOORD7 - (2.0 * (
              dot (N_62, xlv_TEXCOORD7)
             * N_62)));
            lowp vec4 tmpvar_63;
            tmpvar_63 = textureCube (_Cube, tmpvar_61);
            highp float tmpvar_64;
            tmpvar_64 = clamp ((tmpvar_33 + (tmpvar_34 * 0.5)), 0.0, 1.0);
            lowp vec3 tmpvar_65;
            tmpvar_65 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_64));
            highp vec3 tmpvar_66;
            tmpvar_66 = ((tmpvar_63.xyz * tmpvar_65) * faceColor_23.w);
            tmpvar_16 = faceColor_23.xyz;
            tmpvar_17 = -(tmpvar_59);
            tmpvar_18 = tmpvar_66;
            highp float tmpvar_67;
            tmpvar_67 = clamp ((tmpvar_33 + (tmpvar_34 * 0.5)), 0.0, 1.0);
            tmpvar_19 = 1.0;
            tmpvar_20 = faceColor_23.w;
            tmpvar_5 = tmpvar_16;
            tmpvar_7 = tmpvar_18;
            tmpvar_8 = tmpvar_19;
            tmpvar_9 = tmpvar_20;
            lowp float tmpvar_68;
            highp vec4 tmpvar_69;
            tmpvar_69 = texture2D (_LightTexture0, ((xlv_TEXCOORD8.xy / xlv_TEXCOORD8.w) + 0.5));
            tmpvar_68 = tmpvar_69.w;
            lowp float tmpvar_70;
            highp vec4 tmpvar_71;
            tmpvar_71 = texture2D (_LightTextureB0, vec2(dot (xlv_TEXCOORD8.xyz, xlv_TEXCOORD8.xyz)));
            tmpvar_70 = tmpvar_71.x;
            highp float tmpvar_72;
            tmpvar_72 = ((float(
              (xlv_TEXCOORD8.z > 0.0)
            ) * tmpvar_68) * tmpvar_70);
            atten_4 = tmpvar_72;
            lowp float tmpvar_73;
            tmpvar_73 = dot (_unity_tbn_0_14, tmpvar_17);
            worldN_3.x = tmpvar_73;
            lowp float tmpvar_74;
            tmpvar_74 = dot (_unity_tbn_1_13, tmpvar_17);
            worldN_3.y = tmpvar_74;
            lowp float tmpvar_75;
            tmpvar_75 = dot (_unity_tbn_2_12, tmpvar_17);
            worldN_3.z = tmpvar_75;
            highp vec3 tmpvar_76;
            tmpvar_76 = normalize(worldN_3);
            worldN_3 = tmpvar_76;
            tmpvar_6 = tmpvar_76;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_11;
            tmpvar_1 = (tmpvar_1 * atten_4);
            mediump vec3 viewDir_77;
            viewDir_77 = worldViewDir_10;
            lowp vec4 c_78;
            lowp vec4 c_79;
            highp float nh_80;
            lowp float diff_81;
            mediump float tmpvar_82;
            tmpvar_82 = max (0.0, dot (tmpvar_6, tmpvar_2));
            diff_81 = tmpvar_82;
            mediump float tmpvar_83;
            tmpvar_83 = max (0.0, dot (tmpvar_6, normalize(
              (tmpvar_2 + viewDir_77)
            )));
            nh_80 = tmpvar_83;
            mediump float y_84;
            y_84 = (mix (_FaceShininess, _OutlineShininess, tmpvar_67) * 128.0);
            highp float tmpvar_85;
            tmpvar_85 = pow (nh_80, y_84);
            c_79.xyz = (((tmpvar_16 * tmpvar_1) * diff_81) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_85));
            c_79.w = tmpvar_20;
            c_78.w = c_79.w;
            c_78.xyz = c_79.xyz;
            gl_FragData[0] = c_78;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier02"
        {
           Keywords { "SPOT" }
          
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
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp vec4 unity_WorldTransformParams;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp mat4 unity_WorldToLight;
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
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          varying highp vec4 xlv_TEXCOORD8;
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
            highp mat3 tmpvar_20;
            tmpvar_20[0] = unity_WorldToObject[0].xyz;
            tmpvar_20[1] = unity_WorldToObject[1].xyz;
            tmpvar_20[2] = unity_WorldToObject[2].xyz;
            highp vec3 tmpvar_21;
            tmpvar_21 = normalize((tmpvar_6 * tmpvar_20));
            highp mat3 tmpvar_22;
            tmpvar_22[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_22[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_22[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_23;
            tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.xyz));
            worldTangent_3 = tmpvar_23;
            highp float tmpvar_24;
            tmpvar_24 = (_glesTANGENT.w * unity_WorldTransformParams.w);
            tangentSign_2 = tmpvar_24;
            highp vec3 tmpvar_25;
            tmpvar_25 = (((tmpvar_21.yzx * worldTangent_3.zxy) - (tmpvar_21.zxy * worldTangent_3.yzx)) * tangentSign_2);
            worldBinormal_1 = tmpvar_25;
            highp vec3 tmpvar_26;
            tmpvar_26.x = worldTangent_3.x;
            tmpvar_26.y = worldBinormal_1.x;
            tmpvar_26.z = tmpvar_21.x;
            highp vec3 tmpvar_27;
            tmpvar_27.x = worldTangent_3.y;
            tmpvar_27.y = worldBinormal_1.y;
            tmpvar_27.z = tmpvar_21.y;
            highp vec3 tmpvar_28;
            tmpvar_28.x = worldTangent_3.z;
            tmpvar_28.y = worldBinormal_1.z;
            tmpvar_28.z = tmpvar_21.z;
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_19));
            xlv_TEXCOORD0 = tmpvar_4;
            xlv_TEXCOORD1 = ((tmpvar_7.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_26;
            xlv_TEXCOORD3 = tmpvar_27;
            xlv_TEXCOORD4 = tmpvar_28;
            xlv_TEXCOORD5 = (unity_ObjectToWorld * tmpvar_5).xyz;
            xlv_COLOR0 = _glesColor;
            xlv_TEXCOORD6 = tmpvar_8;
            xlv_TEXCOORD7 = (tmpvar_18 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz));
            xlv_TEXCOORD8 = (unity_WorldToLight * (unity_ObjectToWorld * tmpvar_5));
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _WorldSpaceLightPos0;
          uniform highp mat4 unity_ObjectToWorld;
          uniform lowp vec4 _LightColor0;
          uniform lowp vec4 _SpecColor;
          uniform highp sampler2D _LightTexture0;
          uniform highp sampler2D _LightTextureB0;
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
          uniform highp float _Bevel;
          uniform highp float _BevelOffset;
          uniform highp float _BevelWidth;
          uniform highp float _BevelClamp;
          uniform highp float _BevelRoundness;
          uniform sampler2D _BumpMap;
          uniform highp float _BumpOutline;
          uniform highp float _BumpFace;
          uniform lowp samplerCube _Cube;
          uniform lowp vec4 _ReflectFaceColor;
          uniform lowp vec4 _ReflectOutlineColor;
          uniform highp float _ShaderFlags;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          uniform highp float _TextureWidth;
          uniform highp float _TextureHeight;
          uniform highp float _GradientScale;
          uniform mediump float _FaceShininess;
          uniform mediump float _OutlineShininess;
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          varying highp vec4 xlv_TEXCOORD8;
          void main ()
          {
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp float atten_4;
            lowp vec3 tmpvar_5;
            lowp vec3 tmpvar_6;
            lowp vec3 tmpvar_7;
            lowp float tmpvar_8;
            lowp float tmpvar_9;
            highp vec3 worldViewDir_10;
            lowp vec3 lightDir_11;
            lowp vec3 _unity_tbn_2_12;
            lowp vec3 _unity_tbn_1_13;
            lowp vec3 _unity_tbn_0_14;
            _unity_tbn_0_14 = xlv_TEXCOORD2;
            _unity_tbn_1_13 = xlv_TEXCOORD3;
            _unity_tbn_2_12 = xlv_TEXCOORD4;
            highp vec3 tmpvar_15;
            tmpvar_15 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD5));
            lightDir_11 = tmpvar_15;
            worldViewDir_10 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD5));
            tmpvar_5 = vec3(0.0, 0.0, 0.0);
            tmpvar_7 = vec3(0.0, 0.0, 0.0);
            tmpvar_9 = 0.0;
            tmpvar_8 = 0.0;
            tmpvar_6 = vec3(0.0, 0.0, 1.0);
            lowp vec3 tmpvar_16;
            lowp vec3 tmpvar_17;
            lowp vec3 tmpvar_18;
            lowp float tmpvar_19;
            lowp float tmpvar_20;
            tmpvar_16 = tmpvar_5;
            tmpvar_17 = tmpvar_6;
            tmpvar_18 = tmpvar_7;
            tmpvar_19 = tmpvar_8;
            tmpvar_20 = tmpvar_9;
            highp vec3 bump_21;
            highp vec4 outlineColor_22;
            highp vec4 faceColor_23;
            highp float c_24;
            highp vec4 smp4x_25;
            highp vec3 tmpvar_26;
            tmpvar_26.z = 0.0;
            tmpvar_26.x = (1.0/(_TextureWidth));
            tmpvar_26.y = (1.0/(_TextureHeight));
            highp vec2 P_27;
            P_27 = (xlv_TEXCOORD0.xy - tmpvar_26.xz);
            highp vec2 P_28;
            P_28 = (xlv_TEXCOORD0.xy + tmpvar_26.xz);
            highp vec2 P_29;
            P_29 = (xlv_TEXCOORD0.xy - tmpvar_26.zy);
            highp vec2 P_30;
            P_30 = (xlv_TEXCOORD0.xy + tmpvar_26.zy);
            lowp vec4 tmpvar_31;
            tmpvar_31.x = texture2D (_MainTex, P_27).w;
            tmpvar_31.y = texture2D (_MainTex, P_28).w;
            tmpvar_31.z = texture2D (_MainTex, P_29).w;
            tmpvar_31.w = texture2D (_MainTex, P_30).w;
            smp4x_25 = tmpvar_31;
            lowp float tmpvar_32;
            tmpvar_32 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_24 = tmpvar_32;
            highp float tmpvar_33;
            tmpvar_33 = (((
              (0.5 - c_24)
             - xlv_TEXCOORD6.x) * xlv_TEXCOORD6.y) + 0.5);
            highp float tmpvar_34;
            tmpvar_34 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD6.y);
            highp float tmpvar_35;
            tmpvar_35 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD6.y);
            faceColor_23 = _FaceColor;
            outlineColor_22 = _OutlineColor;
            faceColor_23 = (faceColor_23 * xlv_COLOR0);
            outlineColor_22.w = (outlineColor_22.w * xlv_COLOR0.w);
            highp vec2 tmpvar_36;
            tmpvar_36.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_36.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_37;
            tmpvar_37 = texture2D (_FaceTex, tmpvar_36);
            faceColor_23 = (faceColor_23 * tmpvar_37);
            highp vec2 tmpvar_38;
            tmpvar_38.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_38.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_39;
            tmpvar_39 = texture2D (_OutlineTex, tmpvar_38);
            outlineColor_22 = (outlineColor_22 * tmpvar_39);
            mediump float d_40;
            d_40 = tmpvar_33;
            lowp vec4 faceColor_41;
            faceColor_41 = faceColor_23;
            lowp vec4 outlineColor_42;
            outlineColor_42 = outlineColor_22;
            mediump float outline_43;
            outline_43 = tmpvar_34;
            mediump float softness_44;
            softness_44 = tmpvar_35;
            mediump float tmpvar_45;
            tmpvar_45 = (1.0 - clamp ((
              ((d_40 - (outline_43 * 0.5)) + (softness_44 * 0.5))
             / 
              (1.0 + softness_44)
            ), 0.0, 1.0));
            faceColor_41.xyz = (faceColor_41.xyz * faceColor_41.w);
            outlineColor_42.xyz = (outlineColor_42.xyz * outlineColor_42.w);
            mediump vec4 tmpvar_46;
            tmpvar_46 = mix (faceColor_41, outlineColor_42, vec4((clamp (
              (d_40 + (outline_43 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_43)
            ))));
            faceColor_41 = tmpvar_46;
            faceColor_41 = (faceColor_41 * tmpvar_45);
            faceColor_23 = faceColor_41;
            faceColor_23.xyz = (faceColor_23.xyz / max (faceColor_23.w, 0.0001));
            highp vec4 h_47;
            h_47 = smp4x_25;
            highp float tmpvar_48;
            tmpvar_48 = (_ShaderFlags / 2.0);
            highp float tmpvar_49;
            tmpvar_49 = (fract(abs(tmpvar_48)) * 2.0);
            highp float tmpvar_50;
            if ((tmpvar_48 >= 0.0)) {
              tmpvar_50 = tmpvar_49;
            } else {
              tmpvar_50 = -(tmpvar_49);
            };
            h_47 = (smp4x_25 + (xlv_TEXCOORD6.x + _BevelOffset));
            highp float tmpvar_51;
            tmpvar_51 = max (0.01, (_OutlineWidth + _BevelWidth));
            h_47 = (h_47 - 0.5);
            h_47 = (h_47 / tmpvar_51);
            highp vec4 tmpvar_52;
            tmpvar_52 = clamp ((h_47 + 0.5), 0.0, 1.0);
            h_47 = tmpvar_52;
            if (bool(float((tmpvar_50 >= 1.0)))) {
              h_47 = (1.0 - abs((
                (tmpvar_52 * 2.0)
               - 1.0)));
            };
            h_47 = (min (mix (h_47, 
              sin(((h_47 * 3.141592) / 2.0))
            , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((_Bevel * tmpvar_51) * (_GradientScale * -2.0)));
            highp vec3 tmpvar_53;
            tmpvar_53.xy = vec2(1.0, 0.0);
            tmpvar_53.z = (h_47.y - h_47.x);
            highp vec3 tmpvar_54;
            tmpvar_54 = normalize(tmpvar_53);
            highp vec3 tmpvar_55;
            tmpvar_55.xy = vec2(0.0, -1.0);
            tmpvar_55.z = (h_47.w - h_47.z);
            highp vec3 tmpvar_56;
            tmpvar_56 = normalize(tmpvar_55);
            lowp vec3 tmpvar_57;
            tmpvar_57 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
            bump_21 = tmpvar_57;
            bump_21 = (bump_21 * mix (_BumpFace, _BumpOutline, clamp (
              (tmpvar_33 + (tmpvar_34 * 0.5))
            , 0.0, 1.0)));
            highp vec3 tmpvar_58;
            tmpvar_58 = mix (vec3(0.0, 0.0, 1.0), bump_21, faceColor_23.www);
            bump_21 = tmpvar_58;
            highp vec3 tmpvar_59;
            tmpvar_59 = normalize(((
              (tmpvar_54.yzx * tmpvar_56.zxy)
             - 
              (tmpvar_54.zxy * tmpvar_56.yzx)
            ) - tmpvar_58));
            highp mat3 tmpvar_60;
            tmpvar_60[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_60[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_60[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_61;
            highp vec3 N_62;
            N_62 = (tmpvar_60 * tmpvar_59);
            tmpvar_61 = (xlv_TEXCOORD7 - (2.0 * (
              dot (N_62, xlv_TEXCOORD7)
             * N_62)));
            lowp vec4 tmpvar_63;
            tmpvar_63 = textureCube (_Cube, tmpvar_61);
            highp float tmpvar_64;
            tmpvar_64 = clamp ((tmpvar_33 + (tmpvar_34 * 0.5)), 0.0, 1.0);
            lowp vec3 tmpvar_65;
            tmpvar_65 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_64));
            highp vec3 tmpvar_66;
            tmpvar_66 = ((tmpvar_63.xyz * tmpvar_65) * faceColor_23.w);
            tmpvar_16 = faceColor_23.xyz;
            tmpvar_17 = -(tmpvar_59);
            tmpvar_18 = tmpvar_66;
            highp float tmpvar_67;
            tmpvar_67 = clamp ((tmpvar_33 + (tmpvar_34 * 0.5)), 0.0, 1.0);
            tmpvar_19 = 1.0;
            tmpvar_20 = faceColor_23.w;
            tmpvar_5 = tmpvar_16;
            tmpvar_7 = tmpvar_18;
            tmpvar_8 = tmpvar_19;
            tmpvar_9 = tmpvar_20;
            lowp float tmpvar_68;
            highp vec4 tmpvar_69;
            tmpvar_69 = texture2D (_LightTexture0, ((xlv_TEXCOORD8.xy / xlv_TEXCOORD8.w) + 0.5));
            tmpvar_68 = tmpvar_69.w;
            lowp float tmpvar_70;
            highp vec4 tmpvar_71;
            tmpvar_71 = texture2D (_LightTextureB0, vec2(dot (xlv_TEXCOORD8.xyz, xlv_TEXCOORD8.xyz)));
            tmpvar_70 = tmpvar_71.x;
            highp float tmpvar_72;
            tmpvar_72 = ((float(
              (xlv_TEXCOORD8.z > 0.0)
            ) * tmpvar_68) * tmpvar_70);
            atten_4 = tmpvar_72;
            lowp float tmpvar_73;
            tmpvar_73 = dot (_unity_tbn_0_14, tmpvar_17);
            worldN_3.x = tmpvar_73;
            lowp float tmpvar_74;
            tmpvar_74 = dot (_unity_tbn_1_13, tmpvar_17);
            worldN_3.y = tmpvar_74;
            lowp float tmpvar_75;
            tmpvar_75 = dot (_unity_tbn_2_12, tmpvar_17);
            worldN_3.z = tmpvar_75;
            highp vec3 tmpvar_76;
            tmpvar_76 = normalize(worldN_3);
            worldN_3 = tmpvar_76;
            tmpvar_6 = tmpvar_76;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_11;
            tmpvar_1 = (tmpvar_1 * atten_4);
            mediump vec3 viewDir_77;
            viewDir_77 = worldViewDir_10;
            lowp vec4 c_78;
            lowp vec4 c_79;
            highp float nh_80;
            lowp float diff_81;
            mediump float tmpvar_82;
            tmpvar_82 = max (0.0, dot (tmpvar_6, tmpvar_2));
            diff_81 = tmpvar_82;
            mediump float tmpvar_83;
            tmpvar_83 = max (0.0, dot (tmpvar_6, normalize(
              (tmpvar_2 + viewDir_77)
            )));
            nh_80 = tmpvar_83;
            mediump float y_84;
            y_84 = (mix (_FaceShininess, _OutlineShininess, tmpvar_67) * 128.0);
            highp float tmpvar_85;
            tmpvar_85 = pow (nh_80, y_84);
            c_79.xyz = (((tmpvar_16 * tmpvar_1) * diff_81) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_85));
            c_79.w = tmpvar_20;
            c_78.w = c_79.w;
            c_78.xyz = c_79.xyz;
            gl_FragData[0] = c_78;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier00"
        {
           Keywords { "POINT_COOKIE" }
          
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
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp vec4 unity_WorldTransformParams;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp mat4 unity_WorldToLight;
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
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          varying highp vec3 xlv_TEXCOORD8;
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
            highp mat3 tmpvar_20;
            tmpvar_20[0] = unity_WorldToObject[0].xyz;
            tmpvar_20[1] = unity_WorldToObject[1].xyz;
            tmpvar_20[2] = unity_WorldToObject[2].xyz;
            highp vec3 tmpvar_21;
            tmpvar_21 = normalize((tmpvar_6 * tmpvar_20));
            highp mat3 tmpvar_22;
            tmpvar_22[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_22[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_22[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_23;
            tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.xyz));
            worldTangent_3 = tmpvar_23;
            highp float tmpvar_24;
            tmpvar_24 = (_glesTANGENT.w * unity_WorldTransformParams.w);
            tangentSign_2 = tmpvar_24;
            highp vec3 tmpvar_25;
            tmpvar_25 = (((tmpvar_21.yzx * worldTangent_3.zxy) - (tmpvar_21.zxy * worldTangent_3.yzx)) * tangentSign_2);
            worldBinormal_1 = tmpvar_25;
            highp vec3 tmpvar_26;
            tmpvar_26.x = worldTangent_3.x;
            tmpvar_26.y = worldBinormal_1.x;
            tmpvar_26.z = tmpvar_21.x;
            highp vec3 tmpvar_27;
            tmpvar_27.x = worldTangent_3.y;
            tmpvar_27.y = worldBinormal_1.y;
            tmpvar_27.z = tmpvar_21.y;
            highp vec3 tmpvar_28;
            tmpvar_28.x = worldTangent_3.z;
            tmpvar_28.y = worldBinormal_1.z;
            tmpvar_28.z = tmpvar_21.z;
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_19));
            xlv_TEXCOORD0 = tmpvar_4;
            xlv_TEXCOORD1 = ((tmpvar_7.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_26;
            xlv_TEXCOORD3 = tmpvar_27;
            xlv_TEXCOORD4 = tmpvar_28;
            xlv_TEXCOORD5 = (unity_ObjectToWorld * tmpvar_5).xyz;
            xlv_COLOR0 = _glesColor;
            xlv_TEXCOORD6 = tmpvar_8;
            xlv_TEXCOORD7 = (tmpvar_18 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz));
            xlv_TEXCOORD8 = (unity_WorldToLight * (unity_ObjectToWorld * tmpvar_5)).xyz;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _WorldSpaceLightPos0;
          uniform highp mat4 unity_ObjectToWorld;
          uniform lowp vec4 _LightColor0;
          uniform lowp vec4 _SpecColor;
          uniform highp samplerCube _LightTexture0;
          uniform highp sampler2D _LightTextureB0;
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
          uniform highp float _Bevel;
          uniform highp float _BevelOffset;
          uniform highp float _BevelWidth;
          uniform highp float _BevelClamp;
          uniform highp float _BevelRoundness;
          uniform sampler2D _BumpMap;
          uniform highp float _BumpOutline;
          uniform highp float _BumpFace;
          uniform lowp samplerCube _Cube;
          uniform lowp vec4 _ReflectFaceColor;
          uniform lowp vec4 _ReflectOutlineColor;
          uniform highp float _ShaderFlags;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          uniform highp float _TextureWidth;
          uniform highp float _TextureHeight;
          uniform highp float _GradientScale;
          uniform mediump float _FaceShininess;
          uniform mediump float _OutlineShininess;
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          varying highp vec3 xlv_TEXCOORD8;
          void main ()
          {
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp float atten_4;
            lowp vec3 tmpvar_5;
            lowp vec3 tmpvar_6;
            lowp vec3 tmpvar_7;
            lowp float tmpvar_8;
            lowp float tmpvar_9;
            highp vec3 worldViewDir_10;
            lowp vec3 lightDir_11;
            lowp vec3 _unity_tbn_2_12;
            lowp vec3 _unity_tbn_1_13;
            lowp vec3 _unity_tbn_0_14;
            _unity_tbn_0_14 = xlv_TEXCOORD2;
            _unity_tbn_1_13 = xlv_TEXCOORD3;
            _unity_tbn_2_12 = xlv_TEXCOORD4;
            highp vec3 tmpvar_15;
            tmpvar_15 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD5));
            lightDir_11 = tmpvar_15;
            worldViewDir_10 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD5));
            tmpvar_5 = vec3(0.0, 0.0, 0.0);
            tmpvar_7 = vec3(0.0, 0.0, 0.0);
            tmpvar_9 = 0.0;
            tmpvar_8 = 0.0;
            tmpvar_6 = vec3(0.0, 0.0, 1.0);
            lowp vec3 tmpvar_16;
            lowp vec3 tmpvar_17;
            lowp vec3 tmpvar_18;
            lowp float tmpvar_19;
            lowp float tmpvar_20;
            tmpvar_16 = tmpvar_5;
            tmpvar_17 = tmpvar_6;
            tmpvar_18 = tmpvar_7;
            tmpvar_19 = tmpvar_8;
            tmpvar_20 = tmpvar_9;
            highp vec3 bump_21;
            highp vec4 outlineColor_22;
            highp vec4 faceColor_23;
            highp float c_24;
            highp vec4 smp4x_25;
            highp vec3 tmpvar_26;
            tmpvar_26.z = 0.0;
            tmpvar_26.x = (1.0/(_TextureWidth));
            tmpvar_26.y = (1.0/(_TextureHeight));
            highp vec2 P_27;
            P_27 = (xlv_TEXCOORD0.xy - tmpvar_26.xz);
            highp vec2 P_28;
            P_28 = (xlv_TEXCOORD0.xy + tmpvar_26.xz);
            highp vec2 P_29;
            P_29 = (xlv_TEXCOORD0.xy - tmpvar_26.zy);
            highp vec2 P_30;
            P_30 = (xlv_TEXCOORD0.xy + tmpvar_26.zy);
            lowp vec4 tmpvar_31;
            tmpvar_31.x = texture2D (_MainTex, P_27).w;
            tmpvar_31.y = texture2D (_MainTex, P_28).w;
            tmpvar_31.z = texture2D (_MainTex, P_29).w;
            tmpvar_31.w = texture2D (_MainTex, P_30).w;
            smp4x_25 = tmpvar_31;
            lowp float tmpvar_32;
            tmpvar_32 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_24 = tmpvar_32;
            highp float tmpvar_33;
            tmpvar_33 = (((
              (0.5 - c_24)
             - xlv_TEXCOORD6.x) * xlv_TEXCOORD6.y) + 0.5);
            highp float tmpvar_34;
            tmpvar_34 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD6.y);
            highp float tmpvar_35;
            tmpvar_35 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD6.y);
            faceColor_23 = _FaceColor;
            outlineColor_22 = _OutlineColor;
            faceColor_23 = (faceColor_23 * xlv_COLOR0);
            outlineColor_22.w = (outlineColor_22.w * xlv_COLOR0.w);
            highp vec2 tmpvar_36;
            tmpvar_36.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_36.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_37;
            tmpvar_37 = texture2D (_FaceTex, tmpvar_36);
            faceColor_23 = (faceColor_23 * tmpvar_37);
            highp vec2 tmpvar_38;
            tmpvar_38.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_38.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_39;
            tmpvar_39 = texture2D (_OutlineTex, tmpvar_38);
            outlineColor_22 = (outlineColor_22 * tmpvar_39);
            mediump float d_40;
            d_40 = tmpvar_33;
            lowp vec4 faceColor_41;
            faceColor_41 = faceColor_23;
            lowp vec4 outlineColor_42;
            outlineColor_42 = outlineColor_22;
            mediump float outline_43;
            outline_43 = tmpvar_34;
            mediump float softness_44;
            softness_44 = tmpvar_35;
            mediump float tmpvar_45;
            tmpvar_45 = (1.0 - clamp ((
              ((d_40 - (outline_43 * 0.5)) + (softness_44 * 0.5))
             / 
              (1.0 + softness_44)
            ), 0.0, 1.0));
            faceColor_41.xyz = (faceColor_41.xyz * faceColor_41.w);
            outlineColor_42.xyz = (outlineColor_42.xyz * outlineColor_42.w);
            mediump vec4 tmpvar_46;
            tmpvar_46 = mix (faceColor_41, outlineColor_42, vec4((clamp (
              (d_40 + (outline_43 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_43)
            ))));
            faceColor_41 = tmpvar_46;
            faceColor_41 = (faceColor_41 * tmpvar_45);
            faceColor_23 = faceColor_41;
            faceColor_23.xyz = (faceColor_23.xyz / max (faceColor_23.w, 0.0001));
            highp vec4 h_47;
            h_47 = smp4x_25;
            highp float tmpvar_48;
            tmpvar_48 = (_ShaderFlags / 2.0);
            highp float tmpvar_49;
            tmpvar_49 = (fract(abs(tmpvar_48)) * 2.0);
            highp float tmpvar_50;
            if ((tmpvar_48 >= 0.0)) {
              tmpvar_50 = tmpvar_49;
            } else {
              tmpvar_50 = -(tmpvar_49);
            };
            h_47 = (smp4x_25 + (xlv_TEXCOORD6.x + _BevelOffset));
            highp float tmpvar_51;
            tmpvar_51 = max (0.01, (_OutlineWidth + _BevelWidth));
            h_47 = (h_47 - 0.5);
            h_47 = (h_47 / tmpvar_51);
            highp vec4 tmpvar_52;
            tmpvar_52 = clamp ((h_47 + 0.5), 0.0, 1.0);
            h_47 = tmpvar_52;
            if (bool(float((tmpvar_50 >= 1.0)))) {
              h_47 = (1.0 - abs((
                (tmpvar_52 * 2.0)
               - 1.0)));
            };
            h_47 = (min (mix (h_47, 
              sin(((h_47 * 3.141592) / 2.0))
            , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((_Bevel * tmpvar_51) * (_GradientScale * -2.0)));
            highp vec3 tmpvar_53;
            tmpvar_53.xy = vec2(1.0, 0.0);
            tmpvar_53.z = (h_47.y - h_47.x);
            highp vec3 tmpvar_54;
            tmpvar_54 = normalize(tmpvar_53);
            highp vec3 tmpvar_55;
            tmpvar_55.xy = vec2(0.0, -1.0);
            tmpvar_55.z = (h_47.w - h_47.z);
            highp vec3 tmpvar_56;
            tmpvar_56 = normalize(tmpvar_55);
            lowp vec3 tmpvar_57;
            tmpvar_57 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
            bump_21 = tmpvar_57;
            bump_21 = (bump_21 * mix (_BumpFace, _BumpOutline, clamp (
              (tmpvar_33 + (tmpvar_34 * 0.5))
            , 0.0, 1.0)));
            highp vec3 tmpvar_58;
            tmpvar_58 = mix (vec3(0.0, 0.0, 1.0), bump_21, faceColor_23.www);
            bump_21 = tmpvar_58;
            highp vec3 tmpvar_59;
            tmpvar_59 = normalize(((
              (tmpvar_54.yzx * tmpvar_56.zxy)
             - 
              (tmpvar_54.zxy * tmpvar_56.yzx)
            ) - tmpvar_58));
            highp mat3 tmpvar_60;
            tmpvar_60[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_60[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_60[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_61;
            highp vec3 N_62;
            N_62 = (tmpvar_60 * tmpvar_59);
            tmpvar_61 = (xlv_TEXCOORD7 - (2.0 * (
              dot (N_62, xlv_TEXCOORD7)
             * N_62)));
            lowp vec4 tmpvar_63;
            tmpvar_63 = textureCube (_Cube, tmpvar_61);
            highp float tmpvar_64;
            tmpvar_64 = clamp ((tmpvar_33 + (tmpvar_34 * 0.5)), 0.0, 1.0);
            lowp vec3 tmpvar_65;
            tmpvar_65 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_64));
            highp vec3 tmpvar_66;
            tmpvar_66 = ((tmpvar_63.xyz * tmpvar_65) * faceColor_23.w);
            tmpvar_16 = faceColor_23.xyz;
            tmpvar_17 = -(tmpvar_59);
            tmpvar_18 = tmpvar_66;
            highp float tmpvar_67;
            tmpvar_67 = clamp ((tmpvar_33 + (tmpvar_34 * 0.5)), 0.0, 1.0);
            tmpvar_19 = 1.0;
            tmpvar_20 = faceColor_23.w;
            tmpvar_5 = tmpvar_16;
            tmpvar_7 = tmpvar_18;
            tmpvar_8 = tmpvar_19;
            tmpvar_9 = tmpvar_20;
            highp float tmpvar_68;
            tmpvar_68 = (texture2D (_LightTextureB0, vec2(dot (xlv_TEXCOORD8, xlv_TEXCOORD8))).x * textureCube (_LightTexture0, xlv_TEXCOORD8).w);
            atten_4 = tmpvar_68;
            lowp float tmpvar_69;
            tmpvar_69 = dot (_unity_tbn_0_14, tmpvar_17);
            worldN_3.x = tmpvar_69;
            lowp float tmpvar_70;
            tmpvar_70 = dot (_unity_tbn_1_13, tmpvar_17);
            worldN_3.y = tmpvar_70;
            lowp float tmpvar_71;
            tmpvar_71 = dot (_unity_tbn_2_12, tmpvar_17);
            worldN_3.z = tmpvar_71;
            highp vec3 tmpvar_72;
            tmpvar_72 = normalize(worldN_3);
            worldN_3 = tmpvar_72;
            tmpvar_6 = tmpvar_72;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_11;
            tmpvar_1 = (tmpvar_1 * atten_4);
            mediump vec3 viewDir_73;
            viewDir_73 = worldViewDir_10;
            lowp vec4 c_74;
            lowp vec4 c_75;
            highp float nh_76;
            lowp float diff_77;
            mediump float tmpvar_78;
            tmpvar_78 = max (0.0, dot (tmpvar_6, tmpvar_2));
            diff_77 = tmpvar_78;
            mediump float tmpvar_79;
            tmpvar_79 = max (0.0, dot (tmpvar_6, normalize(
              (tmpvar_2 + viewDir_73)
            )));
            nh_76 = tmpvar_79;
            mediump float y_80;
            y_80 = (mix (_FaceShininess, _OutlineShininess, tmpvar_67) * 128.0);
            highp float tmpvar_81;
            tmpvar_81 = pow (nh_76, y_80);
            c_75.xyz = (((tmpvar_16 * tmpvar_1) * diff_77) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_81));
            c_75.w = tmpvar_20;
            c_74.w = c_75.w;
            c_74.xyz = c_75.xyz;
            gl_FragData[0] = c_74;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier01"
        {
           Keywords { "POINT_COOKIE" }
          
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
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp vec4 unity_WorldTransformParams;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp mat4 unity_WorldToLight;
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
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          varying highp vec3 xlv_TEXCOORD8;
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
            highp mat3 tmpvar_20;
            tmpvar_20[0] = unity_WorldToObject[0].xyz;
            tmpvar_20[1] = unity_WorldToObject[1].xyz;
            tmpvar_20[2] = unity_WorldToObject[2].xyz;
            highp vec3 tmpvar_21;
            tmpvar_21 = normalize((tmpvar_6 * tmpvar_20));
            highp mat3 tmpvar_22;
            tmpvar_22[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_22[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_22[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_23;
            tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.xyz));
            worldTangent_3 = tmpvar_23;
            highp float tmpvar_24;
            tmpvar_24 = (_glesTANGENT.w * unity_WorldTransformParams.w);
            tangentSign_2 = tmpvar_24;
            highp vec3 tmpvar_25;
            tmpvar_25 = (((tmpvar_21.yzx * worldTangent_3.zxy) - (tmpvar_21.zxy * worldTangent_3.yzx)) * tangentSign_2);
            worldBinormal_1 = tmpvar_25;
            highp vec3 tmpvar_26;
            tmpvar_26.x = worldTangent_3.x;
            tmpvar_26.y = worldBinormal_1.x;
            tmpvar_26.z = tmpvar_21.x;
            highp vec3 tmpvar_27;
            tmpvar_27.x = worldTangent_3.y;
            tmpvar_27.y = worldBinormal_1.y;
            tmpvar_27.z = tmpvar_21.y;
            highp vec3 tmpvar_28;
            tmpvar_28.x = worldTangent_3.z;
            tmpvar_28.y = worldBinormal_1.z;
            tmpvar_28.z = tmpvar_21.z;
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_19));
            xlv_TEXCOORD0 = tmpvar_4;
            xlv_TEXCOORD1 = ((tmpvar_7.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_26;
            xlv_TEXCOORD3 = tmpvar_27;
            xlv_TEXCOORD4 = tmpvar_28;
            xlv_TEXCOORD5 = (unity_ObjectToWorld * tmpvar_5).xyz;
            xlv_COLOR0 = _glesColor;
            xlv_TEXCOORD6 = tmpvar_8;
            xlv_TEXCOORD7 = (tmpvar_18 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz));
            xlv_TEXCOORD8 = (unity_WorldToLight * (unity_ObjectToWorld * tmpvar_5)).xyz;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _WorldSpaceLightPos0;
          uniform highp mat4 unity_ObjectToWorld;
          uniform lowp vec4 _LightColor0;
          uniform lowp vec4 _SpecColor;
          uniform highp samplerCube _LightTexture0;
          uniform highp sampler2D _LightTextureB0;
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
          uniform highp float _Bevel;
          uniform highp float _BevelOffset;
          uniform highp float _BevelWidth;
          uniform highp float _BevelClamp;
          uniform highp float _BevelRoundness;
          uniform sampler2D _BumpMap;
          uniform highp float _BumpOutline;
          uniform highp float _BumpFace;
          uniform lowp samplerCube _Cube;
          uniform lowp vec4 _ReflectFaceColor;
          uniform lowp vec4 _ReflectOutlineColor;
          uniform highp float _ShaderFlags;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          uniform highp float _TextureWidth;
          uniform highp float _TextureHeight;
          uniform highp float _GradientScale;
          uniform mediump float _FaceShininess;
          uniform mediump float _OutlineShininess;
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          varying highp vec3 xlv_TEXCOORD8;
          void main ()
          {
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp float atten_4;
            lowp vec3 tmpvar_5;
            lowp vec3 tmpvar_6;
            lowp vec3 tmpvar_7;
            lowp float tmpvar_8;
            lowp float tmpvar_9;
            highp vec3 worldViewDir_10;
            lowp vec3 lightDir_11;
            lowp vec3 _unity_tbn_2_12;
            lowp vec3 _unity_tbn_1_13;
            lowp vec3 _unity_tbn_0_14;
            _unity_tbn_0_14 = xlv_TEXCOORD2;
            _unity_tbn_1_13 = xlv_TEXCOORD3;
            _unity_tbn_2_12 = xlv_TEXCOORD4;
            highp vec3 tmpvar_15;
            tmpvar_15 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD5));
            lightDir_11 = tmpvar_15;
            worldViewDir_10 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD5));
            tmpvar_5 = vec3(0.0, 0.0, 0.0);
            tmpvar_7 = vec3(0.0, 0.0, 0.0);
            tmpvar_9 = 0.0;
            tmpvar_8 = 0.0;
            tmpvar_6 = vec3(0.0, 0.0, 1.0);
            lowp vec3 tmpvar_16;
            lowp vec3 tmpvar_17;
            lowp vec3 tmpvar_18;
            lowp float tmpvar_19;
            lowp float tmpvar_20;
            tmpvar_16 = tmpvar_5;
            tmpvar_17 = tmpvar_6;
            tmpvar_18 = tmpvar_7;
            tmpvar_19 = tmpvar_8;
            tmpvar_20 = tmpvar_9;
            highp vec3 bump_21;
            highp vec4 outlineColor_22;
            highp vec4 faceColor_23;
            highp float c_24;
            highp vec4 smp4x_25;
            highp vec3 tmpvar_26;
            tmpvar_26.z = 0.0;
            tmpvar_26.x = (1.0/(_TextureWidth));
            tmpvar_26.y = (1.0/(_TextureHeight));
            highp vec2 P_27;
            P_27 = (xlv_TEXCOORD0.xy - tmpvar_26.xz);
            highp vec2 P_28;
            P_28 = (xlv_TEXCOORD0.xy + tmpvar_26.xz);
            highp vec2 P_29;
            P_29 = (xlv_TEXCOORD0.xy - tmpvar_26.zy);
            highp vec2 P_30;
            P_30 = (xlv_TEXCOORD0.xy + tmpvar_26.zy);
            lowp vec4 tmpvar_31;
            tmpvar_31.x = texture2D (_MainTex, P_27).w;
            tmpvar_31.y = texture2D (_MainTex, P_28).w;
            tmpvar_31.z = texture2D (_MainTex, P_29).w;
            tmpvar_31.w = texture2D (_MainTex, P_30).w;
            smp4x_25 = tmpvar_31;
            lowp float tmpvar_32;
            tmpvar_32 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_24 = tmpvar_32;
            highp float tmpvar_33;
            tmpvar_33 = (((
              (0.5 - c_24)
             - xlv_TEXCOORD6.x) * xlv_TEXCOORD6.y) + 0.5);
            highp float tmpvar_34;
            tmpvar_34 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD6.y);
            highp float tmpvar_35;
            tmpvar_35 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD6.y);
            faceColor_23 = _FaceColor;
            outlineColor_22 = _OutlineColor;
            faceColor_23 = (faceColor_23 * xlv_COLOR0);
            outlineColor_22.w = (outlineColor_22.w * xlv_COLOR0.w);
            highp vec2 tmpvar_36;
            tmpvar_36.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_36.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_37;
            tmpvar_37 = texture2D (_FaceTex, tmpvar_36);
            faceColor_23 = (faceColor_23 * tmpvar_37);
            highp vec2 tmpvar_38;
            tmpvar_38.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_38.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_39;
            tmpvar_39 = texture2D (_OutlineTex, tmpvar_38);
            outlineColor_22 = (outlineColor_22 * tmpvar_39);
            mediump float d_40;
            d_40 = tmpvar_33;
            lowp vec4 faceColor_41;
            faceColor_41 = faceColor_23;
            lowp vec4 outlineColor_42;
            outlineColor_42 = outlineColor_22;
            mediump float outline_43;
            outline_43 = tmpvar_34;
            mediump float softness_44;
            softness_44 = tmpvar_35;
            mediump float tmpvar_45;
            tmpvar_45 = (1.0 - clamp ((
              ((d_40 - (outline_43 * 0.5)) + (softness_44 * 0.5))
             / 
              (1.0 + softness_44)
            ), 0.0, 1.0));
            faceColor_41.xyz = (faceColor_41.xyz * faceColor_41.w);
            outlineColor_42.xyz = (outlineColor_42.xyz * outlineColor_42.w);
            mediump vec4 tmpvar_46;
            tmpvar_46 = mix (faceColor_41, outlineColor_42, vec4((clamp (
              (d_40 + (outline_43 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_43)
            ))));
            faceColor_41 = tmpvar_46;
            faceColor_41 = (faceColor_41 * tmpvar_45);
            faceColor_23 = faceColor_41;
            faceColor_23.xyz = (faceColor_23.xyz / max (faceColor_23.w, 0.0001));
            highp vec4 h_47;
            h_47 = smp4x_25;
            highp float tmpvar_48;
            tmpvar_48 = (_ShaderFlags / 2.0);
            highp float tmpvar_49;
            tmpvar_49 = (fract(abs(tmpvar_48)) * 2.0);
            highp float tmpvar_50;
            if ((tmpvar_48 >= 0.0)) {
              tmpvar_50 = tmpvar_49;
            } else {
              tmpvar_50 = -(tmpvar_49);
            };
            h_47 = (smp4x_25 + (xlv_TEXCOORD6.x + _BevelOffset));
            highp float tmpvar_51;
            tmpvar_51 = max (0.01, (_OutlineWidth + _BevelWidth));
            h_47 = (h_47 - 0.5);
            h_47 = (h_47 / tmpvar_51);
            highp vec4 tmpvar_52;
            tmpvar_52 = clamp ((h_47 + 0.5), 0.0, 1.0);
            h_47 = tmpvar_52;
            if (bool(float((tmpvar_50 >= 1.0)))) {
              h_47 = (1.0 - abs((
                (tmpvar_52 * 2.0)
               - 1.0)));
            };
            h_47 = (min (mix (h_47, 
              sin(((h_47 * 3.141592) / 2.0))
            , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((_Bevel * tmpvar_51) * (_GradientScale * -2.0)));
            highp vec3 tmpvar_53;
            tmpvar_53.xy = vec2(1.0, 0.0);
            tmpvar_53.z = (h_47.y - h_47.x);
            highp vec3 tmpvar_54;
            tmpvar_54 = normalize(tmpvar_53);
            highp vec3 tmpvar_55;
            tmpvar_55.xy = vec2(0.0, -1.0);
            tmpvar_55.z = (h_47.w - h_47.z);
            highp vec3 tmpvar_56;
            tmpvar_56 = normalize(tmpvar_55);
            lowp vec3 tmpvar_57;
            tmpvar_57 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
            bump_21 = tmpvar_57;
            bump_21 = (bump_21 * mix (_BumpFace, _BumpOutline, clamp (
              (tmpvar_33 + (tmpvar_34 * 0.5))
            , 0.0, 1.0)));
            highp vec3 tmpvar_58;
            tmpvar_58 = mix (vec3(0.0, 0.0, 1.0), bump_21, faceColor_23.www);
            bump_21 = tmpvar_58;
            highp vec3 tmpvar_59;
            tmpvar_59 = normalize(((
              (tmpvar_54.yzx * tmpvar_56.zxy)
             - 
              (tmpvar_54.zxy * tmpvar_56.yzx)
            ) - tmpvar_58));
            highp mat3 tmpvar_60;
            tmpvar_60[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_60[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_60[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_61;
            highp vec3 N_62;
            N_62 = (tmpvar_60 * tmpvar_59);
            tmpvar_61 = (xlv_TEXCOORD7 - (2.0 * (
              dot (N_62, xlv_TEXCOORD7)
             * N_62)));
            lowp vec4 tmpvar_63;
            tmpvar_63 = textureCube (_Cube, tmpvar_61);
            highp float tmpvar_64;
            tmpvar_64 = clamp ((tmpvar_33 + (tmpvar_34 * 0.5)), 0.0, 1.0);
            lowp vec3 tmpvar_65;
            tmpvar_65 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_64));
            highp vec3 tmpvar_66;
            tmpvar_66 = ((tmpvar_63.xyz * tmpvar_65) * faceColor_23.w);
            tmpvar_16 = faceColor_23.xyz;
            tmpvar_17 = -(tmpvar_59);
            tmpvar_18 = tmpvar_66;
            highp float tmpvar_67;
            tmpvar_67 = clamp ((tmpvar_33 + (tmpvar_34 * 0.5)), 0.0, 1.0);
            tmpvar_19 = 1.0;
            tmpvar_20 = faceColor_23.w;
            tmpvar_5 = tmpvar_16;
            tmpvar_7 = tmpvar_18;
            tmpvar_8 = tmpvar_19;
            tmpvar_9 = tmpvar_20;
            highp float tmpvar_68;
            tmpvar_68 = (texture2D (_LightTextureB0, vec2(dot (xlv_TEXCOORD8, xlv_TEXCOORD8))).x * textureCube (_LightTexture0, xlv_TEXCOORD8).w);
            atten_4 = tmpvar_68;
            lowp float tmpvar_69;
            tmpvar_69 = dot (_unity_tbn_0_14, tmpvar_17);
            worldN_3.x = tmpvar_69;
            lowp float tmpvar_70;
            tmpvar_70 = dot (_unity_tbn_1_13, tmpvar_17);
            worldN_3.y = tmpvar_70;
            lowp float tmpvar_71;
            tmpvar_71 = dot (_unity_tbn_2_12, tmpvar_17);
            worldN_3.z = tmpvar_71;
            highp vec3 tmpvar_72;
            tmpvar_72 = normalize(worldN_3);
            worldN_3 = tmpvar_72;
            tmpvar_6 = tmpvar_72;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_11;
            tmpvar_1 = (tmpvar_1 * atten_4);
            mediump vec3 viewDir_73;
            viewDir_73 = worldViewDir_10;
            lowp vec4 c_74;
            lowp vec4 c_75;
            highp float nh_76;
            lowp float diff_77;
            mediump float tmpvar_78;
            tmpvar_78 = max (0.0, dot (tmpvar_6, tmpvar_2));
            diff_77 = tmpvar_78;
            mediump float tmpvar_79;
            tmpvar_79 = max (0.0, dot (tmpvar_6, normalize(
              (tmpvar_2 + viewDir_73)
            )));
            nh_76 = tmpvar_79;
            mediump float y_80;
            y_80 = (mix (_FaceShininess, _OutlineShininess, tmpvar_67) * 128.0);
            highp float tmpvar_81;
            tmpvar_81 = pow (nh_76, y_80);
            c_75.xyz = (((tmpvar_16 * tmpvar_1) * diff_77) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_81));
            c_75.w = tmpvar_20;
            c_74.w = c_75.w;
            c_74.xyz = c_75.xyz;
            gl_FragData[0] = c_74;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier02"
        {
           Keywords { "POINT_COOKIE" }
          
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
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp vec4 unity_WorldTransformParams;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp mat4 unity_WorldToLight;
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
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          varying highp vec3 xlv_TEXCOORD8;
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
            highp mat3 tmpvar_20;
            tmpvar_20[0] = unity_WorldToObject[0].xyz;
            tmpvar_20[1] = unity_WorldToObject[1].xyz;
            tmpvar_20[2] = unity_WorldToObject[2].xyz;
            highp vec3 tmpvar_21;
            tmpvar_21 = normalize((tmpvar_6 * tmpvar_20));
            highp mat3 tmpvar_22;
            tmpvar_22[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_22[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_22[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_23;
            tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.xyz));
            worldTangent_3 = tmpvar_23;
            highp float tmpvar_24;
            tmpvar_24 = (_glesTANGENT.w * unity_WorldTransformParams.w);
            tangentSign_2 = tmpvar_24;
            highp vec3 tmpvar_25;
            tmpvar_25 = (((tmpvar_21.yzx * worldTangent_3.zxy) - (tmpvar_21.zxy * worldTangent_3.yzx)) * tangentSign_2);
            worldBinormal_1 = tmpvar_25;
            highp vec3 tmpvar_26;
            tmpvar_26.x = worldTangent_3.x;
            tmpvar_26.y = worldBinormal_1.x;
            tmpvar_26.z = tmpvar_21.x;
            highp vec3 tmpvar_27;
            tmpvar_27.x = worldTangent_3.y;
            tmpvar_27.y = worldBinormal_1.y;
            tmpvar_27.z = tmpvar_21.y;
            highp vec3 tmpvar_28;
            tmpvar_28.x = worldTangent_3.z;
            tmpvar_28.y = worldBinormal_1.z;
            tmpvar_28.z = tmpvar_21.z;
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_19));
            xlv_TEXCOORD0 = tmpvar_4;
            xlv_TEXCOORD1 = ((tmpvar_7.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_26;
            xlv_TEXCOORD3 = tmpvar_27;
            xlv_TEXCOORD4 = tmpvar_28;
            xlv_TEXCOORD5 = (unity_ObjectToWorld * tmpvar_5).xyz;
            xlv_COLOR0 = _glesColor;
            xlv_TEXCOORD6 = tmpvar_8;
            xlv_TEXCOORD7 = (tmpvar_18 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz));
            xlv_TEXCOORD8 = (unity_WorldToLight * (unity_ObjectToWorld * tmpvar_5)).xyz;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform highp vec4 _WorldSpaceLightPos0;
          uniform highp mat4 unity_ObjectToWorld;
          uniform lowp vec4 _LightColor0;
          uniform lowp vec4 _SpecColor;
          uniform highp samplerCube _LightTexture0;
          uniform highp sampler2D _LightTextureB0;
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
          uniform highp float _Bevel;
          uniform highp float _BevelOffset;
          uniform highp float _BevelWidth;
          uniform highp float _BevelClamp;
          uniform highp float _BevelRoundness;
          uniform sampler2D _BumpMap;
          uniform highp float _BumpOutline;
          uniform highp float _BumpFace;
          uniform lowp samplerCube _Cube;
          uniform lowp vec4 _ReflectFaceColor;
          uniform lowp vec4 _ReflectOutlineColor;
          uniform highp float _ShaderFlags;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          uniform highp float _TextureWidth;
          uniform highp float _TextureHeight;
          uniform highp float _GradientScale;
          uniform mediump float _FaceShininess;
          uniform mediump float _OutlineShininess;
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          varying highp vec3 xlv_TEXCOORD8;
          void main ()
          {
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp float atten_4;
            lowp vec3 tmpvar_5;
            lowp vec3 tmpvar_6;
            lowp vec3 tmpvar_7;
            lowp float tmpvar_8;
            lowp float tmpvar_9;
            highp vec3 worldViewDir_10;
            lowp vec3 lightDir_11;
            lowp vec3 _unity_tbn_2_12;
            lowp vec3 _unity_tbn_1_13;
            lowp vec3 _unity_tbn_0_14;
            _unity_tbn_0_14 = xlv_TEXCOORD2;
            _unity_tbn_1_13 = xlv_TEXCOORD3;
            _unity_tbn_2_12 = xlv_TEXCOORD4;
            highp vec3 tmpvar_15;
            tmpvar_15 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD5));
            lightDir_11 = tmpvar_15;
            worldViewDir_10 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD5));
            tmpvar_5 = vec3(0.0, 0.0, 0.0);
            tmpvar_7 = vec3(0.0, 0.0, 0.0);
            tmpvar_9 = 0.0;
            tmpvar_8 = 0.0;
            tmpvar_6 = vec3(0.0, 0.0, 1.0);
            lowp vec3 tmpvar_16;
            lowp vec3 tmpvar_17;
            lowp vec3 tmpvar_18;
            lowp float tmpvar_19;
            lowp float tmpvar_20;
            tmpvar_16 = tmpvar_5;
            tmpvar_17 = tmpvar_6;
            tmpvar_18 = tmpvar_7;
            tmpvar_19 = tmpvar_8;
            tmpvar_20 = tmpvar_9;
            highp vec3 bump_21;
            highp vec4 outlineColor_22;
            highp vec4 faceColor_23;
            highp float c_24;
            highp vec4 smp4x_25;
            highp vec3 tmpvar_26;
            tmpvar_26.z = 0.0;
            tmpvar_26.x = (1.0/(_TextureWidth));
            tmpvar_26.y = (1.0/(_TextureHeight));
            highp vec2 P_27;
            P_27 = (xlv_TEXCOORD0.xy - tmpvar_26.xz);
            highp vec2 P_28;
            P_28 = (xlv_TEXCOORD0.xy + tmpvar_26.xz);
            highp vec2 P_29;
            P_29 = (xlv_TEXCOORD0.xy - tmpvar_26.zy);
            highp vec2 P_30;
            P_30 = (xlv_TEXCOORD0.xy + tmpvar_26.zy);
            lowp vec4 tmpvar_31;
            tmpvar_31.x = texture2D (_MainTex, P_27).w;
            tmpvar_31.y = texture2D (_MainTex, P_28).w;
            tmpvar_31.z = texture2D (_MainTex, P_29).w;
            tmpvar_31.w = texture2D (_MainTex, P_30).w;
            smp4x_25 = tmpvar_31;
            lowp float tmpvar_32;
            tmpvar_32 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_24 = tmpvar_32;
            highp float tmpvar_33;
            tmpvar_33 = (((
              (0.5 - c_24)
             - xlv_TEXCOORD6.x) * xlv_TEXCOORD6.y) + 0.5);
            highp float tmpvar_34;
            tmpvar_34 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD6.y);
            highp float tmpvar_35;
            tmpvar_35 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD6.y);
            faceColor_23 = _FaceColor;
            outlineColor_22 = _OutlineColor;
            faceColor_23 = (faceColor_23 * xlv_COLOR0);
            outlineColor_22.w = (outlineColor_22.w * xlv_COLOR0.w);
            highp vec2 tmpvar_36;
            tmpvar_36.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_36.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_37;
            tmpvar_37 = texture2D (_FaceTex, tmpvar_36);
            faceColor_23 = (faceColor_23 * tmpvar_37);
            highp vec2 tmpvar_38;
            tmpvar_38.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_38.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_39;
            tmpvar_39 = texture2D (_OutlineTex, tmpvar_38);
            outlineColor_22 = (outlineColor_22 * tmpvar_39);
            mediump float d_40;
            d_40 = tmpvar_33;
            lowp vec4 faceColor_41;
            faceColor_41 = faceColor_23;
            lowp vec4 outlineColor_42;
            outlineColor_42 = outlineColor_22;
            mediump float outline_43;
            outline_43 = tmpvar_34;
            mediump float softness_44;
            softness_44 = tmpvar_35;
            mediump float tmpvar_45;
            tmpvar_45 = (1.0 - clamp ((
              ((d_40 - (outline_43 * 0.5)) + (softness_44 * 0.5))
             / 
              (1.0 + softness_44)
            ), 0.0, 1.0));
            faceColor_41.xyz = (faceColor_41.xyz * faceColor_41.w);
            outlineColor_42.xyz = (outlineColor_42.xyz * outlineColor_42.w);
            mediump vec4 tmpvar_46;
            tmpvar_46 = mix (faceColor_41, outlineColor_42, vec4((clamp (
              (d_40 + (outline_43 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_43)
            ))));
            faceColor_41 = tmpvar_46;
            faceColor_41 = (faceColor_41 * tmpvar_45);
            faceColor_23 = faceColor_41;
            faceColor_23.xyz = (faceColor_23.xyz / max (faceColor_23.w, 0.0001));
            highp vec4 h_47;
            h_47 = smp4x_25;
            highp float tmpvar_48;
            tmpvar_48 = (_ShaderFlags / 2.0);
            highp float tmpvar_49;
            tmpvar_49 = (fract(abs(tmpvar_48)) * 2.0);
            highp float tmpvar_50;
            if ((tmpvar_48 >= 0.0)) {
              tmpvar_50 = tmpvar_49;
            } else {
              tmpvar_50 = -(tmpvar_49);
            };
            h_47 = (smp4x_25 + (xlv_TEXCOORD6.x + _BevelOffset));
            highp float tmpvar_51;
            tmpvar_51 = max (0.01, (_OutlineWidth + _BevelWidth));
            h_47 = (h_47 - 0.5);
            h_47 = (h_47 / tmpvar_51);
            highp vec4 tmpvar_52;
            tmpvar_52 = clamp ((h_47 + 0.5), 0.0, 1.0);
            h_47 = tmpvar_52;
            if (bool(float((tmpvar_50 >= 1.0)))) {
              h_47 = (1.0 - abs((
                (tmpvar_52 * 2.0)
               - 1.0)));
            };
            h_47 = (min (mix (h_47, 
              sin(((h_47 * 3.141592) / 2.0))
            , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((_Bevel * tmpvar_51) * (_GradientScale * -2.0)));
            highp vec3 tmpvar_53;
            tmpvar_53.xy = vec2(1.0, 0.0);
            tmpvar_53.z = (h_47.y - h_47.x);
            highp vec3 tmpvar_54;
            tmpvar_54 = normalize(tmpvar_53);
            highp vec3 tmpvar_55;
            tmpvar_55.xy = vec2(0.0, -1.0);
            tmpvar_55.z = (h_47.w - h_47.z);
            highp vec3 tmpvar_56;
            tmpvar_56 = normalize(tmpvar_55);
            lowp vec3 tmpvar_57;
            tmpvar_57 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
            bump_21 = tmpvar_57;
            bump_21 = (bump_21 * mix (_BumpFace, _BumpOutline, clamp (
              (tmpvar_33 + (tmpvar_34 * 0.5))
            , 0.0, 1.0)));
            highp vec3 tmpvar_58;
            tmpvar_58 = mix (vec3(0.0, 0.0, 1.0), bump_21, faceColor_23.www);
            bump_21 = tmpvar_58;
            highp vec3 tmpvar_59;
            tmpvar_59 = normalize(((
              (tmpvar_54.yzx * tmpvar_56.zxy)
             - 
              (tmpvar_54.zxy * tmpvar_56.yzx)
            ) - tmpvar_58));
            highp mat3 tmpvar_60;
            tmpvar_60[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_60[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_60[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_61;
            highp vec3 N_62;
            N_62 = (tmpvar_60 * tmpvar_59);
            tmpvar_61 = (xlv_TEXCOORD7 - (2.0 * (
              dot (N_62, xlv_TEXCOORD7)
             * N_62)));
            lowp vec4 tmpvar_63;
            tmpvar_63 = textureCube (_Cube, tmpvar_61);
            highp float tmpvar_64;
            tmpvar_64 = clamp ((tmpvar_33 + (tmpvar_34 * 0.5)), 0.0, 1.0);
            lowp vec3 tmpvar_65;
            tmpvar_65 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_64));
            highp vec3 tmpvar_66;
            tmpvar_66 = ((tmpvar_63.xyz * tmpvar_65) * faceColor_23.w);
            tmpvar_16 = faceColor_23.xyz;
            tmpvar_17 = -(tmpvar_59);
            tmpvar_18 = tmpvar_66;
            highp float tmpvar_67;
            tmpvar_67 = clamp ((tmpvar_33 + (tmpvar_34 * 0.5)), 0.0, 1.0);
            tmpvar_19 = 1.0;
            tmpvar_20 = faceColor_23.w;
            tmpvar_5 = tmpvar_16;
            tmpvar_7 = tmpvar_18;
            tmpvar_8 = tmpvar_19;
            tmpvar_9 = tmpvar_20;
            highp float tmpvar_68;
            tmpvar_68 = (texture2D (_LightTextureB0, vec2(dot (xlv_TEXCOORD8, xlv_TEXCOORD8))).x * textureCube (_LightTexture0, xlv_TEXCOORD8).w);
            atten_4 = tmpvar_68;
            lowp float tmpvar_69;
            tmpvar_69 = dot (_unity_tbn_0_14, tmpvar_17);
            worldN_3.x = tmpvar_69;
            lowp float tmpvar_70;
            tmpvar_70 = dot (_unity_tbn_1_13, tmpvar_17);
            worldN_3.y = tmpvar_70;
            lowp float tmpvar_71;
            tmpvar_71 = dot (_unity_tbn_2_12, tmpvar_17);
            worldN_3.z = tmpvar_71;
            highp vec3 tmpvar_72;
            tmpvar_72 = normalize(worldN_3);
            worldN_3 = tmpvar_72;
            tmpvar_6 = tmpvar_72;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_11;
            tmpvar_1 = (tmpvar_1 * atten_4);
            mediump vec3 viewDir_73;
            viewDir_73 = worldViewDir_10;
            lowp vec4 c_74;
            lowp vec4 c_75;
            highp float nh_76;
            lowp float diff_77;
            mediump float tmpvar_78;
            tmpvar_78 = max (0.0, dot (tmpvar_6, tmpvar_2));
            diff_77 = tmpvar_78;
            mediump float tmpvar_79;
            tmpvar_79 = max (0.0, dot (tmpvar_6, normalize(
              (tmpvar_2 + viewDir_73)
            )));
            nh_76 = tmpvar_79;
            mediump float y_80;
            y_80 = (mix (_FaceShininess, _OutlineShininess, tmpvar_67) * 128.0);
            highp float tmpvar_81;
            tmpvar_81 = pow (nh_76, y_80);
            c_75.xyz = (((tmpvar_16 * tmpvar_1) * diff_77) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_81));
            c_75.w = tmpvar_20;
            c_74.w = c_75.w;
            c_74.xyz = c_75.xyz;
            gl_FragData[0] = c_74;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier00"
        {
           Keywords { "DIRECTIONAL_COOKIE" }
          
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
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp vec4 unity_WorldTransformParams;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp mat4 unity_WorldToLight;
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
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          varying highp vec2 xlv_TEXCOORD8;
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
            highp mat3 tmpvar_20;
            tmpvar_20[0] = unity_WorldToObject[0].xyz;
            tmpvar_20[1] = unity_WorldToObject[1].xyz;
            tmpvar_20[2] = unity_WorldToObject[2].xyz;
            highp vec3 tmpvar_21;
            tmpvar_21 = normalize((tmpvar_6 * tmpvar_20));
            highp mat3 tmpvar_22;
            tmpvar_22[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_22[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_22[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_23;
            tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.xyz));
            worldTangent_3 = tmpvar_23;
            highp float tmpvar_24;
            tmpvar_24 = (_glesTANGENT.w * unity_WorldTransformParams.w);
            tangentSign_2 = tmpvar_24;
            highp vec3 tmpvar_25;
            tmpvar_25 = (((tmpvar_21.yzx * worldTangent_3.zxy) - (tmpvar_21.zxy * worldTangent_3.yzx)) * tangentSign_2);
            worldBinormal_1 = tmpvar_25;
            highp vec3 tmpvar_26;
            tmpvar_26.x = worldTangent_3.x;
            tmpvar_26.y = worldBinormal_1.x;
            tmpvar_26.z = tmpvar_21.x;
            highp vec3 tmpvar_27;
            tmpvar_27.x = worldTangent_3.y;
            tmpvar_27.y = worldBinormal_1.y;
            tmpvar_27.z = tmpvar_21.y;
            highp vec3 tmpvar_28;
            tmpvar_28.x = worldTangent_3.z;
            tmpvar_28.y = worldBinormal_1.z;
            tmpvar_28.z = tmpvar_21.z;
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_19));
            xlv_TEXCOORD0 = tmpvar_4;
            xlv_TEXCOORD1 = ((tmpvar_7.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_26;
            xlv_TEXCOORD3 = tmpvar_27;
            xlv_TEXCOORD4 = tmpvar_28;
            xlv_TEXCOORD5 = (unity_ObjectToWorld * tmpvar_5).xyz;
            xlv_COLOR0 = _glesColor;
            xlv_TEXCOORD6 = tmpvar_8;
            xlv_TEXCOORD7 = (tmpvar_18 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz));
            xlv_TEXCOORD8 = (unity_WorldToLight * (unity_ObjectToWorld * tmpvar_5)).xy;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform mediump vec4 _WorldSpaceLightPos0;
          uniform highp mat4 unity_ObjectToWorld;
          uniform lowp vec4 _LightColor0;
          uniform lowp vec4 _SpecColor;
          uniform highp sampler2D _LightTexture0;
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
          uniform highp float _Bevel;
          uniform highp float _BevelOffset;
          uniform highp float _BevelWidth;
          uniform highp float _BevelClamp;
          uniform highp float _BevelRoundness;
          uniform sampler2D _BumpMap;
          uniform highp float _BumpOutline;
          uniform highp float _BumpFace;
          uniform lowp samplerCube _Cube;
          uniform lowp vec4 _ReflectFaceColor;
          uniform lowp vec4 _ReflectOutlineColor;
          uniform highp float _ShaderFlags;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          uniform highp float _TextureWidth;
          uniform highp float _TextureHeight;
          uniform highp float _GradientScale;
          uniform mediump float _FaceShininess;
          uniform mediump float _OutlineShininess;
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          varying highp vec2 xlv_TEXCOORD8;
          void main ()
          {
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp float atten_4;
            lowp vec3 tmpvar_5;
            lowp vec3 tmpvar_6;
            lowp vec3 tmpvar_7;
            lowp float tmpvar_8;
            lowp float tmpvar_9;
            highp vec3 worldViewDir_10;
            lowp vec3 lightDir_11;
            lowp vec3 _unity_tbn_2_12;
            lowp vec3 _unity_tbn_1_13;
            lowp vec3 _unity_tbn_0_14;
            _unity_tbn_0_14 = xlv_TEXCOORD2;
            _unity_tbn_1_13 = xlv_TEXCOORD3;
            _unity_tbn_2_12 = xlv_TEXCOORD4;
            mediump vec3 tmpvar_15;
            tmpvar_15 = _WorldSpaceLightPos0.xyz;
            lightDir_11 = tmpvar_15;
            worldViewDir_10 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD5));
            tmpvar_5 = vec3(0.0, 0.0, 0.0);
            tmpvar_7 = vec3(0.0, 0.0, 0.0);
            tmpvar_9 = 0.0;
            tmpvar_8 = 0.0;
            tmpvar_6 = vec3(0.0, 0.0, 1.0);
            lowp vec3 tmpvar_16;
            lowp vec3 tmpvar_17;
            lowp vec3 tmpvar_18;
            lowp float tmpvar_19;
            lowp float tmpvar_20;
            tmpvar_16 = tmpvar_5;
            tmpvar_17 = tmpvar_6;
            tmpvar_18 = tmpvar_7;
            tmpvar_19 = tmpvar_8;
            tmpvar_20 = tmpvar_9;
            highp vec3 bump_21;
            highp vec4 outlineColor_22;
            highp vec4 faceColor_23;
            highp float c_24;
            highp vec4 smp4x_25;
            highp vec3 tmpvar_26;
            tmpvar_26.z = 0.0;
            tmpvar_26.x = (1.0/(_TextureWidth));
            tmpvar_26.y = (1.0/(_TextureHeight));
            highp vec2 P_27;
            P_27 = (xlv_TEXCOORD0.xy - tmpvar_26.xz);
            highp vec2 P_28;
            P_28 = (xlv_TEXCOORD0.xy + tmpvar_26.xz);
            highp vec2 P_29;
            P_29 = (xlv_TEXCOORD0.xy - tmpvar_26.zy);
            highp vec2 P_30;
            P_30 = (xlv_TEXCOORD0.xy + tmpvar_26.zy);
            lowp vec4 tmpvar_31;
            tmpvar_31.x = texture2D (_MainTex, P_27).w;
            tmpvar_31.y = texture2D (_MainTex, P_28).w;
            tmpvar_31.z = texture2D (_MainTex, P_29).w;
            tmpvar_31.w = texture2D (_MainTex, P_30).w;
            smp4x_25 = tmpvar_31;
            lowp float tmpvar_32;
            tmpvar_32 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_24 = tmpvar_32;
            highp float tmpvar_33;
            tmpvar_33 = (((
              (0.5 - c_24)
             - xlv_TEXCOORD6.x) * xlv_TEXCOORD6.y) + 0.5);
            highp float tmpvar_34;
            tmpvar_34 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD6.y);
            highp float tmpvar_35;
            tmpvar_35 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD6.y);
            faceColor_23 = _FaceColor;
            outlineColor_22 = _OutlineColor;
            faceColor_23 = (faceColor_23 * xlv_COLOR0);
            outlineColor_22.w = (outlineColor_22.w * xlv_COLOR0.w);
            highp vec2 tmpvar_36;
            tmpvar_36.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_36.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_37;
            tmpvar_37 = texture2D (_FaceTex, tmpvar_36);
            faceColor_23 = (faceColor_23 * tmpvar_37);
            highp vec2 tmpvar_38;
            tmpvar_38.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_38.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_39;
            tmpvar_39 = texture2D (_OutlineTex, tmpvar_38);
            outlineColor_22 = (outlineColor_22 * tmpvar_39);
            mediump float d_40;
            d_40 = tmpvar_33;
            lowp vec4 faceColor_41;
            faceColor_41 = faceColor_23;
            lowp vec4 outlineColor_42;
            outlineColor_42 = outlineColor_22;
            mediump float outline_43;
            outline_43 = tmpvar_34;
            mediump float softness_44;
            softness_44 = tmpvar_35;
            mediump float tmpvar_45;
            tmpvar_45 = (1.0 - clamp ((
              ((d_40 - (outline_43 * 0.5)) + (softness_44 * 0.5))
             / 
              (1.0 + softness_44)
            ), 0.0, 1.0));
            faceColor_41.xyz = (faceColor_41.xyz * faceColor_41.w);
            outlineColor_42.xyz = (outlineColor_42.xyz * outlineColor_42.w);
            mediump vec4 tmpvar_46;
            tmpvar_46 = mix (faceColor_41, outlineColor_42, vec4((clamp (
              (d_40 + (outline_43 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_43)
            ))));
            faceColor_41 = tmpvar_46;
            faceColor_41 = (faceColor_41 * tmpvar_45);
            faceColor_23 = faceColor_41;
            faceColor_23.xyz = (faceColor_23.xyz / max (faceColor_23.w, 0.0001));
            highp vec4 h_47;
            h_47 = smp4x_25;
            highp float tmpvar_48;
            tmpvar_48 = (_ShaderFlags / 2.0);
            highp float tmpvar_49;
            tmpvar_49 = (fract(abs(tmpvar_48)) * 2.0);
            highp float tmpvar_50;
            if ((tmpvar_48 >= 0.0)) {
              tmpvar_50 = tmpvar_49;
            } else {
              tmpvar_50 = -(tmpvar_49);
            };
            h_47 = (smp4x_25 + (xlv_TEXCOORD6.x + _BevelOffset));
            highp float tmpvar_51;
            tmpvar_51 = max (0.01, (_OutlineWidth + _BevelWidth));
            h_47 = (h_47 - 0.5);
            h_47 = (h_47 / tmpvar_51);
            highp vec4 tmpvar_52;
            tmpvar_52 = clamp ((h_47 + 0.5), 0.0, 1.0);
            h_47 = tmpvar_52;
            if (bool(float((tmpvar_50 >= 1.0)))) {
              h_47 = (1.0 - abs((
                (tmpvar_52 * 2.0)
               - 1.0)));
            };
            h_47 = (min (mix (h_47, 
              sin(((h_47 * 3.141592) / 2.0))
            , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((_Bevel * tmpvar_51) * (_GradientScale * -2.0)));
            highp vec3 tmpvar_53;
            tmpvar_53.xy = vec2(1.0, 0.0);
            tmpvar_53.z = (h_47.y - h_47.x);
            highp vec3 tmpvar_54;
            tmpvar_54 = normalize(tmpvar_53);
            highp vec3 tmpvar_55;
            tmpvar_55.xy = vec2(0.0, -1.0);
            tmpvar_55.z = (h_47.w - h_47.z);
            highp vec3 tmpvar_56;
            tmpvar_56 = normalize(tmpvar_55);
            lowp vec3 tmpvar_57;
            tmpvar_57 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
            bump_21 = tmpvar_57;
            bump_21 = (bump_21 * mix (_BumpFace, _BumpOutline, clamp (
              (tmpvar_33 + (tmpvar_34 * 0.5))
            , 0.0, 1.0)));
            highp vec3 tmpvar_58;
            tmpvar_58 = mix (vec3(0.0, 0.0, 1.0), bump_21, faceColor_23.www);
            bump_21 = tmpvar_58;
            highp vec3 tmpvar_59;
            tmpvar_59 = normalize(((
              (tmpvar_54.yzx * tmpvar_56.zxy)
             - 
              (tmpvar_54.zxy * tmpvar_56.yzx)
            ) - tmpvar_58));
            highp mat3 tmpvar_60;
            tmpvar_60[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_60[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_60[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_61;
            highp vec3 N_62;
            N_62 = (tmpvar_60 * tmpvar_59);
            tmpvar_61 = (xlv_TEXCOORD7 - (2.0 * (
              dot (N_62, xlv_TEXCOORD7)
             * N_62)));
            lowp vec4 tmpvar_63;
            tmpvar_63 = textureCube (_Cube, tmpvar_61);
            highp float tmpvar_64;
            tmpvar_64 = clamp ((tmpvar_33 + (tmpvar_34 * 0.5)), 0.0, 1.0);
            lowp vec3 tmpvar_65;
            tmpvar_65 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_64));
            highp vec3 tmpvar_66;
            tmpvar_66 = ((tmpvar_63.xyz * tmpvar_65) * faceColor_23.w);
            tmpvar_16 = faceColor_23.xyz;
            tmpvar_17 = -(tmpvar_59);
            tmpvar_18 = tmpvar_66;
            highp float tmpvar_67;
            tmpvar_67 = clamp ((tmpvar_33 + (tmpvar_34 * 0.5)), 0.0, 1.0);
            tmpvar_19 = 1.0;
            tmpvar_20 = faceColor_23.w;
            tmpvar_5 = tmpvar_16;
            tmpvar_7 = tmpvar_18;
            tmpvar_8 = tmpvar_19;
            tmpvar_9 = tmpvar_20;
            highp float tmpvar_68;
            tmpvar_68 = texture2D (_LightTexture0, xlv_TEXCOORD8).w;
            atten_4 = tmpvar_68;
            lowp float tmpvar_69;
            tmpvar_69 = dot (_unity_tbn_0_14, tmpvar_17);
            worldN_3.x = tmpvar_69;
            lowp float tmpvar_70;
            tmpvar_70 = dot (_unity_tbn_1_13, tmpvar_17);
            worldN_3.y = tmpvar_70;
            lowp float tmpvar_71;
            tmpvar_71 = dot (_unity_tbn_2_12, tmpvar_17);
            worldN_3.z = tmpvar_71;
            highp vec3 tmpvar_72;
            tmpvar_72 = normalize(worldN_3);
            worldN_3 = tmpvar_72;
            tmpvar_6 = tmpvar_72;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_11;
            tmpvar_1 = (tmpvar_1 * atten_4);
            mediump vec3 viewDir_73;
            viewDir_73 = worldViewDir_10;
            lowp vec4 c_74;
            lowp vec4 c_75;
            highp float nh_76;
            lowp float diff_77;
            mediump float tmpvar_78;
            tmpvar_78 = max (0.0, dot (tmpvar_6, tmpvar_2));
            diff_77 = tmpvar_78;
            mediump float tmpvar_79;
            tmpvar_79 = max (0.0, dot (tmpvar_6, normalize(
              (tmpvar_2 + viewDir_73)
            )));
            nh_76 = tmpvar_79;
            mediump float y_80;
            y_80 = (mix (_FaceShininess, _OutlineShininess, tmpvar_67) * 128.0);
            highp float tmpvar_81;
            tmpvar_81 = pow (nh_76, y_80);
            c_75.xyz = (((tmpvar_16 * tmpvar_1) * diff_77) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_81));
            c_75.w = tmpvar_20;
            c_74.w = c_75.w;
            c_74.xyz = c_75.xyz;
            gl_FragData[0] = c_74;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier01"
        {
           Keywords { "DIRECTIONAL_COOKIE" }
          
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
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp vec4 unity_WorldTransformParams;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp mat4 unity_WorldToLight;
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
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          varying highp vec2 xlv_TEXCOORD8;
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
            highp mat3 tmpvar_20;
            tmpvar_20[0] = unity_WorldToObject[0].xyz;
            tmpvar_20[1] = unity_WorldToObject[1].xyz;
            tmpvar_20[2] = unity_WorldToObject[2].xyz;
            highp vec3 tmpvar_21;
            tmpvar_21 = normalize((tmpvar_6 * tmpvar_20));
            highp mat3 tmpvar_22;
            tmpvar_22[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_22[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_22[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_23;
            tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.xyz));
            worldTangent_3 = tmpvar_23;
            highp float tmpvar_24;
            tmpvar_24 = (_glesTANGENT.w * unity_WorldTransformParams.w);
            tangentSign_2 = tmpvar_24;
            highp vec3 tmpvar_25;
            tmpvar_25 = (((tmpvar_21.yzx * worldTangent_3.zxy) - (tmpvar_21.zxy * worldTangent_3.yzx)) * tangentSign_2);
            worldBinormal_1 = tmpvar_25;
            highp vec3 tmpvar_26;
            tmpvar_26.x = worldTangent_3.x;
            tmpvar_26.y = worldBinormal_1.x;
            tmpvar_26.z = tmpvar_21.x;
            highp vec3 tmpvar_27;
            tmpvar_27.x = worldTangent_3.y;
            tmpvar_27.y = worldBinormal_1.y;
            tmpvar_27.z = tmpvar_21.y;
            highp vec3 tmpvar_28;
            tmpvar_28.x = worldTangent_3.z;
            tmpvar_28.y = worldBinormal_1.z;
            tmpvar_28.z = tmpvar_21.z;
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_19));
            xlv_TEXCOORD0 = tmpvar_4;
            xlv_TEXCOORD1 = ((tmpvar_7.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_26;
            xlv_TEXCOORD3 = tmpvar_27;
            xlv_TEXCOORD4 = tmpvar_28;
            xlv_TEXCOORD5 = (unity_ObjectToWorld * tmpvar_5).xyz;
            xlv_COLOR0 = _glesColor;
            xlv_TEXCOORD6 = tmpvar_8;
            xlv_TEXCOORD7 = (tmpvar_18 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz));
            xlv_TEXCOORD8 = (unity_WorldToLight * (unity_ObjectToWorld * tmpvar_5)).xy;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform mediump vec4 _WorldSpaceLightPos0;
          uniform highp mat4 unity_ObjectToWorld;
          uniform lowp vec4 _LightColor0;
          uniform lowp vec4 _SpecColor;
          uniform highp sampler2D _LightTexture0;
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
          uniform highp float _Bevel;
          uniform highp float _BevelOffset;
          uniform highp float _BevelWidth;
          uniform highp float _BevelClamp;
          uniform highp float _BevelRoundness;
          uniform sampler2D _BumpMap;
          uniform highp float _BumpOutline;
          uniform highp float _BumpFace;
          uniform lowp samplerCube _Cube;
          uniform lowp vec4 _ReflectFaceColor;
          uniform lowp vec4 _ReflectOutlineColor;
          uniform highp float _ShaderFlags;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          uniform highp float _TextureWidth;
          uniform highp float _TextureHeight;
          uniform highp float _GradientScale;
          uniform mediump float _FaceShininess;
          uniform mediump float _OutlineShininess;
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          varying highp vec2 xlv_TEXCOORD8;
          void main ()
          {
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp float atten_4;
            lowp vec3 tmpvar_5;
            lowp vec3 tmpvar_6;
            lowp vec3 tmpvar_7;
            lowp float tmpvar_8;
            lowp float tmpvar_9;
            highp vec3 worldViewDir_10;
            lowp vec3 lightDir_11;
            lowp vec3 _unity_tbn_2_12;
            lowp vec3 _unity_tbn_1_13;
            lowp vec3 _unity_tbn_0_14;
            _unity_tbn_0_14 = xlv_TEXCOORD2;
            _unity_tbn_1_13 = xlv_TEXCOORD3;
            _unity_tbn_2_12 = xlv_TEXCOORD4;
            mediump vec3 tmpvar_15;
            tmpvar_15 = _WorldSpaceLightPos0.xyz;
            lightDir_11 = tmpvar_15;
            worldViewDir_10 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD5));
            tmpvar_5 = vec3(0.0, 0.0, 0.0);
            tmpvar_7 = vec3(0.0, 0.0, 0.0);
            tmpvar_9 = 0.0;
            tmpvar_8 = 0.0;
            tmpvar_6 = vec3(0.0, 0.0, 1.0);
            lowp vec3 tmpvar_16;
            lowp vec3 tmpvar_17;
            lowp vec3 tmpvar_18;
            lowp float tmpvar_19;
            lowp float tmpvar_20;
            tmpvar_16 = tmpvar_5;
            tmpvar_17 = tmpvar_6;
            tmpvar_18 = tmpvar_7;
            tmpvar_19 = tmpvar_8;
            tmpvar_20 = tmpvar_9;
            highp vec3 bump_21;
            highp vec4 outlineColor_22;
            highp vec4 faceColor_23;
            highp float c_24;
            highp vec4 smp4x_25;
            highp vec3 tmpvar_26;
            tmpvar_26.z = 0.0;
            tmpvar_26.x = (1.0/(_TextureWidth));
            tmpvar_26.y = (1.0/(_TextureHeight));
            highp vec2 P_27;
            P_27 = (xlv_TEXCOORD0.xy - tmpvar_26.xz);
            highp vec2 P_28;
            P_28 = (xlv_TEXCOORD0.xy + tmpvar_26.xz);
            highp vec2 P_29;
            P_29 = (xlv_TEXCOORD0.xy - tmpvar_26.zy);
            highp vec2 P_30;
            P_30 = (xlv_TEXCOORD0.xy + tmpvar_26.zy);
            lowp vec4 tmpvar_31;
            tmpvar_31.x = texture2D (_MainTex, P_27).w;
            tmpvar_31.y = texture2D (_MainTex, P_28).w;
            tmpvar_31.z = texture2D (_MainTex, P_29).w;
            tmpvar_31.w = texture2D (_MainTex, P_30).w;
            smp4x_25 = tmpvar_31;
            lowp float tmpvar_32;
            tmpvar_32 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_24 = tmpvar_32;
            highp float tmpvar_33;
            tmpvar_33 = (((
              (0.5 - c_24)
             - xlv_TEXCOORD6.x) * xlv_TEXCOORD6.y) + 0.5);
            highp float tmpvar_34;
            tmpvar_34 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD6.y);
            highp float tmpvar_35;
            tmpvar_35 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD6.y);
            faceColor_23 = _FaceColor;
            outlineColor_22 = _OutlineColor;
            faceColor_23 = (faceColor_23 * xlv_COLOR0);
            outlineColor_22.w = (outlineColor_22.w * xlv_COLOR0.w);
            highp vec2 tmpvar_36;
            tmpvar_36.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_36.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_37;
            tmpvar_37 = texture2D (_FaceTex, tmpvar_36);
            faceColor_23 = (faceColor_23 * tmpvar_37);
            highp vec2 tmpvar_38;
            tmpvar_38.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_38.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_39;
            tmpvar_39 = texture2D (_OutlineTex, tmpvar_38);
            outlineColor_22 = (outlineColor_22 * tmpvar_39);
            mediump float d_40;
            d_40 = tmpvar_33;
            lowp vec4 faceColor_41;
            faceColor_41 = faceColor_23;
            lowp vec4 outlineColor_42;
            outlineColor_42 = outlineColor_22;
            mediump float outline_43;
            outline_43 = tmpvar_34;
            mediump float softness_44;
            softness_44 = tmpvar_35;
            mediump float tmpvar_45;
            tmpvar_45 = (1.0 - clamp ((
              ((d_40 - (outline_43 * 0.5)) + (softness_44 * 0.5))
             / 
              (1.0 + softness_44)
            ), 0.0, 1.0));
            faceColor_41.xyz = (faceColor_41.xyz * faceColor_41.w);
            outlineColor_42.xyz = (outlineColor_42.xyz * outlineColor_42.w);
            mediump vec4 tmpvar_46;
            tmpvar_46 = mix (faceColor_41, outlineColor_42, vec4((clamp (
              (d_40 + (outline_43 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_43)
            ))));
            faceColor_41 = tmpvar_46;
            faceColor_41 = (faceColor_41 * tmpvar_45);
            faceColor_23 = faceColor_41;
            faceColor_23.xyz = (faceColor_23.xyz / max (faceColor_23.w, 0.0001));
            highp vec4 h_47;
            h_47 = smp4x_25;
            highp float tmpvar_48;
            tmpvar_48 = (_ShaderFlags / 2.0);
            highp float tmpvar_49;
            tmpvar_49 = (fract(abs(tmpvar_48)) * 2.0);
            highp float tmpvar_50;
            if ((tmpvar_48 >= 0.0)) {
              tmpvar_50 = tmpvar_49;
            } else {
              tmpvar_50 = -(tmpvar_49);
            };
            h_47 = (smp4x_25 + (xlv_TEXCOORD6.x + _BevelOffset));
            highp float tmpvar_51;
            tmpvar_51 = max (0.01, (_OutlineWidth + _BevelWidth));
            h_47 = (h_47 - 0.5);
            h_47 = (h_47 / tmpvar_51);
            highp vec4 tmpvar_52;
            tmpvar_52 = clamp ((h_47 + 0.5), 0.0, 1.0);
            h_47 = tmpvar_52;
            if (bool(float((tmpvar_50 >= 1.0)))) {
              h_47 = (1.0 - abs((
                (tmpvar_52 * 2.0)
               - 1.0)));
            };
            h_47 = (min (mix (h_47, 
              sin(((h_47 * 3.141592) / 2.0))
            , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((_Bevel * tmpvar_51) * (_GradientScale * -2.0)));
            highp vec3 tmpvar_53;
            tmpvar_53.xy = vec2(1.0, 0.0);
            tmpvar_53.z = (h_47.y - h_47.x);
            highp vec3 tmpvar_54;
            tmpvar_54 = normalize(tmpvar_53);
            highp vec3 tmpvar_55;
            tmpvar_55.xy = vec2(0.0, -1.0);
            tmpvar_55.z = (h_47.w - h_47.z);
            highp vec3 tmpvar_56;
            tmpvar_56 = normalize(tmpvar_55);
            lowp vec3 tmpvar_57;
            tmpvar_57 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
            bump_21 = tmpvar_57;
            bump_21 = (bump_21 * mix (_BumpFace, _BumpOutline, clamp (
              (tmpvar_33 + (tmpvar_34 * 0.5))
            , 0.0, 1.0)));
            highp vec3 tmpvar_58;
            tmpvar_58 = mix (vec3(0.0, 0.0, 1.0), bump_21, faceColor_23.www);
            bump_21 = tmpvar_58;
            highp vec3 tmpvar_59;
            tmpvar_59 = normalize(((
              (tmpvar_54.yzx * tmpvar_56.zxy)
             - 
              (tmpvar_54.zxy * tmpvar_56.yzx)
            ) - tmpvar_58));
            highp mat3 tmpvar_60;
            tmpvar_60[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_60[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_60[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_61;
            highp vec3 N_62;
            N_62 = (tmpvar_60 * tmpvar_59);
            tmpvar_61 = (xlv_TEXCOORD7 - (2.0 * (
              dot (N_62, xlv_TEXCOORD7)
             * N_62)));
            lowp vec4 tmpvar_63;
            tmpvar_63 = textureCube (_Cube, tmpvar_61);
            highp float tmpvar_64;
            tmpvar_64 = clamp ((tmpvar_33 + (tmpvar_34 * 0.5)), 0.0, 1.0);
            lowp vec3 tmpvar_65;
            tmpvar_65 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_64));
            highp vec3 tmpvar_66;
            tmpvar_66 = ((tmpvar_63.xyz * tmpvar_65) * faceColor_23.w);
            tmpvar_16 = faceColor_23.xyz;
            tmpvar_17 = -(tmpvar_59);
            tmpvar_18 = tmpvar_66;
            highp float tmpvar_67;
            tmpvar_67 = clamp ((tmpvar_33 + (tmpvar_34 * 0.5)), 0.0, 1.0);
            tmpvar_19 = 1.0;
            tmpvar_20 = faceColor_23.w;
            tmpvar_5 = tmpvar_16;
            tmpvar_7 = tmpvar_18;
            tmpvar_8 = tmpvar_19;
            tmpvar_9 = tmpvar_20;
            highp float tmpvar_68;
            tmpvar_68 = texture2D (_LightTexture0, xlv_TEXCOORD8).w;
            atten_4 = tmpvar_68;
            lowp float tmpvar_69;
            tmpvar_69 = dot (_unity_tbn_0_14, tmpvar_17);
            worldN_3.x = tmpvar_69;
            lowp float tmpvar_70;
            tmpvar_70 = dot (_unity_tbn_1_13, tmpvar_17);
            worldN_3.y = tmpvar_70;
            lowp float tmpvar_71;
            tmpvar_71 = dot (_unity_tbn_2_12, tmpvar_17);
            worldN_3.z = tmpvar_71;
            highp vec3 tmpvar_72;
            tmpvar_72 = normalize(worldN_3);
            worldN_3 = tmpvar_72;
            tmpvar_6 = tmpvar_72;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_11;
            tmpvar_1 = (tmpvar_1 * atten_4);
            mediump vec3 viewDir_73;
            viewDir_73 = worldViewDir_10;
            lowp vec4 c_74;
            lowp vec4 c_75;
            highp float nh_76;
            lowp float diff_77;
            mediump float tmpvar_78;
            tmpvar_78 = max (0.0, dot (tmpvar_6, tmpvar_2));
            diff_77 = tmpvar_78;
            mediump float tmpvar_79;
            tmpvar_79 = max (0.0, dot (tmpvar_6, normalize(
              (tmpvar_2 + viewDir_73)
            )));
            nh_76 = tmpvar_79;
            mediump float y_80;
            y_80 = (mix (_FaceShininess, _OutlineShininess, tmpvar_67) * 128.0);
            highp float tmpvar_81;
            tmpvar_81 = pow (nh_76, y_80);
            c_75.xyz = (((tmpvar_16 * tmpvar_1) * diff_77) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_81));
            c_75.w = tmpvar_20;
            c_74.w = c_75.w;
            c_74.xyz = c_75.xyz;
            gl_FragData[0] = c_74;
          }
          
          
          #endif
          
          "
        }
        SubProgram "gles hw_tier02"
        {
           Keywords { "DIRECTIONAL_COOKIE" }
          
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
          uniform highp mat4 unity_ObjectToWorld;
          uniform highp mat4 unity_WorldToObject;
          uniform highp vec4 unity_WorldTransformParams;
          uniform highp mat4 glstate_matrix_projection;
          uniform highp mat4 unity_MatrixVP;
          uniform highp mat4 unity_WorldToLight;
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
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          varying highp vec2 xlv_TEXCOORD8;
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
            highp mat3 tmpvar_20;
            tmpvar_20[0] = unity_WorldToObject[0].xyz;
            tmpvar_20[1] = unity_WorldToObject[1].xyz;
            tmpvar_20[2] = unity_WorldToObject[2].xyz;
            highp vec3 tmpvar_21;
            tmpvar_21 = normalize((tmpvar_6 * tmpvar_20));
            highp mat3 tmpvar_22;
            tmpvar_22[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_22[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_22[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_23;
            tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.xyz));
            worldTangent_3 = tmpvar_23;
            highp float tmpvar_24;
            tmpvar_24 = (_glesTANGENT.w * unity_WorldTransformParams.w);
            tangentSign_2 = tmpvar_24;
            highp vec3 tmpvar_25;
            tmpvar_25 = (((tmpvar_21.yzx * worldTangent_3.zxy) - (tmpvar_21.zxy * worldTangent_3.yzx)) * tangentSign_2);
            worldBinormal_1 = tmpvar_25;
            highp vec3 tmpvar_26;
            tmpvar_26.x = worldTangent_3.x;
            tmpvar_26.y = worldBinormal_1.x;
            tmpvar_26.z = tmpvar_21.x;
            highp vec3 tmpvar_27;
            tmpvar_27.x = worldTangent_3.y;
            tmpvar_27.y = worldBinormal_1.y;
            tmpvar_27.z = tmpvar_21.y;
            highp vec3 tmpvar_28;
            tmpvar_28.x = worldTangent_3.z;
            tmpvar_28.y = worldBinormal_1.z;
            tmpvar_28.z = tmpvar_21.z;
            gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_19));
            xlv_TEXCOORD0 = tmpvar_4;
            xlv_TEXCOORD1 = ((tmpvar_7.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
            xlv_TEXCOORD2 = tmpvar_26;
            xlv_TEXCOORD3 = tmpvar_27;
            xlv_TEXCOORD4 = tmpvar_28;
            xlv_TEXCOORD5 = (unity_ObjectToWorld * tmpvar_5).xyz;
            xlv_COLOR0 = _glesColor;
            xlv_TEXCOORD6 = tmpvar_8;
            xlv_TEXCOORD7 = (tmpvar_18 * (_WorldSpaceCameraPos - (unity_ObjectToWorld * tmpvar_5).xyz));
            xlv_TEXCOORD8 = (unity_WorldToLight * (unity_ObjectToWorld * tmpvar_5)).xy;
          }
          
          
          #endif
          #ifdef FRAGMENT
          uniform highp vec4 _Time;
          uniform highp vec3 _WorldSpaceCameraPos;
          uniform mediump vec4 _WorldSpaceLightPos0;
          uniform highp mat4 unity_ObjectToWorld;
          uniform lowp vec4 _LightColor0;
          uniform lowp vec4 _SpecColor;
          uniform highp sampler2D _LightTexture0;
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
          uniform highp float _Bevel;
          uniform highp float _BevelOffset;
          uniform highp float _BevelWidth;
          uniform highp float _BevelClamp;
          uniform highp float _BevelRoundness;
          uniform sampler2D _BumpMap;
          uniform highp float _BumpOutline;
          uniform highp float _BumpFace;
          uniform lowp samplerCube _Cube;
          uniform lowp vec4 _ReflectFaceColor;
          uniform lowp vec4 _ReflectOutlineColor;
          uniform highp float _ShaderFlags;
          uniform highp float _ScaleRatioA;
          uniform sampler2D _MainTex;
          uniform highp float _TextureWidth;
          uniform highp float _TextureHeight;
          uniform highp float _GradientScale;
          uniform mediump float _FaceShininess;
          uniform mediump float _OutlineShininess;
          varying highp vec4 xlv_TEXCOORD0;
          varying highp vec2 xlv_TEXCOORD1;
          varying highp vec3 xlv_TEXCOORD2;
          varying highp vec3 xlv_TEXCOORD3;
          varying highp vec3 xlv_TEXCOORD4;
          varying highp vec3 xlv_TEXCOORD5;
          varying lowp vec4 xlv_COLOR0;
          varying highp vec2 xlv_TEXCOORD6;
          varying highp vec3 xlv_TEXCOORD7;
          varying highp vec2 xlv_TEXCOORD8;
          void main ()
          {
            mediump vec3 tmpvar_1;
            mediump vec3 tmpvar_2;
            highp vec3 worldN_3;
            lowp float atten_4;
            lowp vec3 tmpvar_5;
            lowp vec3 tmpvar_6;
            lowp vec3 tmpvar_7;
            lowp float tmpvar_8;
            lowp float tmpvar_9;
            highp vec3 worldViewDir_10;
            lowp vec3 lightDir_11;
            lowp vec3 _unity_tbn_2_12;
            lowp vec3 _unity_tbn_1_13;
            lowp vec3 _unity_tbn_0_14;
            _unity_tbn_0_14 = xlv_TEXCOORD2;
            _unity_tbn_1_13 = xlv_TEXCOORD3;
            _unity_tbn_2_12 = xlv_TEXCOORD4;
            mediump vec3 tmpvar_15;
            tmpvar_15 = _WorldSpaceLightPos0.xyz;
            lightDir_11 = tmpvar_15;
            worldViewDir_10 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD5));
            tmpvar_5 = vec3(0.0, 0.0, 0.0);
            tmpvar_7 = vec3(0.0, 0.0, 0.0);
            tmpvar_9 = 0.0;
            tmpvar_8 = 0.0;
            tmpvar_6 = vec3(0.0, 0.0, 1.0);
            lowp vec3 tmpvar_16;
            lowp vec3 tmpvar_17;
            lowp vec3 tmpvar_18;
            lowp float tmpvar_19;
            lowp float tmpvar_20;
            tmpvar_16 = tmpvar_5;
            tmpvar_17 = tmpvar_6;
            tmpvar_18 = tmpvar_7;
            tmpvar_19 = tmpvar_8;
            tmpvar_20 = tmpvar_9;
            highp vec3 bump_21;
            highp vec4 outlineColor_22;
            highp vec4 faceColor_23;
            highp float c_24;
            highp vec4 smp4x_25;
            highp vec3 tmpvar_26;
            tmpvar_26.z = 0.0;
            tmpvar_26.x = (1.0/(_TextureWidth));
            tmpvar_26.y = (1.0/(_TextureHeight));
            highp vec2 P_27;
            P_27 = (xlv_TEXCOORD0.xy - tmpvar_26.xz);
            highp vec2 P_28;
            P_28 = (xlv_TEXCOORD0.xy + tmpvar_26.xz);
            highp vec2 P_29;
            P_29 = (xlv_TEXCOORD0.xy - tmpvar_26.zy);
            highp vec2 P_30;
            P_30 = (xlv_TEXCOORD0.xy + tmpvar_26.zy);
            lowp vec4 tmpvar_31;
            tmpvar_31.x = texture2D (_MainTex, P_27).w;
            tmpvar_31.y = texture2D (_MainTex, P_28).w;
            tmpvar_31.z = texture2D (_MainTex, P_29).w;
            tmpvar_31.w = texture2D (_MainTex, P_30).w;
            smp4x_25 = tmpvar_31;
            lowp float tmpvar_32;
            tmpvar_32 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
            c_24 = tmpvar_32;
            highp float tmpvar_33;
            tmpvar_33 = (((
              (0.5 - c_24)
             - xlv_TEXCOORD6.x) * xlv_TEXCOORD6.y) + 0.5);
            highp float tmpvar_34;
            tmpvar_34 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD6.y);
            highp float tmpvar_35;
            tmpvar_35 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD6.y);
            faceColor_23 = _FaceColor;
            outlineColor_22 = _OutlineColor;
            faceColor_23 = (faceColor_23 * xlv_COLOR0);
            outlineColor_22.w = (outlineColor_22.w * xlv_COLOR0.w);
            highp vec2 tmpvar_36;
            tmpvar_36.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
            tmpvar_36.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
            lowp vec4 tmpvar_37;
            tmpvar_37 = texture2D (_FaceTex, tmpvar_36);
            faceColor_23 = (faceColor_23 * tmpvar_37);
            highp vec2 tmpvar_38;
            tmpvar_38.x = (xlv_TEXCOORD1.x + (_OutlineUVSpeedX * _Time.y));
            tmpvar_38.y = (xlv_TEXCOORD1.y + (_OutlineUVSpeedY * _Time.y));
            lowp vec4 tmpvar_39;
            tmpvar_39 = texture2D (_OutlineTex, tmpvar_38);
            outlineColor_22 = (outlineColor_22 * tmpvar_39);
            mediump float d_40;
            d_40 = tmpvar_33;
            lowp vec4 faceColor_41;
            faceColor_41 = faceColor_23;
            lowp vec4 outlineColor_42;
            outlineColor_42 = outlineColor_22;
            mediump float outline_43;
            outline_43 = tmpvar_34;
            mediump float softness_44;
            softness_44 = tmpvar_35;
            mediump float tmpvar_45;
            tmpvar_45 = (1.0 - clamp ((
              ((d_40 - (outline_43 * 0.5)) + (softness_44 * 0.5))
             / 
              (1.0 + softness_44)
            ), 0.0, 1.0));
            faceColor_41.xyz = (faceColor_41.xyz * faceColor_41.w);
            outlineColor_42.xyz = (outlineColor_42.xyz * outlineColor_42.w);
            mediump vec4 tmpvar_46;
            tmpvar_46 = mix (faceColor_41, outlineColor_42, vec4((clamp (
              (d_40 + (outline_43 * 0.5))
            , 0.0, 1.0) * sqrt(
              min (1.0, outline_43)
            ))));
            faceColor_41 = tmpvar_46;
            faceColor_41 = (faceColor_41 * tmpvar_45);
            faceColor_23 = faceColor_41;
            faceColor_23.xyz = (faceColor_23.xyz / max (faceColor_23.w, 0.0001));
            highp vec4 h_47;
            h_47 = smp4x_25;
            highp float tmpvar_48;
            tmpvar_48 = (_ShaderFlags / 2.0);
            highp float tmpvar_49;
            tmpvar_49 = (fract(abs(tmpvar_48)) * 2.0);
            highp float tmpvar_50;
            if ((tmpvar_48 >= 0.0)) {
              tmpvar_50 = tmpvar_49;
            } else {
              tmpvar_50 = -(tmpvar_49);
            };
            h_47 = (smp4x_25 + (xlv_TEXCOORD6.x + _BevelOffset));
            highp float tmpvar_51;
            tmpvar_51 = max (0.01, (_OutlineWidth + _BevelWidth));
            h_47 = (h_47 - 0.5);
            h_47 = (h_47 / tmpvar_51);
            highp vec4 tmpvar_52;
            tmpvar_52 = clamp ((h_47 + 0.5), 0.0, 1.0);
            h_47 = tmpvar_52;
            if (bool(float((tmpvar_50 >= 1.0)))) {
              h_47 = (1.0 - abs((
                (tmpvar_52 * 2.0)
               - 1.0)));
            };
            h_47 = (min (mix (h_47, 
              sin(((h_47 * 3.141592) / 2.0))
            , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((_Bevel * tmpvar_51) * (_GradientScale * -2.0)));
            highp vec3 tmpvar_53;
            tmpvar_53.xy = vec2(1.0, 0.0);
            tmpvar_53.z = (h_47.y - h_47.x);
            highp vec3 tmpvar_54;
            tmpvar_54 = normalize(tmpvar_53);
            highp vec3 tmpvar_55;
            tmpvar_55.xy = vec2(0.0, -1.0);
            tmpvar_55.z = (h_47.w - h_47.z);
            highp vec3 tmpvar_56;
            tmpvar_56 = normalize(tmpvar_55);
            lowp vec3 tmpvar_57;
            tmpvar_57 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
            bump_21 = tmpvar_57;
            bump_21 = (bump_21 * mix (_BumpFace, _BumpOutline, clamp (
              (tmpvar_33 + (tmpvar_34 * 0.5))
            , 0.0, 1.0)));
            highp vec3 tmpvar_58;
            tmpvar_58 = mix (vec3(0.0, 0.0, 1.0), bump_21, faceColor_23.www);
            bump_21 = tmpvar_58;
            highp vec3 tmpvar_59;
            tmpvar_59 = normalize(((
              (tmpvar_54.yzx * tmpvar_56.zxy)
             - 
              (tmpvar_54.zxy * tmpvar_56.yzx)
            ) - tmpvar_58));
            highp mat3 tmpvar_60;
            tmpvar_60[0] = unity_ObjectToWorld[0].xyz;
            tmpvar_60[1] = unity_ObjectToWorld[1].xyz;
            tmpvar_60[2] = unity_ObjectToWorld[2].xyz;
            highp vec3 tmpvar_61;
            highp vec3 N_62;
            N_62 = (tmpvar_60 * tmpvar_59);
            tmpvar_61 = (xlv_TEXCOORD7 - (2.0 * (
              dot (N_62, xlv_TEXCOORD7)
             * N_62)));
            lowp vec4 tmpvar_63;
            tmpvar_63 = textureCube (_Cube, tmpvar_61);
            highp float tmpvar_64;
            tmpvar_64 = clamp ((tmpvar_33 + (tmpvar_34 * 0.5)), 0.0, 1.0);
            lowp vec3 tmpvar_65;
            tmpvar_65 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_64));
            highp vec3 tmpvar_66;
            tmpvar_66 = ((tmpvar_63.xyz * tmpvar_65) * faceColor_23.w);
            tmpvar_16 = faceColor_23.xyz;
            tmpvar_17 = -(tmpvar_59);
            tmpvar_18 = tmpvar_66;
            highp float tmpvar_67;
            tmpvar_67 = clamp ((tmpvar_33 + (tmpvar_34 * 0.5)), 0.0, 1.0);
            tmpvar_19 = 1.0;
            tmpvar_20 = faceColor_23.w;
            tmpvar_5 = tmpvar_16;
            tmpvar_7 = tmpvar_18;
            tmpvar_8 = tmpvar_19;
            tmpvar_9 = tmpvar_20;
            highp float tmpvar_68;
            tmpvar_68 = texture2D (_LightTexture0, xlv_TEXCOORD8).w;
            atten_4 = tmpvar_68;
            lowp float tmpvar_69;
            tmpvar_69 = dot (_unity_tbn_0_14, tmpvar_17);
            worldN_3.x = tmpvar_69;
            lowp float tmpvar_70;
            tmpvar_70 = dot (_unity_tbn_1_13, tmpvar_17);
            worldN_3.y = tmpvar_70;
            lowp float tmpvar_71;
            tmpvar_71 = dot (_unity_tbn_2_12, tmpvar_17);
            worldN_3.z = tmpvar_71;
            highp vec3 tmpvar_72;
            tmpvar_72 = normalize(worldN_3);
            worldN_3 = tmpvar_72;
            tmpvar_6 = tmpvar_72;
            tmpvar_1 = _LightColor0.xyz;
            tmpvar_2 = lightDir_11;
            tmpvar_1 = (tmpvar_1 * atten_4);
            mediump vec3 viewDir_73;
            viewDir_73 = worldViewDir_10;
            lowp vec4 c_74;
            lowp vec4 c_75;
            highp float nh_76;
            lowp float diff_77;
            mediump float tmpvar_78;
            tmpvar_78 = max (0.0, dot (tmpvar_6, tmpvar_2));
            diff_77 = tmpvar_78;
            mediump float tmpvar_79;
            tmpvar_79 = max (0.0, dot (tmpvar_6, normalize(
              (tmpvar_2 + viewDir_73)
            )));
            nh_76 = tmpvar_79;
            mediump float y_80;
            y_80 = (mix (_FaceShininess, _OutlineShininess, tmpvar_67) * 128.0);
            highp float tmpvar_81;
            tmpvar_81 = pow (nh_76, y_80);
            c_75.xyz = (((tmpvar_16 * tmpvar_1) * diff_77) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_81));
            c_75.w = tmpvar_20;
            c_74.w = c_75.w;
            c_74.xyz = c_75.xyz;
            gl_FragData[0] = c_74;
          }
          
          
          #endif
          
          "
        }
      }
      Program "fp"
      {
        SubProgram "gles hw_tier00"
        {
           Keywords { "POINT" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier01"
        {
           Keywords { "POINT" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier02"
        {
           Keywords { "POINT" }
          
          "!!!!GLES
          
          
          "
        }
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
           Keywords { "SPOT" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier01"
        {
           Keywords { "SPOT" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier02"
        {
           Keywords { "SPOT" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier00"
        {
           Keywords { "POINT_COOKIE" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier01"
        {
           Keywords { "POINT_COOKIE" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier02"
        {
           Keywords { "POINT_COOKIE" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier00"
        {
           Keywords { "DIRECTIONAL_COOKIE" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier01"
        {
           Keywords { "DIRECTIONAL_COOKIE" }
          
          "!!!!GLES
          
          
          "
        }
        SubProgram "gles hw_tier02"
        {
           Keywords { "DIRECTIONAL_COOKIE" }
          
          "!!!!GLES
          
          
          "
        }
      }
      
    } // end phase
    Pass // ind: 3, name: Caster
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
      GpuProgramID 162456
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
