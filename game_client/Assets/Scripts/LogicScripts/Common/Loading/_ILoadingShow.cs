using System;

namespace GOE
{
    public interface _ILoadingShow
    {
        void show(Action _doneDelegate);
        void hide(Action _doneDelegate);
    }
}