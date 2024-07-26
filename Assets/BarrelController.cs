using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BarrelController : MonoBehaviour
{
    public GameObject zombie;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 targetPostition = new Vector3(zombie.transform.position.x, 
        //                                      transform.position.y, 
        //                                      zombie.transform.position.z);

        //transform.LookAt(targetPostition);

        //Debug.DrawLine(targetPostition, transform.position, Color.red);
    }

    public void RotateBarrelTo (Vector3 targetPoint)
    {
        transform.LookAt(targetPoint);
    }
}
