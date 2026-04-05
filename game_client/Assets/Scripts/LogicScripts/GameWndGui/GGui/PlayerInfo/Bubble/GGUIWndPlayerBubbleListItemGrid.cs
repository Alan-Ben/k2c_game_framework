using System.Collections.Generic;
using NPEnum;


namespace GOE
{
    // 气泡框列表容器
    public class GGUIWndPlayerBubbleListItemGrid : _AGGUIWndPlayerInfoBaseListItemGrid<GGUIMonoPlayerBubbleListItem, GGUIMonoPlayerBubbleListItemGrid, GGUIWndPlayerBubbleListItem>
    {

        public GGUIWndPlayerBubbleListItemGrid(GGUIMonoPlayerBubbleListItemGrid _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onShowWndEx()
        {
            if (null == wnd)
                return;

            refresh();

            WinMsg.RegisterMsgAct(WinMsgType.PLAYER_INFO_BUBBLE_CHG, refresh);
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onPlayerParamChanged);
        }

        protected override void _onHideWndEx()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.PLAYER_INFO_BUBBLE_CHG, refresh);
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onPlayerParamChanged);
        }

        protected override void _onResetEx()
        {
        }

        protected override void _onDiscardEx()
        {
        }

        protected override void _onWndInitDoneEx()
        {
        }

        protected override GGUIWndPlayerBubbleListItem _createItemWnd(GGUIMonoPlayerBubbleListItem _itemMono)
        {
            // 创建对象
            GGUIWndPlayerBubbleListItem gridItem = new GGUIWndPlayerBubbleListItem(_itemMono);

            //设置回调
            gridItem.setClickDelegate(_selectItem);

            return gridItem;
        }

        protected override void _refreshItemwnd(GGUIWndPlayerBubbleListItem _itemWnd, int _itemIdx)
        {
            if (_itemIdx >= itemList.Count)
                return;

            //获取数据对象
            NPPlayerBubbleShowInfo showData = (NPPlayerBubbleShowInfo)itemList[_itemIdx];
            if (null == showData)
                return;

            //刷新物品UI
            bool isSelected = selectId == showData.id;
            _itemWnd.refreshItem(showData, isSelected, _m_hNeedShowRedTipId.Contains(showData.id));
        }

        /// <summary>
        /// 从静态数据获取所有需要展示的数据，并进行展示
        /// </summary>
        /// <returns></returns>
        protected override void _initItemList(List<_APlayerBaseShowInfo> _recList)
        {
            if (null == _recList)
                return;

            _recList.Clear();

            //取出所有静态数据集
            List<NPPlayerBubbleRefObj> refList = GRefdataCoreMgr.instance.playerBubbleCore.refList;

            //当前玩家使用的Id
            long curId = NPPlayer.instance.playerInfo.getCurrentBubbleId();

            NPPlayerBubbleRefObj tmpObj = null;
            for (int i = 0; i < refList.Count; i++)
            {
                tmpObj = refList[i];
                if (null == tmpObj)
                    continue;

                //查询玩家是否有对应数据，有则创建
                NPPlayerBubbleShowInfo showInfo = null;
                NPPlayerBubbleItem item = NPPlayer.instance.bubbleComp.getBubble(tmpObj.id);
                if (null != item)
                {
                    //已有数据则创建
                    showInfo = new NPPlayerBubbleShowInfo(item);
                }
                else
                {
                    //判断是否无数据，无则创建新的
                    if (!tmpObj.disable_cannot_see)
                        showInfo = new NPPlayerBubbleShowInfo(tmpObj);
                }

                if (null == showInfo)
                    continue;

                //设置当前选中对象
                if (tmpObj.id == curId)
                    _setSelectInfo(showInfo);

                //如果超过有效期，设置为已看
                if (showInfo.bubbleItem != null && showInfo.bubbleItem.expiredTimeS > 0 && showInfo.bubbleItem.isExpired)
                    showInfo.bubbleItem.setIsViewed(true);

                //加入队列
                _recList.Add(showInfo);
            }
        }

        //排序规则
        protected override int _getSort(_APlayerBaseShowInfo _x, _APlayerBaseShowInfo _y)
        {

            //比较是否有锁
            int res = _x.isLock.CompareTo(_y.isLock);
            if (res != 0)
                return res;

            //比较是否过期
            res = _x.isExpired.CompareTo(_y.isExpired);
            if (res != 0)
                return res;

            //比较品质
            if (null != ((NPPlayerBubbleShowInfo)_x).baseData && null != ((NPPlayerBubbleShowInfo)_y).baseData)
                res = -((NPPlayerBubbleShowInfo)_x).baseData.quality.CompareTo(((NPPlayerBubbleShowInfo)_y).baseData.quality);
            if (res != 0)
                return res;

            //比较物品id
            res = ((NPPlayerBubbleShowInfo)_x).refObj.id.CompareTo(((NPPlayerBubbleShowInfo)_y).refObj.id);
            return res;
        }

        private void _onPlayerParamChanged(params object[] _objs)
        {
            if (null == _objs || _objs.Length == 0)
                return;

            ENPPlayerParam paramType = (ENPPlayerParam)_objs[0];
            if (paramType == ENPPlayerParam.BUBBLE)
            {
                resetSelectItem();
            }
        }
    }
}
