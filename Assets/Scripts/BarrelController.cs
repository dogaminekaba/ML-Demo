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
        
    }

    public void RotateBarrelTo (Vector3 targetPoint)
    {
        transform.LookAt(targetPoint);
    }
}
