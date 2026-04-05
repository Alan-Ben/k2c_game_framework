using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p007_CommOp
{

public class GS2GC_007_055_OnOfflineRewardAdd : ALBasicProtocolPack._IALProtocolStructure {
private Common.OfflineRewardObj.OfflineReward_Info reward;


public GS2GC_007_055_OnOfflineRewardAdd() {
	reward = new Common.OfflineRewardObj.OfflineReward_Info();
}

public GS2GC_007_055_OnOfflineRewardAdd(
	Common.OfflineRewardObj.OfflineReward_Info _reward
) {	reward = _reward;
}

public byte getMainOrder() { return (byte)7; }

public byte getSubOrder() { return (byte)55; }

public Common.OfflineRewardObj.OfflineReward_Info getReward() { return reward; }
public void setReward(Common.OfflineRewardObj.OfflineReward_Info _reward) { reward = _reward; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + reward.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + reward.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _rewardCustLen = _buf.getInt();
	int _rewardCurPos = _buf.getCurPos();
	reward.ReadUnzipBuf(_buf, _rewardCurPos + _rewardCustLen);
	_buf.setPosition(_rewardCurPos + _rewardCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(reward.GetBufSize());
	reward.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)55);
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
	builder.Append("reward").Append(":").Append(reward == null ? "null" : reward.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

