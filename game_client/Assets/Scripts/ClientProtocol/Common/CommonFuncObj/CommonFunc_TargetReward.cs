using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.CommonFuncObj
{

/// <summary>
/// 目标奖励信息
/// </summary>
public class CommonFunc_TargetReward : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 目标id
/// </summary>
private long id;
/// <summary>
/// 计数
/// </summary>
private long value;
/// <summary>
/// 是否领取
/// </summary>
private bool hadDraw;


public CommonFunc_TargetReward() {
	id = (long)0;
	value = (long)0;
	hadDraw = false;
}

public CommonFunc_TargetReward(
	long _id
	, long _value
	, bool _hadDraw
) {	id = _id;
	value = _value;
	hadDraw = _hadDraw;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 目标id
/// </summary>
public long getId() { return id; }
/// <summary>
/// 目标id
/// </summary>
public void setId(long _id) { id = _id; }
/// <summary>
/// 计数
/// </summary>
public long getValue() { return value; }
/// <summary>
/// 计数
/// </summary>
public void setValue(long _value) { value = _value; }
/// <summary>
/// 是否领取
/// </summary>
public bool getHadDraw() { return hadDraw; }
/// <summary>
/// 是否领取
/// </summary>
public void setHadDraw(bool _hadDraw) { hadDraw = _hadDraw; }


public int GetBufSize() {
	int _size = 17;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	value = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadDraw = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putLong(value);
	_buf.put(hadDraw?(byte)1:(byte)0);
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
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("value").Append(":").Append(value.ToString()).Append(", ");
	builder.Append("hadDraw").Append(":").Append(hadDraw.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

