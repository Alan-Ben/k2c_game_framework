using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p009_MailOp
{

public class GS2GC_009_054_OnMailReaded : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 邮件唯一id列表
/// </summary>
private List<long> mailUidList;


public GS2GC_009_054_OnMailReaded() {
	mailUidList = new List<long>();
}

public GS2GC_009_054_OnMailReaded(
	List<long> _mailUidList
) {	mailUidList = _mailUidList;
}

public byte getMainOrder() { return (byte)9; }

public byte getSubOrder() { return (byte)54; }

/// <summary>
/// 邮件唯一id列表
/// </summary>
public List<long> getMailUidList() { return mailUidList; }
/// <summary>
/// 邮件唯一id列表
/// </summary>
public void addMailUidList(long _mailUidList) { mailUidList.Add(_mailUidList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (mailUidList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (mailUidList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _mailUidListCount = _buf.getShort();
	for(int _i = 0; _i < _mailUidListCount; _i++) { 
		long _mailUidList = (long)0;
		_mailUidList = _buf.getLong();
		mailUidList.Add(_mailUidList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)mailUidList.Count);
	for(int _i = 0; _i < mailUidList.Count; _i++) { 
		_buf.putLong(mailUidList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)9);
	_buf.put((byte)54);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)9);
	_recBuf.put((byte)54);
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
	builder.Append("mailUidList").Append(":").Append(mailUidList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

