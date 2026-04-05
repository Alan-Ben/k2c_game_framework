package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
/*********
 * 称号初始化数据
 **/
public class GS2GC_002_020_RetPlayerTitle implements ALBasicProtocolPack._IALProtocolStructure {
/** 已解锁的普通称号列表 */
private java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_Title> infoList;
/** 已解锁的组合称号前缀列表 */
private java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre> comboPreList;
/** 已解锁的组合称号后缀列表 */
private java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx> comboSfxList;
/** 已解锁的组合称号底色列表 */
private java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg> comboBgList;
/** 玩家当前穿戴称号数据 */
private NPCommon.PlayerInfo_CurTitle curInfo;
/** 是否展示称号 */
private boolean isShow;


public GS2GC_002_020_RetPlayerTitle() {
	infoList = new java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_Title>();
	comboPreList = new java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre>();
	comboSfxList = new java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx>();
	comboBgList = new java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg>();
	curInfo = new NPCommon.PlayerInfo_CurTitle();
	isShow = false;
}

public GS2GC_002_020_RetPlayerTitle(
	 java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_Title> _infoList
	, java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre> _comboPreList
	, java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx> _comboSfxList
	, java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg> _comboBgList
	, NPCommon.PlayerInfo_CurTitle _curInfo
	, boolean _isShow
) {	infoList = _infoList;
	comboPreList = _comboPreList;
	comboSfxList = _comboSfxList;
	comboBgList = _comboBgList;
	curInfo = _curInfo;
	isShow = _isShow;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)20; }

/** 已解锁的普通称号列表 */
public java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_Title> getInfoList() { return infoList; }
/** 已解锁的普通称号列表 */
public void addInfoList(Common.NpPlayerInfoObj.PlayerInfo_Title _infoList) { infoList.add(_infoList); }
/** 已解锁的组合称号前缀列表 */
public java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre> getComboPreList() { return comboPreList; }
/** 已解锁的组合称号前缀列表 */
public void addComboPreList(Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre _comboPreList) { comboPreList.add(_comboPreList); }
/** 已解锁的组合称号后缀列表 */
public java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx> getComboSfxList() { return comboSfxList; }
/** 已解锁的组合称号后缀列表 */
public void addComboSfxList(Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx _comboSfxList) { comboSfxList.add(_comboSfxList); }
/** 已解锁的组合称号底色列表 */
public java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg> getComboBgList() { return comboBgList; }
/** 已解锁的组合称号底色列表 */
public void addComboBgList(Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg _comboBgList) { comboBgList.add(_comboBgList); }
/** 玩家当前穿戴称号数据 */
public NPCommon.PlayerInfo_CurTitle getCurInfo() { return curInfo; }
/** 玩家当前穿戴称号数据 */
public void setCurInfo(NPCommon.PlayerInfo_CurTitle _curInfo) { curInfo = _curInfo; }
/** 是否展示称号 */
public boolean getIsShow() { return isShow; }
/** 是否展示称号 */
public void setIsShow(boolean _isShow) { isShow = _isShow; }


public final int GetBufSize() {
	int _size = 1;
	_size += 2 + (infoList.size() * 25);
	_size += 2 + (comboPreList.size() * 13);
	_size += 2 + (comboSfxList.size() * 13);
	_size += 2 + (comboBgList.size() * 13);
	_size += 4 + curInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 3;
	_size += 2 + (infoList.size() * 25);
	_size += 2 + (comboPreList.size() * 13);
	_size += 2 + (comboSfxList.size() * 13);
	_size += 2 + (comboBgList.size() * 13);
	_size += 4 + curInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _infoListCount = _buf.getShort();
	for(int _i = 0; _i < _infoListCount; _i++) { 
		Common.NpPlayerInfoObj.PlayerInfo_Title _infoList = new Common.NpPlayerInfoObj.PlayerInfo_Title();
		if(_buf.remaining() <= 0) return;
	int __infoListCustLen = _buf.getInt();
	int __infoListCurPos = _buf.position();
	_infoList.ReadUnzipBuf(_buf, __infoListCurPos + __infoListCustLen);
	_buf.position(__infoListCurPos + __infoListCustLen);

		infoList.add(_infoList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _comboPreListCount = _buf.getShort();
	for(int _i = 0; _i < _comboPreListCount; _i++) { 
		Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre _comboPreList = new Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre();
		if(_buf.remaining() <= 0) return;
	int __comboPreListCustLen = _buf.getInt();
	int __comboPreListCurPos = _buf.position();
	_comboPreList.ReadUnzipBuf(_buf, __comboPreListCurPos + __comboPreListCustLen);
	_buf.position(__comboPreListCurPos + __comboPreListCustLen);

		comboPreList.add(_comboPreList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _comboSfxListCount = _buf.getShort();
	for(int _i = 0; _i < _comboSfxListCount; _i++) { 
		Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx _comboSfxList = new Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx();
		if(_buf.remaining() <= 0) return;
	int __comboSfxListCustLen = _buf.getInt();
	int __comboSfxListCurPos = _buf.position();
	_comboSfxList.ReadUnzipBuf(_buf, __comboSfxListCurPos + __comboSfxListCustLen);
	_buf.position(__comboSfxListCurPos + __comboSfxListCustLen);

		comboSfxList.add(_comboSfxList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _comboBgListCount = _buf.getShort();
	for(int _i = 0; _i < _comboBgListCount; _i++) { 
		Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg _comboBgList = new Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg();
		if(_buf.remaining() <= 0) return;
	int __comboBgListCustLen = _buf.getInt();
	int __comboBgListCurPos = _buf.position();
	_comboBgList.ReadUnzipBuf(_buf, __comboBgListCurPos + __comboBgListCustLen);
	_buf.position(__comboBgListCurPos + __comboBgListCustLen);

		comboBgList.add(_comboBgList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _curInfoCustLen = _buf.getInt();
	int _curInfoCurPos = _buf.position();
	curInfo.ReadUnzipBuf(_buf, _curInfoCurPos + _curInfoCustLen);
	_buf.position(_curInfoCurPos + _curInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isShow = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)infoList.size());
	for(int _i = 0; _i < infoList.size(); _i++) { 
		_buf.putInt(infoList.get(_i).GetBufSize());
	infoList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)comboPreList.size());
	for(int _i = 0; _i < comboPreList.size(); _i++) { 
		_buf.putInt(comboPreList.get(_i).GetBufSize());
	comboPreList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)comboSfxList.size());
	for(int _i = 0; _i < comboSfxList.size(); _i++) { 
		_buf.putInt(comboSfxList.get(_i).GetBufSize());
	comboSfxList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)comboBgList.size());
	for(int _i = 0; _i < comboBgList.size(); _i++) { 
		_buf.putInt(comboBgList.get(_i).GetBufSize());
	comboBgList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt(curInfo.GetBufSize());
	curInfo.PutUnzipBuf(_buf);
	_buf.put(isShow?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)20);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)20);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

