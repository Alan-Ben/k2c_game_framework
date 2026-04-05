package GS2GC.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 获取联姻池待匹配子嗣数据
 **/
public class GS2GC_014_018_RetGetPoolAdult implements ALBasicProtocolPack._IALProtocolStructure {
/** 匹配池子嗣数据 */
private Common.ChildObj.Adult_PoolInfo poolAdult;


public GS2GC_014_018_RetGetPoolAdult() {
	poolAdult = new Common.ChildObj.Adult_PoolInfo();
}

public GS2GC_014_018_RetGetPoolAdult(
	 Common.ChildObj.Adult_PoolInfo _poolAdult
) {	poolAdult = _poolAdult;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)18; }

/** 匹配池子嗣数据 */
public Common.ChildObj.Adult_PoolInfo getPoolAdult() { return poolAdult; }
/** 匹配池子嗣数据 */
public void setPoolAdult(Common.ChildObj.Adult_PoolInfo _poolAdult) { poolAdult = _poolAdult; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + poolAdult.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + poolAdult.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _poolAdultCustLen = _buf.getInt();
	int _poolAdultCurPos = _buf.position();
	poolAdult.ReadUnzipBuf(_buf, _poolAdultCurPos + _poolAdultCustLen);
	_buf.position(_poolAdultCurPos + _poolAdultCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(poolAdult.GetBufSize());
	poolAdult.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)18);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)18);
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

