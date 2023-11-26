using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class BaseBulletHole : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected IObjectPool<BaseBulletHole> bulletHolePooler;

    public void SetBulletHolePooler(IObjectPool<BaseBulletHole> pooler) { this.bulletHolePooler = pooler; }
    
}
