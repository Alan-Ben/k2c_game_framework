using System;

/*****************
 * 游戏中对场景的管理对象
 * 同一时间只允许存在一个场景，当进入某一场景时，原先的场景就应该自动退出
 **/
namespace ALPackage
{
    public class ALGameStageCore
    {
        private static ALGameStageCore _g_instance = new ALGameStageCore();
        public static ALGameStageCore instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new ALGameStageCore();

                return _g_instance;
            }
        }

        /** 当前所在的游戏场景对象 */
        private _AALBasicGameStage _m_gsCurGameStage;

        protected ALGameStageCore()
        {
            _m_gsCurGameStage = null;
        }

        public _AALBasicGameStage curGameStage { get { return _m_gsCurGameStage; } }

        /********************
         * 退出当前的游戏场景
         **/
        public void quitCurGameStage()
        {
            _setCurGameStage(null);
        }

        /*********************
         * 设置当前所在的游戏场景，保护函数，只提供给场景访问不对外开放
         **/
        protected internal void _setCurGameStage(_AALBasicGameStage _gameStage)
        {
            //处于同一场景不进行处理
            if (_m_gsCurGameStage == _gameStage)
                return;

            //当场景不同时，先退出原先场景
            _AALBasicGameStage preStage = _m_gsCurGameStage;
            //设置新场景对象
            _m_gsCurGameStage = _gameStage;

            if (null != preStage)
                preStage._quitStage();
        }
    }
}
