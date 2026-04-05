using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

/********************
 * 浮点数过度的控制与计算对象
 **/
public class ALFloatFadeController
{
    /** 开始数值 */
    private float _m_fBinValue;
    /** 目标数值 */
    private float _m_fTargetValue;

    /** 开始运动的时间标记 */
    private float _m_fStartTimeTag;
    /** 初始化的速度 */
    private float _m_fSrcSpeed;
    /** 过渡总时间 */
    private float _m_fTotalTime;
    /** 过渡过程的加速时间 */
    private float _m_fFirstAccTime;
    /** 第一阶段的加速度 */
    private float _m_fFirstAccSpeed;
    /** 第二阶段的加速度 */
    private float _m_fSecAccSpeed;

    public ALFloatFadeController(float _startNum, float _targetNum, ALFloatFadeTimeInfo _variableInfo)
    {
        _m_fBinValue = _startNum;
        _m_fTargetValue = _targetNum;

        _m_fStartTimeTag = Time.time;

        _m_fSrcSpeed = _variableInfo.srcSpeed;
        _m_fTotalTime = _variableInfo.totalTime;
        _m_fFirstAccTime = _variableInfo.firstAccSpeedTime;

        float moveDis = _m_fTargetValue - _m_fBinValue;
        float secAccTime = _m_fTotalTime - _m_fFirstAccTime;
        //计算加速度数值
        if (0.0001f > secAccTime)
        {
            //此时为加速运动
            _m_fSecAccSpeed = 0;
            _m_fFirstAccSpeed = 2 * (moveDis - (_m_fSrcSpeed * _m_fTotalTime)) / (_m_fTotalTime * _m_fTotalTime);
        }
        else if (0.0001f > _m_fFirstAccTime)
        {
            //此时为减速运动，初始速度需要重新计算
            //先计算加速度
            _m_fFirstAccSpeed = 0;
            _m_fSecAccSpeed = -(2 * moveDis) / (_m_fTotalTime * _m_fTotalTime);
            _m_fSrcSpeed = -_m_fSecAccSpeed * _m_fTotalTime;
        }
        else
        {
            _m_fSecAccSpeed = ((_m_fSrcSpeed * _m_fFirstAccTime) - (2 * moveDis)) / (_m_fTotalTime * secAccTime);
            _m_fFirstAccSpeed = -((_m_fSecAccSpeed * secAccTime) + _m_fSrcSpeed) / _m_fFirstAccTime;
        }
    }
    public ALFloatFadeController(float _startNum, float _targetNum, float _srcSpeed, float _totalTime, float _firstAccSpeedTime)
    {
        _m_fBinValue = _startNum;
        _m_fTargetValue = _targetNum;

        _m_fStartTimeTag = Time.time;

        _m_fSrcSpeed = _srcSpeed;
        _m_fTotalTime = _totalTime;
        _m_fFirstAccTime = _firstAccSpeedTime;

        float moveDis = _m_fTargetValue - _m_fBinValue;
        float secAccTime = _m_fTotalTime - _m_fFirstAccTime;
        //计算加速度数值
        if (0.0001f > secAccTime)
        {
            //此时为加速运动
            _m_fSecAccSpeed = 0;
            _m_fFirstAccSpeed = 2 * (moveDis - (_m_fSrcSpeed * _m_fTotalTime)) / (_m_fTotalTime * _m_fTotalTime);
        }
        else if (0.0001f > _m_fFirstAccTime)
        {
            //此时为减速运动，初始速度需要重新计算
            //先计算加速度
            _m_fFirstAccSpeed = 0;
            _m_fSecAccSpeed = -(2 * moveDis) / (_m_fTotalTime * _m_fTotalTime);
            _m_fSrcSpeed = -_m_fSecAccSpeed * _m_fTotalTime;
        }
        else
        {
            _m_fSecAccSpeed = ((_m_fSrcSpeed * _m_fFirstAccTime) - (2 * moveDis)) / (_m_fTotalTime * secAccTime);
            _m_fFirstAccSpeed = -((_m_fSecAccSpeed * secAccTime) + _m_fSrcSpeed) / _m_fFirstAccTime;
        }
    }
    public ALFloatFadeController(float _startNum, float _targetNum, ALFloatFadeTimeInfo _variableInfo, float _startTime)
    {
        _m_fBinValue = _startNum;
        _m_fTargetValue = _targetNum;

        _m_fStartTimeTag = _startTime;

        _m_fSrcSpeed = _variableInfo.srcSpeed;
        _m_fTotalTime = _variableInfo.totalTime;
        _m_fFirstAccTime = _variableInfo.firstAccSpeedTime;

        float moveDis = _m_fTargetValue - _m_fBinValue;
        float secAccTime = _m_fTotalTime - _m_fFirstAccTime;
        //计算加速度数值
        if (0.0001f > secAccTime)
        {
            //此时为加速运动
            _m_fSecAccSpeed = 0;
            _m_fFirstAccSpeed = 2 * (moveDis - (_m_fSrcSpeed * _m_fTotalTime)) / (_m_fTotalTime * _m_fTotalTime);
        }
        else if (0.0001f > _m_fFirstAccTime)
        {
            //此时为减速运动，初始速度需要重新计算
            //先计算加速度
            _m_fFirstAccSpeed = 0;
            _m_fSecAccSpeed = -(2 * moveDis) / (_m_fTotalTime * _m_fTotalTime);
            _m_fSrcSpeed = -_m_fSecAccSpeed * _m_fTotalTime;
        }
        else
        {
            _m_fSecAccSpeed = ((_m_fSrcSpeed * _m_fFirstAccTime) - (2 * moveDis)) / (_m_fTotalTime * secAccTime);
            _m_fFirstAccSpeed = -((_m_fSecAccSpeed * secAccTime) + _m_fSrcSpeed) / _m_fFirstAccTime;
        }
    }
    public ALFloatFadeController(float _startNum, float _targetNum, float _srcSpeed, float _totalTime, float _firstAccSpeedTime, float _startTime)
    {
        _m_fBinValue = _startNum;
        _m_fTargetValue = _targetNum;

        _m_fStartTimeTag = _startTime;

        _m_fSrcSpeed = _srcSpeed;
        _m_fTotalTime = _totalTime;
        _m_fFirstAccTime = _firstAccSpeedTime;

        float moveDis = _m_fTargetValue - _m_fBinValue;
        float secAccTime = _m_fTotalTime - _m_fFirstAccTime;
        //计算加速度数值
        if (0.0001f > secAccTime)
        {
            //此时为加速运动
            _m_fSecAccSpeed = 0;
            _m_fFirstAccSpeed = 2 * (moveDis - (_m_fSrcSpeed * _m_fTotalTime)) / (_m_fTotalTime * _m_fTotalTime);
        }
        else if (0.0001f > _m_fFirstAccTime)
        {
            //此时为减速运动，初始速度需要重新计算
            //先计算加速度
            _m_fFirstAccSpeed = 0;
            _m_fSecAccSpeed = -(2 * moveDis) / (_m_fTotalTime * _m_fTotalTime);
            _m_fSrcSpeed = -_m_fSecAccSpeed * _m_fTotalTime;
        }
        else
        {
            _m_fSecAccSpeed = ((_m_fSrcSpeed * _m_fFirstAccTime) - (2 * moveDis)) / (_m_fTotalTime * secAccTime);
            _m_fFirstAccSpeed = -((_m_fSecAccSpeed * secAccTime) + _m_fSrcSpeed) / _m_fFirstAccTime;
        }
    }

