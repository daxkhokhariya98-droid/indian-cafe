using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    [Header("Hold Settings")]
    public Transform holdPoint; // Player camera ke aage ka empty object jahan item hold hoga
    public float pickupDistance = 3f;

    private GameObject heldItem = null;

    void Update()
    {
        // 'E' press karke item uthana ya drop karna
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldItem == null)
            {
                TryPickupItem();
            }
            else
            {
                DropItem();
            }
        }
    }

    void TryPickupItem()
    {
        // Direct Main Camera se forward Raycast shoot karenge
        if (Camera.main == null) return;

        Transform camTrans = Camera.main.transform;
        Ray centerRay = new Ray(camTrans.position, camTrans.forward);
        RaycastHit hit;

        if (Physics.Raycast(centerRay, out hit, pickupDistance))
        {
            // Yahan hum check kar rahe hain ki hit object ke paas Rigidbody ho ya koi valid pickable component ho
            // Isse ingredient packet aur crafted matcha cup dono uth jayenge
            if (hit.collider.GetComponent<Rigidbody>() != null && hit.collider.gameObject != gameObject)
            {
                heldItem = hit.collider.gameObject;

                // Physics disable kar do taaki item haath mein steady rahe
                Rigidbody rb = heldItem.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }

                // Item ko holdPoint ka child bana kar center mein positioning karo
                heldItem.transform.SetParent(holdPoint);
                heldItem.transform.localPosition = Vector3.zero;
                heldItem.transform.localRotation = Quaternion.identity;

                Debug.Log("Item picked up successfully!");
            }
        }
    }

    public void DropItem()
    {
        if (heldItem != null)
        {
            Rigidbody rb = heldItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }

            heldItem.transform.SetParent(null);
            heldItem = null;
            Debug.Log("Item dropped!");
        }
    }

    public GameObject GetHeldItem()
    {
        return heldItem;
    }
}