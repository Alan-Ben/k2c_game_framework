package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟加入cd信息
 **/
public class Guild_JoinCdInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 联盟免cd加入次数 */
private int guildFreeJoinCdNum;
/** 加入联盟cd结束时间 */
private long joinGuildCdEndTimeMs;


public Guild_JoinCdInfo() {
	guildFreeJoinCdNum = 0;
	joinGuildCdEndTimeMs = (long)0;
}

public Guild_JoinCdInfo(
	 int _guildFreeJoinCdNum
	, long _joinGuildCdEndTimeMs
) {	guildFreeJoinCdNum = _guildFreeJoinCdNum;
	joinGuildCdEndTimeMs = _joinGuildCdEndTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 联盟免cd加入次数 */
public int getGuildFreeJoinCdNum() { return guildFreeJoinCdNum; }
/** 联盟免cd加入次数 */
public void setGuildFreeJoinCdNum(int _guildFreeJoinCdNum) { guildFreeJoinCdNum = _guildFreeJoinCdNum; }
/** 加入联盟cd结束时间 */
public long getJoinGuildCdEndTimeMs() { return joinGuildCdEndTimeMs; }
/** 加入联盟cd结束时间 */
public void setJoinGuildCdEndTimeMs(long _joinGuildCdEndTimeMs) { joinGuildCdEndTimeMs = _joinGuildCdEndTimeMs; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildFreeJoinCdNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) joinGuildCdEndTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(guildFreeJoinCdNum);
	_buf.putLong(joinGuildCdEndTimeMs);
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

