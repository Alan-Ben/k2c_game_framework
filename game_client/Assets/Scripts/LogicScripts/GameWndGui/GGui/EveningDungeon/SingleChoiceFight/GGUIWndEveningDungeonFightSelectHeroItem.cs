using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 晚间活动选择大臣出战item
    /// </summary>
    public class GGUIWndEveningDungeonFightSelectHeroItem : _ANPGGUIBasicGridItemWnd<GGUIMonoEveningDungeonFightSelectHeroItem>
    {
        //伙伴通用卡牌展示子窗口
        private GGUIWndHeroCommonCardItem _m_wndCommonCard;

        // 伙伴头像子窗口
        private GGUIWndHeroIconItem _m_wHeroIcon;
        
        //点击回调
        private Action<GGUIWndEveningDungeonFightSelectHeroItem> _m_dClickDelegate;

        private _IEveningDungeonHeroFightInfo _m_heroFigthInfo;
        
        //是否选中
        private bool _m_bIsSelect;

        public GGUIWndEveningDungeonFightSelectHeroItem(GGUIMonoEveningDungeonFightSelectHeroItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        public Action<GGUIWndEveningDungeonFightSelectHeroItem> clickDelegate { get { return _m_dClickDelegate; } set { _m_dClickDelegate = value; } }

        /// <summary>
        /// 伙伴数据信息
        /// </summary>
        public _IEveningDungeonHeroFightInfo eveningDungeonHeroFightInfo { get { return _m_heroFigthInfo; } }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool isSelect { get { return _m_bIsSelect; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            if (null != _m_wndCommonCard)
                _m_wndCommonCard.hideWnd();
            
            if (null != _m_wHeroIcon)
                _m_wHeroIcon.hideWnd();
        }

        protected override void _onReset()
        {
            if (null != _m_wndCommonCard)
                _m_wndCommonCard.resetWnd();
            
            if (null != _m_wHeroIcon)
                _m_wHeroIcon.resetWnd();
        }

        protected override void _resetGridItem()
        {
            if (null != _m_wndCommonCard)
                _m_wndCommonCard.resetWnd();
            
            if (null != _m_wHeroIcon)
                _m_wHeroIcon.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (null != _m_wndCommonCard)
                _m_wndCommonCard.discard();
            _m_wndCommonCard = null;

            if (null != _m_wHeroIcon)
                _m_wHeroIcon.discard();
            _m_wHeroIcon = null;
            
            _m_dClickDelegate = null;
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if(wnd.monoCardItem != null)
            {
                _m_wndCommonCard = new GGUIWndHeroCommonCardItem(wnd.monoCardItem);
                _m_wndCommonCard.ClickAction += _onClickItem;
            }
            
            if(wnd.monoHeroInfo != null)
                _m_wHeroIcon = new GGUIWndHeroIconItem(wnd.monoHeroInfo);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroFightInfo">大臣出战信息</param>
        public void setInfo(_IEveningDungeonHeroFightInfo _heroFightInfo)
        {
            if (wnd == null || null == _heroFightInfo)
                return;

            _m_heroFigthInfo = _heroFightInfo;
            _m_bIsSelect = false;

            //刷新卡牌显示
            if (null != _m_wndCommonCard)
            {
                _m_wndCommonCard.showWnd();
                _m_wndCommonCard.setInfo(_m_heroFigthInfo.heroCardShow);
            }

            if (_m_wHeroIcon != null)
            {
                _m_wHeroIcon.showWnd();
                _m_wHeroIcon.setData(_m_heroFigthInfo.heroCardShow);
            }

            string powerKey = string.IsNullOrEmpty(wnd.txtFightATKKey) ? TransKeyConst.common_value : wnd.txtFightATKKey;
            //设置实力
            ALUGUICommon.setLabelTxt(wnd.txtFightATK,
                TextTranslate.instance.getLanguage(powerKey, _m_heroFigthInfo.fightATK.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));

            long leftFightCount = _m_heroFigthInfo.fightMaxCount - _m_heroFigthInfo.nowFightCount;
            string fightCountStr = TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, leftFightCount, _m_heroFigthInfo.fightMaxCount);
            ALUGUICommon.setLabelTxt(wnd.txtFightCount, leftFightCount > 0 ? GCommon.addColorForRichText(fightCountStr, wnd.hasFightCountColor) : GCommon.addColorForRichText(fightCountStr, wnd.noFightCountColor));
         
            if (wnd.fightStateShowList != null)
            {
                EEveningDungeonHeroFightState fightState = getFightState();
                NPCommonEnumStatInfo<EEveningDungeonHeroFightState>.setStat(wnd.fightStateShowList, fightState);
            }
        }

        public EEveningDungeonHeroFightState getFightState()
        {
            if (_m_heroFigthInfo == null)
                return EEveningDungeonHeroFightState.NONE;

            if (_m_heroFigthInfo.fightMaxCount <= 0 || _m_heroFigthInfo.nowFightCount >= _m_heroFigthInfo.fightMaxCount)
                return EEveningDungeonHeroFightState.ALREADY_FIGHT_NO_FIGHT_COUNT;

            if (_m_heroFigthInfo.nowFightCount <= 0)
                return EEveningDungeonHeroFightState.NEVER_FIGHT;
            
            return EEveningDungeonHeroFightState.ALREADY_FIGHT_HAS_FIGHT_COUNT;
        }
        
        //设置选中
        public void setSelect(bool _isSelect)
        {
            if (wnd == null)
                return;

            _m_bIsSelect = _isSelect;
            ALUGUICommon.setGameObjEnable(wnd.goSelectShowList, _m_bIsSelect);
            ALUGUICommon.setGameObjEnable(wnd.goSelectHideList, !_m_bIsSelect);
        }

        //点击item
        private void _onClickItem(GGUIWndHeroCommonCardItem _commonCard)
        {
            EEveningDungeonHeroFightState fightState = getFightState();

            if (fightState == EEveningDungeonHeroFightState.ALREADY_FIGHT_NO_FIGHT_COUNT || fightState == EEveningDungeonHeroFightState.NONE)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.eveningDungeon_heroFightCountMaxTip_none);
                return;
            }

            setSelect(!_m_bIsSelect);
            if (null != _m_dClickDelegate)
                _m_dClickDelegate(this);
        }
    }
}