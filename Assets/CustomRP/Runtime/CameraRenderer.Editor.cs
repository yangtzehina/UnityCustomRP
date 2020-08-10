﻿using UnityEngine;
using UnityEngine.Rendering;

namespace CustomRP.Runtime
{
    partial class CameraRenderer
    {
        partial void DrawUnsupportedShaders();
        
#if UNITY_EDITOR
        private static Material errorMaterial;

        private static ShaderTagId[] _legacyShaderTagIds = new[]
        {
            new ShaderTagId("Always"),
            new ShaderTagId("ForwardBase"),
            new ShaderTagId("PrepassBase"),
            new ShaderTagId("Vertex"),
            new ShaderTagId("VertexLMRGBM"),
            new ShaderTagId("VertexLM"), 
        };
        
        partial void DrawUnsupportedShaders()
        {
            if (errorMaterial == null)
            {
                errorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
            }
            var drawingSettings = new DrawingSettings(_legacyShaderTagIds[0],new SortingSettings(mCamera))
            {
                overrideMaterial = errorMaterial
            };
            for (int i = 1; i < _legacyShaderTagIds.Length; i++)
            {
                drawingSettings.SetShaderPassName(i,_legacyShaderTagIds[i]);
            }
            var filteringSettings = FilteringSettings.defaultValue;
            mContext.DrawRenderers(mCullingResults,ref drawingSettings,ref filteringSettings);
        }  
#endif
    }
}