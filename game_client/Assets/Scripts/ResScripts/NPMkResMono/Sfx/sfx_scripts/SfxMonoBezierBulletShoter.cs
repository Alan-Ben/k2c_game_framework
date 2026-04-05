using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxMonoBezierBulletShoter : MonoBehaviour
{
    public Transform Target;
    public float GlobalDelay;
    public List<ShotData> ShotDatas = new List<ShotData>();
   
    [System.Serializable]public struct ShotData
    {
        public Vector3 ShotPointOffset;
        public float ShotDelay;
        public List<SfxMonoBezierBullet> BulletList;
        public float Interval;
        public bool Random;
        public float Radius;
        public bool RangeOffset;
        public Vector3 MidPointOffset_1;
        public Vector3 MidPointOffset_2;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        if (ShotDatas != null)
        {
            foreach (ShotData shotData in ShotDatas)
            {
                foreach (SfxMonoBezierBullet bullet in shotData.BulletList)
                {
                    if (bullet != null) 
                        bullet.gameObject.SetActive(false);
                }
            }
        }

        StartCoroutine(StartShot());
    }
    private void Update()
    {
        if(null == ShotDatas)
            return;
        
        foreach (ShotData shotData in ShotDatas)
        {
            foreach (SfxMonoBezierBullet bullet in shotData.BulletList)
            {
                if(null == bullet)
                    continue;
                
                bullet.transform.localScale = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y, 1 / transform.localScale.z);
            }
        }
    }
    public Vector3 GetRandomPoint(Vector3 shotPos,float r)
    {
        return shotPos+ new Vector3(Random.Range(-r,r),Random.Range(-r,r),Random.Range(-r,r));
    }

    IEnumerator StartShot()
    {
        if (Target!=null)
        {
            if (null != ShotDatas && ShotDatas.Count > 0)
            {
                yield return new WaitForSeconds(GlobalDelay);
                foreach (ShotData shotData in ShotDatas)
                {
                    StartCoroutine(DoShot(shotData));
                    yield return new WaitForEndOfFrame();
                }
            }
            yield return null;
        }
        else
        {
            Debug.LogError("没有设置子弹预制体或目标！");
            yield return null;
        }
    }
    
    IEnumerator DoShot(ShotData sd)
    {
        yield return new WaitForSeconds(sd.ShotDelay);
        Vector3 sp = transform.position + transform.right * sd.ShotPointOffset.x + transform.up * sd.ShotPointOffset.y + transform.forward * sd.ShotPointOffset.z;
        for(int i = 0; i < sd.BulletList.Count; i++)
        {
            SfxMonoBezierBullet bullet = sd.BulletList[i];
            bullet.transform.position = sp;
            bullet.transform.rotation = Quaternion.identity;
            
            Vector3 midPoint;
            if (sd.RangeOffset)
            {
                Vector3 randomOffset = new Vector3(Random.Range(sd.MidPointOffset_1.x, sd.MidPointOffset_2.x), Random.Range(sd.MidPointOffset_1.y, sd.MidPointOffset_2.y), Random.Range(sd.MidPointOffset_1.z, sd.MidPointOffset_2.z));
                midPoint = sp + transform.right * randomOffset.x + transform.up * randomOffset.y + transform.forward * randomOffset.z;
            }
            else
            {
                midPoint = sp + transform.right * sd.MidPointOffset_1.x + transform.up * sd.MidPointOffset_1.y + transform.forward * sd.MidPointOffset_1.z;

            }
            if (sd.Random)
            {
                midPoint = GetRandomPoint(sp, sd.Radius);
            }
            bullet.Init(sp, midPoint, Target);
            bullet.gameObject.SetActive(true);
            yield return new WaitForSeconds(sd.Interval);
        }
        yield return null;
    }
    
}
