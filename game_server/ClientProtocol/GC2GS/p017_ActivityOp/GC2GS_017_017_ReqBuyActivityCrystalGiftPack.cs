using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p017_ActivityOp
{

/// <summary>
/// 购买活动钻石礼包
/// </summary>
public class GC2GS_017_017_ReqBuyActivityCrystalGiftPack : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动实例id
/// </summary>
private long instanceId;
/// <summary>
/// 礼包组id
/// </summary>
private long groupId;
/// <summary>
/// 礼包id
/// </summary>
private long packId;
/// <summary>
/// 购买数量
/// </summary>
private int num;


public GC2GS_017_017_ReqBuyActivityCrystalGiftPack() {
	instanceId = (long)0;
	groupId = (long)0;
	packId = (long)0;
	num = 0;
}

public GC2GS_017_017_ReqBuyActivityCrystalGiftPack(
	long _instanceId
	, long _groupId
	, long _packId
	, int _num
) {	instanceId = _instanceId;
	groupId = _groupId;
	packId = _packId;
	num = _num;
}

public byte getMainOrder() { return (byte)17; }

public byte getSubOrder() { return (byte)17; }

/// <summary>
/// 活动实例id
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 活动实例id
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 礼包组id
/// </summary>
public long getGroupId() { return groupId; }
/// <summary>
/// 礼包组id
/// </summary>
public void setGroupId(long _groupId) { groupId = _groupId; }
/// <summary>
/// 礼包id
/// </summary>
public long getPackId() { return packId; }
/// <summary>
/// 礼包id
/// </summary>
public void setPackId(long _packId) { packId = _packId; }
/// <summary>
/// 购买数量
/// </summary>
public int getNum() { return num; }
/// <summary>
/// 购买数量
/// </summary>
public void setNum(int _num) { num = _num; }


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
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	packId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	num = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(groupId);
	_buf.putLong(packId);
	_buf.putInt(num);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)17);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)17);
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
	builder.Append("instanceId").Append(":").Append(instanceId.ToString()).Append(", ");
	builder.Append("groupId").Append(":").Append(groupId.ToString()).Append(", ");
	builder.Append("packId").Append(":").Append(packId.ToString()).Append(", ");
	builder.Append("num").Append(":").Append(num.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

