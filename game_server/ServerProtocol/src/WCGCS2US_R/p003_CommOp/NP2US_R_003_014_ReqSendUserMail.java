package WCGCS2US_R.p003_CommOp;

import java.nio.ByteBuffer;
/*********
 * 发送玩家邮件
 **/
public class NP2US_R_003_014_ReqSendUserMail implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;
private Common.NpServerObj.NpServerObj_PlatFormMail mailData;


public NP2US_R_003_014_ReqSendUserMail() {
	cid = (long)0;
	mailData = new Common.NpServerObj.NpServerObj_PlatFormMail();
}

public NP2US_R_003_014_ReqSendUserMail(
	 long _cid
	, Common.NpServerObj.NpServerObj_PlatFormMail _mailData
) {	cid = _cid;
	mailData = _mailData;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)14; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public Common.NpServerObj.NpServerObj_PlatFormMail getMailData() { return mailData; }
public void setMailData(Common.NpServerObj.NpServerObj_PlatFormMail _mailData) { mailData = _mailData; }


public final int GetBufSize() {
	int _size = 8;
	_size += 4 + mailData.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + mailData.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _mailDataCustLen = _buf.getInt();
	int _mailDataCurPos = _buf.position();
	mailData.ReadUnzipBuf(_buf, _mailDataCurPos + _mailDataCustLen);
	_buf.position(_mailDataCurPos + _mailDataCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putInt(mailData.GetBufSize());
	mailData.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)14);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
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

