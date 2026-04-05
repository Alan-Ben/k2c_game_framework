using System;
using ALPackage;
using CommonEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 玩家经验增加进度条
    /// </summary>
    public class GGUIWndPlayerExpAddSld : _ANPGGUIBasicSubWnd<GGUIMonoPlayerExpAddSld>
    {
        private NPGGUIWndProgress _m_wPlayerExpSld;//玩家经验进度条
        private NPGGUIWndPlayerIcon _m_wPlayerIcon;//玩家头像

        private long _m_lSldChgSerializeId;
        
        public GGUIWndPlayerExpAddSld(GGUIMonoPlayerExpAddSld _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoExpSld != null)
                _m_wPlayerExpSld = new NPGGUIWndProgress(wnd.monoExpSld);
            
            if(wnd.monoPlayerIcon != null)
                _m_wPlayerIcon = new NPGGUIWndPlayerIcon(wnd.monoPlayerIcon);
        }
        
        protected override void _onDiscard()
        {
            _m_wPlayerExpSld?.discard();
            _m_wPlayerExpSld = null;
            
            _m_wPlayerIcon?.discard();
            _m_wPlayerIcon = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wPlayerExpSld?.hideWnd();
            _m_wPlayerIcon?.hideWnd();

            _m_lSldChgSerializeId = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
            _m_wPlayerExpSld?.resetWnd();
            _m_wPlayerIcon?.resetWnd();
        }

        /// <summary>
        /// 增加经验
        /// </summary>
        /// <param name="_addExp"></param>
        public void setAddExp(long _addExp)
        {
            long nowPlayerExp = GCommon.getItemCount(ENPItemType.CURRENCY, (int) ECurrency.P_EXP);//当前玩家经验
            long curLvExp = NPPlayer.instance.playerInfo.curLevelRef?.exp ?? 0;
            long nextLvlExp = NPPlayer.instance.playerInfo.nextLevelRef?.exp ?? 0;
            
            if (_m_wPlayerExpSld != null)
            {
                _m_wPlayerExpSld.showWnd();
                _m_wPlayerExpSld.setProgress(nowPlayerExp - curLvExp, nextLvlExp - curLvExp, EValueFormatType.NORMAL_NOT_LARGE_STR);
            }

            if (wnd != null)
            {
                // 若有获得经验
                if (_addExp > 0)
                {
                    ALUGUICommon.setGameObjEnable(wnd.hasExpAddShowGo, true);
                    ALUGUICommon.setLabelTxt(wnd.txtAddExp,
                        TextTranslate.instance.getLanguage(TransKeyConst.common_propAdd_num, _addExp));
                }
                else
                {
                    ALUGUICommon.setGameObjEnable(wnd.hasExpAddShowGo, false);
                }

                if (nowPlayerExp - curLvExp < _addExp)
                {
                    ALUGUICommon.setGameObjEnable(wnd.onLevelUpShow, true);
                }
                else
                {
                    ALUGUICommon.setGameObjEnable(wnd.onLevelUpShow, false);
                }
            }
            
            _m_wPlayerIcon?.showWnd();
            _m_wPlayerIcon?.setSelfInfo();
        }

        /// <summary>
        /// 
        /// </summary>
        public void setAddExpWithSldChg(long _addExp, Action _showDone = null)
        {
            if(wnd == null)
                return;
            
            long nowPlayerExp = GCommon.getItemCount(ENPItemType.CURRENCY, (int) ECurrency.P_EXP);//当前玩家经验值
            long curLvExp = NPPlayer.instance.playerInfo.curLevelRef?.exp ?? 0;//达到当前等级所需经验值
            long nextLvlExp = NPPlayer.instance.playerInfo.nextLevelRef?.exp ?? 0;//达到下一等级所需经验值
            long preLvlExp = NPPlayer.instance.playerInfo.preLevelRef?.exp ?? 0;//达到上一等级所需经验值
            long preLevelUpNeedExp = curLvExp - preLvlExp;//从上一等级升到本等级需要的总经验值
            long nowLevelUpNeedExp = nextLvlExp - curLvExp;//从本等级升到下一等级需要的总经验值
            
            long nowLevelGetExp = nowPlayerExp - curLvExp;//在当前等级已经获得的经验值
            long preLevelUpAddExp = _addExp - nowLevelGetExp;//在升到本等级之前增加的经验(若大于0说明有等级提升)
            long beforeAddExpPlayerExp = curLvExp - preLevelUpAddExp;//在增加经验之前玩家经验
            
            // 存在升级的情况：
            //                                       |-------------------_addExp----------------|
            //    preLvlExp                beforeAddExpPlayerExp        curLvExp         nowPlayerExp                         nextLvlExp
            //       ↓_______________________________⇩______________________↓__________________⇩______________________________________↓
            //       |------------preLevelUpNeedExp-------------------------|--------------------------nowLevelUpNeedExp--------------|     
            //                                                              |--nowLevelGetExp--|
            //                                       |---preLevelUpAddExp---|
            //                        
            
            
            // 不存在升级的情况：
            //                                                                 |---------_addExp----------|
            //    preLvlExp                         curLvExp          beforeAddExpPlayerExp         nowPlayerExp                 nextLvlExp
            //       ↓__________________________________↓______________________⇩__________________________⇩___________________________↓
            //       |--------preLevelUpNeedExp---------|---------------------------------nowLevelUpNeedExp---------------------------|     
            //                                          |---------------------nowLevelGetExp--------------|
            //                                          |【-preLevelUpAddExp】-|
            //                        
            
            if (wnd.sldChgTime <= 0 || _addExp <= 0)//若进度条变化总时长小于等于0, 或者增加的经验小于等于0
            {
                setAddExp(_addExp);
                _showDone?.Invoke();
                return;
            }
            
            ALUGUICommon.setGameObjEnable(wnd.hasExpAddShowGo, true);
            ALUGUICommon.setLabelTxt(wnd.txtAddExp,
                TextTranslate.instance.getLanguage(TransKeyConst.common_propAdd_num, _addExp));
            
            _m_wPlayerIcon?.showWnd();
            _m_wPlayerIcon?.setSelfInfo();

            // 设置进度条初值
            if (preLevelUpAddExp > 0)//若存在升级情况
            {
                _m_wPlayerExpSld?.setProgress(preLevelUpNeedExp - preLevelUpAddExp, preLevelUpNeedExp, EValueFormatType.NORMAL_NOT_LARGE_STR);
            }
            else//不存在升级情况
            {
                if (-preLevelUpAddExp > nowLevelUpNeedExp)//满级时
                {
                    _m_wPlayerExpSld?.setProgress(1);
                    _m_wPlayerExpSld?.setProgressTxt((-preLevelUpAddExp).ToString(), true);
                }
                else
                {
                    _m_wPlayerExpSld?.setProgress(-preLevelUpAddExp, nowLevelUpNeedExp, EValueFormatType.NORMAL_NOT_LARGE_STR);
                }
            }
            
            long serializeId = _m_lSldChgSerializeId = ALSerializeOpMgr.next();
            Action sldChg = () =>
            {
                if (_m_lSldChgSerializeId != serializeId || wnd == null || !isShow)
                    return;

                if (preLevelUpAddExp > 0) //存在升级情况
                {
                    float preLevelUpSldChgTime = preLevelUpAddExp * 1.0f / _addExp * wnd.sldChgTime;//在升级前进度条变化时间
                    float afterLevelUpSldChgTime = wnd.sldChgTime - preLevelUpSldChgTime;//在升级后进度条变化时间
                    
                    _m_wPlayerExpSld?.setProgressChg(preLevelUpNeedExp, preLevelUpNeedExp, preLevelUpSldChgTime, 
                        EValueFormatType.NORMAL_NOT_LARGE_STR,
                        (_curValue, _totalValue)=> TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, _curValue, _totalValue),
                        () =>
                        {
                            if (_m_lSldChgSerializeId != serializeId || wnd == null || !isShow)
                                return;
                            
                            _m_wPlayerExpSld?.setProgress(0, nowLevelUpNeedExp, EValueFormatType.NORMAL_NOT_LARGE_STR);//进度条满后先重置为0
                            ALUGUICommon.setGameObjEnable(wnd.onLevelUpShow, true);
                            _m_wPlayerExpSld?.setProgressChg(nowLevelGetExp, nowLevelUpNeedExp, afterLevelUpSldChgTime, EValueFormatType.NORMAL_NOT_LARGE_STR,
                                (_curValue, _totalValue)=> TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, _curValue, _totalValue), _showDone);
                        });
                }
                else //不存在升级情况
                {
                    if (-preLevelUpAddExp > nowLevelUpNeedExp) //满级时
                    {
                        ALUGUICommon.setGameObjEnable(wnd.onLevelUpShow, false);

                        _m_wPlayerExpSld?.setProgress(1);
                        _m_wPlayerExpSld?.setProgressTxt(nowLevelGetExp.ToString(), true);
                    }
                    else
                    {
                        ALUGUICommon.setGameObjEnable(wnd.onLevelUpShow, false);

                        _m_wPlayerExpSld?.setProgressChg(nowLevelGetExp, nowLevelUpNeedExp, wnd.sldChgTime, EValueFormatType.NORMAL_NOT_LARGE_STR,
                            (_curValue, _totalValue)=> TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, _curValue, _totalValue)
                            , _showDone);
                    }
                }
            };
                
            ALCommonTaskController.CommonActionAddMonoTask(sldChg, wnd.sldStartChgDelayTime);
        }
    }
}