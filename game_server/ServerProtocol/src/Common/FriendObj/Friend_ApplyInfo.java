package Common.FriendObj;

import java.nio.ByteBuffer;
/*********
 * 申请数据
 **/
public class Friend_ApplyInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 申请玩家CID */
private long applyCid;
/** 申请时间 */
private int applyTimeS;


public Friend_ApplyInfo() {
	applyCid = (long)0;
	applyTimeS = 0;
}

public Friend_ApplyInfo(
	 long _applyCid
	, int _applyTimeS
) {	applyCid = _applyCid;
	applyTimeS = _applyTimeS;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 申请玩家CID */
public long getApplyCid() { return applyCid; }
/** 申请玩家CID */
public void setApplyCid(long _applyCid) { applyCid = _applyCid; }
/** 申请时间 */
public int getApplyTimeS() { return applyTimeS; }
/** 申请时间 */
public void setApplyTimeS(int _applyTimeS) { applyTimeS = _applyTimeS; }


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
	if(_buf.remaining() > 0) applyCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) applyTimeS = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(applyCid);
	_buf.putInt(applyTimeS);
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

