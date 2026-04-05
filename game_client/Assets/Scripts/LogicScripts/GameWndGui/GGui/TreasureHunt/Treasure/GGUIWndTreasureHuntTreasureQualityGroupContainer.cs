using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 奇物品质组itemContainer
    /// </summary>
    public class GGUIWndTreasureHuntTreasureQualityGroupContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoTreasureHuntTreasureQualityGroupItem, GGUIMonoTreasureHuntTreasureQualityGroupContainer, GGUIWndTreasureHuntTreasureQualityGroupItem>
    {
        private List<GGUIWndTreasureHuntTreasureQualityGroupItem> _m_lItemWndList;

        public GGUIWndTreasureHuntTreasureQualityGroupContainer(GGUIMonoTreasureHuntTreasureQualityGroupContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
            _m_lItemWndList?.Clear();
            _m_lItemWndList = null;
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
        }

        protected override GGUIWndTreasureHuntTreasureQualityGroupItem _createItemWnd(GGUIMonoTreasureHuntTreasureQualityGroupItem _itemMono)
        {
            GGUIWndTreasureHuntTreasureQualityGroupItem itemWnd = new GGUIWndTreasureHuntTreasureQualityGroupItem(_itemMono);
            return itemWnd;
        }

        private void _dealAllItemWnd(Action<GGUIWndTreasureHuntTreasureQualityGroupItem> _action)
        {
            if(_action == null || _m_lItemWndList == null)
                return;
            
            foreach (var item in _m_lItemWndList)
            {
                _action(item);
            }
        }
        
        public void setData(Dictionary<EQuality, List<_ITreasureHuntTreasureInfo>> _m_dQualityTreasureInfoDic)
        {
            if(wnd == null)
                return;
            
            if (_m_lItemWndList == null)
                _m_lItemWndList = new List<GGUIWndTreasureHuntTreasureQualityGroupItem>();
            
            GGUIWndTreasureHuntTreasureQualityGroupItem itemWnd = null;
            List<_ITreasureHuntTreasureInfo> treasureInfoList = null;
            int wndCount = 0;
            
            if (_m_dQualityTreasureInfoDic != null && _m_dQualityTreasureInfoDic.Count > 0)
            {
                EQuality[] qualityEnumArray = (EQuality[]) Enum.GetValues(typeof(EQuality));
                // 从大到小排序
                Array.Sort(qualityEnumArray, (_enum1, _enum2) =>
                {
                    return -_enum1.CompareTo(_enum2);
                });

                foreach (var qualityEnum in qualityEnumArray)
                {
                    if(!_m_dQualityTreasureInfoDic.TryGetValue(qualityEnum, out treasureInfoList) || treasureInfoList == null || treasureInfoList.Count <= 0)
                        continue;
                    
                    if (wndCount >= _m_lItemWndList.Count)
                    {
                        itemWnd = addItemWnd();
                        if(itemWnd != null)
                        {
                            _m_lItemWndList.Add(itemWnd);
                        }
                    }
                    else
                    {
                        itemWnd = _m_lItemWndList[wndCount];
                    }

                    if (itemWnd != null)
                    {
                        itemWnd.showWnd();
                        itemWnd.setData(qualityEnum, treasureInfoList);

                        wndCount++;
                    }       
                }
            }

            for (int i = wndCount; i < _m_lItemWndList.Count; i++)
            {
                itemWnd = _m_lItemWndList[i];
                if(itemWnd != null)
                    itemWnd.hideWnd();
            }
            
            ALUGUICommon.setGameObjEnable(wnd.noItemShow, wndCount <= 0);
        }

        public void setData(List<_ITreasureHuntTreasureInfo> _treasureInfoList)
        {
            Dictionary<EQuality, List<_ITreasureHuntTreasureInfo>> qualityTreasureInfoDic = new Dictionary<EQuality, List<_ITreasureHuntTreasureInfo>>();
            List<_ITreasureHuntTreasureInfo> tmpTreasureInfoList = null;
            foreach (var treasureInfo in _treasureInfoList)
            {
                if(treasureInfo == null || treasureInfo.treasureRefObj == null)
                    continue;

                if (!qualityTreasureInfoDic.TryGetValue(treasureInfo.treasureRefObj.quality, out tmpTreasureInfoList) || tmpTreasureInfoList == null)
                {
                    tmpTreasureInfoList = new List<_ITreasureHuntTreasureInfo>();
                    qualityTreasureInfoDic[treasureInfo.treasureRefObj.quality] = tmpTreasureInfoList;
                }
                
                tmpTreasureInfoList.Add(treasureInfo);
            }

            setData(qualityTreasureInfoDic);
        }
    }
}