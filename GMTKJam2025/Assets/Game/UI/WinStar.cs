using UnityEngine;
using UnityEngine.UI;

public class WinStar : MonoBehaviour
{
    [SerializeField]
    private Image winStar;
    [SerializeField]
    private bool visible;
    private Animator animator;
    private readonly static int ANIMATOR_WIN_STATE = Animator.StringToHash("Win");

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool(ANIMATOR_WIN_STATE, visible);
    }

    public void SetVisible(bool target)
    {
        if (visible != target)
        {
            visible = target;
            animator.SetBool(ANIMATOR_WIN_STATE, visible);
        }
    }
}
