using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 按钮集合，且只可选中一个按钮的自定义UI，选中按钮后触发配置effect
    /// </summary>
    public class NPGGUICustomMonoSingleSelectBtnGroup : MonoBehaviour
    {
        //按钮列表
        public NPGGUICustomMonoSingleSelectBtn[] btnList;

        [ALHeader("取消选中对象时触发的效果")]
        public string disSelectEffectStr;

        private bool _m_bIsInitedDisSel = false;
        private List<NPPlayerEffectSerializeInfo> _m_lDisSelPlayerEffectList;

        //选中的对象
        private NPGGUICustomMonoSingleSelectBtn _m_bSelectedBtn = null;

        public List<NPPlayerEffectSerializeInfo> disSelPlayerEffectList
        {
            get
            {
                if (_m_bIsInitedDisSel)
                    return _m_lDisSelPlayerEffectList;

                _init();

                return _m_lDisSelPlayerEffectList;
            }
        }

        private void Start()
        {
            //逐个注册点击处理
            for (int i = 0; i < btnList.Length; i++)
            {
                if (null == btnList[i])
                    continue;

                //设置父对象
                btnList[i].initParent(this);
                //设置父对象
                ALUGUICommon.setGameObjEnable(btnList[i].selectTag, btnList[i] == _m_bSelectedBtn);
            }
        }

        //无效时设置无对象被选中
        private void OnDestroy()
        {
            _m_bSelectedBtn = null;
        }

        /// <summary>
        /// 处理选中以及取消选中的处理
        /// </summary>
        public void dealDisSelect()
        {
            NPPlayerEffectSerializeInfo.dealEffect(disSelPlayerEffectList, null);
        }

        //初始操作
        protected void _init()
        {
            _m_lDisSelPlayerEffectList = NPPlayerEffectSerializeInfo.readEffectList(disSelectEffectStr);
            _m_bIsInitedDisSel = true;
        }

        /// <summary>
        /// 设置选中对象
        /// </summary>
        /// <param name="_btn"></param>
        public void setSelectObj(NPGGUICustomMonoSingleSelectBtn _btn)
        {
            if (_m_bSelectedBtn == _btn)
            {
                //取消选中
                _m_bSelectedBtn = null;
            }
            else
            {
                _m_bSelectedBtn = _btn;
            }

            //逐个注册点击处理
            for (int i = 0; i < btnList.Length; i++)
            {
                if (null == btnList[i])
                    continue;

                //设置父对象
                ALUGUICommon.setGameObjEnable(btnList[i].selectTag, btnList[i] == _m_bSelectedBtn);
            }

            //处理点击操作或取消选择操作
            if (null != _m_bSelectedBtn)
                _m_bSelectedBtn.dealSelect();
            else
                dealDisSelect();
        }
        /// <summary>
        /// 设置显示上选中的对象
        /// </summary>
        /// <param name="_btn"></param>
        public void setViewSelectObj(NPGGUICustomMonoSingleSelectBtn _btn)
        {
            _m_bSelectedBtn = _btn;

            //逐个注册点击处理
            for (int i = 0; i < btnList.Length; i++)
            {
                if (null == btnList[i])
                    continue;

                //设置父对象
                ALUGUICommon.setGameObjEnable(btnList[i].selectTag, btnList[i] == _m_bSelectedBtn);
            }
        }
    }
}

