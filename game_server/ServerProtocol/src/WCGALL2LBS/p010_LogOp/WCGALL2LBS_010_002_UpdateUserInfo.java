package WCGALL2LBS.p010_LogOp;

import java.nio.ByteBuffer;
public class WCGALL2LBS_010_002_UpdateUserInfo implements ALBasicProtocolPack._IALProtocolStructure {
private Common.Common_UserLogInfo updateInfo;


public WCGALL2LBS_010_002_UpdateUserInfo() {
	updateInfo = new Common.Common_UserLogInfo();
}

public WCGALL2LBS_010_002_UpdateUserInfo(
	 Common.Common_UserLogInfo _updateInfo
) {	updateInfo = _updateInfo;
}

public final byte getMainOrder() { return (byte)10; }

public final byte getSubOrder() { return (byte)2; }

public Common.Common_UserLogInfo getUpdateInfo() { return updateInfo; }
public void setUpdateInfo(Common.Common_UserLogInfo _updateInfo) { updateInfo = _updateInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + updateInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + updateInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _updateInfoCustLen = _buf.getInt();
	int _updateInfoCurPos = _buf.position();
	updateInfo.ReadUnzipBuf(_buf, _updateInfoCurPos + _updateInfoCustLen);
	_buf.position(_updateInfoCurPos + _updateInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(updateInfo.GetBufSize());
	updateInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)10);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)10);
	_recBuf.put((byte)2);
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

