using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_014_RetMailStatInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 邮件统计信息
/// </summary>
private Common.MailObj.Mail_StatInfo mailStatInfo;


public GS2GC_002_014_RetMailStatInfo() {
	mailStatInfo = new Common.MailObj.Mail_StatInfo();
}

public GS2GC_002_014_RetMailStatInfo(
	Common.MailObj.Mail_StatInfo _mailStatInfo
) {	mailStatInfo = _mailStatInfo;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)14; }

/// <summary>
/// 邮件统计信息
/// </summary>
public Common.MailObj.Mail_StatInfo getMailStatInfo() { return mailStatInfo; }
/// <summary>
/// 邮件统计信息
/// </summary>
public void setMailStatInfo(Common.MailObj.Mail_StatInfo _mailStatInfo) { mailStatInfo = _mailStatInfo; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _mailStatInfoCustLen = _buf.getInt();
	int _mailStatInfoCurPos = _buf.getCurPos();
	mailStatInfo.ReadUnzipBuf(_buf, _mailStatInfoCurPos + _mailStatInfoCustLen);
	_buf.setPosition(_mailStatInfoCurPos + _mailStatInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(mailStatInfo.GetBufSize());
	mailStatInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)14);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)14);
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
	builder.Append("mailStatInfo").Append(":").Append(mailStatInfo == null ? "null" : mailStatInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

