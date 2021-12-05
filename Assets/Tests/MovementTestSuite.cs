using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MovementTestSuite
{
    private Movement movement;

    [UnityTest]
    public IEnumerator DogKnightFall()//π∑ «∑Ò ‹÷ÿ¡¶◊π¬‰≤‚ ‘
    {
        GameObject dogKnight = MonoBehaviour.Instantiate(Resources.Load<GameObject>("DogPolyart"));
        movement = dogKnight.GetComponent<Movement>();
        float positionY = dogKnight.transform.position.y;
        //movement.Move("MoveForward");
        yield return new WaitForSeconds(1f);
        float newPositionY = dogKnight.transform.position.y;
        Assert.AreNotEqual(positionY, newPositionY);
        Object.Destroy(dogKnight);
    }

    [UnityTest]
    public IEnumerator DogKnightMoveForward()//π∑«∞Ω¯≤‚ ‘
    {
        GameObject dogKnight = MonoBehaviour.Instantiate(Resources.Load<GameObject>("DogPolyart"));
        movement = dogKnight.GetComponent<Movement>();
        dogKnight.transform.position = Vector3.zero;
        dogKnight.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        yield return new WaitForSeconds(1f);
        Vector3 initPos = Vector3.zero;
        float x = (initPos + dogKnight.transform.forward).x;
        ArrayList test = new ArrayList();
        test.Add("MoveForward");
        movement.GetCode(test);
        yield return new WaitForSeconds(2f);

        Vector3 endPos = movement.Destination;
        float y = endPos.x;
        Debug.Log(y);
        Assert.AreEqual(x, y);

        Object.Destroy(dogKnight);
    }

    [UnityTest]
    public IEnumerator DogKnightTurnLeft()//π∑◊Û◊™≤‚ ‘
    {
        GameObject dogKnight = MonoBehaviour.Instantiate(Resources.Load<GameObject>("DogPolyart"));
        movement = dogKnight.GetComponent<Movement>();
        dogKnight.transform.position = Vector3.zero;
        dogKnight.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        yield return new WaitForSeconds(1f);
        Vector3 initDir = movement.Direction;
        float y1 = initDir.y - 90f;
        Vector3 initPos = movement.Destination;

        ArrayList test = new ArrayList();
        test.Add("TurnLeft");
        movement.GetCode(test);
        yield return new WaitForSeconds(2f);

        Vector3 endPos = movement.Destination;
        Vector3 endDir = movement.Direction;
        float y2 = endDir.y;
        Assert.AreEqual(y1, y2);
        Assert.AreEqual(initPos, endPos);
        Object.Destroy(dogKnight);
        yield return null;
    }

    [UnityTest]
    public IEnumerator DogKnightTurnRight()//π∑”“◊™≤‚ ‘
    {
        GameObject dogKnight = MonoBehaviour.Instantiate(Resources.Load<GameObject>("DogPolyart"));
        movement = dogKnight.GetComponent<Movement>();
        dogKnight.transform.position = Vector3.zero;
        dogKnight.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        yield return new WaitForSeconds(1f);
        Vector3 initDir = movement.Direction;
        float y1 = initDir.y + 90f;
        Vector3 initPos = movement.Destination;

        ArrayList test = new ArrayList();
        test.Add("TurnRight");
        movement.GetCode(test);
        yield return new WaitForSeconds(2f);

        Vector3 endPos = movement.Destination;
        Vector3 endDir = movement.Direction;
        float y2 = endDir.y;
        Assert.AreEqual(y1, y2);
        Assert.AreEqual(initPos, endPos);
        Object.Destroy(dogKnight);
        yield return null;
    }

    [UnityTest]
    public IEnumerator DogKnightTurnAround()//π∑µÙÕ∑≤‚ ‘
    {
        GameObject dogKnight = MonoBehaviour.Instantiate(Resources.Load<GameObject>("DogPolyart"));
        movement = dogKnight.GetComponent<Movement>();
        dogKnight.transform.position = Vector3.zero;
        dogKnight.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        yield return new WaitForSeconds(1f);
        Vector3 initDir = movement.Direction;
        float y1 = initDir.y + 180f;
        Vector3 initPos = movement.Destination;

        ArrayList test = new ArrayList();
        test.Add("Turn");
        movement.GetCode(test);
        yield return new WaitForSeconds(2f);

        Vector3 endPos = movement.Destination;
        Vector3 endDir = movement.Direction;
        float y2 = System.Math.Abs(endDir.y);
        Assert.AreEqual(y1, y2);
        Assert.AreEqual(initPos, endPos);
        Object.Destroy(dogKnight);
        yield return null;
    }

    [UnityTest]
    public IEnumerator MultiMoveTest()//¡¨–¯“∆∂Ø√¸¡Ó≤‚ ‘
    {
        GameObject dogKnight = MonoBehaviour.Instantiate(Resources.Load<GameObject>("DogPolyart"));
        movement = dogKnight.GetComponent<Movement>();
        dogKnight.transform.position = Vector3.zero;
        dogKnight.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        yield return new WaitForSeconds(1f);
        Vector3 initPos = Vector3.zero;
        Vector3 initDir = movement.Direction;
        float y1 = initDir.y + 90f;

        ArrayList test = new ArrayList();
        test.Add("MoveForward");
        float x1 = (initPos + dogKnight.transform.forward).x;
        test.Add("MoveForward");
        x1 += dogKnight.transform.forward.x;
        test.Add("TurnLeft");
        test.Add("MoveForward");
        float z1 = 1;
        test.Add("TurnLeft");
        movement.GetCode(test);
        yield return new WaitForSeconds(12f);

        Vector3 endPos = movement.Destination;
        Vector3 endDir = movement.Direction;
        float y2 = System.Math.Abs(endDir.y);
        float x2 = endPos.x;
        float z2 = endPos.z;

        UnityEngine.Assertions.Assert.AreApproximatelyEqual(x1, x2);
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(z1, z2, 0.01f);
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(y1, y2, 0.05f);

        Object.Destroy(dogKnight);
    }
}
