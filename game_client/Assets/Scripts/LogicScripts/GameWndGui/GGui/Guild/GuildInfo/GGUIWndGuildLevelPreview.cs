using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟等级预览界面
    /// </summary>
    public class GGUIWndGuildLevelPreview : _ANPGGUIBasicWnd<GGUIMonoGuildLevelPreview>
    {
        private static GGUIWndGuildLevelPreview _g_instance = new GGUIWndGuildLevelPreview();
        public static GGUIWndGuildLevelPreview instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildLevelPreview();
                return _g_instance;
            }
        }

        //预览效果列表
        private GGUIWndGuildLevelPreviewContainer _m_wPreviewContainer;
        //旗帜图标
        private NPGGuiWndTexture _m_wFlagIcon;
        //联盟等级列表
        [NotNull] private List<GuildLevelRefObj> _m_lGuildLevelRefList;
        //当前展示的下标
        private int _m_iCurIndex;

        public GGUIWndGuildLevelPreview() : base(EALUIWndLayer.ADDITION)
        {

        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildLevelPreview.assetPath; } }

        protected override string _monoObjName { get { return GGUIMonoGuildLevelPreview.objName; } }

        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            _m_iCurIndex = -1;
            for (int i = 0; i < _m_lGuildLevelRefList.Count; i++)
            {
                if (_m_lGuildLevelRefList[i] != null && _m_lGuildLevelRefList[i].level == guildInfo.level)
                {
                    _m_iCurIndex = i;
                    break;
                }
            }

            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wFlagIcon?.hideWnd();
            _m_wPreviewContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wFlagIcon?.discardTexture();
            _m_wPreviewContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_lGuildLevelRefList?.Clear();
            _m_lGuildLevelRefList = null;

            _m_wFlagIcon?.discard();
            _m_wFlagIcon = null;

            _m_wPreviewContainer?.discard();
            _m_wPreviewContainer = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnPrevious, _onClickPrevious);
            ALUGUICommon.uncombineBtnClick(wnd.btnNext, _onClickNext);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lGuildLevelRefList = new List<GuildLevelRefObj>();
            if(GRefdataCoreMgr.instance.guildLevelRefCore.refList != null)
                _m_lGuildLevelRefList.AddRange(GRefdataCoreMgr.instance.guildLevelRefCore.refList);

            if (wnd.imgFlag != null)
                _m_wFlagIcon = new NPGGuiWndTexture(wnd.imgFlag);

            if (wnd.monoLevelDescContainer != null)
                _m_wPreviewContainer = new GGUIWndGuildLevelPreviewContainer(wnd.monoLevelDescContainer);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnPrevious, _onClickPrevious);
            ALUGUICommon.combineBtnClick(wnd.btnNext, _onClickNext);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshFlag();
            _refreshLevelInfo();
        }

        //刷新旗帜
        private void _refreshFlag()
        {
            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            //设置图标
            GuildFlagRefObj guildFlagRef = GRefdataCoreMgr.instance.guildFlagRefCore.getRef(guildInfo.flagId);
            if (guildFlagRef != null && _m_wFlagIcon != null)
            {
                _m_wFlagIcon.showWnd();
                _m_wFlagIcon.setTexture(guildFlagRef.icon);
            }
        }

        //刷新等级信息
        private void _refreshLevelInfo()
        {
            if (wnd == null || _m_iCurIndex < 0)
                return;

            GuildLevelRefObj curLevelRefObj = _m_lGuildLevelRefList[_m_iCurIndex];
            if (curLevelRefObj == null)
                return;

            //设置等级描述
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.guild_guildLevel_level, curLevelRefObj.level));
            //设置效果列表
            if (_m_wPreviewContainer != null)
            {
                _m_wPreviewContainer.showWnd();
                _m_wPreviewContainer.showItemList(curLevelRefObj);
            }
            //设置按钮显隐
            ALUGUICommon.setGameObjEnable(wnd.btnPrevious, _m_iCurIndex > 0);
            ALUGUICommon.setGameObjEnable(wnd.btnNext, _m_iCurIndex < (_m_lGuildLevelRefList.Count - 1));
        }

        #region 点击事件

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_LEVEL_PREVIEW);
        }

        //点击上一个
        private void _onClickPrevious(GameObject _go)
        {
            _m_iCurIndex--;
            if (_m_iCurIndex < 0)
                _m_iCurIndex = 0;

            _refreshLevelInfo();
        }

        //点击下一个
        private void _onClickNext(GameObject _go)
        {
            _m_iCurIndex++;
            if (_m_iCurIndex >= _m_lGuildLevelRefList.Count)
                _m_iCurIndex = _m_lGuildLevelRefList.Count - 1;

            _refreshLevelInfo();
        }

        #endregion
    }
}