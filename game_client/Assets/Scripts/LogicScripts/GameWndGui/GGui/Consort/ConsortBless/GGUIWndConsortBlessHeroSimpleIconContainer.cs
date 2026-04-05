using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUIWndConsortBlessHeroSimpleIconContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoConsortBlessHeroSimpleIcon, GGUIMonoConsortBlessHeroSimpleIconContainer, GGUIWndConsortBlessHeroSimpleIcon>
    {
        private long _m_lConsortld;
        
        [NotNull] private List<GGUIWndConsortBlessHeroSimpleIcon> _m_lItemWndList = new List<GGUIWndConsortBlessHeroSimpleIcon>();//子窗口列表
        
        public GGUIWndConsortBlessHeroSimpleIconContainer(GGUIMonoConsortBlessHeroSimpleIconContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
            _m_lItemWndList.Clear();
        }
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_CONSORT_BLESS_SKILL_INFO_CHG, _refreshWnd);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CONSORT_BLESS_SKILL_INFO_CHG, _refreshWnd);

            dealAllItemWnd((_itemWnd) => _itemWnd?.hideWnd());
        }

        protected override void _onReset()
        {
            dealAllItemWnd((_itemWnd) => _itemWnd?.resetWnd());
        }

        protected override GGUIWndConsortBlessHeroSimpleIcon _createItemWnd(GGUIMonoConsortBlessHeroSimpleIcon _itemMono)
        {
            GGUIWndConsortBlessHeroSimpleIcon itemWnd = new GGUIWndConsortBlessHeroSimpleIcon(_itemMono);
            return itemWnd;
        }

        public void setData(long _consortId)
        {
            _m_lConsortld = _consortId;

            _refreshWnd();
        }
        
        private void _refreshWnd()
        {
            if(wnd == null)
                return;
            
            GConsortRefObj consortRefObj = GRefdataCoreMgr.instance.consortRefCore.getRef(_m_lConsortld);

            GGUIWndConsortBlessHeroSimpleIcon itemWnd = null;
            int wndCount = 0;
            if (consortRefObj != null && consortRefObj.relation_hero_id_list != null)
            {
                foreach (long heroId in consortRefObj.relation_hero_id_list)
                {
                    if(wndCount >= _m_lItemWndList.Count)
                    {
                        itemWnd = addItemWnd();
                        _m_lItemWndList.Add(itemWnd);
                    }
                    else
                    {
                        itemWnd = _m_lItemWndList[wndCount];
                    }

                    if (itemWnd == null)
                    {
                        itemWnd = addItemWnd();
                        _m_lItemWndList[wndCount] = itemWnd;
                    }
                    
                    if(itemWnd == null)
                        return;

                    wndCount++;
                    
                    itemWnd.showWnd();
                    itemWnd.setData(_m_lConsortld, heroId);
                }
            }
            
            // 若当前展示窗口数量不足最少展示数量，继续展示窗口
            for(;wndCount < wnd.leastShowItemNum; wndCount++)
            {
                if(wndCount >= _m_lItemWndList.Count)
                {
                    itemWnd = addItemWnd();
                    _m_lItemWndList.Add(itemWnd);
                }
                else
                {
                    itemWnd = _m_lItemWndList[wndCount];
                }

                if (itemWnd == null)
                {
                    itemWnd = addItemWnd();
                    _m_lItemWndList[wndCount] = itemWnd;
                }
                    
                if(itemWnd == null)
                    return;
                
                itemWnd.showWnd();
                itemWnd.setData(_m_lConsortld, 0);
            }
            
            // 隐藏多余的窗口
            for(int i = wndCount; i < _m_lItemWndList.Count; i++)
            {
                _m_lItemWndList[i]?.hideWnd();
            }
        }

        /// <summary>
        /// 处理所有item窗口
        /// </summary>
        public void dealAllItemWnd(Action<GGUIWndConsortBlessHeroSimpleIcon> _action)
        {
            if(_action == null)
                return;

            _m_lItemWndList.ForEach(_action);
        }
    }
}