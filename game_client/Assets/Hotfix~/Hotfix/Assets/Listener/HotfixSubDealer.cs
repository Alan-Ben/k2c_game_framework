using ALBasicProtocolPack;
using ALPackage;
using GOE;
using UnityEngine;

namespace Hotfix
{
    public abstract class HotfixSubDealer<T> : HotfixProtocolSubOrderDealer<T>  where T : _IALProtocolStructure {
        protected abstract void _dealProtocolByLog (_IALProtocolDealer _dealer, T _msg);

        protected override void _dealProtocol (_IALProtocolDealer _dealer, T _msg)
        {
            if (Game.instance.mainCamera.gameSetting.printProtocol)
            {
                if(_msg.GetFullPackBufSize() <= Game.instance.mainCamera.gameSetting.protocolPrintMinSize)
                {
#if AL_ILRUNTIME
                    if(_msg is _IALProtocolStructureAdapter.Adapter protocolAdapter)
                    {
                        GCommon.NetRecv(string.Format("[Hotfix]<color=red>S -> C: {0} ; </color> {1}", _msg.GetType().Name, HotfixGCommon.GetInfoPropertys(_msg)));
                    }
                    else
#endif
                    {
                        GCommon.NetRecv(string.Format("<color=red>S -> C: {0} ; </color> {1}", _msg.GetType().Name, GCommon.GetInfoPropertys(_msg)));
                    }
                }
                else
                {
#if AL_ILRUNTIME
                    if(_msg is _IALProtocolStructureAdapter.Adapter protocolAdapter)
                    {
                        GCommon.NetRecv(string.Format("[Hotfix]<color=red>S -> C: {0} ; </color> {1}", _msg.GetType().Name, HotfixGCommon.GetInfoPropertys(_msg)));
                    }
                    else
#endif
                    {
                        GCommon.NetRecv(string.Format("<color=red>S -> C: {0} ; </color> {1}", _msg.GetType().Name, GCommon.GetInfoPropertys(_msg)));
                    }
                }
            }
#if UNITY_EDITOR
            else if(_msg.GetFullPackBufSize() >= Game.instance.mainCamera.gameSetting.protocolPrintMinSize)
            {
#if AL_ILRUNTIME
                if(_msg is _IALProtocolStructureAdapter.Adapter protocolAdapter)
                {
                    GCommon.NetRecv(string.Format("[Hotfix]<color=red>S -> C: {0} ; </color> {1}", _msg.GetType().Name, HotfixGCommon.GetInfoPropertys(_msg)));
                }
                else
#endif
                {
                    GCommon.NetRecv(string.Format("<color=red>S -> C: {0} ; </color> {1}", _msg.GetType().Name, GCommon.GetInfoPropertys(_msg)));
                }
            }
#endif

            _dealProtocolByLog(_dealer, _msg);
        }
    }
}