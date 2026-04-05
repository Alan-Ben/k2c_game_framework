using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.GC2GS.p201_TileMatchOp
{

/// <summary>
/// 三消游戏方块数据初始化
/// </summary>
public class GC2GS_201_001_ReqTileMatchBlockInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 模式类型
/// </summary>
private Hotfix.TileMatchEnum.ETileMatch_ModeType modeType;


public GC2GS_201_001_ReqTileMatchBlockInit() {
	modeType = 0;
}

public GC2GS_201_001_ReqTileMatchBlockInit(
	Hotfix.TileMatchEnum.ETileMatch_ModeType _modeType
) {	modeType = _modeType;
}

public byte getMainOrder() { return (byte)201; }

public byte getSubOrder() { return (byte)1; }

/// <summary>
/// 模式类型
/// </summary>
public Hotfix.TileMatchEnum.ETileMatch_ModeType getModeType() { return modeType; }
/// <summary>
/// 模式类型
/// </summary>
public void setModeType(Hotfix.TileMatchEnum.ETileMatch_ModeType _modeType) { modeType = _modeType; }


public int GetBufSize() {
	int _size = 4;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	modeType = (Hotfix.TileMatchEnum.ETileMatch_ModeType)_buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)modeType);

}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)201);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)201);
	_recBuf.put((byte)1);
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
	builder.Append("}");
	return builder.ToString();
}

}

}

