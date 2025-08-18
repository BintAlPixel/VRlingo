using UnityEngine;

public class RopeLine : MonoBehaviour
{
    public LineRenderer line;
    public Transform pinata; // اسحبي هنا البنياتا

    void Update()
    {
        if (line != null && pinata != null)
        {
            line.SetPosition(0, transform.position);      // طرف الحبل من الـPivot
            line.SetPosition(1, pinata.position);         // طرف الحبل من البنياتا
        }
    }
}