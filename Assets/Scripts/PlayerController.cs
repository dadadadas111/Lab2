using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Transform gunTransform;
    public Transform muzzlePoint;
    public ParticleSystem muzzleFlash;
    public float fireRate = 0.2f;
    public float recoilDistance = 0.2f;
    public float recoilSpeed = 0.1f;
    public float moveSpeed = 5f;
    public float moveBoundary = 8f;
    public BulletPool bulletPool;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer gunRenderer;
    public BuffLoader buffLoader;

    private Vector3 originalGunPosition;
    private HealthBarFade healthBarFade;
    private float nextFireTime = 0f;
    private float defaultFireRate;
    private float fireBuffExpiredAt;

    private void Start()
    {
        originalGunPosition = gunTransform.localPosition;
        defaultFireRate = fireRate;
        healthBarFade = FindObjectOfType<HealthBarFade>();
        fireBuffExpiredAt = Time.time;
    }

    private void Update()
    {
        HandleMovement();
        RotateGunToMouse();

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        if(Input.GetMouseButtonUp(0))
        {
            if (muzzleFlash != null)
            {
                muzzleFlash.Stop();
            }
        }
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right Arrow
        Vector3 move = new Vector3(moveInput, 0, 0) * moveSpeed * Time.deltaTime;
        transform.position += move;

        // Clamp within boundaries
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -moveBoundary, moveBoundary),
            transform.position.y,
            transform.position.z
        );
    }

    private void RotateGunToMouse()
    {
        if (Time.timeScale == 0) return;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 direction = mousePosition - gunTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gunTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void Shoot()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
        GameObject bullet = bulletPool.GetFromPool();
        bullet.transform.position = muzzlePoint.position;
        bullet.transform.rotation = muzzlePoint.rotation;
        bullet.GetComponent<Rigidbody2D>().velocity = muzzlePoint.right * 10f;

        StartCoroutine(ApplyRecoil());
    }

    public void ApplyFireRateBuff(float multiplier, float duration)
    {
        fireRate *= 1f / multiplier;
        // min fireRate is 0.05f
        if (fireRate < 0.1f)
        {
            fireRate = 0.1f;
        }
        // adjust recoil speed based on fire rate
        recoilSpeed = fireRate * 0.5f;
        Debug.Log("Fire Rate Buff Applied! current fire rate: " + fireRate);
        fireBuffExpiredAt = Time.time + (duration - 0.1f);
        buffLoader.StartBuff(duration);
        Invoke(nameof(ResetFireRate), duration);
    }

    private void ResetFireRate()
    {
        if (Time.time >= fireBuffExpiredAt)
        {
            fireRate = defaultFireRate;
            recoilSpeed = 0.1f;
            Debug.Log("Fire Rate Reset");
        }
    }

    public void RecoverHealth(int amount)
    {
        if (healthBarFade != null)
        {
            healthBarFade.Heal(amount);
            StartCoroutine(FlashGreen());
        }
    }

    // flash when buffed health is applied
    private IEnumerator FlashGreen()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.green;
            yield return new WaitForSeconds(1f); // Adjust flash duration
            spriteRenderer.color = Color.white;
        }
        else
        {
            Debug.LogWarning("SpriteRenderer is null!");
            yield break;
        }
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
