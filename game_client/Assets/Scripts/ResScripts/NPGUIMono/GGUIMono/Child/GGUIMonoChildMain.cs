using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public enum EChildMainTargetChildType
    {
        NONE,
        [InspectorName("NAMED_NO_GRADUATE(已取名未毕业)")]
        NAMED_NO_GRADUATE,
    }

    public enum GGUIMonoChildMainShowType
    { 
        Naming,
        Educating,
        PhaseUp,
        Graduating,
        Empty
    }
    [Serializable]
    public class GGUIMonoChildMainNamingShow
    {
        [ALHeader("要显示的对象列表")]
        public List<GameObject> showList;
        [ALHeader("取名按钮")]
        public GameObject btnNaming;
        [ALHeader("一键取名的勾选框和未解锁时展示的对象")]
        public NPGGUIMonoCommonToggleEx monoOneKeyNamingToggle;
        public List<GameObject> listOneKeyLockShow;
        [ALHeader("一键取名的规则按钮和弹出的提示框的偏移值")]
        public GameObject btnRule;
        public Vector2 tooltipOffset;
    }
    [Serializable]
    public class GGUIMonoChildMainEducatingShow
    {
        [ALHeader("要显示的对象列表")]
        public List<GameObject> showList;
        [ALHeader("上课按钮和其消耗")]
        public GameObject btnEducating;
        public GameObject btnEducating2;
        public NPGGUIMonoCommonItem monoCostItem;
        [ALHeader("如果是免费时显示隐藏的对象")]
        public List<GameObject> listFreeShow;
        public List<GameObject> listFreeHide;
        [ALHeader("上课获得大臣经验使用的上浮 tip id ")]
        public long expCollectTipId = 3;
        public long particleId = 0;
        [ALHeader("脑力值的文本和增加按钮")]
        public Text txtBrainValue;
        public GameObject btnBrainAdd;
        [ALHeader("脑力值自己恢复的所需时间")]
        public Text txtRecoverTime;
        [ALHeader("脑力值处于不同状态时显示的对象")]
        public List<GameObject> listBrainEmptyShow;
        public List<GameObject> listBrainNormalShow;
        public List<GameObject> listBrainMaxShow;
        [ALHeader("一键上课的勾选框和未解锁时展示的对象")]
        public NPGGUIMonoCommonToggleEx monoOneKeyEducatingToggle;
        public List<GameObject> listOneKeyLockShow;
        [ALHeader("停止一键上课按钮")]
        public GameObject btnStopOneKeyEducating;
        [ALHeader("一键上课进阶功能按钮和进阶功能解锁时展示的对象")]
        public GameObject btnOneKeyPlusBtn;
        public List<GameObject> listOneKeyPlusUnlockShow;
        [ALHeader("一键上课的效果中需要显示和隐藏的对象")]
        public List<GameObject> listOneKeyShow;
        public List<GameObject> listOneKeyHide;
        [ALHeader("语音气泡附加窗口")]
        public GGUIMonoChildVoiceBubble monoVoiceBubble;
        [ALHeader("每次触发语音气泡的上课点击次数")]
        public int triggerVoiceBubbleClickCount = 10;
        [ALHeader("长按上课的间隔时间")]
        public float longPressSpace = 0.2f;


        public void setBrainValue(int _value, int _max)
        {
            ALUGUICommon.setGameObjEnable(listBrainEmptyShow, false);
            ALUGUICommon.setGameObjEnable(listBrainNormalShow, false);
            ALUGUICommon.setGameObjEnable(listBrainMaxShow, false);

            if (_value <= 0)
                ALUGUICommon.setGameObjEnable(listBrainEmptyShow, true);
            else if (_value < _max)
                ALUGUICommon.setGameObjEnable(listBrainNormalShow, true);
            else 
                ALUGUICommon.setGameObjEnable(listBrainMaxShow, true);
        }
        public void setOneKeyState(bool _enable)
        {
            ALUGUICommon.setGameObjEnable(listOneKeyShow, false);
            ALUGUICommon.setGameObjEnable(listOneKeyHide, false);
            ALUGUICommon.setGameObjEnable(_enable ? listOneKeyShow : listOneKeyHide, true);
        }
        public void setIsCostFree(bool _isFree)
        {
            ALUGUICommon.setGameObjEnable(listFreeShow, false);
            ALUGUICommon.setGameObjEnable(listFreeHide, false);
            ALUGUICommon.setGameObjEnable(_isFree ? listFreeShow : listFreeHide, true);
        }
    }
    [Serializable]
    public class GGUIMonoChildMainPhaseUp
    {
        [ALHeader("要显示的对象列表")]
        public List<GameObject> showList;
    }
    [Serializable]
    public class GGUIMonoChildMainGraduatingShow
    {
        [ALHeader("要显示的对象列表")]
        public List<GameObject> showList;
        [ALHeader("毕业按钮")]
        public GameObject btnGraduating;
        [ALHeader("一键毕业的勾选框和未解锁时展示的对象")]
        public NPGGUIMonoCommonToggleEx monoOneKeyGraduatingToggle;
        public List<GameObject> listOneKeyLockShow;
    }
    [Serializable]
    public class GGUIMonoChildMainEmpty
    {
        [ALHeader("要显示的对象列表")]
        public List<GameObject> showList;
        [ALHeader("跳转按钮")]
        public GameObject btnGoto;
        [ALHeader("获取子嗣")]
        public GameObject btnGetChild;
    }
    [Serializable]
    public class GGUIMonoChildMainStepShow
    {
        [ALHeader("要显示的对象列表")]
        public List<GameObject> showList;
    }
    public class GGUIMonoChildMain : _ANPBasicUIWndResBarMono
    {
        [ALHeader("当前的拥有的经验值")]
        public Text txtExpValue;
        [ALHeader("顺序配置每个阶段要显示的内容")]
        public List<GGUIMonoChildMainStepShow> listPhaseShow;
        [ALHeader("详情按钮")]
        public GameObject btnDetail;
        [ALHeader("当前阶段的进度以及阶段的分割点列表")]
        public Slider sldPhaseProgress;
        public List<Transform> listPhaseSplit;
        [ALHeader("当前的阶段进度文本")] 
        public Text txtPhaseProgress;
        [ALHeader("当前选中的子嗣的信息")]
        public GGUIMonoChildInfo monoCurChildInfo;
        [ALHeader("未命名状态下的显示")]
        public GGUIMonoChildMainNamingShow namingShow;
        [ALHeader("上课状态下的显示")]
        public GGUIMonoChildMainEducatingShow educatingShow;
        [ALHeader("升学状态下的显示")]
        public GGUIMonoChildMainPhaseUp phaseUpShow;
        [ALHeader("毕业状态下的显示")]
        public GGUIMonoChildMainGraduatingShow graduatingShow;
        [ALHeader("空状态下的显示")]
        public GGUIMonoChildMainEmpty emptyShow;
        [ALHeader("子嗣列表")]
        public GGUIMonoChildContainer monoChildContainer;
        [ALHeader("邀约buff子窗口")]
        public GGUISubMonoConsortInviteBuff monoBuff;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1201); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1201); } }


        public void setPhase(int _step)
        {
            for (int i = 0; i < listPhaseShow.Count; i++)
                ALUGUICommon.setGameObjEnable(listPhaseShow[i].showList, false);

            if (_step >= 0 && _step < listPhaseShow.Count)
                ALUGUICommon.setGameObjEnable(listPhaseShow[_step].showList, true);
        }
        public void setType(GGUIMonoChildMainShowType _type)
        {
            ALUGUICommon.setGameObjEnable(namingShow.showList, false);
            ALUGUICommon.setGameObjEnable(educatingShow.showList, false);
            ALUGUICommon.setGameObjEnable(phaseUpShow.showList, false);
            ALUGUICommon.setGameObjEnable(graduatingShow.showList, false);
            ALUGUICommon.setGameObjEnable(emptyShow.showList, false);

            switch (_type)
            {
                case GGUIMonoChildMainShowType.Naming:
                    ALUGUICommon.setGameObjEnable(namingShow.showList, true);
                    break;
                case GGUIMonoChildMainShowType.Educating:
                    ALUGUICommon.setGameObjEnable(educatingShow.showList, true);
                    break;
                case GGUIMonoChildMainShowType.PhaseUp:
                    ALUGUICommon.setGameObjEnable(phaseUpShow.showList, true);
                    break;
                case GGUIMonoChildMainShowType.Graduating:
                    ALUGUICommon.setGameObjEnable(graduatingShow.showList, true);
                    break;
                case GGUIMonoChildMainShowType.Empty:
                    ALUGUICommon.setGameObjEnable(emptyShow.showList, true);
                    break;
            }
        }
        /// <summary>
        /// 设置阶段的分割点
        /// </summary>
        /// <remarks>
        /// 传入一个 0-1 之间值的 float 列表，表示每个阶段的分割点占总进度的比例，多余的分割点会被隐藏
        /// </remarks>
        public void setPhaseSplit(List<float> _splitValueList)
        {
            if (sldPhaseProgress == null)
                return;
            
            _splitValueList ??= new List<float>(0);
            for (int i = 0; i < listPhaseSplit.Count; i++)
            {
                if (listPhaseSplit[i] == null)
                    continue;
                
                ALUGUICommon.setGameObjEnable(listPhaseSplit[i].gameObject, i < _splitValueList.Count);
                if (i < _splitValueList.Count)
                {
                    ALUGUICommon.setUIPos(listPhaseSplit[i], new Vector3(_splitValueList[i] * ((RectTransform)sldPhaseProgress.transform).rect.width, 0));
                    
                }
            }
        }
    }
}