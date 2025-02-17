using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Transform gunTransform;
    public float fireRate = 0.2f;
    public float recoilDistance = 0.2f;
    public float recoilSpeed = 0.1f;
    public BulletPool bulletPool;

    private Vector3 originalGunPosition;
    private float nextFireTime = 0f;

    private void Start()
    {
        originalGunPosition = gunTransform.localPosition;
    }

    private void Update()
    {
        RotateGunToMouse();

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void RotateGunToMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 direction = mousePosition - gunTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gunTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void Shoot()
    {
        GameObject bullet = bulletPool.GetFromPool();
        bullet.transform.position = gunTransform.position;
        bullet.transform.rotation = gunTransform.rotation;
        bullet.GetComponent<Rigidbody2D>().velocity = gunTransform.right * 10f;

        StartCoroutine(ApplyRecoil());
    }

    private IEnumerator ApplyRecoil()
    {
        gunTransform.localPosition -= gunTransform.right * recoilDistance;

        float elapsedTime = 0f;
        while (elapsedTime < recoilSpeed)
        {
            gunTransform.localPosition = Vector3.Lerp(gunTransform.localPosition, originalGunPosition, elapsedTime / recoilSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gunTransform.localPosition = originalGunPosition;
    }
}
