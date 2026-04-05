package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
public class GS2GC_004_033_RetTodayLikeCidInfo implements ALBasicProtocolPack._IALProtocolStructure {
private Common.Common_TodayLikeCidInfo todayLikeCidInfo;


public GS2GC_004_033_RetTodayLikeCidInfo() {
	todayLikeCidInfo = new Common.Common_TodayLikeCidInfo();
}

public GS2GC_004_033_RetTodayLikeCidInfo(
	 Common.Common_TodayLikeCidInfo _todayLikeCidInfo
) {	todayLikeCidInfo = _todayLikeCidInfo;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)33; }

public Common.Common_TodayLikeCidInfo getTodayLikeCidInfo() { return todayLikeCidInfo; }
public void setTodayLikeCidInfo(Common.Common_TodayLikeCidInfo _todayLikeCidInfo) { todayLikeCidInfo = _todayLikeCidInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + todayLikeCidInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + todayLikeCidInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _todayLikeCidInfoCustLen = _buf.getInt();
	int _todayLikeCidInfoCurPos = _buf.position();
	todayLikeCidInfo.ReadUnzipBuf(_buf, _todayLikeCidInfoCurPos + _todayLikeCidInfoCustLen);
	_buf.position(_todayLikeCidInfoCurPos + _todayLikeCidInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(todayLikeCidInfo.GetBufSize());
	todayLikeCidInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)33);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)33);
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

