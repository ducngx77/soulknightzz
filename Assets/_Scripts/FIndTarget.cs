using UnityEngine;

public class FindTarget : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameObject FindSingleTarget(string tag)
    {
        GameObject target = GameObject.FindGameObjectWithTag(tag);
        return target;
    }

    public static GameObject[] FindAllTargetInrange(string tag, Transform transform)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);

        return targets;
    }

    public static GameObject FindNearestTargetInRange(string tag, Transform transform, float range)
    {
        if (transform == null)
        {
            Debug.LogError("Transform parameter is null. Make sure to provide a valid Transform.");
            return null;
        }

        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);


        GameObject nearestTarget = null;
        float nearestDistanceSqr = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            float distanceToTarget = Vector2.Distance(target.transform.position, transform.position);
            if (distanceToTarget < nearestDistanceSqr && distanceToTarget <= range)
            {
                if (target.GetComponent<Animator>() != null)
                {
                    if (target.GetComponent<Animator>().GetBool("isDead").Equals(false))
                    {
                        nearestDistanceSqr = distanceToTarget;
                        nearestTarget = target;
                    }
                }
                else
                {
                    nearestDistanceSqr = distanceToTarget;
                    nearestTarget = target;
                }
            }
        }  
        if (tag == "Enemy") {
            GameObject[] targets2 = GameObject.FindGameObjectsWithTag("Boss");
            foreach (GameObject target in targets2)
            {
                float distanceToTarget = Vector2.Distance(target.transform.position, transform.position);
                if (distanceToTarget < nearestDistanceSqr && distanceToTarget <= range)
                {
                    if (target.GetComponent<Animator>() != null)
                    {
                        if (target.GetComponent<Animator>().GetBool("isDead").Equals(false))
                        {
                            nearestDistanceSqr = distanceToTarget;
                            nearestTarget = target;
                        }
                    }
                    else
                    {
                        nearestDistanceSqr = distanceToTarget;
                        nearestTarget = target;
                    }
                }
            }
        }

        return nearestTarget;
    }
}
