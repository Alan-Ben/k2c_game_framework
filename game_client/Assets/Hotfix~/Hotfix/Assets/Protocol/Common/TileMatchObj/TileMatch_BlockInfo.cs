using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.Common.TileMatchObj
{

/// <summary>
/// 三消-方块信息
/// </summary>
public class TileMatch_BlockInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 方块索引
/// </summary>
private int index;
/// <summary>
/// 方块类型
/// </summary>
private Hotfix.Common.TileMatchObj.TileMatch_BlockBaseInfo baseInfo;


public TileMatch_BlockInfo() {
	index = 0;
	baseInfo = new Hotfix.Common.TileMatchObj.TileMatch_BlockBaseInfo();
}

public TileMatch_BlockInfo(
	int _index
	, Hotfix.Common.TileMatchObj.TileMatch_BlockBaseInfo _baseInfo
) {	index = _index;
	baseInfo = _baseInfo;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 方块索引
/// </summary>
public int getIndex() { return index; }
/// <summary>
/// 方块索引
/// </summary>
public void setIndex(int _index) { index = _index; }
/// <summary>
/// 方块类型
/// </summary>
public Hotfix.Common.TileMatchObj.TileMatch_BlockBaseInfo getBaseInfo() { return baseInfo; }
/// <summary>
/// 方块类型
/// </summary>
public void setBaseInfo(Hotfix.Common.TileMatchObj.TileMatch_BlockBaseInfo _baseInfo) { baseInfo = _baseInfo; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	index = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _baseInfoCustLen = _buf.getInt();
	int _baseInfoCurPos = _buf.getCurPos();
	baseInfo.ReadUnzipBuf(_buf, _baseInfoCurPos + _baseInfoCustLen);
	_buf.setPosition(_baseInfoCurPos + _baseInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(index);
	_buf.putInt(baseInfo.GetBufSize());
	baseInfo.PutUnzipBuf(_buf);
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
	builder.Append("index").Append(":").Append(index.ToString()).Append(", ");
	builder.Append("baseInfo").Append(":").Append(baseInfo == null ? "null" : baseInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

