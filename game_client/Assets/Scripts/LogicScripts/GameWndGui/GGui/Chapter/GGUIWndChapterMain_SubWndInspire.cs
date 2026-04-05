using System;
using ALPackage;
using NPEnum;
using System.Collections.Generic;
using System.Linq;
using Common.ChapterEnum;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GOE
{
    /// <summary>
    /// 关卡界面鼓舞子窗口
    /// </summary>
    public class GGUIWndChapterMain_SubWndInspire : _ATALBasicUISubWnd<GGUIMonoChapterMain_SubWndInspire>
    {
        public event Action onClickBattle;
        
        private NPGGUIWndPlayerIcon _m_playerIcon;
        private ChapterRefObj _m_chapterRef;
        
        //金币鼓舞消耗显示
        private NPGGUIWndCommonItem _m_costGoldInspire;
        //道具鼓舞消耗显示
        private NPGGUIWndCommonItem _m_costItemInspire;
        //boos头像
        private NPGGuiWndTexture _m_iconWnd;
        private CommonUISfxObj _m_oSfxObj;//特效物体
        
        public GGUIWndChapterMain_SubWndInspire(GGUIMonoChapterMain_SubWndInspire _wnd) : base(_wnd)
        {
            initWnd();
        }
        
        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {
           
            _discardSfx();
        }

        protected override void _onReset()
        {
            _m_playerIcon?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (null == wnd)
                return;
            _m_playerIcon?.discard();
            _m_playerIcon = null;
            
            if (null != _m_costGoldInspire)
                _m_costGoldInspire.discard();
            _m_costGoldInspire = null;
            if (null != _m_costItemInspire)
                _m_costItemInspire.discard();
            _m_costItemInspire = null;
            if (null != _m_iconWnd)
                _m_iconWnd.discard();
            _m_iconWnd = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnInspireGold, _onClickBtnInspireGold);
            ALUGUICommon.uncombineBtnClick(wnd.btnInspireItem, _onClickBtnInspireItem);
            ALUGUICommon.uncombineBtnClick(wnd.btnStartBattle, _onClickBattle);
            ALUGUICommon.uncombineBtnClick(wnd.btnPowerUp, _onClickPowerUp);
            _discardSfx();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            if (null != wnd.playerInfo)
            {
                _m_playerIcon = new NPGGUIWndPlayerIcon(wnd.playerInfo);
            }
            if (null != wnd.imgBossIcon)
                _m_iconWnd = new NPGGuiWndTexture(wnd.imgBossIcon);
            
            if (null != wnd.costItemInspireGold)
                _m_costGoldInspire = new NPGGUIWndCommonItem(wnd.costItemInspireGold);
            if (null != wnd.costItemInspireItem)
                _m_costItemInspire = new NPGGUIWndCommonItem(wnd.costItemInspireItem);
            
            ALUGUICommon.combineBtnClick(wnd.btnInspireGold, _onClickBtnInspireGold);
            ALUGUICommon.combineBtnClick(wnd.btnInspireItem, _onClickBtnInspireItem);
            ALUGUICommon.combineBtnClick(wnd.btnStartBattle, _onClickBattle);
            ALUGUICommon.combineBtnClick(wnd.btnPowerUp, _onClickPowerUp);
        }
        
        public void setInfo(ChapterRefObj _chapterRef)
        {
            if (null == _chapterRef)
                return;
            _m_chapterRef = _chapterRef;
            
            //刷新界面基础信息
            _refreshWnd();
        }
        
        private void _refreshWnd()
        {
            if(null == wnd || null == _m_chapterRef)
                return;
            
            if (_m_playerIcon != null)
            {
                _m_playerIcon.setSelfInfo();
                _m_playerIcon.showWnd();
            }
            
            if (_m_costGoldInspire != null)
            {
                _m_costGoldInspire.setItem(NPPlayer.instance.chapterComp.getCurrentInspireCost());
                _m_costGoldInspire.showWnd();
            }
            if (_m_costItemInspire != null)
            {
                _m_costItemInspire.setItem(GRefdataCoreMgr.instance.npGeneral.item_inspire_cost);
                _m_costItemInspire.showWnd();
            }

            ALUGUICommon.setLabelTxt(wnd.txtSelfPower, NPPlayer.instance.chapterComp.getTotalPower().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));

            long inspirePower = NPPlayer.instance.chapterComp.getInspirePower();
            if (inspirePower > 0)
            {
                ALUGUICommon.setLabelTxt(wnd.txtSelfPowerInspire, $"+{inspirePower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)}");
                ALUGUICommon.setGameObjEnable(wnd.goInspireShow, true);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.goInspireShow, false);
            }
            
            ALUGUICommon.setLabelTxt(wnd.txtBossPower, _m_chapterRef.boss_power.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            
            //boss相关
            ChapterBossStyleRefObj chapterBossStyleRefObj = _m_chapterRef.getBossStyleRefObj();
            if (null != chapterBossStyleRefObj)
            {
                if (null != _m_iconWnd)
                {
                    _m_iconWnd.setTexture(chapterBossStyleRefObj.boss_icon);
                    _m_iconWnd.showWnd();
                }   
                ALUGUICommon.setLabelTxt(wnd.txtBossName, TextTranslate.instance.getLanguage(chapterBossStyleRefObj.boss_name));
            }
            
            //展示鼓舞次数
            ALUGUICommon.setLabelTxt(wnd.goldInspireCount, NPPlayer.instance.chapterComp.getInspireTimes(EChapterInspireType.GOLD));
            ALUGUICommon.setLabelTxt(wnd.itemInspireCount, NPPlayer.instance.chapterComp.getInspireTimes(EChapterInspireType.ITEM));

            //如果战斗力够了
            if (curCanFightBoss())
            {
                ALUGUICommon.setUIObjColor(wnd.txtSelfPower, wnd.colorNormalPower);
                ALUGUICommon.setGameObjEnable(wnd.goPassBossShow, true);
                ALUGUICommon.setGameObjEnable(wnd.goPassBossHide, false);
            }
            else
            {
                ALUGUICommon.setUIObjColor(wnd.txtSelfPower, wnd.colorUnEnoughPower);
                ALUGUICommon.setGameObjEnable(wnd.goPassBossShow, false);
                ALUGUICommon.setGameObjEnable(wnd.goPassBossHide, true);
            }
        }
        
        //点击金币鼓舞
        private void _onClickBtnInspireGold(GameObject _obj)
        {
            dealInspireGold(() =>
            {
                if (wnd != null) 
                    _playSfx(wnd.sfxGoldInspireId);
            }, null);
        }
        
        //点击道具鼓舞
        private void _onClickBtnInspireItem(GameObject _obj)
        {
            dealInspireItem(() =>
            {
                if (wnd != null) 
                    _playSfx(wnd.sfxItemInspireId);
            }, null);
        }

        //处理金币鼓舞逻辑
        public void dealInspireGold(Action _suc, Action _fail)
        {
            if (null == wnd)
                return;
            
            if(null == _m_chapterRef)
                return;
            
            if (NPPlayer.instance.chapterComp.getTotalPower() >= _m_chapterRef.boss_power)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.chapter_power_pass_boss);
                return;
            }

            if (!GCommon.isItemEnough(NPPlayer.instance.chapterComp.getCurrentInspireCost(), true))
            {
                return;
            }
            
            NPPlayer.instance.chapterComp.reqChapterFightBossInspire(EChapterInspireType.GOLD, () =>
            {
                //刷新窗口
                _refreshWnd();
                
                _suc?.Invoke();
            }, () =>
            {
                _fail?.Invoke();
            }, true);
        }
    
        //处理道具鼓舞逻辑
        public void dealInspireItem(Action _suc, Action _fail)
        {
            if (null == wnd)
                return;
            
            if(null == _m_chapterRef)
                return;
            
            if (NPPlayer.instance.chapterComp.getTotalPower() >= _m_chapterRef.boss_power)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.chapter_power_pass_boss);
                return;
            }
            
            if (!GCommon.isItemEnough(GRefdataCoreMgr.instance.npGeneral.item_inspire_cost, true))
            {
                return;
            }
            
            NPPlayer.instance.chapterComp.reqChapterFightBossInspire(EChapterInspireType.ITEM, () =>
            {
                //刷新窗口
                _refreshWnd();
                
                _suc?.Invoke();
            }, () =>
            {
                _fail?.Invoke();
            }, true);
        }
        
        
        //当前战斗力能不能打boss了
        public bool curCanFightBoss()
        {
            if (null == wnd)
                return false;
            
            if(null == _m_chapterRef)
                return false;
            
            //boss战斗力
            long bossPower = _m_chapterRef.boss_power;
            long curPower = NPPlayer.instance.chapterComp.getTotalPower();

            if (curPower < bossPower)
                return false;

            return true;
        }

        private void _onClickBattle(GameObject _obj)
        {
            if (null == wnd)
                return;

            if (null == _m_chapterRef)
                return;

            //如果战斗力够了
            if (curCanFightBoss())
            {
               if (null != onClickBattle)
                   onClickBattle();
            }
            else
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.chapter_click_power_less_than_boss);
            }
        }
        
        private void _onClickPowerUp(GameObject _obj)
        {
            if (null == wnd)
                return;

            if (null == _m_chapterRef)
                return;
            
            //打开界面
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndImproveWay.instance, () =>
            {
                GGUIWndImproveWay.instance.showWnd();
                GGUIWndImproveWay.instance.setInfo(EImproveTargetType.HERO_POWER);
            }, UINodeTagConst.C_IMPROVE_WAY);        
        }
        
        /// <summary>
        /// 播放特效
        /// </summary>
        private void _playSfx(long _sfxId)
        {
            if (wnd == null || wnd.sfxParent == null)
                return;

            _discardSfx();
            
            _m_oSfxObj = PlaySfxMgr.instance.playUISfx(_sfxId, wnd.sfxParent);
        }

        /// <summary>
        /// 销毁特效
        /// </summary>
        private void _discardSfx()
        {
            _m_oSfxObj?.forceDiscard();
            _m_oSfxObj = null;
        }
        
    }
}