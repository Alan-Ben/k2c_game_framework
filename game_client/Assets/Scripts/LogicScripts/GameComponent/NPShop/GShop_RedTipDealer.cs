using ALPackage;

namespace GOE
{
    public partial class NPPlayerShopComponent
    {
        private class RedTipDealer
        {
            //商店组件
            private NPPlayerShopComponent _m_component;
            //刷新序列号
            private int _m_refreshSerialize = 0;
            //下次刷新时间戳
            private long _m_nextRefreshTimeMs = 0;

            public RedTipDealer(NPPlayerShopComponent _component)
            {
                _m_component = _component;
            }

            /// <summary>
            /// 初始化红点
            /// </summary>
            public void init()
            {
                refreshAllRed();
            }

            /// <summary>
            /// 清除数据
            /// </summary>
            public void clear()
            {
                _m_refreshSerialize = ALSerializeOpMgr.next();
            }

            /// <summary>
            /// 刷新红点
            /// </summary>
            public void refreshAllRed()
            {
                _m_refreshSerialize = ALSerializeOpMgr.next();
                _refreshAllRed();
            }

            /// <summary>
            /// 玩家信息详细红点
            /// </summary>
            private void _refreshAllRed()
            {
                //最小刷新时间戳
                long miniNextRefreshTimeMs = 0;

                //刷新所有红点
                for (int i = 0; i < _m_component._m_shopList.Count; i++)
                {
                    long tempTimeMs = refrshRedTip(_m_component._m_shopList[i], false);

                    //获取最小的下次刷新时间戳
                    if (tempTimeMs > 0 && (miniNextRefreshTimeMs == 0 || tempTimeMs < miniNextRefreshTimeMs))
                        miniNextRefreshTimeMs = tempTimeMs;
                }

                //添加下次刷新任务
                if (_m_nextRefreshTimeMs == 0 || _m_nextRefreshTimeMs > miniNextRefreshTimeMs)
                {
                    _m_nextRefreshTimeMs = miniNextRefreshTimeMs;
                    _addNextRefreshTask();
                }
            }

            /// <summary>
            /// 刷新单个商店红点
            /// </summary>
            /// <param name="_info"></param>
            /// <param name="_needAddNextRefreshTask"></param>
            public long refrshRedTip(NPPlayerShop _info, bool _needAddNextRefreshTask = true)
            {
                if (null == _info || _info.shopRefObj == null)
                    return 0;

                long curNextRefreshTimeMs = 0;
                if (_info.nextRefreshTimeMs > 0)
                {
                    //下次刷新时间戳
                    curNextRefreshTimeMs = _info.nextRefreshTimeMs;
                    //上次展示时间
                    long lastShowTimeS = _m_component.remarkInfo.getShopShowTimeS(_info.shopRefId);

                    if (lastShowTimeS <= 0)//初始化玩家，还没看过
                    {
                        RedTipMgr.instance.setCountByRefRedTipId(_info.shopRefObj.red_tip_id, 1);
                        _m_component.setShopNextRefreshTimeMs(_info.shopRefId, _info.nextRefreshTimeMs);
                    }//当前时间已经大于下次刷新时间，或者下次刷新时间跟记录的不同，说明需要刷新或者已经刷新需要显示红点
                    else if (FpsAndPingMgr.instance.serverTimeTag >= _info.nextRefreshTimeMs || _info.nextRefreshTimeMs != _m_component.remarkInfo.getShopNextRefreshTimeMs(_info.shopRefId))
                    {
                        RedTipMgr.instance.setCountByRefRedTipId(_info.shopRefObj.red_tip_id, 1);
                        _m_component.setShopNextRefreshTimeMs(_info.shopRefId, _info.nextRefreshTimeMs);
                    }
                    else
                    {
                        RedTipMgr.instance.setCountByRefRedTipId(_info.shopRefObj.red_tip_id, 0);

                        //需要设置下次刷新任务，并且下次刷新时间小于记录的下次刷新时间，则更新下次刷新时间并重新设置任务
                        if (_needAddNextRefreshTask && (curNextRefreshTimeMs < _m_nextRefreshTimeMs || _m_nextRefreshTimeMs == 0))
                        {
                            _m_nextRefreshTimeMs = curNextRefreshTimeMs;
                            _addNextRefreshTask();
                        }
                    }

                    WinMsg.SendMsg(WinMsgType.ON_SHOP_RED_TIP_CHG, _info.shopRefId);
                }
                else
                    RedTipMgr.instance.setCountByRefRedTipId(_info.shopRefObj.red_tip_id, 0);

                return curNextRefreshTimeMs;
            }

            //添加下次刷新任务
            private void _addNextRefreshTask()
            {
                if (_m_nextRefreshTimeMs > 0)
                {
                    long leftTimeMs = _m_nextRefreshTimeMs - FpsAndPingMgr.instance.serverTimeTag;
                    if (leftTimeMs < 0)
                        return;

                    _m_refreshSerialize = ALSerializeOpMgr.next();
                    int refreshSerialize = _m_refreshSerialize;
                    ALCommonTaskController.CommonActionAddMonoTask(() =>
                    {
                        if (refreshSerialize != _m_refreshSerialize)
                            return;

                        _refreshAllRed();
                    }, (float)(leftTimeMs * 1.0f / 1000 + 0.5f));//多0.5秒保证当前时间比商店记录的下次刷新时间大
                }
            }
        }
    }
}