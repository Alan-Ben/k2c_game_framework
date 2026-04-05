using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// gray为替换置灰材质方式的Mono
    /// </summary>
    public class ImageMatGray : Image , _IMatGrayBase
    {
        private Material _nolMaterial;

        [SerializeField]
        private Material _grayMaterial;

        private bool _m_hasAwake = false;

        protected override void Awake()
        {
            base.Awake();
            _nolMaterial = material;
            _m_hasAwake = true;
        }

        private void discard()
        {
            disgrayImage();
        }
    
        public void grayImage()
        {
            material = _grayMaterial;
        }

        public void disgrayImage()
        {
            if(!_m_hasAwake)
                return;
        
            if(_nolMaterial != null)
            {
                if(material != _nolMaterial)
                    material = _nolMaterial;
            }
            else 
            {
#if UNITY_EDITOR && DEBUG_NORMAL
                Debug.LogError($"[Ben]{gameObject.name}disgrayImage Error 材质为空", gameObject);
#endif
                material = defaultGraphicMaterial;
            }
        }
    }
}
