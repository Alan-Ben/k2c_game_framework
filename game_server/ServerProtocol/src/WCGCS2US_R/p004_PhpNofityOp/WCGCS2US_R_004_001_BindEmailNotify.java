package WCGCS2US_R.p004_PhpNofityOp;

import java.nio.ByteBuffer;
public class WCGCS2US_R_004_001_BindEmailNotify implements ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private String email;


public WCGCS2US_R_004_001_BindEmailNotify() {
	uid = (long)0;
	email = "";
}

public WCGCS2US_R_004_001_BindEmailNotify(
	 long _uid
	, String _email
) {	uid = _uid;
	email = _email;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)1; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public String getEmail() { return email; }
public void setEmail(String _email) { email = _email; }


public final int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(email);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(email);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) email = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(uid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, email);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)1);
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

