using ALPackage;
using UnityEngine;

namespace GOE
{
    public enum ESFXModelType
    {
        MODEL_2D,
        MODEL_3D,
    }
    
    /// <summary>
    /// 显示时，播放特效 隐藏时，回收特效
    /// </summary>
    public class GGUICustomMonoPlaySfx : MonoBehaviour
    {
        [ALHeader("特效表id，播放时，挂在这个物体下面")]
        public long sfx_id;

        [ALHeader("父节点的类型，理论上ui上挂就是2d，场景上3d")]
        public ESFXModelType model_type;
        
#if NP_GAME
        private _ISfxObj _m_sfxObj;
        //是否需要检测
        private bool _m_bNeedCheck = false;

        //有效和无效的时候分别注册和注销显示对象
        private void OnEnable()
        {
            _m_bNeedCheck = true;

            //到管理对象中进行处理
            ALCommonActionMonoTask.addNextFrameTask(_check);
        }

        private void OnDisable()
        {
            _m_bNeedCheck = true;

            //到管理对象中进行处理
            ALCommonActionMonoTask.addNextFrameTask(_check);
        }

        private void OnDestroy()
        {
            _m_bNeedCheck = true;
            //直接检测
            _check();
        }

        protected void _check()
        {
            if (!_m_bNeedCheck || null == this || null == gameObject || sfx_id <= 0)
            {
                return;
            }

            _m_bNeedCheck = false;
            
            //先释放一次
            if(_m_sfxObj != null)
            {
                _m_sfxObj.forceDiscard();
            }
            
            if (gameObject.activeInHierarchy)
            {
                if (model_type == ESFXModelType.MODEL_2D)
                {
                    _m_sfxObj = PlaySfxMgr.instance.playUISfx(sfx_id, transform);
                }
                else
                {
                    _m_sfxObj = PlaySfxMgr.instance.playTDSfx(sfx_id, transform);
                }
            }
            else
            {
                if(_m_sfxObj != null)
                {
                    _m_sfxObj.forceDiscard();
                }
                //无效时不做处理
            }
        }
#endif
    }
}