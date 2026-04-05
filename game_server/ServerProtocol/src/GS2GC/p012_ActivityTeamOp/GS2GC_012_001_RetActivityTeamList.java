package GS2GC.p012_ActivityTeamOp;

import java.nio.ByteBuffer;
public class GS2GC_012_001_RetActivityTeamList implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动队伍基础数据列表 */
private java.util.ArrayList<Common.CrossTeamObj.CrossTeam_BaseInfo> teamBaseList;
/** 队伍总数量，用于客户端分页计算 */
private int totalCount;


public GS2GC_012_001_RetActivityTeamList() {
	teamBaseList = new java.util.ArrayList<Common.CrossTeamObj.CrossTeam_BaseInfo>();
	totalCount = 0;
}

public GS2GC_012_001_RetActivityTeamList(
	 java.util.ArrayList<Common.CrossTeamObj.CrossTeam_BaseInfo> _teamBaseList
	, int _totalCount
) {	teamBaseList = _teamBaseList;
	totalCount = _totalCount;
}

public final byte getMainOrder() { return (byte)12; }

public final byte getSubOrder() { return (byte)1; }

/** 活动队伍基础数据列表 */
public java.util.ArrayList<Common.CrossTeamObj.CrossTeam_BaseInfo> getTeamBaseList() { return teamBaseList; }
/** 活动队伍基础数据列表 */
public void addTeamBaseList(Common.CrossTeamObj.CrossTeam_BaseInfo _teamBaseList) { teamBaseList.add(_teamBaseList); }
/** 队伍总数量，用于客户端分页计算 */
public int getTotalCount() { return totalCount; }
/** 队伍总数量，用于客户端分页计算 */
public void setTotalCount(int _totalCount) { totalCount = _totalCount; }


public final int GetBufSize() {
	int _size = 4;
	_size += 2;
	for(int _i = 0; _i < teamBaseList.size(); _i++) {
	_size += 4 + teamBaseList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 2;
	for(int _i = 0; _i < teamBaseList.size(); _i++) {
	_size += 4 + teamBaseList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _teamBaseListCount = _buf.getShort();
	for(int _i = 0; _i < _teamBaseListCount; _i++) { 
		Common.CrossTeamObj.CrossTeam_BaseInfo _teamBaseList = new Common.CrossTeamObj.CrossTeam_BaseInfo();
		if(_buf.remaining() <= 0) return;
	int __teamBaseListCustLen = _buf.getInt();
	int __teamBaseListCurPos = _buf.position();
	_teamBaseList.ReadUnzipBuf(_buf, __teamBaseListCurPos + __teamBaseListCustLen);
	_buf.position(__teamBaseListCurPos + __teamBaseListCustLen);

		teamBaseList.add(_teamBaseList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalCount = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)teamBaseList.size());
	for(int _i = 0; _i < teamBaseList.size(); _i++) { 
		_buf.putInt(teamBaseList.get(_i).GetBufSize());
	teamBaseList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt(totalCount);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)12);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)12);
	_recBuf.put((byte)1);
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

