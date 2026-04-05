using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.BagItemUseObj
{

/// <summary>
/// 背包使用道具-妃子展示信息
/// </summary>
public class BagItemUse_ConsortShowInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 妃子id
/// </summary>
private long consortId;
private Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType type;
/// <summary>
/// 单次数值
/// </summary>
private long count;


public BagItemUse_ConsortShowInfo() {
	consortId = (long)0;
	type = 0;
	count = (long)0;
}

public BagItemUse_ConsortShowInfo(
	long _consortId
	, Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType _type
	, long _count
) {	consortId = _consortId;
	type = _type;
	count = _count;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 妃子id
/// </summary>
public long getConsortId() { return consortId; }
/// <summary>
/// 妃子id
/// </summary>
public void setConsortId(long _consortId) { consortId = _consortId; }
public Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType getType() { return type; }
public void setType(Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType _type) { type = _type; }
/// <summary>
/// 单次数值
/// </summary>
public long getCount() { return count; }
/// <summary>
/// 单次数值
/// </summary>
public void setCount(long _count) { count = _count; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	type = (Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	count = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(consortId);
	_buf.putInt((int)type);

	_buf.putLong(count);
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
	builder.Append("consortId").Append(":").Append(consortId.ToString()).Append(", ");
	builder.Append("type").Append(":").Append(type.ToString()).Append(", ");
	builder.Append("count").Append(":").Append(count.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

