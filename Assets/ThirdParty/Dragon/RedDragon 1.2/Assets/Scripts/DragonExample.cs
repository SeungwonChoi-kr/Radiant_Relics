using UnityEngine;
using System.Collections;

public class DragonExample : MonoBehaviour
{
    private Animator anim;
    int IdleSimple;
    int IdleAgressive;
    int IdleRestless;
    int Walk;
    int BattleStance;
    int Bite;
    int Drakaris;
    int FlyingFWD;
    int FlyingAttack;
    int Hover;
    int Lands;
    int TakeOff;
    int Die;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        IdleSimple = Animator.StringToHash("IdleSimple");
        IdleAgressive = Animator.StringToHash("IdleAgressive");
        IdleRestless = Animator.StringToHash("IdleRestless");
        Walk = Animator.StringToHash("Walk");
        BattleStance = Animator.StringToHash("BattleStance");
        Bite = Animator.StringToHash("Bite");
        Drakaris = Animator.StringToHash("Drakaris");
        FlyingFWD = Animator.StringToHash("FlyingFWD");
        FlyingAttack = Animator.StringToHash("FlyingAttack");
        Hover = Animator.StringToHash("Hover");
        Lands = Animator.StringToHash("Lands");
        TakeOff = Animator.StringToHash("TakeOff");
        Die = Animator.StringToHash("Die");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("IdleSimple"))
            {
                anim.SetBool(IdleSimple, false);
                anim.SetBool(IdleAgressive, false);
                anim.SetBool(IdleRestless, false);
                anim.SetBool(Walk, true);
                anim.SetBool(BattleStance, false);
                anim.SetBool(Bite, false);
                anim.SetBool(Drakaris, false);
                anim.SetBool(FlyingFWD, false);
                anim.SetBool(FlyingAttack, false);
                anim.SetBool(Hover, false);
                anim.SetBool(Lands, false);
                anim.SetBool(TakeOff, false);
                anim.SetBool(Die, false);

            }
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {            //W to walk
                anim.SetBool(IdleSimple, true);
                anim.SetBool(IdleAgressive, false);
                anim.SetBool(IdleRestless, false);
                anim.SetBool(Walk, false);
                anim.SetBool(BattleStance, false);
                anim.SetBool(Bite, false);
                anim.SetBool(Drakaris, false);
                anim.SetBool(FlyingFWD, false);
                anim.SetBool(FlyingAttack, false);
                anim.SetBool(Hover, false);
                anim.SetBool(Lands, false);
                anim.SetBool(TakeOff, false);
                anim.SetBool(Die, false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("IdleSimple"))
            {
                anim.SetBool(IdleSimple, false);
                anim.SetBool(IdleAgressive, true);
                anim.SetBool(IdleRestless, false);
                anim.SetBool(Walk, false);
                anim.SetBool(BattleStance, false);
                anim.SetBool(Bite, false);
                anim.SetBool(Drakaris, false);
                anim.SetBool(FlyingFWD, false);
                anim.SetBool(FlyingAttack, false);
                anim.SetBool(Hover, false);
                anim.SetBool(Lands, false);
                anim.SetBool(TakeOff, false);
                anim.SetBool(Die, false);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("IdleAgressive"))
            {          //Q to Idle agressive
                anim.SetBool(IdleSimple, true);
                anim.SetBool(IdleAgressive, false);
                anim.SetBool(IdleRestless, false);
                anim.SetBool(Walk, false);
                anim.SetBool(BattleStance, false);
                anim.SetBool(Bite, false);
                anim.SetBool(Drakaris, false);
                anim.SetBool(FlyingFWD, false);
                anim.SetBool(FlyingAttack, false);
                anim.SetBool(Hover, false);
                anim.SetBool(Lands, false);
                anim.SetBool(TakeOff, false);
                anim.SetBool(Die, false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("IdleSimple"))
            {
                anim.SetBool(IdleSimple, false);
                anim.SetBool(IdleAgressive, false);
                anim.SetBool(IdleRestless, true);
                anim.SetBool(Walk, false);
                anim.SetBool(BattleStance, false);
                anim.SetBool(Bite, false);
                anim.SetBool(Drakaris, false);
                anim.SetBool(FlyingFWD, false);
                anim.SetBool(FlyingAttack, false);
                anim.SetBool(Hover, false);
                anim.SetBool(Lands, false);
                anim.SetBool(TakeOff, false);
                anim.SetBool(Die, false);
            }
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("IdleRestless"))
            {            //E to Idle restless
                anim.SetBool(IdleSimple, true);
                anim.SetBool(IdleAgressive, false);
                anim.SetBool(IdleRestless, false);
                anim.SetBool(Walk, false);
                anim.SetBool(BattleStance, false);
                anim.SetBool(Bite, false);
                anim.SetBool(Drakaris, false);
                anim.SetBool(FlyingFWD, false);
                anim.SetBool(FlyingAttack, false);
                anim.SetBool(Hover, false);
                anim.SetBool(Lands, false);
                anim.SetBool(TakeOff, false);
                anim.SetBool(Die, false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("IdleSimple"))
            {
                anim.SetBool(IdleSimple, false);
                anim.SetBool(IdleAgressive, false);
                anim.SetBool(IdleRestless, false);
                anim.SetBool(Walk, false);
                anim.SetBool(BattleStance, true);
                anim.SetBool(Bite, false);
                anim.SetBool(Drakaris, false);
                anim.SetBool(FlyingFWD, false);
                anim.SetBool(FlyingAttack, false);
                anim.SetBool(Hover, false);
                anim.SetBool(Lands, false);
                anim.SetBool(TakeOff, false);
                anim.SetBool(Die, false);
            }
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("BattleStance"))
            {                //R to battle stance
                anim.SetBool(IdleSimple, true);
                anim.SetBool(IdleAgressive, false);
                anim.SetBool(IdleRestless, false);
                anim.SetBool(Walk, false);
                anim.SetBool(BattleStance, false);
                anim.SetBool(Bite, false);
                anim.SetBool(Drakaris, false);
                anim.SetBool(FlyingFWD, false);
                anim.SetBool(FlyingAttack, false);
                anim.SetBool(Hover, false);
                anim.SetBool(Lands, false);
                anim.SetBool(TakeOff, false);
                anim.SetBool(Die, false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("IdleSimple"))
            {
                anim.SetBool(IdleSimple, false);
                anim.SetBool(IdleAgressive, false);
                anim.SetBool(IdleRestless, false);
                anim.SetBool(Walk, false);
                anim.SetBool(BattleStance, false);
                anim.SetBool(Bite, true);
                anim.SetBool(Drakaris, false);
                anim.SetBool(FlyingFWD, false);
                anim.SetBool(FlyingAttack, false);
                anim.SetBool(Hover, false);
                anim.SetBool(Lands, false);
                anim.SetBool(TakeOff, false);
                anim.SetBool(Die, false);
            }
        }
        else if (Input.GetKeyUp(KeyCode.T))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Bite"))
            {              //T to bite
                anim.SetBool(IdleSimple, true);
                anim.SetBool(IdleAgressive, false);
                anim.SetBool(IdleRestless, false);
                anim.SetBool(Walk, false);
                anim.SetBool(BattleStance, false);
                anim.SetBool(Bite, false);
                anim.SetBool(Drakaris, false);
                anim.SetBool(FlyingFWD, false);
                anim.SetBool(FlyingAttack, false);
                anim.SetBool(Hover, false);
                anim.SetBool(Lands, false);
                anim.SetBool(TakeOff, false);
                anim.SetBool(Die, false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("IdleSimple"))
            {
                anim.SetBool(IdleSimple, false);
                anim.SetBool(IdleAgressive, false);
                anim.SetBool(IdleRestless, false);
                anim.SetBool(Walk, false);
                anim.SetBool(BattleStance, false);
                anim.SetBool(Bite, false);
                anim.SetBool(Drakaris, true);
                anim.SetBool(FlyingFWD, false);
                anim.SetBool(FlyingAttack, false);
                anim.SetBool(Hover, false);
                anim.SetBool(Lands, false);
                anim.SetBool(TakeOff, false);
                anim.SetBool(Die, false);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Y))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Drakaris"))
            {          //Y to drakaris
                anim.SetBool(IdleSimple, true);
                anim.SetBool(IdleAgressive, false);
                anim.SetBool(IdleRestless, false);
                anim.SetBool(Walk, false);
                anim.SetBool(BattleStance, false);
                anim.SetBool(Bite, false);
                anim.SetBool(Drakaris, false);
                anim.SetBool(FlyingFWD, false);
                anim.SetBool(FlyingAttack, false);
                anim.SetBool(Hover, false);
                anim.SetBool(Lands, false);
                anim.SetBool(TakeOff, false);
                anim.SetBool(Die, false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("IdleSimple"))
            {
                anim.SetBool(IdleSimple, false);
                anim.SetBool(IdleAgressive, false);
                anim.SetBool(IdleRestless, false);
                anim.SetBool(Walk, false);
                anim.SetBool(BattleStance, false);
                anim.SetBool(Bite, false);
                anim.SetBool(Drakaris, false);
                anim.SetBool(FlyingFWD, true);
                anim.SetBool(FlyingAttack, false);
                anim.SetBool(Hover, false);
                anim.SetBool(Lands, false);
                anim.SetBool(TakeOff, false);
                anim.SetBool(Die, false);
            }
        }
        else if (Input.GetKeyUp(KeyCode.U))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("FlyingFWD"))
            {                        //U to Fly forward
                anim.SetBool(IdleSimple, true);
                anim.SetBool(IdleAgressive, false);
                anim.SetBool(IdleRestless, false);
                anim.SetBool(Walk, false);
                anim.SetBool(BattleStance, false);
                anim.SetBool(Bite, false);
                anim.SetBool(Drakaris, false);
                anim.SetBool(FlyingFWD, false);
                anim.SetBool(FlyingAttack, false);
                anim.SetBool(Hover, false);
                anim.SetBool(Lands, false);
                anim.SetBool(TakeOff, false);
                anim.SetBool(Die, false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("IdleSimple"))
            {
                anim.SetBool(IdleSimple, false);
                anim.SetBool(IdleAgressive, false);
                anim.SetBool(IdleRestless, false);
                anim.SetBool(Walk, false);
                anim.SetBool(BattleStance, false);
                anim.SetBool(Bite, false);
                anim.SetBool(Drakaris, false);
                anim.SetBool(FlyingFWD, false);
                anim.SetBool(FlyingAttack, true);
                anim.SetBool(Hover, false);
                anim.SetBool(Lands, false);
                anim.SetBool(TakeOff, false);
                anim.SetBool(Die, false);
            }
        }
        else if (Input.GetKeyUp(KeyCode.I))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("FlyingAttack"))
            {                        //I to flying attack
                anim.SetBool(IdleSimple, true);
                anim.SetBool(IdleAgressive, false);
                anim.SetBool(IdleRestless, false);
                anim.SetBool(Walk, false);
                anim.SetBool(BattleStance, false);
                anim.SetBool(Bite, false);
                anim.SetBool(Drakaris, false);
                anim.SetBool(FlyingFWD, false);
                anim.SetBool(FlyingAttack, false);
                anim.SetBool(Hover, false);
                anim.SetBool(Lands, false);
                anim.SetBool(TakeOff, false);
                anim.SetBool(Die, false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("IdleSimple"))
            {
                anim.SetBool(IdleSimple, false);
                anim.SetBool(IdleAgressive, false);
                anim.SetBool(IdleRestless, false);
                anim.SetBool(Walk, false);
                anim.SetBool(BattleStance, false);
                anim.SetBool(Bite, false);
                anim.SetBool(Drakaris, false);
                anim.SetBool(FlyingFWD, false);
                anim.SetBool(FlyingAttack, false);
                anim.SetBool(Hover, true);
                anim.SetBool(Lands, false);
                anim.SetBool(TakeOff, false);
                anim.SetBool(Die, false);
            }
        }
        else if (Input.GetKeyUp(KeyCode.O))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Hover"))
            {                        //O to hover
                anim.SetBool(IdleSimple, true);
                anim.SetBool(IdleAgressive, false);
                anim.SetBool(IdleRestless, false);
                anim.SetBool(Walk, false);
                anim.SetBool(BattleStance, false);
                anim.SetBool(Bite, false);
                anim.SetBool(Drakaris, false);
                anim.SetBool(FlyingFWD, false);
                anim.SetBool(FlyingAttack, false);
                anim.SetBool(Hover, false);
                anim.SetBool(Lands, false);
                anim.SetBool(TakeOff, false);
                anim.SetBool(Die, false);
            }

        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("IdleSimple"))
            {
                anim.SetBool(IdleSimple, false);
                anim.SetBool(IdleAgressive, false);
                anim.SetBool(IdleRestless, false);
                anim.SetBool(Walk, false);
                anim.SetBool(BattleStance, false);
                anim.SetBool(Bite, false);
                anim.SetBool(Drakaris, false);
                anim.SetBool(FlyingFWD, false);
                anim.SetBool(FlyingAttack, false);
                anim.SetBool(Hover, false);
                anim.SetBool(Lands, true);
                anim.SetBool(TakeOff, false);
                anim.SetBool(Die, false);
            }

        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Lands"))
            {                        //A to land
                anim.SetBool(IdleSimple, true);
                anim.SetBool(IdleAgressive, false);
                anim.SetBool(IdleRestless, false);
                anim.SetBool(Walk, false);
                anim.SetBool(BattleStance, false);
                anim.SetBool(Bite, false);
                anim.SetBool(Drakaris, false);
                anim.SetBool(FlyingFWD, false);
                anim.SetBool(FlyingAttack, false);
                anim.SetBool(Hover, false);
                anim.SetBool(Lands, false);
                anim.SetBool(TakeOff, false);
                anim.SetBool(Die, false);
            }

        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("IdleSimple"))
            {
                anim.SetBool(IdleSimple, false);
                anim.SetBool(IdleAgressive, false);
                anim.SetBool(IdleRestless, false);
                anim.SetBool(Walk, false);
                anim.SetBool(BattleStance, false);
                anim.SetBool(Bite, false);
                anim.SetBool(Drakaris, false);
                anim.SetBool(FlyingFWD, false);
                anim.SetBool(FlyingAttack, false);
                anim.SetBool(Hover, false);
                anim.SetBool(Lands, false);
                anim.SetBool(TakeOff, true);
                anim.SetBool(Die, false);
            }
        }
        else if (Input.GetKeyUp(KeyCode.P))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("TakeOff"))
            {                        //P to take off
                anim.SetBool(IdleSimple, true);
                anim.SetBool(IdleAgressive, false);
                anim.SetBool(IdleRestless, false);
                anim.SetBool(Walk, false);
                anim.SetBool(BattleStance, false);
                anim.SetBool(Bite, false);
                anim.SetBool(Drakaris, false);
                anim.SetBool(FlyingFWD, false);
                anim.SetBool(FlyingAttack, false);
                anim.SetBool(Hover, false);
                anim.SetBool(Lands, false);
                anim.SetBool(TakeOff, false);
                anim.SetBool(Die, false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("IdleSimple"))
            {
                anim.SetBool(IdleSimple, false);
                anim.SetBool(IdleAgressive, false);
                anim.SetBool(IdleRestless, false);
                anim.SetBool(Walk, false);
                anim.SetBool(BattleStance, false);
                anim.SetBool(Bite, false);
                anim.SetBool(Drakaris, false);
                anim.SetBool(FlyingFWD, false);
                anim.SetBool(FlyingAttack, false);
                anim.SetBool(Hover, false);
                anim.SetBool(Lands, false);
                anim.SetBool(TakeOff, false);
                anim.SetBool(Die, true);
            }
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {                                                                                    //D to die
                anim.SetBool(IdleSimple, true);
                anim.SetBool(IdleAgressive, false);
                anim.SetBool(IdleRestless, false);
                anim.SetBool(Walk, false);
                anim.SetBool(BattleStance, false);
                anim.SetBool(Bite, false);
                anim.SetBool(Drakaris, false);
                anim.SetBool(FlyingFWD, false);
                anim.SetBool(FlyingAttack, false);
                anim.SetBool(Hover, false);
                anim.SetBool(Lands, false);
                anim.SetBool(TakeOff, false);
                anim.SetBool(Die, false);
            }

        }
    }
}
