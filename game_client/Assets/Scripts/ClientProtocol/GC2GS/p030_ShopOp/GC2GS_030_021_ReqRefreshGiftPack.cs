using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p030_ShopOp
{

/// <summary>
/// 刷新礼包
/// </summary>
public class GC2GS_030_021_ReqRefreshGiftPack : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 礼包id列表
/// </summary>
private List<long> giftPackList;


public GC2GS_030_021_ReqRefreshGiftPack() {
	giftPackList = new List<long>();
}

public GC2GS_030_021_ReqRefreshGiftPack(
	List<long> _giftPackList
) {	giftPackList = _giftPackList;
}

public byte getMainOrder() { return (byte)30; }

public byte getSubOrder() { return (byte)21; }

/// <summary>
/// 礼包id列表
/// </summary>
public List<long> getGiftPackList() { return giftPackList; }
/// <summary>
/// 礼包id列表
/// </summary>
public void addGiftPackList(long _giftPackList) { giftPackList.Add(_giftPackList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (giftPackList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (giftPackList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _giftPackListCount = _buf.getShort();
	for(int _i = 0; _i < _giftPackListCount; _i++) { 
		long _giftPackList = (long)0;
		_giftPackList = _buf.getLong();
		giftPackList.Add(_giftPackList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)giftPackList.Count);
	for(int _i = 0; _i < giftPackList.Count; _i++) { 
		_buf.putLong(giftPackList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)30);
	_buf.put((byte)21);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)30);
	_recBuf.put((byte)21);
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
	builder.Append("giftPackList").Append(":").Append(giftPackList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

