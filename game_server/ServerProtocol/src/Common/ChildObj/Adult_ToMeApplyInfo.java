package Common.ChildObj;

import java.nio.ByteBuffer;
/*********
 * 向玩家请求指定联姻请求数据
 **/
public class Adult_ToMeApplyInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 请求玩家CID */
private long applyCid;
/** 请求子嗣的数据 */
private Common.ChildObj.Adult_Info applyAdult;


public Adult_ToMeApplyInfo() {
	applyCid = (long)0;
	applyAdult = new Common.ChildObj.Adult_Info();
}

public Adult_ToMeApplyInfo(
	 long _applyCid
	, Common.ChildObj.Adult_Info _applyAdult
) {	applyCid = _applyCid;
	applyAdult = _applyAdult;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 请求玩家CID */
public long getApplyCid() { return applyCid; }
/** 请求玩家CID */
public void setApplyCid(long _applyCid) { applyCid = _applyCid; }
/** 请求子嗣的数据 */
public Common.ChildObj.Adult_Info getApplyAdult() { return applyAdult; }
/** 请求子嗣的数据 */
public void setApplyAdult(Common.ChildObj.Adult_Info _applyAdult) { applyAdult = _applyAdult; }


public final int GetBufSize() {
	int _size = 8;
	_size += 4 + applyAdult.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + applyAdult.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) applyCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _applyAdultCustLen = _buf.getInt();
	int _applyAdultCurPos = _buf.position();
	applyAdult.ReadUnzipBuf(_buf, _applyAdultCurPos + _applyAdultCustLen);
	_buf.position(_applyAdultCurPos + _applyAdultCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(applyCid);
	_buf.putInt(applyAdult.GetBufSize());
	applyAdult.PutUnzipBuf(_buf);
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

