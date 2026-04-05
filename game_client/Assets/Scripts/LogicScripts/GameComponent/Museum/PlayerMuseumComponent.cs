using System;
using System.Collections.Generic;
using ALPackage;
using Common.MuseumObj;
using GC2GS.p002_InitOp;
using GS2GC.p002_InitOp;
using GS2GC.p035_MuseumOp;
using JetBrains.Annotations;

namespace GOE
{
    public partial class PlayerMuseumComponent : _ANPBasicPlayerComponent
    {
        [ItemNotNull, NotNull] private readonly List<MuseumItemInfo> _m_itemList;
        [NotNull] private readonly Dictionary<long, MuseumItemInfo> _m_itemDict;
        [NotNull] private readonly CommonUnionBonusMgr _m_bonusMgr;
        [NotNull] private readonly RedTipDealer _m_redTipDealer;
        

        public PlayerMuseumComponent(NPPlayerComponentMgr _compMgr) 
            : base(_compMgr)
        {
            _m_itemList = new List<MuseumItemInfo>();
            _m_itemDict = new Dictionary<long, MuseumItemInfo>();
            _m_bonusMgr = new CommonUnionBonusMgr(EUnionBonusMgrTag.MUSEUM);
            _m_redTipDealer = new RedTipDealer(this);
        }
        
        
        public event Action<MuseumItemInfo> onItemChg;
        
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.MUSEUM; } }
        public override ENPPlayerCompType[] dependCompList { get { return null; } }
        public override bool canPreInit { get { return true; } }
        public int allItemCount { get { return _m_itemList.Count; } }
        public CommonUnionBonusMgr bonusMgr { get { return _m_bonusMgr; } }
        
        
        public override void presendInitProtocol()
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_002_058_ReqMuseumInit(), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_002_058_RetMuseumInit>((_isSuc, _msg) =>
                {
                    dealPreInitFunc(() =>
                    {
                        if (_isSuc)
                        {
                            _initData(_msg);
                            setInitDone();
                        }
                        else
                            setInitFail();
                    });
                }));
        }
        protected override void _dealInit()
        {
        }
        protected override void _onInitDone()
        {
            _m_bonusMgr.setParent(NPPlayer.instance.playerBonusMgr);
            _m_redTipDealer.init();
        }
        protected override void _onInitFail()
        {
            ALLog.Error("PlayerMuseumComponent init fail!");
        }
        protected override void _discard()
        {
            _m_redTipDealer.clear();
            _m_bonusMgr.clear();
            _m_itemList.Clear();
            _m_itemDict.Clear();
        }
        
        
        [Pure, ItemNotNull, NotNull]
        public List<MuseumItemInfo> getItemList()
        {
            return new List<MuseumItemInfo>(_m_itemList);
        }
        public void getItemListNonAlloc(List<MuseumItemInfo> _list)
        {
            if (_list == null)
                return;
            
            _list.Clear();
            _list.AddRange(_m_itemList);
        }
        public MuseumItemInfo getItemInfoById(long _itemId)
        {
            return _m_itemDict.GetValueOrDefault(_itemId);
        }
        public int getObtainedItemCount()
        {
            int count = 0;
            foreach (MuseumItemInfo itemInfo in _m_itemList)
            {
                if (itemInfo.isObtain)
                    count++;
            }
            return count;
        }
        
        
        private void _initData(GS2GC_002_058_RetMuseumInit _msg)
        {
            _m_itemList.Clear();
            _m_itemDict.Clear();
            
            GRefdataCoreMgr.instance.museumItemRefCore.dealAllRef(_itemRef =>
            {
                if (_itemRef == null)
                    return;
                
                MuseumItemInfo itemInfo = new MuseumItemInfo(_itemRef, _m_bonusMgr);
                _m_itemList.Add(itemInfo);
                _m_itemDict[_itemRef.id] = itemInfo;
            });
            
            if (_msg != null)
            {
                List<Museum_ItemInfo> serverItemList = _msg.getItemList();
                if (serverItemList != null)
                {
                    foreach (Museum_ItemInfo serverItem in serverItemList)
                    {
                        if (serverItem == null)
                            continue;
                        
                        MuseumItemInfo itemInfo = getItemInfoById(serverItem.getItemId());
                        itemInfo?._updateFromServerData(serverItem);
                    }
                }
            }
        }
        

        internal void _onMuseumItemAdd(GS2GC_035_050_OnMuseumItemAdd _msg)
        {
            Museum_ItemInfo serverInfo = _msg?.getItemInfo();
            _onMuseumItemChg(serverInfo);
        }
        internal void _onMuseumItemChg(GS2GC_035_051_OnMuseumItemChg _msg)
        {
            Museum_ItemInfo serverInfo = _msg?.getItemInfo();
            _onMuseumItemChg(serverInfo);
        }
        internal void _onMuseumItemChg(Museum_ItemInfo _serverInfo)
        {
            if (_serverInfo == null)
                return;
            
            MuseumItemInfo itemInfo = getItemInfoById(_serverInfo.getItemId());
            if (itemInfo == null)
            {
                ALLog.Error($"PlayerMuseumComponent._onMuseumItemChg: itemInfo is null, itemId={_serverInfo.getItemId()}");
                return;
            }
            
            itemInfo._updateFromServerData(_serverInfo);
            onItemChg?.Invoke(itemInfo);
            _m_redTipDealer.refreshItemRedTip();
            GCommon.reloadCustomLoadPrefab();
        }
    }
}