using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

/// <summary>
/// 称号初始化数据
/// </summary>
public class GS2GC_002_020_RetPlayerTitle : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 已解锁的普通称号列表
/// </summary>
private List<Common.NpPlayerInfoObj.PlayerInfo_Title> infoList;
/// <summary>
/// 已解锁的组合称号前缀列表
/// </summary>
private List<Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre> comboPreList;
/// <summary>
/// 已解锁的组合称号后缀列表
/// </summary>
private List<Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx> comboSfxList;
/// <summary>
/// 已解锁的组合称号底色列表
/// </summary>
private List<Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg> comboBgList;
/// <summary>
/// 玩家当前穿戴称号数据
/// </summary>
private NPCommon.PlayerInfo_CurTitle curInfo;
/// <summary>
/// 是否展示称号
/// </summary>
private bool isShow;


public GS2GC_002_020_RetPlayerTitle() {
	infoList = new List<Common.NpPlayerInfoObj.PlayerInfo_Title>();
	comboPreList = new List<Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre>();
	comboSfxList = new List<Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx>();
	comboBgList = new List<Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg>();
	curInfo = new NPCommon.PlayerInfo_CurTitle();
	isShow = false;
}

public GS2GC_002_020_RetPlayerTitle(
	List<Common.NpPlayerInfoObj.PlayerInfo_Title> _infoList
	, List<Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre> _comboPreList
	, List<Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx> _comboSfxList
	, List<Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg> _comboBgList
	, NPCommon.PlayerInfo_CurTitle _curInfo
	, bool _isShow
) {	infoList = _infoList;
	comboPreList = _comboPreList;
	comboSfxList = _comboSfxList;
	comboBgList = _comboBgList;
	curInfo = _curInfo;
	isShow = _isShow;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)20; }

/// <summary>
/// 已解锁的普通称号列表
/// </summary>
public List<Common.NpPlayerInfoObj.PlayerInfo_Title> getInfoList() { return infoList; }
/// <summary>
/// 已解锁的普通称号列表
/// </summary>
public void addInfoList(Common.NpPlayerInfoObj.PlayerInfo_Title _infoList) { infoList.Add(_infoList); }
/// <summary>
/// 已解锁的组合称号前缀列表
/// </summary>
public List<Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre> getComboPreList() { return comboPreList; }
/// <summary>
/// 已解锁的组合称号前缀列表
/// </summary>
public void addComboPreList(Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre _comboPreList) { comboPreList.Add(_comboPreList); }
/// <summary>
/// 已解锁的组合称号后缀列表
/// </summary>
public List<Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx> getComboSfxList() { return comboSfxList; }
/// <summary>
/// 已解锁的组合称号后缀列表
/// </summary>
public void addComboSfxList(Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx _comboSfxList) { comboSfxList.Add(_comboSfxList); }
/// <summary>
/// 已解锁的组合称号底色列表
/// </summary>
public List<Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg> getComboBgList() { return comboBgList; }
/// <summary>
/// 已解锁的组合称号底色列表
/// </summary>
public void addComboBgList(Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg _comboBgList) { comboBgList.Add(_comboBgList); }
/// <summary>
/// 玩家当前穿戴称号数据
/// </summary>
public NPCommon.PlayerInfo_CurTitle getCurInfo() { return curInfo; }
/// <summary>
/// 玩家当前穿戴称号数据
/// </summary>
public void setCurInfo(NPCommon.PlayerInfo_CurTitle _curInfo) { curInfo = _curInfo; }
/// <summary>
/// 是否展示称号
/// </summary>
public bool getIsShow() { return isShow; }
/// <summary>
/// 是否展示称号
/// </summary>
public void setIsShow(bool _isShow) { isShow = _isShow; }


