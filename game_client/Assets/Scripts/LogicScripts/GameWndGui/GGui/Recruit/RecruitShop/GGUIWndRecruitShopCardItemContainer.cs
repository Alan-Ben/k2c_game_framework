using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public class GGUIWndRecruitShopCardItemContainer : _ANPGGUIWndBasicDiffItemContainer<GGUIMonoRecruitShopCardItemContainer, _IRecruitShopCardItemWnd>
    {
        [NotNull]private static Dictionary<Type, Func<_AALBasicUIWndMono, _IRecruitShopCardItemWnd>> _m_entryPointViewDict = new Dictionary<Type, Func<_AALBasicUIWndMono, _IRecruitShopCardItemWnd>>()
        {
            {typeof(GGUIMonoRecruitShopCardItemHero), (_mono) => new GGUIWndRecruitShopCardItemHero(_mono as GGUIMonoRecruitShopCardItemHero)},
            {typeof(GGUIMonoRecruitShopCardItemConsort), (_mono) => new GGUIWndRecruitShopCardItemConsort(_mono as GGUIMonoRecruitShopCardItemConsort)},
        };

        private List<_IRecruitShopCardItemWnd> _m_itemWndList = new List<_IRecruitShopCardItemWnd>();
        
        public GGUIWndRecruitShopCardItemContainer(GGUIMonoRecruitShopCardItemContainer _containerMono) : base(_containerMono)
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

        protected override _IRecruitShopCardItemWnd _createItemWnd(_AALBasicUIWndMono _itemMono)
        {
            if (_itemMono == null)
                return null;

            if (_m_entryPointViewDict.TryGetValue(_itemMono.GetType(), out Func<_AALBasicUIWndMono, _IRecruitShopCardItemWnd> func) && func != null)
            {
                return func(_itemMono);
            }

            return null;
        }

        protected override void _onAddItemWnd(_IRecruitShopCardItemWnd _itemWnd)
        {
        }
        
        private void _dealAllItemWnd(Action<_IRecruitShopCardItemWnd> _action)
        {
            if(_action == null || _m_itemWndList == null)
                return;
            
            foreach (var item in _m_itemWndList)
            {
                _action(item);
            }
        }
        
        public void setData(RecruitShopInfo _recruitShopInfo, List<RecruitRefObj> _recruitRefObjList)
        {
            if(wnd == null)
                return;
            
            if (_m_itemWndList == null)
                _m_itemWndList = new List<_IRecruitShopCardItemWnd>();
            
            _IRecruitShopCardItemWnd itemWnd = null;
            int wndCount = 0;
            
            if (_recruitRefObjList != null && _recruitRefObjList.Count > 0)
            {
                foreach (RecruitRefObj refObj in _recruitRefObjList)
                {
                    if(refObj == null || refObj.gain_item == null)
                        continue;
                    
                    if (wndCount >= _m_itemWndList.Count)
                    {
                        itemWnd = addItemWnd(wnd.getItemPrefab(refObj.gain_item.getItemType()));
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
                        itemWnd.setData(_recruitShopInfo, new RecruitConsortItemInfo(refObj));

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