package NPGameRes.Refs.Consort;

/***
 * 星辉技能数据，包括 星辉技能配表，星辉技能等级配表
 * @author mj
 *
 */
public class ConsortHaloSkillObj 
{
	private RefConsortHaloSkill _m_refHaloSkill;
	private RefConsortHaloSkillLvl _m_refHaloSkillLvl;
	
	public ConsortHaloSkillObj(RefConsortHaloSkill _skillRef, RefConsortHaloSkillLvl _skillLvlRef)
	{
		_m_refHaloSkill = _skillRef;
		_m_refHaloSkillLvl = _skillLvlRef;
	}
	
	public RefConsortHaloSkill getSkilllRef() {return _m_refHaloSkill;};
	public RefConsortHaloSkillLvl getSkillLvlRef() {return _m_refHaloSkillLvl;}
}
