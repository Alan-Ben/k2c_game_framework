using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p024_DungeonOp
{

/// <summary>
/// 查询午间副本宝箱列表
/// </summary>
public class GC2GS_024_003_ReqMiddayDungeonBoxList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宝箱类型
/// </summary>
private Common.DungeonEnum.EDungeonBoxType boxType;


public GC2GS_024_003_ReqMiddayDungeonBoxList() {
	boxType = 0;
}

public GC2GS_024_003_ReqMiddayDungeonBoxList(
	Common.DungeonEnum.EDungeonBoxType _boxType
) {	boxType = _boxType;
}

public byte getMainOrder() { return (byte)24; }

public byte getSubOrder() { return (byte)3; }

/// <summary>
/// 宝箱类型
/// </summary>
public Common.DungeonEnum.EDungeonBoxType getBoxType() { return boxType; }
/// <summary>
/// 宝箱类型
/// </summary>
public void setBoxType(Common.DungeonEnum.EDungeonBoxType _boxType) { boxType = _boxType; }


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
	boxType = (Common.DungeonEnum.EDungeonBoxType)_buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)boxType);

}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)24);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)24);
	_recBuf.put((byte)3);
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
	builder.Append("boxType").Append(":").Append(boxType.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

