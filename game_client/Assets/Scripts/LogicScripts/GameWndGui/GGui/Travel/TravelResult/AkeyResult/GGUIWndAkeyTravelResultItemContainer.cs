using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUIWndAkeyTravelResultItemContainer : _ANPGGUIWndBasicDiffItemContainer<GGUIMonoAkeyTravelResultItemContainer, _IAkeyTravelResultItemWnd>
    {
        [NotNull]private static Dictionary<Type, Func<_AALBasicUIWndMono, _IAkeyTravelResultItemWnd>> _m_dAkeyTravelResultMonoWndDic = new Dictionary<Type, Func<_AALBasicUIWndMono, _IAkeyTravelResultItemWnd>>()
        {
            {typeof(GGUIMonoAkeyTravelConsortEventResultItem), (_mono) => new GGUIWndAkeyTravelConsortEventResultItem(_mono as GGUIMonoAkeyTravelConsortEventResultItem)},
            {typeof(GGUIMonoAkeyTravelCommonEventResultItem), (_mono) => new GGUIWndAkeyTravelCommonEventResultItem(_mono as GGUIMonoAkeyTravelCommonEventResultItem)},
            {typeof(GGUIMonoAkeyTravelChangeEventResultItem), (_mono) => new GGUIWndAkeyTravelChangeEventResultItem(_mono as GGUIMonoAkeyTravelChangeEventResultItem)},
            {typeof(GGUIMonoAkeyTravelGambleEventResultItem), (_mono) => new GGUIWndAkeyTravelGambleEventResultItem(_mono as GGUIMonoAkeyTravelGambleEventResultItem)},
        };

        private List<_IAkeyTravelResultItemWnd> _m_itemWndList = new List<_IAkeyTravelResultItemWnd>();
        
        public GGUIWndAkeyTravelResultItemContainer(GGUIMonoAkeyTravelResultItemContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
            _m_itemWndList?.Clear();
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _dealAllItemWnd((_itemWnd) => _itemWnd?.hideWnd());
        }

        protected override void _onReset()
        {
            _m_itemWndList?.Clear();
        }

        protected override _IAkeyTravelResultItemWnd _createItemWnd(_AALBasicUIWndMono _itemMono)
        {
            if (_itemMono == null)
                return null;

            if (_m_dAkeyTravelResultMonoWndDic.TryGetValue(_itemMono.GetType(), out Func<_AALBasicUIWndMono, _IAkeyTravelResultItemWnd> func) && func != null)
            {
                return func(_itemMono);
            }

            return null;
        }

        protected override void _onAddItemWnd(_IAkeyTravelResultItemWnd _itemWnd)
        {
        }
        
        private void _dealAllItemWnd(Action<_IAkeyTravelResultItemWnd> _action)
        {
            if(_action == null || _m_itemWndList == null)
                return;
            
            foreach (var item in _m_itemWndList)
            {
                _action(item);
            }
        }
        
        public void setData(List<_ITravelEventResultInfo> _eventInfoList)
        {
            if(wnd == null)
                return;
            
            if (_m_itemWndList == null)
                _m_itemWndList = new List<_IAkeyTravelResultItemWnd>();
            
            _IAkeyTravelResultItemWnd itemWnd = null;
            int wndCount = 0;
            
            if (_eventInfoList != null && _eventInfoList.Count > 0)
            {
                foreach (_ITravelEventResultInfo resultInfo in _eventInfoList)
                {
                    if(resultInfo == null || resultInfo.eventInfo == null)
                        continue;
                    
                    if (wndCount >= _m_itemWndList.Count)
                    {
                        itemWnd = addItemWnd(wnd.getItemPrefab(resultInfo.eventInfo.eventType));
                        if(itemWnd != null)
                        {
                            _m_itemWndList.Add(itemWnd);
                        }
                    }
                    else
                    {
                        itemWnd = _m_itemWndList[wndCount];
                    }

                    if (itemWnd != null)
                    {
                        itemWnd.showWnd();
                        itemWnd.setData(resultInfo);

                        wndCount++;
                    }
                }
            }

            for (int i = wndCount; i < _m_itemWndList.Count; i++)
            {
                itemWnd = _m_itemWndList[i];
                if(itemWnd != null)
                    itemWnd.hideWnd();
            }
            
            ALUGUICommon.setGameObjEnable(wnd.noItemShow, wndCount <= 0);
        }
    }
}