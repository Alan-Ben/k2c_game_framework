using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 游戏设置界面-切换语言
    /// </summary>
    public class NPGGUIWndGameSettingSwitchLanguage : _ANPGGUIBasicResBarWnd<NPGGUIMonoGameSettingSwitchLanguage>
    {

        private static NPGGUIWndGameSettingSwitchLanguage _g_instance;
        public static NPGGUIWndGameSettingSwitchLanguage instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new NPGGUIWndGameSettingSwitchLanguage();
                return _g_instance;
            }
        }

        private NPGGUIWndGameSettingSwitchLanguageContainer _m_wndLanguageContainer;
        
        private NPGGUIWndGameSettingSwitchLanguageContainer _m_wndVoiceLanguageContainer;

        protected NPGGUIWndGameSettingSwitchLanguage() : base(EALUIWndLayer.ADDITION)
        {
        }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return NPGGUIMonoGameSettingSwitchLanguage.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoGameSettingSwitchLanguage.objName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            if(_m_wndLanguageContainer != null)
                _m_wndLanguageContainer.hideWnd();
            
            if(_m_wndVoiceLanguageContainer != null)
                _m_wndVoiceLanguageContainer.hideWnd();
        }

        protected override void _onReset()
        {
            if (_m_wndLanguageContainer != null)
                _m_wndLanguageContainer.resetWnd();
            
            if(_m_wndVoiceLanguageContainer != null)
                _m_wndVoiceLanguageContainer.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (_m_wndLanguageContainer != null)
                _m_wndLanguageContainer.discard();
            _m_wndLanguageContainer = null;
            
            if(_m_wndVoiceLanguageContainer != null)
                _m_wndVoiceLanguageContainer.discard();
            _m_wndVoiceLanguageContainer = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoLanguageContainer != null)
                _m_wndLanguageContainer = new NPGGUIWndGameSettingSwitchLanguageContainer(wnd.monoLanguageContainer);

            if (wnd.monoVoiceLanguageContainer != null)
                _m_wndVoiceLanguageContainer = new NPGGUIWndGameSettingSwitchLanguageContainer(wnd.monoVoiceLanguageContainer);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (_m_wndLanguageContainer != null)
            {
                _m_wndLanguageContainer.showWnd();
                _m_wndLanguageContainer.showItemList((PLoginCommonInfo.instance.obj == null || PLoginCommonInfo.instance.obj.supportLanguageList == null) ? null : new List<ENPLanguage>(PLoginCommonInfo.instance.obj.supportLanguageList)
                    , GameSetting.instance.getCurrentLanguage(), _onLanguageContainerItemClick);
            }
            
            if(_m_wndVoiceLanguageContainer != null)
            {
                _m_wndVoiceLanguageContainer.showWnd();
                _m_wndVoiceLanguageContainer.showItemList((PLoginCommonInfo.instance.obj == null || PLoginCommonInfo.instance.obj.supportVoiceLanguageList == null) ? null : new List<ENPLanguage>(PLoginCommonInfo.instance.obj.supportVoiceLanguageList)
                    , GameSetting.instance.getCurrentVoiceLanguage(), _onVoiceLanguageContainerItemClick);
            }
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.DoUIRollBackByEscByTag(UINodeTagConst.C_SETTING_SWITCH_LANGUAGE);
        }

        /// <summary>
        /// 语言ContainerItem被点击时
        /// </summary>
        /// <param name="_language"></param>
        private void _onLanguageContainerItemClick(ENPLanguage _language)
        {
            if (_language == GameSetting.instance.getCurrentLanguage())
                return;

            string languageName = TextTranslate.instance.getLanguage(string.Format("#1_{0}_name", _language.ToString()));

            //您是否要将游戏文本切换成{0}版本？
            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.setting_switchLanguageTo_str, languageName)
                , TextTranslate.instance.getLanguage(TransKeyConst.cancel)
                , null
                , TextTranslate.instance.getLanguage(TransKeyConst.ok)
                , () =>
                {
                    if(wnd != null && wnd.onChgTxtLanguageNeedChgVoiceLanguageTogether)
                        GameVoiceLanguageMgr.instance.setVoiceLanguage(_language);
                    
                    //选择语言后退出当前窗口
                    QueueMgr.instance.DoUIRollBackByEscByTag(UINodeTagConst.C_SETTING_SWITCH_LANGUAGE);
                    
                    // 设置游戏使用的语言
                    GameLanguageMgr.instance.setGameLanguage(_language);
                });
        }
        
        /// <summary>
        /// 音效语言ContainerItem被点击时
        /// </summary>
        /// <param name="_language"></param>
        private void _onVoiceLanguageContainerItemClick(ENPLanguage _language)
        {
            if (_language == GameSetting.instance.getCurrentVoiceLanguage())
                return;

            string languageName = TextTranslate.instance.getLanguage(string.Format("#1_{0}_name", _language.ToString()));

            //您是否要将游戏文本切换成{0}版本？
            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.setting_switchVoiceLanguageTo_str, languageName)
                , TextTranslate.instance.getLanguage(TransKeyConst.cancel)
                , null
                , TextTranslate.instance.getLanguage(TransKeyConst.ok)
                , () =>
                {
                    // 设置游戏使用的语言
                    GameVoiceLanguageMgr.instance.setVoiceLanguage(_language);
                    
                    _refreshWnd();//刷新窗口
                });
        }
    }
}
