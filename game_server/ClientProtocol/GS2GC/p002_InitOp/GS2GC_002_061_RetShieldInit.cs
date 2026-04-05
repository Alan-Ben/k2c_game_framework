using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

/// <summary>
/// 屏蔽数据组件初始化
/// </summary>
public class GS2GC_002_061_RetShieldInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 已屏蔽玩家CID列表
/// </summary>
private List<long> shieldCidList;


public GS2GC_002_061_RetShieldInit() {
	shieldCidList = new List<long>();
}

public GS2GC_002_061_RetShieldInit(
	List<long> _shieldCidList
) {	shieldCidList = _shieldCidList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)61; }

/// <summary>
/// 已屏蔽玩家CID列表
/// </summary>
public List<long> getShieldCidList() { return shieldCidList; }
/// <summary>
/// 已屏蔽玩家CID列表
/// </summary>
public void addShieldCidList(long _shieldCidList) { shieldCidList.Add(_shieldCidList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (shieldCidList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (shieldCidList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _shieldCidListCount = _buf.getShort();
	for(int _i = 0; _i < _shieldCidListCount; _i++) { 
		long _shieldCidList = (long)0;
		_shieldCidList = _buf.getLong();
		shieldCidList.Add(_shieldCidList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)shieldCidList.Count);
	for(int _i = 0; _i < shieldCidList.Count; _i++) { 
		_buf.putLong(shieldCidList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)61);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)61);
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
	builder.Append("shieldCidList").Append(":").Append(shieldCidList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

