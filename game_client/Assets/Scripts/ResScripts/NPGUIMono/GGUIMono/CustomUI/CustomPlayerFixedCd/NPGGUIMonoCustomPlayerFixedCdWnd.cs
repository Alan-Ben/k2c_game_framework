using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// FixedCdCustomMono
    /// </summary>
    public class NPGGUIMonoCustomPlayerFixedCdWnd : MonoBehaviour
    {
        [ALHeader("颜色配置")]
        public List<NPGGUICommonConditionChgColorInfo> chgColorConfig;
        [ALHeader("cd表id")]
        public long fixedCdId;

        [ALHeader("文本")]
        public Text txtNum;
        [ALHeader("是否显示最大数量")]
        public bool isShowMaxCount;

        [ALHeader("刷新间隔（秒）")]
        [Min(0.5f)]
        public float refreshInterval = 1f;

        [ALHeader("查看详情按钮")]
        public GameObject btnInfo;
        [ALHeader("详情弹窗间隔")]
        public float toolTipInterval = 0f;

        [ALHeader("获取按钮")]
        public GameObject btnGet;
        
#if NP_GAME
        [System.NonSerialized]
        private ALCommonEnableTaskController _m_tRefreshTask;//刷新任务
        private void OnEnable()
        {
            _initRefreshTask();
            ALUGUICommon.combineBtnClick(btnInfo, _onClickInfo);
            ALUGUICommon.combineBtnClick(btnGet, _onClickGet);
        }

        private void OnDisable()
        {
            _discardRefreshTask();
            ALUGUICommon.uncombineBtnClick(btnInfo, _onClickInfo);
            ALUGUICommon.uncombineBtnClick(btnGet, _onClickGet);
        }

        /// <summary>
        /// 点击详情
        /// </summary>
        /// <param name="_gameObject"></param>
        private void _onClickInfo(GameObject _gameObject)
        {
            // NPGNodeCommonToolTip_ItemDetail toolTip = new NPGNodeCommonToolTip_ItemDetail(UIResPathConst.WIN_COMMON_RESOURCES_TIP, ENPItemType.FIXED_CD, fixedCdId, _gameObject.GetComponent<RectTransform>(), toolTipInterval);
            // QueueMgr.instance.AddNode(toolTip);
            QueueMgr.instance.AddNode(new GNodeCommonToolTip_Title_Text(
                UIResPathAssistant.getAssetPath(UIResPathConst.WIN_TOOL_TIP_TITLE_TEXT),
                UIResPathAssistant.getObjName(UIResPathConst.WIN_TOOL_TIP_TITLE_TEXT),
                GCommon.getItemName(ENPItemType.FIXED_CD, fixedCdId),
                GCommon.getItemDesc(ENPItemType.FIXED_CD, fixedCdId),
                _gameObject.GetComponent<RectTransform>(), 0, toolTipInterval));
        }

        /// <summary>
        /// 获取按钮
        /// </summary>
        /// <param name="_gameObject"></param>
        private void _onClickGet(GameObject _gameObject)
        {
            GCommon.popItemAccessWays(ENPItemType.FIXED_CD, fixedCdId);
        }
        
        private void _initRefreshTask()
        {
            _discardRefreshTask();
            _m_tRefreshTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_refresh, refreshInterval);
        }

        private void _discardRefreshTask()
        {
            _m_tRefreshTask.setDisable();
        }


        private void _refresh()
        {
            if (!gameObject.activeInHierarchy)
                return;

            NPPlayerFixedCDInfo cdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(fixedCdId);
            if (cdInfo == null)
                return;
            if(isShowMaxCount)
                ALUGUICommon.setLabelTxt(txtNum, TextTranslate.instance.getLanguage(TransKeyConst.player_lazyCd_num, cdInfo.getCount(), cdInfo.getMaxCount()));
            else 
                ALUGUICommon.setLabelTxt(txtNum, cdInfo.getCount());

            for (int i = 0; i < chgColorConfig.Count; i++)
            {
                chgColorConfig[i].refreshColor();
            }
        }
#endif
    }
}
