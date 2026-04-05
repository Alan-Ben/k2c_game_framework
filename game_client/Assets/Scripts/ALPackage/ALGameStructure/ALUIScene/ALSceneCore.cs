using System;
using System.Collections.Generic;
using System.Text;

/*****************
 * 游戏中对ui场景的管理对象，存储和控制当前的ui场景相关信息
 **/
namespace ALPackage
{
    public class ALSceneCore
    {
        private static ALSceneCore _g_instance = new ALSceneCore();
        public static ALSceneCore instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new ALSceneCore();
                return _g_instance;
            }
        }

        /** 当前所在的游戏场景对象 */
        private Dictionary<int, _AALBasicScene> _m_dicUISceneDic;
        /** 当前的附加场景信息存储队列，此对象根据附加场景自身进行控制 */
        private Dictionary<int, List<_AALBasicAdditionScene>> _m_dicAdditionSceneListDic;

        protected ALSceneCore()
        {
            _m_dicUISceneDic = new Dictionary<int, _AALBasicScene>();
            _m_dicAdditionSceneListDic = new Dictionary<int, List<_AALBasicAdditionScene>>();
        }

        /** 获取当前的ui场景控制对象，此对象互斥 */
        public _AALBasicScene getCurScene(int _sceneTypeId)
        {
            if (!_m_dicUISceneDic.ContainsKey(_sceneTypeId))
                return null;

            return _m_dicUISceneDic[_sceneTypeId];
        }

        /********************
         * 退出当前的游戏场景
         **/
        public void quitCurScene(int _sceneTypeId)
        {
            _setCurScene(_sceneTypeId, null);
        }

        /********************
         * 退出所有附加场景
         **/
        public void quitAllAdditionScene(int _sceneTypeId)
        {
            if (!_m_dicAdditionSceneListDic.ContainsKey(_sceneTypeId))
                return;

            List<_AALBasicAdditionScene> sceneList = _m_dicAdditionSceneListDic[_sceneTypeId];
            //将附加uiScene队列清除
            for (int i = 0; i < sceneList.Count;)
            {
                _AALBasicAdditionScene additionUIScene = sceneList[0];
                if (null == additionUIScene)
                    continue;

                //此处quitScene内部会调用sceneList的清除处理
                additionUIScene._quitScene();
            }
            //清空对应数据
            _m_dicAdditionSceneListDic.Remove(_sceneTypeId);
        }

        /// <summary>
        /// 根据Scene类型，提供可以动态判断的方式，退出所有类型下符合判断的Scene
        /// </summary>
        /// <param name="_sceneTypeId"></param>
        /// <param name="_judgeNeedQuit"></param>
        public void quitAllAdditionScene(int _sceneTypeId, Func<_AALBasicAdditionScene, bool> _judgeNeedQuit)
        {
            if(null == _judgeNeedQuit)
            {
                quitAllAdditionScene(_sceneTypeId);
                return;
            }

            if (!_m_dicAdditionSceneListDic.ContainsKey(_sceneTypeId))
                return;

            //是否全部清空，只有在全部清空的情况下，才会从总类型索引中删除队列
            bool isAllClear = true;
            List<_AALBasicAdditionScene> sceneList = _m_dicAdditionSceneListDic[_sceneTypeId];
            //将附加uiScene队列清除
            for (int i = sceneList.Count - 1; i >= 0; i--)
            {
                _AALBasicAdditionScene additionUIScene = sceneList[i];
                if (null == additionUIScene)
                    continue;

                //不匹配判断条件则不处理退出
                if (!_judgeNeedQuit(additionUIScene))
                {
                    isAllClear = false;
                    continue;
                }

                //退出Scene
                //此处quitScene内部会调用sceneList的清除处理
                additionUIScene._quitScene();
            }

            //只有在全部清空的情况下，才会从总类型索引中删除队列
            if (isAllClear)
                _m_dicAdditionSceneListDic.Remove(_sceneTypeId);
        }

        /*********************
         * 设置当前所在的游戏场景，保护函数，只提供给场景访问不对外开放
         **/
        protected internal void _setCurScene(int _sceneTypeId, _AALBasicScene _uiScene)
        {
            //判断是否有对应集合
            if(_m_dicUISceneDic.ContainsKey(_sceneTypeId))
            {
                _AALBasicScene preScene = _m_dicUISceneDic[_sceneTypeId];
                if (_uiScene == preScene)
                    return;

                if (null != preScene)
                    preScene._quitScene();
                _m_dicUISceneDic.Remove(_sceneTypeId);
            }
            
            _m_dicUISceneDic[_sceneTypeId] = _uiScene;
        }

        /*********************
         * 注册一个附加的ui场景对象
         **/
        protected internal void _regAdditionScene(_AALBasicAdditionScene _uiScene)
        {
            if (null == _uiScene)
                return;

            //将本附加场景添加到队列中
            List<_AALBasicAdditionScene> sceneList = null;
            if (_m_dicAdditionSceneListDic.ContainsKey(_uiScene.sceneTypeId))
            {
                sceneList = _m_dicAdditionSceneListDic[_uiScene.sceneTypeId];
            }

            if(null == sceneList)
            {
                sceneList = new List<_AALBasicAdditionScene>();
                _m_dicAdditionSceneListDic.Add(_uiScene.sceneTypeId, sceneList);
            }

            //添加到队列中
            sceneList.Add(_uiScene);
        }
        protected internal void _unregAdditionScene(_AALBasicAdditionScene _uiScene)
        {
            if (null == _uiScene)
                return;

            //将附加场景从集合中删除
            if (!_m_dicAdditionSceneListDic.ContainsKey(_uiScene.sceneTypeId))
                return;

            List<_AALBasicAdditionScene> sceneList = _m_dicAdditionSceneListDic[_uiScene.sceneTypeId];
            if (null == sceneList)
                return;

            //从队列中删除对象
            sceneList.Remove(_uiScene);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("所有Scene:");
            builder.Append(string.Format("{0}:{{", _m_dicUISceneDic.Count));
            int i = 0;
            foreach (var kvp in _m_dicUISceneDic)
            {
                builder.Append($"{kvp.Key.ToString()}:{kvp.Value.ToString()}");
                if (i < _m_dicUISceneDic.Count - 1)
                {
                    builder.Append(", \t");
                }
                i++;
            }
            builder.Append("}\n\n");
            
            builder.AppendLine("所有AdditionScene:");
            builder.Append(string.Format("{0}:{{\n", _m_dicAdditionSceneListDic.Count));
            i = 0;
            foreach (var kvp in _m_dicAdditionSceneListDic)
            {
                builder.Append($"{kvp.Key.ToString()}:    {ToStringList(kvp.Value)}");
                if (i < _m_dicAdditionSceneListDic.Count - 1)
                {
                    builder.Append("\n");
                }
                i++;
            }
            builder.Append("}");
            return builder.ToString();
        }
        
        public string ToStringList<T>(List<T> _selfList, string _itemSplit = "\n\t", string _countFormat = "{0}:[\n\t", string _endString = "]")
        {
            if (null == _selfList)
                return "Null";
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format(_countFormat, _selfList.Count));
            for (int i = 0; i < _selfList.Count; i++)
            {
                builder.Append(_selfList[i]); // Append string to StringBuilder
                if (i < _selfList.Count - 1)
                {
                    builder.Append(_itemSplit);
                }
            }
            builder.Append(_endString);
            return builder.ToString();
        }
    }
}
