using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// showcase对象对单位的控制
    /// </summary>
    public partial class ShowcaseInfo
    {
        /// <summary>
        /// 获取加载出来的单位信息
        /// </summary>
        /// <returns></returns>
        public _AShowCaseUnitInfoObj getUnitInfo(int _unitIndex)
        {
            if (null == _m_unitIndexList || _m_unitIndexList.Length == 0)
                return null;

            if (null == _m_unitIndexList[_unitIndex])
                return null;
            
            return _m_unitIndexList[_unitIndex].caseUnitInfoObj;
        }
        
        /// <summary>
        /// 输入要旋转的角度（degree），旋转整个单位，绕着 Y 轴旋转
        /// </summary>
        public void rotateUnit(int _unitIndex, float _angle)
        {
            _AShowCaseUnitInfoObj showCaseUnitInfo = getUnitInfo(_unitIndex);

            if(null != showCaseUnitInfo)
                showCaseUnitInfo.rotateUnit(_angle); 
        }

        /// <summary>
        /// 重置旋转角度
        /// </summary>
        public void resetRotationUnit(int _unitIndex)
        {
            _AShowCaseUnitInfoObj showCaseUnitInfo = getUnitInfo(_unitIndex);

            if(null != showCaseUnitInfo)
                showCaseUnitInfo.resetRotationUnit(); 
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="_unitIndex"></param>
        /// <param name="_aniName"></param>
        /// <param name="_crossFadeTime"></param>
        public void playAnim(int _unitIndex, string _aniName)
        {
            _AShowCaseUnitInfoObj showCaseUnitInfo = getUnitInfo(_unitIndex);

            if(null != showCaseUnitInfo)
                showCaseUnitInfo.playAnim(_aniName); 
        }

        /// <summary>
        /// 设置动画触发器
        /// </summary>
        /// <param name="_unitIndex"></param>
        /// <param name="_name"></param>
        public void setAnimTrigget(int _unitIndex, string _name)
        {
            _AShowCaseUnitInfoObj showCaseUnitInfo = getUnitInfo(_unitIndex);

            if (null != showCaseUnitInfo)
                showCaseUnitInfo.setAnimTrigger(_name);
        }

        public void setSpeed(int _unitIndex, float _speed)
        {
            _AShowCaseUnitInfoObj showCaseUnitInfo = getUnitInfo(_unitIndex);

            if (null != showCaseUnitInfo)
                showCaseUnitInfo.setSpeed(_speed);
        }
        
        //强制切换动画
        public void forceSetAni(int _unitIndex, string _aniName)
        {
            _AShowCaseUnitInfoObj showCaseUnitInfo = getUnitInfo(_unitIndex);

            if (null != showCaseUnitInfo)
                showCaseUnitInfo.forceSetAni(_aniName);
        }

        /// <summary>
        /// 是否正在播放动画
        /// </summary>
        /// <param name="_unitIndex"></param>
        /// <param name="_aniName"></param>
        /// <returns></returns>
        public bool isPlayingAni(int _unitIndex, string _aniName)
        {
            _AShowCaseUnitInfoObj showCaseUnitInfo = getUnitInfo(_unitIndex);

            return showCaseUnitInfo?.isPlayingAni(_aniName) ?? false;
        }
        
        /// <summary>
        /// 播放特效
        /// </summary>
        /// <param name="_unitIndex"></param>
        /// <param name="_aniName"></param>
        /// <param name="_crossFadeTime"></param>
        public void playSfx(int _unitIndex, long _sfxId)
        {
            _AShowCaseUnitInfoObj showCaseUnitInfo = getUnitInfo(_unitIndex);

            if(null != showCaseUnitInfo)
                showCaseUnitInfo.playSfx(_sfxId);
        }
        
        /// <summary>
        /// 开启变暗效果
        /// </summary>
        public void maskShowMaterial(int _unitIndex, bool _isGray)
        {
            _AShowCaseUnitInfoObj showCaseUnitInfo = getUnitInfo(_unitIndex);

            if(null != showCaseUnitInfo)
                showCaseUnitInfo.maskShowMaterial(_isGray); 
        }

        public void darkMaterial(int _unitIndex, bool _isDark)
        {
            _AShowCaseUnitInfoObj showCaseUnitInfo = getUnitInfo(_unitIndex);

            if (null != showCaseUnitInfo)
                showCaseUnitInfo.darkMaterial(_isDark);
        }
        
        /// <summary>
        /// 显隐对象
        /// </summary>
        public void enableRender(int _unitIndex, bool _isEnable)
        {
            _AShowCaseUnitInfoObj showCaseUnitInfo = getUnitInfo(_unitIndex);

            if(null != showCaseUnitInfo)
                showCaseUnitInfo.enableRender(_isEnable);
        }

        /// <summary>
        /// 设置叠加颜色及强度
        /// </summary>
        /// <param name="_unitIndex"></param>
        /// <param name="_color"></param>
        /// <param name="_value"></param>
        public void addColorAndIntensityMaterial(int _unitIndex, Color _color, float _value)
        {
            _AShowCaseUnitInfoObj showCaseUnitInfo = getUnitInfo(_unitIndex);

            if (null != showCaseUnitInfo)
                showCaseUnitInfo.addColorAndIntensityMaterial(_color, _value);
        }

        /// <summary>
        /// 设置DepthAlphaCutoff
        /// </summary>
        /// <param name="_unitIndex"></param>
        /// <param name="_value"></param>
        public void setDepthAlphaCutoff(int _unitIndex, float _value)
        {
            _AShowCaseUnitInfoObj showCaseUnitInfo = getUnitInfo(_unitIndex);

            if (null != showCaseUnitInfo)
                showCaseUnitInfo.setDepthAlphaCutoff(_value);
        }
        
        public void enableMagicaCloth(int _unitIndex, bool _isEnable)
        {
            _AShowCaseUnitInfoObj showCaseUnitInfo = getUnitInfo(_unitIndex);

            if (null != showCaseUnitInfo)
                showCaseUnitInfo.enableMagicaCloth(_isEnable);
        }
    }
}