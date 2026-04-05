// using System;
// using System.Collections.Generic;
// using ALPackage;
// using UnityEngine;
//
// namespace GOE
// {
//     /// <summary>
//     /// 妃子入口CustomMono
//     /// </summary>
//     public class GUICustomMonoConsortEntrance : MonoBehaviour
//     {
//         [ALHeader("默认显示的妃子id")]
//         public long defaultShowConsortId = 0;
//         
//         [ALHeader("妃子tdShow")]
//         public GGUIMonoConsortShowCaseSubWnd monoConsortShowCase;
//         [ALHeader("初始播放的动画")]
//         public string startAniTag;
//         
//         [ALHeader("打字机附加窗口")]
//         public GGUIMonoTextTypewriter monoTypewriter;
//         
//         [ALHeader("文本显示完气泡隐藏的延迟时间")]
//         public float hideBubbleDelay = 3f;
//         [ALHeader("气泡隐藏后再显示下一个气泡的延迟时间")]
//         public float showNextBubbleDelay = 3f;
//
// #if NP_GAME
//         /// <summary>
//         /// 妃子形象
//         /// </summary>
//         private GGUIWndConsortShowCaseSubWnd _m_wConsortTdShow;
//         
//         /// <summary>
//         /// 气泡
//         /// </summary>
//         private GGUIWndTextTypewriter _m_wTextTypewriter;
//
//         private long _m_lShowSerialize;//显示序列号
//
//         private _IConsortShowInfo _m_iDefaultShowConsortInfo;//默认显示妃子的展示数据
//         private _IConsortShowInfo _m_iConsortShowInfo;//显示的妃子信息
//         private NPGGoIndex _m_ConsortTdShowGoIndex;//加载的妃子形象goIndex
// #endif
//         
//         private void Awake()
//         {
//             if (monoConsortShowCase != null)
//                 _m_wConsortTdShow = new GGUIWndConsortShowCaseSubWnd(monoConsortShowCase);
//
//             if (monoTypewriter != null)
//                 _m_wTextTypewriter = new GGUIWndTextTypewriter(monoTypewriter);
//             
//             _m_iDefaultShowConsortInfo = new ConsortRefShowInfo(defaultShowConsortId);
//         }
//
//         private void OnDestroy()
//         {
//             _m_wConsortTdShow?.discard();
//             _m_wConsortTdShow = null;
//             
//             _m_wTextTypewriter?.discard();
//             _m_wTextTypewriter = null;
//
//             _m_iDefaultShowConsortInfo = null;
//             _m_iConsortShowInfo = null;
//             _m_ConsortTdShowGoIndex = null;
//         }
//
//         private void OnEnable()
//         {
//             _m_lShowSerialize = ALSerializeOpMgr.next();
//             _refreshWnd();
//             
//             WinMsg.RegisterMsgAct(WinMsgType.MAIN_ROOM_WND_SHOW, _refreshWnd);
//         }
//         
//         private void OnDisable()
//         {
//             long serialize = _m_lShowSerialize = ALSerializeOpMgr.next();
//
//             WinMsg.UnregisterMsgAct(WinMsgType.MAIN_ROOM_WND_SHOW, _refreshWnd);
//             
//             _m_iConsortShowInfo = null;
//             _m_ConsortTdShowGoIndex = null;
//
//             ALCommonTaskController.CommonActionAddNextFrameTask(()=>
//             {
//                 if(serialize != _m_lShowSerialize)
//                     return;
//                
//                 _m_wConsortTdShow?.hideWnd();
//                 _m_wTextTypewriter?.hideWnd();
//             });
//         }
//
//         private void _refreshWnd()
//         {
//             // 先根据客户端存储在服务器的数据获取当前要展示的妃子
//             _m_iConsortShowInfo = NPPlayer.instance.consortComp.getConsortInfo(NPPlayer.instance.consortComp.remarkInfo?.getConsortEntranceShowConsortId() ?? 0);
//             // 若找不到对应已经有的妃子, 则根据默认妃子id获取
//             if (_m_iConsortShowInfo == null)
//                 _m_iConsortShowInfo = NPPlayer.instance.consortComp.getConsortInfo(defaultShowConsortId);
//             // 若默认显示妃子玩家不拥有, 则尝试显示以获取妃子的第一个
//             if (_m_iConsortShowInfo == null)
//             {
//                 _m_iConsortShowInfo = NPPlayer.instance.consortComp.consortList?.SafeGet(0);
//             }
//             // 若上述都没有找到已有妃子, 显示默认妃子形象
//             if (_m_iConsortShowInfo == null)
//             {
//                 _m_iConsortShowInfo = _m_iDefaultShowConsortInfo;
//                 NPPlayer.instance.consortComp.remarkInfo?.setConsortEntranceShowConsortId(defaultShowConsortId);
//             }
//             else
//             {
//                 NPPlayer.instance.consortComp.remarkInfo?.setConsortEntranceShowConsortId(_m_iConsortShowInfo.consortId);
//             }
//
//             bool needResetAni = _m_iConsortShowInfo == null || _m_ConsortTdShowGoIndex != _m_iConsortShowInfo.tdShow;
//             _m_ConsortTdShowGoIndex = _m_iConsortShowInfo?.tdShow;
//             
//             if (_m_wConsortTdShow != null)
//             {
//                 _m_wConsortTdShow.showWnd();
//                 _m_wConsortTdShow.setData(_m_iConsortShowInfo);
//                 
//                 if(needResetAni)
//                     _m_wConsortTdShow.setTdShowAni(startAniTag, false, null);
//             }
//         }
//     }
// }