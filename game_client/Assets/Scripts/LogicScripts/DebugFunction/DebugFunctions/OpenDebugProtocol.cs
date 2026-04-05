using ALPackage;

namespace GOE
{
    /// <summary>
    /// 开启协议打印
    /// </summary>
    public class OpenDebugProtocol : AFileDebugFunction
    {
        public OpenDebugProtocol(string _filePath) : base(_filePath)
        {
        }

        /// <summary>
        /// 调试文件是否有内容
        /// </summary>
        protected override bool hasContent
        {
            get { return false; }
        }

        /// <summary>
        /// 根据Debug文件内的内容，刷新自己需要的信息
        /// </summary>
        /// <param name="_content"></param>
        protected override void _doRefreshStatus(string _content)
        {
        }

        /// <summary>
        /// 调试功能开启时，要如何调用其他逻辑
        /// </summary>
        protected override void _doOpenFunction()
        {
            if(MainCameraMono.selfInstance != null)
            {
                MainCameraMono.selfInstance.gameSetting.printProtocol = true;
            }
            else
            {
                Debug.LogError("_AALMonoMain.instance 不存在");
            }
        }

        /// <summary>
        /// 调试功能关闭时，要如何调用其他逻辑
        /// </summary>
        protected override void _doCloseFunction()
        {
            if(MainCameraMono.selfInstance != null)
            {
                MainCameraMono.selfInstance.gameSetting.printProtocol = false;
            }
            else
            {
                Debug.LogError("_AALMonoMain.instance 不存在");
            }
        }
    }
}