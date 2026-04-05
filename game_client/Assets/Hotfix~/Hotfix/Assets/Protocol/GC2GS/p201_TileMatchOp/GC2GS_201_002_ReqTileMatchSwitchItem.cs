using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.GC2GS.p201_TileMatchOp
{

/// <summary>
/// 三消交换格子
/// </summary>
public class GC2GS_201_002_ReqTileMatchSwitchItem : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 模式类型
/// </summary>
private Hotfix.TileMatchEnum.ETileMatch_ModeType modeType;
/// <summary>
/// 交换起点
/// </summary>
private int startIndex;
/// <summary>
/// 交换终点
/// </summary>
private int endIndex;


public GC2GS_201_002_ReqTileMatchSwitchItem() {
	modeType = 0;
	startIndex = 0;
	endIndex = 0;
}

public GC2GS_201_002_ReqTileMatchSwitchItem(
	Hotfix.TileMatchEnum.ETileMatch_ModeType _modeType
	, int _startIndex
	, int _endIndex
) {	modeType = _modeType;
	startIndex = _startIndex;
	endIndex = _endIndex;
}

public byte getMainOrder() { return (byte)201; }

public byte getSubOrder() { return (byte)2; }

/// <summary>
/// 模式类型
/// </summary>
public Hotfix.TileMatchEnum.ETileMatch_ModeType getModeType() { return modeType; }
/// <summary>
/// 模式类型
/// </summary>
public void setModeType(Hotfix.TileMatchEnum.ETileMatch_ModeType _modeType) { modeType = _modeType; }
/// <summary>
/// 交换起点
/// </summary>
public int getStartIndex() { return startIndex; }
/// <summary>
/// 交换起点
/// </summary>
public void setStartIndex(int _startIndex) { startIndex = _startIndex; }
/// <summary>
/// 交换终点
/// </summary>
public int getEndIndex() { return endIndex; }
/// <summary>
/// 交换终点
/// </summary>
public void setEndIndex(int _endIndex) { endIndex = _endIndex; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	modeType = (Hotfix.TileMatchEnum.ETileMatch_ModeType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	startIndex = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	endIndex = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)modeType);

	_buf.putInt(startIndex);
	_buf.putInt(endIndex);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)201);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)201);
	_recBuf.put((byte)2);
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
	builder.Append("modeType").Append(":").Append(modeType.ToString()).Append(", ");
	builder.Append("startIndex").Append(":").Append(startIndex.ToString()).Append(", ");
	builder.Append("endIndex").Append(":").Append(endIndex.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

