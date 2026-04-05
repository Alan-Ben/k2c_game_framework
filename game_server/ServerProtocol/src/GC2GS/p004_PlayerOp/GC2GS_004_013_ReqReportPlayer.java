package GC2GS.p004_PlayerOp;

import java.nio.ByteBuffer;
/*********
 * 举报指定玩家
 **/
public class GC2GS_004_013_ReqReportPlayer implements ALBasicProtocolPack._IALProtocolStructure {
/** 举报目标玩家CID */
private long targetCid;
/** 举报内容 */
private String content;


public GC2GS_004_013_ReqReportPlayer() {
	targetCid = (long)0;
	content = "";
}

public GC2GS_004_013_ReqReportPlayer(
	 long _targetCid
	, String _content
) {	targetCid = _targetCid;
	content = _content;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)13; }

/** 举报目标玩家CID */
public long getTargetCid() { return targetCid; }
/** 举报目标玩家CID */
public void setTargetCid(long _targetCid) { targetCid = _targetCid; }
/** 举报内容 */
public String getContent() { return content; }
/** 举报内容 */
public void setContent(String _content) { content = _content; }


public final int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) targetCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) content = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(targetCid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, content);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)13);
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

