using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p032_GuildOp
{

/// <summary>
/// 联盟协作奖励据点解锁推送
/// </summary>
public class GS2GC_032_080_OnRewardPointUnlock : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 奖励据点位置列表
/// </summary>
private List<Common.GuildCooperateObj.GuildCooperate_RewardPointPos> posList;


public GS2GC_032_080_OnRewardPointUnlock() {
	posList = new List<Common.GuildCooperateObj.GuildCooperate_RewardPointPos>();
}

public GS2GC_032_080_OnRewardPointUnlock(
	List<Common.GuildCooperateObj.GuildCooperate_RewardPointPos> _posList
) {	posList = _posList;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)80; }

/// <summary>
/// 奖励据点位置列表
/// </summary>
public List<Common.GuildCooperateObj.GuildCooperate_RewardPointPos> getPosList() { return posList; }
/// <summary>
/// 奖励据点位置列表
/// </summary>
public void addPosList(Common.GuildCooperateObj.GuildCooperate_RewardPointPos _posList) { posList.Add(_posList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (posList.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (posList.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _posListCount = _buf.getShort();
	for(int _i = 0; _i < _posListCount; _i++) { 
		Common.GuildCooperateObj.GuildCooperate_RewardPointPos _posList = new Common.GuildCooperateObj.GuildCooperate_RewardPointPos();
		int __posListCustLen = _buf.getInt();
	int __posListCurPos = _buf.getCurPos();
	_posList.ReadUnzipBuf(_buf, __posListCurPos + __posListCustLen);
	_buf.setPosition(__posListCurPos + __posListCustLen);

		posList.Add(_posList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)posList.Count);
	for(int _i = 0; _i < posList.Count; _i++) { 
		_buf.putInt(posList[_i].GetBufSize());
	posList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)80);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)80);
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
	builder.Append("posList").Append(":").Append(posList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

