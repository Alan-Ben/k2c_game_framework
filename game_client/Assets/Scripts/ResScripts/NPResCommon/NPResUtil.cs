
using ALPackage;
using UnityEngine;

namespace GOE
{
    public static class NPResUtil
    {
        /// <summary>
        /// 是否是Prefab模式
        /// </summary>
        /// <returns></returns>
        public static bool isInPrefabStage()
        {
#if UNITY_EDITOR && UNITY_2018_3_OR_NEWER
            var stage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            return stage != null;
#else
            return false;
#endif
        }
        
        /// <summary>
        /// 根据带入文件拼凑最后位置
        /// </summary>
        /// <param name="_settingPath"></param>
        public static string makeLocalSavePath(string _settingPath)
        {
            //拼凑完整路径
#if UNITY_ANDROID && !UNITY_EDITOR
            string tmpPath = Application.persistentDataPath;
            if (null == tmpPath)
            {
                tmpPath = "/data/data" + Application.dataPath.Substring(9);
                if (tmpPath.LastIndexOf('-') == -1)
                    tmpPath = tmpPath.Substring(0, tmpPath.LastIndexOf('/')) + "/files";
                else
                    tmpPath = tmpPath.Substring(0, tmpPath.LastIndexOf('-')) + "/files";
            }
            return tmpPath + "/" + _settingPath;
#else
            return Application.persistentDataPath + "/" + _settingPath;
#endif
        }
        /// <summary>
        /// 根据相机的位置和视野尺寸，计算出对应Viewport到和_groudY平面的交点。
        /// </summary>
        public static Vector3 getGroundPosByOrthographicCamera(Rect _cameraRect, Transform _cameraTrans, float _cameraOrthographicSize, Vector2 _viewPortPos, float _groundY = 0)
        {
            return getGroundPosByOrthographicCamera(_cameraRect, _cameraTrans.position, _cameraTrans.forward, _cameraTrans.up, _cameraTrans.right, _cameraOrthographicSize, _viewPortPos, _groundY);
        }
        
        /// <summary>
        /// 根据相机的位置和视野尺寸，计算出对应Viewport到和_groudY平面的交点。
        /// </summary>
        public static Vector3 getGroundPosByOrthographicCamera(Rect _cameraRect, Vector3 _cameraPosition, Vector3 _cameraForward, Vector3 _cameraUp, Vector3 _cameraRight, float _cameraOrthographicSize, Vector2 _viewPortPos, float _groundY = 0)
        {
            Vector3 forward;
            Vector3 tmpPoint;

            float planeHeight = _cameraOrthographicSize * 2;
            //            float planeWidth = planeHeight / Screen.height * Screen.width;
            float planeWidth = planeHeight / (Screen.height * _cameraRect.height) * (Screen.width * _cameraRect.width);

            Vector3 leftBottom = _cameraPosition + (-_cameraRight) * planeWidth / 2 + (-_cameraUp) * planeHeight / 2;
            Vector3 clickPoint = leftBottom + _cameraRight * Mathf.Lerp(0, planeWidth, _viewPortPos.x) + _cameraUp * Mathf.Lerp(0, planeHeight, _viewPortPos.y);

            forward = _cameraForward;
            tmpPoint = clickPoint;

            return tmpPoint + (((tmpPoint.y - _groundY) / Mathf.Abs(forward.y)) * forward);
        }

        //计算源坐标在一个方向上与某个平面的投影坐标
        public static Vector3 getGroundPos(Vector3 _srcPos, Vector3 _forward, float _groundY = 0)
        {
            return _srcPos + (((_srcPos.y - _groundY) / Mathf.Abs(_forward.y)) * _forward);
        }
        
        /// <summary>
        /// 根据相机的位置和FOV，计算出对应Viewport到和_groudY平面的交点。
        /// </summary>
        public static Vector3 getGroundPosByPerspectiveCamera(Rect _cameraRect, Transform _cameraTrans, float _cameraFOV, Vector2 _viewPortPos, float _groundY = 0)
        {
            return getGroundPosByPerspectiveCamera(_cameraRect, _cameraTrans.position, _cameraTrans.forward, _cameraTrans.up, _cameraTrans.right, _cameraFOV, _viewPortPos, _groundY);
        }
       
