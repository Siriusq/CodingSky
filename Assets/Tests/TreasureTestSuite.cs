using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TreasureTestSuite
{
    private LevelComplete levelComplete;
    private Movement movement;
    [UnityTest]
    public IEnumerator ChestCollider()
    {
        GameObject dogKnight = MonoBehaviour.Instantiate(Resources.Load<GameObject>("DogPolyart"));
        GameObject chest = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Golden_Chest"));
        levelComplete = chest.GetComponent<LevelComplete>();
        chest.transform.position = Vector3.zero;
        chest.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        dogKnight.transform.position = chest.transform.position;
        yield return new WaitForSeconds(0.1f);

        Assert.IsTrue(levelComplete.enter);
        Object.Destroy(dogKnight);
        Object.Destroy(chest);
    }

    [UnityTest]
    public IEnumerator OpenChest()
    {
        GameObject dogKnight = MonoBehaviour.Instantiate(Resources.Load<GameObject>("DogPolyart"));
        movement = dogKnight.GetComponent<Movement>();
        yield return new WaitForSeconds(1f);
        bool b1 = movement.canOPen;

        ArrayList test = new ArrayList();
        test.Add("Treasure");
        movement.GetCode(test);
        yield return new WaitForSeconds(1f);
        bool b2 = movement.canOPen;
        yield return new WaitForSeconds(1f);
        bool b3 = movement.canOPen;

        Assert.IsTrue(b2);
        Assert.IsFalse(b1);
        Assert.IsFalse(b3);
        Object.Destroy(dogKnight);
    }
}
