using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.OfflineRewardObj
{

/// <summary>
/// 玩家记录操作
/// </summary>
public class Offline_ChgPlayerRecord : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 记录类型
/// </summary>
private NPEnum.ENPPlayerRecordParam type;
/// <summary>
/// 操作类型
/// </summary>
private NPEnum.ENCounterDealType dealType;
/// <summary>
/// 数值
/// </summary>
private long num;


public Offline_ChgPlayerRecord() {
	type = 0;
	dealType = 0;
	num = (long)0;
}

public Offline_ChgPlayerRecord(
	NPEnum.ENPPlayerRecordParam _type
	, NPEnum.ENCounterDealType _dealType
	, long _num
) {	type = _type;
	dealType = _dealType;
	num = _num;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 记录类型
/// </summary>
public NPEnum.ENPPlayerRecordParam getType() { return type; }
/// <summary>
/// 记录类型
/// </summary>
public void setType(NPEnum.ENPPlayerRecordParam _type) { type = _type; }
/// <summary>
/// 操作类型
/// </summary>
public NPEnum.ENCounterDealType getDealType() { return dealType; }
/// <summary>
/// 操作类型
/// </summary>
public void setDealType(NPEnum.ENCounterDealType _dealType) { dealType = _dealType; }
/// <summary>
/// 数值
/// </summary>
public long getNum() { return num; }
/// <summary>
/// 数值
/// </summary>
public void setNum(long _num) { num = _num; }


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
	type = (NPEnum.ENPPlayerRecordParam)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dealType = (NPEnum.ENCounterDealType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	num = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)type);

	_buf.putInt((int)dealType);

	_buf.putLong(num);
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
	builder.Append("type").Append(":").Append(type.ToString()).Append(", ");
	builder.Append("dealType").Append(":").Append(dealType.ToString()).Append(", ");
	builder.Append("num").Append(":").Append(num.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

