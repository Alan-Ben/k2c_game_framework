using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.DinnerObj
{

/// <summary>
/// 宴会凭证数据
/// </summary>
public class Dinner_Permit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 凭证实例ID
/// </summary>
private long id;
/// <summary>
/// 凭证类型
/// </summary>
private Common.DinnerEnum.EDinnerPermitType permitType;
/// <summary>
/// 凭证类型ID
/// </summary>
private long typeId;
/// <summary>
/// 凭证过期时间（秒）
/// </summary>
private int expiredTs;


public Dinner_Permit() {
	id = (long)0;
	permitType = 0;
	typeId = (long)0;
	expiredTs = 0;
}

public Dinner_Permit(
	long _id
	, Common.DinnerEnum.EDinnerPermitType _permitType
	, long _typeId
	, int _expiredTs
) {	id = _id;
	permitType = _permitType;
	typeId = _typeId;
	expiredTs = _expiredTs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 凭证实例ID
/// </summary>
public long getId() { return id; }
/// <summary>
/// 凭证实例ID
/// </summary>
public void setId(long _id) { id = _id; }
/// <summary>
/// 凭证类型
/// </summary>
public Common.DinnerEnum.EDinnerPermitType getPermitType() { return permitType; }
/// <summary>
/// 凭证类型
/// </summary>
public void setPermitType(Common.DinnerEnum.EDinnerPermitType _permitType) { permitType = _permitType; }
/// <summary>
/// 凭证类型ID
/// </summary>
public long getTypeId() { return typeId; }
/// <summary>
/// 凭证类型ID
/// </summary>
public void setTypeId(long _typeId) { typeId = _typeId; }
/// <summary>
/// 凭证过期时间（秒）
/// </summary>
public int getExpiredTs() { return expiredTs; }
/// <summary>
/// 凭证过期时间（秒）
/// </summary>
public void setExpiredTs(int _expiredTs) { expiredTs = _expiredTs; }


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
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	permitType = (Common.DinnerEnum.EDinnerPermitType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	typeId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	expiredTs = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putInt((int)permitType);

	_buf.putLong(typeId);
	_buf.putInt(expiredTs);
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
	builder.Append("permitType").Append(":").Append(permitType.ToString()).Append(", ");
	builder.Append("typeId").Append(":").Append(typeId.ToString()).Append(", ");
	builder.Append("expiredTs").Append(":").Append(expiredTs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

