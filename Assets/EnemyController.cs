using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject bulletPref; 
    public Transform player;
    public float bulletSpeed = 20f;
    public float maxDistance = 20f;
    public float shootCooldown;

    private float shootTimer = 0f;
    private List<GameObject> bulletList;

    // Start is called before the first frame update
    void Start()
    {
        shootTimer = 0f;
        bulletList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if enough time has passed to shoot again
        if (shootTimer <= 0f)
        {
            Shoot(); // Call the shoot method
            shootTimer = shootCooldown; // Reset the timer
        }

        // Update the shoot timer
        shootTimer -= Time.deltaTime;
    }

    public void ResetPosition()
    {
        foreach(GameObject bullet in bulletList)
        {
            Destroy(bullet);
        }

        transform.localPosition = new Vector3(Random.value * 8 - 4, 0.25f, Random.value * 8 - 4);
    }

    void Shoot()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPref, transform.position, player.rotation);
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

        Destroy(bullet, maxDistance / bulletSpeed);
    }
}
