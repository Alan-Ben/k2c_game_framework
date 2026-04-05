using UnityEngine;
using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 游戏设置界面
    /// </summary>
    public class NPGGUIWndGameSetting : _ANPGGUIBasicResBarWnd<NPGGUIMonoGameSetting>
    {

        private static NPGGUIWndGameSetting _g_instance;
        public static NPGGUIWndGameSetting instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new NPGGUIWndGameSetting();
                return _g_instance;
            }
        }

        //玩家信息
        private NPGGUIWndPlayerIcon _m_playerIconWnd;

        // 定时task
        private ALCommonEnableTaskController _m_iCheckDelTask;

        protected NPGGUIWndGameSetting() : base(EALUIWndLayer.ADDITION)
        {
        }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return NPGGUIMonoGameSetting.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoGameSetting.objName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        /// <summary>
        /// 在窗口作为Scene中的主展示窗口的时候，在切换时是否会需要释放
        /// </summary>
        public override bool needDiscardOnSwitch { get { return true; } }


        protected override void _onShowWnd()
        {
            _refresh();
            _m_iCheckDelTask.setDisable();
            _m_iCheckDelTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_tickDeal, 1.0f);
        }

        protected override void _onHideWnd()
        {
            _m_iCheckDelTask.setDisable();
        }

        protected override void _onReset()
        {

        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (null != _m_playerIconWnd)
                _m_playerIconWnd.discard();
            _m_playerIconWnd = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnSwitchLanguage, _onClickSwitchLan);
            ALUGUICommon.uncombineBtnClick(wnd.btnAccount, _onClickAccount);
            ALUGUICommon.uncombineBtnClick(wnd.btnAudio, _onClickAudio);
            ALUGUICommon.uncombineBtnClick(wnd.btnCopy, _onClickCopyBtn);
            ALUGUICommon.uncombineBtnClick(wnd.btnEffect, _onClickEffect);
            ALUGUICommon.uncombineBtnClick(wnd.btnLocalPush, _onClickLocalPushSetting);
            ALUGUICommon.uncombineBtnClick(wnd.btnChangeServer, _onClickChangeServer);
            ALUGUICommon.uncombineBtnClick(wnd.btnAIHelp, _onClickAIHelp);//点击客服按钮
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);//点击关闭按钮
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (null != wnd.playerIconMono)
                _m_playerIconWnd = new NPGGUIWndPlayerIcon(wnd.playerIconMono);

            ALUGUICommon.combineBtnClick(wnd.btnSwitchLanguage,_onClickSwitchLan);//点击切换服务器
            ALUGUICommon.combineBtnClick(wnd.btnAccount, _onClickAccount);//点击查看账号信息
            ALUGUICommon.combineBtnClick(wnd.btnAudio, _onClickAudio);//点击设置音效
            ALUGUICommon.combineBtnClick(wnd.btnCopy, _onClickCopyBtn);//点击复制cid
            ALUGUICommon.combineBtnClick(wnd.btnEffect, _onClickEffect);//点击设置游戏效果
            ALUGUICommon.combineBtnClick(wnd.btnLocalPush, _onClickLocalPushSetting);//点击本地推送设置按钮
            ALUGUICommon.combineBtnClick(wnd.btnChangeServer, _onClickChangeServer);//点击切换服务器
            ALUGUICommon.combineBtnClick(wnd.btnAIHelp, _onClickAIHelp);//点击客服按钮
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);//点击关闭按钮
        }

        private void _refresh()
        {
            if (null != _m_playerIconWnd)
                _m_playerIconWnd.setSelfInfo();

            WCGClientInfo info = ClientVersionSetting.instance.ClientVersionInfo;
            string version = "";
            if (null != info)
                version = String.Format("V{0}.{1}.{2}.{3}", info.majorVersion, info.minorVersion, info.revisionVersion, info.buildVersion);
            if (!string.IsNullOrEmpty(version))
                ALUGUICommon.setLabelTxt(wnd.versionTxt, version);
        }

        private void _tickDeal()
        {
            if (null == wnd)
                return;

            string serverTimeStr = TextTranslate.instance.getLanguage(wnd.serverTimeKey, TimeUtil.getServerTimeZone(), TimeUtil.DateTime2StringMDYHMS(TimeUtil.FromUTCMilliseconds(FpsAndPingMgr.instance.serverTimeTag)));
            ALUGUICommon.setLabelTxt(wnd.serverTimeTxt, serverTimeStr);
        }

        //点击切换语言
        private void _onClickSwitchLan(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(NPGGUIWndGameSettingSwitchLanguage.instance,
                NPGGUIWndGameSettingSwitchLanguage.instance.showWnd,
                EUIQueueStageType.MAIN,
                UINodeTagConst.C_SETTING_SWITCH_LANGUAGE,
                false,
                true);
        }

        //点击查看账号信息
        private void _onClickAccount(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(NPGGUIWndAccountBind.instance,
                NPGGUIWndAccountBind.instance.showWnd,
                EUIQueueStageType.MAIN,
                UINodeTagConst.C_ACCOUNT_BIND,
                false,
                true);
        }

        //点击音效设置按钮
        private void _onClickAudio(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(NPGGUIWndGameSettingAudio.instance,
                NPGGUIWndGameSettingAudio.instance.showWnd,
                EUIQueueStageType.MAIN,
                UINodeTagConst.C_SETTING_AUDIO,
                false,
                true);
        }

        //点击复制cid
        private void _onClickCopyBtn(GameObject _go)
        {
            GUIUtility.systemCopyBuffer = NPPlayer.instance.playerInfo.CID.ToString();
            NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_copySuc_str));
        }

        //点击设置游戏效果
        private void _onClickEffect(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(NPGGUIWndGameSettingEffect.instance,
                NPGGUIWndGameSettingEffect.instance.showWnd,
                EUIQueueStageType.MAIN,
                UINodeTagConst.C_SETTING_EFFECT,
                false,
                true);
        }

        //点击本地推送设置
        private void _onClickLocalPushSetting(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGameSettingLocalPush.instance,
                GGUIWndGameSettingLocalPush.instance.showWnd,
                EUIQueueStageType.MAIN,
                UINodeTagConst.C_SETTING_LOCAL_PUSH,
                false,
                true);
        }

        private void _onClickChangeServer(GameObject _go)
        {
            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.setting_change_server_tip),//是否返回登入界面切换服务器？
                TextTranslate.instance.getLanguage(TransKeyConst.cancel),
                null,
                TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                () =>
                {
                    //设置自动打开选服界面
                    NPGGUIWndStartGame.instance.isAutoOpenSelectServer = true;
                    //回到登录界面
                    Game.instance.reloginByDefault();
                });
        }

        //点击客服按钮
        private void _onClickAIHelp(GameObject _go)
        {
            GCommon.showAIHelp();
        }

        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
    }
}
