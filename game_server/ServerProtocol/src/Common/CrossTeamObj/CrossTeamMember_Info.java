package Common.CrossTeamObj;

import java.nio.ByteBuffer;
/*********
 * 组队成员数据
 **/
public class CrossTeamMember_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 成员ID */
private long cid;
/** 成员职位 */
private Common.CrossTeamEnum.ENPCrossTeamMemberPos pos;
/** 加入时间 */
private long joinMs;


public CrossTeamMember_Info() {
	cid = (long)0;
	pos = Common.CrossTeamEnum.ENPCrossTeamMemberPos.values()[0];
	joinMs = (long)0;
}

public CrossTeamMember_Info(
	 long _cid
	, Common.CrossTeamEnum.ENPCrossTeamMemberPos _pos
	, long _joinMs
) {	cid = _cid;
	pos = _pos;
	joinMs = _joinMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 成员ID */
public long getCid() { return cid; }
/** 成员ID */
public void setCid(long _cid) { cid = _cid; }
/** 成员职位 */
public Common.CrossTeamEnum.ENPCrossTeamMemberPos getPos() { return pos; }
/** 成员职位 */
public void setPos(Common.CrossTeamEnum.ENPCrossTeamMemberPos _pos) { pos = _pos; }
/** 加入时间 */
public long getJoinMs() { return joinMs; }
/** 加入时间 */
public void setJoinMs(long _joinMs) { joinMs = _joinMs; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) pos = Common.CrossTeamEnum.ENPCrossTeamMemberPos.ENPCrossTeamMemberPos_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) joinMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putInt(pos.ordinal());

	_buf.putLong(joinMs);
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

