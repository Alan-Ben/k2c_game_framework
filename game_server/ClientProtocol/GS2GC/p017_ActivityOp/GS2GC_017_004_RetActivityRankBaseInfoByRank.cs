using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p017_ActivityOp
{

public class GS2GC_017_004_RetActivityRankBaseInfoByRank : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 基础数据
/// </summary>
private Common.RankObj.Rank_BaseItem baseItem;


public GS2GC_017_004_RetActivityRankBaseInfoByRank() {
	baseItem = new Common.RankObj.Rank_BaseItem();
}

public GS2GC_017_004_RetActivityRankBaseInfoByRank(
	Common.RankObj.Rank_BaseItem _baseItem
) {	baseItem = _baseItem;
}

public byte getMainOrder() { return (byte)17; }

public byte getSubOrder() { return (byte)4; }

/// <summary>
/// 基础数据
/// </summary>
public Common.RankObj.Rank_BaseItem getBaseItem() { return baseItem; }
/// <summary>
/// 基础数据
/// </summary>
public void setBaseItem(Common.RankObj.Rank_BaseItem _baseItem) { baseItem = _baseItem; }


public int GetBufSize() {
	int _size = 32;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _baseItemCustLen = _buf.getInt();
	int _baseItemCurPos = _buf.getCurPos();
	baseItem.ReadUnzipBuf(_buf, _baseItemCurPos + _baseItemCustLen);
	_buf.setPosition(_baseItemCurPos + _baseItemCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(baseItem.GetBufSize());
	baseItem.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)4);
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
	builder.Append("baseItem").Append(":").Append(baseItem == null ? "null" : baseItem.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

