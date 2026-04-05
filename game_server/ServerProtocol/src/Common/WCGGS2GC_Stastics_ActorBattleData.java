package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_Stastics_ActorBattleData implements ALBasicProtocolPack._IALProtocolStructure {
private long ownerId;
private long actorId;
private int actorLevel;
private java.util.ArrayList<Long> dataList;
private java.util.ArrayList<Common.WCGGS2GC_Stastics_ActorBattleReliveCostData> reliveCostDataList;


public WCGGS2GC_Stastics_ActorBattleData() {
	ownerId = (long)0;
	actorId = (long)0;
	actorLevel = 0;
	dataList = new java.util.ArrayList<Long>();
	reliveCostDataList = new java.util.ArrayList<Common.WCGGS2GC_Stastics_ActorBattleReliveCostData>();
}

public WCGGS2GC_Stastics_ActorBattleData(
	 long _ownerId
	, long _actorId
	, int _actorLevel
	, java.util.ArrayList<Long> _dataList
	, java.util.ArrayList<Common.WCGGS2GC_Stastics_ActorBattleReliveCostData> _reliveCostDataList
) {	ownerId = _ownerId;
	actorId = _actorId;
	actorLevel = _actorLevel;
	dataList = _dataList;
	reliveCostDataList = _reliveCostDataList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getOwnerId() { return ownerId; }
public void setOwnerId(long _ownerId) { ownerId = _ownerId; }
public long getActorId() { return actorId; }
public void setActorId(long _actorId) { actorId = _actorId; }
public int getActorLevel() { return actorLevel; }
public void setActorLevel(int _actorLevel) { actorLevel = _actorLevel; }
public java.util.ArrayList<Long> getDataList() { return dataList; }
public void addDataList(long _dataList) { dataList.add(_dataList); }
public java.util.ArrayList<Common.WCGGS2GC_Stastics_ActorBattleReliveCostData> getReliveCostDataList() { return reliveCostDataList; }
public void addReliveCostDataList(Common.WCGGS2GC_Stastics_ActorBattleReliveCostData _reliveCostDataList) { reliveCostDataList.add(_reliveCostDataList); }


public final int GetBufSize() {
	int _size = 20;
	_size += 2 + (dataList.size() * 8);
	_size += 2 + (reliveCostDataList.size() * 12);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;
	_size += 2 + (dataList.size() * 8);
	_size += 2 + (reliveCostDataList.size() * 12);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) ownerId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) actorId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) actorLevel = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _dataListCount = _buf.getShort();
	for(int _i = 0; _i < _dataListCount; _i++) { 
		long _dataList = (long)0;
		if(_buf.remaining() > 0) _dataList = _buf.getLong();
		dataList.add(_dataList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _reliveCostDataListCount = _buf.getShort();
	for(int _i = 0; _i < _reliveCostDataListCount; _i++) { 
		Common.WCGGS2GC_Stastics_ActorBattleReliveCostData _reliveCostDataList = new Common.WCGGS2GC_Stastics_ActorBattleReliveCostData();
		if(_buf.remaining() <= 0) return;
	int __reliveCostDataListCustLen = _buf.getInt();
	int __reliveCostDataListCurPos = _buf.position();
	_reliveCostDataList.ReadUnzipBuf(_buf, __reliveCostDataListCurPos + __reliveCostDataListCustLen);
	_buf.position(__reliveCostDataListCurPos + __reliveCostDataListCustLen);

		reliveCostDataList.add(_reliveCostDataList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(ownerId);
	_buf.putLong(actorId);
	_buf.putInt(actorLevel);
	_buf.putShort((short)dataList.size());
	for(int _i = 0; _i < dataList.size(); _i++) { 
		_buf.putLong(dataList.get(_i));
	}
	_buf.putShort((short)reliveCostDataList.size());
	for(int _i = 0; _i < reliveCostDataList.size(); _i++) { 
		_buf.putInt(reliveCostDataList.get(_i).GetBufSize());
	reliveCostDataList.get(_i).PutUnzipBuf(_buf);
	}
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

