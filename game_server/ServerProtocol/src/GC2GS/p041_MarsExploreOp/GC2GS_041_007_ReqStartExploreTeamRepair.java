package GC2GS.p041_MarsExploreOp;

import java.nio.ByteBuffer;
/*********
 * 火星探险-修复队伍
 **/
public class GC2GS_041_007_ReqStartExploreTeamRepair implements ALBasicProtocolPack._IALProtocolStructure {
private long teamId;
/** 修复数量，0-当前全部伤兵 */
private long repairNum;


public GC2GS_041_007_ReqStartExploreTeamRepair() {
	teamId = (long)0;
	repairNum = (long)0;
}

public GC2GS_041_007_ReqStartExploreTeamRepair(
	 long _teamId
	, long _repairNum
) {	teamId = _teamId;
	repairNum = _repairNum;
}

public final byte getMainOrder() { return (byte)41; }

public final byte getSubOrder() { return (byte)7; }

public long getTeamId() { return teamId; }
public void setTeamId(long _teamId) { teamId = _teamId; }
/** 修复数量，0-当前全部伤兵 */
public long getRepairNum() { return repairNum; }
/** 修复数量，0-当前全部伤兵 */
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
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)7);
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

