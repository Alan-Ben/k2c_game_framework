using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndInnLevelInfo : _ATALBasicUIWnd<GGUIMonoInnLevelInfo>
    {
        [NotNull] public static GGUIWndInnLevelInfo instance { get { return _g_instance ??= new GGUIWndInnLevelInfo(); } }
        private static GGUIWndInnLevelInfo _g_instance;
        
        private GGUISubWndInnLevelIconContainer _m_starIconContainerWnd;


        public GGUIWndInnLevelInfo()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoInnLevelInfo.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnLevelInfo.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_starIconContainerWnd?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_starIconContainerWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_starIconContainerWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onBtnDetailClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onBtnConfirmClick);
            
            _m_starIconContainerWnd?.discard();
            _m_starIconContainerWnd = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onBtnDetailClick);
            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onBtnConfirmClick);
            
            if (wnd.monoStarIconContainer != null)
                _m_starIconContainerWnd = new GGUISubWndInnLevelIconContainer(wnd.monoStarIconContainer);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            InnLevelRefObj levelRef = NPPlayer.instance.innComp.levelRef;
            InnLevelRefObj nextLevelRef = NPPlayer.instance.innComp.nextLevelRef;
            long popularity = NPPlayer.instance.innComp.popularity;

            // 设置等级名称
            ALUGUICommon.setLabelTxt(wnd.txtName, levelRef?.nameTranslated);
            // 设置当前等级
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, levelRef?.level ?? 0));
            // 设置升级进度
            ALUGUICommon.setLabelTxt(wnd.txtLevelProgress,
                TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num,
                    popularity.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT),
                    nextLevelRef?.need_popularity.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT) ?? popularity.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            if (wnd.sldLevelProgress != null)
            {
                wnd.sldLevelProgress.minValue = levelRef?.need_popularity ?? 0;
                wnd.sldLevelProgress.maxValue = nextLevelRef?.need_popularity ?? levelRef?.need_popularity ?? 0;
                wnd.sldLevelProgress.value = popularity;
            }
            
            // 设置当前人气值
            ALUGUICommon.setLabelTxt(wnd.txtCurPopularity, popularity.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            // 设置迎宾次数上限
            ALUGUICommon.setLabelTxt(wnd.txtCurReceiveGuestLimit, levelRef?.receive_guest_limit ?? 0);
            // 设置当前解锁客人数量
            ALUGUICommon.setLabelTxt(wnd.txtCurGuestUnlockNum, NPPlayer.instance.innComp.getUnlockedGuestNum());
            // 设置满级状态显示
            wnd.setLevelMax(nextLevelRef == null);
            
            // 刷新星级图标容器
            _m_starIconContainerWnd?.refreshWnd(levelRef);
        }


        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_INN_LEVEL_INFO);
        }
        private void _onBtnDetailClick(GameObject _)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndInnLevelDetail.instance, GGUIWndInnLevelDetail.instance.showWnd, UINodeTagConst.C_INN_LEVEL_DETAIL);
        }
        private void _onBtnConfirmClick(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_INN_LEVEL_INFO);
        }
    }
}