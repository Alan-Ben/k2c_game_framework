// using System.Collections.Generic;
// using ALPackage;
// using NPEnum;
// using UnityEngine;
//
// namespace GOE
// {
//     public class GGUICustomMonoAchieveStepItemWnd_Hero: _AGGUICustomMonoAchieveStepItemWnd
//     {
//         
//         [ALHeader("骑士id")] 
//         public long heroId;
// #if NP_GAME
//         
//         private HeroRefShowData _m_heroData;
//         private HeroRefShowData heroData
//         {
//             get
//             {
//                 if (null == _m_heroData)
//                 {
//                     _m_heroData = new HeroRefShowData(heroId);
//                 }
//                 return _m_heroData;
//             }
//         }
//
//         protected override NPGTextureIndex _getIcon()
//         {
//             return heroData.getIcon();
//         }
//
//         protected override void _onSelected(bool _isSelected)
//         {
//         }
//
//         protected override void _onDiscardWnd()
//         {
//             
//         }
//
//         /// <summary>
//         /// 点击信息按钮
//         /// </summary>
//         public override void dealClickInfo()
//         {
//             HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(heroId);
//             HeroCardShowInfo heroCardShowInfo = new HeroCardShowInfo(heroInfo, _m_heroData.heroRefObj);
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
//         public override void dealGetReward()
//         {
//             if(null == achieveInfo)
//                 return;
//             NPPlayer.instance.achieveComp.reqDoneAchieveStep(achieveInfo.achieveId, stepId, null);
//         }
//
// #endif
//     }
// }