using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.Common.TileMatchObj
{

/// <summary>
/// 三消-格子位置变更
/// </summary>
public class TileMatch_BlockPosChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 原始方块索引
/// </summary>
private int oriBlockIndex;
/// <summary>
/// 目标方块索引
/// </summary>
private int tarBlockIndex;


public TileMatch_BlockPosChg() {
	oriBlockIndex = 0;
	tarBlockIndex = 0;
}

public TileMatch_BlockPosChg(
	int _oriBlockIndex
	, int _tarBlockIndex
) {	oriBlockIndex = _oriBlockIndex;
	tarBlockIndex = _tarBlockIndex;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 原始方块索引
/// </summary>
public int getOriBlockIndex() { return oriBlockIndex; }
/// <summary>
/// 原始方块索引
/// </summary>
public void setOriBlockIndex(int _oriBlockIndex) { oriBlockIndex = _oriBlockIndex; }
/// <summary>
/// 目标方块索引
/// </summary>
public int getTarBlockIndex() { return tarBlockIndex; }
/// <summary>
/// 目标方块索引
/// </summary>
public void setTarBlockIndex(int _tarBlockIndex) { tarBlockIndex = _tarBlockIndex; }


public int GetBufSize() {
	int _size = 8;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	oriBlockIndex = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	tarBlockIndex = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(oriBlockIndex);
	_buf.putInt(tarBlockIndex);
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
	builder.Append("oriBlockIndex").Append(":").Append(oriBlockIndex.ToString()).Append(", ");
	builder.Append("tarBlockIndex").Append(":").Append(tarBlockIndex.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

