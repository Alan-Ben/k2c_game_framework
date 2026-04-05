package GS2GC.p041_MarsExploreOp;

import java.nio.ByteBuffer;
/*********
 * 火星探索队伍-损耗数量变更
 **/
public class GS2GC_041_062_OnExploreTeamLossValueChg implements ALBasicProtocolPack._IALProtocolStructure {
private long teamId;
/** 队伍损耗数量 */
private long lossValue;


public GS2GC_041_062_OnExploreTeamLossValueChg() {
	teamId = (long)0;
	lossValue = (long)0;
}

public GS2GC_041_062_OnExploreTeamLossValueChg(
	 long _teamId
	, long _lossValue
) {	teamId = _teamId;
	lossValue = _lossValue;
}

public final byte getMainOrder() { return (byte)41; }

public final byte getSubOrder() { return (byte)62; }

public long getTeamId() { return teamId; }
public void setTeamId(long _teamId) { teamId = _teamId; }
/** 队伍损耗数量 */
public long getLossValue() { return lossValue; }
/** 队伍损耗数量 */
public void setLossValue(long _lossValue) { lossValue = _lossValue; }


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
	if(_buf.remaining() > 0) lossValue = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(teamId);
	_buf.putLong(lossValue);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)62);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)62);
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

