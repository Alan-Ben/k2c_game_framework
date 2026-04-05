using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.TreasureHuntObj
{

/// <summary>
/// 太空寻宝-捕捉奖励
/// </summary>
public class TreasureHunt_CaptureReward : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 获得类型
/// </summary>
private Common.TreasureHuntEnum.ETreasureHuntGainType gainType;
/// <summary>
/// 数据
/// </summary>
private byte[] data;


public TreasureHunt_CaptureReward() {
	gainType = 0;
	data = null;
}

public TreasureHunt_CaptureReward(
	Common.TreasureHuntEnum.ETreasureHuntGainType _gainType
	, byte[] _data
) {	gainType = _gainType;
	data = _data;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 获得类型
/// </summary>
public Common.TreasureHuntEnum.ETreasureHuntGainType getGainType() { return gainType; }
/// <summary>
/// 获得类型
/// </summary>
public void setGainType(Common.TreasureHuntEnum.ETreasureHuntGainType _gainType) { gainType = _gainType; }
/// <summary>
/// 数据
/// </summary>
public byte[] getData() { return data; }

/// <summary>
/// 数据
/// </summary>
public void setData(byte[] _data) { data = _data; }



public int GetBufSize() {
	int _size = 4;
	_size += 4 + (data == null ? 0 : data.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += 4 + (data == null ? 0 : data.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gainType = (Common.TreasureHuntEnum.ETreasureHuntGainType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	data = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)gainType);

	_buf.putByteBuffer(data);

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
	builder.Append("gainType").Append(":").Append(gainType.ToString()).Append(", ");
	builder.Append("data").Append(":").Append(data == null ? "null" : data.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

