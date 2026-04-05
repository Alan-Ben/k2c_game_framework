using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

/// <summary>
/// 离线玩家奖励初始化数据
/// </summary>
public class GS2GC_002_054_RetOfflineRewardInit : ALBasicProtocolPack._IALProtocolStructure {
private List<Common.OfflineRewardObj.OfflineReward_Info> rewardList;


public GS2GC_002_054_RetOfflineRewardInit() {
	rewardList = new List<Common.OfflineRewardObj.OfflineReward_Info>();
}

public GS2GC_002_054_RetOfflineRewardInit(
	List<Common.OfflineRewardObj.OfflineReward_Info> _rewardList
) {	rewardList = _rewardList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)54; }

public List<Common.OfflineRewardObj.OfflineReward_Info> getRewardList() { return rewardList; }
public void addRewardList(Common.OfflineRewardObj.OfflineReward_Info _rewardList) { rewardList.Add(_rewardList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < rewardList.Count; _i++) {
	_size += 4 + rewardList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < rewardList.Count; _i++) {
	_size += 4 + rewardList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _rewardListCount = _buf.getShort();
	for(int _i = 0; _i < _rewardListCount; _i++) { 
		Common.OfflineRewardObj.OfflineReward_Info _rewardList = new Common.OfflineRewardObj.OfflineReward_Info();
		int __rewardListCustLen = _buf.getInt();
	int __rewardListCurPos = _buf.getCurPos();
	_rewardList.ReadUnzipBuf(_buf, __rewardListCurPos + __rewardListCustLen);
	_buf.setPosition(__rewardListCurPos + __rewardListCustLen);

		rewardList.Add(_rewardList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)rewardList.Count);
	for(int _i = 0; _i < rewardList.Count; _i++) { 
		_buf.putInt(rewardList[_i].GetBufSize());
	rewardList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)54);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)54);
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
	builder.Append("rewardList").Append(":").Append(rewardList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

