using Common.GuildObj;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 联盟日志列表
    /// </summary>
    public class GGUIWndGuildLogGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoGuildLogGridItem, GGUIMonoGuildLogGrid, GGUIWndGuildLogGridItem>
    {
        //信息列表
        private List<Guild_LogInfo> _m_lInfoList;
        //日期栏
        private List<GGUIWndGuildLogListBarController> _m_lBarControlerList;

        public GGUIWndGuildLogGrid(GGUIMonoGuildLogGrid _wnd) : base(_wnd)
        {
            _m_lInfoList = new List<Guild_LogInfo>();
            initWnd();
        }

        protected override GGUIWndGuildLogGridItem _createItemWnd(GGUIMonoGuildLogGridItem _itemMono)
        {
            GGUIWndGuildLogGridItem item = new GGUIWndGuildLogGridItem(_itemMono);
            return item;
        }

        protected override void _onRefreshItemWnd(GGUIWndGuildLogGridItem _itemWnd, int _itemIdx)
        {
            if(_itemIdx < 0 || _itemIdx >= _m_lInfoList.Count)
                return;

            _itemWnd?.setInfo(_m_lInfoList[_itemIdx]);
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
            if (_m_lBarControlerList != null)
            {
                for (int i = 0; i < _m_lBarControlerList.Count; i++)
                {
                    if (_m_lBarControlerList[i] != null)
                    {
                        removeBar(_m_lBarControlerList[i]);
                        _m_lBarControlerList[i].discard();
                    }
                }
                _m_lBarControlerList.Clear();
                _m_lBarControlerList = null;
            }
        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_guildLogList"></param>
        public void setShowData(List<Guild_LogInfo> _guildLogList)
        {
            if (_guildLogList == null)
                return;

            _m_lInfoList.Clear();
            _m_lInfoList.AddRange(_guildLogList);
            setItemCount(_m_lInfoList.Count);
            //显示bar
            _showListBar();

            ALUGUICommon.setGameObjEnable(wnd?.goEmptyShowList, _m_lInfoList?.Count <= 0);
        }

        //显示bar
        private void _showListBar()
        {
            if (wnd == null || _m_lInfoList == null || _m_lInfoList.Count <= 0)
                return;

            //先清空bar
            if (_m_lBarControlerList != null)
            {
                for (int i = 0; i < _m_lBarControlerList.Count; i++)
                {
                    if (_m_lBarControlerList[i] != null)
                    {
                        removeBar(_m_lBarControlerList[i]);
                        _m_lBarControlerList[i].discard();
                    }
                }
                _m_lBarControlerList.Clear();
            }
            
            //bar需要插入的位置
            List<int> barIndexList = new List<int>();
            //bar需要的时间戳列表
            List<long> timeMsList = new List<long>();

            string recordTimeTag = "";
            for (int i = 0; i < _m_lInfoList.Count; i++)
            {
                if (_m_lInfoList[i] == null)
                    continue;

                string tempTime = TimeUtil.DateTime2StringYMD(TimeUtil.FromUTCByTimeZone(_m_lInfoList[i].getSendTimeMs()));
                //判断是否是同一天
                if (tempTime != recordTimeTag)
                {
                    recordTimeTag = tempTime;
                    barIndexList.Add(i);
                    timeMsList.Add(_m_lInfoList[i].getSendTimeMs());
                }
            }
            
            if (_m_lBarControlerList == null)
                _m_lBarControlerList = new List<GGUIWndGuildLogListBarController>();
            
            for (int i = 0; i < timeMsList.Count; i++)
            {
                long timeMs = timeMsList[i];
                int barIndex = barIndexList[i];
                GGUIWndGuildLogListBarController barController = new GGUIWndGuildLogListBarController(wnd.gridAreaUIObj);
                addBar(barController);
                barController.regLoadDoneDelegate(() =>
                {
                    barController.setInsertIndex(barIndex);
                    barController.setInfo(timeMs);
                    forceRefreshBar();
                });
                _m_lBarControlerList.Add(barController);
            }
        }
    }
}
