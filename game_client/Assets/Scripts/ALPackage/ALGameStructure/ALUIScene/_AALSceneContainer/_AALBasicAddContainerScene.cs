using System;
using System.Collections.Generic;

/************************
 * 可在Container框架内被包含或被容纳的scene对象
 **/
namespace ALPackage
{
    public abstract class _AALBasicSubContainerScene : _AALBasicSubContainerScene_NoChild
    {
        //当前展示的Scene对象
        private _AALBasicSubContainerScene_NoChild _m_usCurShowScene;
        //当前展示的Wnd对象
        private _AALBasicLoadUIWndBasicClass _m_wCurShowWnd;

        /// <summary>
        /// 已经在本对象中加载的共存子scene对象
        /// </summary>
        private HashSet<_AALBasicSubContainerScene_NoChild> _m_hsLoadedAddScene;
        /// <summary>
        /// 已经在本对象中加载的共存子窗口对象
        /// </summary>
        private HashSet<_AALBasicLoadUIWndBasicClass> _m_hsLoadedAddWnd;

        //会在切换的时候释放的Scene对象
        private _AALBasicSubContainerScene_NoChild _m_usDiscardOnSwitchScene;
        private _AALBasicLoadUIWndBasicClass _m_usDiscardOnSwitchWnd;

        public _AALBasicSubContainerScene(int _sceneTypeId) : base(_sceneTypeId)
        {
            _m_usCurShowScene = null;
            _m_wCurShowWnd = null;
            _m_hsLoadedAddScene = new HashSet<_AALBasicSubContainerScene_NoChild>();
            _m_hsLoadedAddWnd = new HashSet<_AALBasicLoadUIWndBasicClass>();
            _m_usDiscardOnSwitchScene = null;
            _m_usDiscardOnSwitchWnd = null;
        }

        /// <summary>
        /// 当前展示的Scene对象
        /// </summary>
        public _AALBasicSubContainerScene_NoChild curShowScene { get { return _m_usCurShowScene; } }
        public _AALBasicLoadUIWndBasicClass curShowWnd { get { return _m_wCurShowWnd; } }

        /// <summary>
        /// 在离开本Scene时触发的处理，禁止子类继承，对应处理可以重载_dealQuitScene函数进行处理
        /// </summary>
        protected override void _onQuitScene()
        {
            //调用对应视图的离开处理
            if (null != _m_usCurShowScene)
            {
                //隐藏主视图
                _m_usCurShowScene.hideScene();
                //调用事件函数
                _m_usCurShowScene.onSwitchHideScene();

                //重置变量
                _m_usCurShowScene = null;
            }
            if (null != _m_wCurShowWnd)
            {
                //隐藏主视图
                _m_wCurShowWnd.hideWnd();

                //重置变量
                _m_wCurShowWnd = null;
            }

            //释放所有已加载场景
            foreach (_AALBasicSubContainerScene_NoChild _scene in _m_hsLoadedAddScene)
            {
                //过滤无效数据
                if(null == _scene)
                    continue;

                _scene.quitScene();
            }
            _m_hsLoadedAddScene.Clear();

            //释放所有已加载窗口
            foreach(_AALBasicLoadUIWndBasicClass wnd in _m_hsLoadedAddWnd)
            {
                //过滤无效数据
                if(null == wnd)
                    continue;

                wnd.discard();
            }
            _m_hsLoadedAddWnd.Clear();

            if(null != _m_usDiscardOnSwitchScene)
            {
                _m_usDiscardOnSwitchScene.hideScene();
                _m_usDiscardOnSwitchScene.quitScene();
            }
            _m_usDiscardOnSwitchScene = null;

            if (null != _m_usDiscardOnSwitchWnd)
            {
                _m_usDiscardOnSwitchWnd.discard();
            }
            _m_usDiscardOnSwitchWnd = null;

            //当前展示的对象，如果需要在切换释放的，放在Switch对象，如果切换不释放的会放在AddScene表中
            //所以这里直接置空
            _m_usCurShowScene = null;
            _m_wCurShowWnd = null;

            //处理基类的退出Scene的操作
            base._onQuitScene();
        }

