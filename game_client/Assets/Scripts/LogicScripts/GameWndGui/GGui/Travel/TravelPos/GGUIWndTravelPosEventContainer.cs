using System;
using System.Collections.Generic;
using ALPackage;
using Common.TravelEnum;

namespace GOE
{
    /// <summary>
    /// 游历地点事件容器Wnd
    /// </summary>
    public class GGUIWndTravelPosEventContainer : _ANPGGUIWndBasicDiffItemContainer<GGUIMonoTravelPosEventContainer, _ITravelPosEventItemWnd>
    {
        /// <summary>
        /// Mono类型 → Wnd构造函数 工厂字典
        /// </summary>
        private static readonly Dictionary<Type, Func<_AALBasicUIWndMono, _ITravelPosEventItemWnd>> _g_dItemWndFactory
            = new Dictionary<Type, Func<_AALBasicUIWndMono, _ITravelPosEventItemWnd>>()
        {
            { typeof(GGUIMonoTravelPosEventItemCommon), (_mono) => new GGUIWndTravelPosEventItemCommon(_mono as GGUIMonoTravelPosEventItemCommon) },
            { typeof(GGUIMonoTravelPosEventItemConsortLike), (_mono) => new GGUIWndTravelPosEventItemConsortLike(_mono as GGUIMonoTravelPosEventItemConsortLike) },
            { typeof(GGUIMonoTravelPosEventItemConsortIntimacy), (_mono) => new GGUIWndTravelPosEventItemConsortIntimacy(_mono as GGUIMonoTravelPosEventItemConsortIntimacy) },
        };

        /// <summary>
        /// 已创建的item窗口列表（含隐藏的，用于复用）
        /// </summary>
        private List<_ITravelPosEventItemWnd> _m_lItemWndList = new List<_ITravelPosEventItemWnd>();

        public GGUIWndTravelPosEventContainer(GGUIMonoTravelPosEventContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }


        protected override _ITravelPosEventItemWnd _createItemWnd(_AALBasicUIWndMono _itemMono)
        {
            if (_itemMono == null)
                return null;

            if (_g_dItemWndFactory.TryGetValue(_itemMono.GetType(), out Func<_AALBasicUIWndMono, _ITravelPosEventItemWnd> _func) && _func != null)
                return _func(_itemMono);

            Debug.LogError($"没有找到游历地点事件item窗口的工厂函数，Mono类型：{_itemMono.GetType()}");
            return null;
        }

        protected override void _onAddItemWnd(_ITravelPosEventItemWnd _itemWnd)
        {
        }

        protected override void _onWndInitDone()
        {
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_lItemWndList?.Clear();
            _m_lItemWndList = null;
        }


        /// <summary>
        /// 设置事件数据并刷新容器
        /// </summary>
        /// <param name="_eventRefObjList">事件信息列表</param>
        public void setData(List<TravelEventRefObj> _eventRefObjList)
        {
            if (wnd == null)
                return;

            if (_m_lItemWndList == null)
                _m_lItemWndList = new List<_ITravelPosEventItemWnd>();

            _ITravelPosEventItemWnd itemWnd = null;
            int wndCount = 0;

            if (_eventRefObjList != null && _eventRefObjList.Count > 0)
            {
                for (int i = 0; i < _eventRefObjList.Count; i++)
                {
                    TravelEventRefObj eventRefObj = _eventRefObjList[i];
                    if (eventRefObj == null)
                        continue;

                    if (wndCount >= _m_lItemWndList.Count)
                    {
                        // 不够用，新增item
                        _AGGUIMonoTravelPosEventItem prefab = wnd.getItemPrefab(eventRefObj.eEventType);
                        if (prefab == null)
                            continue;

                        itemWnd = addItemWnd(prefab);
                        if (itemWnd != null)
                            _m_lItemWndList.Add(itemWnd);
                    }
                    else
                    {
                        itemWnd = _m_lItemWndList[wndCount];
                    }

                    if (itemWnd != null)
                    {
                        itemWnd.showWnd();
                        itemWnd.setData(eventRefObj);
                        wndCount++;
                    }
                }
            }

            // 隐藏多余的item
            for (int i = wndCount; i < _m_lItemWndList.Count; i++)
            {
                itemWnd = _m_lItemWndList[i];
                if (itemWnd != null)
                    itemWnd.hideWnd();
            }
        }
    }
}