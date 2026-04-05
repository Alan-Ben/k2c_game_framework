using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 带一些基础功能的showcase单位对象，如旋转
    /// </summary>
    public abstract class _AShowCaseUnitInfoCommonObj :_AShowCaseUnitInfoObj
    {
        //默认旋转角
        private Vector3 _m_defaultRotation = Vector3.zero;
        //当前的旋转角
        private float _m_rotationAngle;

        /// <summary>
        /// 旋转单位角度
        /// </summary>
        /// <param name="_angle"></param>
        public override void rotateUnit(float _angle)
        {
            if(null == _m_controlUnit || null == _m_controlUnit.transform)
                return;
            
            _m_rotationAngle += _angle;
            _m_rotationAngle = Mathf.Repeat(_m_rotationAngle, 360);
            _m_controlUnit.transform.localRotation = Quaternion.Euler(_m_defaultRotation.x, _m_rotationAngle, _m_defaultRotation.z);
        }
        
        /// <summary>
        /// 重置旋转角度
        /// </summary>
        public override void resetRotationUnit()
        {
            if(null == _m_controlUnit || null == _m_controlUnit.transform)
                return;

            _m_rotationAngle = _m_defaultRotation.y;
            _m_controlUnit.transform.localRotation = Quaternion.identity;
        }

        protected override void _onInit(Transform _parent)
        {
            base._onInit(_parent);

            //设置默认旋转角
            if (_m_controlUnit != null) 
                _m_defaultRotation = _m_controlUnit.transform.localRotation.eulerAngles;
            //旋转角度重置
            _m_rotationAngle = _m_defaultRotation.y;
        }
        
        protected override void _onDiscard()
        {
            //旋转角度重置
            _m_rotationAngle = _m_defaultRotation.y;
            
            base._onDiscard();
        }
    }
}