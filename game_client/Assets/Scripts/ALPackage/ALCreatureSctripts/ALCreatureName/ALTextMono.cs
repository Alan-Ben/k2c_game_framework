using UnityEngine;
using System.Collections;

#if AL_CREATURE_SYS
namespace ALPackage
{
    public class ALTextMono : MonoBehaviour
    {
        public Camera focusCamera = null;


        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (null != focusCamera)
            {
                transform.LookAt(focusCamera.transform);
            }
        }
    }
}

#endif
