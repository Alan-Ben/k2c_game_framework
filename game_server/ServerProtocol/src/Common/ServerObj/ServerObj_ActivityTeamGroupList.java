package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 活动队伍-活动下的队伍列表
 **/
public class ServerObj_ActivityTeamGroupList implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动组ID */
private long groupId;
/** 队伍列表 */
private java.util.ArrayList<Common.ServerObj.ServerObj_ActivityTeamGroup> teamList;


public ServerObj_ActivityTeamGroupList() {
	groupId = (long)0;
	teamList = new java.util.ArrayList<Common.ServerObj.ServerObj_ActivityTeamGroup>();
}

public ServerObj_ActivityTeamGroupList(
	 long _groupId
	, java.util.ArrayList<Common.ServerObj.ServerObj_ActivityTeamGroup> _teamList
) {	groupId = _groupId;
	teamList = _teamList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 活动组ID */
public long getGroupId() { return groupId; }
/** 活动组ID */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** 队伍列表 */
public java.util.ArrayList<Common.ServerObj.ServerObj_ActivityTeamGroup> getTeamList() { return teamList; }
/** 队伍列表 */
public void addTeamList(Common.ServerObj.ServerObj_ActivityTeamGroup _teamList) { teamList.add(_teamList); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (teamList.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (teamList.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _teamListCount = _buf.getShort();
	for(int _i = 0; _i < _teamListCount; _i++) { 
		Common.ServerObj.ServerObj_ActivityTeamGroup _teamList = new Common.ServerObj.ServerObj_ActivityTeamGroup();
		if(_buf.remaining() <= 0) return;
	int __teamListCustLen = _buf.getInt();
	int __teamListCurPos = _buf.position();
	_teamList.ReadUnzipBuf(_buf, __teamListCurPos + __teamListCustLen);
	_buf.position(__teamListCurPos + __teamListCustLen);

		teamList.add(_teamList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(groupId);
	_buf.putShort((short)teamList.size());
	for(int _i = 0; _i < teamList.size(); _i++) { 
		_buf.putInt(teamList.get(_i).GetBufSize());
	teamList.get(_i).PutUnzipBuf(_buf);
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

