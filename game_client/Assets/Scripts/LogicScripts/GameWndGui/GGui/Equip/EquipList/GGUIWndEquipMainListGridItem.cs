using System;
using ALPackage;
using Common.PlayerEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 藏品列表item
    /// </summary>
    public class GGUIWndEquipMainListGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoEquipMainListGridItem>
    {
        //展示信息
        private _IEquipCardShow _m_showInfo;
        //页签类型
        private EEquipMainTabType _m_eTabType;
        //点击回调
        private Action<GGUIWndEquipMainListGridItem> _m_dClickDelegate;
        //藏品item
        private GGUIWndEquipCommonItem _m_wEquipItem;

        /// <summary>
        /// 展示信息
        /// </summary>
        public _IEquipCardShow showInfo { get { return _m_showInfo; } }
        /// <summary>
        /// 页签类型
        /// </summary>
        public EEquipMainTabType tabType { get { return _m_eTabType; } }
        /// <summary>
        /// 点击回调
        /// </summary>
        public Action<GGUIWndEquipMainListGridItem> clickDelegate { get { return _m_dClickDelegate; } set { _m_dClickDelegate = value; } }

        public GGUIWndEquipMainListGridItem(GGUIMonoEquipMainListGridItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wEquipItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wEquipItem?.resetWnd();
        }

        protected override void _resetGridItem()
        {
            _m_wEquipItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_dClickDelegate = null;

            _m_wEquipItem?.discard();
            _m_wEquipItem = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickItem);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (wnd.monoEquipItem != null)
                _m_wEquipItem = new GGUIWndEquipCommonItem(wnd.monoEquipItem);

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickItem);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(EEquipMainTabType _tabType, _IEquipCardShow _info)
        {
            _m_eTabType = _tabType;
            _m_showInfo = _info;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_showInfo == null || _m_showInfo.equipRef == null)
                return;

            EquipInfo equipInfo = _m_showInfo.equipInfo;

            //设置藏品item
            if (_m_wEquipItem != null)
            {
                _m_wEquipItem.showWnd();
                _m_wEquipItem.setInfo(_m_showInfo);
            }

            //根据页签显示内容
            if (_m_eTabType == EEquipMainTabType.OWN)
            {
                ALUGUICommon.setGameObjEnable(wnd.goIllustratedHandbookShowList, false);
                ALUGUICommon.setGameObjEnable(wnd.goIllustratedHandbookHideList, true);
                if (equipInfo == null)
                    return;

                bool haveHero = equipInfo.wearHeroId > 0;
                bool isLock = equipInfo.isLock;

                //设置显隐
                ALUGUICommon.setGameObjEnable(wnd.goSetLockShow, isLock);
                //设置伙伴
                if (haveHero)
                {
                    //加成值
                    ALUGUICommon.setLabelTxt(wnd.txtAddValue, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num,
                        TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, equipInfo.skillAddValue /100f)));
                }
            }
            else
            {
                //设置显隐
                ALUGUICommon.setGameObjEnable(wnd.goIllustratedHandbookHideList, false);
                ALUGUICommon.setGameObjEnable(wnd.goIllustratedHandbookShowList, true);

                //设置已解锁显示
                long ownCount = NPPlayer.instance.eventRecordComp.getValue(EPlayerEventRecordType.GAIN_EQUIP, _m_showInfo.equipRef.id);
                ALUGUICommon.setGameObjEnable(wnd.goUnlockHideList, ownCount <= 0);
                ALUGUICommon.setGameObjEnable(wnd.goUnlockShowList, ownCount > 0);
            }
        }

        //点击item
        private void _onClickItem(GameObject _go)
        {
            _m_dClickDelegate?.Invoke(this);
        }
    }
}
