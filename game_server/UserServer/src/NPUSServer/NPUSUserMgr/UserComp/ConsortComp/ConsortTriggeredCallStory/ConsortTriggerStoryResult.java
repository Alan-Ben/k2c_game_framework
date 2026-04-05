package NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortTriggeredCallStory;

import NPGameRes.Refs.Consort.RefConsortStory;

public class ConsortTriggerStoryResult 
{
	private RefConsortStory _m_refConsortStoryRef;
	private boolean _m_bGainCg;
	
	public void setConsortStoryRef(RefConsortStory _ref) {_m_refConsortStoryRef = _ref;}
	public RefConsortStory getConsortStoryRef() {return _m_refConsortStoryRef;}
	
	public void setGainCg(boolean _gainCg) {_m_bGainCg = _gainCg;}
	public boolean getGainCg() {return _m_bGainCg;}
}
