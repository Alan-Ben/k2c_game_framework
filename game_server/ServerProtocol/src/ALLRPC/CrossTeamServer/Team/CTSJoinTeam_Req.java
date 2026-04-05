package ALLRPC.CrossTeamServer.Team;

import java.nio.ByteBuffer;
public class CTSJoinTeam_Req implements ALBasicProtocolPack._IALProtocolStructure {
/** 队伍实例ID */
private long teamId;
/** 玩家CID */
private long cid;
/** 玩家入队条件列表 */
private java.util.ArrayList<Common.CrossTeamObj.CrossTeam_SetInfo_Join> joinCondList;


public CTSJoinTeam_Req() {
	teamId = (long)0;
	cid = (long)0;
	joinCondList = new java.util.ArrayList<Common.CrossTeamObj.CrossTeam_SetInfo_Join>();
}

public CTSJoinTeam_Req(
	 long _teamId
	, long _cid
	, java.util.ArrayList<Common.CrossTeamObj.CrossTeam_SetInfo_Join> _joinCondList
) {	teamId = _teamId;
	cid = _cid;
	joinCondList = _joinCondList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 队伍实例ID */
public long getTeamId() { return teamId; }
/** 队伍实例ID */
public void setTeamId(long _teamId) { teamId = _teamId; }
/** 玩家CID */
public long getCid() { return cid; }
/** 玩家CID */
public void setCid(long _cid) { cid = _cid; }
/** 玩家入队条件列表 */
public java.util.ArrayList<Common.CrossTeamObj.CrossTeam_SetInfo_Join> getJoinCondList() { return joinCondList; }
/** 玩家入队条件列表 */
public void addJoinCondList(Common.CrossTeamObj.CrossTeam_SetInfo_Join _joinCondList) { joinCondList.add(_joinCondList); }


public final int GetBufSize() {
	int _size = 16;
	_size += 2 + (joinCondList.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += 2 + (joinCondList.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _joinCondListCount = _buf.getShort();
	for(int _i = 0; _i < _joinCondListCount; _i++) { 
		Common.CrossTeamObj.CrossTeam_SetInfo_Join _joinCondList = new Common.CrossTeamObj.CrossTeam_SetInfo_Join();
		if(_buf.remaining() <= 0) return;
	int __joinCondListCustLen = _buf.getInt();
	int __joinCondListCurPos = _buf.position();
	_joinCondList.ReadUnzipBuf(_buf, __joinCondListCurPos + __joinCondListCustLen);
	_buf.position(__joinCondListCurPos + __joinCondListCustLen);

		joinCondList.add(_joinCondList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(teamId);
	_buf.putLong(cid);
	_buf.putShort((short)joinCondList.size());
	for(int _i = 0; _i < joinCondList.size(); _i++) { 
		_buf.putInt(joinCondList.get(_i).GetBufSize());
	joinCondList.get(_i).PutUnzipBuf(_buf);
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

