package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_014_RetMailStatInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 邮件统计信息 */
private Common.MailObj.Mail_StatInfo mailStatInfo;


public GS2GC_002_014_RetMailStatInfo() {
	mailStatInfo = new Common.MailObj.Mail_StatInfo();
}

public GS2GC_002_014_RetMailStatInfo(
	 Common.MailObj.Mail_StatInfo _mailStatInfo
) {	mailStatInfo = _mailStatInfo;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)14; }

/** 邮件统计信息 */
public Common.MailObj.Mail_StatInfo getMailStatInfo() { return mailStatInfo; }
/** 邮件统计信息 */
public void setMailStatInfo(Common.MailObj.Mail_StatInfo _mailStatInfo) { mailStatInfo = _mailStatInfo; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _mailStatInfoCustLen = _buf.getInt();
	int _mailStatInfoCurPos = _buf.position();
	mailStatInfo.ReadUnzipBuf(_buf, _mailStatInfoCurPos + _mailStatInfoCustLen);
	_buf.position(_mailStatInfoCurPos + _mailStatInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(mailStatInfo.GetBufSize());
	mailStatInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)14);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)14);
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

