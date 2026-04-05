using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GNodeBuildingEffect : BaseQueueNode
    {
        private int _m_enterSerialize;


        public GNodeBuildingEffect()
            : base(EUIQueueStageType.MAIN, UINodeTagConst.C_BUILDING_EFFECT)
        {
        }


        public override bool isRootNode { get { return false; } }
        public override bool IsCanRollBackQuit { get { return false; } }
        public override bool NeedAutoRemove { get { return false; } }
        public override bool IsMainViewNode { get { return true; } }
        public override bool isEnable { get { return true; } }


        public override void onEnterQueue()
        {
        }
        public override void onClose()
        {
        }
        public override void doEnterNode(Action _triggerEnterDone)
        {
            base.doEnterNode(null);

            int serialize = ++_m_enterSerialize;
            GTDSceneMain.instance.showMainScene(MainAdditionBuildingEffectTDScene.instance, () =>
            {
                if (serialize != _m_enterSerialize)
                    return;

                GUISceneMain.instance.showMainScene(GGUIAddSceneBuildingEffect.instance, () =>
                {
                    if (serialize != _m_enterSerialize)
                        return;

                    _triggerEnterDone?.Invoke();
                    _startEffect(serialize);
                });
            });
        }
        public override void EnterNode()
        {
        }
        public override void QuitNode()
        {
            _m_enterSerialize++;
        }
        public override void onCannotEscBack()
        {
        }


        private void _startEffect(int _serialize)
        {
            if (_serialize != _m_enterSerialize)
                return;

            Vector3 endPos = MainAdditionBuildingEffectTDScene.instance.cameraEndPos;
            float cameraDuration = MainAdditionBuildingEffectTDScene.instance.cameraDuration;

            ALStepCounter doneCounter = new ALStepCounter();
            doneCounter.regAllDoneDelegate(() =>
            {
                if (_serialize != _m_enterSerialize)
                    return;

                QueueMgr.instance.forceCloseNode(this);
            });
            doneCounter.chgTotalStepCount(3);

            GGUIAddSceneBuildingEffect.instance.playAnimation(doneCounter.addDoneStepCount);
            MainAdditionBuildingEffectTDScene.instance.playAnimation(doneCounter.addDoneStepCount);
            CameraController.instance.setCameraMoveController(new CameraMoveEaseController(endPos, cameraDuration, doneCounter.addDoneStepCount));
        }
    }
}