        /// <summary>
        /// 根据相机的位置和FOV，计算出对应Viewport到和_groudY平面的交点。
        /// </summary>
        public static Vector3 getGroundPosByPerspectiveCamera(Rect _cameraRect, Vector3 _cameraPosition, Vector3 _cameraForward, Vector3 _cameraUp, Vector3 _cameraRight, float _cameraFOV, Vector2 _viewPortPos, float _groundY = 0)
        {
            Vector3 forward;
            Vector3 tmpPoint;
            // 计算垂直于相机视野方向，并且与相机相距100的视野平面的四个角
            float distance = 100;
            float planeHeight = Mathf.Tan(_cameraFOV / 2 * Mathf.Deg2Rad) * distance * 2;   // 用相机在高方向的开角算出高度
            //            float planeWidth = planeHeight / Screen.height * Screen.width;                  // 用屏幕比例算出宽度
            float planeWidth = planeHeight / (Screen.height * _cameraRect.height) * (Screen.width * _cameraRect.width);

            // 计算平面的四个角的坐标。先把相机的坐标推到平面的中点
            Vector3 center = _cameraForward * distance + _cameraPosition;
            Vector3 leftBottom = center + (-_cameraRight) * planeWidth / 2 + (-_cameraUp) * planeHeight / 2;
            Vector3 clickPoint = leftBottom + _cameraRight * Mathf.Lerp(0, planeWidth, _viewPortPos.x) + _cameraUp * Mathf.Lerp(0, planeHeight, _viewPortPos.y);

            forward = (clickPoint - _cameraPosition).normalized;
            tmpPoint = _cameraPosition;

            return tmpPoint + (((tmpPoint.y - _groundY) / Mathf.Abs(forward.y)) * forward);
        }

        
        public static Vector3 GetVector3(string _str)
        {
            Vector3 v3 = new Vector3();
            
            if(!string.IsNullOrEmpty(_str) && !_str.Trim().Equals(""))
            {
                string[] strs = _str.Split(new char[] { ';', ':' });
                if(strs.Length < 3)
                {
                    Debug.LogError($"数据填写错误，格式 [x;y:z] : {_str}");
                    return v3;
                }

                v3.Set(ALCommon.ParseFloat(strs[0]), ALCommon.ParseFloat(strs[1]), ALCommon.ParseFloat(strs[2]));
            }
            return v3;
        }

        /// <summary>
        /// 通用加载本地资源的方法
        /// </summary>
        /// <param name="_assetPath"></param>
        /// <param name="_objName"></param>
        /// <param name="_exname"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T loadLocalRes<T>(string _assetPath, string _objName, string _exname)
            where T : UnityEngine.Object
        {
            //声明临时变量
            T finalGo = null;
#if UNITY_EDITOR

            string judgeS = _objName + _exname;
            judgeS = judgeS.ToLowerInvariant();

            //使用GetAssetPathsFromAssetBundleAndAssetName方式加载比FindAssets匹配加载快很多，FindAssets底层逻辑是全遍历去匹配
            //直接先使用这个方式获取ab路径，不需要缓存，耗时很短可以忽略
            string[] paths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(_assetPath, _objName);
            foreach (string pathItem in paths)
            {
                if (string.IsNullOrEmpty(pathItem))
                    continue;

                finalGo = _getPathAssetsMatch<T>(pathItem, _objName, judgeS);
                //如果有对象直接返回   
                if (null != finalGo)
                    return finalGo;
            }
            
#endif
            return finalGo;
        }
        // 检查资源是否匹配，增加了检查生成的物体是否名字一致的判断，来实现加载TexturePacker的sprite的功能
        private static T _getPathAssetsMatch<T>(string _path, string _objName, string judgeS) where T : UnityEngine.Object
        {
            T finalGo = default(T);
#if UNITY_EDITOR
            if(_path.ToLowerInvariant().EndsWith(judgeS))
            {
                finalGo = UnityEditor.AssetDatabase.LoadAssetAtPath(_path, typeof(T)) as T;
                if(finalGo != null )
                    return finalGo;
            }
  
            UnityEngine.Object[] allAssets = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(_path);
            if(allAssets == null)
                return null;

            for (int i = 0; i < allAssets.Length; i++)
            {
                UnityEngine.Object obj = allAssets[i];
                if (null == obj || !(obj is T))
                    continue;

                finalGo = obj as T;
                if(finalGo != null && finalGo.name == _objName)
                    return finalGo;
            }

#endif
            return null;
        }
    }
}