package GS2GC.p013_HeroOp;

import java.nio.ByteBuffer;
public class GS2GC_013_027_RetEquipSkillList implements ALBasicProtocolPack._IALProtocolStructure {
/** 技能列表 */
private java.util.ArrayList<Common.HeroObj.Equip_SkillInfo> skillList;


public GS2GC_013_027_RetEquipSkillList() {
	skillList = new java.util.ArrayList<Common.HeroObj.Equip_SkillInfo>();
}

public GS2GC_013_027_RetEquipSkillList(
	 java.util.ArrayList<Common.HeroObj.Equip_SkillInfo> _skillList
) {	skillList = _skillList;
}

public final byte getMainOrder() { return (byte)13; }

public final byte getSubOrder() { return (byte)27; }

/** 技能列表 */
public java.util.ArrayList<Common.HeroObj.Equip_SkillInfo> getSkillList() { return skillList; }
/** 技能列表 */
public void addSkillList(Common.HeroObj.Equip_SkillInfo _skillList) { skillList.add(_skillList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (skillList.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (skillList.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _skillListCount = _buf.getShort();
	for(int _i = 0; _i < _skillListCount; _i++) { 
		Common.HeroObj.Equip_SkillInfo _skillList = new Common.HeroObj.Equip_SkillInfo();
		if(_buf.remaining() <= 0) return;
	int __skillListCustLen = _buf.getInt();
	int __skillListCurPos = _buf.position();
	_skillList.ReadUnzipBuf(_buf, __skillListCurPos + __skillListCustLen);
	_buf.position(__skillListCurPos + __skillListCustLen);

		skillList.add(_skillList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)skillList.size());
	for(int _i = 0; _i < skillList.size(); _i++) { 
		_buf.putInt(skillList.get(_i).GetBufSize());
	skillList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)27);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)27);
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

