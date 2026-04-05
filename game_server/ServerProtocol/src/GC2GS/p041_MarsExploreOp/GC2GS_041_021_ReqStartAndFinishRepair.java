package GC2GS.p041_MarsExploreOp;

import java.nio.ByteBuffer;
/*********
 * 火星探险-开始并立即完成队伍维修
 **/
public class GC2GS_041_021_ReqStartAndFinishRepair implements ALBasicProtocolPack._IALProtocolStructure {
/** 队伍ID */
private long teamId;
/** 修复士兵数，0表示全部 */
private long repairNum;


public GC2GS_041_021_ReqStartAndFinishRepair() {
	teamId = (long)0;
	repairNum = (long)0;
}

public GC2GS_041_021_ReqStartAndFinishRepair(
	 long _teamId
	, long _repairNum
) {	teamId = _teamId;
	repairNum = _repairNum;
}

public final byte getMainOrder() { return (byte)41; }

public final byte getSubOrder() { return (byte)21; }

/** 队伍ID */
public long getTeamId() { return teamId; }
/** 队伍ID */
public void setTeamId(long _teamId) { teamId = _teamId; }
/** 修复士兵数，0表示全部 */
public long getRepairNum() { return repairNum; }
/** 修复士兵数，0表示全部 */
public void setRepairNum(long _repairNum) { repairNum = _repairNum; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) repairNum = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(teamId);
	_buf.putLong(repairNum);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)21);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)21);
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

