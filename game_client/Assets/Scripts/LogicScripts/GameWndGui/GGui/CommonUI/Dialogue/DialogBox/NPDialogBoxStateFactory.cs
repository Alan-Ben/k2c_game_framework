using JetBrains.Annotations;
using ALPackage;
using System;

namespace GOE
{
    public partial class NPGGUIWndDialogBox
    {
        private class NPDialogBoxStateFactory : _ATALStateFactory<_ANPDialogBoxState, ENPDialogBoxState>
        {
            public NPDialogBoxStateFactory([NotNull] NPGGUIWndDialogBox _wnd) : base()
            {
                regCacheController(
                    typeof(NPDialogBoxState_Wait),
                    new StateCache(() => { return new NPDialogBoxState_Wait(_wnd); }));

                regCacheController(
                    typeof(NPDialogBoxState_Playing),
                    new StateCache(() => { return new NPDialogBoxState_Playing(_wnd, GRefdataCoreMgr.instance.npGeneral.dialogue_talk_speed); }));

                regCacheController(
                    typeof(NPDialogBoxState_End),
                    new StateCache(() => { return new NPDialogBoxState_End(_wnd); }));
            }

            private class StateCache : _TALBasicStateCacheController<_ANPDialogBoxState, ENPDialogBoxState>
            {
                public StateCache(Func<_ANPDialogBoxState> _createFunc) : base(_createFunc, 1, 2)
                {

                }
            }
        }
    }
}
