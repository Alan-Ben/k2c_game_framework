using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildCooperateObj
{

/// <summary>
/// 联盟协作属性据点信息
/// </summary>
public class GuildCooperate_PropertyPointInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 属性据点索引，从0开始
/// </summary>
private int index;
/// <summary>
/// 所属属性
/// </summary>
private CommonEnum.ESpecAttrType attr;
/// <summary>
/// 已攻击血量
/// </summary>
private long hadAttackHp;
/// <summary>
/// 总血量
/// </summary>
private long totalHp;


public GuildCooperate_PropertyPointInfo() {
	index = 0;
	attr = 0;
	hadAttackHp = (long)0;
	totalHp = (long)0;
}

public GuildCooperate_PropertyPointInfo(
	int _index
	, CommonEnum.ESpecAttrType _attr
	, long _hadAttackHp
	, long _totalHp
) {	index = _index;
	attr = _attr;
	hadAttackHp = _hadAttackHp;
	totalHp = _totalHp;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 属性据点索引，从0开始
/// </summary>
public int getIndex() { return index; }
/// <summary>
/// 属性据点索引，从0开始
/// </summary>
public void setIndex(int _index) { index = _index; }
/// <summary>
/// 所属属性
/// </summary>
public CommonEnum.ESpecAttrType getAttr() { return attr; }
/// <summary>
/// 所属属性
/// </summary>
public void setAttr(CommonEnum.ESpecAttrType _attr) { attr = _attr; }
/// <summary>
/// 已攻击血量
/// </summary>
public long getHadAttackHp() { return hadAttackHp; }
/// <summary>
/// 已攻击血量
/// </summary>
public void setHadAttackHp(long _hadAttackHp) { hadAttackHp = _hadAttackHp; }
/// <summary>
/// 总血量
/// </summary>
public long getTotalHp() { return totalHp; }
/// <summary>
/// 总血量
/// </summary>
public void setTotalHp(long _totalHp) { totalHp = _totalHp; }


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	index = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	attr = (CommonEnum.ESpecAttrType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadAttackHp = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	totalHp = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(index);
	_buf.putInt((int)attr);

	_buf.putLong(hadAttackHp);
	_buf.putLong(totalHp);
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
	builder.Append("index").Append(":").Append(index.ToString()).Append(", ");
	builder.Append("attr").Append(":").Append(attr.ToString()).Append(", ");
	builder.Append("hadAttackHp").Append(":").Append(hadAttackHp.ToString()).Append(", ");
	builder.Append("totalHp").Append(":").Append(totalHp.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

