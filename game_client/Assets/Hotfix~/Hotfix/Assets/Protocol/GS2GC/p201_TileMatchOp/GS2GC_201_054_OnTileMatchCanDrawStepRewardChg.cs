using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.GS2GC.p201_TileMatchOp
{

/// <summary>
/// 三消阶段可领取奖励数据变更
/// </summary>
public class GS2GC_201_054_OnTileMatchCanDrawStepRewardChg : ALBasicProtocolPack._IALProtocolStructure {
private List<Hotfix.Common.TileMatchObj.TileMatch_CanDrawStepReward> canDrawStepRewardList;


public GS2GC_201_054_OnTileMatchCanDrawStepRewardChg() {
	canDrawStepRewardList = new List<Hotfix.Common.TileMatchObj.TileMatch_CanDrawStepReward>();
}

public GS2GC_201_054_OnTileMatchCanDrawStepRewardChg(
	List<Hotfix.Common.TileMatchObj.TileMatch_CanDrawStepReward> _canDrawStepRewardList
) {	canDrawStepRewardList = _canDrawStepRewardList;
}

public byte getMainOrder() { return (byte)201; }

public byte getSubOrder() { return (byte)54; }

public List<Hotfix.Common.TileMatchObj.TileMatch_CanDrawStepReward> getCanDrawStepRewardList() { return canDrawStepRewardList; }
public void addCanDrawStepRewardList(Hotfix.Common.TileMatchObj.TileMatch_CanDrawStepReward _canDrawStepRewardList) { canDrawStepRewardList.Add(_canDrawStepRewardList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (canDrawStepRewardList.Count * 12);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (canDrawStepRewardList.Count * 12);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _canDrawStepRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _canDrawStepRewardListCount; _i++) { 
		Hotfix.Common.TileMatchObj.TileMatch_CanDrawStepReward _canDrawStepRewardList = new Hotfix.Common.TileMatchObj.TileMatch_CanDrawStepReward();
		int __canDrawStepRewardListCustLen = _buf.getInt();
	int __canDrawStepRewardListCurPos = _buf.getCurPos();
	_canDrawStepRewardList.ReadUnzipBuf(_buf, __canDrawStepRewardListCurPos + __canDrawStepRewardListCustLen);
	_buf.setPosition(__canDrawStepRewardListCurPos + __canDrawStepRewardListCustLen);

		canDrawStepRewardList.Add(_canDrawStepRewardList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)canDrawStepRewardList.Count);
	for(int _i = 0; _i < canDrawStepRewardList.Count; _i++) { 
		_buf.putInt(canDrawStepRewardList[_i].GetBufSize());
	canDrawStepRewardList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)201);
	_buf.put((byte)54);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)201);
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
	builder.Append("canDrawStepRewardList").Append(":").Append(canDrawStepRewardList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

