using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟日志界面
    /// </summary>
    public class GGUIWndGuildLog : _ANPGGUIBasicWnd<GGUIMonoGuildLog>
    {
        private static GGUIWndGuildLog _g_instance = new GGUIWndGuildLog();
        public static GGUIWndGuildLog instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildLog();
                return _g_instance;
            }
        }

        //日志列表
        private GGUIWndGuildLogGrid _m_wLogGrid;
        //显示序列
        private long _m_lShowSerialize;

        public GGUIWndGuildLog() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildLog.assetPath; } }

        protected override string _monoObjName { get { return GGUIMonoGuildLog.objName; } }

        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_wLogGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wLogGrid?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wLogGrid?.discard();
            _m_wLogGrid = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoGrid != null)
                _m_wLogGrid = new GGUIWndGuildLogGrid(wnd.monoGrid);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            int maxShowCount = GRefdataCoreMgr.instance.npGeneral.guild_log_max_show_count;
            long serialize = _m_lShowSerialize;

            //请求列表
            NPPlayer.instance.guildComp.reqGuildLogList(0, maxShowCount, (_infoList) =>
            {
                if (wnd == null || serialize != _m_lShowSerialize)
                    return;

                //展示日志列表
                _m_wLogGrid?.showWnd();
                _m_wLogGrid?.setShowData(_infoList);
            });

            //描述
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(TransKeyConst.guild_logShowMaxCountDesc_num, maxShowCount));
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_LOG);
        }
    }
}