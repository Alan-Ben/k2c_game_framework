using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 玩家皮肤升级成功界面
    /// </summary>
    public class GGUIWndPlayerSkinUpgradeSuc : _ANPGGUIBasicWnd<GGUIMonoPlayerSkinUpgradeSuc>
    {
        private static GGUIWndPlayerSkinUpgradeSuc _g_instance;
        public static GGUIWndPlayerSkinUpgradeSuc instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndPlayerSkinUpgradeSuc();
                return _g_instance;
            }
        }

        //形象
        private NPGGUIWndCommonShowCase _m_wShowCase;
        //皮肤id
        private long _m_lSkinId;
        //皮肤等级
        private long _m_lLevel;

        private GGUIWndPlayerSkinUpgradeSuc() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoPlayerSkinUpgradeSuc.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoPlayerSkinUpgradeSuc.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _refresh();
        }

        protected override void _onHideWnd()
        {
            _m_wShowCase?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wShowCase?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wShowCase?.discard();
            _m_wShowCase = null;

            if (null == wnd)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoShowCase != null)
                _m_wShowCase = new NPGGUIWndCommonShowCase(wnd.monoShowCase);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_item"></param>
        /// <param name="_doneAction"></param>
        public void setInfo(long _skinId, long _curLevel)
        {
            _m_lSkinId = _skinId;
            _m_lLevel = _curLevel;
            _refresh();
        }

        //刷新窗口
        private void _refresh()
        {
            if (null == wnd)
                return;

            PlayerSkinRefObj skinRef = GRefdataCoreMgr.instance.playerSkinRefCore.getRef(_m_lSkinId);
            PlayerSkinLevelRefObj lastSkinLevelRef = GRefdataCoreMgr.instance.getPlayerSkinLevelRef(_m_lSkinId, _m_lLevel - 1);
            PlayerSkinLevelRefObj curSkinLevelRef = GRefdataCoreMgr.instance.getPlayerSkinLevelRef(_m_lSkinId, _m_lLevel);
            if (skinRef == null)
                return;

            //是否拥有属性
            bool isLastHaveIntimacy = lastSkinLevelRef != null && lastSkinLevelRef.player_property != null && lastSkinLevelRef.player_property.getPropertyValue(ENPPlayerPropertyType.INTIMACY) > 0;
            bool isCurHaveIntimacy = curSkinLevelRef != null && curSkinLevelRef.player_property != null && curSkinLevelRef.player_property.getPropertyValue(ENPPlayerPropertyType.INTIMACY) > 0;
            bool isLastHaveCharm = lastSkinLevelRef != null && lastSkinLevelRef.player_property != null && lastSkinLevelRef.player_property.getPropertyValue(ENPPlayerPropertyType.CHARM) > 0;
            bool isCurHaveCharm = curSkinLevelRef != null && curSkinLevelRef.player_property != null && curSkinLevelRef.player_property.getPropertyValue(ENPPlayerPropertyType.CHARM) > 0;

            if (lastSkinLevelRef != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtLastLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, lastSkinLevelRef.skin_level));
                ALUGUICommon.setLabelTxt(wnd.txtLastIntimacy, isLastHaveIntimacy ? lastSkinLevelRef.player_property.getPropertyValue(ENPPlayerPropertyType.INTIMACY) : 0);
                ALUGUICommon.setLabelTxt(wnd.txtLastCharm, isLastHaveCharm ? lastSkinLevelRef.player_property.getPropertyValue(ENPPlayerPropertyType.CHARM) : 0);
            }

            if (curSkinLevelRef != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtCurLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, curSkinLevelRef.skin_level));
                ALUGUICommon.setLabelTxt(wnd.txtCurIntimacy, isCurHaveIntimacy ? curSkinLevelRef.player_property.getPropertyValue(ENPPlayerPropertyType.INTIMACY) : 0);
                ALUGUICommon.setLabelTxt(wnd.txtCurCharm, isCurHaveCharm ? curSkinLevelRef.player_property.getPropertyValue(ENPPlayerPropertyType.CHARM) : 0);
            }

            if (_m_wShowCase != null)
            {
                _AShowCaseUnitInfoObj[] showCaseUnitInfoObjList = new _AShowCaseUnitInfoObj[1];
                showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(skinRef.td_show), 0);
                _m_wShowCase.showWnd(showCaseUnitInfoObjList);
            }

            ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.getItemName(ENPItemType.PLAYER_SKIN, _m_lSkinId));

            //设置显隐
            ALUGUICommon.setGameObjEnable(wnd.goHaveIntimacyHideList, !isLastHaveIntimacy && !isCurHaveIntimacy);
            ALUGUICommon.setGameObjEnable(wnd.goHaveIntimacyShowList, isLastHaveIntimacy || isCurHaveIntimacy);
            ALUGUICommon.setGameObjEnable(wnd.goHaveCharmHideList, !isLastHaveCharm && !isCurHaveCharm);
            ALUGUICommon.setGameObjEnable(wnd.goHaveCharmShowList, isLastHaveCharm || isCurHaveCharm);
        }

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_PlayerInfo.C_MAIN_PLAYERINFO_SKIN_UPGRADE_NODE);
        }
    }
}
