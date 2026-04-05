using ALPackage;
using NPCommon;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace GOE
{
	/// <summary>
	/// 系统任务展示
	/// </summary>
	public class GGUICustomMonoSystemQuest : MonoBehaviour
    {
        [ALHeader("系统任务组id")]
        public long systemQuestGroupId;
        [ALHeader("任务详情弹窗标题key")]
        public string detailWndTitleKey;
        [ALHeader("系统任务名称")]
        public Text txtName;
        [ALHeader("任务进度文本")]
        public Text txtProgress;
        [ALHeader("任务名称及进度文本")]
        public Text txtNameAndProgress;
        [ALHeader("任务名称及进度文本翻译key(不填默认{0}{1})")]
        public string nameAndProgressTransKey;
        [ALHeader("未完成任务名称文本颜色")]
        public Color canNotGetRewardNameColor = Color.white;
        [ALHeader("已完成任务名称文本颜色")]
        public Color canGetRewardNameColor = Color.black;
        [ALHeader("未完成任务进度文本颜色")]
        public Color canNotGetRewardProgressColor = Color.yellow;
        [ALHeader("已完成任务进度文本颜色")]
        public Color canGetRewardProgressColor = Color.green;
        [ALHeader("领奖按钮")]
        public GameObject btnGetReward;
        [ALHeader("打开详情按钮")]
        public GameObject btnDetail;
        [ALHeader("可领奖时需要显示的GO列表")]
        public List<GameObject> goFinishShowList;
        [ALHeader("可领奖时需要隐藏的GO列表")]
        public List<GameObject> goFinishHideList;
        [ALHeader("没有任务时需要隐藏的GO列表")]
        public List<GameObject> goEmptyHideList;
        [ALHeader("奖励粒子开始位置")]
        public RectTransform particleStartRectTransform;
        [ALHeader("领奖特效父节点")]
        public Transform getRewardSfxParent;
        [ALHeader("领奖特效id")]
        public long getRewardSfxId;

#if NP_GAME
        //特效列表
        private List<CommonUISfxObj> _m_lSfxObjList;
#endif

        private void Awake()
        {
#if NP_GAME
            ALUGUICommon.combineBtnClick(btnDetail, _onClickDetail);
            ALUGUICommon.combineBtnClick(btnGetReward, _onClickGetReward);
#endif
        }

        private void OnDestroy()
        {
#if NP_GAME
            ALUGUICommon.uncombineBtnClick(btnDetail, _onClickDetail);
            ALUGUICommon.uncombineBtnClick(btnGetReward, _onClickGetReward);
#endif
        }

        private void OnEnable()
        {
#if NP_GAME
            WinMsg.RegisterMsg(WinMsgType.DAILY_QUEST_CHG, _onDailyQuestChg);//日常任务信息变更
            WinMsg.RegisterMsg(WinMsgType.DAILY_QUEST_GROUP_CHG, _onDailyQuestChg);//任务组变动
            WinMsg.RegisterMsg(WinMsgType.ON_SYSTEM_QUEST_INFO_CHG, _onSystemQuestInfoChg);//系统任务信息变更
            WinMsg.RegisterMsg(WinMsgType.ON_SYSTEM_QUEST_COUNT_CHG, _onSystemQuestCountChg);//系统任务计数变更
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_SYSTEM_QUEST_DETAIL_BUTTON, _onSimulateClickDetailButton);//模拟点击系统任务详情按钮
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_SYSTEM_QUEST_BAR_REWARD, _onSimulateClickGetRewardNew);//模拟点击系统任务栏领取奖励按钮
            _refresh();
#endif
        }

        private void OnDisable()
        {
#if NP_GAME
            WinMsg.UnregisterMsg(WinMsgType.DAILY_QUEST_CHG, _onDailyQuestChg);//日常任务信息变更
            WinMsg.UnregisterMsg(WinMsgType.DAILY_QUEST_GROUP_CHG, _onDailyQuestChg);//任务组变动
            WinMsg.UnregisterMsg(WinMsgType.ON_SYSTEM_QUEST_INFO_CHG, _onSystemQuestInfoChg);//系统任务信息变更
            WinMsg.UnregisterMsg(WinMsgType.ON_SYSTEM_QUEST_COUNT_CHG, _onSystemQuestCountChg);//系统任务计数变更
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_SYSTEM_QUEST_DETAIL_BUTTON, _onSimulateClickDetailButton);//模拟点击系统任务详情按钮
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_SYSTEM_QUEST_BAR_REWARD, _onSimulateClickGetRewardNew);//模拟点击系统任务栏领取奖励按钮
            if (_m_lSfxObjList != null)
            {
                foreach (CommonUISfxObj sfxObj in _m_lSfxObjList)
                {
                    sfxObj?.forceDiscard();
                }
                _m_lSfxObjList.Clear();
                _m_lSfxObjList = null;
            }
