using System;
using System.Collections.Generic;
using ALPackage;
using Common.GuildDungeonEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟PVE自动开启
    /// </summary>
    public class GGUIWndGuildDungeonAutoOpenGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoGuildDungeonAutoOpenGridItem>
    {
        
        // <AutoGen:WndDeclaration>
        private NPGGuiWndTexture _m_icoBossWnd; // boss图标
        private GGUIWndLongProgress _m_bossBloodWnd;  // boss血量
        private NPGGUIWndCommonToggleEx _m_toggleOpenWnd;  // 勾选自动开启
        // </AutoGen:WndDeclaration>
        
        private GuildDungeonInfo _m_data; // 数据
        private bool _m_isAutoOpen; // 是否自动开启
        private Action<GuildDungeonInfo> _m_onItemClick; // 点击事件
        
        public GGUIWndGuildDungeonAutoOpenGridItem(GGUIMonoGuildDungeonAutoOpenGridItem _wnd, Action<GuildDungeonInfo> _onItemClick) : base(_wnd)
        {
            _m_onItemClick = _onItemClick;
            initWnd();
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
            _m_icoBossWnd?.hideWnd();
            _m_bossBloodWnd?.hideWnd();
        }
    
        protected override void _onReset()
        {
            _m_icoBossWnd?.hideWnd();
            _m_bossBloodWnd?.resetWnd();
        }
    
        protected override void _onDiscard()
        {
            _m_icoBossWnd?.discard();
            _m_icoBossWnd = null;
            _m_bossBloodWnd?.discard();
            _m_bossBloodWnd = null;
            _m_toggleOpenWnd?.discard();
            _m_toggleOpenWnd = null;
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (wnd.icoBoss != null)
                _m_icoBossWnd = new NPGGuiWndTexture(wnd.icoBoss);
            if (wnd.bossBlood != null)
                _m_bossBloodWnd = new GGUIWndLongProgress(wnd.bossBlood);
            if (wnd.toggleOpen != null)
                _m_toggleOpenWnd = new NPGGUIWndCommonToggleEx(wnd.toggleOpen);
            _m_toggleOpenWnd.clickDelegate += _onToggleClicktoggleOpen;
        }

        protected override void _resetGridItem()
        {
            
            
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public void setInfo(GuildDungeonInfo _data, bool _isAutoOpen)
        {
            _m_data = _data;
            updateToggleState(_isAutoOpen);
            _refreshWnd();
        }
        
        public void updateToggleState(bool _isAutoOpen)
        {
            _m_isAutoOpen = _isAutoOpen;
            if (_m_toggleOpenWnd != null)
            {
                _m_toggleOpenWnd.setSelected(_m_isAutoOpen);
            }
        }

        /// <summary>
        /// 刷新界面显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            if(null == _m_data)
                return;
            if(_m_icoBossWnd != null)
            {
                _m_icoBossWnd.setTexture(_m_data.dungeonRefObj?.icon);
                _m_icoBossWnd.showWnd();
            }
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_data.dungeonRefObj?.name));
            ALUGUICommon.setLabelTxt(wnd.txtOpenCost, _m_data.starCostGuildWealth);
            ALUGUICommon.setLabelTxt(wnd.txtLockCondition, TextTranslate.instance.getLanguage(TransKeyConst.guild_dungeon_unlockCondition_num, _m_data.dungeonRefObj?.unlock_need_guild_lvl));
            if(_m_bossBloodWnd != null)
            {
                string _getCommonSliderTxtStr(string _cur, string _max)
                {
                    return TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, _cur, _max);
                }
                _m_bossBloodWnd.showWnd();
                long bossMaxHp = _m_data.lvlRefObj?.getMonsterHp(EGuildDungeon_MonsterType.BOSS) ?? 0;
                _m_bossBloodWnd.initSld(0, bossMaxHp, _getCommonSliderTxtStr, EValueFormatType.NORMAL);
                _m_bossBloodWnd.setNowValue(bossMaxHp);
            }

            bool isLock = _m_data != null && _m_data.state == EGuildDungeonState.Lock;
            ALUGUICommon.setGameObjEnable(wnd.lockShowGos, isLock);
            ALUGUICommon.setGameObjEnable(wnd.lockHideGos, !isLock);
        }
        
        // <AutoGen:Method>
        
        private void _onToggleClicktoggleOpen(NPGGUIWndCommonToggleEx _toggleWnd)
        {
            if(_m_data != null && _m_data.state == EGuildDungeonState.Lock)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.guild_dungeon_lock_tip));
                return;
            }
            _m_onItemClick?.Invoke(_m_data);
        }
        // </AutoGen:Method>
    }
}
