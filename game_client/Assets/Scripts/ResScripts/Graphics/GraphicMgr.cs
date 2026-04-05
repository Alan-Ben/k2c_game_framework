using ALPackage;
using UnityEngine;
using UnityEngine.Rendering.Universal;


namespace GOE
{
	/// <summary>
	/// 全局的图像部分控制管理器对象
	/// </summary>
	public class GraphicMgr
	{
	    private static GraphicMgr _g_instance = new GraphicMgr();
	    public static GraphicMgr instance
	    {
	        get
	        {
	            if (null == _g_instance)
	                _g_instance = new GraphicMgr();
	            return _g_instance;
	        }
	    }

	    //TODO 更具当前硬件评分返回合适的尺寸
	    public RenderTextureDescriptor getEnvLightRTDescriptor()
	    {
	        return new RenderTextureDescriptor(Screen.width, Screen.height);
	    }

	    /// <summary>
	    /// 每帧刷新的方法
	    /// </summary>
	    public void update()
	    {
		    
	    }
	}
}