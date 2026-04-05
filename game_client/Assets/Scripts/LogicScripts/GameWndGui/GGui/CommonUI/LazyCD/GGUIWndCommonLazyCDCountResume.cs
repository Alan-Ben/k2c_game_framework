using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndCommonLazyCDCountResume:_ANPGGUIBasicSubWnd<GGUIMonoCommonLazyCDCountResume>
    {
        private long _m_lazyCDId;
        private int _m_serialize;
        private long _m_lastCount = -1;//-1表示初始化显示
        private CommonUISfxObj _m_sfxObj;
        private bool _m_isSfxPlaying = false;
        private string _m_sCountTransKey;//显示数量的翻译key

        public GGUIWndCommonLazyCDCountResume(GGUIMonoCommonLazyCDCountResume _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
            WinMsg.RegisterMsg(WinMsgType.ON_LAZY_CD_CHG, _onLazyCDChg);
        }

        protected override void _onHideWnd()
        {
            _m_serialize = ALSerializeOpMgr.next();
            _m_lastCount = -1;
            
            if (_m_sfxObj != null) 
                _m_sfxObj.forceDiscard();
            _m_sfxObj = null;
            
            _m_isSfxPlaying = false;
            _m_sCountTransKey = null;
            WinMsg.UnregisterMsg(WinMsgType.ON_LAZY_CD_CHG, _onLazyCDChg);
        }

        protected override void _onReset()
        {
            
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnResume, _onClickResume);
            }
            
            if (_m_sfxObj != null) 
                _m_sfxObj.forceDiscard();
            _m_sfxObj = null;
            _m_isSfxPlaying = false;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnResume, _onClickResume);
        }
        
        private void _onLazyCDChg(object[] _objs)
        {
            if (_objs.Length == 0)
                return;
            long cdId = (long) _objs[0];
            if (cdId != _m_lazyCDId)
                return;
            _refreshEnergyCount();
        }
        
        /// <summary>
        /// 设置显示信息
        /// </summary>
        /// <param name="_lazyCdId"></param>
        public void setInfo(long _lazyCdId, string _countTransKey = null)
        {
            _m_lazyCDId = _lazyCdId;
            _m_sCountTransKey = _countTransKey;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            _m_serialize = ALSerializeOpMgr.next();
            _refreshEnergyCount();
        }


        private void _refreshEnergyCount()
        {
            if(null == wnd)
                return;
            int serialize = _m_serialize;
            //看消耗cd  consort_rand_call_cd
            PlayerLazyCDInfo lazyCdInfo = NPPlayer.instance.lazyCdComp.getLazyCDInfo(_m_lazyCDId);
            if(null == lazyCdInfo)
                return;
            int curCount = lazyCdInfo.getCount();
            if (curCount <= 0)
            {
                ALUGUICommon.setLabelTxt(wnd.txtTimeDown,  TimeUtil.millisecondsToTime_dhms(lazyCdInfo.getRemainMs()));
            }
            
            string curCountStr = curCount.ToString();
            //当前数量是否需要设置颜色
            if (wnd.isSetCurTextColorWhenFull)
            {
                //根据是否满设置当前数量文本颜色
                if (curCount >= lazyCdInfo.MaxCount)
                    curCountStr = GCommon.addColorForRichText(curCountStr, wnd.curTextFullColor);
                else
                    curCountStr = GCommon.addColorForRichText(curCountStr, wnd.curTextNotFullColor);
            }
            
            ALUGUICommon.setLabelTxt(wnd.txtCount,   TextTranslate.instance.getLanguage(string.IsNullOrEmpty(_m_sCountTransKey) ? TransKeyConst.common_currentTotalNum_num_num : _m_sCountTransKey, curCountStr, lazyCdInfo.MaxCount));
            ALUGUICommon.setGameObjEnable(wnd.goListCDingShow,curCount <= 0);
            ALUGUICommon.setGameObjEnable(wnd.goListCDingHide,curCount > 0);
            ALUGUICommon.setGameObjEnable(wnd.goListMaxShow,curCount >= lazyCdInfo.MaxCount);

            if (_m_lastCount != -1 && _m_lastCount < curCount && !_m_isSfxPlaying)//非初始记录，说明数值有变化，播放恢复特效
            {
                _m_isSfxPlaying = true;
                _m_sfxObj = PlaySfxMgr.instance.playUISfx(wnd.addCountSfxId, wnd.sfxParent);
                if (_m_sfxObj != null)
                    _m_sfxObj.regPlayCompleteDelegate(() => { _m_isSfxPlaying = false; });
                else
                {
                    _m_isSfxPlaying = false;
                }
            }
            _m_lastCount = curCount;
            if (curCount < lazyCdInfo.MaxCount)
            {
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    if (serialize != _m_serialize)
                        return;
                    _refreshEnergyCount();
                },1f);
            }
        }
        
        /// <summary>
        /// 恢复按钮
        /// </summary>
        /// <param name="_gameObject"></param>
        private void _onClickResume(GameObject _gameObject)
        {
            GCommon.popItemAccessWays(ENPItemType.LAZY_CD, _m_lazyCDId);
        }
    }
}