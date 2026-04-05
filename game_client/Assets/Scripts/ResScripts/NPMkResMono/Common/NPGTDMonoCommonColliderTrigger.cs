using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class NPGTDMonoCommonColliderTrigger: MonoBehaviour
    {
        public Action<string, string> onTriggerEnter;
        public Action<string, string> onTriggerExit;
        
        private void OnTriggerEnter(Collider other)
        {
            if (null == other)
                return;
            onTriggerEnter?.Invoke(other.name, other.tag);
        }

        private void OnTriggerExit(Collider other)
        {
            if (null == other)
                return;
            onTriggerExit?.Invoke(other.name, other.tag);
        }
    }
}