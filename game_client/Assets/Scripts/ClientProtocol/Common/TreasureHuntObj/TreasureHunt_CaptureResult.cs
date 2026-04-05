using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.TreasureHuntObj
{

/// <summary>
/// 太空寻宝-捕捉结果
/// </summary>
public class TreasureHunt_CaptureResult : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 捕捉奖励列表
/// </summary>
private List<Common.TreasureHuntObj.TreasureHunt_CaptureReward> captureRewardList;


public TreasureHunt_CaptureResult() {
	captureRewardList = new List<Common.TreasureHuntObj.TreasureHunt_CaptureReward>();
}

public TreasureHunt_CaptureResult(
	List<Common.TreasureHuntObj.TreasureHunt_CaptureReward> _captureRewardList
) {	captureRewardList = _captureRewardList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 捕捉奖励列表
/// </summary>
public List<Common.TreasureHuntObj.TreasureHunt_CaptureReward> getCaptureRewardList() { return captureRewardList; }
/// <summary>
/// 捕捉奖励列表
/// </summary>
public void addCaptureRewardList(Common.TreasureHuntObj.TreasureHunt_CaptureReward _captureRewardList) { captureRewardList.Add(_captureRewardList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < captureRewardList.Count; _i++) {
	_size += 4 + captureRewardList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < captureRewardList.Count; _i++) {
	_size += 4 + captureRewardList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _captureRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _captureRewardListCount; _i++) { 
		Common.TreasureHuntObj.TreasureHunt_CaptureReward _captureRewardList = new Common.TreasureHuntObj.TreasureHunt_CaptureReward();
		int __captureRewardListCustLen = _buf.getInt();
	int __captureRewardListCurPos = _buf.getCurPos();
	_captureRewardList.ReadUnzipBuf(_buf, __captureRewardListCurPos + __captureRewardListCustLen);
	_buf.setPosition(__captureRewardListCurPos + __captureRewardListCustLen);

		captureRewardList.Add(_captureRewardList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)captureRewardList.Count);
	for(int _i = 0; _i < captureRewardList.Count; _i++) { 
		_buf.putInt(captureRewardList[_i].GetBufSize());
	captureRewardList[_i].PutUnzipBuf(_buf);
	}
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
	builder.Append("captureRewardList").Append(":").Append(captureRewardList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

