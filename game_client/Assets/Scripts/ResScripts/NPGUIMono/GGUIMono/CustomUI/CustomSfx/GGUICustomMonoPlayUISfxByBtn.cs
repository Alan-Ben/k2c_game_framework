using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 点击播放自定义UI特效
    /// </summary>
    public class GGUICustomMonoPlayUISfxByBtn : MonoBehaviour
    {
        [ALHeader("点击按钮")]
        public GameObject clickBtn;

        [ALHeader("特效父节点,可以使用这个来控制特效位置")]
        public Transform sfxParent;

        [ALHeader("特效id")]
        public long sfxRefId;

        [ALHeader("特效数上限")]
        public int sfxLimit;

        [ALHeader("延迟播放时间秒")]
        public float dealyTimeS;
        
#if NP_GAME
        private long _m_serialId = 0;
        private List<CommonUISfxObj> _m_sfxList;//特效对象列表

        //有效和无效的时候分别注册和注销显示对象
        private void OnEnable()
        {
            _m_serialId++;
            // 绑定按钮点击事件
            ALUGUICommon.combineBtnClick(clickBtn, _onClickBtn);
        }

        private void OnDisable()
        {
            _m_serialId++;
            // 解除绑定按钮点击事件
            ALUGUICommon.uncombineBtnClick(clickBtn, _onClickBtn);

            //清空正在播放的特效
            _clearSfx();
        }

        /// <summary>
        ///  点击响应事件 
        /// </summary>
        protected void _onClickBtn(GameObject _go)
        {
            if(null == _m_sfxList)
                _m_sfxList = new List<CommonUISfxObj>();

            long serialId = _m_serialId;
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                if(serialId != _m_serialId)
                    return;
                
                CommonUISfxObj sfxObj = PlaySfxMgr.instance.playUISfx(sfxRefId, sfxParent);
                if(null == sfxObj)
                    return;
            
                //有配置特效数量上限，且到达数量上限了,把第一个删掉，保证特效数量
                if (sfxLimit > 0 && _m_sfxList.Count >= sfxLimit)
                {
                    if(null != _m_sfxList[0])
                        _m_sfxList[0].forceDiscard();
                    _m_sfxList.RemoveAt(0);
                }
                _m_sfxList.Add(sfxObj);
            }, dealyTimeS);
        }
        
        
        /// <summary>
        /// 清空特效
        /// </summary>
        private void _clearSfx()
        {
            if (_m_sfxList != null)
            {
                foreach (CommonUISfxObj npSfxObj in _m_sfxList)
                {
                    if(null == npSfxObj)
                        continue;
                    npSfxObj.forceDiscard();
                }

                _m_sfxList.Clear();
            }
        }
#endif
    }
}