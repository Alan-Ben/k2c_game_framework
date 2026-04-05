using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 对话主窗体
    /// </summary>
    public class NPGGUIMonoDialogue : _AALBasicUIWndMono
    {
        [ALHeader("对话框窗体父节点")]
        public Transform dialogParent;
        [ALHeader("跳过按钮")]
        public GameObject btnSkip;
        [ALHeader("可跳过对话时 显示的物体")]
        public List<GameObject> goListShowOnCanSkip;
        [ALHeader("可回顾对话时 显示的物体")]
        public List<GameObject> goListShowOnCanReview;
        [ALHeader("可自动播放对话时 显示的物体")]
        public List<GameObject> goListShowOnCanAutoPlay;
        [ALHeader("对话演出showcase")]
        public GGUIMonoCommonShowCase monoShowcase;
        [ALHeader("自动播放按钮")]
        public NPGGUIMonoCommonTab monoAutoPlayTab;
        [ALHeader("自动播放间隔")]
        public float autoPlayInterval;
        [ALHeader("展示对话历史按钮")]
        public GameObject btnShowHistory;
        [ALHeader("漫画窗体父节点")]
        public Transform comicParent;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(22000); } }
        public static string objName { get { return UIResPathAssistant.getObjName(22000); } }
    }
}