#endif
        }

#if NP_GAME
        private void _refresh()
        {
            _ISystemQuest systemQuest = NPPlayer.instance.systemQuestComp.getCurShowSystemQuest(systemQuestGroupId);
            if (systemQuest == null)
            {
                ALUGUICommon.setGameObjEnable(this.goEmptyHideList, false);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(this.goEmptyHideList, true);

                //任务名称
                string questNameShowStr = GCommon.addColorForRichText(systemQuest.showNameStr, systemQuest.canGetReward ? this.canGetRewardNameColor : this.canNotGetRewardNameColor);
                ALUGUICommon.setLabelTxt(this.txtName, questNameShowStr);

                //任务进度
                string curCountStr = GCommon.getValueFormatStr(systemQuest.processNumFormat, systemQuest.curCount);
                string targetCountStr = GCommon.getValueFormatStr(systemQuest.processNumFormat, systemQuest.targetCount);
                string questProgress = TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, curCountStr, targetCountStr);
                questProgress = GCommon.addColorForRichText(questProgress, systemQuest.canGetReward ? this.canGetRewardProgressColor : this.canNotGetRewardProgressColor);
                ALUGUICommon.setLabelTxt(this.txtProgress, questProgress);

                //任务名称及进度
                string nameAndProgressKey = string.IsNullOrEmpty(this.nameAndProgressTransKey) ? TransKeyConst.common_twoParam_str_str : this.nameAndProgressTransKey;
                ALUGUICommon.setLabelTxt(this.txtNameAndProgress, TextTranslate.instance.getLanguage(nameAndProgressKey, questNameShowStr, questProgress));

                //显隐状态
                ALUGUICommon.setGameObjEnable(this.goFinishHideList, !systemQuest.canGetReward);
                ALUGUICommon.setGameObjEnable(this.goFinishShowList, systemQuest.canGetReward);
            }
        }

        /// <summary>
        /// 领取奖励回包
        /// </summary>
        /// <param name="_itemList"></param>
        private void _retGetReward(List<NPCommon_ItemInfo> _itemList)
        {
            if (particleStartRectTransform == null || _itemList == null || _itemList.Count == 0)
                return;

            //展示粒子效果
            GCommon.showItemParticle(_itemList, particleStartRectTransform);
            //播放领奖特效
            _playGetRewardSfx();
        }

        /// <summary>
        /// 播放领奖特效
        /// </summary>
        private void _playGetRewardSfx()
        {
            if (_m_lSfxObjList == null)
                _m_lSfxObjList = new List<CommonUISfxObj>();

            //播放处理成功特效
            if (getRewardSfxId > 0 && getRewardSfxParent != null)
            {
                CommonUISfxObj newSfxObj = PlaySfxMgr.instance.playUISfx(getRewardSfxId, getRewardSfxParent);
                _m_lSfxObjList.Add(newSfxObj);
            }
        }

        //点击领取奖励
        private void _onClickGetReward(GameObject _go)
        {
            _ISystemQuest systemQuest = NPPlayer.instance.systemQuestComp.getCurShowSystemQuest(systemQuestGroupId);
            systemQuest?.dealGetReward(_retGetReward);
        }

        //点击打开详情
        private void _onClickDetail(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndSystemQuestDetail.instance, () =>
            {
                GGUIWndSystemQuestDetail.instance.showWnd();
                GGUIWndSystemQuestDetail.instance.setInfo(this.detailWndTitleKey, this.systemQuestGroupId);
            }, UINodeTagConst.C_SYSTEM_QUEST_DETAIL);
        }

        //系统任务信息变更
        private void _onSystemQuestInfoChg(params object[] _objects)
        {
            _refresh();
        }

        //系统任务计数变更
        private void _onSystemQuestCountChg(params object[] _objects)
        {
            _refresh();
        }

        //每日任务变更
        private void _onDailyQuestChg(params object[] _objects)
        {
            _refresh();
        }

        //模拟点击系统任务详情按钮
        private void _onSimulateClickDetailButton()
        {
            //触发详情按钮点击
            _onClickDetail(btnDetail);
        }

        //模拟点击系统任务栏领取奖励按钮
        private void _onSimulateClickGetRewardNew()
        {
            //触发领取奖励按钮点击
            _onClickGetReward(btnGetReward);
        }
#endif
    }
}