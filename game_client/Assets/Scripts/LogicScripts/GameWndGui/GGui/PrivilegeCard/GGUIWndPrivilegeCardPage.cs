using System;
using ALPackage;
using Common.PrivilegeCardEnum;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 权益卡主页面
    /// </summary>
    public class GGUIWndPrivilegeCardPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoPrivilegeCardPage>
    {
        //资源id
        private long _m_lUIResId;
        //子权益卡列表
        [NotNull] private List<GGUIWndSubPrivilegeCard> _m_lSubPrivilegeCardList = new List<GGUIWndSubPrivilegeCard>();

        public GGUIWndPrivilegeCardPage(long _uiResId, Transform _parent) : base(_parent)
        {
            _m_lUIResId = _uiResId;
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lUIResId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lUIResId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            foreach (GGUIWndSubPrivilegeCard subPrivilegeCard in _m_lSubPrivilegeCardList)
            {
                subPrivilegeCard?.showWnd();
            }
        }
        
        protected override void _onHideWnd()
        {
            foreach (GGUIWndSubPrivilegeCard subPrivilegeCard in _m_lSubPrivilegeCardList)
            {
                subPrivilegeCard?.hideWnd();
            }
        }
        
        protected override void _onReset()
        {
            foreach (GGUIWndSubPrivilegeCard subPrivilegeCard in _m_lSubPrivilegeCardList)
            {
                subPrivilegeCard?.resetWnd();
            }
        }
        
        protected override void _onDiscard()
        {
            foreach (GGUIWndSubPrivilegeCard subPrivilegeCard in _m_lSubPrivilegeCardList)
            {
                subPrivilegeCard?.discard();
            }
            _m_lSubPrivilegeCardList.Clear();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.subPrivilegeCardList != null)
            {
                _m_lSubPrivilegeCardList?.Clear();
                foreach (GGUIPrivilegeCardType cardType in wnd.subPrivilegeCardList)
                {
                    if(cardType == null)
                        continue;

                    GGUIWndSubPrivilegeCard wndSubCard = new GGUIWndSubPrivilegeCard(cardType.type, cardType.monoSubPrivilegeCard);
                    _m_lSubPrivilegeCardList.Add(wndSubCard);
                }
            }
        }

        /// <summary>
        /// 设置展示引导购买手势
        /// </summary>
        /// <param name="_type"></param>
        public void showGuideHand(ECashGiftPackSpecialDealType _type)
        {
            EPrivilegeCardType targetType = EPrivilegeCardType.NONE;
            switch (_type)
            {
                case ECashGiftPackSpecialDealType.SHOW_MONTH_CARD_GUIDE_HAND:
                    targetType = EPrivilegeCardType.MONTH;
                    break;
                case ECashGiftPackSpecialDealType.SHOW_YEAR_CARD_GUIDE_HAND:
                    targetType = EPrivilegeCardType.YEAR;
                    break;
            }

            if (targetType != EPrivilegeCardType.NONE)
            {
                for (int i = 0; i < _m_lSubPrivilegeCardList.Count; i++)
                {
                    if (_m_lSubPrivilegeCardList[i]?.cardType == targetType)
                    {
                        _m_lSubPrivilegeCardList[i].showHandGuide();
                        break;
                    }
                }
            }
        }
    }
}
