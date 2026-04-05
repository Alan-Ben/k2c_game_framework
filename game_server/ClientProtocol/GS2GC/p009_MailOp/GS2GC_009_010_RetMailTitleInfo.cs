using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p009_MailOp
{

public class GS2GC_009_010_RetMailTitleInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 邮件标题列表
/// </summary>
private List<Common.MailObj.Mail_TitleInfo> titleList;


public GS2GC_009_010_RetMailTitleInfo() {
	titleList = new List<Common.MailObj.Mail_TitleInfo>();
}

public GS2GC_009_010_RetMailTitleInfo(
	List<Common.MailObj.Mail_TitleInfo> _titleList
) {	titleList = _titleList;
}

public byte getMainOrder() { return (byte)9; }

public byte getSubOrder() { return (byte)10; }

/// <summary>
/// 邮件标题列表
/// </summary>
public List<Common.MailObj.Mail_TitleInfo> getTitleList() { return titleList; }
/// <summary>
/// 邮件标题列表
/// </summary>
public void addTitleList(Common.MailObj.Mail_TitleInfo _titleList) { titleList.Add(_titleList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < titleList.Count; _i++) {
	_size += 4 + titleList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < titleList.Count; _i++) {
	_size += 4 + titleList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _titleListCount = _buf.getShort();
	for(int _i = 0; _i < _titleListCount; _i++) { 
		Common.MailObj.Mail_TitleInfo _titleList = new Common.MailObj.Mail_TitleInfo();
		int __titleListCustLen = _buf.getInt();
	int __titleListCurPos = _buf.getCurPos();
	_titleList.ReadUnzipBuf(_buf, __titleListCurPos + __titleListCustLen);
	_buf.setPosition(__titleListCurPos + __titleListCustLen);

		titleList.Add(_titleList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)titleList.Count);
	for(int _i = 0; _i < titleList.Count; _i++) { 
		_buf.putInt(titleList[_i].GetBufSize());
	titleList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)9);
	_buf.put((byte)10);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)9);
	_recBuf.put((byte)10);
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
	builder.Append("titleList").Append(":").Append(titleList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

