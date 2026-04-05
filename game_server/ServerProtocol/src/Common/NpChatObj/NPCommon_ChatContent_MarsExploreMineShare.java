package Common.NpChatObj;

import java.nio.ByteBuffer;
/*********
 * 火星探险矿分享
 **/
public class NPCommon_ChatContent_MarsExploreMineShare implements ALBasicProtocolPack._IALProtocolStructure {
/** 联盟分享矿消息id */
private long guildShareMineMsgId;
/** 矿配表id */
private long refId;


public NPCommon_ChatContent_MarsExploreMineShare() {
	guildShareMineMsgId = (long)0;
	refId = (long)0;
}

public NPCommon_ChatContent_MarsExploreMineShare(
	 long _guildShareMineMsgId
	, long _refId
) {	guildShareMineMsgId = _guildShareMineMsgId;
	refId = _refId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 联盟分享矿消息id */
public long getGuildShareMineMsgId() { return guildShareMineMsgId; }
/** 联盟分享矿消息id */
public void setGuildShareMineMsgId(long _guildShareMineMsgId) { guildShareMineMsgId = _guildShareMineMsgId; }
/** 矿配表id */
public long getRefId() { return refId; }
/** 矿配表id */
public void setRefId(long _refId) { refId = _refId; }


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
	if(_buf.remaining() > 0) guildShareMineMsgId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) refId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(guildShareMineMsgId);
	_buf.putLong(refId);
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

