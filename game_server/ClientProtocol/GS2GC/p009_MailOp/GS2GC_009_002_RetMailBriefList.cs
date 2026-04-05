using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p009_MailOp
{

public class GS2GC_009_002_RetMailBriefList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 全部邮件排序列表
/// </summary>
private List<Common.MailObj.Mail_BriefInfo> briefList;


public GS2GC_009_002_RetMailBriefList() {
	briefList = new List<Common.MailObj.Mail_BriefInfo>();
}

public GS2GC_009_002_RetMailBriefList(
	List<Common.MailObj.Mail_BriefInfo> _briefList
) {	briefList = _briefList;
}

public byte getMainOrder() { return (byte)9; }

public byte getSubOrder() { return (byte)2; }

/// <summary>
/// 全部邮件排序列表
/// </summary>
public List<Common.MailObj.Mail_BriefInfo> getBriefList() { return briefList; }
/// <summary>
/// 全部邮件排序列表
/// </summary>
public void addBriefList(Common.MailObj.Mail_BriefInfo _briefList) { briefList.Add(_briefList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < briefList.Count; _i++) {
	_size += 4 + briefList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < briefList.Count; _i++) {
	_size += 4 + briefList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _briefListCount = _buf.getShort();
	for(int _i = 0; _i < _briefListCount; _i++) { 
		Common.MailObj.Mail_BriefInfo _briefList = new Common.MailObj.Mail_BriefInfo();
		int __briefListCustLen = _buf.getInt();
	int __briefListCurPos = _buf.getCurPos();
	_briefList.ReadUnzipBuf(_buf, __briefListCurPos + __briefListCustLen);
	_buf.setPosition(__briefListCurPos + __briefListCustLen);

		briefList.Add(_briefList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)briefList.Count);
	for(int _i = 0; _i < briefList.Count; _i++) { 
		_buf.putInt(briefList[_i].GetBufSize());
	briefList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)9);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)9);
	_recBuf.put((byte)2);
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
	builder.Append("briefList").Append(":").Append(briefList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

