using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace RemarkData
{

public class FuncUnlockData : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 功能解锁id列表
/// </summary>
private List<long> funcUnlockIdList;


public FuncUnlockData() {
	funcUnlockIdList = new List<long>();
}

public FuncUnlockData(
	List<long> _funcUnlockIdList
) {	funcUnlockIdList = _funcUnlockIdList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 功能解锁id列表
/// </summary>
public List<long> getFuncUnlockIdList() { return funcUnlockIdList; }
/// <summary>
/// 功能解锁id列表
/// </summary>
public void addFuncUnlockIdList(long _funcUnlockIdList) { funcUnlockIdList.Add(_funcUnlockIdList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (funcUnlockIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (funcUnlockIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _funcUnlockIdListCount = _buf.getShort();
	for(int _i = 0; _i < _funcUnlockIdListCount; _i++) { 
		long _funcUnlockIdList = (long)0;
		_funcUnlockIdList = _buf.getLong();
		funcUnlockIdList.Add(_funcUnlockIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)funcUnlockIdList.Count);
	for(int _i = 0; _i < funcUnlockIdList.Count; _i++) { 
		_buf.putLong(funcUnlockIdList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("funcUnlockIdList").Append(":").Append(funcUnlockIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

