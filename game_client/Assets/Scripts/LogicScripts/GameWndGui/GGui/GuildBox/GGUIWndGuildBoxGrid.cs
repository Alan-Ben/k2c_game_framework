using ALPackage;
using DG.Tweening;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 联盟宝箱容器
    /// </summary>
    public class GGUIWndGuildBoxGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoGuildBoxGridItem,GGUIMonoGuildBoxGrid,GGUIWndGuildBoxGridItem>
    {
        [NotNull] private List<GuildBoxInfo> _m_itemDataList = new List<GuildBoxInfo>();
        [NotNull] private Dictionary<long, string> _m_dPlayerNameDic = new Dictionary<long, string>();
        
        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>
        
        public GGUIWndGuildBoxGrid(GGUIMonoGuildBoxGrid gridMono) : base(gridMono)
        {
            initWnd();
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
            _m_itemDataList.Clear();
            _m_dPlayerNameDic.Clear();
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
        }

        protected override void _onRefreshItemWnd(GGUIWndGuildBoxGridItem _itemWnd, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_itemDataList.Count)
                return;

            GuildBoxInfo boxInfo = _m_itemDataList[_itemIdx];
            if (boxInfo == null)
                return;

            _m_dPlayerNameDic.TryGetValue(boxInfo.senderCid, out string playerName);
            _itemWnd?.setInfo(boxInfo, playerName, _onScrollList, (_cid, _name) =>
            {
                _m_dPlayerNameDic[_cid] = _name;
            });

        }

        protected override GGUIWndGuildBoxGridItem _createItemWnd(GGUIMonoGuildBoxGridItem _itemMono)
        {
            GGUIWndGuildBoxGridItem itemWnd = new GGUIWndGuildBoxGridItem(_itemMono);
            return itemWnd;
        }

        /// <summary>
        /// 刷新倒计时
        /// </summary>
        public void refreshCD()
        {
            refreshAllItem((_item, _index) =>
            {
                _item?.refreshCD();
            });
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(List<GuildBoxInfo> _itemDataList)
        {
            if (_itemDataList == null)
            {
                if (wnd != null) ALUGUICommon.setGameObjEnable(wnd.noItemShowList, true);
                setItemCount(0);
                return;
            }

            _m_itemDataList.Clear();
            _m_itemDataList.AddRange(_itemDataList);
            _m_itemDataList.Sort(_sortList);

            setItemCount(_m_itemDataList.Count);

            if (wnd != null) 
                ALUGUICommon.setGameObjEnable(wnd.noItemShowList, _m_itemDataList.Count <= 0);
        }

        /// <summary>
        /// 检查并刷新列表，移除过期的item
        /// </summary>
        public void checkRefreshInfoList()
        {
            for (int i = _m_itemDataList.Count - 1; i >= 0; i--)
            {
                if(_m_itemDataList[i] == null || _m_itemDataList[i].endTimeMs<=FpsAndPingMgr.instance.serverTimeTag)
                    _m_itemDataList.RemoveAt(i);
            }

            setItemCount(_m_itemDataList.Count);

            if (wnd != null)
                ALUGUICommon.setGameObjEnable(wnd.noItemShowList, _m_itemDataList.Count <= 0);
        }

        //排序：未领取>已领取，结束时间远>结束时间近
        private int _sortList(GuildBoxInfo _a, GuildBoxInfo _b)
        {
            if (_a == null || _b == null)
                return 0;

            if (_a.isArealdyGain != _b.isArealdyGain)
                return _a.isArealdyGain.CompareTo(_b.isArealdyGain);

            return -(_a.endTimeMs.CompareTo(_b.endTimeMs));
        }

        /// <summary>
        /// 点击领取奖励滚动列表
        /// </summary>
        /// <param name="_index"></param>
        private void _onScrollList(int _index)
        {
            //第一个和最后一个不处理
            if (_index == 0 || _index == _m_itemDataList.Count - 1 || wnd == null || !isShow)
                return;

            if (wnd.gridAreaUIObj != null && wnd.gridAreaMaskObj != null)
            {
                float canMoveDistance = wnd.gridAreaUIObj.rect.height - wnd.gridAreaMaskObj.rect.height;
                float targetMoveY = wnd.gridAreaUIObj.anchoredPosition.y + wnd.onClickItemMoveUpLength;
                if (targetMoveY > canMoveDistance)
                    targetMoveY = canMoveDistance;
                wnd.gridAreaUIObj.DOAnchorPosY(targetMoveY, 0.25f);
            }
        }
        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}
