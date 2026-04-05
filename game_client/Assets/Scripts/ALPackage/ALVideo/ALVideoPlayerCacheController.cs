using System;
using ALPackage;
using UnityEngine;
using UnityEngine.Video;

/*********************
 * 创建和管理VideoPlayer对象的缓存池
 **/
namespace ALPackage
{
    public class ALVideoPlayerCacheController : _AALCacheHashSetController<_AALVideoPlayerDealer, _AALVideoPlayerDealer>
    {
        //根节点对象
        private Transform _m_tRootTrans;
        //创建处理器的Func
        private Func<Transform, string, _AALVideoPlayerDealer> _m_fCreateDealer = null;

        public ALVideoPlayerCacheController(Func<Transform, string, _AALVideoPlayerDealer> _createFunc, int _minCount, int _maxCount, Transform _rootTrans)
            : base(_minCount, _maxCount)
        {
            _m_fCreateDealer = _createFunc;
            _m_tRootTrans = _rootTrans;

            //直接调用初始化
            _AALVideoPlayerDealer tmplateObj = _createNewVideoPlayer("template");
            tmplateObj.reset();

            init(tmplateObj);
        }

        //警告信息文字
        protected override string _warningTxt { get { return $"video_player_cache_controller"; } }

        //初始化时的事件函数
        protected override void _onInit(_AALVideoPlayerDealer _template)
        {
            //暂无处理对象
        }
        //根据模板创建对象的函数
        protected override _AALVideoPlayerDealer _createItem(_AALVideoPlayerDealer _template)
        {
            //创建空go
            return _createNewVideoPlayer($"vp_{totalCount}");
        }
        //释放创建出来的对象的资源
        protected override void _discardItem(_AALVideoPlayerDealer _vp)
        {
            //直接调用释放函数
            _vp.discard();
        }
        //设置对象无效
        protected override void _resetItem(_AALVideoPlayerDealer _vp)
        {
            //重置相关数据
            _vp.reset();
        }
        

        /// <summary>
        /// 凭空创建一个新的VideoPlayer对象
        /// </summary>
        /// <returns></returns>
        private _AALVideoPlayerDealer _createNewVideoPlayer(string _name)
        {
            if (null == _m_fCreateDealer)
                return null;

            return _m_fCreateDealer(_m_tRootTrans, _name);
        }
    }
}
