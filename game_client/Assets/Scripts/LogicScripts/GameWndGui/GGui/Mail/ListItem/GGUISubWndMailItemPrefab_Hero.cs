using ALPackage;
using UnityEngine;
using System;
using Common.MailObj;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 邮件列表item-- hero
    /// </summary>
    public class GGUISubWndMailItemPrefab_Hero : GGUISubWndMailItemPrefab_Base<GGUISubMonoMailItemPrefab_Hero>
    {
        private NPGGuiWndTexture _m_heroIcon;
        private Mail_ExtTitleHeroInfo _m_titleHeroInfo;


        public GGUISubWndMailItemPrefab_Hero(long _uiPathId, Transform _parent)
            : base(_uiPathId, _parent)
        {
            
        }

        protected override void _refreshWndEx()
        {
            if (null == wnd)
                return;
            if (null == _m_heroIcon && null != wnd.heroIcon)
            {
                _m_heroIcon = new NPGGuiWndTexture(wnd.heroIcon);
            }

            if (null != _m_heroIcon && null != _m_titleHeroInfo)
            {
                _m_heroIcon.showWnd();
                _m_heroIcon.setTexture(GCommon.getItemTexIcon(ENPItemType.HERO, _m_titleHeroInfo.getHeroId()));
            }
        }

        protected override void _onDiscardEx()
        {
            _m_heroIcon?.discard();
            _m_heroIcon = null;
        }

        protected override void _onSetDataInfo(GMailDataInfo _mailDataInfo)
        {
            if (null == _mailDataInfo)
                return;
            if (null != _mailDataInfo.getExTitleData())
            {
                _m_titleHeroInfo = new Mail_ExtTitleHeroInfo();
                _m_titleHeroInfo.readPackage(_mailDataInfo.getExTitleData());
            }
        }

        protected override string _getSender()
        {
            if (null != _m_titleHeroInfo)
            {
                return GCommon.getItemName(ENPItemType.HERO, _m_titleHeroInfo.getHeroId());
            }
            return base._getSender();
        }
    }
}
