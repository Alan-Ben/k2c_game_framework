package ALLRPC.US.Child;

import java.nio.ByteBuffer;
public class GetServerPoolAdult_Return implements ALBasicProtocolPack._IALProtocolStructure {
/** 匹配池子嗣数据 */
private Common.ChildObj.Adult_PoolInfo poolAdult;


public GetServerPoolAdult_Return() {
	poolAdult = new Common.ChildObj.Adult_PoolInfo();
}

public GetServerPoolAdult_Return(
	 Common.ChildObj.Adult_PoolInfo _poolAdult
) {	poolAdult = _poolAdult;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

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

