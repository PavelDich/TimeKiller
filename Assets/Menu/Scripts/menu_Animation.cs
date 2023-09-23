using UnityEngine;

public class menu_Animation : MonoBehaviour
{
    public Animator Anim;

    public void menu_ID(int menu_ID)
    {
        Anim.SetInteger("ID", menu_ID);
    }
    public void menu_Layer(int menu_Layer)
    {
        Anim.SetInteger("Layer", menu_Layer);
    }

    public void menu_bool(string BoolName)
    {
        Anim.SetBool(BoolName, !Anim.GetBool(BoolName));
    }
}
