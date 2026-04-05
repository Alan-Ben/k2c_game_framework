package Common;

import java.nio.ByteBuffer;
public class Common_FriendBaseInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private String name;
private long icon;
private int grade;
private int starHonor;
private int legendScore;
private int addTime;
private int level;
private long iconBgk;


public Common_FriendBaseInfo() {
	uid = (long)0;
	name = "";
	icon = (long)0;
	grade = 0;
	starHonor = 0;
	legendScore = 0;
	addTime = 0;
	level = 0;
	iconBgk = (long)0;
}

public Common_FriendBaseInfo(
	 long _uid
	, String _name
	, long _icon
	, int _grade
	, int _starHonor
	, int _legendScore
	, int _addTime
	, int _level
	, long _iconBgk
) {	uid = _uid;
	name = _name;
	icon = _icon;
	grade = _grade;
	starHonor = _starHonor;
	legendScore = _legendScore;
	addTime = _addTime;
	level = _level;
	iconBgk = _iconBgk;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public String getName() { return name; }
public void setName(String _name) { name = _name; }
public long getIcon() { return icon; }
public void setIcon(long _icon) { icon = _icon; }
public int getGrade() { return grade; }
public void setGrade(int _grade) { grade = _grade; }
public int getStarHonor() { return starHonor; }
public void setStarHonor(int _starHonor) { starHonor = _starHonor; }
public int getLegendScore() { return legendScore; }
public void setLegendScore(int _legendScore) { legendScore = _legendScore; }
public int getAddTime() { return addTime; }
public void setAddTime(int _addTime) { addTime = _addTime; }
public int getLevel() { return level; }
public void setLevel(int _level) { level = _level; }
public long getIconBgk() { return iconBgk; }
public void setIconBgk(long _iconBgk) { iconBgk = _iconBgk; }


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
	if(_buf.remaining() > 0) uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) name = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) icon = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) grade = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) starHonor = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) legendScore = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) addTime = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) level = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) iconBgk = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(uid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, name);
	_buf.putLong(icon);
	_buf.putInt(grade);
	_buf.putInt(starHonor);
	_buf.putInt(legendScore);
	_buf.putInt(addTime);
	_buf.putInt(level);
	_buf.putLong(iconBgk);
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

