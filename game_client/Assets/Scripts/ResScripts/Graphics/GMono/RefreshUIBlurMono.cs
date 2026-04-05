
using UnityEngine;

public class RefreshUIBlurMono : MonoBehaviour
{
    [ALHeader("动画里在合适的时机触发事件 refreshBlur")] 
    [ReadOnly]
    public bool tip;
    public void refreshBlur()
    {
#if NP_GAME
        ScreenBlurMgr.instance.refreshBlurRT();
#endif
    }
}
