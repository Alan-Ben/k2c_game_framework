using ALPackage;
using System;
using System.Text;

namespace GOE
{
    /// <summary>
    /// 游戏初始化过程处理函数
    /// 语言相关资源下载加载处理
    /// </summary>
    public class GameInit_LanguageResLoad : _AGameInitProcess, _INPPGUILoadingBkProcessRefresher
    {
        private static GameInit_LanguageResLoad _g_instance = new GameInit_LanguageResLoad();
        public static GameInit_LanguageResLoad instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GameInit_LanguageResLoad();

                return _g_instance;
            }
        }
        
        //最后一次记录的下载信息
        private string _m_sLastDownloadProcessMsg;
        
        //全体资源加载的进度记录对象
        private ALProcessSubNode _m_pAllResProcess;
        //下载监控对象
        private ALAssetBundleGroupMonitor _m_gmGroupMonitor;

        private static readonly string _m_gProcessName = "GameInit_LanguageResLoad";
        public static string gProcessName { get { return _m_gProcessName; } }
        
        protected GameInit_LanguageResLoad()
        {
            _m_pAllResProcess = new ALProcessSubNode();
            _m_sLastDownloadProcessMsg = string.Empty;
        }


        protected override _IALProgressnterface[] getChildProcess()
        {
            return new _IALProgressnterface[] {
                _m_pAllResProcess };
        }

        protected override void _resetData()
        {
            _m_sLastDownloadProcessMsg = string.Empty;

            if (_m_gmGroupMonitor != null) 
                _m_gmGroupMonitor.discard();
            _m_gmGroupMonitor = null;
            
            //重置进度对象
            if (_m_pAllResProcess != null) 
                _m_pAllResProcess.reset();
        }

        protected override void _dealInit(Action _doneDelegate)
        {
            string[] pathList = _getLanguageAssetPath();
            if(null == pathList)
                return;

            //发送埋点-更新语言资源
            GCommon.sendStepReport(TraceConst.START_UPDATE_LANGUAGE_RES.setMarkParam(GameSetting.instance.getCurrentLanguage()));

            //创建下载ab组
            ALAssetBundleDownloadGroup downloadGroup = new ALAssetBundleDownloadGroup(RefdataResCore.instance);   
            foreach (string path in pathList)
            {
                downloadGroup.addLoadItem(path);
            }

            //创建监控对象开始下载
            _m_gmGroupMonitor = new ALAssetBundleGroupMonitor(downloadGroup, _onDownloadingGameRes, (_failCount) =>
            {
                //发送埋点-更新语言资源完成
                GCommon.sendStepReport(TraceConst.UPDATE_LANGUAGE_RES_DONE);
                if (_doneDelegate != null) 
                    _doneDelegate();
            });
            _m_gmGroupMonitor.startMonitor();
        }

        //获取语言相关的ab包，后续有添加种类要补到这边
        private string[] _getLanguageAssetPath()
        {
            //语言相关包
            ENPLanguage curLanguage = GameSetting.instance.getCurrentLanguage();
            string languageAssetPath = LanuageAsset.getLanguageAssetPath(curLanguage);
            string playerNameAssetPath = MultiLanguageAsset.getLanguageAssetPath(curLanguage, GSOPlayerNameRefSet.objName);
            string childNameAssetPath = MultiLanguageAsset.getLanguageAssetPath(curLanguage, GSOChildNameRefSet.objName);

            return new string[]
            {
                languageAssetPath,
                playerNameAssetPath,
                childNameAssetPath
            };
        }
        
        /// <summary>
        /// 获取当前操作的文本
        /// </summary>
        public string curOPTxt { get { return _m_sLastDownloadProcessMsg; } }
        /// <summary>
        /// 刷新间隔
        /// </summary>
        public float refreshDuration { get { return 0.2f; } }
        
        
        protected void _onDownloadingGameRes(long _loadedSize, long _unzipSize, long _totalSize, bool _isDownloading)
        {
            //计算进度
            double process = (double)_loadedSize / _totalSize;

            //设置进度
            _setProcess(0.2f + (float)(process * 0.8f));

            //拼凑显示文字
            StringBuilder txt = new StringBuilder();
            txt.Append(TextTranslate.instance.getLanguage(TransKeyConst.login_res_downloading));
            GCommon.appendFileSize(_loadedSize, txt);
            txt.Append("/");
            GCommon.appendFileSize(_totalSize, txt);

            //设置最后的下载进度信息
            _m_sLastDownloadProcessMsg = txt.ToString();
        }
        
        /// <summary>
        /// 设置当前进度
        /// </summary>
        /// <param name="_process"></param>
        protected void _setProcess(float _process)
        {
            if (_m_pAllResProcess != null) 
                _m_pAllResProcess.setProcess(_process);
        }
    }
}
