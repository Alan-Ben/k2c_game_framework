using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 举报弹窗
    /// </summary>
    public class GGUIWndPlayerReportEditor : _ANPGGUIBasicWnd<GGUIMonoPlayerReportEditor>
    {
        private static GGUIWndPlayerReportEditor _g_instance;
        public static GGUIWndPlayerReportEditor instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndPlayerReportEditor();
                return _g_instance;
            }
        }

        // 被举报玩家CID
        private long _m_lTargetCid;

        public GGUIWndPlayerReportEditor() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoPlayerReportEditor.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoPlayerReportEditor.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (wnd.reportInputField != null)
                wnd.reportInputField.onValueChanged?.RemoveListener(_onInputValueChanged);

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnSend, _onBtnSendClick);
        }

        /// <summary>
        /// 窗口初始化完成时绑定按钮事件
        /// </summary>
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.reportInputField != null)
                wnd.reportInputField.onValueChanged?.AddListener(_onInputValueChanged);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnSend, _onBtnSendClick);
        }

        /// <summary>
        /// 设置举报目标信息
        /// </summary>
        /// <param name="_targetCid">被举报玩家CID</param>
        public void setInfo(long _targetCid)
        {
            _m_lTargetCid = _targetCid;
            _refreshWnd();
        }

        // 刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            _refreshInputCount();
        }

        // 刷新字数显示
        private void _refreshInputCount(string _value = null)
        {
            if (wnd == null)
                return;

            int currentLength = string.IsNullOrEmpty(_value) ? 0 : CharacterDetermineMgr.instance.getUnicodeStringLength(_value);
            ALUGUICommon.setLabelTxt(wnd.txtInputCount, TextTranslate.instance.getLanguage(TransKeyConst.common_words_limit_tip, currentLength, wnd.maxInputNum));
        }

        /// <summary>
        /// 输入框内容变化时更新字数显示
        /// </summary>
        private void _onInputValueChanged(string _value)
        {
            _refreshInputCount(_value);
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        private void _onBtnCloseClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_PlayerInfo.C_ADD_PLAYER_REPORT);
        }

        /// <summary>
        /// 点击发送举报按钮
        /// </summary>
        private void _onBtnSendClick(GameObject _go)
        {
            if (wnd == null)
                return;

            string content = wnd.reportInputField != null ? wnd.reportInputField.text : string.Empty;

            //判断输入文本是否为空
            if (string.IsNullOrEmpty(content))
            {
                //请填写举报说明
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.chat_reportInputEmptyTip_none);
                return;
            }

            //判断输入文本长度是否超过限制
            if (!CharacterDetermineMgr.instance.isSuitableLength(content, 0, wnd.maxInputNum, true))
                return;

            //上次举报时间
            long lastReportTime = AccountSettingMgr.instance.accountSetting.getPlayerReportTimeRecord(_m_lTargetCid);
            //是否在冷却中
            bool isInCD = (lastReportTime > 0) &&
                          ((FpsAndPingMgr.instance.serverTimeTagS - lastReportTime) / 3600 < GRefdataCoreMgr.instance.npGeneral.player_report_cd_time_hour);
            if (!isInCD)
            {
                //记录举报时间
                AccountSettingMgr.instance.accountSetting.setPlayerReportTimeRecord(_m_lTargetCid,FpsAndPingMgr.instance.serverTimeTagS);
                //发送举报请求
                NPPlayer.instance.chatComp.reqReportPlayer(_m_lTargetCid, content);
                //举报发送成功
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.chat_reportSendSuc_none);
            }
            else
            {
                //您已举报该玩家，客服将尽快处理！
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.chat_alreadyReport_none);
            }
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_PlayerInfo.C_ADD_PLAYER_REPORT);
        }
    }
}
