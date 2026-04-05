package Common.NpServerObj;

import java.nio.ByteBuffer;
/*********
 * 比武擂台玩家战斗记录数据
 **/
public class NpServerObj_Arena_FighterInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 使用的宠物皮肤列表 */
private java.util.ArrayList<NPCommon.NPCommon_PetInfo> petInfoList;
/** 战斗时使用的avatar数据 */
private java.util.ArrayList<Long> avatarIdList;
/** 玩家预制形象ID */
private long prefab;
/** 战斗数据 */
private NPCommon.NPCommon_PlayerFightInfo fightInfo;


public NpServerObj_Arena_FighterInfo() {
	petInfoList = new java.util.ArrayList<NPCommon.NPCommon_PetInfo>();
	avatarIdList = new java.util.ArrayList<Long>();
	prefab = (long)0;
	fightInfo = new NPCommon.NPCommon_PlayerFightInfo();
}

public NpServerObj_Arena_FighterInfo(
	 java.util.ArrayList<NPCommon.NPCommon_PetInfo> _petInfoList
	, java.util.ArrayList<Long> _avatarIdList
	, long _prefab
	, NPCommon.NPCommon_PlayerFightInfo _fightInfo
) {	petInfoList = _petInfoList;
	avatarIdList = _avatarIdList;
	prefab = _prefab;
	fightInfo = _fightInfo;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 使用的宠物皮肤列表 */
public java.util.ArrayList<NPCommon.NPCommon_PetInfo> getPetInfoList() { return petInfoList; }
/** 使用的宠物皮肤列表 */
public void addPetInfoList(NPCommon.NPCommon_PetInfo _petInfoList) { petInfoList.add(_petInfoList); }
/** 战斗时使用的avatar数据 */
public java.util.ArrayList<Long> getAvatarIdList() { return avatarIdList; }
/** 战斗时使用的avatar数据 */
public void addAvatarIdList(long _avatarIdList) { avatarIdList.add(_avatarIdList); }
/** 玩家预制形象ID */
public long getPrefab() { return prefab; }
/** 玩家预制形象ID */
public void setPrefab(long _prefab) { prefab = _prefab; }
/** 战斗数据 */
public NPCommon.NPCommon_PlayerFightInfo getFightInfo() { return fightInfo; }
/** 战斗数据 */
public void setFightInfo(NPCommon.NPCommon_PlayerFightInfo _fightInfo) { fightInfo = _fightInfo; }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (petInfoList.size() * 24);
	_size += 2 + (avatarIdList.size() * 8);
	_size += 4 + fightInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (petInfoList.size() * 24);
	_size += 2 + (avatarIdList.size() * 8);
	_size += 4 + fightInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _petInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _petInfoListCount; _i++) { 
		NPCommon.NPCommon_PetInfo _petInfoList = new NPCommon.NPCommon_PetInfo();
		if(_buf.remaining() <= 0) return;
	int __petInfoListCustLen = _buf.getInt();
	int __petInfoListCurPos = _buf.position();
	_petInfoList.ReadUnzipBuf(_buf, __petInfoListCurPos + __petInfoListCustLen);
	_buf.position(__petInfoListCurPos + __petInfoListCustLen);

		petInfoList.add(_petInfoList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _avatarIdListCount = _buf.getShort();
	for(int _i = 0; _i < _avatarIdListCount; _i++) { 
		long _avatarIdList = (long)0;
		if(_buf.remaining() > 0) _avatarIdList = _buf.getLong();
		avatarIdList.add(_avatarIdList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) prefab = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _fightInfoCustLen = _buf.getInt();
	int _fightInfoCurPos = _buf.position();
	fightInfo.ReadUnzipBuf(_buf, _fightInfoCurPos + _fightInfoCustLen);
	_buf.position(_fightInfoCurPos + _fightInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)petInfoList.size());
	for(int _i = 0; _i < petInfoList.size(); _i++) { 
		_buf.putInt(petInfoList.get(_i).GetBufSize());
	petInfoList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)avatarIdList.size());
	for(int _i = 0; _i < avatarIdList.size(); _i++) { 
		_buf.putLong(avatarIdList.get(_i));
	}
	_buf.putLong(prefab);
	_buf.putInt(fightInfo.GetBufSize());
	fightInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

