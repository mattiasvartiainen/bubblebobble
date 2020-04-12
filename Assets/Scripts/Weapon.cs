using Assets.Scripts;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;

    public GameObject bubblePrefab;

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

        var bubble = Instantiate(bubblePrefab, firePoint.position, Quaternion.identity);
        bubble.transform.localScale = new Vector3((int)firePoint.parent.localScale.x, 1, 1);
        bubble.GetComponent<Controller2D>().FacingRight = firePoint.parent.localScale.x > 0;
    }
}
