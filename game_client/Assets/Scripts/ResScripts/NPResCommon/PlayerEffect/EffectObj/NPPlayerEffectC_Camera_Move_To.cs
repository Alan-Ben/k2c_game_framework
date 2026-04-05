
using System;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    //摄像头移动到某个位置（世界坐标） C_CAMERA_MOVE_TO:(x,y,z):duration
    public class NPPlayerEffectC_Camera_Move_To : _ANPPlayerEffectInfo
    {
        private Vector3 _m_vTargetVector;
        private float _m_duration;

        /************
         * 效果类型
         **/
        public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_CAMERA_MOVE_TO; } }

        public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            if (GTDSceneMain.instance.curShowScene is _ABasicAdditionMainTDScene focusableScene)
                focusableScene.moveCameraTo(_m_vTargetVector, _m_duration);
            else
                CameraController.instance.setCameraMoveController(new CameraMoveEaseController(_m_vTargetVector, _m_duration));
#endif
        }

        public static NPPlayerEffectC_Camera_Move_To readEffect(string _str)
        {
            string[] strs = _str.Split(':');
            if (strs.Length < 2)
            {
                UnityEngine.Debug.LogError("配置错误 - C_CAMERA_MOVE_TO   example: C_CAMERA_MOVE_TO:(x,y,z):duration Error Str: " + _str);
                return null;
            }

            NPPlayerEffectC_Camera_Move_To effectObj = new NPPlayerEffectC_Camera_Move_To();

            try
            {
                effectObj._m_vTargetVector = ParseVector3(strs[0]);
                effectObj._m_duration = ALCommon.ParseFloat(strs[1]);

                return effectObj;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("配置错误 - C_CAMERA_MOVE_TO   example: C_CAMERA_MOVE_TO:(x,y,z):duration Error Str: " + _str);
                return null;
            }
        }
        
        private static Vector3 ParseVector3(string _str)
        {
            _str = _str.Replace("(", "").Replace(")", "");

            string[] s = _str.Split(',');

            if (s.Length < 3)
                return Vector3.zero;

            return new Vector3(ALCommon.ParseFloat(s[0]), ALCommon.ParseFloat(s[1]), ALCommon.ParseFloat(s[2]));
        }
    }
}