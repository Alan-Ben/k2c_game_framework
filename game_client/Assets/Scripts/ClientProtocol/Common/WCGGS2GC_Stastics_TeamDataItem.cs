using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_Stastics_TeamDataItem : ALBasicProtocolPack._IALProtocolStructure {
private int groupId;
private List<Common.WCGGS2GC_Stastics_ActorBattleData> actorBattleDataList;
private List<Common.WCGGS2GC_Stastics_ActorBattleData> heroBattleDataList;
private List<Common.WCGGS2GC_Stastics_ResourceBattleData> resDataList;
private int totalKillNum;
private int totalHeroKill;
private int TotalHeroDie;
private int totalDragronKill;
private long maxPopulayion;
private int miningSameTime;
private int skillBeBreakDown;
private int skillBreakDown;


public WCGGS2GC_Stastics_TeamDataItem() {
	groupId = 0;
	actorBattleDataList = new List<Common.WCGGS2GC_Stastics_ActorBattleData>();
	heroBattleDataList = new List<Common.WCGGS2GC_Stastics_ActorBattleData>();
	resDataList = new List<Common.WCGGS2GC_Stastics_ResourceBattleData>();
	totalKillNum = 0;
	totalHeroKill = 0;
	TotalHeroDie = 0;
	totalDragronKill = 0;
	maxPopulayion = (long)0;
	miningSameTime = 0;
	skillBeBreakDown = 0;
	skillBreakDown = 0;
}

public WCGGS2GC_Stastics_TeamDataItem(
	int _groupId
	, List<Common.WCGGS2GC_Stastics_ActorBattleData> _actorBattleDataList
	, List<Common.WCGGS2GC_Stastics_ActorBattleData> _heroBattleDataList
	, List<Common.WCGGS2GC_Stastics_ResourceBattleData> _resDataList
	, int _totalKillNum
	, int _totalHeroKill
	, int _TotalHeroDie
	, int _totalDragronKill
	, long _maxPopulayion
	, int _miningSameTime
	, int _skillBeBreakDown
	, int _skillBreakDown
) {	groupId = _groupId;
	actorBattleDataList = _actorBattleDataList;
	heroBattleDataList = _heroBattleDataList;
	resDataList = _resDataList;
	totalKillNum = _totalKillNum;
	totalHeroKill = _totalHeroKill;
	TotalHeroDie = _TotalHeroDie;
	totalDragronKill = _totalDragronKill;
	maxPopulayion = _maxPopulayion;
	miningSameTime = _miningSameTime;
	skillBeBreakDown = _skillBeBreakDown;
	skillBreakDown = _skillBreakDown;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public int getGroupId() { return groupId; }
public void setGroupId(int _groupId) { groupId = _groupId; }
public List<Common.WCGGS2GC_Stastics_ActorBattleData> getActorBattleDataList() { return actorBattleDataList; }
public void addActorBattleDataList(Common.WCGGS2GC_Stastics_ActorBattleData _actorBattleDataList) { actorBattleDataList.Add(_actorBattleDataList); }
public List<Common.WCGGS2GC_Stastics_ActorBattleData> getHeroBattleDataList() { return heroBattleDataList; }
public void addHeroBattleDataList(Common.WCGGS2GC_Stastics_ActorBattleData _heroBattleDataList) { heroBattleDataList.Add(_heroBattleDataList); }
public List<Common.WCGGS2GC_Stastics_ResourceBattleData> getResDataList() { return resDataList; }
public void addResDataList(Common.WCGGS2GC_Stastics_ResourceBattleData _resDataList) { resDataList.Add(_resDataList); }
public int getTotalKillNum() { return totalKillNum; }
public void setTotalKillNum(int _totalKillNum) { totalKillNum = _totalKillNum; }
public int getTotalHeroKill() { return totalHeroKill; }
public void setTotalHeroKill(int _totalHeroKill) { totalHeroKill = _totalHeroKill; }
public int getTotalHeroDie() { return TotalHeroDie; }
public void setTotalHeroDie(int _TotalHeroDie) { TotalHeroDie = _TotalHeroDie; }
public int getTotalDragronKill() { return totalDragronKill; }
public void setTotalDragronKill(int _totalDragronKill) { totalDragronKill = _totalDragronKill; }
public long getMaxPopulayion() { return maxPopulayion; }
public void setMaxPopulayion(long _maxPopulayion) { maxPopulayion = _maxPopulayion; }
public int getMiningSameTime() { return miningSameTime; }
public void setMiningSameTime(int _miningSameTime) { miningSameTime = _miningSameTime; }
public int getSkillBeBreakDown() { return skillBeBreakDown; }
public void setSkillBeBreakDown(int _skillBeBreakDown) { skillBeBreakDown = _skillBeBreakDown; }
public int getSkillBreakDown() { return skillBreakDown; }
public void setSkillBreakDown(int _skillBreakDown) { skillBreakDown = _skillBreakDown; }


public int GetBufSize() {
	int _size = 40;
	_size += 2;
for(int _i = 0; _i < actorBattleDataList.Count; _i++) {
	_size += 4 + actorBattleDataList[_i].GetBufSize();
	}

	_size += 2;
for(int _i = 0; _i < heroBattleDataList.Count; _i++) {
	_size += 4 + heroBattleDataList[_i].GetBufSize();
	}

	_size += 2;
for(int _i = 0; _i < resDataList.Count; _i++) {
	_size += 4 + resDataList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 42;
	_size += 2;
for(int _i = 0; _i < actorBattleDataList.Count; _i++) {
	_size += 4 + actorBattleDataList[_i].GetBufSize();
	}

	_size += 2;
for(int _i = 0; _i < heroBattleDataList.Count; _i++) {
	_size += 4 + heroBattleDataList[_i].GetBufSize();
	}

	_size += 2;
for(int _i = 0; _i < resDataList.Count; _i++) {
	_size += 4 + resDataList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	groupId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _actorBattleDataListCount = _buf.getShort();
	for(int _i = 0; _i < _actorBattleDataListCount; _i++) { 
		Common.WCGGS2GC_Stastics_ActorBattleData _actorBattleDataList = new Common.WCGGS2GC_Stastics_ActorBattleData();
		int __actorBattleDataListCustLen = _buf.getInt();
	int __actorBattleDataListCurPos = _buf.getCurPos();
	_actorBattleDataList.ReadUnzipBuf(_buf, __actorBattleDataListCurPos + __actorBattleDataListCustLen);
	_buf.setPosition(__actorBattleDataListCurPos + __actorBattleDataListCustLen);

		actorBattleDataList.Add(_actorBattleDataList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _heroBattleDataListCount = _buf.getShort();
	for(int _i = 0; _i < _heroBattleDataListCount; _i++) { 
		Common.WCGGS2GC_Stastics_ActorBattleData _heroBattleDataList = new Common.WCGGS2GC_Stastics_ActorBattleData();
		int __heroBattleDataListCustLen = _buf.getInt();
	int __heroBattleDataListCurPos = _buf.getCurPos();
	_heroBattleDataList.ReadUnzipBuf(_buf, __heroBattleDataListCurPos + __heroBattleDataListCustLen);
	_buf.setPosition(__heroBattleDataListCurPos + __heroBattleDataListCustLen);

		heroBattleDataList.Add(_heroBattleDataList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _resDataListCount = _buf.getShort();
	for(int _i = 0; _i < _resDataListCount; _i++) { 
		Common.WCGGS2GC_Stastics_ResourceBattleData _resDataList = new Common.WCGGS2GC_Stastics_ResourceBattleData();
		int __resDataListCustLen = _buf.getInt();
	int __resDataListCurPos = _buf.getCurPos();
	_resDataList.ReadUnzipBuf(_buf, __resDataListCurPos + __resDataListCustLen);
	_buf.setPosition(__resDataListCurPos + __resDataListCustLen);

		resDataList.Add(_resDataList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	totalKillNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	totalHeroKill = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	TotalHeroDie = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	totalDragronKill = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	maxPopulayion = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	miningSameTime = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	skillBeBreakDown = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	skillBreakDown = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(groupId);
	_buf.putShort((short)actorBattleDataList.Count);
	for(int _i = 0; _i < actorBattleDataList.Count; _i++) { 
		_buf.putInt(actorBattleDataList[_i].GetBufSize());
	actorBattleDataList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)heroBattleDataList.Count);
	for(int _i = 0; _i < heroBattleDataList.Count; _i++) { 
		_buf.putInt(heroBattleDataList[_i].GetBufSize());
	heroBattleDataList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)resDataList.Count);
	for(int _i = 0; _i < resDataList.Count; _i++) { 
		_buf.putInt(resDataList[_i].GetBufSize());
	resDataList[_i].PutUnzipBuf(_buf);
	}
	_buf.putInt(totalKillNum);
	_buf.putInt(totalHeroKill);
	_buf.putInt(TotalHeroDie);
	_buf.putInt(totalDragronKill);
	_buf.putLong(maxPopulayion);
	_buf.putInt(miningSameTime);
	_buf.putInt(skillBeBreakDown);
	_buf.putInt(skillBreakDown);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("groupId").Append(":").Append(groupId.ToString()).Append(", ");
	builder.Append("actorBattleDataList").Append(":").Append(actorBattleDataList.ToString()).Append(", ");
	builder.Append("heroBattleDataList").Append(":").Append(heroBattleDataList.ToString()).Append(", ");
	builder.Append("resDataList").Append(":").Append(resDataList.ToString()).Append(", ");
	builder.Append("totalKillNum").Append(":").Append(totalKillNum.ToString()).Append(", ");
	builder.Append("totalHeroKill").Append(":").Append(totalHeroKill.ToString()).Append(", ");
	builder.Append("TotalHeroDie").Append(":").Append(TotalHeroDie.ToString()).Append(", ");
	builder.Append("totalDragronKill").Append(":").Append(totalDragronKill.ToString()).Append(", ");
	builder.Append("maxPopulayion").Append(":").Append(maxPopulayion.ToString()).Append(", ");
	builder.Append("miningSameTime").Append(":").Append(miningSameTime.ToString()).Append(", ");
	builder.Append("skillBeBreakDown").Append(":").Append(skillBeBreakDown.ToString()).Append(", ");
	builder.Append("skillBreakDown").Append(":").Append(skillBreakDown.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

