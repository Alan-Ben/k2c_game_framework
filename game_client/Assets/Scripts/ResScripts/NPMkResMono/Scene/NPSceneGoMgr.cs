// using System.Collections.Generic;
//
// using UnityEngine;
// using ALPackage;
//
// /// <summary>
// /// 场景相关对象的管理器
// /// </summary>
// public class NPSceneGoMgr : WCGSingleton<NPSceneGoMgr>
// {
//     /// <summary>
//     /// 当前使用的场景Id
//     /// </summary>
//     private long _m_lCurUseSceneId;
//     /// <summary>
//     /// 对应场景Id需要管理的Go对象映射关系
//     /// </summary>
//     private Dictionary<long, List<GameObject>> _m_dicGoDic;
//
//     public NPSceneGoMgr()
//     {
//         _m_lCurUseSceneId = -1;
//         _m_dicGoDic = new Dictionary<long, List<GameObject>>();
//     }
//
//     /// <summary>
//     /// 注册对应scene 管理的go对象
//     /// </summary>
//     /// <param name="_sceneId"></param>
//     /// <param name="_go"></param>
//     public void regGo(long _sceneId, GameObject _go)
//     {
//         if(null == _go)
//             return;
//
//         List<GameObject> list = null;
//         if(!_m_dicGoDic.TryGetValue(_sceneId, out list))
//         {
//             list = new List<GameObject>();
//             _m_dicGoDic.Add(_sceneId, list);
//         }
//
//         //添加到队列
//         list.Add(_go);
//
//         //判断是否当前用场景，设置对应的状态
//         ALUGUICommon.setGameObjEnable(_go, _m_lCurUseSceneId == _sceneId);
//     }
//
//     /// <summary>
//     /// 注销对应scene 管理的go对象
//     /// </summary>
//     /// <param name="_sceneId"></param>
//     /// <param name="_go"></param>
//     public void unregGo(long _sceneId, GameObject _go)
//     {
//         if(null == _go)
//             return;
//
//         List<GameObject> list = null;
//         if(!_m_dicGoDic.TryGetValue(_sceneId, out list))
//         {
//             return;
//         }
//
//         //从队列删除
//         list.Remove(_go);
//     }
//
//     /// <summary>
//     /// 设置使用的场景Id
//     /// </summary>
//     /// <param name="_sceneId"></param>
//     public void setUseScene(long _sceneId)
//     {
//         if(_m_lCurUseSceneId == _sceneId)
//             return;
//
//         //将旧对象都设置为无效
//         List<GameObject> list = null;
//         if(_m_dicGoDic.TryGetValue(_m_lCurUseSceneId, out list))
//         {
//             //设置对象列表无效
//             ALUGUICommon.setGameObjEnable(list, false);
//         }
//
//         _m_lCurUseSceneId = _sceneId;
//
//         //获取新场景队列
//         if(_m_dicGoDic.TryGetValue(_m_lCurUseSceneId, out list))
//         {
//             //设置对象列表无效
//             ALUGUICommon.setGameObjEnable(list, true);
//         }
//     }
// }