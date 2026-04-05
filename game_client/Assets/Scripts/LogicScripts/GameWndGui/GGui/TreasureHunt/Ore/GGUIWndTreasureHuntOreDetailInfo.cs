using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 矿石详情信息窗口
    /// </summary>
    public class GGUIWndTreasureHuntOreDetailInfo : _ANPGGUIBasicWnd<GGUIMonoTreasureHuntOreDetailInfo>
    {
        private static GGUIWndTreasureHuntOreDetailInfo _g_instance;
        public static GGUIWndTreasureHuntOreDetailInfo instance{ get { return _g_instance ??= new GGUIWndTreasureHuntOreDetailInfo(); } }
        
        private List<_ITreasureHuntOreInfo> _m_AllOreList;
        private int _m_iCurShowOreIndex = -1;//当前显示的矿石索引
        
        private GGUIWndTreasureHuntOreInfo _m_wOreInfo;
        private GGUIWndTreasureHuntSkillInfo<GGUIMonoTreasureHuntSkillInfo> _m_wNormalSkillInfo;
        private GGUIWndTreasureHuntSkillInfo<GGUIMonoTreasureHuntSkillInfo> _m_wAdvancedSkillInfo;

        public GGUIWndTreasureHuntOreDetailInfo() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntOreDetailInfo.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntOreDetailInfo.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }
        
        /// <summary>
        /// 当前显示的矿石信息
        /// </summary>
        public _ITreasureHuntOreInfo curShowOreInfo { get { return _m_AllOreList?.SafeGet(_m_iCurShowOreIndex); } }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoOreInfo != null)
                _m_wOreInfo = new GGUIWndTreasureHuntOreInfo(wnd.monoOreInfo);

            if (wnd.monoNormalSkillInfo != null)
            {
                _m_wNormalSkillInfo = new GGUIWndTreasureHuntSkillInfo<GGUIMonoTreasureHuntSkillInfo>(wnd.monoNormalSkillInfo);
                _m_wNormalSkillInfo.onSkillOpClick += _onClickNormalSkillOpBtn;
            }

            if (wnd.monoAdvancedSkillInfo != null)
            {
                _m_wAdvancedSkillInfo = new GGUIWndTreasureHuntSkillInfo<GGUIMonoTreasureHuntSkillInfo>(wnd.monoAdvancedSkillInfo);
                _m_wAdvancedSkillInfo.onSkillOpClick += _onClickAdvanceSkillOpBtn;
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnMaxMassRecordDetail, _onClickMaxMassRecordDetail);
            ALUGUICommon.combineBtnClick(wnd.btnPre, _onClickPre);
            ALUGUICommon.combineBtnClick(wnd.btnNext, _onClickNext);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
                ALUGUICommon.uncombineBtnClick(wnd.btnMaxMassRecordDetail, _onClickMaxMassRecordDetail);
                ALUGUICommon.uncombineBtnClick(wnd.btnPre, _onClickPre);
                ALUGUICommon.uncombineBtnClick(wnd.btnNext, _onClickNext);
            }

            _m_AllOreList?.Clear();
            _m_AllOreList = null;
            
            _m_wOreInfo?.discard();
            _m_wOreInfo = null;

            if (_m_wNormalSkillInfo != null)
            {
                _m_wNormalSkillInfo.onSkillOpClick -= _onClickNormalSkillOpBtn;
                _m_wNormalSkillInfo.discard();
                _m_wNormalSkillInfo = null;                
            }

            if (_m_wAdvancedSkillInfo != null)
            {
                _m_wAdvancedSkillInfo.onSkillOpClick -= _onClickAdvanceSkillOpBtn;
                _m_wAdvancedSkillInfo.discard();
                _m_wAdvancedSkillInfo = null;       
            }
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wOreInfo?.hideWnd();
            _m_wNormalSkillInfo?.hideWnd();
            _m_wAdvancedSkillInfo?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wOreInfo?.resetWnd();
            _m_wNormalSkillInfo?.resetWnd();
            _m_wAdvancedSkillInfo?.resetWnd();
        }

        public void setData(List<_ITreasureHuntOreInfo> _allOreList, _ITreasureHuntOreInfo _showOreInfo)
        {
            if(_m_AllOreList == null)
                _m_AllOreList = new List<_ITreasureHuntOreInfo>();
            _m_AllOreList.Clear();
            if(_allOreList != null)
                _m_AllOreList.AddRange(_allOreList);

            if (_showOreInfo != null)
            {
                _m_iCurShowOreIndex = _m_AllOreList.IndexOf(_showOreInfo);
                if (_m_iCurShowOreIndex < 0)//若没有在传入的矿石列表中找到要显示的矿石
                {
                    _m_AllOreList.Add(_showOreInfo);// 则将要显示的矿石添加到列表中
                    _m_iCurShowOreIndex = _m_AllOreList.Count - 1;
                }
            }
            else// 若没有传入要显示的矿石信息, 则默认显示第一个
            {
                _m_iCurShowOreIndex = 0;
            }
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || !isShow)
                return;
                
            _ITreasureHuntOreInfo showOreInfo = curShowOreInfo;
            if(_m_AllOreList != null)
                showOreInfo = _m_AllOreList.SafeGet(_m_iCurShowOreIndex);
            
            if(showOreInfo == null)
                return;

            if (_m_wOreInfo != null)
            {
                _m_wOreInfo.showWnd();
                _m_wOreInfo.setData(showOreInfo);
            }

            ALUGUICommon.setLabelTxt(wnd.txtFirstGetTime,
                TextTranslate.instance.getLanguage(TransKeyConst.treasureHunt_oreDetailOreFirstGainTime_str,
                    TimeUtil.DateTime2StringMDYHMS(TimeUtil.FromUTCMilliseconds(showOreInfo.getTimeMs))));

            if (_m_wNormalSkillInfo != null)
            {
                if (showOreInfo.normalSkillInfo != null)
                {
                    _m_wNormalSkillInfo.showWnd();
                    _m_wNormalSkillInfo.setData(showOreInfo.normalSkillInfo);
                }
                else
                {
                    _m_wNormalSkillInfo.hideWnd();
                }
            }
            
            if (_m_wAdvancedSkillInfo != null)
            {
                if (showOreInfo.advanceSkillInfo != null)
                {
                    _m_wAdvancedSkillInfo.showWnd();
                    _m_wAdvancedSkillInfo.setData(showOreInfo.advanceSkillInfo);
                }
                else
                {
                    _m_wAdvancedSkillInfo.hideWnd();
                }
            }

            if (curShowOreInfo is TreasureHuntGotOreInfo gotTreasureInfo)
            {
                bool hasMaxMassRecordCanDraw = gotTreasureInfo.hasCanDrawRecordReward();
                ALUGUICommon.setGameObjEnable(wnd.hasMaxMassRecordCanDrawShow, hasMaxMassRecordCanDraw);
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

            // 若可展示的矿石列表为空或仅有一个, 则前后翻页按钮均不显示
            if (_m_AllOreList == null || _m_AllOreList.Count <= 1)
            {
                ALUGUICommon.setGameObjEnable(wnd.btnPre, false);
                ALUGUICommon.setGameObjEnable(wnd.btnNext, false);
                return;
            }

            ALUGUICommon.setGameObjEnable(wnd.btnPre, _m_iCurShowOreIndex > 0);
            ALUGUICommon.setGameObjEnable(wnd.btnNext, _m_iCurShowOreIndex < _m_AllOreList.Count - 1);
        }
        
        /// <summary>
        /// 点击上一个矿石按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickPre(GameObject _go)
        {
            if (_m_AllOreList == null || _m_AllOreList.Count <= 1)
                return;

            if (_m_iCurShowOreIndex > 0)
            {
                _m_iCurShowOreIndex--;
                _refreshWnd();
            }
        }

        /// <summary>
        /// 点击下一个矿石按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickNext(GameObject _go)
        {
            if (_m_AllOreList == null || _m_AllOreList.Count <= 1)
                return;

            if (_m_iCurShowOreIndex < _m_AllOreList.Count - 1)
            {
                _m_iCurShowOreIndex++;
                _refreshWnd();
            }
        }
        
        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_ORE_DETAIL_INFO);
        }

        /// <summary>
        /// 点击最大质量记录详情按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickMaxMassRecordDetail(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndTreasureHuntOreMassRank.instance, () =>
            {
                GGUIWndTreasureHuntOreMassRank.instance.showWnd();
                GGUIWndTreasureHuntOreMassRank.instance.setData(curShowOreInfo);
            }, UINodeTagConst.C_TREASURE_HUNT_ORE_MASS_RANK);
        }

        /// <summary>
        /// 点击普通技能的操作按钮时的处理
        /// </summary>
        private void _onClickNormalSkillOpBtn(_ITreasureHuntSkillInfo _skillInfo)
        {
            if (_skillInfo == null)
                return;

            _ITreasureHuntOreInfo oreInfo = curShowOreInfo;
            if(oreInfo == null)
                return;
            
            ETreasureHuntSkillState skillState = _skillInfo.skillState;
            if (skillState is ETreasureHuntSkillState.UNLOCK_NOT_ACTIVATE)
            {
                NPPlayer.instance.treasureHuntComponent.reqTreasureHuntOreSkillActive(oreInfo.oreId, true,
                    (_isSucc, _msg) =>
                    {
                        if(!_isSucc || wnd == null || !isShow || _m_wNormalSkillInfo == null)
                            return;

                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.treasureHunt_skillActiveSuccTip_none);
                        // 进行激活表现
                        _m_wNormalSkillInfo.showSkillActive();
                    });
            }
            else if(skillState is ETreasureHuntSkillState.UNLOCK_ACTIVATE)
            {
                NPPlayer.instance.treasureHuntComponent.reqTreasureHuntOreSkillUpgrade(oreInfo.oreId, true, 
                    (_isSucc, _msg) =>
                    {
                        if(!_isSucc || wnd == null || !isShow || _m_wNormalSkillInfo == null)
                            return;

                        // 刷新技能
                        _m_wNormalSkillInfo.showSkillLevelUp();
                        
                        // 升级成功提示
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.treasureHunt_skillUpgradeSuccTip_none);
                    });
            }
        }

        /// <summary>
        /// 点击高级技能的操作按钮时的处理
        /// </summary>
        /// <param name="_skillInfo"></param>
        private void _onClickAdvanceSkillOpBtn(_ITreasureHuntSkillInfo _skillInfo)
        {
            if (_skillInfo == null)
                return;

            _ITreasureHuntOreInfo oreInfo = curShowOreInfo;
            if(oreInfo == null)
                return;
            
            ETreasureHuntSkillState skillState = _skillInfo.skillState;
            if (skillState is ETreasureHuntSkillState.UNLOCK_NOT_ACTIVATE)
            {
                NPPlayer.instance.treasureHuntComponent.reqTreasureHuntOreSkillActive(oreInfo.oreId, false,
                    (_isSucc, _msg) =>
                    {
                        if(!_isSucc || wnd == null || !isShow || _m_wAdvancedSkillInfo == null)
                            return;

                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.treasureHunt_skillActiveSuccTip_none);
                        // 进行激活表现
                        _m_wAdvancedSkillInfo.showSkillActive();
                    });
            }
            else if(skillState is ETreasureHuntSkillState.UNLOCK_ACTIVATE)
            {
                NPPlayer.instance.treasureHuntComponent.reqTreasureHuntOreSkillUpgrade(oreInfo.oreId, false, 
                    (_isSucc, _msg) =>
                    {
                        if(!_isSucc || wnd == null || !isShow || _m_wAdvancedSkillInfo == null)
                            return;

                        // 刷新技能
                        _m_wAdvancedSkillInfo.showSkillLevelUp();
                        
                        // 升级成功提示
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.treasureHunt_skillUpgradeSuccTip_none);
                    });
            }
        }
    }
}