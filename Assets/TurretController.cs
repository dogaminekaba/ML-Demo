using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public GameObject bulletPref; 
    public GameObject zombie;
    public float bulletSpeed;
    public float maxDistance;
    public float shootCooldown;

    private float shootTimer = 0f;
    private List<GameObject> bulletList;
    private BarrelController barrelController;

    // Start is called before the first frame update
    void Start()
    {
        shootTimer = 0f;
        bulletList = new List<GameObject>();
        barrelController = gameObject.GetComponentInChildren<BarrelController>();
    }

    // Update is called once per frame
    void Update()
    {
        // get mouse position and move barrel


        //barrelController.RotateBarrelTo(zombieT);

        // check if enough time has passed to shoot again
        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootCooldown;

            //if (PlayerShoot())
            //{
            //    shootTimer = shootCooldown; // reset the timer
            //}

        }

        shootTimer -= Time.deltaTime;
    }

    public void ResetPosition()
    {
        foreach(GameObject bullet in bulletList)
        {
            Destroy(bullet);
        }

        transform.localPosition = new Vector3(Random.value * maxDistance - maxDistance/2, transform.position.y, Random.value * maxDistance - maxDistance / 2);
    }

    // Auto-shoot
    void Shoot()
    {
        Transform zombieT = zombie.transform;
        Vector3 direction = (zombieT.position - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPref, transform.position, zombieT.rotation);
        bulletList.Add(bullet);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = direction * bulletSpeed;
        }
        else
        {
            Debug.LogError("Bullet prefab does not have a Rigidbody component!");
        }

    }

    bool PlayerShoot()
    {
        RaycastHit _hit;
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Transform zombieT = zombie.transform;
        Vector3 direction = (zombieT.position - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPref, transform.position, zombieT.rotation);
        bulletList.Add(bullet);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = direction * bulletSpeed;
        }
        else
        {
            Debug.LogError("Bullet prefab does not have a Rigidbody component!");
        }

        return false;
    }
}
