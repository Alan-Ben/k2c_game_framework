package ALLRPC.DinnerServer.Dinner;

import java.nio.ByteBuffer;
public class DnsAddDinner_Req implements ALBasicProtocolPack._IALProtocolStructure {
/** 分组ID */
private long groupId;
/** 宴会索引数据 */
private Common.DinnerObj.Dinner_Idx dinnerIdx;
/** 宴会开启时间 */
private int startTs;
/** 宴会排序ID */
private int sortId;
/** 宴会参与玩家CID列表 */
private java.util.ArrayList<Long> joinerCidList;


public DnsAddDinner_Req() {
	groupId = (long)0;
	dinnerIdx = new Common.DinnerObj.Dinner_Idx();
	startTs = 0;
	sortId = 0;
	joinerCidList = new java.util.ArrayList<Long>();
}

public DnsAddDinner_Req(
	 long _groupId
	, Common.DinnerObj.Dinner_Idx _dinnerIdx
	, int _startTs
	, int _sortId
	, java.util.ArrayList<Long> _joinerCidList
) {	groupId = _groupId;
	dinnerIdx = _dinnerIdx;
	startTs = _startTs;
	sortId = _sortId;
	joinerCidList = _joinerCidList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 分组ID */
public long getGroupId() { return groupId; }
/** 分组ID */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** 宴会索引数据 */
public Common.DinnerObj.Dinner_Idx getDinnerIdx() { return dinnerIdx; }
/** 宴会索引数据 */
public void setDinnerIdx(Common.DinnerObj.Dinner_Idx _dinnerIdx) { dinnerIdx = _dinnerIdx; }
/** 宴会开启时间 */
public int getStartTs() { return startTs; }
/** 宴会开启时间 */
public void setStartTs(int _startTs) { startTs = _startTs; }
/** 宴会排序ID */
public int getSortId() { return sortId; }
/** 宴会排序ID */
public void setSortId(int _sortId) { sortId = _sortId; }
/** 宴会参与玩家CID列表 */
public java.util.ArrayList<Long> getJoinerCidList() { return joinerCidList; }
/** 宴会参与玩家CID列表 */
public void addJoinerCidList(long _joinerCidList) { joinerCidList.add(_joinerCidList); }


public final int GetBufSize() {
	int _size = 73;
	_size += 2 + (joinerCidList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 75;
	_size += 2 + (joinerCidList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _dinnerIdxCustLen = _buf.getInt();
	int _dinnerIdxCurPos = _buf.position();
	dinnerIdx.ReadUnzipBuf(_buf, _dinnerIdxCurPos + _dinnerIdxCustLen);
	_buf.position(_dinnerIdxCurPos + _dinnerIdxCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startTs = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sortId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _joinerCidListCount = _buf.getShort();
	for(int _i = 0; _i < _joinerCidListCount; _i++) { 
		long _joinerCidList = (long)0;
		if(_buf.remaining() > 0) _joinerCidList = _buf.getLong();
		joinerCidList.add(_joinerCidList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(groupId);
	_buf.putInt(dinnerIdx.GetBufSize());
	dinnerIdx.PutUnzipBuf(_buf);
	_buf.putInt(startTs);
	_buf.putInt(sortId);
	_buf.putShort((short)joinerCidList.size());
	for(int _i = 0; _i < joinerCidList.size(); _i++) { 
		_buf.putLong(joinerCidList.get(_i));
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

