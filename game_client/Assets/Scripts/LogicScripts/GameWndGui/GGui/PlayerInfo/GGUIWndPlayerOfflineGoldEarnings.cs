using System;
using ALPackage;
using CommonEnum;
using JetBrains.Annotations;
using NPCommon;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndPlayerOfflineGoldEarnings : _ANPGGUIBasicWnd<GGUIMonoPlayerOfflineGoldEarnings>
    {
        [NotNull] public static GGUIWndPlayerOfflineGoldEarnings instance { get { return _g_instance ??= new GGUIWndPlayerOfflineGoldEarnings(); } }
        private static GGUIWndPlayerOfflineGoldEarnings _g_instance;


        private OfflineGoldData _m_offlineData;
        
        
        public GGUIWndPlayerOfflineGoldEarnings() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoPlayerOfflineGoldEarnings.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoPlayerOfflineGoldEarnings.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
        }
        protected override void _onHideWnd()
        {
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClicked);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClicked);
            refreshWnd();
        }


        public void refreshWnd(OfflineGoldData _offlineData)
        {
            _m_offlineData = _offlineData;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.setLabelTxt(wnd.offlineTimeTxt, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_offlineGoldTime_num, TimeUtil.millisecondsToTime_Two_NoSec(_m_offlineData.time)));
            ALUGUICommon.setLabelTxt(wnd.offlineCountTxt, _m_offlineData.count.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD));
            long offlineMaxTime = NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.OFFLINE_OUTPUT_LIMIT_SEC);
            ALUGUICommon.setLabelTxt(wnd.offlineTimeMaxTxt, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_offlineGoldMaxTime_num, TimeUtil.millisecondsToTime_Two_NoSec(offlineMaxTime * 1000)));
            
            float timeRatio = Math.Min(1f, (float) _m_offlineData.time / (offlineMaxTime * 1000));
            ALUGUICommon.setSliderScale(wnd.sldTime, timeRatio);
            
            foreach (var sldShowInfo in wnd.sldShowInfoList)
            {
                bool inRange = timeRatio >= sldShowInfo.rangeStart && timeRatio <= sldShowInfo.rangeEnd;
                foreach (var go in sldShowInfo.goListShow)
                {
                    ALUGUICommon.setGameObjEnable(go, inRange);
                }
            }
        }


        private void _onCloseBtnClicked(GameObject _)
        {
            if (wnd == null)
                return;
            
            if (_m_offlineData is { count: > 0 } && wnd.particleStart != null)
                GCommon.showItemParticle(new NPCommon_ItemInfo((int) ENPItemType.CURRENCY, (long) ECurrency.SILVER, _m_offlineData.count, null), wnd.particleStart, wnd.specialParticleId);
            QueueMgr.instance.DoUIRollBackByEsc();
        }
    }
}