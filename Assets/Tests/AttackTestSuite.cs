using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AttackTestSuite
{
    private Attack attack;
    private Movement movement;
    [UnityTest]
    public IEnumerator AttackCountTest()
    {
        GameObject slime = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Slime"));
        attack = slime.GetComponent<Attack>();
        Attack.attackCount++;
        GetMethod("AttackWait");
        yield return null;
        Assert.AreEqual(Attack.attackCount, 1);
        Object.Destroy(slime);
    }

    [UnityTest]
    public IEnumerator IsSlime()
    {
        GameObject slime = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Slime"));
        attack = slime.GetComponent<Attack>();
        GetMethod("Wait");
        yield return null;
        Assert.IsFalse(Attack.isSlime);
        Object.Destroy(slime);
    }

    [UnityTest]
    public IEnumerator CanAttack()
    {
        GameObject dogKnight = MonoBehaviour.Instantiate(Resources.Load<GameObject>("DogPolyart"));
        movement = dogKnight.GetComponent<Movement>();
        yield return new WaitForSeconds(1f);
        bool b1 = movement.canAttack;

        ArrayList test = new ArrayList();
        test.Add("Attack");
        movement.GetCode(test);
        yield return new WaitForSeconds(1f);
        bool b2 = movement.canAttack;
        yield return new WaitForSeconds(1f);
        bool b3 = movement.canAttack;

        Assert.IsTrue(b2);
        Assert.IsFalse(b1);
        Assert.IsFalse(b3);
        Object.Destroy(dogKnight);
    }

    [UnityTest]
    public IEnumerator AttackCollider()
    {
        GameObject dogKnight = MonoBehaviour.Instantiate(Resources.Load<GameObject>("DogPolyart"));
        GameObject slime = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Slime"));
        attack = slime.GetComponent<Attack>();
        dogKnight.transform.position = Vector3.zero;
        dogKnight.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        dogKnight.transform.position = slime.transform.position;
        yield return new WaitForSeconds(0.1f);
        bool b1 = attack.attackTest;
        bool b2 = true;

        Vector3 v = new Vector3(2f, 2f, 2f);
        dogKnight.transform.position = slime.transform.position + v;
        yield return new WaitForSeconds(0.1f);
        b2 = attack.attackTest;

        Assert.IsFalse(b1);
        Assert.IsFalse(b2);

        Object.Destroy(dogKnight);
        Object.Destroy(slime);
    }


    private MethodInfo GetMethod(string methodName)
    {
        if (string.IsNullOrWhiteSpace(methodName))
            Assert.Fail("methodName cannot be null or whitespace");

        var method = this.attack.GetType()
            .GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

        if (method == null)
            Assert.Fail(string.Format("{0} method not found", methodName));

        return method;
    }
}
