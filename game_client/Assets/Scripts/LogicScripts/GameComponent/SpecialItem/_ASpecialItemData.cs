using System;
using CommonEnum;
using JetBrains.Annotations;

namespace GOE
{
    public abstract class _ASpecialItemData
    {
        protected _ASpecialItemData()
        {
        }
        
        
        public abstract ESpecialItemType type { get; }
        

        public abstract void presendInitProtocol([NotNull] Action<Action> _preInitFunc);
        public abstract void init();
        public abstract void discard();
        public abstract long getValue();
        public virtual void onAllCompInited()
        {
            
        }
    }
}