        /// <summary>
        /// 展示对应的主视图Scene，处理上隐藏所有其他控制scene，之后展示带入的scene
        /// </summary>
        /// <param name="_scene"></param>
        public void showMainScene(_AALBasicSubContainerScene_NoChild _scene, Action _doneDelegate = null)
        {
            //如果不在本视图则不处理
            if (!isEntered)
            {
                ALLog.Error($"Show Main Scene:[{_scene.ToString()}] when Not in Scene:[{this.ToString()}]!");
                return;
            }

            if (null == _scene)
            {
                if(null != _doneDelegate)
                    _doneDelegate();

                return;
            }

            //调用切换处理
            if(_m_usCurShowScene == _scene)
            {
                if(null != _doneDelegate)
                    _doneDelegate();

                //同一个scene不做处理
                return;
            }

            //在尝试切换的时候触发的函数
            _onSwitchScene(_scene);

            //隐藏所有视图，然后显示需要显示的视图
            _hideAll(
                () => _loadAndShowScene(_scene
                , () =>
                {
                    //设置显示scene
                    _m_usCurShowScene = _scene;

                    //调用显示处理
                    _scene.showScene(
                        () => {
                            //调用回调
                            if (null != _doneDelegate)
                            {
                                _doneDelegate();
                            }

                            //处理切换完成的触发事件函数
                            _onSwitchSceneDone(_scene);
                        });
                }));
        }

        /// <summary>
        /// 展示对应的主视图Wnd，处理上隐藏所有其他控制scene，之后展示带入的scene
        /// </summary>
        /// <param name="_scene"></param>
        public void showMainWnd(_AALBasicLoadUIWndBasicClass _wnd, Action _doneDelegate = null)
        {
            //如果不在本视图则不处理
            if (!isEntered)
            {
                ALLog.Error($"Show Main Wnd:[{_wnd.ToString()}] when Not in Scene:[{this.ToString()}]!");
                return;
            }

            if (null == _wnd)
            {
                if (null != _doneDelegate)
                    _doneDelegate();

                return;
            }

            //调用切换处理
            if (_m_wCurShowWnd == _wnd)
            {
                if (null != _doneDelegate)
                    _doneDelegate();

                //同一个scene不做处理
                return;
            }

            //在尝试切换的时候触发的函数
            _onSwitchWnd(_wnd);

            //隐藏所有视图，然后显示需要显示的视图
            _hideAll(
                () => _loadAndShowWnd(_wnd
                , () =>
                {
                    //设置显示窗口
                    _m_wCurShowWnd = _wnd;

                    //调用显示处理
                    _wnd.showWnd(
                        () => {
                            //调用回调
                            if (null != _doneDelegate)
                            {
                                _doneDelegate();
                            }

                            //处理切换完成的触发事件函数
                            _onSwitchWndDone(_wnd);
                        });
                }));
        }

        /// <summary>
        /// 显示附加视图，注意此时的needDiscardOnSwitch属性是无效的
        /// </summary>
        /// <param name="_scene"></param>
        /// <param name="_showDelegate"></param>
        public void showAddScene(_AALBasicSubContainerScene_NoChild _scene, Action _showDelegate)
        {
            //如果不在本视图则不处理
            if (!isEntered)
            {
                ALLog.Error($"Show Add Scene:[{_scene.ToString()}] when Not in Scene:[{this.ToString()}]!");
                return;
            }

            if (null == _scene)
            {
                //调用回调
                if(null != _showDelegate)
                    _showDelegate();
                return;
            }

            //加载并显示
            _loadAndShowAddScene(_scene, () =>
                {
                    //调用显示处理
                    _scene.showScene(_showDelegate);
                });
        }

