using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p009_MailOp
{

public class GS2GC_009_051_OnMailAdded : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 邮件标题
/// </summary>
private Common.MailObj.Mail_TitleInfo titleInfo;
/// <summary>
/// 简要信息
/// </summary>
private Common.MailObj.Mail_BriefInfo briefInfo;


public GS2GC_009_051_OnMailAdded() {
	titleInfo = new Common.MailObj.Mail_TitleInfo();
	briefInfo = new Common.MailObj.Mail_BriefInfo();
}

public GS2GC_009_051_OnMailAdded(
	Common.MailObj.Mail_TitleInfo _titleInfo
	, Common.MailObj.Mail_BriefInfo _briefInfo
) {	titleInfo = _titleInfo;
	briefInfo = _briefInfo;
}

public byte getMainOrder() { return (byte)9; }

public byte getSubOrder() { return (byte)51; }

/// <summary>
/// 邮件标题
/// </summary>
public Common.MailObj.Mail_TitleInfo getTitleInfo() { return titleInfo; }
/// <summary>
/// 邮件标题
/// </summary>
public void setTitleInfo(Common.MailObj.Mail_TitleInfo _titleInfo) { titleInfo = _titleInfo; }
/// <summary>
/// 简要信息
/// </summary>
public Common.MailObj.Mail_BriefInfo getBriefInfo() { return briefInfo; }
/// <summary>
/// 简要信息
/// </summary>
public void setBriefInfo(Common.MailObj.Mail_BriefInfo _briefInfo) { briefInfo = _briefInfo; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + titleInfo.GetBufSize();
	_size += 4 + briefInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + titleInfo.GetBufSize();
	_size += 4 + briefInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _titleInfoCustLen = _buf.getInt();
	int _titleInfoCurPos = _buf.getCurPos();
	titleInfo.ReadUnzipBuf(_buf, _titleInfoCurPos + _titleInfoCustLen);
	_buf.setPosition(_titleInfoCurPos + _titleInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _briefInfoCustLen = _buf.getInt();
	int _briefInfoCurPos = _buf.getCurPos();
	briefInfo.ReadUnzipBuf(_buf, _briefInfoCurPos + _briefInfoCustLen);
	_buf.setPosition(_briefInfoCurPos + _briefInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(titleInfo.GetBufSize());
	titleInfo.PutUnzipBuf(_buf);
	_buf.putInt(briefInfo.GetBufSize());
	briefInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)9);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)9);
	_recBuf.put((byte)51);
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
	builder.Append("titleInfo").Append(":").Append(titleInfo == null ? "null" : titleInfo.ToString()).Append(", ");
	builder.Append("briefInfo").Append(":").Append(briefInfo == null ? "null" : briefInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

