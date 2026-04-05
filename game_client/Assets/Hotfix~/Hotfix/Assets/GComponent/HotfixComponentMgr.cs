using System;
using System.Collections.Generic;
using System.Text;
using ALPackage;
using JetBrains.Annotations;

namespace Hotfix
{
    public class HotfixComponentMgr
    {
		[NotNull] protected List<HotfixBaseComponent> _m_componenList = new List<HotfixBaseComponent>();

		#region Public Methods

		public void RegisterComponent(HotfixBaseComponent component)
		{
			if(component == null)
			{
				Debug.LogError("Component is null");
				return;
			}
			
			_m_componenList.Add(component);
		}

		public void UnregisterComponent(HotfixBaseComponent component)
		{
			if(component == null)
			{
				Debug.LogError("Component is null");
				return;
			}
			
			_m_componenList.Remove(component);
		}

		public void initComponents(Action<bool> _onInitDone)
        {
            bool isAllInitDone = true;
            ALStepCounter _m_sStep = new ALStepCounter();
            _m_sStep.chgTotalStepCount(_m_componenList.Count);
            _m_sStep.regAllDoneDelegate(() =>
            {
                if (_onInitDone != null)
                    _onInitDone(isAllInitDone);
            });

            foreach (var component in _m_componenList)
			{
				if (component == null || component.isInitDone)
				{
					_m_sStep.addDoneStepCount();
					continue;
				}
				
				component.init((_isInitSuc) =>
				{
					isAllInitDone &= _isInitSuc;
					_m_sStep.addDoneStepCount();
				});
			}
		}
		
		public void discardComponents()
		{
			foreach (var component in _m_componenList)
			{
				component?.discard();
			}
		}

        public string getAllUnInitComp()
        {
            StringBuilder builder = new StringBuilder();
            
            foreach (var component in _m_componenList)
            {
                if (component != null && !component.isInitDone)
                {
                    if (builder.Length > 0)
                        builder.Append(" & ");
                    builder.Append(component.GetType().Name);
                }
            }
            return builder.ToString();
        }

        public void forceInited()
        {
	        foreach (var component in _m_componenList)
            {
                if (component != null && !component.isInitDone)
                {
	                component.setInitDone();
                }
            }
        }

        #endregion

        #region Protected & Internal Methods

        protected virtual void InitializeModel()
		{
		}

		#endregion
    }
}