package GS2GC.p012_ActivityTeamOp;

import java.nio.ByteBuffer;
/*********
 * 活动队伍成员增加
 **/
public class GS2GC_012_055_OnActivityTeamMemberAdd implements ALBasicProtocolPack._IALProtocolStructure {
/** 队伍实例ID */
private long teamId;
/** 队伍玩家数据 */
private Common.CrossTeamObj.CrossTeamMember_Info member;


public GS2GC_012_055_OnActivityTeamMemberAdd() {
	teamId = (long)0;
	member = new Common.CrossTeamObj.CrossTeamMember_Info();
}

public GS2GC_012_055_OnActivityTeamMemberAdd(
	 long _teamId
	, Common.CrossTeamObj.CrossTeamMember_Info _member
) {	teamId = _teamId;
	member = _member;
}

public final byte getMainOrder() { return (byte)12; }

public final byte getSubOrder() { return (byte)55; }

/** 队伍实例ID */
public long getTeamId() { return teamId; }
/** 队伍实例ID */
public void setTeamId(long _teamId) { teamId = _teamId; }
/** 队伍玩家数据 */
public Common.CrossTeamObj.CrossTeamMember_Info getMember() { return member; }
/** 队伍玩家数据 */
public void setMember(Common.CrossTeamObj.CrossTeamMember_Info _member) { member = _member; }


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _memberCustLen = _buf.getInt();
	int _memberCurPos = _buf.position();
	member.ReadUnzipBuf(_buf, _memberCurPos + _memberCustLen);
	_buf.position(_memberCurPos + _memberCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(teamId);
	_buf.putInt(member.GetBufSize());
	member.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)12);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)12);
	_recBuf.put((byte)55);
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

