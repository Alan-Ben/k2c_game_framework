using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 战斗用特效的对象，兼容旧版本
    /// </summary>
    public class Battle3DSfxObj : _BaseSfxObj<NPSfxMono>
    {
        /// <summary>
        /// 朝向，朝向需要单独记录是因为部分特效可能会在根节点通过mono自动调整朝向
        /// </summary>
        private Vector3 _m_vDirection;
        
        /** 关联actor对象 */
        // private _ASfxContainer _m_avContainer;
        //
        // /** 如果存在特效关联连接效果，特效源头actor对象 */
        // private _ASfxContainer _m_avSrcContainer;
        // /** 链接双方对象的监控任务 */
        // private SfxConnectMonitorTask _m_mtConnectMonitorTask;

        protected override void _onInit()
        {
            
        }

        protected override void _onLoadDonePlay()
        {
            //设置朝向，2D特效不设置，由脚本控制
            if (_m_vDirection.x != 0 || _m_vDirection.y != 0 || _m_vDirection.z != 0)
                setRotation(_m_vDirection);
        }

        protected override void _onDiscard()
        {
            // //终止链接任务
            // if (null != _m_mtConnectMonitorTask)
            //     _m_mtConnectMonitorTask.exit();
            //
            // //删除与对象关联
            // if (null != _m_avContainer)
            //     _m_avContainer.FollowSfxMgr.removeSfx(this);
            // _m_avContainer = null;
            //
            // //删除与对象关联
            // if (null != _m_avSrcContainer)
            //     _m_avSrcContainer.FollowSfxMgr.removeSfx(this);
            // _m_avSrcContainer = null;
        }

        public void setRotation(Vector3 _direction)
        {
            _m_vDirection = _direction;

            if (null == _m_sfxMono)
                return;

            _m_sfxMono.setRotation(_m_vDirection);
        }
        
        // /**************
        //  * 设置关联对象
        //  **/
        // public void setActorContainer(_ASfxContainer _srcActorContainer, _ASfxContainer _actorContainer)
        // {
        //     if (null == _m_srSfxRef || null == _m_srSfxRef.sfx3DRef)
        //         return;
        //
        //     _m_avSrcContainer = _srcActorContainer;
        //     _m_avContainer = _actorContainer;
        //
        //     //判断是否需要链接对象，是则进行处理
        //     if (_m_srSfxRef.sfx3DRef.connect_src && null != _m_avSrcContainer && null != _actorContainer)
        //     {
        //         //需要链接对象则需要开启任务处理双方的链接效果
        //         _m_mtConnectMonitorTask = new SfxConnectMonitorTask(this);
        //         ALMonoTaskMgr.instance.addMonoTask(_m_mtConnectMonitorTask);
        //     }
        // }
        //
        // /**************
        //  * 将特效对象指向源头
        //  **/
        // public void pointToSrc()
        // {
        //     if (null == _m_sfxMono)
        //         return;
        //     if (null == _m_sfxMono.transform || null == _m_avSrcContainer || null == _m_avContainer)
        //         return;
        //
        //     Vector3 srcPos = _m_avSrcContainer.RealPos + _m_avSrcContainer.getConnectSfxSrcOffset();
        //     Vector3 targetPos = _m_avContainer.RealPos + _m_avContainer.getConnectTargetOffset();
        //     //计算位移差
        //     Vector3 vDistance = srcPos - targetPos;
        //     float fDistance = vDistance.magnitude;
        //     //设置旋转角度
        //     _m_sfxMono.transform.rotation = Quaternion.FromToRotation(Vector3.forward, vDistance);
        //     //设置长度比例，transform.localScale返回了一个值，直接设置无效，需要setter
        //     Vector3 scale = _m_sfxMono.transform.localScale;
        //     scale.Set(fDistance, fDistance, fDistance);
        //     _m_sfxMono.transform.localScale = scale;
        // }
    }
}