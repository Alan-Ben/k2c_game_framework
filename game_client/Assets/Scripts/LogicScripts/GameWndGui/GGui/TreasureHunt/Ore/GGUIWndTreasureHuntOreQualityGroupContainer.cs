using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 矿石品质组itemContainer
    /// </summary>
    public class GGUIWndTreasureHuntOreQualityGroupContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoTreasureHuntOreQualityGroupItem, GGUIMonoTreasureHuntOreQualityGroupContainer, GGUIWndTreasureHuntOreQualityGroupItem>
    {
        private List<GGUIWndTreasureHuntOreQualityGroupItem> _m_lItemWndList;

        public GGUIWndTreasureHuntOreQualityGroupContainer(GGUIMonoTreasureHuntOreQualityGroupContainer _containerMono) : base(_containerMono)
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

        protected override GGUIWndTreasureHuntOreQualityGroupItem _createItemWnd(GGUIMonoTreasureHuntOreQualityGroupItem _itemMono)
        {
            GGUIWndTreasureHuntOreQualityGroupItem itemWnd = new GGUIWndTreasureHuntOreQualityGroupItem(_itemMono);
            return itemWnd;
        }

        private void _dealAllItemWnd(Action<GGUIWndTreasureHuntOreQualityGroupItem> _action)
        {
            if(_action == null || _m_lItemWndList == null)
                return;
            
            foreach (var item in _m_lItemWndList)
            {
                _action(item);
            }
        }
        
        public void setData(Dictionary<EQuality, List<_ITreasureHuntOreInfo>> _m_dQualityOreInfoDic)
        {
            if(wnd == null)
                return;
            
            if (_m_lItemWndList == null)
                _m_lItemWndList = new List<GGUIWndTreasureHuntOreQualityGroupItem>();
            
            GGUIWndTreasureHuntOreQualityGroupItem itemWnd = null;
            List<_ITreasureHuntOreInfo> oreInfoList = null;
            int wndCount = 0;
            
            if (_m_dQualityOreInfoDic != null && _m_dQualityOreInfoDic.Count > 0)
            {
                EQuality[] qualityEnumArray = (EQuality[]) Enum.GetValues(typeof(EQuality));
                // 从大到小排序
                Array.Sort(qualityEnumArray, (_enum1, _enum2) =>
                {
                    return -_enum1.CompareTo(_enum2);
                });

                foreach (var qualityEnum in qualityEnumArray)
                {
                    if(!_m_dQualityOreInfoDic.TryGetValue(qualityEnum, out oreInfoList) || oreInfoList == null || oreInfoList.Count <= 0)
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
                        itemWnd.setData(qualityEnum, oreInfoList);

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
        
        public void setData(List<_ITreasureHuntOreInfo> _oreInfoList)
        {
            Dictionary<EQuality, List<_ITreasureHuntOreInfo>> qualityOreInfoDic = new Dictionary<EQuality, List<_ITreasureHuntOreInfo>>();
            List<_ITreasureHuntOreInfo> tmpOreInfoList = null;
            foreach (var oreInfo in _oreInfoList)
            {
                if(oreInfo == null || oreInfo.oreRefObj == null)
                    continue;

                if (!qualityOreInfoDic.TryGetValue(oreInfo.oreRefObj.quality, out tmpOreInfoList) || tmpOreInfoList == null)
                {
                    tmpOreInfoList = new List<_ITreasureHuntOreInfo>();
                    qualityOreInfoDic[oreInfo.oreRefObj.quality] = tmpOreInfoList;
                }
                
                tmpOreInfoList.Add(oreInfo);
            }

            setData(qualityOreInfoDic);
        }
    }
}