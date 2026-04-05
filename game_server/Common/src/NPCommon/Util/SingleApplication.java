package NPCommon.Util;


import NPCommon.Log.CommLog;

import java.io.File;
import java.io.RandomAccessFile;
import java.nio.channels.FileChannel;
import java.nio.channels.FileLock;

public class SingleApplication
{
    // 在应用程序的main方法里调用此函数保证程序只有一个实例在运行.
    public static void makeSingle(String _singleId)
    {

        RandomAccessFile raf = null;

        FileChannel channel = null;

        FileLock lock = null;

        String filePathName = "./" + _singleId + ".single";
        try
        {

            // 在临时文件夹创建一个临时文件，锁住这个文件用来保证应用程序只有一个实例被创建.
            File sf = new File(filePathName);
            sf.deleteOnExit();
            if (!sf.exists())
            {
                sf.createNewFile();
            }
            raf = new RandomAccessFile(sf, "rw");

            channel = raf.getChannel();

            lock = channel.tryLock();
            while (lock == null)
            {
                CommLog.info("lock file:{} failed,try in next second.", filePathName);
                Thread.sleep(3000);

                lock = channel.tryLock();
            }

        } catch (Exception e)
        {
            CommLog.error("lock file:{} error", filePathName, e);
            System.exit(0);

        }
    }
}
