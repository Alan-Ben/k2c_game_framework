using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_047_RetGachaInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 卡池列表
/// </summary>
private List<Common.GachaObj.Gacha_PoolInfo> poolList;


public GS2GC_002_047_RetGachaInit() {
	poolList = new List<Common.GachaObj.Gacha_PoolInfo>();
}

public GS2GC_002_047_RetGachaInit(
	List<Common.GachaObj.Gacha_PoolInfo> _poolList
) {	poolList = _poolList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)47; }

/// <summary>
/// 卡池列表
/// </summary>
public List<Common.GachaObj.Gacha_PoolInfo> getPoolList() { return poolList; }
/// <summary>
/// 卡池列表
/// </summary>
public void addPoolList(Common.GachaObj.Gacha_PoolInfo _poolList) { poolList.Add(_poolList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < poolList.Count; _i++) {
	_size += 4 + poolList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < poolList.Count; _i++) {
	_size += 4 + poolList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _poolListCount = _buf.getShort();
	for(int _i = 0; _i < _poolListCount; _i++) { 
		Common.GachaObj.Gacha_PoolInfo _poolList = new Common.GachaObj.Gacha_PoolInfo();
		int __poolListCustLen = _buf.getInt();
	int __poolListCurPos = _buf.getCurPos();
	_poolList.ReadUnzipBuf(_buf, __poolListCurPos + __poolListCustLen);
	_buf.setPosition(__poolListCurPos + __poolListCustLen);

		poolList.Add(_poolList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)poolList.Count);
	for(int _i = 0; _i < poolList.Count; _i++) { 
		_buf.putInt(poolList[_i].GetBufSize());
	poolList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)47);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)47);
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
	builder.Append("poolList").Append(":").Append(poolList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

