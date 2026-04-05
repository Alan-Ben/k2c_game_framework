using ALPackage;

namespace GOE
{
    /// <summary>
    /// 开启showDebugOutput
    /// </summary>
    public class OpenDebugFunction : AFileDebugFunction
    {
        public OpenDebugFunction(string _filePath) : base(_filePath)
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
            if(_AALMonoMain.instance != null)
            {
                _AALMonoMain.instance.showDebugOutput = true;
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
            if(_AALMonoMain.instance != null)
            {
                _AALMonoMain.instance.showDebugOutput = false;
            }
            else
            {
                Debug.LogError("_AALMonoMain.instance 不存在");
            }
        }
    }
}