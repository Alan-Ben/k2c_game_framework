using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnGetCashRegisterReward : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("迎接宾客数量")]
        public Text txtGuestNum;
        [ALHeader("人气值获得量")]
        public Text txtPopularity;
        [ALHeader("厨具蓝图数量")] 
        public Text txtStationBlueprint;
        [ALHeader("心意值获得量")]
        public Text txtAffection;
        [ALHeader("结算详情列表")]
        public GGUIMonoInnGetCashRegisterRewardDetailGrid monoDetailGrid;
        [ALHeader("确定按钮")]
        public GameObject btnConfirm;

        [ALHeader("顾客数量粒子起始位置")]
        public RectTransform tranGuestNumParticleStart;
        [ALHeader("顾客数量粒子数量配置列表")]
        public List<ParticleNumRangeInfo> guestNumParticleNumConfigList;
        [ALHeader("顾客数量粒子样式id")]
        public long guestNumParticleId;
        
        [ALHeader("熟练度粒子起始位置")]
        public RectTransform tranFinesseParticleStart;
        [ALHeader("熟练度粒子数量配置列表")]
        public List<ParticleNumRangeInfo> finesseParticleNumConfigList;
        [ALHeader("熟练度粒子样式id")]
        public long finesseParticleId;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6401); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6401); } }
    }
}