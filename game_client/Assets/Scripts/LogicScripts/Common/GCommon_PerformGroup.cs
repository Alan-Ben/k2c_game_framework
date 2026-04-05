using System;

namespace GOE
{
    public static partial class GCommon
    {
        public static void playPerformGroup(long _groupId, Action _complete)
        {
            PerformGroupRefObj groupRef = GRefdataCoreMgr.instance.performGroupRefCore.getRef(_groupId);
            if (groupRef == null || groupRef.item_ids == null)
            {
                _complete?.Invoke();
                return;
            }

            for (int i = 0; i < groupRef.item_ids.Count; i++)
            {
                PerformGroupItemRefObj itemRef = GRefdataCoreMgr.instance.performGroupItemRefCore.getRef(groupRef.item_ids[i]);
                if (itemRef == null)
                    continue;

                if (!itemRef.condition.IsEnable(null))
                    continue;

                _playPerformGroupItem(itemRef, _complete);
                return;
            }
            
            _complete?.Invoke();
        }

        private static void _playPerformGroupItem(PerformGroupItemRefObj _itemRef, Action _complete)
        {
            switch (_itemRef.type)
            {
                case EPerformGroupType.Dialogue:
                    PerformGroupDialogueRefObj dialogueRef = GRefdataCoreMgr.instance.performGroupDialogueRefCore.getRef(_itemRef.id);
                    if (dialogueRef == null)
                    {
                        _complete?.Invoke();
                        return;
                    }
                    GCommon.enterDialogueNode(dialogueRef.dialogue_id, _complete);
                    break;
                default:
                    _complete?.Invoke();
                    break;
            }
        }
    }
}