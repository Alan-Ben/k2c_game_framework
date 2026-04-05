using System;
using ALPackage;

namespace GOE
{
    public class GGUIWndHeroCommonSimpleDispatchSelectHeroItem : _ATNPGGUIWndStateShow<ECommonSelectState, _ATNPGGUIStateShowParam<ECommonSelectState>, GGUIMonoHeroCommonSimpleDispatchSelectHeroItem>
    {
        private CommonSimpleDispatchEventAgent _m_dispatchEventAgent;
        private HeroSatisfyConditionCount _m_heroSatisfyConditionCount;

        /// <summary>
        /// 大臣信息子窗口窗口
        /// </summary>
        // private GGUIWndHeroCommonCardItem _m_wHeroCardItem;

        public GGUIWndHeroCommonSimpleDispatchSelectHeroItem(GGUIMonoHeroCommonSimpleDispatchSelectHeroItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        public _IHeroCardShow heroCardShowInfo { get { return _m_heroSatisfyConditionCount.heroShowInfo; } }
        public event Action<GGUIWndHeroCommonSimpleDispatchSelectHeroItem> ClickAction;

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            // _m_wHeroCardItem?.hideWnd();
        }

        protected override void _onReset()
        {
            // _m_wHeroCardItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            // if (_m_wHeroCardItem != null)
            // {
            //     _m_wHeroCardItem.ClickAction = default;
            //     _m_wHeroCardItem.discard();
            //     _m_wHeroCardItem = null;    
            // }

            ClickAction = default;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
        }
        
        protected override void _resetGridItem()
        {
        }

        //点击事件
        // private void _onClick(GGUIWndHeroCommonCardItem _item)
        // {
        //     ClickAction?.Invoke(this);
        // }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(CommonSimpleDispatchEventAgent _dispatchEventAgent, HeroSatisfyConditionCount _heroSatisfyConditionCount)
        {
            _m_dispatchEventAgent = _dispatchEventAgent;
            _m_heroSatisfyConditionCount = _heroSatisfyConditionCount;
            
            _refreshShow();
        }

        //刷新显示
        private void _refreshShow()
        {
            if(wnd == null)
                return;

            // if (_m_wHeroCardItem != null)
            // {
            //     _m_wHeroCardItem.showWnd();
            //     _m_wHeroCardItem.setInfo(_m_heroSatisfyConditionCount.heroShowInfo);
            // }

            ALUGUICommon.setGameObjEnable(wnd.allConditionSatisfyShowList, false);
            ALUGUICommon.setGameObjEnable(wnd.noConditionSatisfyShowList, false);
            ALUGUICommon.setGameObjEnable(wnd.partConditionSatisfyShowList, false);
            if (_m_heroSatisfyConditionCount.count >= (_m_dispatchEventAgent?.dispatchCondtionNum ?? 0))
            {
                ALUGUICommon.setGameObjEnable(wnd.allConditionSatisfyShowList, true);
            }
            else if(_m_heroSatisfyConditionCount.count <= 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.noConditionSatisfyShowList, true);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.partConditionSatisfyShowList, true);
            }
        }
    }
}