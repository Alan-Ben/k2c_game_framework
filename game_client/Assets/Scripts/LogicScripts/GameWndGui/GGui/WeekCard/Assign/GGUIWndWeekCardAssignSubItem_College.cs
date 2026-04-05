using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 委派列表item--大学
    /// </summary>
    public class GGUIWndWeekCardAssignSubItem_College : _AGGUIWndWeekCardAssignSubItemBase
    {
        private GGUIMonoWeekCardAssignSubItem_College _m_realWndMono;
        private WeekCardAssignItemShowInfo_College _m_realInfo;
        private NPGGUIWndCommonToggleEx _m_togSelected;
        private readonly string _m_assetPath;
        private readonly string _m_objName;
        private bool _m_isSelected;
        public event Action<_IWeekCardAssignItemShowInfo> onSelectedItem;
        public event Action<_IWeekCardAssignItemShowInfo> onDisSelectedItem;

        public GGUIWndWeekCardAssignSubItem_College(string _assetPath, string _objName, Transform _parent) : base(_parent)
        {
            _m_assetPath = _assetPath;
            _m_objName = _objName;
        }

        protected override string _monoAssetPath { get => _m_assetPath; }
        protected override string _monoObjName { get => _m_objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        protected override void _onShowWndEx()
        {
            
        }

        protected override void _onHideWndEx()
        {
            
        }

        protected override void _onDiscardEx()
        {
            
            _m_togSelected?.discard();
            _m_togSelected = null;
        }

        protected void _onSelectedTogOn(bool _isSelected)
        {
            _m_realInfo?.settingInfo?.setIsClose(!_isSelected);
        }

        protected override void _onWndInitDoneEx()
        {
            if(null == wnd)
                return;
            _m_realWndMono = wnd as GGUIMonoWeekCardAssignSubItem_College;
            if(null == _m_realWndMono)
                return;
            

            if (null != _m_realWndMono.togSelected)
            {
                _m_togSelected = new NPGGUIWndCommonToggleEx(_m_realWndMono.togSelected);
                _m_togSelected.clickDelegate += _onClickSelected;
            }
            ALUGUICommon.combineBtnClick(_m_realWndMono.btnSelectedHero, _onClickSelectedHero);
        }

        private void _onClickSelected(NPGGUIWndCommonToggleEx obj)
        {
            setSelectedTogOn(!_m_isSelected);
            if (_m_isSelected)
            {
                onSelectedItem?.Invoke(_m_realInfo);
            }
            else
            {
                onDisSelectedItem?.Invoke(_m_realInfo);
            }
        }
        
        private void setSelectedTogOn(bool _isSelected)
        {
            _m_isSelected = _isSelected;
            _m_togSelected?.setSelected(_isSelected);
            _onSelectedTogOn(_m_isSelected);
        }


        protected override void _onRefreshWnd(_IWeekCardAssignItemShowInfo _info)
        {
            _m_realInfo = _info as WeekCardAssignItemShowInfo_College;
            if(null == _m_realInfo)
                return;
            if (null == _m_realWndMono)
                return;

            setSelectedTogOn(!_m_realInfo.getIsClose());
            _refreshCount();
        }

        private void _refreshCount()
        {
            if(null == _m_realInfo)
                return;
            if (null == _m_realWndMono)
                return;
            // ALUGUICommon.setLabelTxt(_m_realWndMono.txtAssignCount,
            //     TextTranslate.instance.getLanguage(TransKeyConst.week_card_college_hero_count,
            //         _m_realInfo.settingInfo.exInfo.getHeroList().Count,
            //         NPPlayer.instance.collegeComp.getPosLimitNum()));
        }

        /// <summary>
        /// 点击选择骑士，打开骑士选择弹窗
        /// </summary>
        /// <param name="obj"></param>
        private void _onClickSelectedHero(GameObject obj)
        {
            //打开选择骑士弹窗，回调设置数据，刷新显示
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndWeekCardSelectedHero.instance, () =>
            {
                GGUIWndWeekCardSelectedHero.instance.onSelectedConfirm += list =>
                {
                    _m_realInfo?.settingInfo?.setExInfo(list);

                    _refreshCount();
                };
                List<HeroInfo> allHero = new List<HeroInfo>();
                NPPlayer.instance.heroComponent.getAllList(allHero);
                allHero.Sort(_sortHero);
                // GGUIWndWeekCardSelectedHero.instance.setInfo(allHero, _m_realInfo.settingInfo.exInfo.getHeroList(), NPPlayer.instance.collegeComp.getPosLimitNum());
            });
        }

        private int _sortHero(HeroInfo x, HeroInfo y)
        {
            // if (x.getTotalAttr() > y.getTotalAttr())
            //     return -1;
            // if (x.getTotalAttr() < y.getTotalAttr())
            //     return 1;
            if (x.id > y.id)
                return 1;
            if (x.id < y.id)
                return -1;
            return 0;
        }
    }
}
