using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;

    public GameObject bubblePrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if ((int)firePoint.localScale.x != (int)firePoint.parent.localScale.x)
        {
            firePoint.localScale = firePoint.parent.localScale;
            firePoint.Rotate(0, 180f, 0);
        }

        var bubble = Instantiate(bubblePrefab, firePoint.position, firePoint.rotation);
    }
}
