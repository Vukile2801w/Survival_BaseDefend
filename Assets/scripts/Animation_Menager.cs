using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Animation_Menager : MonoBehaviour
{

    [SerializeField] private Animations[] animations;

    private Animations Get_Animator_By_ID(string animatorID)
    {
        for (int i = 0; i < animations.Length; i++)
        {
            if (animations[i].AnimatorID == animatorID)
            {
                return animations[i];
            }
        }

        return null;
    }


    public void Play(string animatorID)
    {
        Debug.Log($"Playing animation: {animatorID}", this);

        Animations animations = Get_Animator_By_ID(animatorID);
        if (animations != null)
        {
            animations.animator.SetTrigger(animations.trigger_name);
        }
        else
        {
            Debug.LogError($"Animation not found: {animatorID}", this);
        }

    }

    public struct StateInfo
    {
        public bool nameCorrect;
        public float normTimeCorrect;
        public bool inTransition;

        public bool isValid;
        public bool isAllCorrect => nameCorrect && normTimeCorrect < 0.9f && !inTransition && isValid;
    }

    public StateInfo IsPlayingAnimation(string stateName, string animatorID)
    {
        StateInfo info = new StateInfo();


        Animations anim = Get_Animator_By_ID(animatorID);
        if (anim == null || anim.animator == null)
        {
            Debug.LogWarning($"Animator '{animatorID}' nije pronađen ili nije dodeljen.", this);
            info.isValid = false;

            return info;
        }

        AnimatorStateInfo stateInfo = anim.animator.GetCurrentAnimatorStateInfo(0);

        info.nameCorrect = stateInfo.IsName(stateName);
        info.normTimeCorrect = stateInfo.normalizedTime;
        info.inTransition = anim.animator.IsInTransition(0);
        info.isValid = true;

        return info;
    }

}

[Serializable]
public class Animations
{
    public Animator animator;
    public string AnimatorID;     // name id of animator (e.g. "Cut", "Run", etc.)
    public string trigger_name;
}

