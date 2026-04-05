using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.Common.TileMatchObj
{

/// <summary>
/// 三消-逻辑信息
/// </summary>
public class TileMatch_LogicInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 逻辑序列号
/// </summary>
private int serialId;
/// <summary>
/// 逻辑类型
/// </summary>
private Hotfix.TileMatchEnum.ETileMatch_LogicType logicType;
/// <summary>
/// 逻辑信息
/// </summary>
private byte[] data;
/// <summary>
/// 分数
/// </summary>
private int score;


public TileMatch_LogicInfo() {
	serialId = 0;
	logicType = 0;
	data = null;
	score = 0;
}

public TileMatch_LogicInfo(
	int _serialId
	, Hotfix.TileMatchEnum.ETileMatch_LogicType _logicType
	, byte[] _data
	, int _score
) {	serialId = _serialId;
	logicType = _logicType;
	data = _data;
	score = _score;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 逻辑序列号
/// </summary>
public int getSerialId() { return serialId; }
/// <summary>
/// 逻辑序列号
/// </summary>
public void setSerialId(int _serialId) { serialId = _serialId; }
/// <summary>
/// 逻辑类型
/// </summary>
public Hotfix.TileMatchEnum.ETileMatch_LogicType getLogicType() { return logicType; }
/// <summary>
/// 逻辑类型
/// </summary>
public void setLogicType(Hotfix.TileMatchEnum.ETileMatch_LogicType _logicType) { logicType = _logicType; }
/// <summary>
/// 逻辑信息
/// </summary>
public byte[] getData() { return data; }

/// <summary>
/// 逻辑信息
/// </summary>
public void setData(byte[] _data) { data = _data; }

/// <summary>
/// 分数
/// </summary>
public int getScore() { return score; }
/// <summary>
/// 分数
/// </summary>
public void setScore(int _score) { score = _score; }


public int GetBufSize() {
	int _size = 12;
	_size += 4 + (data == null ? 0 : data.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;
	_size += 4 + (data == null ? 0 : data.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	serialId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	logicType = (Hotfix.TileMatchEnum.ETileMatch_LogicType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	data = _buf.getByteBuffer();

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	score = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(serialId);
	_buf.putInt((int)logicType);

	_buf.putByteBuffer(data);

	_buf.putInt(score);
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
	builder.Append("serialId").Append(":").Append(serialId.ToString()).Append(", ");
	builder.Append("logicType").Append(":").Append(logicType.ToString()).Append(", ");
	builder.Append("data").Append(":").Append(data == null ? "null" : data.ToString()).Append(", ");
	builder.Append("score").Append(":").Append(score.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

