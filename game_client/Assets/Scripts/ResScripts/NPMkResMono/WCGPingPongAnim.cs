using ALPackage;
using UnityEngine;
using UnityEngine.UI;

public class WCGPingPongAnim : MonoBehaviour
{
    public Image _m_aImageSource;  //图片对象
    public float _m_fTimeScale; 
    public RectTransform _m_vPingPos;  //移动起始点
    public RectTransform _m_vPongDis;  //移动终点
    public Animation _m_aAnimation; //动画机

    public bool _m_bScaleChange; //是否需要在移动时改变GOScale
    public float _m_fTargetscale = 1f;//目标比例大小
    public GameObject _m_gChangeScaleGo;//需要被改变比例的GO

    void Start()
    {
        //PingPong效果
        ALMonoTaskMgr.instance.addMonoTask(new WCGMonoTaskPingPongAnimPlay(_m_fTimeScale, _m_vPingPos, _m_aImageSource, _m_vPongDis, _m_aAnimation
            , _m_bScaleChange, _m_fTargetscale, _m_gChangeScaleGo));
    }

    /****************
     * PingPong动画任务
     **/
    protected class WCGMonoTaskPingPongAnimPlay : _IALBaseMonoTask
    {
        private RectTransform _m_vPingPos;   //起始点
        private RectTransform _m_vPongPos;   //目标点
        private Animation _m_aAnimation;
        private Image _m_aImageSouce;  //Image对象
        private float _m_fStartTime; //开始时间
        private float _m_fTimeScale;

        private bool _m_bScaleChange; //是否需要在移动时改变GOScale
        private float _m_fTargetscale = 1f;//目标比例大小
        private GameObject _m_gChangeScaleGo;//需要被改变比例的GO
        private Vector3 _m_vOrdiScale;
        public WCGMonoTaskPingPongAnimPlay(float _TimeScale, RectTransform _pingPos, Image _audioSource, RectTransform _PongPos, Animation _animation
            , bool _scaleChange = false, float _targetscale = 1f, GameObject _changeScaleGo = null)
        {
            _m_vPingPos = _pingPos;
            _m_vPongPos = _PongPos;
            _m_aImageSouce = _audioSource;
            _m_fStartTime = Time.realtimeSinceStartup;
            _m_fTimeScale = _TimeScale;
            _m_aAnimation = _animation;
            _m_bScaleChange = _scaleChange;
            _m_fTargetscale = _targetscale;
            _m_gChangeScaleGo = _changeScaleGo;
            _m_vOrdiScale = _m_gChangeScaleGo == null ? Vector3.zero : _m_gChangeScaleGo.transform.localScale;
        }

        public void deal()
        {
            float passTime = Time.realtimeSinceStartup - _m_fStartTime;

            if (_m_aImageSouce == null)
                return;

            //_m_fTimeScale越大整体变化速度比例越慢，变化率看Lerp
            _m_aImageSouce.rectTransform.position = Vector3.Lerp(_m_vPingPos.position, _m_vPongPos.position, passTime / _m_fTimeScale % 1 );
            
            if(_m_gChangeScaleGo != null && _m_bScaleChange)
            {
                _m_gChangeScaleGo.transform.localScale = Vector3.Lerp(_m_vOrdiScale, new Vector3(_m_fTargetscale, _m_fTargetscale, _m_fTargetscale), passTime / _m_fTimeScale % 1);
            }

            if(_m_aImageSouce.rectTransform.position == _m_vPingPos.position)
                _playAnimation("ping");

            if(_m_aImageSouce.rectTransform.position == _m_vPongPos.position)
                _playAnimation("pong");

            //继续任务
            ALMonoTaskMgr.instance.addNextFrameTask(this);
        }

        private void _playAnimation(string _str)
        {
            if (_m_aAnimation == null)
                return;

            _m_aAnimation.Play(_str);
        }
    }
}