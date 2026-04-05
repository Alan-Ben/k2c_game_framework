package WCGCS2US_R.p003_MailOp;

import java.nio.ByteBuffer;
public class WCGCS2US_R_003_002_ReqSendPersonalMail implements ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private Common.WCGCS2US_PersonalMail pmail;


public WCGCS2US_R_003_002_ReqSendPersonalMail() {
	uid = (long)0;
	pmail = new Common.WCGCS2US_PersonalMail();
}

public WCGCS2US_R_003_002_ReqSendPersonalMail(
	 long _uid
	, Common.WCGCS2US_PersonalMail _pmail
) {	uid = _uid;
	pmail = _pmail;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)2; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public Common.WCGCS2US_PersonalMail getPmail() { return pmail; }
public void setPmail(Common.WCGCS2US_PersonalMail _pmail) { pmail = _pmail; }


public final int GetBufSize() {
	int _size = 8;
	_size += 4 + pmail.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + pmail.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _pmailCustLen = _buf.getInt();
	int _pmailCurPos = _buf.position();
	pmail.ReadUnzipBuf(_buf, _pmailCurPos + _pmailCustLen);
	_buf.position(_pmailCurPos + _pmailCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(uid);
	_buf.putInt(pmail.GetBufSize());
	pmail.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
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

