using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
	public GameObject bulletPref;
	public GameObject zombie;
	public float bulletSpeed;
	public float maxDistance;
	public float shootCooldown = 0.5f;
	public GameController gameManager;

	private float shootTimer = 0f;
	private List<GameObject> bulletList;
	private BarrelController barrelController;
	private bool onCooldown = false;

	// Start is called before the first frame update
	void Start()
	{
		shootTimer = shootCooldown;
		bulletList = new List<GameObject>();
		barrelController = gameObject.GetComponentInChildren<BarrelController>();
	}

	// Update is called once per frame
	void Update()
	{
		// check if enough time has passed to shoot again
		if (shootTimer <= 0f)
		{
			onCooldown = false;
			shootTimer = shootCooldown; // reset the timer
		}

		if (onCooldown)
		{
			shootTimer -= Time.deltaTime;
		}

		if (gameManager.IsSimulating())
		{
			TryShoot();
		}
		else
		{
			CheckPlayerShoot();
		}

		gameManager.UpdateCooldownUI(shootTimer * 2);
	}

	public void ResetPosition()
	{
		foreach (GameObject bullet in bulletList)
		{
			Destroy(bullet);
		}

		transform.localPosition = new Vector3(Random.value * maxDistance - maxDistance / 2, transform.position.y, Random.value * maxDistance - maxDistance / 2);
	}

	// Auto-shoot
	void TryShoot()
	{
		// update barrel rotation
		Vector3 targetPosition = new Vector3(	zombie.transform.position.x,
												barrelController.transform.position.y,
												zombie.transform.position.z);
		barrelController.RotateBarrelTo(targetPosition);

		// shoot
		if (!onCooldown)
		{
			Transform zombieT = zombie.transform;
			Vector3 direction = (zombieT.position - transform.position).normalized;

			GameObject bullet = Instantiate(bulletPref, transform.position, zombieT.rotation);
			bulletList.Add(bullet);
			Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

			if (bulletRigidbody != null)
			{
				bulletRigidbody.velocity = direction * bulletSpeed;
				onCooldown = true;
			}
			else
			{
				Debug.LogError("Bullet prefab does not have a Rigidbody component!");
			}

			
		}

	}

	void CheckPlayerShoot()
	{
		RaycastHit _hit;
		Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(_ray, out _hit))
		{
			// update barrel rotation
			Vector3 targetPoint = new Vector3(	_hit.point.x, 
												barrelController.transform.position.y, 
												_hit.point.z);
			barrelController.RotateBarrelTo(targetPoint);

			// shoot when left click is pressed
			if (!onCooldown && Input.GetMouseButtonDown(0))
			{
				Vector3 direction = (targetPoint - transform.position).normalized;
				GameObject bullet = Instantiate(bulletPref, transform.position, transform.rotation);
				Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

				if (bulletRigidbody != null)
				{
					bulletRigidbody.velocity = direction * bulletSpeed;
					bulletList.Add(bullet);

					onCooldown = true;
				}
				else
				{
					Debug.LogError("Bullet prefab does not have a Rigidbody component!");
					Destroy(bullet);
				}

			}
		}
	}
}
