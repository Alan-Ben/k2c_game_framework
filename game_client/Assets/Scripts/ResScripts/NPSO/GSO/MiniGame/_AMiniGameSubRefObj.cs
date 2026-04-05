using System;
using ALPackage;

namespace GOE
{
    [Serializable]
    public abstract class _AMiniGameSubRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id;//唯一id
    }
}