package Common.ChildObj;

import java.nio.ByteBuffer;
/*********
 * 联姻池待匹配子嗣（成年未婚）数据
 **/
public class Adult_PoolInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家CID */
private long cid;
/** 子嗣数据 */
private Common.ChildObj.Adult_Info adult;


public Adult_PoolInfo() {
	cid = (long)0;
	adult = new Common.ChildObj.Adult_Info();
}

public Adult_PoolInfo(
	 long _cid
	, Common.ChildObj.Adult_Info _adult
) {	cid = _cid;
	adult = _adult;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 玩家CID */
public long getCid() { return cid; }
/** 玩家CID */
public void setCid(long _cid) { cid = _cid; }
/** 子嗣数据 */
public Common.ChildObj.Adult_Info getAdult() { return adult; }
/** 子嗣数据 */
public void setAdult(Common.ChildObj.Adult_Info _adult) { adult = _adult; }


public final int GetBufSize() {
	int _size = 8;
	_size += 4 + adult.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + adult.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _adultCustLen = _buf.getInt();
	int _adultCurPos = _buf.position();
	adult.ReadUnzipBuf(_buf, _adultCurPos + _adultCustLen);
	_buf.position(_adultCurPos + _adultCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putInt(adult.GetBufSize());
	adult.PutUnzipBuf(_buf);
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

