using System.Collections;

namespace ALPackage
{
    public class ALCoroutineWrapper : _IALCoroutineDealer
    {
        private IEnumerator _m_coroutine;

        public ALCoroutineWrapper(IEnumerator _coroutine)
        {
            _m_coroutine = _coroutine;
        }

        /*******************
         * Coroutine的执行函数体
         **/
        public IEnumerator dealCoroutine()
        {
            yield return _m_coroutine;
        }
    }
}