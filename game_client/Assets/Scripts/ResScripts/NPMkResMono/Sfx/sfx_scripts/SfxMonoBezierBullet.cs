using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxMonoBezierBullet : MonoBehaviour
{
    public GameObject BulletObj;
    public float Speed = 20;
    public float DeltaTimeMuti = 1;
    public float BulletLifeTime = 10;
    public float HitDistance = 0.25f;
    public bool PlayBlastSfx;
    public GameObject BlastSfx;
    public float SfxLifeTime = 1;
    float timer = 0f;
    Vector3 startPoint, midPoint;
    Transform target;
    
    [Header("无视TimeScale")] 
    public bool ignoreTimeScale;

    private void OnEnable()
    {
        if (PlayBlastSfx)
        {
            if (BlastSfx != null) 
                BlastSfx.SetActive(false);
        }

        if (BulletObj != null) 
            BulletObj.SetActive(true);
        StartCoroutine(Move());
        timer = 0;
    }
    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            if (timer < BulletLifeTime)
            {
                timer += _getDeltaTime();
                if (target != null)
                {
                    if (Vector3.Distance(transform.position, target.position) <= HitDistance)
                    {
                        OnHit();
                    }
                }
            }
            else
            {
                OnHit();
            }
            
        }
    }
    private void OnHit()
    {
        StopAllCoroutines();
        if (BulletObj != null) 
            BulletObj.SetActive(false);
        if (PlayBlastSfx)
        {
            if(BlastSfx != null)
            {
                BlastSfx.SetActive(true);
                Invoke("blastEnd",SfxLifeTime);
            }
        }
    }
    public void Init(Vector3 sp,Vector3 mp,Transform tar)
    {
        startPoint = sp;
        midPoint = mp;
        target = tar;
    }
    public IEnumerator Move()
    {
        for (float i = 0; i <= 1; i += _getDeltaTime() * DeltaTimeMuti)
        {
            Vector3 p1 = Vector3.Lerp(startPoint, midPoint, i);
            if (target != null)
            {
                Vector3 p2 = Vector3.Lerp(midPoint, target.position, i);
                Vector3 p = Vector3.Lerp(p1, p2, i);
                //让子弹移动到P点
                yield return StartCoroutine(MoveToPoint(p));
            }
        }
        yield return StartCoroutine(MoveToTarget(target));
    }
    IEnumerator MoveToPoint(Vector3 p)
    {
        while (Vector3.Distance(transform.position, p) > 0.1f)
        {
            Vector3 dir = p-transform.position;
            transform.forward = dir;
            transform.position = Vector3.MoveTowards(transform.position, p, Speed * _getDeltaTime());
            yield return null;
        }
        yield return null;

    }
    IEnumerator MoveToTarget(Transform target)
    {
        while (target != null && Vector3.Distance(transform.position, target.position) > 0.1f)
        {
            Vector3 dir = target.position - transform.position;
            transform.forward = dir;
            transform.position = Vector3.MoveTowards(transform.position, target.position, Speed * _getDeltaTime());
            yield return null;
        }
        yield return null;

        
    }
    void blastEnd()
    {
        if (BlastSfx != null) 
            BlastSfx.SetActive(false);
        gameObject.SetActive(false);
    }

    //获取本帧时间
    private float _getDeltaTime()
    {
        if (ignoreTimeScale)
        {
            return Time.unscaledDeltaTime;
        }
        else
        {
            return Time.deltaTime;
        }
    }
}
