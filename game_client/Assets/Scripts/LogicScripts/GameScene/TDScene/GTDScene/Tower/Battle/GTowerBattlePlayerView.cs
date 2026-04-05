// using ALPackage;
//
// namespace GOE
// {
//     public class GTowerBattlePlayerView : _AALBasicLoadObj
//     {
//         private readonly NPGGoIndex _m_goIndex;
//         public GTowerBattlePlayerView(NPGGoIndex _goIndex)
//         {
//             _m_goIndex = _stageGoIndex;
//         }
//         
//           
//         protected override void _loadOp()
//         {
//             GGoIndexCacheMgr.instance.popItem(_m_goIndex, _go =>
//             {
//                 if (_go == null)
//                 {
//                     ALLog.Error($"[**Chapter**] 加载关卡对象 {_m_goIndex} 失败");
//                     _setLoadDone();
//                     return;
//                 }
//
//                 // _m_tdShow = _go.GetComponent<GTDMonoChapterGameStage>();
//                 // if (_m_tdShow == null)
//                 // {
//                 //     ALLog.Error($"[**Chapter**] 关卡对象资源 {_m_goIndex} 不含有 GTDMonoChapterGameStage ");
//                 //     _setLoadDone();
//                 //     return;
//                 // }
//                     
//                 _setLoadDone();
//             });
//         }
//
//         protected override void _discard()
//         {
//             // if (_m_tdShow == null)
//             //     return;
//                 
//             GGoIndexCacheMgr.instance.pushbackItem(_m_goIndex, _m_tdShow.gameObject);
//             // _m_tdShow = null;
//         }
//     }
// }