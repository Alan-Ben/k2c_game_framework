using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 奇物详情信息窗口
    /// </summary>
    public class GGUIWndTreasureHuntTreasureDetailInfo : _ANPGGUIBasicWnd<GGUIMonoTreasureHuntTreasureDetailInfo>
    {
        private static GGUIWndTreasureHuntTreasureDetailInfo _g_instance;
        public static GGUIWndTreasureHuntTreasureDetailInfo instace { get { return _g_instance ??= new GGUIWndTreasureHuntTreasureDetailInfo(); } }

        private List<_ITreasureHuntTreasureInfo> _m_AllTreasureList;
        private int _m_iCurShowTreasureIndex = -1;//当前显示的奇物索引
        
        private GGUIWndTreasureHuntTreasureInfo _m_wTreasureInfo;
        private GGUIWndTreasureHuntSkillInfo<GGUIMonoTreasureHuntSkillInfo> _m_wSkillInfo;

        public GGUIWndTreasureHuntTreasureDetailInfo() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntTreasureDetailInfo.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntTreasureDetailInfo.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoTreasureInfo != null)
                _m_wTreasureInfo = new GGUIWndTreasureHuntTreasureInfo(wnd.monoTreasureInfo);

            if (wnd.monoSkillInfo != null)
            {
                _m_wSkillInfo = new GGUIWndTreasureHuntSkillInfo<GGUIMonoTreasureHuntSkillInfo>(wnd.monoSkillInfo);
                _m_wSkillInfo.onSkillOpClick += _onClickSkillOpBtn;
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnPre, _onClickPre);
            ALUGUICommon.combineBtnClick(wnd.btnNext, _onClickNext);
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
                ALUGUICommon.uncombineBtnClick(wnd.btnPre, _onClickPre);
                ALUGUICommon.uncombineBtnClick(wnd.btnNext, _onClickNext);
            }

            _m_wTreasureInfo?.discard();
            _m_wTreasureInfo = null;

            if (_m_wSkillInfo != null)
            {
                _m_wSkillInfo.onSkillOpClick -= _onClickSkillOpBtn;
                _m_wSkillInfo.discard();
                _m_wSkillInfo = null;
            }

            _m_AllTreasureList?.Clear();
            _m_AllTreasureList = null;
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wTreasureInfo?.hideWnd();
            _m_wSkillInfo?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wTreasureInfo?.resetWnd();
            _m_wSkillInfo?.resetWnd();
        }

        public void setData(List<_ITreasureHuntTreasureInfo> _allTreasureList, _ITreasureHuntTreasureInfo _showTreasureInfo)
        {
            if(_m_AllTreasureList == null)
                _m_AllTreasureList = new List<_ITreasureHuntTreasureInfo>();
            _m_AllTreasureList.Clear();
            if(_allTreasureList != null)
                _m_AllTreasureList.AddRange(_allTreasureList);

            if (_showTreasureInfo != null)
            {
                _m_iCurShowTreasureIndex = _m_AllTreasureList.IndexOf(_showTreasureInfo);
                if (_m_iCurShowTreasureIndex < 0)//若没有在传入的奇物列表中找到要显示的奇物
                {
                    _m_AllTreasureList.Add(_showTreasureInfo);// 则将要显示的奇物添加到列表中
                    _m_iCurShowTreasureIndex = _m_AllTreasureList.Count - 1;
                }
            }
            else// 若没有传入要显示的奇物信息, 则默认显示第一个
            {
                _m_iCurShowTreasureIndex = 0;
            }
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || !isShow || _m_AllTreasureList == null)
                return;

            _ITreasureHuntTreasureInfo showTreasureInfo = _m_AllTreasureList.SafeGet(_m_iCurShowTreasureIndex);
            if(showTreasureInfo == null)
                return;
            
            // 刷新奇物信息
            if (_m_wTreasureInfo != null)
            {
                _m_wTreasureInfo.showWnd();
                _m_wTreasureInfo.setData(showTreasureInfo);
            }

            // 刷新获取时间
            ALUGUICommon.setLabelTxt(wnd.txtGainTime,
                TextTranslate.instance.getLanguage(TransKeyConst.treasureHunt_treasureDetailGainTime_str,
                    TimeUtil.DateTime2StringMDYHMS(TimeUtil.FromUTCMilliseconds(showTreasureInfo.gainTimeMs))));

            // 刷新技能信息
            if (_m_wSkillInfo != null)
            {
                if (showTreasureInfo.skillInfo != null)
                {
                    _m_wSkillInfo.showWnd();
                    _m_wSkillInfo.setData(showTreasureInfo.skillInfo);    
                }
                else
                {
                    _m_wSkillInfo.hideWnd();    
                }
            }

            _refreshPreNextBtn();
        }

        /// <summary>
        /// 刷新前后翻页按钮
        /// </summary>
        private void _refreshPreNextBtn()
        {
            if(wnd == null)
                return;

            // 若可展示的奇物列表为空或仅有一个, 则前后翻页按钮均不显示
            if (_m_AllTreasureList == null || _m_AllTreasureList.Count <= 1)
            {
                ALUGUICommon.setGameObjEnable(wnd.btnPre, false);
                ALUGUICommon.setGameObjEnable(wnd.btnNext, false);
                return;
            }

            ALUGUICommon.setGameObjEnable(wnd.btnPre, _m_iCurShowTreasureIndex > 0);
            ALUGUICommon.setGameObjEnable(wnd.btnNext, _m_iCurShowTreasureIndex < _m_AllTreasureList.Count - 1);
        }
        
        /// <summary>
        /// 点击上一个奇物按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickPre(GameObject _go)
        {
            if (_m_AllTreasureList == null || _m_AllTreasureList.Count <= 1)
                return;

            if (_m_iCurShowTreasureIndex > 0)
            {
                _m_iCurShowTreasureIndex--;
                _refreshWnd();
            }
        }

        /// <summary>
        /// 点击下一个奇物按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickNext(GameObject _go)
        {
            if (_m_AllTreasureList == null || _m_AllTreasureList.Count <= 1)
                return;

            if (_m_iCurShowTreasureIndex < _m_AllTreasureList.Count - 1)
            {
                _m_iCurShowTreasureIndex++;
                _refreshWnd();
            }
        }
        
        /// <summary>
        /// 点击技能操作按钮
        /// </summary>
        /// <param name="_skillInfo">技能信息</param>
        private void _onClickSkillOpBtn(_ITreasureHuntSkillInfo _skillInfo)
        {
            if (_skillInfo == null || _m_AllTreasureList == null)
                return;

            _ITreasureHuntTreasureInfo showTreasureInfo = _m_AllTreasureList.SafeGet(_m_iCurShowTreasureIndex);
            if(showTreasureInfo == null)
                return;
            
            ETreasureHuntSkillState skillState = _skillInfo.skillState;
            if (skillState == ETreasureHuntSkillState.UNLOCK_NOT_ACTIVATE)
            {
                if (showTreasureInfo is TreasureHuntGotTreasureInfo gotTreasureInfo)
                {
                    // 激活技能, 直接前往实验室激活
                    GNodeTreasureHuntLab.addNodeByPutInTreasureInfo(gotTreasureInfo);    
                }
                else
                {
                    Debug.LogError($"[GGUIWndTreasureHuntTreasureDetailInfo _onClickSkillOpBtn] 奇物 {showTreasureInfo.treasureId} 的技能状态为 UNLOCK_NOT_ACTIVATE, 但奇物信息不是 TreasureHuntGotTreasureInfo 类型");
                }
            }
            else if (skillState == ETreasureHuntSkillState.UNLOCK_ACTIVATE)
            {
                // 升级技能
                NPPlayer.instance.treasureHuntComponent.reqTreasureHuntTreasureSkillUpgrade(showTreasureInfo.treasureId,
                    (_isSucc, _msg) =>
                    {
                        if(!_isSucc || wnd == null || !isShow || _m_wSkillInfo == null)
                            return;

                        // 刷新技能显示
                        _m_wSkillInfo.showSkillLevelUp();
                        
                        // 升级成功提示
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.treasureHunt_skillUpgradeSuccTip_none);
                    });
            }
        }
        
        
        
        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_TREASURE_DETAIL_INFO);
        }
    }
}