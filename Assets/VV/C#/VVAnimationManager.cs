using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VVAnimationManager
{

    Animator _anim;
    List<string> listNameClip = new List<string>();

    public void SetUpAnimator(Animator anim)
    {
        _anim = anim;

        // get name of all clip
        if (_anim.runtimeAnimatorController != null)
        {
            AnimationClip[] arrclip = _anim.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in arrclip)
            {
                listNameClip.Add(clip.name);
            }
        }

    }

    public bool isExistClip(string n)
    {
        foreach (string name in listNameClip)
        {
            if (name == n)
                return true;
        }
        return false;
    }

    public bool AnimPlay(string clipName, bool resetNormalTime = false)
    {

        if (!isExistClip(clipName))
            return false;

        if (resetNormalTime)
        {
            _anim.Play(clipName, -1, 0F);
        }
        else
        {
            _anim.Play(clipName);
        }

        return true;

    }

    public bool CheckFinishClip(string clipName)
    {
        if (!isExistClip(clipName))
        {
            return true;
        }

        if (clipName == "shoot")
        {
            bool isName = _anim.GetCurrentAnimatorStateInfo(0).IsName(clipName);
            float mormalTime = _anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        if (_anim.GetCurrentAnimatorStateInfo(0).IsName(clipName) &&
                _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95F)
        {
            return true;
        }
        return false;
    }

    public bool CheckFinishCurrentClip(float finishAtPercent = 95F)
    {
        float per = finishAtPercent / 100;
        if (_anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= per)
        {
            return true;
        }
        return false;
    }

    public float GetPercentClipPlaying(string clipName)
    {
        float percent = 0;
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName(clipName))
        {
            percent = _anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
        return percent * 100F;
    }

    public bool CheckClipFinish2(string clipname)
    {
        AnimatorClipInfo[] m_CurrentClipInfo = _anim.GetCurrentAnimatorClipInfo(0);
        string curname = m_CurrentClipInfo[0].clip.name;
        if (curname == clipname)
        {
            
               AnimatorStateInfo animationState = _anim.GetCurrentAnimatorStateInfo(0);
            // A value of 1 is the end of the animation. A value of 0.5 is the middle of the animation.

            float norm = animationState.normalizedTime % 1;
            if (norm >= 1)
            {
                return true;
            }
        }
        else
        {
            return false;
        }
        return false;
    }

    public string GetNameClipPlaying()
    {
        AnimatorClipInfo[] m_CurrentClipInfo = _anim.GetCurrentAnimatorClipInfo(0);
        return m_CurrentClipInfo[0].clip.name;
    }

    public void SetFloat(string name, float value)
    {
        if (_anim != null)
        {
            _anim.SetFloat(name, value);
        }
    }

    public void TestDeBug_TimeCurrClip()
    {
        AnimatorStateInfo animationState = _anim.GetCurrentAnimatorStateInfo(0);
        AnimatorClipInfo[] myAnimatorClip = _anim.GetCurrentAnimatorClipInfo(0);
        float currentTime = myAnimatorClip[0].clip.length * animationState.normalizedTime;
        string name = myAnimatorClip[0].clip.name;
        Debug.Log(name + " reach : " + GetPercentClipPlaying(name).ToString() + "%" + ", currentTime : " + currentTime.ToString());
    }

    public float GetLengthTime_Clip(string clipname)
    {
        if (isExistClip(clipname) == false)
        {
            return 0;
        }
        float lenTime = 0;
        AnimationClip[] clips = _anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == clipname)
            {
                return clip.length;
            }
        }
        return lenTime;
    }
}
