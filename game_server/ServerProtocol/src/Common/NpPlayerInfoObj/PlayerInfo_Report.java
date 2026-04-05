package Common.NpPlayerInfoObj;

import java.nio.ByteBuffer;
/*********
 * 玩家举报数据
 **/
public class PlayerInfo_Report implements ALBasicProtocolPack._IALProtocolStructure {
/** 目标玩家CID */
private long targetCid;
/** 举报内容 */
private String contenxt;


public PlayerInfo_Report() {
	targetCid = (long)0;
	contenxt = "";
}

public PlayerInfo_Report(
	 long _targetCid
	, String _contenxt
) {	targetCid = _targetCid;
	contenxt = _contenxt;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 目标玩家CID */
public long getTargetCid() { return targetCid; }
/** 目标玩家CID */
public void setTargetCid(long _targetCid) { targetCid = _targetCid; }
/** 举报内容 */
public String getContenxt() { return contenxt; }
/** 举报内容 */
public void setContenxt(String _contenxt) { contenxt = _contenxt; }


public final int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(contenxt);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(contenxt);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) targetCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) contenxt = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(targetCid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, contenxt);
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

