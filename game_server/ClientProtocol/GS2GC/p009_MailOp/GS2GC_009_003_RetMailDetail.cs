using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p009_MailOp
{

public class GS2GC_009_003_RetMailDetail : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 邮件详情
/// </summary>
private Common.MailObj.Mail_DetailInfo mailDetail;


public GS2GC_009_003_RetMailDetail() {
	mailDetail = new Common.MailObj.Mail_DetailInfo();
}

public GS2GC_009_003_RetMailDetail(
	Common.MailObj.Mail_DetailInfo _mailDetail
) {	mailDetail = _mailDetail;
}

public byte getMainOrder() { return (byte)9; }

public byte getSubOrder() { return (byte)3; }

/// <summary>
/// 邮件详情
/// </summary>
public Common.MailObj.Mail_DetailInfo getMailDetail() { return mailDetail; }
/// <summary>
/// 邮件详情
/// </summary>
public void setMailDetail(Common.MailObj.Mail_DetailInfo _mailDetail) { mailDetail = _mailDetail; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + mailDetail.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + mailDetail.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _mailDetailCustLen = _buf.getInt();
	int _mailDetailCurPos = _buf.getCurPos();
	mailDetail.ReadUnzipBuf(_buf, _mailDetailCurPos + _mailDetailCustLen);
	_buf.setPosition(_mailDetailCurPos + _mailDetailCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(mailDetail.GetBufSize());
	mailDetail.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)9);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)9);
	_recBuf.put((byte)3);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("mailDetail").Append(":").Append(mailDetail == null ? "null" : mailDetail.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

