using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GemTestSuite
{
    private Movement movement;
    private CollectGem collectGem;

    [UnityTest]
    public IEnumerator GemCollider()//狗子进入和退出宝石
    {
        GameObject dogKnight = MonoBehaviour.Instantiate(Resources.Load<GameObject>("DogPolyart"));
        GameObject gems = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Cuboid"));
        collectGem = gems.GetComponent<CollectGem>();
        gems.transform.position = Vector3.zero;
        gems.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        dogKnight.transform.position = gems.transform.position;
        yield return new WaitForSeconds(0.1f);
        bool b1 = collectGem.gemsTest;
        Vector3 v = new Vector3(2f, 2f, 2f);
        dogKnight.transform.position = gems.transform.position + v;
        yield return new WaitForSeconds(0.1f);
        bool b2 = collectGem.gemsTest;

        Assert.IsTrue(b1);
        Assert.IsFalse(b2);
        Object.Destroy(dogKnight);
        Object.Destroy(gems);
    }

    [UnityTest]
    public IEnumerator CollectGems()
    {
        GameObject dogKnight = MonoBehaviour.Instantiate(Resources.Load<GameObject>("DogPolyart"));
        movement = dogKnight.GetComponent<Movement>();
        yield return new WaitForSeconds(1f);
        bool b1 = movement.canCollect;

        ArrayList test = new ArrayList();
        test.Add("Collect");
        movement.GetCode(test);
        yield return new WaitForSeconds(1f);
        bool b2 = movement.canCollect;
        yield return new WaitForSeconds(1f);
        bool b3 = movement.canCollect;

        Assert.IsTrue(b2);
        Assert.IsFalse(b1);
        Assert.IsFalse(b3);
        Object.Destroy(dogKnight);
    }
}
