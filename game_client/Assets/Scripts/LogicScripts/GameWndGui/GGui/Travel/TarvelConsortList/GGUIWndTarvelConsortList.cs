using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 游历妃子列表
    /// </summary>
    public class GGUIWndTarvelConsortList: _ANPGGUIBasicWnd<GGUIMonoTarvelConsortList>
    {
        private static GGUIWndTarvelConsortList _g_instance = new GGUIWndTarvelConsortList();

        public static GGUIWndTarvelConsortList instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new  GGUIWndTarvelConsortList();
                return _g_instance;
            }
        }
        
        private GGUIWndTarvelConsortItemContainer _m_consortItemContainer;

        public GGUIWndTarvelConsortList() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoTarvelConsortList.assetPath; }
        protected override string _monoObjName { get => GGUIMonoTarvelConsortList.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_consortItemContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_consortItemContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (_m_consortItemContainer != null)
            {
                _m_consortItemContainer.onItemClick -= _onItemClick;
                _m_consortItemContainer.discard();
            }
            _m_consortItemContainer = null;       
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnClose, _clickClose);

            if (null != wnd.itemContainer)
            {
                _m_consortItemContainer = new GGUIWndTarvelConsortItemContainer(wnd.itemContainer);
                _m_consortItemContainer.onItemClick += _onItemClick;
            }
        }

        private void _clickClose(GameObject obj)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }

        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            if (null != _m_consortItemContainer)
            {
                List<_IConsortShowInfo> list = new List<_IConsortShowInfo>();
                Dictionary<long, ETravelConsortUnlockStat> travelConsortUnlockStatDic = new Dictionary<long, ETravelConsortUnlockStat>();
                
                GRefdataCoreMgr.instance.consortRefCore.dealAllRef(_refObj =>
                {
                    if (null == _refObj)
                        return;
                    
                    list.Add(new ConsortInfo_UnlockNotAutoRefresh(_refObj));
                    travelConsortUnlockStatDic[_refObj.id] = NPPlayer.instance.travelComp.getTravelConsortUnlockStat(_refObj.id);
                });
                
                //排序
                list.Sort((_a, _b) =>
                {
                    if (_b == null)
                        return -1;
                    if (_a == null)
                        return 1;
                    if (ReferenceEquals(_a, _b))
                        return 0;
                    
                    ETravelConsortUnlockStat unlockStatA = travelConsortUnlockStatDic[_a.consortId];//上面在遍历的时候已经赋值了, 所以travelConsortUnlockStatDic中必定有这个key
                    ETravelConsortUnlockStat unlockStatB = travelConsortUnlockStatDic[_b.consortId];
                    int compareResult = ETravelConsortUnlockStatComparer.compare(unlockStatA, unlockStatB);
                    if (compareResult != 0)
                        return compareResult;
                    
                    long posIdA = _a.consortRefObj?.travel_pos ?? 0;
                    long posIdB = _b.consortRefObj?.travel_pos ?? 0;
                    compareResult = posIdA.CompareTo(posIdB);
                    if (compareResult != 0)
                        return compareResult;

                    return _a.consortId.CompareTo(_b.consortId);
                });
                
                _m_consortItemContainer.showWnd();
                _m_consortItemContainer.showItemList(list, travelConsortUnlockStatDic);
            }
        }

        private void _onItemClick(GGUIWndTarvelConsortItem _itemWnd)
        {
            if(_itemWnd == null || _itemWnd.consortShowInfo == null)
                return;
            
            QueueMgr.instance.AddNode(new GNodeLockConsortDetail(_itemWnd.consortShowInfo));
        }
    }
}