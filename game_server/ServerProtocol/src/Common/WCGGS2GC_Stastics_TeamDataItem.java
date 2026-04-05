package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_Stastics_TeamDataItem implements ALBasicProtocolPack._IALProtocolStructure {
private int groupId;
private java.util.ArrayList<Common.WCGGS2GC_Stastics_ActorBattleData> actorBattleDataList;
private java.util.ArrayList<Common.WCGGS2GC_Stastics_ActorBattleData> heroBattleDataList;
private java.util.ArrayList<Common.WCGGS2GC_Stastics_ResourceBattleData> resDataList;
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
	actorBattleDataList = new java.util.ArrayList<Common.WCGGS2GC_Stastics_ActorBattleData>();
	heroBattleDataList = new java.util.ArrayList<Common.WCGGS2GC_Stastics_ActorBattleData>();
	resDataList = new java.util.ArrayList<Common.WCGGS2GC_Stastics_ResourceBattleData>();
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
	, java.util.ArrayList<Common.WCGGS2GC_Stastics_ActorBattleData> _actorBattleDataList
	, java.util.ArrayList<Common.WCGGS2GC_Stastics_ActorBattleData> _heroBattleDataList
	, java.util.ArrayList<Common.WCGGS2GC_Stastics_ResourceBattleData> _resDataList
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

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public int getGroupId() { return groupId; }
public void setGroupId(int _groupId) { groupId = _groupId; }
public java.util.ArrayList<Common.WCGGS2GC_Stastics_ActorBattleData> getActorBattleDataList() { return actorBattleDataList; }
public void addActorBattleDataList(Common.WCGGS2GC_Stastics_ActorBattleData _actorBattleDataList) { actorBattleDataList.add(_actorBattleDataList); }
public java.util.ArrayList<Common.WCGGS2GC_Stastics_ActorBattleData> getHeroBattleDataList() { return heroBattleDataList; }
public void addHeroBattleDataList(Common.WCGGS2GC_Stastics_ActorBattleData _heroBattleDataList) { heroBattleDataList.add(_heroBattleDataList); }
public java.util.ArrayList<Common.WCGGS2GC_Stastics_ResourceBattleData> getResDataList() { return resDataList; }
public void addResDataList(Common.WCGGS2GC_Stastics_ResourceBattleData _resDataList) { resDataList.add(_resDataList); }
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


public final int GetBufSize() {
	int _size = 40;
	_size += 2;
	for(int _i = 0; _i < actorBattleDataList.size(); _i++) {
	_size += 4 + actorBattleDataList.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < heroBattleDataList.size(); _i++) {
	_size += 4 + heroBattleDataList.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < resDataList.size(); _i++) {
	_size += 4 + resDataList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 42;
	_size += 2;
	for(int _i = 0; _i < actorBattleDataList.size(); _i++) {
	_size += 4 + actorBattleDataList.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < heroBattleDataList.size(); _i++) {
	_size += 4 + heroBattleDataList.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < resDataList.size(); _i++) {
	_size += 4 + resDataList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _actorBattleDataListCount = _buf.getShort();
	for(int _i = 0; _i < _actorBattleDataListCount; _i++) { 
		Common.WCGGS2GC_Stastics_ActorBattleData _actorBattleDataList = new Common.WCGGS2GC_Stastics_ActorBattleData();
		if(_buf.remaining() <= 0) return;
	int __actorBattleDataListCustLen = _buf.getInt();
	int __actorBattleDataListCurPos = _buf.position();
	_actorBattleDataList.ReadUnzipBuf(_buf, __actorBattleDataListCurPos + __actorBattleDataListCustLen);
	_buf.position(__actorBattleDataListCurPos + __actorBattleDataListCustLen);

		actorBattleDataList.add(_actorBattleDataList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _heroBattleDataListCount = _buf.getShort();
	for(int _i = 0; _i < _heroBattleDataListCount; _i++) { 
		Common.WCGGS2GC_Stastics_ActorBattleData _heroBattleDataList = new Common.WCGGS2GC_Stastics_ActorBattleData();
		if(_buf.remaining() <= 0) return;
	int __heroBattleDataListCustLen = _buf.getInt();
	int __heroBattleDataListCurPos = _buf.position();
	_heroBattleDataList.ReadUnzipBuf(_buf, __heroBattleDataListCurPos + __heroBattleDataListCustLen);
	_buf.position(__heroBattleDataListCurPos + __heroBattleDataListCustLen);

		heroBattleDataList.add(_heroBattleDataList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _resDataListCount = _buf.getShort();
	for(int _i = 0; _i < _resDataListCount; _i++) { 
		Common.WCGGS2GC_Stastics_ResourceBattleData _resDataList = new Common.WCGGS2GC_Stastics_ResourceBattleData();
		if(_buf.remaining() <= 0) return;
	int __resDataListCustLen = _buf.getInt();
	int __resDataListCurPos = _buf.position();
	_resDataList.ReadUnzipBuf(_buf, __resDataListCurPos + __resDataListCustLen);
	_buf.position(__resDataListCurPos + __resDataListCustLen);

		resDataList.add(_resDataList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalKillNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalHeroKill = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) TotalHeroDie = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalDragronKill = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) maxPopulayion = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) miningSameTime = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) skillBeBreakDown = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) skillBreakDown = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(groupId);
	_buf.putShort((short)actorBattleDataList.size());
	for(int _i = 0; _i < actorBattleDataList.size(); _i++) { 
		_buf.putInt(actorBattleDataList.get(_i).GetBufSize());
	actorBattleDataList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)heroBattleDataList.size());
	for(int _i = 0; _i < heroBattleDataList.size(); _i++) { 
		_buf.putInt(heroBattleDataList.get(_i).GetBufSize());
	heroBattleDataList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)resDataList.size());
	for(int _i = 0; _i < resDataList.size(); _i++) { 
		_buf.putInt(resDataList.get(_i).GetBufSize());
	resDataList.get(_i).PutUnzipBuf(_buf);
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

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

