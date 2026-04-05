using ALPackage;
using Common.GuildCooperateObj;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 公会协作日志列表
    /// </summary>
    public class GGUIWndGuildCooperateLogGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoGuildCooperateLogGridItem, GGUIMonoGuildCooperateLogGrid, GGUIWndGuildCooperateLogGridItem>
    {
        //总的数据列表
        [NotNull]
        private List<GuildCooperate_AttackLog> _m_lInfoList = new List<GuildCooperate_AttackLog>();
        //是否正在请求过数据
        private bool _m_bIsRequestingData;
        //显示操作序列号
        private long _m_lShowSerialize;
        //最后一个dbId
        private long _m_lLastDbId;

        public GGUIWndGuildCooperateLogGrid(GGUIMonoGuildCooperateLogGrid _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_lLastDbId = 0;
            ALUGUICommon.setGameObjEnable(wnd?.goRequestingShow, false);
            _reqData();
            wnd?.scrollRect?.onValueChanged?.AddListener(_onScrollChg);
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_bIsRequestingData = false;
            _m_lLastDbId = 0;
            _m_lInfoList.Clear();
            wnd?.scrollRect?.onValueChanged?.RemoveAllListeners();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            if (_m_lInfoList != null)
                _m_lInfoList.Clear();
        }

        protected override void _onWndInitDone()
        {
            // 初始化容器
            setItemCount(0);
        }

        // 创建对象
        protected override GGUIWndGuildCooperateLogGridItem _createItemWnd(GGUIMonoGuildCooperateLogGridItem _itemMono)
        {
            // 创建对象
            GGUIWndGuildCooperateLogGridItem gridItem = new GGUIWndGuildCooperateLogGridItem(_itemMono);
            return gridItem;
        }

        // 刷新对象
        protected override void _onRefreshItemWnd(GGUIWndGuildCooperateLogGridItem _itemMono, int _itemIdx)
        {
            if (_m_lInfoList.Count <= _itemIdx)
                return;

            GuildCooperate_AttackLog logInfo = _m_lInfoList[_itemIdx];
            _itemMono?.setInfo(logInfo);
        }

        /// <summary>
        /// 请求数据
        /// </summary>
        private void _reqData()
        {
            if (wnd == null || _m_bIsRequestingData)
                return;

            long curSerialize = _m_lShowSerialize;
            _m_bIsRequestingData = true;
            ALUGUICommon.setGameObjEnable(wnd.goRequestingShow, true);
            NPPlayer.instance.guildCooperateComp.reqGuildCooperateAttackLogList(_m_lLastDbId, 50, (_isSuc, _msg) =>
            {
                if (curSerialize != _m_lShowSerialize || _msg == null)
                    return;

                _m_bIsRequestingData = false;
                ALUGUICommon.setGameObjEnable(wnd?.goRequestingShow, false);
                List<GuildCooperate_AttackLog> logList = _msg.getAttackLogList();
                if (logList == null)
                    return;

                logList.Sort((_a,_b)=> -(_a.getDbId().CompareTo(_b.getDbId())));
                if(logList.Count > 0)
                    _m_lLastDbId = logList.GetLast().getDbId();
                addInfo(logList);
            });
        }

        /// <summary>
        /// 初始化列表
        /// </summary>
        /// <param name="_infoList"></param>
        public void addInfo(List<GuildCooperate_AttackLog> _infoList)
        {
            if (wnd == null || _infoList == null)
                return;

            _m_lInfoList.AddRange(_infoList);

            //刷新grid
            setItemCount(_m_lInfoList.Count);
            //空物品提示
            ALUGUICommon.setUIObjScale(wnd.noneItemsTips, _m_lInfoList.Count <= 0 ? 1 : 0);
        }

        /// <summary>
        /// 列表滚动
        /// </summary>
        /// <param name="_vector2"></param>
        private void _onScrollChg(Vector2 _vector2)
        {
            if (wnd == null || wnd.scrollRect == null || _m_bIsRequestingData)
                return;

            if (wnd.scrollRect.verticalNormalizedPosition <= 0.05f)
                _reqData();
        }
    }
}
