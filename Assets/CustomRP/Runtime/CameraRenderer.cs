using UnityEngine;
using UnityEngine.Rendering;

namespace CustomRP.Runtime
{
    public partial class CameraRenderer
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
            PrepareBuffer();
            PrepareForSceneWindow();
             if (!Cull())
             {
                 return;
             }
            Setup();
            DrawVisibleGeometry();
            DrawUnsupportedShaders();
            DrawGizmos();
            Submit();
        }
        
        void DrawVisibleGeometry()
        {
            var sortingSettings = new SortingSettings(mCamera)
            {
                criteria =  SortingCriteria.CommonOpaque
            };
            var drawingSettings = new DrawingSettings(_unlitShaderTagId,sortingSettings);
            var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
            
            mContext.DrawRenderers(mCullingResults,ref drawingSettings,ref filteringSettings);
            mContext.DrawSkybox(mCamera);
            sortingSettings.criteria = SortingCriteria.CommonTransparent;
            drawingSettings.sortingSettings = sortingSettings;
            filteringSettings.renderQueueRange = RenderQueueRange.transparent;
            mContext.DrawRenderers(mCullingResults,ref drawingSettings,ref filteringSettings);
        }

        void Submit()
        {   
            mCommandBuffer.EndSample(SampleName);
            ExecuteBuffer();
            mContext.Submit();
        }

        void Setup()
        {
            mContext.SetupCameraProperties(mCamera);
            CameraClearFlags flags = mCamera.clearFlags;
            mCommandBuffer.ClearRenderTarget(true,true,Color.clear);
            mCommandBuffer.BeginSample(SampleName);
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