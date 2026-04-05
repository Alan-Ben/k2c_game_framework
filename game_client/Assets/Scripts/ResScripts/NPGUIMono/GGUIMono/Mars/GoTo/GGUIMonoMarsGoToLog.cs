using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{

    /// <summary>
    /// 航行进度节点状态
    /// </summary>
    [System.Serializable]
    public class GGUIMonoMarsProcessState
    {
        [ALHeader("节点名称")]
        public Text txtName;
        [ALHeader("已到达节点显示的GO列表")]
        public List<GameObject> goArrivedShowList;
        [ALHeader("进行中节点显示的GO列表")]
        public List<GameObject> goInProgressShowList;
        [ALHeader("未到达节点显示的GO列表")]
        public List<GameObject> goNotArrivedShowList;
    }

    /// <summary>
    /// 火星航行日志界面
    /// </summary>
    public class GGUIMonoMarsGoToLog : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("关闭按钮")]
        public GameObject btnClose2;
        [ALHeader("节点状态列表（按照节点顺序配置，要和配表数量一致）")]
        public List<GGUIMonoMarsProcessState> processStateList;
        [ALHeader("火星航行日志列表")]
        public GGUIMonoMarsGoToLogGrid monoLogGrid;

        /// <summary>
        /// 设置航行进度显隐状态
        /// </summary>
        /// <param name="_curStageId"></param>
        public void setProcessState(long _curStageId)
        {
            if (processStateList == null)
                return;

            for (int i = 0; i < processStateList.Count; i++)
            {
                if(processStateList[i] == null)
                    continue;

                //已到达
                if (i <= _curStageId - 1)
                {
                    ALUGUICommon.setGameObjEnable(processStateList[i].goNotArrivedShowList, false);
                    ALUGUICommon.setGameObjEnable(processStateList[i].goInProgressShowList, false);
                    ALUGUICommon.setGameObjEnable(processStateList[i].goArrivedShowList, true);
                }//进行中
                else if (i == _curStageId)
                {
                    ALUGUICommon.setGameObjEnable(processStateList[i].goNotArrivedShowList, false);
                    ALUGUICommon.setGameObjEnable(processStateList[i].goArrivedShowList, false);
                    ALUGUICommon.setGameObjEnable(processStateList[i].goInProgressShowList, true);
                }//未到达
                else
                {
                    ALUGUICommon.setGameObjEnable(processStateList[i].goArrivedShowList, false);
                    ALUGUICommon.setGameObjEnable(processStateList[i].goInProgressShowList, false);
                    ALUGUICommon.setGameObjEnable(processStateList[i].goNotArrivedShowList, true);
                }
            }
        }

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7003); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7003); } }
    }
}
