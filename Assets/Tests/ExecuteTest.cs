using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class ExecuteTest
{
    private Movement movement;
    private HighLightButton highLightButton;
    
    [UnityTest]
    public IEnumerator Execute()
    {
        GameObject dogKnight = MonoBehaviour.Instantiate(Resources.Load<GameObject>("DogPolyart"));
        movement = dogKnight.GetComponent<Movement>();
        ArrayList test = new ArrayList();
        test.Add("MoveForward");
        movement.GetCode(test);
        Assert.IsNotNull(movement.Received);
        yield return null;
        Object.Destroy(dogKnight);
    }

    [UnityTest]
    public IEnumerator MoveCommandTest()//测试移动数组是否传递
    {
        GameObject dogKnight = MonoBehaviour.Instantiate(Resources.Load<GameObject>("DogPolyart"));
        movement = dogKnight.GetComponent<Movement>();
        yield return new WaitForSeconds(1f);
        ArrayList test = new ArrayList();
        test.Add("MoveForward");
        movement.GetCode(test);
        yield return new WaitForSeconds(3f);
        Assert.IsTrue(movement.testMove);
        
        Object.Destroy(dogKnight);
    }

    [UnityTest]
    public IEnumerator HighLightTest()//测试高亮数组是否传递
    {
        yield return new WaitForSeconds(1f);
        ArrayList test = new ArrayList();
        test.Add("MoveForward");
        HighLightButton.GetCode(test);
        yield return new WaitForSeconds(3f);
        Assert.IsTrue(HighLightButton.testHighlight);
    }


}
