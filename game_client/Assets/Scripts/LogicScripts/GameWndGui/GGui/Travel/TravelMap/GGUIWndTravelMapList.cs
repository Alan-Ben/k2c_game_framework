// using System.Collections.Generic;
// using ALPackage;
// using Common.TravelObj;
// using NPEnum;
// using UnityEngine;
//
// namespace GOE
// {
//     /// <summary>
//     /// 游历主界面
//     /// </summary>
//     public class GGUIWndTravelMapList: _ANPGGUIBasicWnd<GGUIMonoTravelMapList>
//     {
//         private static GGUIWndTravelMapList _g_instance = new GGUIWndTravelMapList();
//         private GGUIWndTarvelMapItemContainer _m_mapPosItemContainer;
//
//         public static GGUIWndTravelMapList instance
//         {
//             get
//             {
//                 if(null == _g_instance)
//                     _g_instance = new  GGUIWndTravelMapList();
//                 return _g_instance;
//             }
//         }
//         
//
//         public GGUIWndTravelMapList() : base(EALUIWndLayer.NORMAL)
//         {
//         }
//
//         public override bool needDiscardOnSwitch { get => true; }
//
//         protected override string _monoAssetPath { get => GGUIMonoTravelMapList.assetPath; }
//         protected override string _monoObjName { get => GGUIMonoTravelMapList.objName; }
//         protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
//         
//         protected override void _onShowWnd()
//         {
//             _refreshWnd();
//
//             // RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_TRAVEL_MAP_ENTER, 0);
//         }
//
//         protected override void _onHideWnd()
//         {
//         }
//
//         protected override void _onReset()
//         {
//         }
//
//         protected override void _onDiscard()
//         {
//             _m_mapPosItemContainer?.discard();
//             _m_mapPosItemContainer = null;
//         }
//
//         protected override void _onWndInitDone()
//         {
//             if(null == wnd)
//                 return;
//             ALUGUICommon.combineBtnClick(wnd.btnAddMessValue, _clickAddMessValue);
//             if (null != wnd.mapPosItemContainer)
//             {
//                 _m_mapPosItemContainer = new GGUIWndTarvelMapItemContainer(wnd.mapPosItemContainer);
//             }
//         }
//
//         private void _clickAddMessValue(GameObject obj)
//         {
//             QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndTravelMessGetWay.instance ,GGUIWndTravelMessGetWay.instance.showWnd);
//         }
//
//         private void _refreshWnd()
//         {
//             if(null == wnd)
//                 return;
//             ALUGUICommon.setLabelTxt(wnd.txtMessCount, NPPlayer.instance.travelComp.getTravelMessItemCount());
//             
//             _m_mapPosItemContainer?.showWnd();
//             _m_mapPosItemContainer?.showItemList(GRefdataCoreMgr.instance.travelPosCore.makeNewAllRefList());
//         }
//     }
// }