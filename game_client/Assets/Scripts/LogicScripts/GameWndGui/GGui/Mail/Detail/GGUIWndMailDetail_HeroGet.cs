using ALPackage;
using Common.MailEnum;
using Common.MailObj;
using NPCommon;
using NPEnum;

namespace GOE
{
    //骑士获得邮件详情
    public class GGUIWndMailDetail_HeroGet : GGUIWndMailDetail_Base<GGUIMonoMailDetail_Hero>
    {
        //骑士图片
        private NPGGuiWndTexture _m_texHeroIcon;
        private GGUIWndMailRewardItemContainer _m_wndRewardItemContainer; //奖励列表
        private Mail_DetailExt_HeroGain _m_heroGain;

        public GGUIWndMailDetail_HeroGet(GMailDataInfo _info)
            : base(_info)
        {
        }

        protected override void _onResetEx()
        {
            base._onResetEx();
            _m_texHeroIcon?.discardTexture();
        }

        protected override void _onWndInitDoneEx()
        {
            base._onWndInitDoneEx();

            if (null == wnd)
                return;
            if (null != wnd.texHeroIcon)
            {
                _m_texHeroIcon = new NPGGuiWndTexture(wnd.texHeroIcon);
            }
            if (null != wnd.monoItemContainer)
            {
                _m_wndRewardItemContainer = new GGUIWndMailRewardItemContainer(wnd.monoItemContainer);
            }
        }

        protected override void _onDiscardEx()
        {
            base._onDiscardEx();
            
            _m_texHeroIcon?.discard();
            _m_texHeroIcon = null;
            _m_wndRewardItemContainer?.discard();
            _m_wndRewardItemContainer = null;
        }

        protected override void _onRetMailDataInfo(GMailDataInfo _mailDataInfo)
        {
            if (null != _mailDataInfo && null != _mailDataInfo.mailDetailInfo)
            {
                _m_heroGain = new Mail_DetailExt_HeroGain();
                _m_heroGain.readPackage(_mailDataInfo.mailDetailInfo.getExData());
            }
            base._onRetMailDataInfo(_mailDataInfo);
        }

        protected override void _refreshWndEx()
        {
            if (null == wnd)
                return;
            if (null == _m_miMailDataInfo)
                return;
            if (_m_miMailDataInfo.mailDetailInfo == null)
                return;
            if (_m_miMailDataInfo.mailDetailInfo.getExType() == (int) EMailExtType.HERO_GAIN)
            {
                if (null == _m_heroGain || _m_heroGain.getHeroId() == 0)
                    return;
                _m_texHeroIcon?.showWnd();
                _m_texHeroIcon?.setTexture(GCommon.getItemTexIcon(ENPItemType.HERO, _m_heroGain.getHeroId()));
            }
            
            bool hasItem = _m_miMailDataInfo.getHasItem();
            bool hasTaken = _m_miMailDataInfo.getHasTaken();
            if (hasItem)
            {
                //有奖励，展示奖励列表
                if (null != _m_wndRewardItemContainer)
                {
                    _m_wndRewardItemContainer.showItemList(hasTaken,_m_miMailDataInfo.mailDetailInfo.getItemList());
                    _m_wndRewardItemContainer.showWnd();
                }
            }
            else
            {   
                //没有奖励的时候隐藏奖励列表
                if (null != _m_wndRewardItemContainer)
                {
                    _m_wndRewardItemContainer.hideWnd();
                }
            }
        }

        protected override string _getMailContent()
        {
            if (null != _m_heroGain)
            {
                // HeroRefObj heroRefObj = GRefdataCoreMgr.instance.heroRefCore.getRef(_m_heroGain.getHeroId());
                // if (null != heroRefObj)
                //     return TextTranslate.instance.getLanguage(heroRefObj.gain_mail_text);
            }
            return base._getMailContent();
        }

        protected override string _getSender()
        {
            if (null != _m_heroGain)
            {
                return GCommon.getItemName(ENPItemType.HERO, _m_heroGain.getHeroId());
            }
            return base._getSender();
        }
    }
}