using UnityEngine;
using UnityEngine.Rendering;

namespace CustomRP.Runtime
{
    public class CameraRenderer
    {
        private ScriptableRenderContext mContext;
        private Camera mCamera;
        private const string bufferName = "Render Camera"; 
        CommandBuffer mCommandBuffer = new CommandBuffer{name = bufferName};
        private CullingResults mCullingResults;
        static ShaderTagId _unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");
        public void Render(ScriptableRenderContext context, Camera camera)
        {
            this.mContext = context;
            this.mCamera = camera;
             if (!Cull())
             {
                 return;
             }
            Setup();
            DrawVisibleGeometry();
            Submit();
        }

        void DrawVisibleGeometry()
        {
            var sortingSettings = new SortingSettings(mCamera);
            var drawingSettings = new DrawingSettings(_unlitShaderTagId,sortingSettings);
            var filteringSettings = new FilteringSettings();
            
            mContext.DrawRenderers(mCullingResults,ref drawingSettings,ref filteringSettings);
            mContext.DrawSkybox(mCamera);
        }

        void Submit()
        {   
            mCommandBuffer.EndSample(bufferName);
            ExecuteBuffer();
            mContext.Submit();
        }

        void Setup()
        {
            mContext.SetupCameraProperties(mCamera);
            mCommandBuffer.ClearRenderTarget(true,true,Color.clear);
            mCommandBuffer.BeginSample(bufferName);
            ExecuteBuffer();
        }

        void ExecuteBuffer()
        {
            mContext.ExecuteCommandBuffer(mCommandBuffer);
            mCommandBuffer.Clear();
        }

        bool Cull()
        {
            if (mCamera.TryGetCullingParameters(out ScriptableCullingParameters p))
            {
                mCullingResults = mContext.Cull(ref p);
                return true;
            }
            return false;
        }
    }
}