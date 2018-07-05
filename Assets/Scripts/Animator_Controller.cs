using UnityEngine;
using System.Collections;

public class Animator_Controller : MonoBehaviour
{
    // Public variables
    public static Animator_Controller instance;

    // Private variables
    Transform myTrans;
    Animator myAnim;
    Vector3 artScaleCache;

    // Use this for initialization
    void Start()
    {
        myTrans = this.transform;
        myAnim = this.gameObject.GetComponent<Animator>();
        instance = this;

        artScaleCache = myTrans.localScale;
    }

    void FilpArt(float currentSpeed)
    {
        if((currentSpeed < 0 && artScaleCache.x > 0) ||     // Going left and facing right
           (currentSpeed > 0 && artScaleCache.x < 0))       // Going right and facing left
        {
            // Flip the art
            artScaleCache.x *= -1;
            myTrans.localScale = artScaleCache;
        }
    }

    // Update is called once per frame
    public void UpdateSpeed(float currentSpeed)
    {
        myAnim.SetFloat("speed", currentSpeed);
        FilpArt(currentSpeed);
    }

    public void UpdateIsGrounded(bool isGrounded)
    {
        myAnim.SetBool("isGrounded", isGrounded);
    }
}
