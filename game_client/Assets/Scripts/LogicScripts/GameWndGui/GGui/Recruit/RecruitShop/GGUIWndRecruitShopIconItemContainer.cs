using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUIWndRecruitShopIconItemContainer : _ANPGGUIWndBasicDiffItemContainer<GGUIMonoRecruitShopIconItemContainer, _IRecruitShopIconItemWnd>
    {
        private List<_IRecruitShopIconItemWnd> _m_itemWndList = new List<_IRecruitShopIconItemWnd>();
        private _IRecruitShopIconItemWnd _m_wNowSelectItemWnd = null;
        
        public GGUIWndRecruitShopIconItemContainer(GGUIMonoRecruitShopIconItemContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        public event Action<_IRecruitShopIconItemWnd> onItemClick; 

        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
            onItemClick = null;
            _m_wNowSelectItemWnd = null;
            
            _m_itemWndList?.Clear();
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wNowSelectItemWnd = null;
            _dealAllItemWnd((_itemWnd) => _itemWnd?.hideWnd());
        }

        protected override void _onReset()
        {
            _m_itemWndList?.Clear();
        }

        protected override _IRecruitShopIconItemWnd _createItemWnd(_AALBasicUIWndMono _itemMono)
        {
            if (_itemMono == null)
                return null;

            return _getItemWnd(_itemMono);
        }
        
        private _IRecruitShopIconItemWnd _getItemWnd(_AALBasicUIWndMono _mono)
        {
            if (_mono == null)
                return null;

            if (_mono is GGUIMonoRecruitShopIconItem_Consort)
            {
                GGUIWndRecruitShopIconItem_Consort consortItemWnd = new GGUIWndRecruitShopIconItem_Consort(_mono as GGUIMonoRecruitShopIconItem_Consort);
                consortItemWnd.onClick += _onItemClick;
                return consortItemWnd;
            }
            else if (_mono is GGUIMonoRecruitShopIconItem_Hero)
            {
                GGUIWndRecruitShopIconItem_Hero heroItemWnd = new GGUIWndRecruitShopIconItem_Hero(_mono as GGUIMonoRecruitShopIconItem_Hero);
                heroItemWnd.onClick += _onItemClick;
                return heroItemWnd;
            }
            else if(_mono is GGUIMonoRecruitShopIconItem_Normal)
            {
                GGUIWndRecruitShopIconItem_Normal normalItemWnd = new GGUIWndRecruitShopIconItem_Normal(_mono as GGUIMonoRecruitShopIconItem_Normal);
                normalItemWnd.onClick += _onItemClick;
                return normalItemWnd;
            }
            else
            {
                return null;
            }
        }

        protected override void _onAddItemWnd(_IRecruitShopIconItemWnd _itemWnd)
        {
        }
        
        private void _dealAllItemWnd(Action<_IRecruitShopIconItemWnd> _action)
        {
            if(_action == null || _m_itemWndList == null)
                return;
            
            foreach (var item in _m_itemWndList)
            {
                _action(item);
            }
        }
        
        public void setData(RecruitShopInfo _recruitShopInfo, List<_ARecruitItemInfo> _recruitItemInfoList)
        {
            if(wnd == null)
                return;
            
            if (_m_itemWndList == null)
                _m_itemWndList = new List<_IRecruitShopIconItemWnd>();
            
            _IRecruitShopIconItemWnd itemWnd = null;
            int wndCount = 0;
            
            if (_recruitItemInfoList != null && _recruitItemInfoList.Count > 0)
            {
                foreach (_ARecruitItemInfo recruitItemInfo in _recruitItemInfoList)
                {
                    if(recruitItemInfo == null)
                        continue;
                    
                    if (wndCount >= _m_itemWndList.Count)
                    {
                        itemWnd = addItemWnd(wnd.getItemPrefab(recruitItemInfo.recruitItemType));
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
                        itemWnd.setData(_recruitShopInfo, recruitItemInfo);
                        itemWnd.setItemSelect(false);

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

        private void _onItemClick(_IRecruitShopIconItemWnd _itemWnd)
        {
            if(_itemWnd == null || _m_wNowSelectItemWnd == _itemWnd)
                return;
            
            onItemClick?.Invoke(_itemWnd);
        }

        public void setItemSelect(_ARecruitItemInfo _recruitItemInfo)
        {
            if(_recruitItemInfo == null)
                return;

            _IRecruitShopIconItemWnd selectItemWnd = null;
            foreach (_IRecruitShopIconItemWnd itemWnd in _m_itemWndList)
            {
                if (itemWnd != null && itemWnd.recruitItemInfo == _recruitItemInfo)
                {
                    selectItemWnd = itemWnd;
                    break;
                }
            }

            if (selectItemWnd != null)
                setItemSelect(selectItemWnd);
        }
        
        /// <summary>
        /// 设置某item被点击
        /// </summary>
        /// <param name="_itemWnd"></param>
        public void setItemSelect(_IRecruitShopIconItemWnd _itemWnd)
        {
            if (_itemWnd == null || _m_wNowSelectItemWnd == _itemWnd)
                return;

            _m_wNowSelectItemWnd?.setItemSelect(false);
            _m_wNowSelectItemWnd = _itemWnd;
            _m_wNowSelectItemWnd.setItemSelect(true);
        }
    }
}