using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoStageGoalOverviewGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("不同任务状态下显示的内容"),
         ALInfo("分别是：已领奖、已结束、当前进行中、未解锁、未公开")]
        public Color rewardGotTitleColor = Color.white;
        public Color rewardGotDescColor = Color.white;
        public List<GameObject> listRewardGotShow;
        public Color completedTitleColor = Color.white;
        public Color completedDescColor = Color.white;
        public List<GameObject> listCompletedShow;
        public Color currentTitleColor = Color.black;
        public Color currentDescColor = Color.white;
        public List<GameObject> listCurrentShow;
        public Color lockedTitleColor = Color.gray;
        public Color lockedDescColor = Color.gray;
        public List<GameObject> listLockedShow;
        public Color privateTitleColor = Color.gray;
        public Color privateDescColor = Color.gray;
        public List<GameObject> listPrivateShow;

        [ALHeader("不同位置显示的内容"),
         ALInfo("分别是：第一个、最后一个、中间的")]
        public List<GameObject> listFirstShow;
        public List<GameObject> listLastShow;
        public List<GameObject> listMiddleShow;

        [ALHeader("当前任务的进度")]
        public Slider sldProgress;
        [ALHeader("第几个任务、标题、描述")]
        public Text txtTaskNum;
        public Text txtTitle;
        public Text txtDesc;
        [ALHeader("未公开任务序号")]
        public Text txtPrivateNum;
        [ALHeader("完成结束的时间")]
        public Text txtCompletedTime;
        [ALHeader("任务的图片")]
        public RawImage imgTex;
        [ALHeader("播放任务结束时的对话按钮")]
        public GameObject btnPlayDialogue;
        [ALHeader("跳转到隔壁页签的按钮")]
        public GameObject btnJumpTo;
        [ALHeader("没有对话时要隐藏的内容")]
        public List<GameObject> listNoDialogueHide;
        [ALHeader("横着的进度条和小阶段的点点")]
        public Slider sldProgress2;
        public Text txtProgress2;
        public GGUIMonoStageGoalOverviewSmallStepPointContainer monoSmallStepPointContainer;
        [ALHeader("奖励列表和领奖按钮")]
        public GGUIMonoCommonRewardContainer monoRewardContainer;
        public GameObject btnGetReward;
        [ALHeader("解锁功能列表")]
        public NPGGUIMonoCommonItemContainer monoUnlockContainer;
        [ALHeader("需要先领取小阶段奖励的提示")]
        public GameObject goNeedGetSmallStageRewardFirstTip;
        [ALHeader("解锁动画")]
        public CommonAnimationSingleInfo aniUnlock;
        [ALHeader("完成动画")]
        public CommonAnimationSingleInfo aniFinish;


        public void setState(long _itemStep, long _currentStep, bool _isAllDone, bool _isPrivate, ECommonRewardType _getRewardType)
        {
            ALUGUICommon.setGameObjEnable(listCompletedShow, false);
            ALUGUICommon.setGameObjEnable(listCurrentShow, false);
            ALUGUICommon.setGameObjEnable(listLockedShow, false);
            ALUGUICommon.setGameObjEnable(listPrivateShow, false);
            ALUGUICommon.setGameObjEnable(listRewardGotShow, false);

            if (_isPrivate)
            {
                setTextColor(privateTitleColor,privateDescColor);

                ALUGUICommon.setGameObjEnable(listPrivateShow, true);
            }
            else if (_isAllDone || _itemStep < _currentStep)
            {
                if (_getRewardType == ECommonRewardType.HAS_GET_REWARD)
                {
                    setTextColor(rewardGotTitleColor,rewardGotDescColor);
                    ALUGUICommon.setGameObjEnable(listRewardGotShow, true);
                }
                else
                {
                    setTextColor(completedTitleColor,completedDescColor);
                    ALUGUICommon.setGameObjEnable(listCompletedShow, true);
                }
            }
            else if (_itemStep == _currentStep)
            {
                if (_getRewardType == ECommonRewardType.CAN_GET_REWARD)
                {
                    setTextColor(completedTitleColor,completedDescColor);
                    ALUGUICommon.setGameObjEnable(listCompletedShow, true);
                }
                else
                {
                    setTextColor(currentTitleColor,currentDescColor);
                    ALUGUICommon.setGameObjEnable(listCurrentShow, true);
                }
            }
            else
            {
                setTextColor(lockedTitleColor,lockedDescColor);
                ALUGUICommon.setGameObjEnable(listLockedShow, true);
            }
        }
        public void setIsFirst(bool _isFirst, bool _isLast)
        {
            ALUGUICommon.setGameObjEnable(listFirstShow, false);
            ALUGUICommon.setGameObjEnable(listLastShow, false);
            ALUGUICommon.setGameObjEnable(listMiddleShow, false);
            
            if (_isFirst)
                ALUGUICommon.setGameObjEnable(listFirstShow, true);
            else if (_isLast)
                ALUGUICommon.setGameObjEnable(listLastShow, true);
            else
                ALUGUICommon.setGameObjEnable(listMiddleShow, true);
        }
        public void setDialogue(bool _hasDialogue)
        {
            ALUGUICommon.setGameObjEnable(listNoDialogueHide, _hasDialogue);
        }
        public void setTextColor(Color _titleColor, Color _descColor)
        {
            ALUGUICommon.setUIObjColor(txtTitle, _titleColor);
            ALUGUICommon.setUIObjColor(txtDesc, _descColor);
        }
    }
}