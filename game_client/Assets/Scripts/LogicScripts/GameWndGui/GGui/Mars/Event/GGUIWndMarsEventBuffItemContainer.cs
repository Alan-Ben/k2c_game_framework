using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 火星基地事件Buff项容器
    /// </summary>
    public class GGUIWndMarsEventBuffItemContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoMarsEventBuffItem, GGUIMonoMarsEventBuffItemContainer, GGUIWndMarsEventBuffItem>
    {
        private Dictionary<long, _IMarsEventInfo> _m_dBuffIdToEventInfoDic;

        private List<GGUIWndMarsEventBuffItem> _m_lItemWndList;
            
        public GGUIWndMarsEventBuffItemContainer(GGUIMonoMarsEventBuffItemContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _updateEventInfo();
        }

        protected override void _onDiscard()
        {
            _m_dBuffIdToEventInfoDic?.Clear();
            _m_dBuffIdToEventInfoDic = null;
            
            _m_lItemWndList?.Clear();
            _m_lItemWndList = null;
        }

        protected override void _onShowWnd()
        {
            // 注册buff变化监听
            if (NPPlayer.instance?.playerBuffComp != null)
            {
                NPPlayer.instance.playerBuffComp.onChgPlayerBuff += _onPlayerBuffChg;
            }
            
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            // 反注册buff变化监听
            if (NPPlayer.instance?.playerBuffComp != null)
            {
                NPPlayer.instance.playerBuffComp.onChgPlayerBuff -= _onPlayerBuffChg;
            }
            
            if (_m_lItemWndList != null)
            {
                foreach (var itemWnd in _m_lItemWndList)
                {
                    itemWnd?.hideWnd();
                }
            }
        }

        protected override void _onReset()
        {
            if (_m_lItemWndList != null)
            {
                foreach (var itemWnd in _m_lItemWndList)
                {
                    itemWnd?.resetWnd();
                }
            }
        }
        
        protected override GGUIWndMarsEventBuffItem _createItemWnd(GGUIMonoMarsEventBuffItem _itemMono)
        {
            return new GGUIWndMarsEventBuffItem(_itemMono);
        }
        
        private void _updateEventInfo()
        {
            if (_m_dBuffIdToEventInfoDic == null)
                _m_dBuffIdToEventInfoDic = new Dictionary<long, _IMarsEventInfo>();
            _m_dBuffIdToEventInfoDic.Clear();

            foreach (var eventInfo in NPPlayer.instance.marsComp.peopleSubComponent.eventInfoList)
            {
                if(eventInfo == null || eventInfo.buffRefObj == null)
                    continue;

                _m_dBuffIdToEventInfoDic[eventInfo.buffId] = eventInfo;
            }
        }
        
        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || !isShow)
                return;

            if (_m_lItemWndList == null)
                _m_lItemWndList = new List<GGUIWndMarsEventBuffItem>();

            // 创建或更新item窗口来显示有效的buff
            int itemWndCount = 0;
            GGUIWndMarsEventBuffItem itemWnd = null;
            if (_m_dBuffIdToEventInfoDic != null)
            {
                foreach (var eventInfo in _m_dBuffIdToEventInfoDic.Values)
                {
                    if(eventInfo == null)
                        continue;

                    NPPlayerBuffInfo buffInfo = eventInfo.buffInfo;
                    if(buffInfo == null || buffInfo.hasExpired())
                        continue;
                    
                    // 如果容器内部个数不足则新增窗口
                    if(itemWndCount >= _m_lItemWndList.Count)
                    {
                        itemWnd = addItemWnd();
                        if (itemWnd == null)
                            continue;
                        
                        _m_lItemWndList.Add(itemWnd);
                    }
                    else
                    {
                        itemWnd = _m_lItemWndList[itemWndCount];
                    }
                    
                    if (itemWnd != null)
                    {
                        itemWnd.showWnd();
                        itemWnd.setData(eventInfo);
                        itemWndCount++;
                    }
                }
            }
            
            // 隐藏并移除容器中多余的视图
            for (int i = itemWndCount; i < _m_lItemWndList.Count; i++)
            {
                itemWnd = _m_lItemWndList[i];
                itemWnd?.hideWnd();
            }
        }

        /// <summary>
        /// 玩家buff变化处理
        /// </summary>
        /// <param name="_buffInfo">buff信息</param>
        /// <param name="_layer">层数</param>
        /// <param name="_leftTimeMS">剩余时间</param>
        private void _onPlayerBuffChg(NPPlayerBuffInfo _buffInfo, int _layer, long _leftTimeMS)
        {
            if (_buffInfo == null || !isShow || _m_dBuffIdToEventInfoDic == null)
                return;
            
            if(_m_dBuffIdToEventInfoDic.ContainsKey(_buffInfo.buffId))
            {
                _refreshWnd();
            }
        }
    }
}