    public float targetValue { get { return _m_fTargetValue; } }
    public bool isDone 
    {
        get 
        {
            float activeTime = Time.time - _m_fStartTimeTag;
            if (activeTime >= _m_fTotalTime)
                return true;

            return false;
        }
    }

    /*****************
     * 重新初始化参数
     **/
    public void resetVariables(float _startNum, float _targetNum, float _srcSpeed, float _totalTime, float _firstAccSpeedTime)
    {
        _m_fBinValue = _startNum;
        _m_fTargetValue = _targetNum;

        _m_fStartTimeTag = Time.time;

        _m_fSrcSpeed = _srcSpeed;
        _m_fTotalTime = _totalTime;
        _m_fFirstAccTime = _firstAccSpeedTime;

        float moveDis = _m_fTargetValue - _m_fBinValue;
        float secAccTime = _m_fTotalTime - _m_fFirstAccTime;
        //计算加速度数值
        if (0.0001f > secAccTime)
        {
            //此时为加速运动
            _m_fSecAccSpeed = 0;
            _m_fFirstAccSpeed = 2 * (moveDis - (_m_fSrcSpeed * _m_fTotalTime)) / (_m_fTotalTime * _m_fTotalTime);
        }
        else if (0.0001f > _m_fFirstAccTime)
        {
            //此时为减速运动，初始速度需要重新计算
            //先计算加速度
            _m_fFirstAccSpeed = 0;
            _m_fSecAccSpeed = -(2 * moveDis) / (_m_fTotalTime * _m_fTotalTime);
            _m_fSrcSpeed = -_m_fSecAccSpeed * _m_fTotalTime;
        }
        else
        {
            _m_fSecAccSpeed = ((_m_fSrcSpeed * _m_fFirstAccTime) - (2 * moveDis)) / (_m_fTotalTime * secAccTime);
            _m_fFirstAccSpeed = -((_m_fSecAccSpeed * secAccTime) + _m_fSrcSpeed) / _m_fFirstAccTime;
        }
    }

    /*******************
     * 获取本过渡对象当前的数值信息
     **/
    public float curValue
    {
        get
        {
            float activeTime = Time.time - _m_fStartTimeTag;
            if (activeTime >= _m_fTotalTime)
                return _m_fTargetValue;

            if (activeTime < _m_fFirstAccTime)
            {
                //当运动轨迹在第一加速阶段时，使用加速度进行计算
                float dis = (_m_fSrcSpeed + (0.5f * _m_fFirstAccSpeed * activeTime)) * activeTime;
                //返回累计结果
                return (_m_fBinValue + dis);
            }

            //不在第一阶段则在第二阶段
            //此时使用反向加速计算，更易得出结果
            float timeLeft = _m_fTotalTime - activeTime;
            float deltaDis = 0.5f * _m_fSecAccSpeed * timeLeft * timeLeft;
            //相减获得实际位置信息
            return (_m_fTargetValue + deltaDis);
        }
    }

    /********************
     * 获取本过渡对象当前的渐变速度信息
     **/
    public float curFadeSpeed
    {
        get
        {
            float activeTime = Time.time - _m_fStartTimeTag;
            if (activeTime >= _m_fTotalTime)
                return 0;

            if (activeTime < _m_fFirstAccTime)
            {
                //当运动轨迹在第一加速阶段时，使用加速度进行计算
                float addSpeed = _m_fFirstAccSpeed * activeTime;
                //返回累计结果
                return (_m_fSrcSpeed + addSpeed);
            }

            //不在第一阶段则在第二阶段
            //此时使用反向加速计算，更易得出结果
            float timeLeft = _m_fTotalTime - activeTime;
            float deltaSpeed = _m_fSecAccSpeed * timeLeft;
            //相减获得实际位置信息
            return -deltaSpeed;
        }
    }

}
