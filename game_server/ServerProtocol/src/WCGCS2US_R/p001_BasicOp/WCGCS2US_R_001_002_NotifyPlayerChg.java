package WCGCS2US_R.p001_BasicOp;

import java.nio.ByteBuffer;
public class WCGCS2US_R_001_002_NotifyPlayerChg implements ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private WCGCS2US_R.p001_BasicOp.WCGCS2US_R_001_002_Inter_Param param;


public WCGCS2US_R_001_002_NotifyPlayerChg() {
	uid = (long)0;
	param = new WCGCS2US_R.p001_BasicOp.WCGCS2US_R_001_002_Inter_Param();
}

public WCGCS2US_R_001_002_NotifyPlayerChg(
	 long _uid
	, WCGCS2US_R.p001_BasicOp.WCGCS2US_R_001_002_Inter_Param _param
) {	uid = _uid;
	param = _param;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)2; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public WCGCS2US_R.p001_BasicOp.WCGCS2US_R_001_002_Inter_Param getParam() { return param; }
public void setParam(WCGCS2US_R.p001_BasicOp.WCGCS2US_R_001_002_Inter_Param _param) { param = _param; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _paramCustLen = _buf.getInt();
	int _paramCurPos = _buf.position();
	param.ReadUnzipBuf(_buf, _paramCurPos + _paramCustLen);
	_buf.position(_paramCurPos + _paramCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(uid);
	_buf.putInt(param.GetBufSize());
	param.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
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

