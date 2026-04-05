using ALPackage;

namespace GOE
{
    public abstract class _AUpgradePropertyShow<T, Y>
    {
        private readonly CommonUpgradePropertyShow<T> _m_propertyShow;
        
        
        protected _AUpgradePropertyShow(CommonUpgradePropertyShow<T> _propertyShow)
        {
            _m_propertyShow = _propertyShow;
        }

        
        public void setValue(Y _valueCurrent, Y _valueNext)
        {
            if (_m_propertyShow == null)
                return;

            _setValue(_m_propertyShow.current, _valueCurrent);
            _setValue(_m_propertyShow.next, _valueNext);
            bool isSame = Equals(_valueCurrent, _valueNext);
            ALUGUICommon.setGameObjEnable(_m_propertyShow.listSameShow, isSame);
            ALUGUICommon.setGameObjEnable(_m_propertyShow.listDiffShow, !isSame);
        }
        
        
        protected abstract void _setValue(T _property, Y _value);
    }
}