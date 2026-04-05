package NP2CRS_R.p001_CrossRankOp;

import java.nio.ByteBuffer;
public class NP2CRS_R_001_008_RegUploadRankData implements ALBasicProtocolPack._IALProtocolStructure {
/** 跨服分组ID */
private long crossInstanceId;
/** 排行榜ID */
private long rankId;
private java.util.ArrayList<Common.ServerObj.ServerObj_RankObjInfo> rankList;


public NP2CRS_R_001_008_RegUploadRankData() {
	crossInstanceId = (long)0;
	rankId = (long)0;
	rankList = new java.util.ArrayList<Common.ServerObj.ServerObj_RankObjInfo>();
}

public NP2CRS_R_001_008_RegUploadRankData(
	 long _crossInstanceId
	, long _rankId
	, java.util.ArrayList<Common.ServerObj.ServerObj_RankObjInfo> _rankList
) {	crossInstanceId = _crossInstanceId;
	rankId = _rankId;
	rankList = _rankList;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)8; }

/** 跨服分组ID */
public long getCrossInstanceId() { return crossInstanceId; }
/** 跨服分组ID */
public void setCrossInstanceId(long _crossInstanceId) { crossInstanceId = _crossInstanceId; }
/** 排行榜ID */
public long getRankId() { return rankId; }
/** 排行榜ID */
public void setRankId(long _rankId) { rankId = _rankId; }
public java.util.ArrayList<Common.ServerObj.ServerObj_RankObjInfo> getRankList() { return rankList; }
public void addRankList(Common.ServerObj.ServerObj_RankObjInfo _rankList) { rankList.add(_rankList); }


public final int GetBufSize() {
	int _size = 16;
	_size += 2;
	for(int _i = 0; _i < rankList.size(); _i++) {
	_size += 4 + rankList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += 2;
	for(int _i = 0; _i < rankList.size(); _i++) {
	_size += 4 + rankList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) crossInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rankId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _rankListCount = _buf.getShort();
	for(int _i = 0; _i < _rankListCount; _i++) { 
		Common.ServerObj.ServerObj_RankObjInfo _rankList = new Common.ServerObj.ServerObj_RankObjInfo();
		if(_buf.remaining() <= 0) return;
	int __rankListCustLen = _buf.getInt();
	int __rankListCurPos = _buf.position();
	_rankList.ReadUnzipBuf(_buf, __rankListCurPos + __rankListCustLen);
	_buf.position(__rankListCurPos + __rankListCustLen);

		rankList.add(_rankList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(crossInstanceId);
	_buf.putLong(rankId);
	_buf.putShort((short)rankList.size());
	for(int _i = 0; _i < rankList.size(); _i++) { 
		_buf.putInt(rankList.get(_i).GetBufSize());
	rankList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)8);
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

