using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ChildObj
{

/// <summary>
/// 子嗣（成年未婚）数据
/// </summary>
public class Adult_UnmarriedInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 子嗣数据
/// </summary>
private Common.ChildObj.Adult_Info adult;
/// <summary>
/// 子嗣状态
/// </summary>
private Common.ChildEnum.EAdultStatus status;
/// <summary>
/// 请求过期截至时间（秒）
/// </summary>
private int expiredTs;
/// <summary>
/// 允许联姻的最小收益数值
/// </summary>
private long minAllowBonus;


public Adult_UnmarriedInfo() {
	adult = new Common.ChildObj.Adult_Info();
	status = 0;
	expiredTs = 0;
	minAllowBonus = (long)0;
}

public Adult_UnmarriedInfo(
	Common.ChildObj.Adult_Info _adult
	, Common.ChildEnum.EAdultStatus _status
	, int _expiredTs
	, long _minAllowBonus
) {	adult = _adult;
	status = _status;
	expiredTs = _expiredTs;
	minAllowBonus = _minAllowBonus;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 子嗣数据
/// </summary>
public Common.ChildObj.Adult_Info getAdult() { return adult; }
/// <summary>
/// 子嗣数据
/// </summary>
public void setAdult(Common.ChildObj.Adult_Info _adult) { adult = _adult; }
/// <summary>
/// 子嗣状态
/// </summary>
public Common.ChildEnum.EAdultStatus getStatus() { return status; }
/// <summary>
/// 子嗣状态
/// </summary>
public void setStatus(Common.ChildEnum.EAdultStatus _status) { status = _status; }
/// <summary>
/// 请求过期截至时间（秒）
/// </summary>
public int getExpiredTs() { return expiredTs; }
/// <summary>
/// 请求过期截至时间（秒）
/// </summary>
public void setExpiredTs(int _expiredTs) { expiredTs = _expiredTs; }
/// <summary>
/// 允许联姻的最小收益数值
/// </summary>
public long getMinAllowBonus() { return minAllowBonus; }
/// <summary>
/// 允许联姻的最小收益数值
/// </summary>
public void setMinAllowBonus(long _minAllowBonus) { minAllowBonus = _minAllowBonus; }


public int GetBufSize() {
	int _size = 16;
	_size += 4 + adult.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;
	_size += 4 + adult.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _adultCustLen = _buf.getInt();
	int _adultCurPos = _buf.getCurPos();
	adult.ReadUnzipBuf(_buf, _adultCurPos + _adultCustLen);
	_buf.setPosition(_adultCurPos + _adultCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	status = (Common.ChildEnum.EAdultStatus)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	expiredTs = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	minAllowBonus = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(adult.GetBufSize());
	adult.PutUnzipBuf(_buf);
	_buf.putInt((int)status);

	_buf.putInt(expiredTs);
	_buf.putLong(minAllowBonus);
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
	builder.Append("adult").Append(":").Append(adult == null ? "null" : adult.ToString()).Append(", ");
	builder.Append("status").Append(":").Append(status.ToString()).Append(", ");
	builder.Append("expiredTs").Append(":").Append(expiredTs.ToString()).Append(", ");
	builder.Append("minAllowBonus").Append(":").Append(minAllowBonus.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

