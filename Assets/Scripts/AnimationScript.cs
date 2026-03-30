using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class AnimationScript : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Animator cameraAnimator;
    [SerializeField] bool isOnLevel;
    [SerializeField] ReworkedCameraSystem rcs;
    private float timer1 = 0f;

    void Start()
    {
        if (isOnLevel)
        {
            cameraBorderOff();
            cameraAnimator.Play("CameraIn");
        }
    }

    void Update()
    {
        if (timer1 < 2f)
        {
            timer1 += Time.deltaTime;
            cameraBorderOff();
        }
    }

    void cameraBorderOff()
    {
        if (timer1 >= 2f)
        {
            rcs.borderSwitch = true;
        }
    }
    public void MainMenuPanelIn()
    {
        animator.Play("MainMenuPanelIn");
    }

    public void MainMenuPanelOut()
    {
        animator.Play("MainMenuPanelOut");
    }

    public void ScaleUp(GameObject buttonObj)
    {
        buttonObj.transform.DOScale(1.2f, 0.2f);
    }

    public void ScaleDown(GameObject buttonObj)
    {
        buttonObj.transform.DOScale(1f, 0.2f);
    }
}
