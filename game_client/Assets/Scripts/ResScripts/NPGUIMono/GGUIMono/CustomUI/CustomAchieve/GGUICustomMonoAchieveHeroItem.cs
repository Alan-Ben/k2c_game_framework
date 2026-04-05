// using System.Collections.Generic;
// using ALPackage;
// using UnityEngine;
//
// namespace GOE
// {
//     
//     /// <summary>
//     ///  自定义的骑士头像item显示窗口
//     /// </summary>
//     public class GGUICustomMonoAchieveHeroItem : MonoBehaviour
//     {
//         [ALHeader("骑士id")]
//         public int heroId;
//         [ALHeader("骑士mono")]
//         public GGUIMonoAchieveHeroIconItem itemMono;
//         [ALHeader("选中的骑士的名字")]
//         public TextEx txtHeroName;
//         [ALHeader("擅长属性")]
//         public GGUIMonoCommonAttrContainer attrContainer;
//         [ALHeader("星级")]
//         public GGUIMonoHeroCommonStarContainer starContainer;
//         [ALHeader("骑士showcase")] 
//         public GGUIMonoCommonShowCase showCase;
//         
//         
//         #if NP_GAME
//         
//         private GGUIWndAchieveHeroIconItem _m_itemWnd;
//         private HeroRefShowData _m_heroInfo;
//         private NPGGUIWndCommonShowCase _m_showCaseWnd;
//         private GGUIWndCommonAttrContainer _m_attrContainer;
//         public GGUIWndHeroCommonStarContainer _m_starContainer;
//         
//         private void OnEnable()
//         {
//             _m_heroInfo = new HeroRefShowData(heroId);
//             if (null != itemMono)
//             {
//                 _m_itemWnd = new GGUIWndAchieveHeroIconItem(itemMono);
//                 _m_itemWnd.clickAction += _clickShowHeroDetail;
//             }
//             
//             _m_itemWnd?.showWnd();
//             _m_itemWnd?.setInfo(_m_heroInfo);
//             //展示形象
//             if (null == _m_showCaseWnd && showCase != null)
//             {
//                 _m_showCaseWnd = new NPGGUIWndCommonShowCase(showCase);
//             }
//             _m_showCaseWnd?.showWnd(new ShowCaseCommonResUnitInfoObj(_m_heroInfo.getTdShow()));
//                 
//             ALUGUICommon.setLabelTxt(txtHeroName, _m_heroInfo.heroRefObj.transName);
//
//             //属性列表
//             if (null == _m_attrContainer && null != attrContainer)
//                 _m_attrContainer = new GGUIWndCommonAttrContainer(attrContainer);
//
//             List<CommonAttrItemInfo> attrList = new List<CommonAttrItemInfo>();
//             for (int i = 0; i < _m_heroInfo.heroRefObj.attr_type_list.Count; i++)
//             {
//                 attrList.Add(new CommonAttrItemInfo(_m_heroInfo.heroRefObj.attr_type_list[i],0));
//             }
//             _m_attrContainer?.showWnd();
//             _m_attrContainer?.showItemList(attrList);
//
//             //星级列表
//             if (_m_starContainer == null && null != starContainer)
//                 _m_starContainer = new GGUIWndHeroCommonStarContainer(starContainer);
//             _m_starContainer?.showWnd();
//             _m_starContainer?.showItemList(_m_heroInfo.getStar());
//         }
//
//         private void _clickShowHeroDetail(GGUIWndAchieveHeroIconItem obj)
//         {
//             
//             HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(heroId);
//             HeroCardShowInfo heroCardShowInfo = new HeroCardShowInfo(heroInfo, _m_heroInfo.heroRefObj);
//             List<HeroCardShowInfo> heroCardShowInfoList = new List<HeroCardShowInfo>();
//             heroCardShowInfoList.Add(heroCardShowInfo);
//
//             if (heroInfo != null)
//             {
//                 //打开骑士详情弹窗
//                 QueueMgr.instance.AddNode(new GMainQueueHeroInfoNode(heroCardShowInfo, heroCardShowInfoList));
//             }
//             else
//             {
//                 //打开未解锁骑士详情弹窗
//                 QueueMgr.instance.AddNode(new GMainQueueHeroLockInfoNode(heroCardShowInfo, heroCardShowInfoList));
//             }
//         }
//
//         private void OnDisable()
//         {
//             _m_itemWnd?.discard();
//             _m_itemWnd = null;
//             _m_showCaseWnd?.discard();
//             _m_showCaseWnd = null;
//             _m_attrContainer?.discard();
//             _m_attrContainer = null;
//             _m_starContainer?.discard();
//             _m_starContainer = null;
//         }
// #endif
//         
//     }
// }