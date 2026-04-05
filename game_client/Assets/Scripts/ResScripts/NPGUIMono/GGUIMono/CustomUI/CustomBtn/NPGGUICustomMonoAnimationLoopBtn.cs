using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    //单个效果节点的信息
    [System.Serializable]
    public class NPGGUIMonoAnimationLoopInfo
    {
        public string animationName;//播放动作名称
        public PlayMode playMode;//播放模式
        public string clickConditionStr;//点击的判断条件字符串

        //效果数据
        private bool _m_bIsInited = false;
        private NPPlayerConditionGroupObj _m_lClickCondition;
        public NPPlayerConditionGroupObj clickCondition
        {
            get
            {
                if (_m_bIsInited)
                    return _m_lClickCondition;

                _init();

                return _m_lClickCondition;
            }
        }

        //初始操作
        protected void _init()
        {
            _m_lClickCondition = NPPlayerConditionGroupObj.readConditionGroupList(clickConditionStr, "custAniB");
            _m_bIsInited = true;
        }

        /** 根据条件播放动作 */
        public bool playAni(Animation _animation)
        {
            //调用点击处理
            //判断条件  当提示信息为空时不弹
            if (null == _animation || !clickCondition.IsEnable(null))
            {
                return false;
            }

            //播放动作
            _animation.Play(animationName, playMode);

            return true;
        }
    }


    /// <summary>
    /// 动画循环播放按钮脚本
    /// </summary>
    public class NPGGUICustomMonoAnimationLoopBtn : _AALBasicUIWndMono
    {
        public GameObject Btn; // 按钮

        //控制的动画播放对象
        public Animation playAnimationObj;

        //需要处理的效果队列
        public List<NPGGUIMonoAnimationLoopInfo> playInfoList;
        //当没有动画播放的时候的提示信息翻译索引
        public string noticeTxt;

        //当前播放的索引
        private int _m_iCurIdx;

        private void Start()
        {
            // 绑定按钮点击事件
            ALUGUICommon.combineBtnClick(Btn, _onClickBtn);
            _m_iCurIdx = 0;
        }

        /** 点击响应事件 */
        protected void _onClickBtn(GameObject _go)
        {
            NPGGUIMonoAnimationLoopInfo info = null;
            int nextIdx = _m_iCurIdx;
            do
            {
                nextIdx++;

                if (nextIdx >= playInfoList.Count)
                    nextIdx = 0;

                if (nextIdx == _m_iCurIdx)
                    break;

                //播放
                info = playInfoList[nextIdx];
                if (null == info)
                    continue;

                if (info.playAni(playAnimationObj))
                {
                    //设置索引
                    _m_iCurIdx = nextIdx;
                    return;
                }
            } while (true);
        }

    }
}

