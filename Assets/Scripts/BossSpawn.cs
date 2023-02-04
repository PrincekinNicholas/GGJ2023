using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    [SerializeField] private GameObject BreakableTreeBranch;
    [SerializeField] private GameObject WeakBossObj;
    [SerializeField] private GameObject BossObj;
    public void SwitchBoss()
    {
        BreakableRoot breakableRoot = BreakableTreeBranch.GetComponent<BreakableRoot>();
        bool isWeak = breakableRoot.GetAlreadyBorkenValue();
        print(breakableRoot.name);
        print(isWeak);

        if( isWeak )
        {
            GameObject boss = (GameObject)Resources.Load("Monster/Boss_Weak");
            Instantiate(boss);
            boss.transform.position = new Vector3((float)25.2, (float)-1.1, 0);
            boss.gameObject.transform.localScale = new Vector3((float)5.3, (float)5.3, 1);
        }
        else
        {
            GameObject boss = (GameObject)Resources.Load("Monster/Boss");
            Instantiate(boss);
            boss.transform.position = new Vector3((float)25.1, (float)-0.42, 0);
            boss.gameObject.transform.localScale = new Vector3((float)10, (float)10, 1);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Hello World");
        SwitchBoss();
    }
}
