
using System.Collections.Generic;
using Common.BagItemUseEnum;
using Common.BagItemUseObj;

namespace GOE
{
    /// <summary>
    ///伙伴家人使用道具完成弹窗
    /// </summary>
    public class NoticeDealer_BagItemHeroConsortUseResult : NPUINoticeMgr._ANPUINoticeDealer
    {
        //窗口是否已经加载，如果已经加载的情况下。在shownotice的时候不会再加载，避免窗口驻留
        private bool _m_bWndLoaded;
        private EBagItemUse_HeroDrawShowType _m_eHeroType;
        private List<BagItemUse_HeroShowInfo> _m_lHeroInfoList;
        private EBagItemUse_ConsortDrawShowType _m_eConsortType;
        private List<BagItemUse_ConsortShowInfo> _m_lConsortInfoList;

        public NoticeDealer_BagItemHeroConsortUseResult(EBagItemUse_HeroDrawShowType _type, List<BagItemUse_HeroShowInfo> _infoList)
        {
            _m_eHeroType = _type;
            _m_lHeroInfoList = _infoList;
        }

        public NoticeDealer_BagItemHeroConsortUseResult(EBagItemUse_ConsortDrawShowType _type, List<BagItemUse_ConsortShowInfo> _infoList)
        {
            _m_eConsortType = _type;
            _m_lConsortInfoList = _infoList;
        }
        
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return true; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return false; } }
        /// <summary>
        /// 字符串标记，可以用来根据tag开区别node的tag
        /// </summary>
        public override string nodeTag { get { return UINodeTagConst.C_BAG_ITEM_HERO_CONSORT_USE_RESULT; } }


        public override void dealShowNotice()
        {
            //未加载的时候加载
            if (!_m_bWndLoaded)
            {
                _m_bWndLoaded = true;

                GGUIWndBagItemHeroConsortUseResult.instance.load(() =>
                {
                    GGUIWndBagItemHeroConsortUseResult.instance.showWnd();
                    if(_m_lHeroInfoList != null)
                        GGUIWndBagItemHeroConsortUseResult.instance.setInfo(_m_eHeroType, _m_lHeroInfoList);
                    else if(_m_lConsortInfoList != null)
                        GGUIWndBagItemHeroConsortUseResult.instance.setInfo(_m_eConsortType, _m_lConsortInfoList);
                });
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != GGUIWndBagItemHeroConsortUseResult.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndBagItemHeroConsortUseResult.instance.rectTransform);
            }
        }

        public override void dealHideNotice()
        {
            //已加载的时候才卸载，同时重置状态
            if (_m_bWndLoaded)
            {
                GGUIWndBagItemHeroConsortUseResult.instance.discard();
                _m_bWndLoaded = false;
            }
        }

        protected override void _onDealerDone()
        {
        }
    }
}
