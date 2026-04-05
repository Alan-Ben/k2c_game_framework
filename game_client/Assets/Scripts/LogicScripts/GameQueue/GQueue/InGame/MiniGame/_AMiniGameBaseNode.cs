using System;
using ALPackage;
using GOE.MiniGame;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 小游戏的游戏主窗口节点
    /// EUIQueueStageType为EUIQueueStageType.MINI_GAME
    /// </summary>
    public abstract class _AMiniGameBaseNode : BaseQueueNode
    {
        [NotNull] protected MiniGameMainRefObj _m_rMiniGameMainRefObj;

        public _AMiniGameBaseNode([NotNull] MiniGameMainRefObj _miniGameMainRefObj, string _nodeUITag) : base(EUIQueueStageType.MINI_GAME, _nodeUITag)
        {
            _m_rMiniGameMainRefObj = _miniGameMainRefObj;
        }

        protected abstract _ANPBasicAddContainerUIScene _m_uiScene { get; }
        protected abstract _AMainAdditionMiniGameTDScene _m_tdScene { get; }

        /// <summary>
        /// 重写基类的方法都不让子类重写了
        /// </summary>
        public sealed override void onEnterQueue()
        {
            onEnterQueueSub();
            
            // 这里先加载是为了将scene的实际quitScene权限增加onClose时的控制, 即只有当onClose时, scene才会被真正销毁
            if(_m_uiScene != null)
                _m_uiScene.enterScene();
            
            _AMainAdditionMiniGameTDScene scene = _m_tdScene;
            if (scene != null && _m_rMiniGameMainRefObj.scene_id > 0)
            {
                // 设置小游戏主表数据
                scene.setMiniGameMainRefObj(_m_rMiniGameMainRefObj);
                scene.enterScene();
            }
            
            // 小游戏通用窗口加载放到onEnterQueueSub加载完成后
            if (_m_rMiniGameMainRefObj.canSkip)
            {
                GGUIWndMiniGameCommunalWnd.instance.setData(_m_rMiniGameMainRefObj);
                GGUIWndMiniGameCommunalWnd.instance.setOnBtnSkipClick(onMiniGameCommunalWndSkipBtnClick);
                GGUIWndMiniGameCommunalWnd.instance.load();
            }
        }

        /// <summary>
        /// 重写基类的方法都不让子类重写了
        /// </summary>
        public sealed  override void onClose()
        {
            onCloseSub();
            
            if(_m_uiScene != null)
                _m_uiScene.quitScene();
            if(_m_tdScene != null && _m_tdScene.isEntered)
                _m_tdScene.quitScene();
            
            GGUIWndMiniGameCommunalWnd.instance.setOnBtnSkipClick(null);
            GGUIWndMiniGameCommunalWnd.instance.discard();
        }

        /// <summary>
        /// 重写基类的方法都不让子类重写了
        /// </summary>
        /// <param name="_triggerEnterDone"></param>
        public sealed  override void doEnterNode(Action _triggerEnterDone)
        {
            base.doEnterNode(null);
            
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);
            stepCounter.regAllDoneDelegate(() =>
            {
                doEnterNodeLastStep(_triggerEnterDone);
            });
            
            GStageMain.instance.enterStage();
            GStageMain.instance.regInitDelegate(() =>
            {
                _ANPBasicAddContainerUIScene uiScene = _m_uiScene;
                if (uiScene != null)
                {
                    stepCounter.chgTotalStepCount(1);
                    GUISceneMain.instance.showMainScene(uiScene, stepCounter.addDoneStepCount);
                }
                
                // 进行3D场景加载
                _AMainAdditionMiniGameTDScene scene = _m_tdScene;
                if (scene != null && _m_rMiniGameMainRefObj.scene_id > 0)
                {
                    stepCounter.chgTotalStepCount(1);
                    // 设置小游戏主表数据
                    scene.setMiniGameMainRefObj(_m_rMiniGameMainRefObj);
                    GTDSceneMain.instance.showMainScene(scene, stepCounter.addDoneStepCount);
                }
                else if (scene == null && _m_rMiniGameMainRefObj.scene_id > 0)
                {
                    Debug.LogError_EditorOnly(
                        $"[_AMiniGameBaseNode onEnterQueue] mini_game_main表中配置了scene_id:{_m_rMiniGameMainRefObj.scene_id}, 但是代码却没有找到对应的_TDScene, 小游戏类型:{_m_rMiniGameMainRefObj.eGameType}");
                }

                stepCounter.chgTotalStepCount(1);
                doEnterNodeSub(() =>
                {
                    stepCounter.addDoneStepCount();
                });

                stepCounter.addDoneStepCount();
            });
        }
        
        /// <summary>
        /// 重写基类的方法都不让子类重写了, 子类需要可以重写onEnterNodeSub
        /// </summary>
        public sealed override void EnterNode()
        {
        }

        /// <summary>
        /// 重写基类的方法都不让子类重写了, 子类需要可以重写onQuitNodeSub
        /// </summary>
        public sealed override void QuitNode()
        {
            onQuitNodeSub();
            
            GGUIWndMiniGameCommunalWnd.instance.hideWnd();
        }
        
        protected virtual void onEnterQueueSub()
        {
        }
        
        protected abstract void onCloseSub();
        
        /// <summary>
        /// 子类重写这个方法时要注意, 在这个方法中uiScene和tdScene不一定处于Entered状态
        /// </summary>
        /// <param name="_triggerSubEnterDone"></param>
        protected virtual void doEnterNodeSub(Action _triggerSubEnterDone)
        {
            _triggerSubEnterDone?.Invoke();
        }
        
        /// <summary>
        /// 在自身doEnterNode加载逻辑 和 子类doEnterNodeSub加载逻辑完成后 调用基类_triggerEnterDone前的最后一步, 执行到这个方法时uiScene和tdScene已经处于Entered状态
        /// </summary>
        /// <param name="_triggerEnterDone"></param>
        protected virtual void doEnterNodeLastStep(Action _triggerEnterDone)
        {
            onEnterNodeSub();
            
            if (_m_rMiniGameMainRefObj.canSkip)
            {
                GGUIWndMiniGameCommunalWnd.instance.regLoadDoneDelegate(() =>
                {
                    GGUIWndMiniGameCommunalWnd.instance.showWnd();
                        
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndMiniGameCommunalWnd.instance.getGameObj());
                });
            }
            
            _triggerEnterDone?.Invoke();
        }
        
        /// <summary>
        /// 执行到这个方法时uiScene和tdScene已经处于Entered状态
        /// </summary>
        protected abstract void onEnterNodeSub();

        protected abstract void onQuitNodeSub();
        
        /// <summary>
        /// 小游戏公用窗口跳过按钮被点击时执行事件
        /// </summary>
        protected abstract void onMiniGameCommunalWndSkipBtnClick();
    }
}