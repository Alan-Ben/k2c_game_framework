package GC2GS.p032_GuildOp.GuildOpStructure;

import java.nio.ByteBuffer;
/*********
 * 加入联盟的附加信息
 **/
public class GuildOp_002_UserJoinInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 赚速 */
private long earnings;
/** 等级 */
private long level;
/** 玩家是否达到申请上限 */
private boolean canRequest;


public GuildOp_002_UserJoinInfo() {
	earnings = (long)0;
	level = (long)0;
	canRequest = false;
}

public GuildOp_002_UserJoinInfo(
	 long _earnings
	, long _level
	, boolean _canRequest
) {	earnings = _earnings;
	level = _level;
	canRequest = _canRequest;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 赚速 */
public long getEarnings() { return earnings; }
/** 赚速 */
public void setEarnings(long _earnings) { earnings = _earnings; }
/** 等级 */
public long getLevel() { return level; }
/** 等级 */
public void setLevel(long _level) { level = _level; }
/** 玩家是否达到申请上限 */
public boolean getCanRequest() { return canRequest; }
/** 玩家是否达到申请上限 */
public void setCanRequest(boolean _canRequest) { canRequest = _canRequest; }


public final int GetBufSize() {
	int _size = 17;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) earnings = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) level = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) canRequest = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(earnings);
	_buf.putLong(level);
	_buf.put(canRequest?(byte)1:(byte)0);
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

