using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p015_ConsortOp
{

/// <summary>
/// 随机邀约中指定的妃子ID列表变更
/// </summary>
public class GS2GC_015_062_OnRandCallConsortChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 空
/// </summary>
private List<long> consortIdList;


public GS2GC_015_062_OnRandCallConsortChg() {
	consortIdList = new List<long>();
}

public GS2GC_015_062_OnRandCallConsortChg(
	List<long> _consortIdList
) {	consortIdList = _consortIdList;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)62; }

/// <summary>
/// 空
/// </summary>
public List<long> getConsortIdList() { return consortIdList; }
/// <summary>
/// 空
/// </summary>
public void addConsortIdList(long _consortIdList) { consortIdList.Add(_consortIdList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (consortIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (consortIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _consortIdListCount = _buf.getShort();
	for(int _i = 0; _i < _consortIdListCount; _i++) { 
		long _consortIdList = (long)0;
		_consortIdList = _buf.getLong();
		consortIdList.Add(_consortIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)consortIdList.Count);
	for(int _i = 0; _i < consortIdList.Count; _i++) { 
		_buf.putLong(consortIdList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)62);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)62);
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
	builder.Append("consortIdList").Append(":").Append(consortIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

