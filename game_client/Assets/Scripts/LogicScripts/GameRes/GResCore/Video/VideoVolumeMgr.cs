using ALPackage;

namespace GOE
{
    public class VideoVolumeMgr : _ATALBasicSingleAssetObj<GSOVideoVolumeRefSet>
    {
        private static VideoVolumeMgr _g_instance = new VideoVolumeMgr();
        public static VideoVolumeMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new VideoVolumeMgr();
                return _g_instance;
            }
        }

        /** 获取资源管理对象 */
        protected override _AALResourceCore _resCore { get { return GameResCore.instance; } }
        /** 获取加载路径 */
        protected override string _resPath { get { return GSOVideoVolumeRefSet.assetPath; } }
        /** 获取加载对象名 */
        protected override string _objName { get { return GSOVideoVolumeRefSet.objName; } }

        public float getVolume(GVideoClipIndex _videoClipIndex)
        {
            if (_videoClipIndex != null && obj != null && obj.videoVolumeDic != null && obj.videoVolumeDic.TryGetValue(_videoClipIndex, out float volume)) 
                return volume;
            return 1;
        }
    }
}