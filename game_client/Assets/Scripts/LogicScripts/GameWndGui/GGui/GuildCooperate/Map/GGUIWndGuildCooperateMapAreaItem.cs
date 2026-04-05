using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 公会协作地图预览区域
    /// </summary>
    public class GGUIWndGuildCooperateMapAreaItem : _ANPGGUIBasicSubWnd<GGUIMonoGuildCooperateMapAreaItem>
    {
        // 区域id
        private GuildCooperateAreaRefObj _m_areaRef;
        // 点击事件
        private Action<long> _m_aOnClickItem;

        public GGUIWndGuildCooperateMapAreaItem(GuildCooperateAreaRefObj _areaRef, Action<long> _onClickItem, GGUIMonoGuildCooperateMapAreaItem _wnd) : base(_wnd)
        {
            _m_areaRef = _areaRef;
            _m_aOnClickItem = _onClickItem;
            initWnd();
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_GUILD_COOPERATE_PROPERTY_POINT_CHG, _onPropertyPointChg);
            _refreshWnd();
        }
        
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_GUILD_COOPERATE_PROPERTY_POINT_CHG, _onPropertyPointChg);
        }
        
        protected override void _onReset()
        {
        }
        
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickItem);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickItem);
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || _m_areaRef == null)
                return;

            wnd.areaStateShow?.setShowData(NPPlayer.instance.guildCooperateComp.getAreaState(_m_areaRef.area_id));
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_areaRef.area_name));
            ALUGUICommon.setLabelTxt(wnd.txtProgress, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, NPPlayer.instance.guildCooperateComp.getAreaAttackProgressPercent(_m_areaRef.area_id)));
        }

        /// <summary>
        /// 点击item
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickItem(GameObject _go)
        {
            if (_m_areaRef == null)
                return;

            _m_aOnClickItem?.Invoke(_m_areaRef.area_id);
        }

        /// <summary>
        /// 属性据点变化
        /// </summary>
        /// <param name="_objects"></param>
        private void _onPropertyPointChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 3)
                return;

            long areaId = (long)_objects[0];
            if (_m_areaRef == null || areaId != _m_areaRef.area_id)
                return;

            _refreshWnd();
        }
    }
}