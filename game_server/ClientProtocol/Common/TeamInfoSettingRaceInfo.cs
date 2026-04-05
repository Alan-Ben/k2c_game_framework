using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class TeamInfoSettingRaceInfo : ALBasicProtocolPack._IALProtocolStructure {
private long raceId;
private List<long> actorIdList;
private List<long> heroIdList;
private long selectHouseActorId;
private long selectBowTowerActorId;
private List<long> allActorIdList;


public TeamInfoSettingRaceInfo() {
	raceId = (long)0;
	actorIdList = new List<long>();
	heroIdList = new List<long>();
	selectHouseActorId = (long)0;
	selectBowTowerActorId = (long)0;
	allActorIdList = new List<long>();
}

public TeamInfoSettingRaceInfo(
	long _raceId
	, List<long> _actorIdList
	, List<long> _heroIdList
	, long _selectHouseActorId
	, long _selectBowTowerActorId
	, List<long> _allActorIdList
) {	raceId = _raceId;
	actorIdList = _actorIdList;
	heroIdList = _heroIdList;
	selectHouseActorId = _selectHouseActorId;
	selectBowTowerActorId = _selectBowTowerActorId;
	allActorIdList = _allActorIdList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getRaceId() { return raceId; }
public void setRaceId(long _raceId) { raceId = _raceId; }
public List<long> getActorIdList() { return actorIdList; }
public void addActorIdList(long _actorIdList) { actorIdList.Add(_actorIdList); }
public List<long> getHeroIdList() { return heroIdList; }
public void addHeroIdList(long _heroIdList) { heroIdList.Add(_heroIdList); }
public long getSelectHouseActorId() { return selectHouseActorId; }
public void setSelectHouseActorId(long _selectHouseActorId) { selectHouseActorId = _selectHouseActorId; }
public long getSelectBowTowerActorId() { return selectBowTowerActorId; }
public void setSelectBowTowerActorId(long _selectBowTowerActorId) { selectBowTowerActorId = _selectBowTowerActorId; }
public List<long> getAllActorIdList() { return allActorIdList; }
public void addAllActorIdList(long _allActorIdList) { allActorIdList.Add(_allActorIdList); }


public int GetBufSize() {
	int _size = 24;
	_size += 2 + (actorIdList.Count * 8);
	_size += 2 + (heroIdList.Count * 8);
	_size += 2 + (allActorIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;
	_size += 2 + (actorIdList.Count * 8);
	_size += 2 + (heroIdList.Count * 8);
	_size += 2 + (allActorIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	raceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _actorIdListCount = _buf.getShort();
	for(int _i = 0; _i < _actorIdListCount; _i++) { 
		long _actorIdList = (long)0;
		_actorIdList = _buf.getLong();
		actorIdList.Add(_actorIdList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _heroIdListCount = _buf.getShort();
	for(int _i = 0; _i < _heroIdListCount; _i++) { 
		long _heroIdList = (long)0;
		_heroIdList = _buf.getLong();
		heroIdList.Add(_heroIdList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	selectHouseActorId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	selectBowTowerActorId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _allActorIdListCount = _buf.getShort();
	for(int _i = 0; _i < _allActorIdListCount; _i++) { 
		long _allActorIdList = (long)0;
		_allActorIdList = _buf.getLong();
		allActorIdList.Add(_allActorIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(raceId);
	_buf.putShort((short)actorIdList.Count);
	for(int _i = 0; _i < actorIdList.Count; _i++) { 
		_buf.putLong(actorIdList[_i]);
	}
	_buf.putShort((short)heroIdList.Count);
	for(int _i = 0; _i < heroIdList.Count; _i++) { 
		_buf.putLong(heroIdList[_i]);
	}
	_buf.putLong(selectHouseActorId);
	_buf.putLong(selectBowTowerActorId);
	_buf.putShort((short)allActorIdList.Count);
	for(int _i = 0; _i < allActorIdList.Count; _i++) { 
		_buf.putLong(allActorIdList[_i]);
	}
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
	builder.Append("raceId").Append(":").Append(raceId.ToString()).Append(", ");
	builder.Append("actorIdList").Append(":").Append(actorIdList.ToString()).Append(", ");
	builder.Append("heroIdList").Append(":").Append(heroIdList.ToString()).Append(", ");
	builder.Append("selectHouseActorId").Append(":").Append(selectHouseActorId.ToString()).Append(", ");
	builder.Append("selectBowTowerActorId").Append(":").Append(selectBowTowerActorId.ToString()).Append(", ");
	builder.Append("allActorIdList").Append(":").Append(allActorIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

