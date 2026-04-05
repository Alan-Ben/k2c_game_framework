using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace GOE
{
    public partial class CommonQualityMgr
    {
        private RenderTextureDescriptor _m_renderTextureDescriptor = new RenderTextureDescriptor(1024,1024, GraphicsFormat.R8G8B8A8_SRGB,24);
        private float _m_screenRTScaleFactor = 1;

        //根据当前手机等级获取合适的Rendertexture设置
        public RenderTextureDescriptor getRTDescriptor()
        {
            return _m_renderTextureDescriptor;
        }
        
        //缩放系数
        public float screenRTScaleFactor
        {
            get { return _m_screenRTScaleFactor; }
        }

        /// <summary>
        /// 根据内存大小设置RT的大小
        /// </summary>
        public void setRenderTextureDescriptor()
        {
            int size = 1024;
            int depth = 24;
            
            float memorySize = SystemInfo.systemMemorySize;
            size = (int) (Screen.height * memorySize.RemapClamp(512, 2048, 0.3f, 1f));

            _m_screenRTScaleFactor = memorySize.RemapClamp(512, 2048, 0.5f, 1f);
             
            if(memorySize < 1500)
                depth = 16;

            if(SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32))
            {
                _m_renderTextureDescriptor = new RenderTextureDescriptor(size, size, SystemInfo.GetGraphicsFormat(DefaultFormat.LDR), depth);
                Debug.Log($"[Quality] setRenderTextureDescriptor：{size} * {size}, RenderTextureFormat.ARGB32, {depth}");
            }
            else
            {
                _m_renderTextureDescriptor = new RenderTextureDescriptor(size, size, RenderTextureFormat.Default, depth);
                Debug.Log($"[Quality] setRenderTextureDescriptor：{size} * {size}, RenderTextureFormat.Default, {depth}");
            }
            _m_renderTextureDescriptor.useMipMap = false; //虽然默认也是false，但是还是set一下吧
            _m_renderTextureDescriptor.enableRandomWrite = false; //虽然默认也是false，但是还是set一下吧
        }
    }
}