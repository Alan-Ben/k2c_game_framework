using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.GS2GC.p201_TileMatchOp
{

/// <summary>
/// 三消阶段奖励数据变更
/// </summary>
public class GS2GC_201_053_OnTileMatchStepRewardChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 阶段奖励数据
/// </summary>
private Hotfix.Common.TileMatchObj.TileMatch_StepRewardInfo stepRewardInfo;


public GS2GC_201_053_OnTileMatchStepRewardChg() {
	stepRewardInfo = new Hotfix.Common.TileMatchObj.TileMatch_StepRewardInfo();
}

public GS2GC_201_053_OnTileMatchStepRewardChg(
	Hotfix.Common.TileMatchObj.TileMatch_StepRewardInfo _stepRewardInfo
) {	stepRewardInfo = _stepRewardInfo;
}

public byte getMainOrder() { return (byte)201; }

public byte getSubOrder() { return (byte)53; }

/// <summary>
/// 阶段奖励数据
/// </summary>
public Hotfix.Common.TileMatchObj.TileMatch_StepRewardInfo getStepRewardInfo() { return stepRewardInfo; }
/// <summary>
/// 阶段奖励数据
/// </summary>
public void setStepRewardInfo(Hotfix.Common.TileMatchObj.TileMatch_StepRewardInfo _stepRewardInfo) { stepRewardInfo = _stepRewardInfo; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _stepRewardInfoCustLen = _buf.getInt();
	int _stepRewardInfoCurPos = _buf.getCurPos();
	stepRewardInfo.ReadUnzipBuf(_buf, _stepRewardInfoCurPos + _stepRewardInfoCustLen);
	_buf.setPosition(_stepRewardInfoCurPos + _stepRewardInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(stepRewardInfo.GetBufSize());
	stepRewardInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)201);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)201);
	_recBuf.put((byte)53);
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
	builder.Append("stepRewardInfo").Append(":").Append(stepRewardInfo == null ? "null" : stepRewardInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

