using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p032_GuildOp
{

/// <summary>
/// 攻击属性据点
/// </summary>
public class GC2GS_032_044_ReqAttackPropertyPoint : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 奖励据点位置
/// </summary>
private Common.GuildCooperateObj.GuildCooperate_RewardPointPos pos;
/// <summary>
/// 属性据点索引
/// </summary>
private int propertyPointIndex;
/// <summary>
/// 出战大臣ID
/// </summary>
private long heroId;


public GC2GS_032_044_ReqAttackPropertyPoint() {
	pos = new Common.GuildCooperateObj.GuildCooperate_RewardPointPos();
	propertyPointIndex = 0;
	heroId = (long)0;
}

public GC2GS_032_044_ReqAttackPropertyPoint(
	Common.GuildCooperateObj.GuildCooperate_RewardPointPos _pos
	, int _propertyPointIndex
	, long _heroId
) {	pos = _pos;
	propertyPointIndex = _propertyPointIndex;
	heroId = _heroId;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)44; }

/// <summary>
/// 奖励据点位置
/// </summary>
public Common.GuildCooperateObj.GuildCooperate_RewardPointPos getPos() { return pos; }
/// <summary>
/// 奖励据点位置
/// </summary>
public void setPos(Common.GuildCooperateObj.GuildCooperate_RewardPointPos _pos) { pos = _pos; }
/// <summary>
/// 属性据点索引
/// </summary>
public int getPropertyPointIndex() { return propertyPointIndex; }
/// <summary>
/// 属性据点索引
/// </summary>
public void setPropertyPointIndex(int _propertyPointIndex) { propertyPointIndex = _propertyPointIndex; }
/// <summary>
/// 出战大臣ID
/// </summary>
public long getHeroId() { return heroId; }
/// <summary>
/// 出战大臣ID
/// </summary>
public void setHeroId(long _heroId) { heroId = _heroId; }


public int GetBufSize() {
	int _size = 28;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _posCustLen = _buf.getInt();
	int _posCurPos = _buf.getCurPos();
	pos.ReadUnzipBuf(_buf, _posCurPos + _posCustLen);
	_buf.setPosition(_posCurPos + _posCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	propertyPointIndex = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	heroId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(pos.GetBufSize());
	pos.PutUnzipBuf(_buf);
	_buf.putInt(propertyPointIndex);
	_buf.putLong(heroId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)44);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)44);
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
	builder.Append("pos").Append(":").Append(pos == null ? "null" : pos.ToString()).Append(", ");
	builder.Append("propertyPointIndex").Append(":").Append(propertyPointIndex.ToString()).Append(", ");
	builder.Append("heroId").Append(":").Append(heroId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

