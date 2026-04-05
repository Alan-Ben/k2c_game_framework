package GC2GS.p041_MarsExploreOp;

import java.nio.ByteBuffer;
/*********
 * 火星探险-获取联盟分享矿是否被其他人攻击标识
 **/
public class GC2GS_041_024_ReqGuildShareMineHadAttackByOthersTag implements ALBasicProtocolPack._IALProtocolStructure {
/** 联盟分享矿信息消息ID */
private long guildShareMineMsgId;


public GC2GS_041_024_ReqGuildShareMineHadAttackByOthersTag() {
	guildShareMineMsgId = (long)0;
}

public GC2GS_041_024_ReqGuildShareMineHadAttackByOthersTag(
	 long _guildShareMineMsgId
) {	guildShareMineMsgId = _guildShareMineMsgId;
}

public final byte getMainOrder() { return (byte)41; }

public final byte getSubOrder() { return (byte)24; }

/** 联盟分享矿信息消息ID */
public long getGuildShareMineMsgId() { return guildShareMineMsgId; }
/** 联盟分享矿信息消息ID */
public void setGuildShareMineMsgId(long _guildShareMineMsgId) { guildShareMineMsgId = _guildShareMineMsgId; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildShareMineMsgId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(guildShareMineMsgId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)24);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)24);
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

