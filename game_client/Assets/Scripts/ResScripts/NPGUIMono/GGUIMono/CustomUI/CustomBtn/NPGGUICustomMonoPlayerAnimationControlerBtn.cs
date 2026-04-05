using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

#if NP_GAME
using GOE;
#endif

namespace GOE
{
    //单个效果节点的信息
    [System.Serializable]
    public class NPGGUIMonoPlayerAnimationControlerInfo
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
    /// 动画播放按钮脚本
    /// </summary>
    public class NPGGUICustomMonoPlayerAnimationControlerBtn : _AALBasicUIWndMono
    {
        public GameObject Btn; // 按钮

        //控制的动画播放对象
        public Animation playAnimationObj;

        //需要处理的效果队列
        public List<NPGGUIMonoPlayerAnimationControlerInfo> playInfoList;
        //当没有动画播放的时候的提示信息翻译索引
        public string noticeTxt;

        private void Start()
        {
            // 绑定按钮点击事件
            ALUGUICommon.combineBtnClick(Btn, _onClickBtn);
        }

        /** 点击响应事件 */
        protected void _onClickBtn(GameObject _go)
        {
            NPGGUIMonoPlayerAnimationControlerInfo info = null;
            for (int i = 0; i < playInfoList.Count; i++)
            {
                info = playInfoList[i];
                if (null == info)
                    continue;

                if (info.playAni(playAnimationObj))
                {
                    return;
                }
            }

            //调用到这里表示没有播放成功的动画，此时展示展示提示
            if (!string.IsNullOrEmpty(noticeTxt))
            {
#if NP_GAME
                NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(noticeTxt), TextTranslate.instance.getLanguage(TransKeyConst.ok), null);
#endif
                return;
            }
        }

    }
}

