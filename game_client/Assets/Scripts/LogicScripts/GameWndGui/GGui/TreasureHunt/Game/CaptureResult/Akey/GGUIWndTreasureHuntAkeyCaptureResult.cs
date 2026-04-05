using System.Collections.Generic;
using ALPackage;
using Common.TreasureHuntObj;
using GS2GC.p036_TreasureHuntOp;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 一键探索结果窗口
    /// </summary>
    public class GGUIWndTreasureHuntAkeyCaptureResult : _ANPGGUIBasicWnd<GGUIMonoTreasureHuntAkeyCaptureResult>
    {
        private static GGUIWndTreasureHuntAkeyCaptureResult _g_instance;
        public static GGUIWndTreasureHuntAkeyCaptureResult instance { get { return _g_instance ??= new GGUIWndTreasureHuntAkeyCaptureResult(); } }

        private long _m_lAddExp = 0;
        private List<TreasureHuntCaptureResultBase> _m_lCaptureResultList;
        private int _m_iExporteCount = 0;//总探索次数

        private GGUIWndTreasureHuntAkeyCaptureResultContainer _m_wResultContainer;

        public GGUIWndTreasureHuntAkeyCaptureResult() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntAkeyCaptureResult.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntAkeyCaptureResult.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 初始化结果容器
            if (wnd.monoOreShow != null)
                _m_wResultContainer = new GGUIWndTreasureHuntAkeyCaptureResultContainer(wnd.monoOreShow);

            // 绑定确认按钮点击事件
            ALUGUICommon.combineBtnClick(wnd.btnSure, _onClickSure);
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnSure, _onClickSure);
            }

            _m_lCaptureResultList?.Clear();
            _m_lCaptureResultList = null;
            
            _m_wResultContainer?.discard();
            _m_wResultContainer = null;
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lCaptureResultList?.Clear();
            
            _m_wResultContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_lCaptureResultList?.Clear();

            _m_wResultContainer?.resetWnd();
        }

        /// <summary>
        /// 设置一键探索结果数据
        /// </summary>
        /// <param name="_retMsg">服务器返回的探索结果消息</param>
        public void setData(GS2GC_036_001_RetTreasureHuntOreCapture _retMsg)
        {
            if(_retMsg == null)
                return;

            _m_lAddExp = _retMsg.getAddExp();
            _m_iExporteCount = 0;
            
            // 解析捕获结果列表
            if (_m_lCaptureResultList == null)
                _m_lCaptureResultList = new List<TreasureHuntCaptureResultBase>();
            _m_lCaptureResultList.Clear();
            List<TreasureHunt_CaptureResult> resultList = _retMsg.getResultList();
            if (resultList != null)
            {
                TreasureHuntCaptureResultBase resultBaseInfo;
                foreach (TreasureHunt_CaptureResult serverResultItem in resultList)
                {
                    _m_iExporteCount++;
                    List<TreasureHunt_CaptureReward> rewardList = serverResultItem?.getCaptureRewardList();
                    if (rewardList == null || rewardList.Count == 0)
                        continue;

                    foreach (TreasureHunt_CaptureReward reward in rewardList)
                    {
                        resultBaseInfo = TreasureHuntCaptureResultBase.CreateCaptureResult(reward);
                        if (resultBaseInfo != null)
                            _m_lCaptureResultList.Add(resultBaseInfo);   
                    }
                }
            }

            _refreshWnd();
        }

        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || !isShow)
                return;

            // 显示总探索次数
            ALUGUICommon.setLabelTxt(wnd.txtTotalExploreCount, TextTranslate.instance.getLanguage(TransKeyConst.treasureHunt_akeyExploreTotalCount_num, _m_iExporteCount));

            // 显示增加的经验
            ALUGUICommon.setLabelTxt(wnd.txtAddExp, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _m_lAddExp));

            // 刷新捕获结果容器
            if (_m_wResultContainer != null)
            {
                _m_wResultContainer.showWnd();
                _m_wResultContainer.setData(_m_lCaptureResultList);
            }
        }

        /// <summary>
        /// 点击确认按钮
        /// </summary>
        /// <param name="_go">按钮对象</param>
        private void _onClickSure(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_AKEY_CAPTURE_RESULT);
        }
    }
}