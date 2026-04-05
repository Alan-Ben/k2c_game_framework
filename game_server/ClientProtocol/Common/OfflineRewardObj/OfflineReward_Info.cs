using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.OfflineRewardObj
{

/// <summary>
/// 离线奖励数据
/// </summary>
public class OfflineReward_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 实例ID
/// </summary>
private long id;
/// <summary>
/// 奖励类型 - ECommonOfflineRewardType类型
/// </summary>
private int rewardType;
/// <summary>
/// 奖励展示
/// </summary>
private byte[] rewardShow;


public OfflineReward_Info() {
	id = (long)0;
	rewardType = 0;
	rewardShow = null;
}

public OfflineReward_Info(
	long _id
	, int _rewardType
	, byte[] _rewardShow
) {	id = _id;
	rewardType = _rewardType;
	rewardShow = _rewardShow;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 实例ID
/// </summary>
public long getId() { return id; }
/// <summary>
/// 实例ID
/// </summary>
public void setId(long _id) { id = _id; }
/// <summary>
/// 奖励类型 - ECommonOfflineRewardType类型
/// </summary>
public int getRewardType() { return rewardType; }
/// <summary>
/// 奖励类型 - ECommonOfflineRewardType类型
/// </summary>
public void setRewardType(int _rewardType) { rewardType = _rewardType; }
/// <summary>
/// 奖励展示
/// </summary>
public byte[] getRewardShow() { return rewardShow; }

/// <summary>
/// 奖励展示
/// </summary>
public void setRewardShow(byte[] _rewardShow) { rewardShow = _rewardShow; }



public int GetBufSize() {
	int _size = 12;
	_size += 4 + (rewardShow == null ? 0 : rewardShow.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;
	_size += 4 + (rewardShow == null ? 0 : rewardShow.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	rewardType = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	rewardShow = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putInt(rewardType);
	_buf.putByteBuffer(rewardShow);

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
	builder.Append("rewardType").Append(":").Append(rewardType.ToString()).Append(", ");
	builder.Append("rewardShow").Append(":").Append(rewardShow == null ? "null" : rewardShow.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

