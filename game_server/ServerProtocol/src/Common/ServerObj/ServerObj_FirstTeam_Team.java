package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * GLS GroupMgr-队伍数据
 **/
public class ServerObj_FirstTeam_Team implements ALBasicProtocolPack._IALProtocolStructure {
/** 队长CID */
private long leaderCid;
/** 成员列表 */
private java.util.ArrayList<Common.ServerObj.ServerObj_GLSTeamMember> memberList;


public ServerObj_FirstTeam_Team() {
	leaderCid = (long)0;
	memberList = new java.util.ArrayList<Common.ServerObj.ServerObj_GLSTeamMember>();
}

public ServerObj_FirstTeam_Team(
	 long _leaderCid
	, java.util.ArrayList<Common.ServerObj.ServerObj_GLSTeamMember> _memberList
) {	leaderCid = _leaderCid;
	memberList = _memberList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 队长CID */
public long getLeaderCid() { return leaderCid; }
/** 队长CID */
public void setLeaderCid(long _leaderCid) { leaderCid = _leaderCid; }
/** 成员列表 */
public java.util.ArrayList<Common.ServerObj.ServerObj_GLSTeamMember> getMemberList() { return memberList; }
/** 成员列表 */
public void addMemberList(Common.ServerObj.ServerObj_GLSTeamMember _memberList) { memberList.add(_memberList); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (memberList.size() * 12);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (memberList.size() * 12);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) leaderCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _memberListCount = _buf.getShort();
	for(int _i = 0; _i < _memberListCount; _i++) { 
		Common.ServerObj.ServerObj_GLSTeamMember _memberList = new Common.ServerObj.ServerObj_GLSTeamMember();
		if(_buf.remaining() <= 0) return;
	int __memberListCustLen = _buf.getInt();
	int __memberListCurPos = _buf.position();
	_memberList.ReadUnzipBuf(_buf, __memberListCurPos + __memberListCustLen);
	_buf.position(__memberListCurPos + __memberListCustLen);

		memberList.add(_memberList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(leaderCid);
	_buf.putShort((short)memberList.size());
	for(int _i = 0; _i < memberList.size(); _i++) { 
		_buf.putInt(memberList.get(_i).GetBufSize());
	memberList.get(_i).PutUnzipBuf(_buf);
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

