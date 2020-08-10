using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Profiling;

namespace CustomRP.Runtime
{
    partial class CameraRenderer
    {
        partial void DrawUnsupportedShaders();
        partial void DrawGizmos();
        partial void PrepareForSceneWindow();
        partial void PrepareBuffer();
        
#if UNITY_EDITOR
        private static Material errorMaterial;

        private string SampleName { get; set; }
        
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

        partial void DrawGizmos()
        {
            if (Handles.ShouldRenderGizmos())
            {
                mContext.DrawGizmos(mCamera,GizmoSubset.PreImageEffects);
                mContext.DrawGizmos(mCamera,GizmoSubset.PostImageEffects);
            }
        }

        partial void PrepareForSceneWindow()
        {
            if (mCamera.cameraType == CameraType.SceneView)
            {
                ScriptableRenderContext.EmitWorldGeometryForSceneView(mCamera);
            }
        }

        partial void PrepareBuffer()
        {
            Profiler.BeginSample("Editor Only");
            mCommandBuffer.name = SampleName =mCamera.name;
            Profiler.EndSample();
        }
#else
        
        const string SampleName = bufferName;

#endif
    }
}