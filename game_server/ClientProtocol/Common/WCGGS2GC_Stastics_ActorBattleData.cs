using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_Stastics_ActorBattleData : ALBasicProtocolPack._IALProtocolStructure {
private long ownerId;
private long actorId;
private int actorLevel;
private List<long> dataList;
private List<Common.WCGGS2GC_Stastics_ActorBattleReliveCostData> reliveCostDataList;


public WCGGS2GC_Stastics_ActorBattleData() {
	ownerId = (long)0;
	actorId = (long)0;
	actorLevel = 0;
	dataList = new List<long>();
	reliveCostDataList = new List<Common.WCGGS2GC_Stastics_ActorBattleReliveCostData>();
}

public WCGGS2GC_Stastics_ActorBattleData(
	long _ownerId
	, long _actorId
	, int _actorLevel
	, List<long> _dataList
	, List<Common.WCGGS2GC_Stastics_ActorBattleReliveCostData> _reliveCostDataList
) {	ownerId = _ownerId;
	actorId = _actorId;
	actorLevel = _actorLevel;
	dataList = _dataList;
	reliveCostDataList = _reliveCostDataList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getOwnerId() { return ownerId; }
public void setOwnerId(long _ownerId) { ownerId = _ownerId; }
public long getActorId() { return actorId; }
public void setActorId(long _actorId) { actorId = _actorId; }
public int getActorLevel() { return actorLevel; }
public void setActorLevel(int _actorLevel) { actorLevel = _actorLevel; }
public List<long> getDataList() { return dataList; }
public void addDataList(long _dataList) { dataList.Add(_dataList); }
public List<Common.WCGGS2GC_Stastics_ActorBattleReliveCostData> getReliveCostDataList() { return reliveCostDataList; }
public void addReliveCostDataList(Common.WCGGS2GC_Stastics_ActorBattleReliveCostData _reliveCostDataList) { reliveCostDataList.Add(_reliveCostDataList); }


public int GetBufSize() {
	int _size = 20;
	_size += 2 + (dataList.Count * 8);
	_size += 2 + (reliveCostDataList.Count * 12);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;
	_size += 2 + (dataList.Count * 8);
	_size += 2 + (reliveCostDataList.Count * 12);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	ownerId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	actorId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	actorLevel = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _dataListCount = _buf.getShort();
	for(int _i = 0; _i < _dataListCount; _i++) { 
		long _dataList = (long)0;
		_dataList = _buf.getLong();
		dataList.Add(_dataList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _reliveCostDataListCount = _buf.getShort();
	for(int _i = 0; _i < _reliveCostDataListCount; _i++) { 
		Common.WCGGS2GC_Stastics_ActorBattleReliveCostData _reliveCostDataList = new Common.WCGGS2GC_Stastics_ActorBattleReliveCostData();
		int __reliveCostDataListCustLen = _buf.getInt();
	int __reliveCostDataListCurPos = _buf.getCurPos();
	_reliveCostDataList.ReadUnzipBuf(_buf, __reliveCostDataListCurPos + __reliveCostDataListCustLen);
	_buf.setPosition(__reliveCostDataListCurPos + __reliveCostDataListCustLen);

		reliveCostDataList.Add(_reliveCostDataList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(ownerId);
	_buf.putLong(actorId);
	_buf.putInt(actorLevel);
	_buf.putShort((short)dataList.Count);
	for(int _i = 0; _i < dataList.Count; _i++) { 
		_buf.putLong(dataList[_i]);
	}
	_buf.putShort((short)reliveCostDataList.Count);
	for(int _i = 0; _i < reliveCostDataList.Count; _i++) { 
		_buf.putInt(reliveCostDataList[_i].GetBufSize());
	reliveCostDataList[_i].PutUnzipBuf(_buf);
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
	builder.Append("ownerId").Append(":").Append(ownerId.ToString()).Append(", ");
	builder.Append("actorId").Append(":").Append(actorId.ToString()).Append(", ");
	builder.Append("actorLevel").Append(":").Append(actorLevel.ToString()).Append(", ");
	builder.Append("dataList").Append(":").Append(dataList.ToString()).Append(", ");
	builder.Append("reliveCostDataList").Append(":").Append(reliveCostDataList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

