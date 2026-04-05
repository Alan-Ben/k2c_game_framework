using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p009_MailOp
{

public class GS2GC_009_008_RetAKeyDelAll : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 已删除邮件列表
/// </summary>
private List<long> delIdList;


public GS2GC_009_008_RetAKeyDelAll() {
	delIdList = new List<long>();
}

public GS2GC_009_008_RetAKeyDelAll(
	List<long> _delIdList
) {	delIdList = _delIdList;
}

public byte getMainOrder() { return (byte)9; }

public byte getSubOrder() { return (byte)8; }

/// <summary>
/// 已删除邮件列表
/// </summary>
public List<long> getDelIdList() { return delIdList; }
/// <summary>
/// 已删除邮件列表
/// </summary>
public void addDelIdList(long _delIdList) { delIdList.Add(_delIdList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (delIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (delIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _delIdListCount = _buf.getShort();
	for(int _i = 0; _i < _delIdListCount; _i++) { 
		long _delIdList = (long)0;
		_delIdList = _buf.getLong();
		delIdList.Add(_delIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)delIdList.Count);
	for(int _i = 0; _i < delIdList.Count; _i++) { 
		_buf.putLong(delIdList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)9);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)9);
	_recBuf.put((byte)8);
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
	builder.Append("delIdList").Append(":").Append(delIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

