using UnityEngine;

public class bossWeaponRenderScript : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Bounce Properties")]

    public float bounceAmplitude = 0.05f;
    public float bounceFrequency = 2f;

    private float bounceTime;

    private Vector3 originalLocalPosition;

    private SpriteRenderer spriteRenderer;

    private testAgentScript boss;

    void Start()
    {
        //to keep it centered
        originalLocalPosition = transform.localPosition;

        spriteRenderer = GetComponent<SpriteRenderer>();

        boss = GetComponentInParent<testAgentScript>();
        
        if (boss == null)
        {
            Debug.LogError("boss not found on parent object");
        }
        
        
    }

     void Update()
    {
        HandleBounce();
    }

    void HandleBounce()
    {
        if (boss != null)
        {
            bounceTime += Time.deltaTime * bounceFrequency;
            float bounceOffset = Mathf.Cos(bounceTime) * bounceAmplitude;
            transform.localPosition = originalLocalPosition + new Vector3(bounceOffset, 0, 0);
        }
        else
        {
            bounceTime = 0;
            transform.localPosition = originalLocalPosition;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
}
