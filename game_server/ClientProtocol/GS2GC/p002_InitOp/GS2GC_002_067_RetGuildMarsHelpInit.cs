using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_067_RetGuildMarsHelpInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 可以帮助的火星求助实例ID列表
/// </summary>
private List<long> canDealIdList;


public GS2GC_002_067_RetGuildMarsHelpInit() {
	canDealIdList = new List<long>();
}

public GS2GC_002_067_RetGuildMarsHelpInit(
	List<long> _canDealIdList
) {	canDealIdList = _canDealIdList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)67; }

/// <summary>
/// 可以帮助的火星求助实例ID列表
/// </summary>
public List<long> getCanDealIdList() { return canDealIdList; }
/// <summary>
/// 可以帮助的火星求助实例ID列表
/// </summary>
public void addCanDealIdList(long _canDealIdList) { canDealIdList.Add(_canDealIdList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (canDealIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (canDealIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _canDealIdListCount = _buf.getShort();
	for(int _i = 0; _i < _canDealIdListCount; _i++) { 
		long _canDealIdList = (long)0;
		_canDealIdList = _buf.getLong();
		canDealIdList.Add(_canDealIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)canDealIdList.Count);
	for(int _i = 0; _i < canDealIdList.Count; _i++) { 
		_buf.putLong(canDealIdList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)67);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)67);
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
	builder.Append("canDealIdList").Append(":").Append(canDealIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

