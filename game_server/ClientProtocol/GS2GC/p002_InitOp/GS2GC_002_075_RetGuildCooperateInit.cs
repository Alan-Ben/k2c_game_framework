using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

/// <summary>
/// 联盟协作初始化响应
/// </summary>
public class GS2GC_002_075_RetGuildCooperateInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 联盟协作信息
/// </summary>
private Common.GuildCooperateObj.GuildCooperate_Info info;
/// <summary>
/// 大臣使用信息列表
/// </summary>
private List<Common.GuildCooperateObj.GuildCooperate_HeroUseInfo> heroUseInfoList;
/// <summary>
/// 已领取奖励点列表
/// </summary>
private Common.GuildCooperateObj.GuildCooperate_HadDrawRewardPointList hadDrawRewardPointList;


public GS2GC_002_075_RetGuildCooperateInit() {
	info = new Common.GuildCooperateObj.GuildCooperate_Info();
	heroUseInfoList = new List<Common.GuildCooperateObj.GuildCooperate_HeroUseInfo>();
	hadDrawRewardPointList = new Common.GuildCooperateObj.GuildCooperate_HadDrawRewardPointList();
}

public GS2GC_002_075_RetGuildCooperateInit(
	Common.GuildCooperateObj.GuildCooperate_Info _info
	, List<Common.GuildCooperateObj.GuildCooperate_HeroUseInfo> _heroUseInfoList
	, Common.GuildCooperateObj.GuildCooperate_HadDrawRewardPointList _hadDrawRewardPointList
) {	info = _info;
	heroUseInfoList = _heroUseInfoList;
	hadDrawRewardPointList = _hadDrawRewardPointList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)75; }

/// <summary>
/// 联盟协作信息
/// </summary>
public Common.GuildCooperateObj.GuildCooperate_Info getInfo() { return info; }
/// <summary>
/// 联盟协作信息
/// </summary>
public void setInfo(Common.GuildCooperateObj.GuildCooperate_Info _info) { info = _info; }
/// <summary>
/// 大臣使用信息列表
/// </summary>
public List<Common.GuildCooperateObj.GuildCooperate_HeroUseInfo> getHeroUseInfoList() { return heroUseInfoList; }
/// <summary>
/// 大臣使用信息列表
/// </summary>
public void addHeroUseInfoList(Common.GuildCooperateObj.GuildCooperate_HeroUseInfo _heroUseInfoList) { heroUseInfoList.Add(_heroUseInfoList); }
/// <summary>
/// 已领取奖励点列表
/// </summary>
public Common.GuildCooperateObj.GuildCooperate_HadDrawRewardPointList getHadDrawRewardPointList() { return hadDrawRewardPointList; }
/// <summary>
/// 已领取奖励点列表
/// </summary>
public void setHadDrawRewardPointList(Common.GuildCooperateObj.GuildCooperate_HadDrawRewardPointList _hadDrawRewardPointList) { hadDrawRewardPointList = _hadDrawRewardPointList; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + info.GetBufSize();
	_size += 2 + (heroUseInfoList.Count * 28);
	_size += 4 + hadDrawRewardPointList.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + info.GetBufSize();
	_size += 2 + (heroUseInfoList.Count * 28);
	_size += 4 + hadDrawRewardPointList.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _infoCustLen = _buf.getInt();
	int _infoCurPos = _buf.getCurPos();
	info.ReadUnzipBuf(_buf, _infoCurPos + _infoCustLen);
	_buf.setPosition(_infoCurPos + _infoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _heroUseInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _heroUseInfoListCount; _i++) { 
		Common.GuildCooperateObj.GuildCooperate_HeroUseInfo _heroUseInfoList = new Common.GuildCooperateObj.GuildCooperate_HeroUseInfo();
		int __heroUseInfoListCustLen = _buf.getInt();
	int __heroUseInfoListCurPos = _buf.getCurPos();
	_heroUseInfoList.ReadUnzipBuf(_buf, __heroUseInfoListCurPos + __heroUseInfoListCustLen);
	_buf.setPosition(__heroUseInfoListCurPos + __heroUseInfoListCustLen);

		heroUseInfoList.Add(_heroUseInfoList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _hadDrawRewardPointListCustLen = _buf.getInt();
	int _hadDrawRewardPointListCurPos = _buf.getCurPos();
	hadDrawRewardPointList.ReadUnzipBuf(_buf, _hadDrawRewardPointListCurPos + _hadDrawRewardPointListCustLen);
	_buf.setPosition(_hadDrawRewardPointListCurPos + _hadDrawRewardPointListCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(info.GetBufSize());
	info.PutUnzipBuf(_buf);
	_buf.putShort((short)heroUseInfoList.Count);
	for(int _i = 0; _i < heroUseInfoList.Count; _i++) { 
		_buf.putInt(heroUseInfoList[_i].GetBufSize());
	heroUseInfoList[_i].PutUnzipBuf(_buf);
	}
	_buf.putInt(hadDrawRewardPointList.GetBufSize());
	hadDrawRewardPointList.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)75);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)75);
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
	builder.Append("info").Append(":").Append(info == null ? "null" : info.ToString()).Append(", ");
	builder.Append("heroUseInfoList").Append(":").Append(heroUseInfoList.ToString()).Append(", ");
	builder.Append("hadDrawRewardPointList").Append(":").Append(hadDrawRewardPointList == null ? "null" : hadDrawRewardPointList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