public int GetBufSize() {
	int _size = 1;
	_size += 2 + (infoList.Count * 25);
	_size += 2 + (comboPreList.Count * 13);
	_size += 2 + (comboSfxList.Count * 13);
	_size += 2 + (comboBgList.Count * 13);
	_size += 4 + curInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 3;
	_size += 2 + (infoList.Count * 25);
	_size += 2 + (comboPreList.Count * 13);
	_size += 2 + (comboSfxList.Count * 13);
	_size += 2 + (comboBgList.Count * 13);
	_size += 4 + curInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _infoListCount = _buf.getShort();
	for(int _i = 0; _i < _infoListCount; _i++) { 
		Common.NpPlayerInfoObj.PlayerInfo_Title _infoList = new Common.NpPlayerInfoObj.PlayerInfo_Title();
		int __infoListCustLen = _buf.getInt();
	int __infoListCurPos = _buf.getCurPos();
	_infoList.ReadUnzipBuf(_buf, __infoListCurPos + __infoListCustLen);
	_buf.setPosition(__infoListCurPos + __infoListCustLen);

		infoList.Add(_infoList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _comboPreListCount = _buf.getShort();
	for(int _i = 0; _i < _comboPreListCount; _i++) { 
		Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre _comboPreList = new Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre();
		int __comboPreListCustLen = _buf.getInt();
	int __comboPreListCurPos = _buf.getCurPos();
	_comboPreList.ReadUnzipBuf(_buf, __comboPreListCurPos + __comboPreListCustLen);
	_buf.setPosition(__comboPreListCurPos + __comboPreListCustLen);

		comboPreList.Add(_comboPreList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _comboSfxListCount = _buf.getShort();
	for(int _i = 0; _i < _comboSfxListCount; _i++) { 
		Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx _comboSfxList = new Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx();
		int __comboSfxListCustLen = _buf.getInt();
	int __comboSfxListCurPos = _buf.getCurPos();
	_comboSfxList.ReadUnzipBuf(_buf, __comboSfxListCurPos + __comboSfxListCustLen);
	_buf.setPosition(__comboSfxListCurPos + __comboSfxListCustLen);

		comboSfxList.Add(_comboSfxList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _comboBgListCount = _buf.getShort();
	for(int _i = 0; _i < _comboBgListCount; _i++) { 
		Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg _comboBgList = new Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg();
		int __comboBgListCustLen = _buf.getInt();
	int __comboBgListCurPos = _buf.getCurPos();
	_comboBgList.ReadUnzipBuf(_buf, __comboBgListCurPos + __comboBgListCustLen);
	_buf.setPosition(__comboBgListCurPos + __comboBgListCustLen);

		comboBgList.Add(_comboBgList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _curInfoCustLen = _buf.getInt();
	int _curInfoCurPos = _buf.getCurPos();
	curInfo.ReadUnzipBuf(_buf, _curInfoCurPos + _curInfoCustLen);
	_buf.setPosition(_curInfoCurPos + _curInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isShow = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)infoList.Count);
	for(int _i = 0; _i < infoList.Count; _i++) { 
		_buf.putInt(infoList[_i].GetBufSize());
	infoList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)comboPreList.Count);
	for(int _i = 0; _i < comboPreList.Count; _i++) { 
		_buf.putInt(comboPreList[_i].GetBufSize());
	comboPreList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)comboSfxList.Count);
	for(int _i = 0; _i < comboSfxList.Count; _i++) { 
		_buf.putInt(comboSfxList[_i].GetBufSize());
	comboSfxList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)comboBgList.Count);
	for(int _i = 0; _i < comboBgList.Count; _i++) { 
		_buf.putInt(comboBgList[_i].GetBufSize());
	comboBgList[_i].PutUnzipBuf(_buf);
	}
	_buf.putInt(curInfo.GetBufSize());
	curInfo.PutUnzipBuf(_buf);
	_buf.put(isShow?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)20);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)20);
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
	builder.Append("infoList").Append(":").Append(infoList.ToString()).Append(", ");
	builder.Append("comboPreList").Append(":").Append(comboPreList.ToString()).Append(", ");
	builder.Append("comboSfxList").Append(":").Append(comboSfxList.ToString()).Append(", ");
	builder.Append("comboBgList").Append(":").Append(comboBgList.ToString()).Append(", ");
	builder.Append("curInfo").Append(":").Append(curInfo == null ? "null" : curInfo.ToString()).Append(", ");
	builder.Append("isShow").Append(":").Append(isShow.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

