using ALPackage;

namespace GOE
{
    /// <summary>
    /// 任务完成提示界面
    /// </summary>
    public class GGUIWndQuestCompleteTips : _ANPGGUIBasicWnd<GGUIMonoQuestCompleteTips>
    {

        private static GGUIWndQuestCompleteTips _g_instance;
        public static GGUIWndQuestCompleteTips instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndQuestCompleteTips();
                return _g_instance;
            }
        }

        private long _m_lOpSerialize;//操作序列号

        protected GGUIWndQuestCompleteTips() : base(EALUIWndLayer.NOTICE)
        {
        }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return GGUIMonoQuestCompleteTips.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoQuestCompleteTips.objName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            _m_lOpSerialize++;
        }

        protected override void _onHideWnd()
        {
            _m_lOpSerialize++;
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_questName"></param>
        public void setInfo(string _questName)
        {
            if (wnd == null)
                return;

            _m_lOpSerialize++;
            long serialize = _m_lOpSerialize;

            ALUGUICommon.setLabelTxt(wnd.txtName, _questName);

            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if (serialize != _m_lOpSerialize)
                    return;

                hideWnd();
            }, wnd.delayCloseTime);
        }
    }
}
