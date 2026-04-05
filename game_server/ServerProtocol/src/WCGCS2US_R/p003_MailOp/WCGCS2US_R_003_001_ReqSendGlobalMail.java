package WCGCS2US_R.p003_MailOp;

import java.nio.ByteBuffer;
public class WCGCS2US_R_003_001_ReqSendGlobalMail implements ALBasicProtocolPack._IALProtocolStructure {
private Common.WCGCS2US_GlobalMail gmail;


public WCGCS2US_R_003_001_ReqSendGlobalMail() {
	gmail = new Common.WCGCS2US_GlobalMail();
}

public WCGCS2US_R_003_001_ReqSendGlobalMail(
	 Common.WCGCS2US_GlobalMail _gmail
) {	gmail = _gmail;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)1; }

public Common.WCGCS2US_GlobalMail getGmail() { return gmail; }
public void setGmail(Common.WCGCS2US_GlobalMail _gmail) { gmail = _gmail; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + gmail.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + gmail.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _gmailCustLen = _buf.getInt();
	int _gmailCurPos = _buf.position();
	gmail.ReadUnzipBuf(_buf, _gmailCurPos + _gmailCustLen);
	_buf.position(_gmailCurPos + _gmailCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(gmail.GetBufSize());
	gmail.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
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