        /// <summary>
        /// 显示附加视图，注意此时的needDiscardOnSwitch属性是无效的
        /// </summary>
        /// <param name="_scene"></param>
        /// <param name="_showDelegate"></param>
        public void showAddWnd(_AALBasicLoadUIWndBasicClass _wnd, Action _showDelegate)
        {
            //如果不在本视图则不处理
            if (!isEntered)
            {
                ALLog.Error($"Show Add Wnd:[{_wnd.ToString()}] when Not in Scene:[{this.ToString()}]!");
                return;
            }

            if (null == _wnd)
            {
                //调用回调
                if (null != _showDelegate)
                    _showDelegate();
                return;
            }

            //加载并显示
            _loadAndShowAddWnd(_wnd, () =>
            {
                //调用显示处理
                _wnd.showWnd(_showDelegate);
            });
        }

        /****************
         * 加载并显示对应的附加Scene
         **/
        protected void _loadAndShowScene(_AALBasicSubContainerScene_NoChild _scene, Action _enterDelegate)
        {
            if(null == _scene)
                return;

            //如果还有未处理的Scene则释放
            if(null != _m_usDiscardOnSwitchScene)
            {
                _m_usDiscardOnSwitchScene.hideScene();
                _m_usDiscardOnSwitchScene.quitScene();
                _m_usDiscardOnSwitchScene = null;
            }
            if (null != _m_usDiscardOnSwitchWnd)
            {
                _m_usDiscardOnSwitchWnd.discard();
                _m_usDiscardOnSwitchWnd = null;
            }

            //根据切换的是否是需要在切换时释放进行不同的处理
            if (_scene.needDiscardOnSwitch)
            {
                _m_usDiscardOnSwitchScene = _scene;

                //处理加载操作
                _m_usDiscardOnSwitchScene.enterScene();
                _m_usDiscardOnSwitchScene.regInitDelegate(_enterDelegate);
                return;
            }

            //判断是否已经加载，已加载则直接进入
            if(_m_hsLoadedAddScene.Contains(_scene))
            {
                //已经enter直接执行enter完成回调，未进入说明外部quit了，要重新进入
                if(_scene.isEntered)
                {
                    _scene.regInitDelegate(_enterDelegate);
                }
                else
                {
#if UNITY_EDITOR
                    ALLog.Error($"_m_hsLoadedAddScene列表存在未进入的scene，需要检查");
#endif
                    _scene.enterScene();
                    _scene.regInitDelegate(_enterDelegate);
                }
            }
            else
            {
                //加入集合
                _m_hsLoadedAddScene.Add(_scene);

                _scene.enterScene();
                _scene.regInitDelegate(_enterDelegate);
            }
        }

        /// <summary>
        /// 加载并显示对应的附加Scene
        /// </summary>
        /// <param name="_scene"></param>
        /// <param name="_enterDelegate"></param>
        protected void _loadAndShowWnd(_AALBasicLoadUIWndBasicClass _wnd, Action _enterDelegate)
        {
            if (null == _wnd)
                return;

            //如果还有未处理的Scene则释放
            if (null != _m_usDiscardOnSwitchScene)
            {
                _m_usDiscardOnSwitchScene.hideScene();
                _m_usDiscardOnSwitchScene.quitScene();
                _m_usDiscardOnSwitchScene = null;
            }
            if (null != _m_usDiscardOnSwitchWnd)
            {
                _m_usDiscardOnSwitchWnd.discard();
                _m_usDiscardOnSwitchWnd = null;
            }

            //根据切换的是否是需要在切换时释放进行不同的处理
            if (_wnd.needDiscardOnSwitch)
            {
                _m_usDiscardOnSwitchWnd = _wnd;

                //处理加载操作
                _m_usDiscardOnSwitchWnd.load();
                _m_usDiscardOnSwitchWnd.regLoadDoneDelegate(_enterDelegate);
                return;
            }

            //判断是否已经加载，已加载则直接进入
            if (_m_hsLoadedAddWnd.Contains(_wnd))
            {
                //已经load直接执行load完成回调，未进入说明外部discard了，要重新进入
                if(_wnd.isLoaded)
                {
                    _wnd.regLoadDoneDelegate(_enterDelegate);
                }
                else
                {
#if UNITY_EDITOR
                    ALLog.Error($"_m_hsLoadedAddWnd 列表存在未load的wnd，需要检查");
#endif
                    _wnd.load();
                    _wnd.regLoadDoneDelegate(_enterDelegate);
                }
            }
            else
            {
                //加入集合
                _m_hsLoadedAddWnd.Add(_wnd);

                _wnd.load();
                _wnd.regLoadDoneDelegate(_enterDelegate);
            }
        }

