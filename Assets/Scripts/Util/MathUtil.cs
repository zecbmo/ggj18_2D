using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DestroyOptions {DontDestory, Self, Parent};

public static class MathUtil
{

    //Coroutine that will move an object to a location at a given speed
    //paramas: deadzone: if the object is within this range the lerp will end
    //paramas: isLocalTransform: if it is true, will use the local position otherwise will use the global position
    public static IEnumerator MoveObjectTowardsLocation(GameObject objectToMove, Vector3 endLocation, float speed, float deadZone = 0.5f, bool isLocalTransform = false, DestroyOptions destroyObjectWhenDone = DestroyOptions.DontDestory)
    {
        if (isLocalTransform)
        {
            while ((Vector2.Distance(objectToMove.transform.localPosition, endLocation) > deadZone))
            {
                objectToMove.transform.localPosition = Vector2.MoveTowards(objectToMove.transform.localPosition, endLocation, Time.deltaTime * speed);
                yield return new WaitForEndOfFrame();
            }

            objectToMove.transform.localPosition = endLocation;

            switch (destroyObjectWhenDone)
            {
                case DestroyOptions.DontDestory:
                    break;
                case DestroyOptions.Self:
                    GameObject.Destroy(objectToMove);
                    break;
                case DestroyOptions.Parent:
                    GameObject.Destroy(objectToMove.transform.parent.gameObject);
                    break;
                default:
                    break;
            }



        }
        else
        {
            while ((Vector2.Distance(objectToMove.transform.position, endLocation) > deadZone))
            {
                objectToMove.transform.position = Vector2.MoveTowards(objectToMove.transform.position, endLocation, Time.deltaTime * speed);
                yield return new WaitForEndOfFrame();
            }

            objectToMove.transform.position = endLocation;


            switch (destroyObjectWhenDone)
            {
                case DestroyOptions.DontDestory:
                    break;
                case DestroyOptions.Self:
                    GameObject.Destroy(objectToMove);
                    break;
                case DestroyOptions.Parent:
                    GameObject.Destroy(objectToMove.transform.parent.gameObject);
                    break;
                default:
                    break;
            }
        }
    }

    public static IEnumerator ScaleLerp(GameObject objectToLerp, Vector3 newScale, float speed)
    {
        float elapsedTime = 0;
        Vector3 startingScale = objectToLerp.transform.localScale;
        while (elapsedTime < 1)
        {
            objectToLerp.transform.localScale = Vector3.Lerp(startingScale, newScale, (elapsedTime / 1));
            elapsedTime += Time.deltaTime * speed;
            yield return new WaitForEndOfFrame();
        }
    }
}
