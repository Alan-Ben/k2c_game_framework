package Common;

import java.nio.ByteBuffer;
public class MCC_Common_User_Extra implements ALBasicProtocolPack._IALProtocolStructure {
private String name;
private long icon;
private long iconFrame;
private long lvl;
private long grade;
private int starhoner;
private long legendscore;


public MCC_Common_User_Extra() {
	name = "";
	icon = (long)0;
	iconFrame = (long)0;
	lvl = (long)0;
	grade = (long)0;
	starhoner = 0;
	legendscore = (long)0;
}

public MCC_Common_User_Extra(
	 String _name
	, long _icon
	, long _iconFrame
	, long _lvl
	, long _grade
	, int _starhoner
	, long _legendscore
) {	name = _name;
	icon = _icon;
	iconFrame = _iconFrame;
	lvl = _lvl;
	grade = _grade;
	starhoner = _starhoner;
	legendscore = _legendscore;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public String getName() { return name; }
public void setName(String _name) { name = _name; }
public long getIcon() { return icon; }
public void setIcon(long _icon) { icon = _icon; }
public long getIconFrame() { return iconFrame; }
public void setIconFrame(long _iconFrame) { iconFrame = _iconFrame; }
public long getLvl() { return lvl; }
public void setLvl(long _lvl) { lvl = _lvl; }
public long getGrade() { return grade; }
public void setGrade(long _grade) { grade = _grade; }
public int getStarhoner() { return starhoner; }
public void setStarhoner(int _starhoner) { starhoner = _starhoner; }
public long getLegendscore() { return legendscore; }
public void setLegendscore(long _legendscore) { legendscore = _legendscore; }


public final int GetBufSize() {
	int _size = 44;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 46;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) name = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) icon = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) iconFrame = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lvl = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) grade = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) starhoner = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) legendscore = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, name);
	_buf.putLong(icon);
	_buf.putLong(iconFrame);
	_buf.putLong(lvl);
	_buf.putLong(grade);
	_buf.putInt(starhoner);
	_buf.putLong(legendscore);
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

