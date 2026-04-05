using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.WeekCardObj
{

/// <summary>
/// 周卡-信息
/// </summary>
public class WeekCard_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 过期时间 超时时间标记，单位秒
/// </summary>
private int expireTimeTagS;
/// <summary>
/// 是否使用过免费试用
/// </summary>
private bool hadUseFreeTrial;
/// <summary>
/// NPC类型
/// </summary>
private CommonEnum.EWeekCardNPCType npcType;
private long npcId;


public WeekCard_Info() {
	expireTimeTagS = 0;
	hadUseFreeTrial = false;
	npcType = 0;
	npcId = (long)0;
}

public WeekCard_Info(
	int _expireTimeTagS
	, bool _hadUseFreeTrial
	, CommonEnum.EWeekCardNPCType _npcType
	, long _npcId
) {	expireTimeTagS = _expireTimeTagS;
	hadUseFreeTrial = _hadUseFreeTrial;
	npcType = _npcType;
	npcId = _npcId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 过期时间 超时时间标记，单位秒
/// </summary>
public int getExpireTimeTagS() { return expireTimeTagS; }
/// <summary>
/// 过期时间 超时时间标记，单位秒
/// </summary>
public void setExpireTimeTagS(int _expireTimeTagS) { expireTimeTagS = _expireTimeTagS; }
/// <summary>
/// 是否使用过免费试用
/// </summary>
public bool getHadUseFreeTrial() { return hadUseFreeTrial; }
/// <summary>
/// 是否使用过免费试用
/// </summary>
public void setHadUseFreeTrial(bool _hadUseFreeTrial) { hadUseFreeTrial = _hadUseFreeTrial; }
/// <summary>
/// NPC类型
/// </summary>
public CommonEnum.EWeekCardNPCType getNpcType() { return npcType; }
/// <summary>
/// NPC类型
/// </summary>
public void setNpcType(CommonEnum.EWeekCardNPCType _npcType) { npcType = _npcType; }
public long getNpcId() { return npcId; }
public void setNpcId(long _npcId) { npcId = _npcId; }


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
	expireTimeTagS = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadUseFreeTrial = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	npcType = (CommonEnum.EWeekCardNPCType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	npcId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(expireTimeTagS);
	_buf.put(hadUseFreeTrial?(byte)1:(byte)0);
	_buf.putInt((int)npcType);

	_buf.putLong(npcId);
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
	builder.Append("expireTimeTagS").Append(":").Append(expireTimeTagS.ToString()).Append(", ");
	builder.Append("hadUseFreeTrial").Append(":").Append(hadUseFreeTrial.ToString()).Append(", ");
	builder.Append("npcType").Append(":").Append(npcType.ToString()).Append(", ");
	builder.Append("npcId").Append(":").Append(npcId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