        /// <summary>
        /// 加载并显示对应的附加Scene，此时不管needDiscardOnSwitch是否有效都不会在切换的时候释放
        /// </summary>
        /// <param name="_scene"></param>
        /// <param name="_enterDelegate"></param>
        protected void _loadAndShowAddScene(_AALBasicSubContainerScene_NoChild _scene, Action _enterDelegate)
        {
            if(null == _scene)
                return;

            //判断是否已经加载，已加载则直接进入
            if(_m_hsLoadedAddScene.Contains(_scene))
            {
                //已经enter直接执行enter完成回调，未进入说明外部quit了，要重新进入
                if(_scene.isEntered)
                {
                    _scene.regInitDelegate(_enterDelegate);
                }
                else
                {
#if UNITY_EDITOR
                    ALLog.Error($"_m_hsLoadedAddScene列表存在未进入的scene，需要检查");
#endif
                    _scene.enterScene();
                    _scene.regInitDelegate(_enterDelegate);
                }
            }
            else
            {
                //加入集合
                _m_hsLoadedAddScene.Add(_scene);

                _scene.enterScene();
                _scene.regInitDelegate(_enterDelegate);
            }
        }

        /// <summary>
        /// 加载并显示对应的附加窗口
        /// </summary>
        /// <param name="_scene"></param>
        /// <param name="_enterDelegate"></param>
        protected void _loadAndShowAddWnd(_AALBasicLoadUIWndBasicClass _wnd, Action _loadDelegate)
        {
            if(null == _wnd)
                return;

            //判断是否已经加载，已加载则直接进入
            if(_m_hsLoadedAddWnd.Contains(_wnd))
            {
                //已经load直接执行load完成回调，未进入说明外部discard了，要重新进入
                if(_wnd.isLoaded)
                {
                    _wnd.regLoadDoneDelegate(_loadDelegate);
                }
                else
                {
#if UNITY_EDITOR
                    ALLog.Error($"_m_hsLoadedAddWnd 列表存在未load的wnd，需要检查");
#endif
                    _wnd.load();
                    _wnd.regLoadDoneDelegate(_loadDelegate);
                }
            }
            else
            {
                //加入集合
                _m_hsLoadedAddWnd.Add(_wnd);

                _wnd.load();
                _wnd.regLoadDoneDelegate(_loadDelegate);
            }
        }

