using System;
using ALPackage;
using Common.GuildDungeonEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟副本升级成功
    /// </summary>
    public class GGUIWndGuildDungeonUpgradeSuc : _ATALBasicUIWnd<GGUIMonoGuildDungeonUpgradeSuc>
    {
        private static GGUIWndGuildDungeonUpgradeSuc _g_instance = new GGUIWndGuildDungeonUpgradeSuc();
    
        public static GGUIWndGuildDungeonUpgradeSuc instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildDungeonUpgradeSuc();
                return _g_instance;
            }
        }
        
        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>
        private Action _m_setDealerDone;
        private long _m_oldLvl = 0; // 升级前等级
        private long _m_newLvl = 0; // 升级后等级
        private GuildDungeonRefObj _m_dungeonRefObj; // 联盟副本参考对象
        private NPGGuiWndTexture _m_dungeonIconWnd;
        
        public GGUIWndGuildDungeonUpgradeSuc() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoGuildDungeonUpgradeSuc.assetPath; }
        protected override string _monoObjName { get => GGUIMonoGuildDungeonUpgradeSuc.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_dungeonIconWnd?.hideWnd();
        }
    
        protected override void _onReset()
        {
            _m_dungeonIconWnd?.discardTexture();
        }
    
        protected override void _onDiscard()
        {
            _m_dungeonIconWnd?.discard();
            _m_dungeonIconWnd = null;
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickbtnClose);
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (wnd.dungeonIcon != null)
            {
                _m_dungeonIconWnd = new NPGGuiWndTexture(wnd.dungeonIcon);
            }
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickbtnClose);
        }

        private void _onClickbtnClose(GameObject _obj)
        {
            _m_setDealerDone?.Invoke();
        }

        public void setInfo(GuildDungeonRefObj _dungeonRefObj, long _oldLevel, long _newLevel, Action _setDealerDone)
        {
            _m_dungeonRefObj = _dungeonRefObj;
            _m_oldLvl = _oldLevel;
            _m_newLvl = _newLevel;
            _m_setDealerDone = _setDealerDone;
            _refreshWnd();
        }
        
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
                
            if(_m_dungeonRefObj == null)
                return;
            if (_m_dungeonIconWnd != null)
            {
                _m_dungeonIconWnd.showWnd();
                _m_dungeonIconWnd.setTexture(_m_dungeonRefObj.icon);
            }
            ALUGUICommon.setLabelTxt(wnd.txtDungeonName, TextTranslate.instance.getLanguage(_m_dungeonRefObj.name));
            // <AutoGen:_refreshWnd>
            // </AutoGen:_refreshWnd>
            ALUGUICommon.setLabelTxt(wnd.txtOldLevel, TextTranslate.instance.getLanguage(TransKeyConst.guild_dungeon_level_name_num, _m_oldLvl));
            ALUGUICommon.setLabelTxt(wnd.txtNewLevel, TextTranslate.instance.getLanguage(TransKeyConst.guild_dungeon_level_name_num, _m_newLvl));
            var oldLevelRef = _m_dungeonRefObj.getLvlRef(_m_oldLvl);
            var newLevelRef = _m_dungeonRefObj.getLvlRef(_m_newLvl);
            if (oldLevelRef != null)
                ALUGUICommon.setLabelTxt(wnd.txtOldBossBlood, oldLevelRef.getMonsterHp(EGuildDungeon_MonsterType.BOSS));
            if (newLevelRef != null)
                ALUGUICommon.setLabelTxt(wnd.txtNewBossBlood, newLevelRef.getMonsterHp(EGuildDungeon_MonsterType.BOSS));
            ALUGUICommon.setLabelTxt(wnd.txtOldTotalBlood, _m_dungeonRefObj.getTotalMonsterHp(_m_oldLvl).ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            ALUGUICommon.setLabelTxt(wnd.txtNewTotalBlood, _m_dungeonRefObj.getTotalMonsterHp(_m_newLvl).ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
        }
        
        // <AutoGen:Method>
        // </AutoGen:Method>
    }
}