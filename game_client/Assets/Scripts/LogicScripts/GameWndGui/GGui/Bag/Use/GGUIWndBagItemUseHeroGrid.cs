using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 使用物品-伙伴列表
    /// </summary>
    public class GGUIWndBagItemUseHeroGrid : _ANPGGUIBasicGridSubWnd<GGUIMonoBagItemUseHeroGridItem, GGUIMonoBagItemUseHeroGrid, GGUIWndBagItemUseHeroGridItem>
    {
        //骑士列表数据
        private List<HeroInfo> _m_heroInfoList;
        //使用道具
        private BagItem _m_bagItem;
        //使用回调
        private Action<BagItem,long> _m_useAction;

        public GGUIWndBagItemUseHeroGrid(GGUIMonoBagItemUseHeroGrid _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.BAG_ITEM_USE_HERO_GRID_SCROLL_MOVE_TO, _onScrollMoveToHero);
            WinMsg.RegisterMsg(WinMsgType.BAG_ITEM_USE_HERO_GRID_USE_FOR_HERO, _simulateUseForHero);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.BAG_ITEM_USE_HERO_GRID_SCROLL_MOVE_TO, _onScrollMoveToHero);
            WinMsg.UnregisterMsg(WinMsgType.BAG_ITEM_USE_HERO_GRID_USE_FOR_HERO, _simulateUseForHero);
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_heroInfoList?.Clear();
            _m_heroInfoList = null;
        }

        protected override void _onWndInitDone()
        {
        }

        protected override GGUIWndBagItemUseHeroGridItem _createItemWnd(GGUIMonoBagItemUseHeroGridItem _itemMono)
        {
            // 创建对象
            return new GGUIWndBagItemUseHeroGridItem(_itemMono, _m_useAction);
        }

        protected override void _refreshItemwnd(GGUIWndBagItemUseHeroGridItem _itemWnd, int _itemIdx)
        {
            if (_m_heroInfoList == null || _itemIdx >= _m_heroInfoList.Count)
                return;

            //获取数据对象
            HeroInfo heroInfo = _m_heroInfoList[_itemIdx];
            if (null == heroInfo)
                return;

            _itemWnd.setItem(_m_bagItem, heroInfo);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_bagItem"></param>
        /// <param name="_useAction"></param>
        public void setInfo(BagItem _bagItem, Action<BagItem,long> _useAction)
        {
            if (wnd == null || _bagItem == null)
                return;

            _m_bagItem = _bagItem;
            _m_useAction = _useAction;

            //获取当前拥有的骑士数据
            if(_m_heroInfoList == null)
                _m_heroInfoList = new List<HeroInfo>();
            _m_heroInfoList.Clear();

            //获取伙伴列表
            NPPlayer.instance.heroComponent.getAllList(_m_heroInfoList);
            //按照实力从大到小排序
            _m_heroInfoList.Sort((_a,_b)=> -(_a.power.CompareTo(_b.power)));
            //刷新grid
            setItemCount(_m_heroInfoList.Count);
            //显示无数据提示
            ALUGUICommon.setGameObjEnable(wnd.zeroShowGoList, _m_heroInfoList.Count == 0);
        }
        
        /// <summary>
        /// 滚动到指定下标的位置
        /// </summary>
        /// <param name="_index">下标</param>
        /// <param name="_smoothTime">平滑移动时间，默认0.25秒</param>
        /// <param name="_complete">移动完成回调</param>
        public void scrollMoveToIndex(int _index, EScrollToItemType _scrollToItemType = EScrollToItemType.TOP_OR_RIGHT, bool _needFade = true, float _smoothTime = 0.25f, Action _complete = null)
        {
            if (wnd == null || _index < 0)
            {
                _complete?.Invoke();
                return;
            }

            MoveItem(_index, _scrollToItemType, _needFade, _smoothTime, _complete);
        }

        /// <summary>
        /// 滚动到指定类型的伙伴
        /// 参数：_objs[0] 为 EGGUIMonoBagItemUseHeroGridTargetHeroType 枚举类型
        /// </summary>
        private void _onScrollMoveToHero(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || _objs[0] == null)
                return;

            EGGUIMonoBagItemUseHeroGridTargetHeroType _targetHeroType;
            if(_objs[0] is EGGUIMonoBagItemUseHeroGridTargetHeroType)
                _targetHeroType = (EGGUIMonoBagItemUseHeroGridTargetHeroType)_objs[0];
            else if(_objs[0] is string)
                ALCommon.TryEnumParse(typeof(EGGUIMonoBagItemUseHeroGridTargetHeroType), (string)_objs[0], out _targetHeroType);
            else
                return;
            
            string paramsStr = _objs.Length >= 2 && _objs[1] is string ? (string)_objs[1] : string.Empty;
            string[] paramsArr = paramsStr?.Split(':');
            
            switch (_targetHeroType)
            {
                case EGGUIMonoBagItemUseHeroGridTargetHeroType.INDEX:
                    if(paramsArr == null || paramsArr.Length < 1)
                        break;
                    
                    if(!int.TryParse(paramsArr[0], out int index))
                        break;

                    float moveTime = 0f;
                    if(paramsArr.Length >= 2)
                    {
                        ALCommon.TryParseFloat(paramsArr[1], out moveTime);
                    }
                    
                    EScrollToItemType scrollType = EScrollToItemType.TOP_OR_RIGHT;
                    if(paramsArr.Length >= 3)
                    {
                        if(ALCommon.TryEnumParse(typeof(EScrollToItemType), paramsArr[2], out EScrollToItemType parsedScrollType))
                        {
                            scrollType = parsedScrollType;
                        }
                    }
                    
                    scrollMoveToIndex(index, scrollType, moveTime > 0f, moveTime);
                    break;
                default:
                    Debug.LogError_EditorOnly($"[GGUIWndBagItemUseHeroGrid _onScrollMoveToHero] 未实现滚动目标类型：{_targetHeroType}");
                    break;
            }
        }

        private void _simulateUseForHero(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || _objs[0] == null)
                return;

            EGGUIMonoBagItemUseHeroGridTargetHeroType _targetHeroType;
            if(_objs[0] is EGGUIMonoBagItemUseHeroGridTargetHeroType)
                _targetHeroType = (EGGUIMonoBagItemUseHeroGridTargetHeroType)_objs[0];
            else if(_objs[0] is string)
                ALCommon.TryEnumParse(typeof(EGGUIMonoBagItemUseHeroGridTargetHeroType), (string)_objs[0], out _targetHeroType);
            else
                return;
            
            string paramsStr = _objs.Length >= 2 && _objs[1] is string ? (string)_objs[1] : string.Empty;
            string[] paramsArr = paramsStr?.Split(':');
            
            switch (_targetHeroType)
            {
                case EGGUIMonoBagItemUseHeroGridTargetHeroType.INDEX:
                    if(paramsArr == null || paramsArr.Length < 1)
                        break;
                    
                    if(!int.TryParse(paramsArr[0], out int index))
                        break;
                    
                    refreshAllItem((_itemWnd, _index) =>
                    {
                        if (_index == index && _itemWnd != null)
                        {
                            _itemWnd.simulateClickUse();
                        }
                    });
                    
                    break;
                default:
                    Debug.LogError_EditorOnly($"[GGUIWndBagItemUseHeroGrid _simulateUseForHero] 未实现使用目标类型：{_targetHeroType}");
                    break;
            }
        }
    }
}
