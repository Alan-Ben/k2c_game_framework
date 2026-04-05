// using ALPackage;
// using UnityEngine;
//
// namespace GOE
// {
//     public class GGUIPrefabSubWndStageGoalPageTaskLinePath : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoStageGoalPageTaskLinePath>
//     {
//         private readonly GGUIPrefabSubWndStageGoalPageTask _m_pageWnd;
//         private readonly long _m_uiResId;
//         
//         
//         public GGUIPrefabSubWndStageGoalPageTaskLinePath(Transform _parent, GGUIPrefabSubWndStageGoalPageTask _pageWnd, long _uiResId) 
//             : base(_parent)
//         {
//             _m_pageWnd = _pageWnd;
//             _m_uiResId = _uiResId;
//         }
//         
//         
//         public long uiResId { get { return _m_uiResId; } }
//         protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_uiResId); } }
//         protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_uiResId); } }
//         protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
//         
//
//         protected override void _onShowWnd()
//         {            
//         }
//         protected override void _onHideWnd()
//         {
//         }
//         protected override void _onReset()
//         {
//         }
//         protected override void _onDiscard()
//         {
//         }
//         protected override void _onWndInitDone()
//         {
//             if (wnd == null)
//                 return;
//
//             _m_pageWnd?.showLine(wnd.linePathList);
//         }
//         protected override void _loadOp()
//         {
//             _m_pageWnd?.clearLine();
//             base._loadOp();
//         }
//     }
// }