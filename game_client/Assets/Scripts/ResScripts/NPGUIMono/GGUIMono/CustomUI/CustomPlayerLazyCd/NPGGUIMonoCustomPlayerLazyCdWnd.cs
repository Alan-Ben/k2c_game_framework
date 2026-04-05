using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 体力CustomMono
    /// </summary>
    public class NPGGUIMonoCustomPlayerLazyCdWnd : MonoBehaviour
    {
        [ALHeader("颜色配置")]
        public List<NPGGUICommonConditionChgColorInfo> chgColorConfig;
        [ALHeader("cd表id")]
        public long lazyCdId;

        [ALHeader("文本")]
        public Text txtNum;
        [ALHeader("在显示数量时是否显示能获取的最多数量")]
        public bool needShowMaxCount = true;

        [ALHeader("刷新间隔（秒）")]
        [Min(0.5f)]
        public float refreshInterval = 1f;

        [ALHeader("查看详情按钮")]
        public GameObject btnInfo;
        [ALHeader("详情弹窗间隔")]
        public float toolTipInterval = 0f;

        [ALHeader("获取按钮")]
        public GameObject btnGet;

        
        [ALHeader("震动的条件字符串")]
        public string conditionStr;
        [ALHeader("震动控制动画")]
        public Animation shakeAnimation;
        [ALHeader("震动动画名称")]
        public string shakeAnimationName;

        
        private ALCommonEnableTaskController _m_tRefreshTask;//刷新任务
        
        private bool _m_bIsInited = false;
        private NPPlayerConditionGroupObj _m_cgConditionGroupObj;
        private bool _m_shakePlayering;

        public NPPlayerConditionGroupObj conditionGroup
        {
            get
            {
                if (_m_bIsInited)
                    return _m_cgConditionGroupObj;

                _m_cgConditionGroupObj = NPPlayerConditionGroupObj.readConditionGroupList(conditionStr, "condPref");
                _m_bIsInited = true;
                return _m_cgConditionGroupObj;
            }
        }

#if NP_GAME
        private void OnEnable()
        {
            _m_shakePlayering = false;
            _initRefreshTask();
            ALUGUICommon.combineBtnClick(btnInfo, _onClickInfo);
            ALUGUICommon.combineBtnClick(btnGet, _onClickGet);
        }

        private void OnDisable()
        {
            _discardRefreshTask();
            ALUGUICommon.uncombineBtnClick(btnInfo, _onClickInfo);
            ALUGUICommon.uncombineBtnClick(btnGet, _onClickGet);
            if (null != shakeAnimation)
            {
                shakeAnimation.Stop();
            }
            _m_shakePlayering = false;
        }

        /// <summary>
        /// 点击详情
        /// </summary>
        /// <param name="_gameObject"></param>
        private void _onClickInfo(GameObject _gameObject)
        {
            NPGNodeCommonToolTip_LazyCdItemDetail toolTip = new NPGNodeCommonToolTip_LazyCdItemDetail(UIResPathConst.WIN_COMMON_LAZYCD_RESOURCES_TIP, lazyCdId, _gameObject.GetComponent<RectTransform>(), toolTipInterval);
            QueueMgr.instance.AddNode(toolTip);
        }

        /// <summary>
        /// 获取按钮
        /// </summary>
        /// <param name="_gameObject"></param>
        private void _onClickGet(GameObject _gameObject)
        {
            GCommon.popItemAccessWays(ENPItemType.LAZY_CD, lazyCdId);
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

            PlayerLazyCDInfo cdInfo = NPPlayer.instance.lazyCdComp.getLazyCDInfo(lazyCdId);
            if (cdInfo == null)
                return;

            if (needShowMaxCount)
            {
                ALUGUICommon.setLabelTxt(txtNum, TextTranslate.instance.getLanguage(TransKeyConst.player_lazyCd_num, cdInfo.getCount(), cdInfo.MaxCount));
            }
            else
            {
                ALUGUICommon.setLabelTxt(txtNum, TextTranslate.instance.getLanguage(TransKeyConst.common_value, cdInfo.getCount()));
            }

            for (int i = 0; i < chgColorConfig.Count; i++)
            {
                chgColorConfig[i].refreshColor();
            }
            //需要震动
            if (null != conditionGroup && null != shakeAnimation)
            {
                if (conditionGroup.IsEnable(null))
                {
                    if (_m_shakePlayering)//播放中
                    {
                        return;
                    }

                    _m_shakePlayering = true;
                    shakeAnimation.ForcePlay(shakeAnimationName);
                }
                else
                {
                    
                    _m_shakePlayering = false;
                    shakeAnimation.Stop();
                }
            }
        }
#endif
    }
}
