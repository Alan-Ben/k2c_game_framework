using System;
using UnityEngine;

namespace GOE
{
    public interface _ITravelParkable
    {
        void setIsParking(bool _isParking, bool _needPlayAnimation, Action _onPlayDone = null);
        
        /// <summary>
        /// 获取飞机停靠位置
        /// </summary>
        /// <returns></returns>
        Vector3 getAircraftParkingPos();
    }
}