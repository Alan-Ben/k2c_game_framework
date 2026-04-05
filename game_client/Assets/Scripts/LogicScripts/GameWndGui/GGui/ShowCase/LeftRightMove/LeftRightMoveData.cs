using System;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public partial class _NGGUIWndCommonShowCase_LeftRightMove<T_ITEM>
    {
        //左右移动showcase数据类
        private class LeftRightMoveData
        {
            //滑动多少距离算是移动下一个
            public float _m_nextOffsetDistance;
            //回正自动滑动时间秒
            public float _m_moveTimeS;
            
            //左边对象信息
            [NotNull]private LeftRightMoveIndex _m_left;
            //中间对象信息
            [NotNull]private LeftRightMoveIndex _m_center;
            //右边对象信息
            [NotNull]private LeftRightMoveIndex _m_right;
            //背景
            private _AShowCaseUnitInfoObj _m_bg;
            
            //屏幕宽度对应td的的偏移量
            private Vector3 _m_offset;
            //左中右的默认位置
            private Vector3 _m_leftDefaultPos;
            private Vector3 _m_centerDefaultPos;
            private Vector3 _m_rightDefaultPos;

            public LeftRightMoveData(int _leftIndex, int _centerIndex, int _rightIndex, float _nextOffsetDistance, float _moveTimeS)
            {
                _m_nextOffsetDistance = _nextOffsetDistance;
                _m_moveTimeS = _moveTimeS;
                    
                _m_left = new LeftRightMoveIndex(_leftIndex, this);
                _m_center = new LeftRightMoveIndex(_centerIndex, this);
                _m_right = new LeftRightMoveIndex(_rightIndex, this);
            }

            [NotNull] public LeftRightMoveIndex left { get { return _m_left; } set { _m_left = value; } }
            [NotNull] public LeftRightMoveIndex center { get { return _m_center; } set { _m_center = value; } }
            [NotNull] public LeftRightMoveIndex right { get { return _m_right; } set { _m_right = value; } }
            public _AShowCaseUnitInfoObj bg { get { return _m_bg; } set { _m_bg = value; } }
            public Vector3 leftDefaultPos { get { return _m_leftDefaultPos; } }
            public Vector3 centerDefaultPos { get { return _m_centerDefaultPos; } }
            public Vector3 rightDefaultPos { get { return _m_rightDefaultPos; } }
            public Vector3 offset { get { return _m_offset; } }

            public void initData(ShowcaseInfo _showcaseInfo)
            {
                if(null == _showcaseInfo)
                    return;
                
                //屏幕左下角对应场景位置
                Vector3 tdPosLeft = _showcaseInfo.showcaseCameraController.getOnlyGroundPos(Vector2.zero);
                //屏幕右下角对应场景位置
                Vector3 tdPosRight = _showcaseInfo.showcaseCameraController.getOnlyGroundPos(new Vector2(Screen.width, 0));
                //获取屏幕宽度对应场景坐标差，这个大小可以当作左右两边的位置
                _m_offset = tdPosRight - tdPosLeft;
                
                //中间位置坐标默认0，左右位置默认坐标是屏幕宽度的偏移量
                _m_centerDefaultPos = Vector3.zero;
                _m_leftDefaultPos = _m_centerDefaultPos - _m_offset;
                _m_rightDefaultPos = _m_centerDefaultPos + _m_offset;
            }
        }   
        
        //位置信息
        private class LeftRightMoveIndex
        {
            private LeftRightMoveData _m_data;
            
            //showcase位置索引
            public int index;
            //加载的对象
            public _AShowCaseUnitInfoObj infoObj;

            public LeftRightMoveIndex(int _index, LeftRightMoveData _data)
            {
                index = _index;
                _m_data = _data;
            }
            
            public Vector3 localPosition { get { return infoObj != null ? infoObj.localPosition : Vector3.zero; } }
            
            public void setInfoObj(_AShowCaseUnitInfoObj _infoObj)
            {
                infoObj = _infoObj;
            }
            
            //设置坐标
            public void setLocalPosition(Vector3 _localPosition)
            {
                if(null == infoObj)
                    return;
                
                //设置坐标
                infoObj.setLocalPosition(_localPosition);

                //获取t代表靠近中间item的比例
                float t = 1 - Mathf.InverseLerp(0f, Vector3.Distance(Vector3.zero, _m_data.offset),Vector3.Distance(Vector3.zero, _localPosition));
                t = Mathf.Clamp01(t);
                
                infoObj.setAlpha(t);
            }
        }
    }
}