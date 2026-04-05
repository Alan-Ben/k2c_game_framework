using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 已解锁妃子详情页子信息窗口
    /// </summary>
    public class GGUISubWndUnlockConsortDetailInfo : _ATALBasicUISubWnd<GGUISubMonoUnlockConsortDetailInfo>
    {
        private GGottenConsortInfo _m_consortInfo; //妃子展示信息

        public GGUISubWndConsortFetterInfo _m_wndConsortFetterInfo;//羁绊信息子窗口

        private NPGGuiWndTexture _m_wConsortRawImg;//妃子半身像
        private NPGGUIWndCommonShowCase _m_wndCommonShowCase; //展示窗口

        //加载出来的品质GO序号
        private NPGGoIndex _m_qualityGoIndex;
        //加载出来的品质GO
        private GameObject _m_qualityGo;
        
        public GGUISubWndUnlockConsortDetailInfo(GGUISubMonoUnlockConsortDetailInfo _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.consortRawImage != null)
                _m_wConsortRawImg = new NPGGuiWndTexture(wnd.consortRawImage);
            
            if (wnd.monoShowcase != null)
                _m_wndCommonShowCase = new NPGGUIWndCommonShowCase(wnd.monoShowcase);
            
            if (wnd.monoFetterInfo != null)
                _m_wndConsortFetterInfo = new GGUISubWndConsortFetterInfo(wnd.monoFetterInfo);
            
            ALUGUICommon.combineBtnClick(wnd.btnConsortProfile, _onConsortProfileBtnClick);
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnConsortProfile, _onConsortProfileBtnClick);
            }
            
            _m_wndConsortFetterInfo?.discard();
            _m_wndConsortFetterInfo = null;
            
            _m_wConsortRawImg?.discard();
            _m_wConsortRawImg = null;
            
            _m_wndCommonShowCase?.discard();
            _m_wndCommonShowCase = null;
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_INTIMACY_CHG, _onConsortIntimacyChg);
            WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_CHARM_CHG, _onConsortCharmChg);
            WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_FETTER_LEVEL_CHG, _onConsortFetterLvlChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_CONSORT_INTIMACY_CHG, _onConsortIntimacyChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_CONSORT_CHARM_CHG, _onConsortCharmChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_CONSORT_FETTER_LEVEL_CHG, _onConsortFetterLvlChg);

            _pushBackQualityGo();

            _m_wConsortRawImg?.hideWnd();
            _m_wndConsortFetterInfo?.hideWnd();
            _m_wndCommonShowCase?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wConsortRawImg?.discardTexture();
            _m_wndConsortFetterInfo?.resetWnd();
            _m_wndCommonShowCase?.resetWnd();
        }

        public void setData(GGottenConsortInfo _consortInfo)
        {
            _m_consortInfo = _consortInfo;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (wnd == null || _m_consortInfo == null || _m_consortInfo.consortRefObj == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtConsortName, _m_consortInfo.consortTransName);
            ALUGUICommon.setLabelTxt(wnd.txtConsortTitle, _m_consortInfo.consortTransTitle);
            ALUGUICommon.setLabelTxt(wnd.txtConsortBirthPlace, TextTranslate.instance.getLanguage(_m_consortInfo.consortRefObj.birthplace));
            ALUGUICommon.setLabelTxt(wnd.txtDesc, _m_consortInfo.consortRefObj.transDesc);

            //品质图标GO
            if (wnd.goQualityIconParent != null)
            {
                _pushBackQualityGo();
                _popQualityGo();
            }
            
            long curSkinId = _m_consortInfo.consortSkinShowInfo == null
                ? _m_consortInfo.consortRefObj.default_skin_id
                : _m_consortInfo.consortSkinShowInfo.skinId;
            ALUGUICommon.setLabelTxt(wnd.txtConsortSkinName, GCommon.getItemName(ENPItemType.CONSORT_SKIN, curSkinId));

            if (curSkinId == _m_consortInfo.consortRefObj.default_skin_id) //默认皮肤
            {
                ALUGUICommon.setGameObjEnable(wnd.onWearingDefaultSkinShow, true);
                ALUGUICommon.setGameObjEnable(wnd.onWearingDefaultSkinHide, false);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.onWearingDefaultSkinShow, false);
                ALUGUICommon.setGameObjEnable(wnd.onWearingDefaultSkinHide, true);
            }

            if(_m_wConsortRawImg != null)
            {
                _m_wConsortRawImg.showWnd();
                _m_wConsortRawImg.setTexture(_m_consortInfo.consortSkinShowInfo?.consortCardImage);
            }
            
            if (_m_wndCommonShowCase != null)
            {
                int maxShowCaseUnitCount = Mathf.Max(wnd.consortActorInShowCaseIndex, wnd.bgInShowCaseIndex) + 1;
                if (maxShowCaseUnitCount > 0)
                {
                    _AShowCaseUnitInfoObj[] showCaseUnitInfoObjList = new _AShowCaseUnitInfoObj[maxShowCaseUnitCount];
                
                    if(wnd.consortActorInShowCaseIndex >= 0)
                        showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(_m_consortInfo.consortSkinShowInfo?.tdShow), wnd.consortActorInShowCaseIndex);
                
                    if(wnd.bgInShowCaseIndex >= 0)
                        showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(_m_consortInfo.consortSkinShowInfo?.tdBgIndex), wnd.bgInShowCaseIndex);
                
                    _m_wndCommonShowCase.showWnd(showCaseUnitInfoObjList);
                }
                else
                {
                    _m_wndCommonShowCase.hideWnd();
                }
            }

            _refreshIntimacy();
            _refreshCharm();

            ALUGUICommon.setLabelTxt(wnd.txtConsortSource,
                GCommon.getItemSource(ENPItemType.CONSORT, _m_consortInfo.consortId));

            _refreshFetterLvl();
        }
        
        /// <summary>
        /// 刷新亲密度
        /// </summary>
        private void _refreshIntimacy()
        {
            if (wnd == null || _m_consortInfo == null)
                return;

            if (string.IsNullOrEmpty(wnd.intimacyTransKey))
                ALUGUICommon.setLabelTxt(wnd.txtIntimacy, _m_consortInfo.intimacy);
            else
                ALUGUICommon.setLabelTxt(wnd.txtIntimacy,
                    TextTranslate.instance.getLanguage(wnd.intimacyTransKey, _m_consortInfo.intimacy));
        }

        /// <summary>
        /// 刷新魅力值
        /// </summary>
        private void _refreshCharm()
        {
            if (wnd == null || _m_consortInfo == null)
                return;

            if (string.IsNullOrEmpty(wnd.charmTransKey))
                ALUGUICommon.setLabelTxt(wnd.txtCharm, _m_consortInfo.charm);
            else
                ALUGUICommon.setLabelTxt(wnd.txtCharm,
                    TextTranslate.instance.getLanguage(wnd.charmTransKey, _m_consortInfo.charm));
        }
        
        /// <summary>
        /// 刷新羁绊等级
        /// </summary>
        private void _refreshFetterLvl()
        {
            if (wnd == null || _m_consortInfo == null || _m_consortInfo.fetterInfo == null || _m_consortInfo.fetterInfo.consortFettersLvlRef == null)
                return;

            if (_m_wndConsortFetterInfo != null)
            {
                _m_wndConsortFetterInfo.showWnd();
                _m_wndConsortFetterInfo.setData(_m_consortInfo.fetterInfo);
            }
        }

        /// <summary>
        /// 妃子简介按钮点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onConsortProfileBtnClick(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndUnlockConsortDetailProfile.instance, () =>
            {
                GGUIWndUnlockConsortDetailProfile.instance.showWnd();
                GGUIWndUnlockConsortDetailProfile.instance.setData(_m_consortInfo.consortRefObj);
            }, EUIQueueStageType.MAIN, UINodeTagConst.C_UNLOCK_CONSORT_DETAIL_PROFILE_WND, false, false);
        }
        
        #region 品质图标GO

        private void _popQualityGo()
        {
            if (wnd == null || wnd.goQualityIconParent == null || _m_consortInfo == null)
                return;

            NPQualityExtRefObj qualityExtRef = GCommon.getQualityExtRefObj(ENPItemType.CONSORT, _m_consortInfo.consortId);
            if (qualityExtRef != null && qualityExtRef.quality_go_index != null)
            {
                _m_qualityGoIndex = qualityExtRef.quality_go_index;
                GGoIndexCacheMgr.instance.popItem(_m_qualityGoIndex, _go =>
                {
                    if (_go == null || wnd == null || wnd.goQualityIconParent == null)
                        return;

                    _go.transform.SetParent(wnd.goQualityIconParent);
                    _go.transform.localPosition = Vector3.zero;
                    _go.transform.localScale = Vector3.one;
                    _m_qualityGo = _go;
                });
            }
        }

        //回收品质GO
        private void _pushBackQualityGo()
        {
            if (_m_qualityGoIndex != null && _m_qualityGo != null)
                GGoIndexCacheMgr.instance.pushbackItem(_m_qualityGoIndex, _m_qualityGo);
            _m_qualityGoIndex = null;
            _m_qualityGo = null;
        }

        #endregion
        
        #region 窗口消息

        /// <summary>
        /// 亲密度变化
        /// </summary>
        private void _onConsortIntimacyChg(params object[] _objs)
        {
            if (wnd == null || _m_consortInfo == null || _objs == null || _objs.Length < 1 || !(_objs[0] is long))
                return;

            long consortId = (long) _objs[0];
            if(consortId != _m_consortInfo.consortId)
                return;

            _refreshIntimacy();
        }
        
        /// <summary>
        /// 魅力值
        /// </summary>
        /// <param name="_objs"></param>
        private void _onConsortCharmChg(params object[] _objs)
        {
            if (wnd == null || _m_consortInfo == null || _objs == null || _objs.Length < 1 || !(_objs[0] is long))
                return;

            long consortId = (long) _objs[0];
            if(consortId != _m_consortInfo.consortId)
                return;

            _refreshCharm();
        }
        
        /// <summary>
        /// 羁绊等级变化
        /// </summary>
        /// <param name="_objs"></param>
        private void _onConsortFetterLvlChg(params object[] _objs)
        {
            if (wnd == null || _m_consortInfo == null || _objs == null || _objs.Length < 1 || !(_objs[0] is long))
                return;

            long consortId = (long) _objs[0];
            if(consortId != _m_consortInfo.consortId)
                return;

            _refreshFetterLvl();
        }

        #endregion
    }
}