using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// RT摄像头管理器
    /// </summary>
    public class RTCameraMgr
    {
        private static RTCameraMgr _g_instance = new RTCameraMgr();
        [NotNull]
        public static RTCameraMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new RTCameraMgr();
                return _g_instance;
            }
        }

        //需要警告的数量
        private int _m_iIsWarningCount = 5;
        
        /** 总的缓存队列 */
        [NotNull]private List<RTCameraController> _m_lTotalCacheList = new List<RTCameraController>();
        /** 还可使用的缓存队列 */
        [NotNull]private List<RTCameraController> _m_lEnableCacheList = new List<RTCameraController>();
        /** 已经被使用的缓存队列 */
        [NotNull]private List<RTCameraController> _m_lUsedItemList = new List<RTCameraController>();
        
        /// <summary>
        /// 获取一个有效的对象
        /// </summary>
        /// <returns></returns>
        public RTCameraController popAvailableInfo()
        {
            if (_m_lEnableCacheList.Count <= 0)
            {
                //根据增量创建
                _addCache();
                
                //增加特效超出上限的警告
                if (_m_lTotalCacheList.Count > _m_iIsWarningCount)
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError($"RTCameraController超过上限：{_m_iIsWarningCount}");
#endif
                }
            }

            //取出最后一个对象
            RTCameraController firstItem = _m_lEnableCacheList[_m_lEnableCacheList.Count - 1];
            _m_lEnableCacheList.RemoveAt(_m_lEnableCacheList.Count - 1);
            //放入使用队列
            _m_lUsedItemList.Add(firstItem);
#if UNITY_EDITOR
            if (firstItem == null)
            {
                Debug.LogError($"RTCameraController中拿到的对象为空，外面应该有哪里持有的这个引用并且销毁了对象，请认真检查!");
            }
#endif
            return firstItem;
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        public void pushBackInfo(RTCameraController _rtCameraController)
        {
            if (null == _rtCameraController)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("RTCameraController中pushBack了一个空对象，请检查逻辑代码是否有问题");
#endif
                return;
            }
            
            //从使用队列删除 判断是否成功删除，未成功删除则不处理
            if(!_m_lUsedItemList.Remove(_rtCameraController))
                return;
            
            //设置对象无效
            _resetItem(_rtCameraController);
            
            //放入缓存队列
            _m_lEnableCacheList.Add(_rtCameraController);
        }
        
        //增加一个缓存
        private void _addCache()
        {
            RTCameraController newItem = new RTCameraController($"RTCamera_{_m_lTotalCacheList.Count + 1}");
            
            //放入总缓存队列
            _m_lTotalCacheList.Add(newItem);
            _m_lEnableCacheList.Add(newItem);
        }
        
        //重制item
        private void _resetItem(RTCameraController _cameraController)
        {
            if(null == _cameraController)
                return;
            
            _cameraController.disableShowcaseCamera();
        }
    }
}