        /// <summary>
        /// 隐藏所有的子视图对象
        /// </summary>
        /// <param name="_delegate"></param>
        private List<_AALBasicSubContainerScene_NoChild> _m_tmpSceneDeleteList = new List<_AALBasicSubContainerScene_NoChild>();
        private List<_AALBasicLoadUIWndBasicClass> _m_tmpWndDeleteList = new List<_AALBasicLoadUIWndBasicClass>();
        protected void _hideAll(Action _delegate)
        {
            //调用对应视图的离开处理
            if(null != _m_usCurShowScene)
            {
                //隐藏主视图
                _m_usCurShowScene.hideScene();
                //调用事件函数
                _m_usCurShowScene.onSwitchHideScene();

                //重置变量
                _m_usCurShowScene = null;
            }
            if (null != _m_wCurShowWnd)
            {
                //隐藏主视图
                _m_wCurShowWnd.hideWnd();

                //重置变量
                _m_wCurShowWnd = null;
            }

            //释放需要释放的切换释放Scene
            if (null != _m_usDiscardOnSwitchScene)
            {
                _m_usDiscardOnSwitchScene.hideScene();
                _m_usDiscardOnSwitchScene.quitScene();
                _m_usDiscardOnSwitchScene = null;
            }
            if (null != _m_usDiscardOnSwitchWnd)
            {
                _m_usDiscardOnSwitchWnd.discard();
                _m_usDiscardOnSwitchWnd = null;
            }

            if (_m_hsLoadedAddScene.Count > 0 || _m_hsLoadedAddWnd.Count > 0)
            {
                ALStepCounter stepCounter = new ALStepCounter();
                //多加一次，避免在循环中调用后续处理，导致数据集变更
                stepCounter.chgTotalStepCount(_m_hsLoadedAddScene.Count + _m_hsLoadedAddWnd.Count + 1);
                //注册回调，后注册避免回调中进入了新的scene
                stepCounter.regAllDoneDelegate(_delegate);

                /** 遍历已经加载的场景进行隐藏 */
                _m_hsLoadedAddScene.Clear();
                foreach (_AALBasicSubContainerScene_NoChild _scene in _m_hsLoadedAddScene)
                {
                    //过滤无效数据
                    if(null == _scene)
                    {
                        stepCounter.addDoneStepCount();
                        continue;
                    }


                    if (_scene.needDiscardOnSwitch)
                    {
                        //加入临时删除列表，避免循环报错
                        _m_tmpSceneDeleteList.Add(_scene);

                        //退出Scene
                        _scene.quitScene();

                        //调用阶段步骤
                        stepCounter.addDoneStepCount();
                    }
                    else
                    {
                        _scene.hideScene(stepCounter.addDoneStepCount);
                    }
                }
                //将临时列表中数据删除
                for (int i = 0; i < _m_tmpSceneDeleteList.Count; i++)
                {
                    _m_hsLoadedAddScene.Remove(_m_tmpSceneDeleteList[i]);
                }
                _m_tmpSceneDeleteList.Clear();

                /** 遍历已经加载的窗口进行隐藏 */
                _m_tmpWndDeleteList.Clear();
                foreach (_AALBasicLoadUIWndBasicClass wnd in _m_hsLoadedAddWnd)
                {
                    //过滤无效数据
                    if (null == wnd)
                    {
                        stepCounter.addDoneStepCount();
                        continue;
                    }


                    if (wnd.needDiscardOnSwitch)
                    {
                        //加入临时删除列表，避免循环报错
                        _m_tmpWndDeleteList.Add(wnd);

                        //释放窗口
                        wnd.discard();

                        //调用阶段步骤
                        stepCounter.addDoneStepCount();
                    }
                    else
                    {
                        wnd.hideWnd(stepCounter.addDoneStepCount);
                    }
                }
                //将临时列表中数据删除
                for (int i = 0; i < _m_tmpWndDeleteList.Count; i++)
                {
                    _m_hsLoadedAddWnd.Remove(_m_tmpWndDeleteList[i]);
                }
                _m_tmpWndDeleteList.Clear();

                //调用一次额外处理
                stepCounter.addDoneStepCount();
            }
            else
            {
                if(null != _delegate)
                    _delegate();
            }
        }

        /// <summary>
        /// 容器对象的隐藏操作
        /// </summary>
        /// <param name="_delegate"></param>
        public override void hideScene()
        {
            //先处理子节点的隐藏操作，在所有子节点隐藏之后处理自己的隐藏操作
            _hideAll(base.hideScene);
        }
        public override void hideScene(Action _delegate)
        {
            //先处理子节点的隐藏操作，在所有子节点隐藏之后处理自己的隐藏操作
            _hideAll(() => base.hideScene(_delegate));
        }

        /// <summary>
        /// 在尝试切换Scene的时候触发的函数
        /// </summary>
        protected abstract void _onSwitchScene(_AALBasicSubContainerScene_NoChild _tarScene);
        protected abstract void _onSwitchWnd(_AALBasicLoadUIWndBasicClass _tarWnd);

        /// <summary>
        /// 在切换Scene完成的时候触发的函数
        /// </summary>
        protected abstract void _onSwitchSceneDone(_AALBasicSubContainerScene_NoChild _tarScene);
        protected abstract void _onSwitchWndDone(_AALBasicLoadUIWndBasicClass _tarWnd);
    }
}
