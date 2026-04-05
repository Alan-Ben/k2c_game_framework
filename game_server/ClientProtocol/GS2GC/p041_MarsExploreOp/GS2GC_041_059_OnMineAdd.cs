using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p041_MarsExploreOp
{

/// <summary>
/// 火星探索-矿产数据增加
/// </summary>
public class GS2GC_041_059_OnMineAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 索引数据
/// </summary>
private Common.MarsObj.Mars_MineIdx idx;


public GS2GC_041_059_OnMineAdd() {
	idx = new Common.MarsObj.Mars_MineIdx();
}

public GS2GC_041_059_OnMineAdd(
	Common.MarsObj.Mars_MineIdx _idx
) {	idx = _idx;
}

public byte getMainOrder() { return (byte)41; }

public byte getSubOrder() { return (byte)59; }

/// <summary>
/// 索引数据
/// </summary>
public Common.MarsObj.Mars_MineIdx getIdx() { return idx; }
/// <summary>
/// 索引数据
/// </summary>
public void setIdx(Common.MarsObj.Mars_MineIdx _idx) { idx = _idx; }


public int GetBufSize() {
	int _size = 44;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 46;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _idxCustLen = _buf.getInt();
	int _idxCurPos = _buf.getCurPos();
	idx.ReadUnzipBuf(_buf, _idxCurPos + _idxCustLen);
	_buf.setPosition(_idxCurPos + _idxCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(idx.GetBufSize());
	idx.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)59);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)59);
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
	builder.Append("idx").Append(":").Append(idx == null ? "null" : idx.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

